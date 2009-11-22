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
				NicoApiSharp.Logger.Default.LogException(ex);
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
				NicoApiSharp.Logger.Default.LogException(ex);
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
					NicoApiSharp.Logger.Default.LogException(ex);
				}

			}

			return null;
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
				NicoApiSharp.Logger.Default.LogException(ex);
				System.Windows.Forms.MessageBox.Show("クリップボードへの貼り付けに失敗しました。このエラーが頻繁に発生する場合は作者まで連絡してください。", "コピーが失敗しました。",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
			}
		}

		#region 文字操作

		public static bool IsZenkakuJapanese(char c)
		{
			return (char)0x3040 <= c && (char)c <= 0x30ff;
		}

		public static bool IsHankakuJapanese(char c) {
			return (0xFF66 <= c && c <= 0xFF9F);
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

		

		public static string ToHankaku(char c)
		{
		
			if (0xFF01 <= c && c <= 0xFF5E) {
				c -= (char)0xFEE0;
				return c.ToString();
			} else if (c == 0x3000) {
				c = (char)0x0020;
				return c.ToString();
			}

			if(0x3040 <= c && c<= 0x30FF){
				int i = (c - 0x3040) % 0x60;
				if (i < _dic.Length) {
					return _dic[i];
				}
			}

			return c.ToString();
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

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(url, NicoApiSharp.ApiSettings.Default.LiveIdRegPattern);

			if (match.Success) {
				return match.Groups["id"].Value;

			} else {
				return null;
			}
		}

		#endregion

	}
}
