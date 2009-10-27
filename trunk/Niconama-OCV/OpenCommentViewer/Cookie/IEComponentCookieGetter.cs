using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenCommentViewer.Cookie
{

	/// <summary>
	/// XP上のIEやトライデントエンジンを利用しているブラウザのクッキーを取得する
	/// </summary>
	class IEComponentCookieGetter : ICookieGetter
	{

		[DllImport("wininet.dll")]
		private extern static bool InternetGetCookie(string lpszUrl, string lpszCookieName,
		StringBuilder lpCookieData, ref uint lpdwSize);

		public string GetCookieValue(string url, string key)
		{
			try {
				string cookie = GetIECookies(url);
				string[] datas = cookie.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < datas.Length; i++) {
					string[] data = datas[i].Split('=');
					if (data[0].Equals(key)) {
						return data[1];
					}
				}
			} catch {

			}

			return null;
		}

		// IEが利用しているCookieを取ってくる
		private string GetIECookies(string url)
		{
			StringBuilder cookieData = new StringBuilder(new String(' ', 4096), 4096);
			uint size = (uint)cookieData.Length;
			InternetGetCookie(url, null, cookieData, ref size);

			// 念のため、取得して来たCookieのサイズが4096以上ではないかを調べる
			// 4096以上だった場合は、sizeを指定し直して、再度InternetGetCookieを実行する
			if (size > 4096) {
				// StringBuilderの容量をsizeまで広げる
				cookieData.Capacity = (int)size;
				// 再度IEのCookieを取得する
				InternetGetCookie(url, null, cookieData, ref size);
			}

			return cookieData.ToString();
		}


	}
}
