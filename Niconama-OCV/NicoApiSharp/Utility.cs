using System;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Web;

namespace Hal.NicoApiSharp
{

	/// <summary>
	/// 汎用関数群
	/// </summary>
	public static class Utility
	{
		#region シリアライズ

		/// <summary>
		/// Typeで指定されたクラスをXMLシリアライズする
		/// </summary>
		/// <param name="filePath">保存場所</param>
		/// <param name="graph">シリアライズ対象</param>
		/// <param name="type">シリアライズするクラス情報</param>
		/// <returns>成功・失敗</returns>
		static public bool XmlSerialize(string filePath, object graph, Type type)
		{

			try {
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);

				using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create)) {
					serializer.Serialize(fs, graph);
				}

				return true;

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return false;
		}

		/// <summary>
		///  Typeで指定されたクラスをXMLシリアライズする
		/// </summary>
		/// <param name="graph"></param>
		/// <param name="type"></param>
		/// <returns>シリアル化された文字列</returns>
		static public string XmlSerialize(object graph, Type type)
		{

			try {
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);

				using (System.IO.StringWriter fs = new StringWriter()) {
					serializer.Serialize(fs, graph);
					return fs.ToString();
				}
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}

		/// <summary>
		/// Xmlシリアライズされたオブジェクトを復元する
		/// 失敗した場合はnullを返す
		/// </summary>
		/// <param name="filePath">対象のファイル</param>
		/// <param name="type">対象のクラス情報</param>
		/// <returns>復元されたオブジェクト・失敗した場合はnullを返す</returns>
		static public object XmlDeserialize(string filePath, Type type)
		{

			if (File.Exists(filePath)) {
				try {

					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);

					using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open)) {
						return serializer.Deserialize(fs);
					}


				} catch (Exception ex) {
					Logger.Default.LogException(ex);
				}

			}

			return null;
		}

		/// <summary>
		/// graphで指定されたオブジェクトをシリアライズする
		/// </summary>
		/// <param name="path">保存先</param>
		/// <param name="graph">シリアライズ対象</param>
		/// <returns>成否</returns>
		public static bool Serialize(string path, object graph)
		{
			try {

				IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
					formatter.Serialize(stream, graph);
					stream.Close();
				}

				return true;
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return false;
		}

		/// <summary>
		/// シリアライズされたオブジェクトを復元する
		/// 失敗した場合はnullを返す
		/// </summary>
		/// <param name="filePath">対象のファイル</param>
		/// <returns>復元されたオブジェクト・失敗した場合はnullを返す</returns>
		public static object Deserialize(string filePath)
		{

			if (File.Exists(filePath)) {
				try {
					IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
						return formatter.Deserialize(stream);
					}

				} catch (Exception ex) {
					Logger.Default.LogException(ex);
				}
			}

			return null;
		}

		#endregion

		#region 通信

		

		/// <summary>
		/// url上のページを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cookies"></param>
		/// <param name="defaultTimeout"></param>
		/// <returns></returns>
		static public string GetResponseText(string url, CookieContainer cookies, int defaultTimeout)
		{
			HttpWebResponse webRes = null;
			StreamReader sr = null;

			try {
				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);

				webReq.Timeout = defaultTimeout;
				webReq.CookieContainer = cookies;

				try {
					webRes = (HttpWebResponse)webReq.GetResponse();
				} catch (WebException ex) {
					Logger.Default.LogException(ex);
					webRes = ex.Response as HttpWebResponse;

				}

				if (webRes == null) {
					return null;
				}

				sr = new StreamReader(webRes.GetResponseStream(), System.Text.Encoding.UTF8);
				return sr.ReadToEnd();

			} finally {
				if (webRes != null)
					webRes.Close();
				if (sr != null)
					sr.Close();
			}
		}

		/// <summary>
		/// URLに対してPOSTします
		/// </summary>
		/// <param name="url"></param>
		/// <param name="postData"></param>
		/// <param name="cookies"></param>
		/// <param name="defaultTimeout"></param>
		/// <param name="referer"></param>
		/// <returns></returns>
		static public string PostData(string url, string postData, CookieContainer cookies, int defaultTimeout, string referer)
		{

			HttpWebResponse webRes = null;
			StreamReader sr = null;

			try {

				byte[] postDataBytes = System.Text.Encoding.UTF8.GetBytes(postData);

				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
				webReq.Method = "POST";
				webReq.Referer = referer;
				webReq.ContentType = "application/x-www-form-urlencoded";
				webReq.UserAgent = ApiSettings.Default.PostOwnerCommentUserAgent;

				webReq.ContentLength = postDataBytes.Length;
				webReq.Timeout = defaultTimeout;
				webReq.CookieContainer = cookies;

				Stream reqStream = webReq.GetRequestStream();
				reqStream.Write(postDataBytes, 0, postDataBytes.Length);
				reqStream.Close();

				webRes = (HttpWebResponse)webReq.GetResponse();
				sr = new StreamReader(webRes.GetResponseStream());
				return sr.ReadToEnd();

			} catch (WebException ex) {
				Logger.Default.LogException(ex);
			} finally {

				if (sr != null)
					sr.Close();
				if (webRes != null)
					webRes.Close();

			}

			return null;
		}

		/// <summary>
		/// Urlで指定された画像を取得する
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static System.Drawing.Bitmap LoadImage(string url)
		{
			System.Drawing.Bitmap bitmap = null;
			HttpWebRequest req = null;
			Stream stream = null;

			if (url == null) {
				return null;
			}

			try {
				req = (HttpWebRequest)WebRequest.Create(url);
				req.Timeout = 1000;
				WebResponse res = req.GetResponse();

				stream = res.GetResponseStream();

				if (stream != null) {

					bitmap = new System.Drawing.Bitmap(stream);
					stream.Close();

				} else {
					return null;
				}

			} catch {
				return null;
			}

			return bitmap;

		}


		#endregion

		#region フォーマッティング

		/// <summary>
		/// XMLやHTML上でエスケープされた文字を復元する
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static string Unsanitizing(string src)
		{
			src = src.Replace("&amp;", "&");
			src = src.Replace("&lt;", "<");
			src = src.Replace("&gt;", ">");
			src = src.Replace("&quot;", "\"");
			src = src.Replace("&#39;", "'");

			return src;
		}

		/// <summary>
		/// XMLやHTML用に文字をエスケープする
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static string Sanitizing(string src)
		{
			src = src.Replace("&", "&amp;");
			src = src.Replace("<", "&lt;");
			src = src.Replace(">", "&gt;");
			src = src.Replace("\"", "&quot;");
			src = src.Replace("'", "&#39;");

			return src;

		}

		/// <summary>
		/// Unix時間をDateTimeに変換する
		/// </summary>
		/// <param name="UnixTime"></param>
		/// <returns></returns>
		public static DateTime UnixTimeToDateTime(int UnixTime)
		{
			return new DateTime(1970, 1, 1, 9, 0, 0).AddSeconds(UnixTime);
		}

		/// <summary>
		/// DateTimeをUnix時間に変換する
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int DateTimeToUnixTime(DateTime time)
		{
			TimeSpan t = time.Subtract(new DateTime(1970, 1, 1, 9, 0, 0));
			return (int)t.TotalSeconds;
		}

		/// <summary>
		/// %APPDATA%などを実際のパスに変換する
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ReplacePathSymbols(string path) {
			path = path.Replace("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			path = path.Replace("%LOCALAPPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			path = path.Replace("%COOKIES%", Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
			return path;
		}

		/// <summary>
		/// HSVカラー
		/// </summary>
		/// <param name="h"></param>
		/// <param name="s"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public static System.Drawing.Color HSV(double h, double s, double v)
		{

			h = (h + 360) % 360;

			int hi = (int)(h / 60) % 6;
			double f = h / 60 - hi;
			int vv = (int)(v * 255);
			int p = (int)(v * (1 - s) * 255);
			int q = (int)(v * (1 - f * s) * 255);
			int t = (int)(v * (1 - (1 - f) * s) * 255);
			int[] arr = { vv, q, p, p, t, vv };

			return System.Drawing.Color.FromArgb(arr[hi], arr[(hi + 4) % 6], arr[(hi + 2) % 6]);

		}

		/// <summary>
		/// UserIdからプロフィール画像URLを取得する
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public static string MakeUserIconUrl(string userID)
		{
			const string urlBase = "http://usericon.nimg.jp/usericon/{0}/{1}.jpg?{2}";
			int dir = 0;
			if (4 < userID.Length) {
				int.TryParse(userID.Substring(0, userID.Length - 4), out dir);
			}
			return string.Format(urlBase, dir, userID, DateTimeToUnixTime(DateTime.Now));
		}

		#endregion

		
		#region XML操作

		/// <summary>
		/// XMLノードから文字列を取得する
		/// </summary>
		/// <param name="node"></param>
		/// <param name="xpath"></param>
		/// <returns></returns>
		public static string SelectString(System.Xml.XmlNode node, string xpath)
		{
			System.Diagnostics.Debug.Assert(node != null, "Utility SelectString node is null!");
			System.Xml.XmlNode tnode = node.SelectSingleNode(xpath);
			if (tnode != null) {
				if (tnode.NodeType == System.Xml.XmlNodeType.Element) {
					return tnode.InnerText;
				} else if (tnode.NodeType == System.Xml.XmlNodeType.Attribute) {
					return tnode.Value;
				}
			}

			return null;
		}

		/// <summary>
		/// XMLノードから整数値を取得する
		/// </summary>
		/// <param name="node"></param>
		/// <param name="xpath"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int SelectInt(System.Xml.XmlNode node, string xpath, int defaultValue)
		{
			string data = SelectString(node, xpath);

			if (data != null) {
				int result;
				if (int.TryParse(data, out result)) {
					return result;
				}
			}

			return defaultValue;
		}

		/// <summary>
		/// XMLノードから日付を取得する
		/// </summary>
		/// <param name="node"></param>
		/// <param name="xpath"></param>
		/// <returns></returns>
		public static DateTime SelectDateTime(System.Xml.XmlNode node, string xpath)
		{
			int unixTime = SelectInt(node, xpath, 0);
			return Utility.UnixTimeToDateTime(unixTime);
		}
		#endregion

		#region OSオペレーション

		/// <summary>
		/// テキストをコピーする
		/// </summary>
		/// <param name="text"></param>
		public static void SetTxetToClipboard(string text)
		{
			//virtual pc 導入環境だとエラーが発生する可能性がある

			if (string.IsNullOrEmpty(text)) return;

			try {
				System.Windows.Forms.Clipboard.SetText(text);
			} catch {
			}


		}

		/// <summary>
		/// 指定のURLを標準ブラウザで開く
		/// </summary>
		/// <param name="url"></param>
		public static void OpenURL(string url)
		{

			if (string.IsNullOrEmpty(url)) return;

			try {
				System.Diagnostics.Process.Start(url);
			} catch (System.ComponentModel.Win32Exception) {
				try {
					OpenUrlEx(url);
				} catch {
				}

			}
		}


		private static void OpenUrlEx(string targetUrl)
		{
			System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
			// URLに関連づけられたアプリケーションを探す
			Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
			String val = rkey.GetValue("").ToString();
			// レジストリ値には、起動パラメータも含まれるので、
			// 実行ファイル名と起動パラメータを分離する
			if (val.StartsWith("\"")) {
				int n = val.IndexOf("\"", 1);
				info.FileName = val.Substring(1, n - 1);
				info.Arguments = val.Substring(n + 1);
			} else {
				string[] a = val.Split(new char[] { ' ' });
				info.FileName = a[0];
				info.Arguments = val.Substring(a[0].Length + 1);
			}
			// 作業ディレクトリも指定しないとダメなようだ・・・
			info.WorkingDirectory = System.IO.Path.GetDirectoryName(info.FileName);
			// 引数の最後にURLを加える
			info.Arguments += targetUrl;
			System.Diagnostics.Process.Start(info);

		}


		#endregion

		#region 暗号化

		/// <summary>
		/// 文字列を暗号化する
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string EncryptString(string str)
		{
			return EncryptString(str, MakeEncryptKey());
		}

		/// <summary>
		/// 文字列を暗号化する
		/// http://dobon.net/vb/dotnet/string/encryptstring.html参照
		/// </summary>
		/// <param name="str">暗号化する文字列</param>
		/// <param name="key">パスワード</param>
		/// <returns>暗号化された文字列</returns>
		public static string EncryptString(string str, string key)
		{
			//文字列をバイト型配列にする
			byte[] bytesIn = System.Text.Encoding.UTF8.GetBytes(str);

			//DESCryptoServiceProviderオブジェクトの作成
			System.Security.Cryptography.DESCryptoServiceProvider des =
					new System.Security.Cryptography.DESCryptoServiceProvider();


			//共有キーと初期化ベクタを決定
			//パスワードをバイト配列にする
			byte[] bytesKey = System.Text.Encoding.UTF8.GetBytes(key);
			//共有キーと初期化ベクタを設定
			des.Key = ResizeBytesArray(bytesKey, des.Key.Length);
			des.IV = ResizeBytesArray(bytesKey, des.IV.Length);

			//暗号化されたデータを書き出すためのMemoryStream
			System.IO.MemoryStream msOut = new System.IO.MemoryStream();
			//DES暗号化オブジェクトの作成
			System.Security.Cryptography.ICryptoTransform desdecrypt =
					des.CreateEncryptor();
			//書き込むためのCryptoStreamの作成
			System.Security.Cryptography.CryptoStream cryptStream =
					new System.Security.Cryptography.CryptoStream(msOut,
					desdecrypt,
					System.Security.Cryptography.CryptoStreamMode.Write);
			//書き込む
			cryptStream.Write(bytesIn, 0, bytesIn.Length);
			cryptStream.FlushFinalBlock();
			//暗号化されたデータを取得
			byte[] bytesOut = msOut.ToArray();

			//閉じる
			cryptStream.Close();
			msOut.Close();

			//Base64で文字列に変更して結果を返す
			return System.Convert.ToBase64String(bytesOut);
		}

		/// <summary>
		/// 文字列を暗号化する
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string DecryptString(string str)
		{
			return DecryptString(str, MakeEncryptKey());
		}

		/// <summary>
		/// 暗号化された文字列を復号化する
		/// </summary>
		/// <param name="str">暗号化された文字列</param>
		/// <param name="key">パスワード</param>
		/// <returns>復号化された文字列</returns>
		public static string DecryptString(string str, string key)
		{
			try {
				//DESCryptoServiceProviderオブジェクトの作成
				System.Security.Cryptography.DESCryptoServiceProvider des =
						new System.Security.Cryptography.DESCryptoServiceProvider();

				//共有キーと初期化ベクタを決定
				//パスワードをバイト配列にする
				byte[] bytesKey = System.Text.Encoding.UTF8.GetBytes(key);
				//共有キーと初期化ベクタを設定
				des.Key = ResizeBytesArray(bytesKey, des.Key.Length);
				des.IV = ResizeBytesArray(bytesKey, des.IV.Length);

				//Base64で文字列をバイト配列に戻す
				byte[] bytesIn = System.Convert.FromBase64String(str);
				//暗号化されたデータを読み込むためのMemoryStream
				System.IO.MemoryStream msIn =
						new System.IO.MemoryStream(bytesIn);
				//DES復号化オブジェクトの作成
				System.Security.Cryptography.ICryptoTransform desdecrypt =
						des.CreateDecryptor();
				//読み込むためのCryptoStreamの作成
				System.Security.Cryptography.CryptoStream cryptStream =
						new System.Security.Cryptography.CryptoStream(msIn,
						desdecrypt,
						System.Security.Cryptography.CryptoStreamMode.Read);

				//復号化されたデータを取得するためのStreamReader
				System.IO.StreamReader srOut =
						new System.IO.StreamReader(cryptStream,
						System.Text.Encoding.UTF8);
				//復号化されたデータを取得する
				string result = srOut.ReadToEnd();

				//閉じる
				srOut.Close();
				cryptStream.Close();
				msIn.Close();

				return result;
			} catch {

			}

			return "";

		}

		/// <summary>
		/// 共有キー用に、バイト配列のサイズを変更する
		/// </summary>
		/// <param name="bytes">サイズを変更するバイト配列</param>
		/// <param name="newSize">バイト配列の新しい大きさ</param>
		/// <returns>サイズが変更されたバイト配列</returns>
		private static byte[] ResizeBytesArray(byte[] bytes, int newSize)
		{
			byte[] newBytes = new byte[newSize];
			if (bytes.Length <= newSize) {
				for (int i = 0; i < bytes.Length; i++)
					newBytes[i] = bytes[i];
			} else {
				int pos = 0;
				for (int i = 0; i < bytes.Length; i++) {
					newBytes[pos++] ^= bytes[i];
					if (pos >= newBytes.Length)
						pos = 0;
				}
			}
			return newBytes;
		}

		private static string MakeEncryptKey()
		{
			return string.Format("{0}{1}", Environment.UserName, Environment.MachineName);
		}

		#endregion

		#region エラー報告

		/// <summary>
		/// 現在の環境を報告する文字列を生成します
		/// </summary>
		/// <returns></returns>
		public static string MakeEnvironmentReport()
		{
			return string.Format(
				"User:{0}\r\nPath:{1}\r\nTime:{2}\r\nOS:{3}\r\n{4}:{5}\r\n.net framework:{6}",
				Environment.UserName,
				System.Windows.Forms.Application.ExecutablePath,
				DateTime.Now.ToString(),
				Environment.OSVersion.ToString(),
				System.Windows.Forms.Application.ProductName,
				System.Windows.Forms.Application.ProductVersion,
				Environment.Version.ToString()
			);
		}

		/// <summary>
		/// 例外を文字列に展開します
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string MakeExceptionReport(Exception ex)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			while (ex != null) {
				sb.AppendFormat(
					"Name:{0}\r\nMessage:{1}\r\nStackTrace:{2}\r\n",
					 ex.GetType().Name,
					 ex.Message,
					 ex.StackTrace
				);
				ex = ex.InnerException;
			}
			return sb.ToString();
		}

		#endregion

		#region 文字操作

		/// <summary>
		/// 全角日本語かどうかを判定します
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsZenkakuJapanese(char c)
		{
			return (char)0x3040 <= c && (char)c <= 0x30ff;
		}

		/// <summary>
		/// 半角日本語化を判定します
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsHankakuJapanese(char c)
		{
			return (0xFF66 <= c && c <= 0xFF9F);
		}

		/// <summary>
		/// 平仮名かどうかを判定します
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsHiragana(char c)
		{
			return (char)0x3040 <= c && (char)c <= 0x309f;
		}

		/// <summary>
		/// カタカナかどうかを判定します
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsKatakana(char c)
		{
			return (char)0x30A0 <= c && (char)c <= 0x30ff;
		}

		/// <summary>
		/// 全角かどうかを判定します
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsZenkakuCase(char c)
		{
			return (0xFF01 <= c && c <= 0xFF5E) || c == 0x3000;
		}

		/// <summary>
		/// 半角かどうかを判定します
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsHankakuCase(char c)
		{
			return (0x0020 <= c && c <= 0x007E);
		}

		/// <summary>
		/// カタカナをひらがなにします
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static char ToHiragana(char c)
		{
			if (IsKatakana(c)) {
				return (char)(c - 0x30A0 + 0x3040);
			}
			return c;
		}

		/// <summary>
		/// ひらがなをカタカナにします
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static char ToKatakana(char c)
		{
			if (IsHiragana(c)) {
				return (char)(c + 0x30A0 - 0x3040);
			}
			return c;
		}

		static string[] _dic = new string[]{
			" ", "ｧ", "ｱ","ｨ","ｲ","ｩ","ｳ","ｪ","ｴ","ｫ","ｵ",
			"ｶ","ｶﾞ","ｷ","ｷﾞ","ｸ","ｸﾞ","ｹ","ｹﾞ","ｺ","ｺﾞ",
			"ｻ","ｻﾞ","ｼ","ｼﾞ","ｽ","ｽﾞ","ｾ","ｾﾞ","ｿ","ｿﾞ",
			"ﾀ","ﾀﾞ","ﾁ","ｼﾞ","ｯ","ﾂ","ﾂﾞ","ﾃ","ﾃﾞ","ﾄ","ﾄﾞ",
			"ﾅ","ﾆ","ﾇ","ﾈ","ﾉ",
			"ﾊ","ﾊﾞ","ﾊﾟ","ﾋ","ﾋﾞ","ﾋﾟ","ﾌ","ﾌﾞ","ﾌﾟ","ﾍ","ﾍﾞ","ﾍﾟ","ﾎ","ﾎﾞ","ﾎﾟ",
			"ﾏ","ﾐ","ﾑ","ﾒ","ﾓ",
			"ｬ","ﾔ","ｭ","ﾕ","ｮ","ﾖ",
			"ﾗ", "ﾘ", "ﾙ", "ﾚ", "ﾛ",
			"ﾜ","ﾜ","ｳｨ","ｳ","ｦ","ﾝ","ｳﾞ","ｶ","ｹ"
		};

		/// <summary>
		/// 文字を半角にします
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static string ToHankaku(char c)
		{

			if (0xFF01 <= c && c <= 0xFF5E) {
				c -= (char)0xFEE0;
				return c.ToString();
			} else if (c == 0x3000) {
				c = (char)0x0020;
				return c.ToString();
			}

			if (0x3040 <= c && c <= 0x30FF) {
				int i = (c - 0x3040) % 0x60;
				if (i < _dic.Length) {
					return _dic[i];
				}
			}

			return c.ToString();
		}

		/// <summary>
		/// 文字を全角にします
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static char ToZenkaku(char c)
		{
			if (IsHankakuCase(c)) {
				return (char)(c + 0xFEE0);
			}

			return c;

		}

		#endregion

		#region ID検出

		/// <summary>
		/// 生放送のId部分を抽出します。
		/// </summary>
		/// <param name="url">http://live.nicovideo.jp/watch/lv[数値]とかlv[数値]とか</param>
		/// <returns></returns>
		public static string GetLiveIdFromUrl(string url)
		{

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(url, NicoApiSharp.ApiSettings.Default.LiveIdRegPattern);

			if (match.Success) {
				return match.Groups["id"].Value;

			} else {
				return null;
			}
		}

		/// <summary>
		/// 実況のId部分を抽出します。
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string GetJikkyoIdFromUrl(string url)
		{

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(url, NicoApiSharp.ApiSettings.Default.JikkyoIdRegPattern);

			if (match.Success) {
				return match.Groups["id"].Value;

			} else {
				return null;
			}
		}

		#endregion

		


	}
}
