using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// ブラウザの種類
	/// </summary>
	public enum BrowserType
	{

		/// <summary>
		/// IE系ブラウザ(IEComponent + IESafemode)
		/// </summary>
		IE,

		/// <summary>
		/// XPのIEやトライデントエンジンを使用しているブラウザ
		/// </summary>
		IEComponent,

		/// <summary>
		/// Vista以降のIE
		/// </summary>
		IESafemode,

		/// <summary>
		/// Firefox
		/// </summary>
		Firefox3,

		/// <summary>
		/// Google Chrome
		/// </summary>
		GoogleChrome3,

		/// <summary>
		/// Opera10
		/// </summary>
		Opera10,

		/// <summary>
		/// Safari4
		/// </summary>
		Safari4,

		/// <summary>
		/// Lunascape5 Geckoエンジン
		/// </summary>
		Lunascape5Gecko,

		/// <summary>
		/// Lunascape6 Geckoエンジン
		/// </summary>
		Lunascape6Gecko,

		/// <summary>
		/// Lunascape6 Webkitエンジン
		/// </summary>
		Lunascape6Webkit,

		/// <summary>
		/// Google Chromium
		/// </summary>
		Chromium
	}
}
