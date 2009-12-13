using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Firefox3BrowserManager : IBrowserManager
	{
		const string DATAFOLDER = "%APPDATA%\\Mozilla\\Firefox\\";
		const string INIFILE_NAME = "profiles.ini";
		const string COOKEFILE_NAME = "cookies.sqlite";

		#region IBrowserManager メンバ

		public BrowserType BrowserType
		{
			get { return BrowserType.Firefox3; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			Firefox3Profile prof = Firefox3Profile.GetDefaultProfile(Utility.ReplacePathSymbols(DATAFOLDER), INIFILE_NAME);
			return CreateCookieGetter(prof);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			Firefox3Profile[] profs = Firefox3Profile.GetProfiles(Utility.ReplacePathSymbols(DATAFOLDER), INIFILE_NAME);

			if (profs.Length == 0) {
				return new ICookieGetter[] { CreateCookieGetter(null) };
			}

			ICookieGetter[] cgs = new ICookieGetter[profs.Length];
			for (int i = 0; i < profs.Length; i++) {
				cgs[i] = CreateCookieGetter(profs[i]);
			}
			return cgs;
		}

		#endregion

		private ICookieGetter CreateCookieGetter(Firefox3Profile prof)
		{
			string name = "Firefox3";
			string path = null;

			if (prof != null) {
				name += " " + prof.name;
				path = System.IO.Path.Combine(prof.path, COOKEFILE_NAME);				
			}

			CookieStatus status = new CookieStatus(name, path, this.BrowserType, PathType.File);
			return new Firefox3CookieGetter(status);
		}
	}
}
