using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class GoogleChrome3BrowserManager : IBrowserManager
	{
		const string COOKIEPATH = "%LOCALAPPDATA%\\Google\\Chrome\\User Data\\Default\\Cookies";

		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.GoogleChrome3; }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				path = null;
			}

			BrowserStatus bs = new BrowserStatus();
			bs.Name = BrowserType.ToString();
			bs.CookiePath = path;
			bs.CookieGetter = new GoogleChrome3CookieGetter(path);

			return bs;
		}

		public IBrowserStatus[] GetStatus()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				return new BrowserStatus[0];
			}

			return new IBrowserStatus[] { GetDefaultStatus() };
		}

		#endregion
	}
}
