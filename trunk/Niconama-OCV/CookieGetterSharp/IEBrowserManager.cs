using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class IEBrowserManager : IBrowserManager
	{
		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.IE; }
		}

		public string BrowserName
		{
			get { return BrowserType.ToString(); }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			BrowserStatus bs = new BrowserStatus();
			bs.Name = CookieGetter.BROWSER_TYPE.IE.ToString();
			bs.CookiePath = cookieFolder;
			bs.CookieGetter = new IECookieGetter();
			return bs;
		}

		public IBrowserStatus[] GetStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			string lowFolder = System.IO.Path.Combine(cookieFolder, "low");
			if (System.IO.Directory.Exists(lowFolder)) {
				IEComponentBrowserManager iec = new IEComponentBrowserManager();
				IESafemodeBrowserManager ies = new IESafemodeBrowserManager();
				return new IBrowserStatus[] { iec.GetDefaultStatus(), ies.GetDefaultStatus() };
			} else {
				return new IBrowserStatus[] { GetDefaultStatus() };
			}
		}

		#endregion
	}
}
