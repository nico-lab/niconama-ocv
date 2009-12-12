using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class IEComponentBrowserManager : IBrowserManager
	{
		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.IEComponet; }
		}

		public string BrowserName
		{
			get { return BrowserType.ToString(); }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			BrowserStatus bs = new BrowserStatus();
			bs.Name = CookieGetter.BROWSER_TYPE.IEComponet.ToString();
			bs.CookiePath = cookieFolder;
			bs.CookieGetter = new IECookieGetter(cookieFolder, false);
			return bs;
		}

		public IBrowserStatus[] GetStatus()
		{
			return new IBrowserStatus[0];
		}

		#endregion
	}
}
