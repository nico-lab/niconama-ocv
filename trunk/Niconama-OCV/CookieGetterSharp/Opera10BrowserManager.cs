using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Opera10BrowserManager : IBrowserManager
	{
		
		const string COOKIEPATH = "%APPDATA%\\Opera\\Opera\\cookies4.dat";

		#region IBrowserManager ÉÅÉìÉo

		public BrowserType BrowserType
		{
			get { return BrowserType.Opera10; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				path = null;
			}

			CookieStatus status = new CookieStatus(this.BrowserType.ToString(), path, this.BrowserType, PathType.File);
			return new Opera10CookieGetter(status);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[] { CreateDefaultCookieGetter() };
		}

		#endregion
	}
}
