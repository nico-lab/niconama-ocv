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

		#endregion

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
	}
}
