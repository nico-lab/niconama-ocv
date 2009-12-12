using System;
using System.Collections.Generic;
using System.Text;
using Hal.CookieGetterSharp;

namespace Hal.NicoApiSharp
{
	/// <summary>
	/// ���O�C�������Ǘ�����N���X
	/// </summary>
	public static class LoginManager
	{

		static internal System.Net.CookieContainer DefaultCookies = null;

		/// <summary>
		/// �N�b�L�[���L�\�ȃu���E�U���ʖ��ꗗ���擾���܂��B
		/// </summary>
		/// <returns></returns>
		public static string[] GetAvailableBrowserName() {
			List<string> results = new List<string>();

			foreach (ICookieGetter getter in CookieGetter.CreateInstances(true)) {
				results.Add(getter.CookieStatus.Name);
			}

			return results.ToArray();
		}

		/// <summary>
		/// ���O�C������
		/// </summary>
		/// <param name="browserType"></param>
		/// <param name="cookieFilePath">�N�b�L�[���ۑ�����Ă���t�@�C���Anull�̏ꍇ����̃t�@�C����Ώ̂ɂ���</param>
		/// <returns>���s�����ꍇ��null���Ԃ����</returns>
		public static AccountInfomation Login(BrowserType browserType, string cookieFilePath)
		{
			ICookieGetter cookieGetter = CookieGetter.CreateInstance(browserType);
			if (cookieGetter == null) return null;

			if(!string.IsNullOrEmpty(cookieFilePath)){
				cookieGetter.CookieStatus.CookiePath = cookieFilePath;
			}

			return Login(cookieGetter);

		}		

		/// <summary>
		/// ���O�C������
		/// </summary>
		/// <param name="browserName">GetAvailableBrowserName�Ŏ擾�����u���E�U���̂ǂꂩ</param>
		/// <returns>���s�����ꍇ��null���Ԃ����</returns>
		public static AccountInfomation Login(string browserName) {
			foreach (ICookieGetter getter in CookieGetter.CreateInstances(true)) {
				if (getter.CookieStatus.Name.Equals(browserName)) { 
					return Login(getter);
				}
			}
			return null;
		}

		private static AccountInfomation Login(ICookieGetter cookieGetter)
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
		/// ���[���A�h���X�ƃp�X���[�h���g�p���Ē��ڃ��O�C������
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="pass"></param>
		/// <returns>���s�����ꍇ��null���Ԃ����</returns>
		public static AccountInfomation Login(string mail, string pass)
		{
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
