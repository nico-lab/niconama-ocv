using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Lunascape5GeckoBrowserManager : IBrowserManager
	{
		const string COOKIEPATH = "%APPDATA%\\Lunascape\\Lunascape5\\ApplicationData\\gecko\\cookies.sqlite";

		#region IBrowserManager メンバ

		public BrowserType BrowserType
		{
			get { return BrowserType.Lunascape5Gecko; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				path = null;
			}

			CookieStatus status = new CookieStatus("Lunascape5 Gecko", path, this.BrowserType, PathType.File);
			return new Firefox3CookieGetter(status);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[] { CreateDefaultCookieGetter() };
		}

		#endregion
	}
}
