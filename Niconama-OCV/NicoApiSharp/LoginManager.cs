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
			string[] userSessions = CookieGetter.GetCookies("nicovideo.jp", "user_session", browserType, cookieFilePath);

			if (userSessions == null) {
				Logger.Default.LogMessage(string.Format("Login not found: type-{0}", browserType.ToString()));
				return null;
			}

			Logger.Default.LogMessage(string.Format("Login: type-{0}, cookies-{1}", browserType.ToString(), userSessions.Length));

			foreach (string session in userSessions) {

				System.Net.Cookie cuid = new System.Net.Cookie("user_session", session, "/", ".nicovideo.jp");
				System.Net.CookieContainer cookies = new System.Net.CookieContainer();
				cookies.Add(cuid);

				AccountInfomation accountInfomation = NicoApiSharp.AccountInfomation.GetMyAccountInfomation(cookies);
				if (accountInfomation != null) {
					DefaultCookies = cookies;
					return accountInfomation;
				} 
			}

			return null;

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
