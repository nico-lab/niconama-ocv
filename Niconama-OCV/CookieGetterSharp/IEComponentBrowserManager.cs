using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// IEコンポーネントでアクセス可能なクッキーのみを取得する
	/// </summary>
	class IEComponentBrowserManager : IBrowserManager
	{
		#region IBrowserManager メンバ

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.IEComponet; }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			BrowserStatus bs = new BrowserStatus();
			bs.Name = BrowserType.ToString();
			bs.CookiePath = cookieFolder;
			bs.CookieGetter = new IECookieGetter(cookieFolder, false);
			return bs;
		}

		/// <summary>
		/// IEBrowserManagerで環境にあわせて適切な物を返すようにしてあるので、ここでは何もしない
		/// </summary>
		/// <returns></returns>
		public IBrowserStatus[] GetStatus()
		{
			return new IBrowserStatus[0];
		}

		#endregion
	}
}
