using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Cookie
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
			/// VISTAの保護モード
			/// </summary>
			IESafeMode,

			/// <summary>
			/// Firefox
			/// </summary>
			Firefox,

			/// <summary>
			/// Google Chrome
			/// </summary>
			Chrome
		}

		/// <summary>
		/// 指定したブラウザからクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <returns>成功したらその値、失敗したらnull</returns>
		public static string GetCookie(string url, string key, BROWSER_TYPE type)
		{

			if (url != null && key != null) {

				url = ModifyUrl(url, type);

				ICookieGetter getter = null;

				switch (type) {
					case BROWSER_TYPE.IEComponent:
						getter = new IEComponentCookieGetter();
						break;

					case BROWSER_TYPE.IESafeMode:
						getter = new IESafeModeCookieGetter();
						break;

					case BROWSER_TYPE.Firefox:
						getter = new FirefoxCookieGetter();
						break;

					case BROWSER_TYPE.Chrome:
						getter = new ChromeCookieGetter();
						break;
				}

				if (getter != null) {
					return getter.GetCookieValue(url, key);
				}

			}

			return null;
		}

		/// <summary>
		/// クッキーの保存先を指定して取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <param name="path">クッキーが保存されているファイル</param>
		/// <returns>成功したらその値、失敗したらnull</returns>
		public static string GetCookie(string url, string key, BROWSER_TYPE type, string path)
		{
			if (path == null) {
				return GetCookie(url, key, type);
			}

			if (url != null && key != null) {

				url = ModifyUrl(url, type);

				ICookieGetter getter = null;

				switch (type) {
					case BROWSER_TYPE.IEComponent:
						getter = new IEComponentCookieGetter();
						break;

					case BROWSER_TYPE.IESafeMode:
						getter = new IESafeModeCookieGetter();
						break;

					case BROWSER_TYPE.Firefox:
						getter = new FirefoxCookieGetter(path);
						break;

					case BROWSER_TYPE.Chrome:
						getter = new ChromeCookieGetter(path);
						break;
				}

				if (getter != null) {
					return getter.GetCookieValue(url, key);
				}

			}

			return null;
		}

		/// <summary>
		/// URLを各ブラウザに対応したものに変換する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private static string ModifyUrl(string url, BROWSER_TYPE type)
		{

			url = url.Replace("http://", "");

			if (url.StartsWith(".")) {
				url.Remove(0, 1);
			}

			switch (type) {
				case BROWSER_TYPE.IEComponent:
					return "http://" + url;

				case BROWSER_TYPE.IESafeMode:
					return url;

				case BROWSER_TYPE.Firefox:
				case BROWSER_TYPE.Chrome:
					if (url.StartsWith("www")) {
						return url;
					} else {
						return "." + url;
					}
			}

			return null;
		}


	}
}
