using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// 指定したブラウザからクッキーを取得する
	/// </summary>
	public static class CookieGetter
	{

		/// <summary>
		/// ブラウザの種類
		/// </summary>
		public enum BROWSER_TYPE
		{

			/// <summary>
			/// XPのIEやトライデントエンジンを使用しているブラウザ
			/// </summary>
			IEComponent,

			/// <summary>
			/// Firefox
			/// </summary>
			Firefox,

			/// <summary>
			/// Google Chrome
			/// </summary>
			Chrome,

			/// <summary>
			/// Opera10
			/// </summary>
			Opera
		}

		/// <summary>
		/// 指定したブラウザ用のクッキーゲッターを取得する
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ICookieGetter GetInstance(BROWSER_TYPE type)
		{
			switch (type) {
				case BROWSER_TYPE.IEComponent:
					return new IEComponentCookieGetter();
				case BROWSER_TYPE.Firefox:
					return new FirefoxCookieGetter();
				case BROWSER_TYPE.Chrome:
					return new ChromeCookieGetter();
				case BROWSER_TYPE.Opera:
					return new OperaCookieGetter();
				default:
					return null;
				
			}
		}

	}
}
