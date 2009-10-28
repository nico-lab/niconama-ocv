using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	class AccountInfomation : IAccountInfomation
	{

		public static AccountInfomation GetInstance(System.Net.CookieContainer cookies)
		{
			try {
				string url = ApplicationSettings.Default.GetFlvUrl;
				string res = Utility.GetResponseText(url, cookies, ApplicationSettings.Default.DefaultApiTimeout);

				if (string.IsNullOrEmpty(res)) {
					return null;
				}

				System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(res, ApplicationSettings.Default.AccountInfomationRegPattern);
				if (m.Success) {
					AccountInfomation ac = new AccountInfomation();
					ac._userId = int.Parse(m.Groups["id"].Value);
					ac._userName = System.Web.HttpUtility.UrlDecode(m.Groups["name"].Value);
					ac._isPremium = int.Parse(m.Groups["pre"].Value) == 1;
					return ac;
				}
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}

		private int _userId;
		private string _userName;
		private bool _isPremium;

		#region IAccountInfomation メンバ

		public int UserId
		{
			get { return _userId; }
		}

		public string UserName
		{
			get { return _userName; }
		}

		public bool IsPremium
		{
			get { return _isPremium; }
		}

		#endregion
	}
}
