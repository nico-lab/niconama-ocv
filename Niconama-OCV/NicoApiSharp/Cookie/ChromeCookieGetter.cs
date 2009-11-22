using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// GoogleChrome3.0からクッキーを取得する
	/// </summary>
	class ChromeCookieGetter : SqliteCookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns>1件も取得できなかった場合はnullを返す</returns>
		public override System.Net.Cookie[] GetCookies(Uri url, string key)
		{
			string path = Utility.ReplacePathSymbols(ApiSettings.Default.GoogleChromeDatabasePath);
			return GetCookies(url, key, path);
		}

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns>1件も取得できなかった場合はnullを返す</returns>
		public override System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{

			if (!System.IO.File.Exists(path)) {
				return null;
			}

			try {
				string query = string.Format(ApiSettings.Default.GoogleChromeKeyQueryFormat, url, key);

				// SqliteCookieGetterに処理を投げる
				object[] data = base.getDatabaseValues(path, query);
				if (data != null && data.Length == 5) {
					System.Net.Cookie cookie = new System.Net.Cookie();
					cookie.Value = data[0] as string;
					cookie.Name = data[1] as string;
					cookie.Domain = data[2] as string;
					cookie.Path = data[3] as string;
					try {
						long exp = (long)data[4];
						cookie.Expires = new DateTime(exp);
					} catch {
						Logger.Default.LogMessage("googlechromeのexpires変換に失敗しました");					
					}

					return new System.Net.Cookie[] { cookie };
				}


			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}


	}
}
