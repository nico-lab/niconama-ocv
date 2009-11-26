using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// GoogleChrome3.0からクッキーを取得する
	/// </summary>
	class ChromeCookieGetter : ICookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public System.Net.Cookie[] GetCookies(Uri url, string key)
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
		/// <returns></returns>
		public System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{

			if (!System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return new System.Net.Cookie[0];
			}

			try {
				string query = MakeUrlKeyQueryString(url, key);

				// SqliteCookieGetterに処理を投げる
				object[][] datas = SqliteManager.GetDatabaseValues(path, query);
				foreach (object[] data in datas) {
					
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

			return new System.Net.Cookie[0];

		}

		public System.Net.CookieCollection[] GetCookieCollection(Uri url)
		{
			string path = Utility.ReplacePathSymbols(ApiSettings.Default.GoogleChromeDatabasePath);
			return GetCookieCollection(url, path);
		}

		public System.Net.CookieCollection[] GetCookieCollection(Uri url, string path)
		{

			if (path != null && !System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return new System.Net.CookieCollection[0];
			}

			try {

				string query = MakeUrlQueryString(url);

				// SqliteCookieGetterに処理を投げる
				object[][] datas = SqliteManager.GetDatabaseValues(path, query);
				System.Net.CookieCollection collection = new System.Net.CookieCollection();
				foreach (object[] data in datas) {

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

					collection.Add(cookie);
				}

				return new System.Net.CookieCollection[] { collection };
			

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}


			return new System.Net.CookieCollection[0];
		}

		private string MakeUrlQueryString(Uri url) 
		{ 
			Stack<string> hostStack = new Stack<string>(url.Host.Split('.'));
			StringBuilder hostBuilder = new StringBuilder('.' + hostStack.Pop());
			string[] pathes = url.Segments;

			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT value, name, host_key, path, expires_utc FROM cookies WHERE");
			bool needOr = false;
			while (hostStack.Count != 0) {
				if (needOr) {
					sb.Append(" OR");
				}

				if (hostStack.Count != 1) {
					hostBuilder.Insert(0, '.' + hostStack.Pop());
					sb.AppendFormat(" host_key = \"{0}\"", hostBuilder.ToString());
				} else {
					hostBuilder.Insert(0, '%' + hostStack.Pop());
					sb.AppendFormat(" host_key LIKE \"{0}\"", hostBuilder.ToString());
				}

				needOr = true;
			}

			return sb.ToString();
		
		}

		private string MakeUrlKeyQueryString(Uri url, string key)
		{
			Stack<string> hostStack = new Stack<string>(url.Host.Split('.'));
			StringBuilder hostBuilder = new StringBuilder('.' + hostStack.Pop());
			string[] pathes = url.Segments;

			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT value, name, host_key, path, expires_utc FROM cookies WHERE name = \"");
			sb.Append(key);
			sb.Append("\" AND (");
			bool needOr = false;
			while (hostStack.Count != 0) {
				if (needOr) {
					sb.Append(" OR");
				}

				if (hostStack.Count != 1) {
					hostBuilder.Insert(0, '.' + hostStack.Pop());
					sb.AppendFormat(" host_key = \"{0}\"", hostBuilder.ToString());
				} else {
					hostBuilder.Insert(0, '%' + hostStack.Pop());
					sb.AppendFormat(" host_key LIKE \"{0}\"", hostBuilder.ToString());
				}

				needOr = true;
			}

			sb.Append(')');
			return sb.ToString();

		}
	}
}
