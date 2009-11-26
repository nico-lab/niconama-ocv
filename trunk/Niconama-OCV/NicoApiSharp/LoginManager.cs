using System;
using System.Collections.Generic;
using System.Text;
using Hal.NicoApiSharp.Cookie;

namespace Hal.NicoApiSharp
{
	/// <summary>
	/// ログイン情報を管理するクラス
	/// </summary>
	public static class LoginManager
	{
		static internal System.Net.CookieContainer DefaultCookies = null;

		/// <summary>
		/// ログインする
		/// </summary>
		/// <param name="browserType"></param>
		/// <param name="cookieFilePath">クッキーが保存されているファイル、nullの場合既定のファイルを対称にする</param>
		/// <returns>失敗した場合はnullが返される</returns>
		public static AccountInfomation Login(CookieGetter.BROWSER_TYPE browserType, string cookieFilePath)
		{
			ICookieGetter cookieGetter = CookieGetter.GetInstance(browserType);
			System.Net.Cookie[] cookies;

			if(!string.IsNullOrEmpty(cookieFilePath)){
				cookies = cookieGetter.GetCookies(new Uri("http://www.nicovideo.jp/"), "user_session", cookieFilePath);
			}else{
				cookies = cookieGetter.GetCookies(new Uri("http://www.nicovideo.jp/"), "user_session");
			}

			if (cookies == null || cookies.Length == 0) {
				Logger.Default.LogMessage(string.Format("Login not found: type-{0}", browserType.ToString()));
				return null;
			}

			List<System.Net.Cookie> cookieList = new List<System.Net.Cookie>(cookies);
			cookieList.Sort(CompareCookieExpires);

			Logger.Default.LogMessage(string.Format("Found Cookies: type-{0}, cookies-{1}", browserType.ToString(), cookieList.Count));
			foreach (System.Net.Cookie cookie in cookieList) {
				
		
				System.Net.CookieContainer container = new System.Net.CookieContainer();
				container.Add(cookie);

				AccountInfomation accountInfomation = NicoApiSharp.AccountInfomation.GetMyAccountInfomation(container);
				if (accountInfomation != null) {
					DefaultCookies = container;
					return accountInfomation;
				} 
			}

			return null;

		}

		private static int CompareCookieExpires(System.Net.Cookie a, System.Net.Cookie b) { 
			if (a == null) {
				return -1;
			}
			if (b == null) {
				return 1;
			}
			return -a.Expires.CompareTo(b.Expires);
		}

		/// <summary>
		/// メールアドレスとパスワードを使用して直接ログインする
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="pass"></param>
		/// <returns>失敗した場合はnullが返される</returns>
		public static AccountInfomation Login(string mail, string pass) {
			System.Net.CookieContainer cookies = new System.Net.CookieContainer();
			string postData = String.Format("mail={0}&password={1}", mail, pass);

			Utility.PostData(ApiSettings.Default.LoginUrl, postData, cookies, 1000);

			AccountInfomation accountInfomation = NicoApiSharp.AccountInfomation.GetMyAccountInfomation(cookies);
			if (accountInfomation != null) {
				DefaultCookies = cookies;
				return accountInfomation;
			}

			return null;
		}
	}
}
