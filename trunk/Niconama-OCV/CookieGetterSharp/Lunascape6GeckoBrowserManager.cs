using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Lunascape6GeckoBrowserManager : IBrowserManager
	{
		const string LUNASCAPE_PLUGIN_FOLDER = "%APPDATA%\\Lunascape\\Lunascape6\\plugins";
		const string COOKIEPATH = "data\\cookies.sqlite";

		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.Lunascape6Gecko; }
		}

		public string BrowserName
		{
			get { return BrowserType.ToString(); }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string path = SearchDirectory();

			BrowserStatus bs = new BrowserStatus();
			bs.Name = BrowserName;
			bs.CookiePath = path;
			bs.CookieGetter = new Firefox3CookieGetter(path);

			return bs;
		}

		public IBrowserStatus[] GetStatus()
		{
			string path = SearchDirectory();

			if (path == null) {
				return new BrowserStatus[0];
			}

			return new IBrowserStatus[] { GetDefaultStatus() };
		}

		#endregion

		private string SearchDirectory() {
			foreach (string folder in System.IO.Directory.GetDirectories(Utility.ReplacePathSymbols(LUNASCAPE_PLUGIN_FOLDER))) {
				string path = System.IO.Path.Combine(folder, COOKIEPATH);
				if (System.IO.File.Exists(path)) {
					return path;
				}
			}
			return null;
		}
	}
}
