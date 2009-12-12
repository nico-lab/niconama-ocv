using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Opera10BrowserManager : IBrowserManager
	{
		
		const string COOKIEPATH = "%APPDATA%\\Opera\\Opera\\cookies4.dat";

		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.Opera10; }
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
			bs.CookieGetter = new Opera10CookieGetter(path);

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
