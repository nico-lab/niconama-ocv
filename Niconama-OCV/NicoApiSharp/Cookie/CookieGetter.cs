using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// 指定したブラウザからクッキーを取得する
	/// </summary>
	abstract public class CookieGetter : ICookieGetter
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
			Firefox3,

			/// <summary>
			/// Google Chrome
			/// </summary>
			Chrome3,

			/// <summary>
			/// Opera10
			/// </summary>
			Opera10,

			/// <summary>
			/// Safari4
			/// </summary>
			Safari4
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
					return IEComponentCookieGetter.GetInstance(type);
				case BROWSER_TYPE.Firefox3:
					return FirefoxCookieGetter.GetInstance(type);
				case BROWSER_TYPE.Chrome3:
					return GoogleChromeCookieGetter.GetInstance(type);
				case BROWSER_TYPE.Opera10:
					return OperaCookieGetter.GetInstance(type);
				default:
					return null;
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected string _defaultPath = "";

		public virtual System.Net.Cookie[] GetCookies(Uri url, string key)
		{
			System.Net.Cookie cookie = GetCookie(url, key, _defaultPath);
			if(cookie != null){
				return new System.Net.Cookie[]{ cookie };
			}else{
				return new System.Net.Cookie[0];
			}
		}

		public virtual System.Net.Cookie GetCookie(Uri url, string key, string filePath)
		{
			System.Net.CookieCollection collection = GetCookieCollection(url, filePath);
			return collection[key];
		}

		public virtual System.Net.CookieCollection[] GetCookieCollections(Uri url)
		{
			return new System.Net.CookieCollection[] { GetCookieCollection(url, _defaultPath) };
		}

		public virtual System.Net.CookieCollection GetCookieCollection(Uri url, string path)
		{
			System.Net.CookieContainer container = GetAllCookies(path);
			return container.GetCookies(url);
		}

		public virtual System.Net.CookieContainer[] GetAllCookies() {
			return new System.Net.CookieContainer[] { GetAllCookies(_defaultPath) };
		}

		/// <summary>
		/// パスで指定されたファイルからすべてのクッキーを取得する
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public abstract System.Net.CookieContainer GetAllCookies(string path);

	}
}
