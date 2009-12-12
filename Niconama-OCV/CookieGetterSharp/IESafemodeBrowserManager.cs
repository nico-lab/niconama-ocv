using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class IESafemodeBrowserManager : IBrowserManager
	{
		#region IBrowserManager ÉÅÉìÉo

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.IESafemode; }
		}

		public string BrowserName
		{
			get { return BrowserType.ToString(); }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			string lowFolder = System.IO.Path.Combine(cookieFolder, "low");

			BrowserStatus bs = new BrowserStatus();
			bs.Name = CookieGetter.BROWSER_TYPE.IESafemode.ToString();
			bs.CookiePath = lowFolder;
			bs.CookieGetter = new IECookieGetter(lowFolder, false);
			return bs;
		}

		public IBrowserStatus[] GetStatus()
		{
			return new IBrowserStatus[0];
		}

		#endregion
	}
}
