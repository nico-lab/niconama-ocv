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

		#region IBrowserManager ÉÅÉìÉo
		
		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.Firefox3; }
		}

		public string BrowserName
		{
			get { return BrowserType.ToString(); }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			Firefox3Profile prof = Firefox3Profile.GetDefaultProfile(Utility.ReplacePathSymbols(DATAFOLDER), INIFILE_NAME);
			return CreateBrowserStatus(prof); 
		}

		public IBrowserStatus[] GetStatus()
		{
			Firefox3Profile[] profs = Firefox3Profile.GetProfiles(Utility.ReplacePathSymbols(DATAFOLDER), INIFILE_NAME);
			BrowserStatus[] infos = new BrowserStatus[profs.Length];
			for (int i = 0; i < profs.Length; i++) {
				infos[i] = CreateBrowserStatus(profs[i]);
			}
			return infos;
		}

		#endregion

		private BrowserStatus CreateBrowserStatus(Firefox3Profile prof)
		{
			BrowserStatus bs = new BrowserStatus();
			if (prof != null) {
				bs.CookiePath = System.IO.Path.Combine(prof.path, COOKEFILE_NAME);
				bs.CookieGetter = new Firefox3CookieGetter(bs.CookiePath);
				bs.Name = BrowserName + "-" + prof.name;
			} else {
				bs.CookiePath = null;
				bs.CookieGetter = new Firefox3CookieGetter();
				bs.Name = BrowserName;
			}
			return bs;
		}
	}
}
