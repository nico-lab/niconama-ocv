using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Live
{

	/// <summary>
	/// コメントをポストするためのキーを取得するためのクラス
	/// </summary>
	public class PostKey
	{

		/// <summary>
		/// サーバーからキーを取得します
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="lastCommentNo"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static PostKey GetInstance(int thread, int lastCommentNo, System.Net.CookieContainer cookies)
		{
			try {
				int blockNo = (lastCommentNo + 1) / 100;
				string url = string.Format(ApiSettings.Default.GetPostKeyUrlFormat, thread, blockNo);
				string res = Utility.GetResponseText(url, cookies, ApiSettings.Default.DefaultApiTimeout);
				if (res != null) {
					string[] p = res.Split('=');
					if (p.Length == 2) {
						PostKey w = new PostKey();
						w._value = p[1];
						return w;
					}
				}
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}
			return null;
		}

		/// <summary>
		/// サーバーからキーを取得します
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="lastCommentNo"></param>
		/// <returns></returns>
		public static PostKey GetInstance(int thread, int lastCommentNo) {
			if (LoginManager.DefaultCookies != null) {
				return GetInstance(thread, lastCommentNo, LoginManager.DefaultCookies);
			}

			return null;
		}

		private string _value = null;

		/// <summary>
		/// キーを取得します
		/// </summary>
		public string Value
		{
			get { return _value; }
		}
	}
}
