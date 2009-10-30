using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Live
{
	public class Waybackkey
	{
		/// <summary>
		/// Waybackkeyをサーバーから取得します
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static Waybackkey GetInstance(int thread, System.Net.CookieContainer cookies)
		{
			try {
				string url = string.Format(ApiSettings.Default.WaybackkeyUrlFormat, thread);
				string res = Utility.GetResponseText(url, cookies, ApiSettings.Default.DefaultApiTimeout);
				if (res != null) {
					string[] p = res.Split('=');
					if (p.Length == 2) {
						Waybackkey w = new Waybackkey();
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
		/// Waybackkeyを取得します
		/// </summary>
		public string Value
		{
			get { return _value; }
		}

	}
}
