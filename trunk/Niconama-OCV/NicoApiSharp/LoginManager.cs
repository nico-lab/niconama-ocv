using System;
using System.Collections.Generic;
using System.Text;
using Hal.CookieGetterSharp;

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
		public static AccountInfomation Login(BrowserType browserType, string cookieFilePath)
		{
			ICookieGetter cookieGetter = CookieGetter.CreateInstance(browserType);
			if (cookieGetter == null) return null;

			if(!string.IsNullOrEmpty(cookieFilePath)){
				cookieGetter.Status.CookiePath = cookieFilePath;
			}

			return Login(cookieGetter);

		}		

		/// <summary>
		/// ログインする
		/// </summary>
		/// <param name="cookieGetter"></param>
		/// <returns></returns>
		public static AccountInfomation Login(ICookieGetter cookieGetter)
		{
			System.Net.Cookie cookie = cookieGetter.GetCookie(new Uri("http://www.nicovideo.jp/"), "user_session");
			if (cookie == null) {
				Logger.Default.LogMessage(string.Format("Login failed, cookie dosen't found"));
				return null;
			}

			System.Net.CookieContainer container = new System.Net.CookieContainer();
			container.Add(cookie);

			AccountInfomation accountInfomation = NicoApiSharp.AccountInfomation.GetMyAccountInfomation(container);
			if (accountInfomation != null) {
				DefaultCookies = container;
				return accountInfomation;
			}

			return null;
		}

		/// <summary>
		/// メールアドレスとパスワードを使用して直接ログインする
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="pass"></param>
		/// <returns>失敗した場合はnullが返される</returns>
		public static AccountInfomation Login(string mail, string pass)
		{
			System.Net.CookieContainer cookies = new System.Net.CookieContainer();
			string postData = String.Format("mail={0}&password={1}", mail, pass);

			Utility.PostData(ApiSettings.Default.LoginUrl, postData, cookies, 1000, "");

			AccountInfomation accountInfomation = NicoApiSharp.AccountInfomation.GetMyAccountInfomation(cookies);
			if (accountInfomation != null) {
				DefaultCookies = cookies;
				return accountInfomation;
			}

			return null;
		}
	}
}
