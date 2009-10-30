using System;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Web;

namespace Hal.OpenCommentViewer
{

	/// <summary>
	/// 汎用関数群
	/// </summary>
	public static class Utility
	{

		/// <summary>
		/// Typeで指定されたクラスをXMLシリアライズする
		/// </summary>
		/// <param name="filePath">保存場所</param>
		/// <param name="graph">シリアライズ対象</param>
		/// <param name="type">シリアライズするクラス情報</param>
		/// <returns>成功・失敗</returns>
		static public bool Serialize(string filePath, object graph, Type type)
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
		static public string Serialize(object graph, Type type)
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
		/// シリアライズされたオブジェクトを復元する
		/// 失敗した場合はnullを返す
		/// </summary>
		/// <param name="filePath">対象のファイル</param>
		/// <param name="type">対象のクラス情報</param>
		/// <returns>復元されたオブジェクト・失敗した場合はnullを返す</returns>
		static public object Deserialize(string filePath, Type type)
		{

			if (File.Exists(filePath)) {
				try {

					if (System.IO.File.Exists(filePath)) {
						System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);

						using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open)) {
							return serializer.Deserialize(fs);
						}
					}

				} catch (Exception ex) {
					Logger.Default.LogException(ex);
				}

			}

			return null;
		}

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
		/// <returns></returns>
		static public string PostData(string url, string postData, CookieContainer cookies, int defaultTimeout)
		{

			HttpWebResponse webRes = null;
			StreamReader sr = null;

			try {

				byte[] postDataBytes = System.Text.Encoding.UTF8.GetBytes(postData);

				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
				webReq.Method = "POST";
				webReq.Referer = ApiSettings.Default.PostOwnerCommentReferer;
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
		/// <param name="UnixTime"></param>
		/// <returns></returns>
		public static int DateTimeToUnixTime(DateTime time)
		{
			TimeSpan t = time.Subtract(new DateTime(1970, 1, 1, 9, 0, 0));
			return (int)t.TotalSeconds;
		}

		public static void SetTxetToClipboard(string text)
		{
			//virtual pc 導入環境だとエラーが発生する可能性がある

			try {
				if (string.IsNullOrEmpty(text)) {
					System.Windows.Forms.Clipboard.Clear();
				} else { 
					System.Windows.Forms.Clipboard.SetText(text);
				}
				
			} catch (Exception ex){
				Logger.Default.LogException(ex);
				System.Windows.Forms.MessageBox.Show("クリップボードへの貼り付けに失敗しました。このエラーが頻繁に発生する場合は作者まで連絡してください。", "コピーが失敗しました。",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
			}
		}

		#region 文字操作

		public static bool IsZenkakuJapanese(char c)
		{
			return (char)0x3040 <= c && (char)c <= 0x30ff;
		}

		public static bool IsHiragana(char c)
		{
			return (char)0x3040 <= c && (char)c <= 0x309f;
		}

		public static bool IsKatakana(char c)
		{
			return (char)0x30A0 <= c && (char)c <= 0x30ff;
		}

		public static bool IsZenkakuCase(char c)
		{
			return (0xFF01 <= c && c <= 0xFF5E) || c == 0x3000;
		}

		public static bool IsHankakuCase(char c)
		{
			return (0x0020 <= c && c <= 0x007E);
		}

		public static char ToHiragana(char c)
		{
			if (IsKatakana(c)) {
				return (char)(c - 0x30A0 + 0x3040);
			}
			return c;
		}

		public static char ToKatakana(char c)
		{
			if (IsHiragana(c)) {
				return (char)(c + 0x30A0 - 0x3040);
			}
			return c;
		}

		public static string ToHankaku(string str)
		{
			if (str.Length == 1) {
				char c = str[0];
				if (0xFF01 <= c && c <= 0xFF5E) {
					c -= (char)0xFEE0;
				} else if (c == 0x3000) {
					c = (char)0x0020;
				}
				str = c.ToString();
			}

			str = System.Text.RegularExpressions.Regex.Replace(str, "\\p{IsKatakana}",
			(System.Text.RegularExpressions.MatchEvaluator)delegate(System.Text.RegularExpressions.Match match)
			{
#if MONO
				return match.Value;
#else
				return Microsoft.VisualBasic.Strings.StrConv(match.Value, Microsoft.VisualBasic.VbStrConv.Narrow, 0x0411);
#endif
			});

			return str;
		}

		public static char ToZenkaku(char c)
		{
			if (IsHankakuCase(c)) {
				return (char)(c + 0xFEE0);
			}

			return c;

		}

		/// <summary>
		/// 生放送のId部分を抽出します。
		/// </summary>
		/// <param name="url">http://live.nicovideo.jp/watch/lv[数値]とかlv[数値]とか</param>
		/// <returns></returns>
		public static string GetLiveIdFromUrl(string url)
		{

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(url, ApiSettings.Default.LiveIdRegPattern);

			if (match.Success) {
				return match.Groups["id"].Value;

			} else {
				return null;
			}
		}

		#endregion

		#region XML操作

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

		public static DateTime SelectDateTime(System.Xml.XmlNode node, string xpath)
		{
			int unixTime = SelectInt(node, xpath, 0);
			return Utility.UnixTimeToDateTime(unixTime);
		}
		#endregion
	}
}
