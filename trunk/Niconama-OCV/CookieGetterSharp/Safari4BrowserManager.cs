using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Safari4BrowserManager : IBrowserManager
	{
		const string COOKIEPATH = "%APPDATA%\\Apple Computer\\Safari\\Cookies\\Cookies.plist";

		#region IBrowserManager メンバ

		public BrowserType BrowserType
		{
			get { return BrowserType.Safari4; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string path = Utility.ReplacePathSymbols(COOKIEPATH);

			if (!System.IO.File.Exists(path)) {
				path = null;
			}

			CookieStatus status = new CookieStatus(this.BrowserType.ToString(), path, this.BrowserType, PathType.File);
			return new Safari4CookieGetter(status);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[] { CreateDefaultCookieGetter() };
		}

		#endregion
	}
}
