using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Lunascape6GeckoBrowserManager : IBrowserManager
	{
		const string LUNASCAPE_PLUGIN_FOLDER = "%APPDATA%\\Lunascape\\Lunascape6\\plugins";
		const string COOKIEPATH = "data\\cookies.sqlite";

		#region IBrowserManager メンバ

		public BrowserType BrowserType
		{
			get { return BrowserType.Lunascape6Gecko; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string path = SearchDirectory();

			CookieStatus status = new CookieStatus("Lunascape6 Gecko", path, this.BrowserType, PathType.File);
			return new Firefox3CookieGetter(status);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[] { CreateDefaultCookieGetter() };
		}

		#endregion

		/// <summary>
		/// Lunascape6のプラグインフォルダからFirefoxのクッキーが保存されているパスを検索する
		/// </summary>
		/// <returns></returns>
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
