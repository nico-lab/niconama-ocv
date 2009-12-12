using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class GoogleChrome3BrowserManager : IBrowserManager
	{
		const string COOKIEPATH = "%LOCALAPPDATA%\\Google\\Chrome\\User Data\\Default\\Cookies";

		#region IBrowserManager ÉÅÉìÉo

		public BrowserType BrowserType
		{
			get { return BrowserType.GoogleChrome3; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				path = null;
			}

			CookieStatus status = new CookieStatus(this.BrowserType.ToString(), path, this.BrowserType, PathType.File);
			return new GoogleChrome3CookieGetter(status);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[] { CreateDefaultCookieGetter() };
		}

		#endregion
	}
}
