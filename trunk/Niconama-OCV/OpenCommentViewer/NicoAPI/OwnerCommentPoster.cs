using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;


namespace OpenCommentViewer.NicoAPI
{

	/// <summary>
	/// 主米を投稿するためのクラス
	/// </summary>
	public static class OwnerCommentPoster
	{
		/// <summary>
		/// 放送主コメントを送信します
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static bool Post(string liveId, string message, string command, CookieContainer cookies) {

			HttpWebResponse webRes = null;
			StreamReader sr = null;
			bool result = false;

			try {

				//ポストデータ
				message = System.Web.HttpUtility.UrlEncode(message);
				command = System.Web.HttpUtility.UrlEncode(command);
				string postData;

				postData = String.Format(ApplicationSettings.Default.PostOwnerCommentDataFormat, message, command);
				
				
				byte[] postDataBytes = System.Text.Encoding.UTF8.GetBytes(postData);

				string url = string.Format(ApplicationSettings.Default.PostOwnerCommentUrlFormat, liveId);
				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
				webReq.Method = "POST";
				webReq.Referer = ApplicationSettings.Default.PostOwnerCommentReferer;
				webReq.ContentType = "application/x-www-form-urlencoded";
				webReq.UserAgent = ApplicationSettings.Default.PostOwnerCommentUserAgent;

				webReq.ContentLength = postDataBytes.Length;
				webReq.Timeout = 1000;
				webReq.CookieContainer = cookies;

				Stream reqStream = webReq.GetRequestStream();
				reqStream.Write(postDataBytes, 0, postDataBytes.Length);
				reqStream.Close();

				webRes = (HttpWebResponse)webReq.GetResponse();
				sr = new StreamReader(webRes.GetResponseStream());
				string res = sr.ReadToEnd();
				result = res.Equals("status=ok");

			} catch (WebException ex) {
				Logger.Default.LogException(ex);
			} finally {

				if (sr != null)
					sr.Close();
				if (webRes != null)
					webRes.Close();

			}

			return result;
		}
	}
}
