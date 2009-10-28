﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	public class Waybackkey
	{
		public static Waybackkey GetInstance(int thread, System.Net.CookieContainer cookies)
		{
			try {
				string url = string.Format(ApplicationSettings.Default.WaybackkeyUrlFormat, thread);
				string res = Utility.GetResponseText(url, cookies, ApplicationSettings.Default.DefaultApiTimeout);
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

		public string Value
		{
			get { return _value; }
		}

	}
}