using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace Hal.NicoApiSharp
{
	/// <summary>
	/// アカウント情報を格納するクラス
	/// </summary>
	public class AccountInfomation : IAccountInfomation
	{
		/// <summary>
		/// ログインした人のアカウント情報を取得します
		/// </summary>
		/// <param name="cookies"></param>
		/// <returns>ログインが完了していない場合はＮｕｌｌを返します</returns>
		public static AccountInfomation GetMyAccountInfomation(System.Net.CookieContainer cookies)
		{
			string page = null;

			try {
				page = Utility.GetResponseText(ApiSettings.Default.MyAccountCheckUrl, cookies, ApiSettings.Default.DefaultApiTimeout);
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
				return null;
			}

			if (page == null) {
				return null;
			}

			Match matchId = Regex.Match(page, ApiSettings.Default.MyAccountIdRegPattern, RegexOptions.Singleline);
			Match matchPreimum = Regex.Match(page, ApiSettings.Default.MyAccountPremiumRegPattern, RegexOptions.Singleline);
			Match matchName = Regex.Match(page, ApiSettings.Default.MyAccountNameRegPattern, RegexOptions.Singleline);

			if (matchId.Success && matchName.Success) {
				AccountInfomation ac = new AccountInfomation();
				ac._userId = int.Parse(matchId.Groups[1].Value);
				ac._userName = matchName.Groups[1].Value;
				ac._isPremium = matchPreimum.Success;

				return ac;
			}

			return null;
		}

		/// <summary>
		/// ログインした人のアカウント情報を取得します
		/// </summary>
		/// <returns>ログインが完了していない場合はＮｕｌｌを返します</returns>
		public static AccountInfomation GetMyAccountInfomation() {
			if (LoginManager.DefaultCookies != null) {
				return GetMyAccountInfomation(LoginManager.DefaultCookies);
			}

			return null;
		}

		/// <summary>
		/// ログインした人のアカウント情報を取得します
		/// </summary>
		/// <param name="cookieGetter"></param>
		/// <returns>ログインが完了していない場合はＮｕｌｌを返します</returns>
		public static AccountInfomation GetMyAccountInfomation(CookieGetterSharp.ICookieGetter cookieGetter)
		{
			if (cookieGetter.Status.IsAvailable) {
				try {
					System.Net.CookieContainer container = new System.Net.CookieContainer();
					container.Add(cookieGetter.GetCookieCollection(new Uri("http://live.nicovideo.jp/")));
					return GetMyAccountInfomation(container);
				} catch { 
				}
			}

			return null;
		}

		/// <summary>
		/// 指定したIDのユーザーアカウントを取得します
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="cookies"></param>
		/// <returns>取得に失敗した場合はNullを返します</returns>
		public static AccountInfomation GetUserAccountInfomation(string userId, System.Net.CookieContainer cookies)
		{
			string page = null;

			try {
			    page = Utility.GetResponseText(string.Format(ApiSettings.Default.UserPageUrlFormat, userId), cookies, ApiSettings.Default.DefaultApiTimeout);
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
				return null;
			}

			if (page == null) {
				return null;
			}

			Match matchId = Regex.Match(page, ApiSettings.Default.UserProfileIdRegPattern, RegexOptions.Singleline);
			Match matchPreimum = Regex.Match(page, ApiSettings.Default.UserProfilePremiumRegPattern, RegexOptions.Singleline);
			Match matchName = Regex.Match(page, ApiSettings.Default.UserProfileNameRegPattern, RegexOptions.Singleline);

			if (matchId.Success && matchName.Success) {
				AccountInfomation ac = new AccountInfomation();
				ac._userId = int.Parse(matchId.Groups[1].Value);
				ac._userName = matchName.Groups[1].Value;
				ac._isPremium = matchPreimum.Success;

				return ac;
			}

			return null;

		}

		/// <summary>
		/// 指定したIDのユーザーアカウントを取得します
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>取得に失敗した場合はNullを返します</returns>
		public static AccountInfomation GetUserAccountInfomation(string userId) {
			if (LoginManager.DefaultCookies != null) {
				return GetUserAccountInfomation(userId, LoginManager.DefaultCookies);
			}

			return null;
		}

		private int _userId;
		private string _userName;
		private bool _isPremium;

		#region IAccountInfomation メンバ

		/// <summary>
		/// ユーザーID
		/// </summary>
		public int UserId
		{
			get { return _userId; }
		}

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string UserName
		{
			get { return _userName; }
		}

		/// <summary>
		/// プレミアムかどうか
		/// </summary>
		public bool IsPremium
		{
			get { return _isPremium; }
		}

		#endregion
	}
}
