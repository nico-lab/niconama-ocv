using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{

	/// <summary>
	/// IE系のすべてのクッキーを取得する
	/// </summary>
	class IEBrowserManager : IBrowserManager
	{
		#region IBrowserManager メンバ

		public BrowserType BrowserType
		{
			get { return BrowserType.IE; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			CookieStatus status = new CookieStatus(this.BrowserType.ToString(), cookieFolder, this.BrowserType, PathType.Directory);
			return new IECookieGetter(status, true);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			string lowFolder = System.IO.Path.Combine(cookieFolder, "low");
			if (System.IO.Directory.Exists(lowFolder)) {
				IEComponentBrowserManager iec = new IEComponentBrowserManager();
				IESafemodeBrowserManager ies = new IESafemodeBrowserManager();
				return new ICookieGetter[] { iec.CreateDefaultCookieGetter(), ies.CreateDefaultCookieGetter() };
			} else {
				return new ICookieGetter[] { CreateDefaultCookieGetter() };
			}
		}

		#endregion
	}
}
