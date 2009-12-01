using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;


namespace Hal.NicoApiSharp.Live
{

	/// <summary>
	/// 主米を投稿するためのクラス
	/// </summary>
	public class OwnerCommentPoster : IDisposable
	{
		System.Threading.Thread _thread;
		System.Threading.ManualResetEvent _manualResetEvent;
		Queue<PostData> _queue;
		bool _cancel = false;

		/// <summary>
		/// コンストラクタ（非同期的に主米を送信する必要がある際に使用する）
		/// </summary>
		public OwnerCommentPoster() {
			_queue = new Queue<PostData>();
			_thread = new System.Threading.Thread(ThreadLoop);
			_manualResetEvent = new System.Threading.ManualResetEvent(true);
			_thread.IsBackground = true;
			_thread.Start();
		}

		private void AddTask(PostData postData)
		{ 
			if(!_cancel){
				lock (_queue) {
					_queue.Enqueue(postData);
				}

				_manualResetEvent.Set();
			}
		
		}

		private void ThreadLoop() {
			while (!_cancel) {
				PostData data = null;

				lock (_queue) {
					if (0 < _queue.Count) {
						data = _queue.Dequeue();
					}
				}

				if (data != null) {
					OwnerCommentPoster.Post(data.LiveId, data.Message, data.Command, data.Name, data.Cookies);
				} else {
					_manualResetEvent.Reset();
					_manualResetEvent.WaitOne();
				}
			}
		}

		/// <summary>
		/// 非同期的に運営コメントを投稿する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="name"></param>
		/// <param name="cookies"></param>
		public void PostAsync(string liveId, string message, string command, string name, CookieContainer cookies)
		{ 
			AddTask(new PostData(liveId, message, command, name, cookies));
		}

		/// <summary>
		/// 非同期的に運営コメントを投稿する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="name"></param>
		public void PostAsync(string liveId, string message, string command, string name)
		{
			AddTask(new PostData(liveId, message, command, name, null));
		}

		/// <summary>
		/// 非同期的に運営コメントを投稿する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="cookies"></param>
		public void PostAsync(string liveId, string message, string command, CookieContainer cookies)
		{
			AddTask(new PostData(liveId, message, command, null, cookies));
		}

		/// <summary>
		/// 非同期的に運営コメントを投稿する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		public void PostAsync(string liveId, string message, string command)
		{
			AddTask(new PostData(liveId, message, command, null, null));
		}


		#region IDisposable メンバ

		/// <summary>
		/// 現在のタスクを中断し、たまっているタスクを破棄する
		/// </summary>
		public void Dispose()
		{
			_cancel = true;
			_manualResetEvent.Set();
			if (_thread.IsAlive) {
				_thread.Join();
			}
			_manualResetEvent.Close();
			_manualResetEvent = null;
			_thread = null;
			_queue.Clear();
			_queue = null;
		}

		#endregion


		class PostData {
			public string LiveId;
			public string Message;
			public string Command;
			public string Name;
			public CookieContainer Cookies;

			public PostData(string liveId, string message, string command, string name, CookieContainer cookies)
			{
				this.LiveId = liveId;
				this.Message = message;
				this.Command = command;
				this.Name = name;
				this.Cookies = cookies;
			}
		}


		/// <summary>
		/// 放送主コメントを送信します
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="cookies"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool Post(string liveId, string message, string command, string name, CookieContainer cookies) {

			HttpWebResponse webRes = null;
			StreamReader sr = null;
			bool result = false;

			try {

				//ポストデータ
				message = System.Web.HttpUtility.UrlEncode(message);
				command = System.Web.HttpUtility.UrlEncode(command);
				
				string postData;

				if (name == null) {
					postData = String.Format(ApiSettings.Default.PostOwnerCommentDataFormat, message, command);
				} else { 
					name = System.Web.HttpUtility.UrlEncode(name);
					postData = String.Format(ApiSettings.Default.PostOwnerCommentWithNameDataFormat, message, command, name);
				}
				
				byte[] postDataBytes = System.Text.Encoding.UTF8.GetBytes(postData);

				string url = string.Format(ApiSettings.Default.PostOwnerCommentUrlFormat, liveId);
				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
				webReq.Method = "POST";
				webReq.Referer = ApiSettings.Default.PostOwnerCommentReferer;
				webReq.ContentType = "application/x-www-form-urlencoded";
				webReq.UserAgent = ApiSettings.Default.PostOwnerCommentUserAgent;

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

		/// <summary>
		/// 放送主コメントを送信します
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static bool Post(string liveId, string message, string command, CookieContainer cookies)
		{

			return Post(liveId, message, command, null, cookies);

		}

		/// <summary>
		/// 放送主コメントを送信します
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public static bool Post(string liveId, string message, string command)
		{
			if (LoginManager.DefaultCookies != null) {
				return Post(liveId, message, command, LoginManager.DefaultCookies);
			}

			return false;
		}

		/// <summary>
		/// 放送主コメントを送信します
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool Post(string liveId, string message, string command, string name)
		{
			if (LoginManager.DefaultCookies != null) {
				return Post(liveId, message, command, name, LoginManager.DefaultCookies);
			}

			return false;
		}

		
	}
}
