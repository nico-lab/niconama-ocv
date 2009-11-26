using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// Firefoxからクッキーを取得する
	/// </summary>
	class FirefoxCookieGetter : ICookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public System.Net.Cookie[] GetCookies(Uri url, string key)
		{

			//すべてのプロフィールから取得する
			List<System.Net.Cookie> cookies = new List<System.Net.Cookie>();
			foreach (string dir in GetProfileDirs()) {
				string path = System.IO.Path.Combine(dir, ApiSettings.Default.FirefoxDatabaseName);
				System.Net.Cookie[] c = GetCookies(url, key, path);
				cookies.AddRange(c);
			}
			
			return cookies.ToArray();
		}

		/// <summary>
		/// クッキーの保存先を指定して取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{
			if (path != null && !System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return new System.Net.Cookie[0];
			}

			Logger.Default.LogMessage("Firefoxパス指定取得 " + path);
			string tempdbpath = ApiSettings.Default.FirefoxTempSqliteFileName;
			List<System.Net.Cookie> list = new List<System.Net.Cookie>();
			try {

				// FireFox3.5以上からDBがロックされるようになったのでコピーしてこれを回避する
				if (System.IO.File.Exists(tempdbpath)) {
					System.IO.File.Delete(tempdbpath);
				}

				System.IO.File.Copy(path, tempdbpath);
				string query = MakeUrlKeyQueryString(url, key);

				object[][] datas = SqliteManager.GetDatabaseValues(tempdbpath, query);
				
				foreach (object[] data in datas) {
					System.Net.Cookie cookie = DataToCookie(data);
					list.Add(cookie);
				}

				Logger.Default.LogMessage("Firefox取得成功");
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			} finally {
				if (System.IO.File.Exists(tempdbpath)) {
					System.IO.File.Delete(tempdbpath);
				}
			}

			return list.ToArray();
		}

		/// <summary>
		/// 既定のプロファイルが保存されているディレクトリーを取得する
		/// </summary>
		/// <returns></returns>
		private string GetProfileDir()
		{
			string moz_path = Utility.ReplacePathSymbols(ApiSettings.Default.FirefoxDataFolder);
			string profile_path = System.IO.Path.Combine(moz_path, ApiSettings.Default.FirefoxProfilesIniFileName);

			string path = null;

			if (System.IO.File.Exists(profile_path)) {
				using (System.IO.StreamReader sr = new System.IO.StreamReader(profile_path)) {
					bool isRelative = false;

					while (!sr.EndOfStream) {
						string line = sr.ReadLine();

						if (line.StartsWith("IsRelative")) {
							isRelative = (line.Equals("IsRelative=1"));
						}

						if (line.StartsWith("Path")) {
							path = line.Substring(5).Replace('/', '\\');
						}

						if (line.StartsWith("Default=1")) {
							break;
						}
					}

					if (path != null && isRelative) {
						path = System.IO.Path.Combine(moz_path, path);
					}
				}

			}

			return path;

		}

		/// <summary>
		/// Firefoxのプロフィールフォルダ内のフォルダをすべて取得する
		/// </summary>
		/// <returns></returns>
		private string[] GetProfileDirs()
		{

			string profiles = Utility.ReplacePathSymbols(ApiSettings.Default.FirefoxProfieFolders);
			return System.IO.Directory.GetDirectories(profiles);

		}



		public System.Net.CookieCollection[] GetCookieCollection(Uri url)
		{
			List<System.Net.CookieCollection> collectionList = new List<System.Net.CookieCollection>();

			foreach (string dir in GetProfileDirs()) {
				string path = System.IO.Path.Combine(dir, ApiSettings.Default.FirefoxDatabaseName);
				collectionList.AddRange(GetCookieCollection(url, path));
			}

			return collectionList.ToArray();
		}

		public System.Net.CookieCollection[] GetCookieCollection(Uri url, string path)
		{
			string tempdbpath = ApiSettings.Default.FirefoxTempSqliteFileName;

			if (path != null && !System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return new System.Net.CookieCollection[0];
			}

			try {


				// FireFox3.5以上からDBがロックされるようになったのでコピーしてこれを回避する
				System.IO.File.Copy(path, tempdbpath);
				string query = MakeQueryString(url);

				// SqliteCookieGetterに処理を投げる
				object[][] datas = SqliteManager.GetDatabaseValues(tempdbpath, query);
				System.Net.CookieCollection collection = new System.Net.CookieCollection();
				foreach (object[] data in datas) {
					System.Net.Cookie cookie = DataToCookie(data);
					collection.Add(cookie);
				}

				return new System.Net.CookieCollection[] { collection };
			
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			} finally {
				if (System.IO.File.Exists(tempdbpath)) {
					System.IO.File.Delete(tempdbpath);
				}
			}


			return new System.Net.CookieCollection[0];
		}

		private System.Net.Cookie DataToCookie(object[] data)
		{
			System.Net.Cookie cookie = new System.Net.Cookie();
			cookie.Value = data[0] as string;
			cookie.Name = data[1] as string;
			cookie.Domain = data[2] as string;
			cookie.Path = data[3] as string;

			try {
				long exp = (long)data[4];
				cookie.Expires = Utility.UnixTimeToDateTime((int)exp);
			} catch {
				Logger.Default.LogMessage("firefoxのexpires変換に失敗しました");
			}
			
			return cookie;
		}

		private string MakeQueryString(Uri url)
		{
			Stack<string> hostStack = new Stack<string>(url.Host.Split('.'));
			StringBuilder hostBuilder = new StringBuilder('.' + hostStack.Pop());
			string[] pathes = url.Segments;

			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT value, name, host, path, expiry FROM moz_cookies WHERE");
			bool needOr = false;
			while (hostStack.Count != 0) {
				if (needOr) {
					sb.Append(" OR");
				}
				
				if (hostStack.Count != 1) {
					hostBuilder.Insert(0, '.' + hostStack.Pop());
					sb.AppendFormat(" host = \"{0}\"", hostBuilder.ToString());
				} else {
					hostBuilder.Insert(0, '%' + hostStack.Pop());
					sb.AppendFormat(" host LIKE \"{0}\"", hostBuilder.ToString());
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
			sb.Append("SELECT value, name, host, path, expiry FROM moz_cookies WHERE name = \"");
			sb.Append(key);
			sb.Append("\" AND (");
			bool needOr = false;
			while (hostStack.Count != 0) {
				if (needOr) {
					sb.Append(" OR");
				}

				if (hostStack.Count != 1) {
					hostBuilder.Insert(0, '.' + hostStack.Pop());
					sb.AppendFormat(" host = \"{0}\"", hostBuilder.ToString());
				} else {
					hostBuilder.Insert(0, '%' + hostStack.Pop());
					sb.AppendFormat(" host LIKE \"{0}\"", hostBuilder.ToString());
				}

				needOr = true;
			}

			sb.Append(')');
			return sb.ToString();

		}
	}
}
