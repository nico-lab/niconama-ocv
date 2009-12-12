using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
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
			/// IE系ブラウザ(IEComponet + IESafemode)
			/// </summary>
			IE,

			/// <summary>
			/// XPのIEやトライデントエンジンを使用しているブラウザ
			/// </summary>
			IEComponet,

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
			Lunascape6Gecko
		}


		#region Static Member

		static IBrowserManager[] _browserManagers;

		static CookieGetter() {
			_browserManagers = new IBrowserManager[]{
				new IEBrowserManager(),
				new IEComponentBrowserManager(),
				new IESafemodeBrowserManager(),
				new Firefox3BrowserManager(),
				new GoogleChrome3BrowserManager(),
				new Opera10BrowserManager(),
				new Safari4BrowserManager(),
				new Lunascape5GeckoBrowserManager(),
				new Lunascape6GeckoBrowserManager()
			};
		}

		/// <summary>
		/// 指定したブラウザ用のクッキーゲッターを取得する
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ICookieGetter GetInstance(BROWSER_TYPE type)
		{
			foreach (IBrowserManager manager in _browserManagers) {
				if (manager.BrowserType == type) {
					return manager.GetDefaultStatus().CookieGetter;
				}
			}

			return null;
		}

		/// <summary>
		/// 利用可能なブラウザの情報を取得します。
		/// </summary>
		/// <returns></returns>
		public static IBrowserStatus[] GetBrowserStatus()
		{
			List<IBrowserStatus> results = new List<IBrowserStatus>();

			foreach (IBrowserManager manager in _browserManagers) {
				results.AddRange(manager.GetStatus());
			}

			return results.ToArray();
		}

		#endregion


		private string _cookiePath = "";

		#region ICookieGetter メンバ

		/// <summary>
		/// クッキーが保存されているファイル・ディレクトリへのパスを取得・設定します。
		/// </summary>
		public string CookiePath
		{
			get { return _cookiePath; }
			set { _cookiePath = value; }
		}

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <exception cref="CookieGetterException"></exception>
		/// <returns>対象のクッキー。なければnull</returns>
		public virtual System.Net.Cookie GetCookie(Uri url, string key)
		{
			System.Net.CookieCollection collection = GetCookieCollection(url);
			return collection[key];
		}

		/// <summary>
		/// urlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <exception cref="CookieGetterException"></exception>
		/// <returns></returns>
		public virtual System.Net.CookieCollection GetCookieCollection(Uri url)
		{
			System.Net.CookieContainer container = GetAllCookies();
			return container.GetCookies(url);
		}

		/// <summary>
		/// すべてのクッキーを取得します。
		/// </summary>
		/// <exception cref="CookieGetterException"></exception>
		/// <returns></returns>
		public abstract System.Net.CookieContainer GetAllCookies();

		#endregion

		/// <summary>
		/// 必要があればuriの最後に/をつける
		/// Pathの指定がある場合、uriの最後に/があるかないかで取得できない場合があるので
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		protected static Uri AddSrashLast(Uri uri) { 
			string o = uri.Segments[uri.Segments.Length - 1];
			string no = uri.OriginalString;//.Replace("http://", "http://o.");
			if (!o.Contains(".") && o[o.Length - 1] != '/') {
				no += "/";
			} 
			return new Uri(no);
		}

		/// <summary>
		/// クッキーコンテナにクッキーを追加する
		/// domainが.hal.fscs.jpなどだと http://hal.fscs.jp でクッキーが有効にならないので.ありとなし両方指定する
		/// </summary>
		/// <param name="container"></param>
		/// <param name="cookie"></param>
		protected static void AddCookieToContainer(System.Net.CookieContainer container, System.Net.Cookie cookie)
		{

			if (container == null) {
				throw new ArgumentNullException("container");
			}

			if (cookie == null) {
				throw new ArgumentNullException("cookie");
			}

			container.Add(cookie);
			if (cookie.Domain.StartsWith(".")) {
				container.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain.Substring(1)));
			}

		}

		
	}
}
