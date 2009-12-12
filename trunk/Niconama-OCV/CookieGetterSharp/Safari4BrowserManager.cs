using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Safari4BrowserManager : IBrowserManager
	{
		const string COOKIEPATH = "%APPDATA%\\Apple Computer\\Safari\\Cookies\\Cookies.plist";

		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.Safari4; }
		}

		public string BrowserName
		{
			get { return BrowserType.ToString(); }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				path = null;
			}

			BrowserStatus bs = new BrowserStatus();
			bs.Name = BrowserName;
			bs.CookiePath = path;
			bs.CookieGetter = new Safari4CookieGetter(path);

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
