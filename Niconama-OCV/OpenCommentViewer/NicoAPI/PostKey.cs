using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
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
				string url = string.Format(ApplicationSettings.Default.GetPostKeyUrlFormat, thread, blockNo);
				string res = Utility.GetResponseText(url, cookies, ApplicationSettings.Default.DefaultApiTimeout);
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
