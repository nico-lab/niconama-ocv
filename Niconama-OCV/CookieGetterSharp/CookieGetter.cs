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

		#region [静的メンバー]

		static IBrowserManager[] _browserManagers;

		static CookieGetter()
		{
			_browserManagers = new IBrowserManager[]{
				new IEBrowserManager(),
				new IEComponentBrowserManager(),
				new IESafemodeBrowserManager(),
				new Firefox3BrowserManager(),
				new GoogleChrome3BrowserManager(),
				new Opera10BrowserManager(),
				new Safari4BrowserManager(),
				new Lunascape5GeckoBrowserManager(),
				new Lunascape6GeckoBrowserManager(),
				new Lunascape6WebkitBrowserManager(),
				new ChromiumBrowserManager()				
			};
		}

		/// <summary>
		/// 指定したブラウザ用のクッキーゲッターを取得する
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ICookieGetter CreateInstance(BrowserType type)
		{
			foreach (IBrowserManager manager in _browserManagers) {
				if (manager.BrowserType == type) {
					return manager.CreateDefaultCookieGetter();
				}
			}

			return null;
		}

		/// <summary>
		/// すべてのクッキーゲッターを取得する
		/// </summary>
		/// <param name="availableOnly">利用可能なものだけを選択するかどうか</param>
		/// <returns></returns>
		public static ICookieGetter[] CreateInstances(bool availableOnly)
		{
			List<ICookieGetter> results = new List<ICookieGetter>();

			foreach (IBrowserManager manager in _browserManagers) {
				if (availableOnly) {
					foreach (ICookieGetter cg in manager.CreateCookieGetters()) {
						if (cg.Status.IsAvailable) {
							results.Add(cg);
						}
					}
				} else {
					results.AddRange(manager.CreateCookieGetters());
				}
			}

			return results.ToArray();
		}

		#endregion [静的メンバー]

		private readonly ICookieStatus _cookieStatus;

		internal CookieGetter(ICookieStatus status) {
			if (status == null) {
				throw new ArgumentNullException("status");
			}
			_cookieStatus = status;
		}

		#region ICookieGetter メンバ

		/// <summary>
		/// クッキーが保存されているファイル・ディレクトリへのパスを取得・設定します。
		/// </summary>
		internal string CookiePath
		{
			get {
				return this.Status.CookiePath;
			}

			set {
				this.Status.CookiePath = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ICookieStatus Status
		{
			get { return _cookieStatus; }
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

		#region Objectのオーバーライド

		/// <summary>
		/// 設定の名前を返します。
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Status.Name;
		}

		/// <summary>
		/// クッキーゲッターを比較して等しいか検査します
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ICookieGetter that = obj as ICookieGetter;
			if (that == null) return false;

			return this.Status.Equals(that.Status);
		}

		/// <summary>
		/// ハッシュ値を計算します
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Status.GetHashCode();
		}

		#endregion


	}
}
