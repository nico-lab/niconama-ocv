using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// Firefoxからクッキーを取得する
	/// </summary>
	class FirefoxCookieGetter : SqlCookieGetter
	{
		const string SELECT_QUERY = "SELECT value, name, host, path, expiry FROM moz_cookies";

		/// <summary>
		/// Firefox系のクッキーゲッターを生成する
		/// </summary>
		/// <param name="type">Firefoxのバージョンを指定する</param>
		/// <returns></returns>
		public static new ICookieGetter GetInstance(Cookie.CookieGetter.BROWSER_TYPE type) {
			switch (type) { 
				case CookieGetter.BROWSER_TYPE.Firefox3:
					return new FirefoxCookieGetter();
			}

			return null;
		}

		private FirefoxCookieGetter() { 
		
		}

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public override System.Net.Cookie[] GetCookies(Uri url, string key)
		{

			//すべてのプロフィールから取得する
			List<System.Net.Cookie> cookies = new List<System.Net.Cookie>();
			foreach (string dir in GetProfileDirs()) {
				string path = System.IO.Path.Combine(dir, ApiSettings.Default.Firefox3DatabaseName);
				System.Net.Cookie c = base.GetCookie(url, key, path);
				if (c != null) {
					cookies.Add(c);
				}
			}
			
			return cookies.ToArray();
		}

		public System.Net.CookieCollection[] GetCookieCollection(Uri url)
		{
			List<System.Net.CookieCollection> collectionList = new List<System.Net.CookieCollection>();
			foreach (string dir in GetProfileDirs()) {
				string path = System.IO.Path.Combine(dir, ApiSettings.Default.Firefox3DatabaseName);
				collectionList.Add(base.GetCookieCollection(url, path));
			}

			return collectionList.ToArray();
		}

		public override System.Net.CookieContainer[] GetAllCookies()
		{
			List<System.Net.CookieContainer> containerList = new List<System.Net.CookieContainer>();
			foreach (string dir in GetProfileDirs()) {
				string path = System.IO.Path.Combine(dir, ApiSettings.Default.Firefox3DatabaseName);
				containerList.Add(base.GetAllCookies(path));
			}

			return containerList.ToArray();
		}

		/// <summary>
		/// クッキーの保存先を指定して取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public override System.Net.CookieContainer GetAllCookies(string path)
		{
			if (path != null && !System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return new System.Net.CookieContainer();
			}

			Logger.Default.LogMessage("Firefoxパス指定取得 " + path);
			string tempdbpath = ApiSettings.Default.Firefox3TempSqliteFileName;
			try {

				// FireFox3.5以上からDBがロックされるようになったのでコピーしてこれを回避する
				if (System.IO.File.Exists(tempdbpath)) {
					System.IO.File.Delete(tempdbpath);
				}

				System.IO.File.Copy(path, tempdbpath);

				return base.GetAllCookies(tempdbpath);

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			} finally {
				if (System.IO.File.Exists(tempdbpath)) {
					System.IO.File.Delete(tempdbpath);
				}
			}

			return new System.Net.CookieContainer();
		}

		/// <summary>
		/// 既定のプロファイルが保存されているディレクトリーを取得する
		/// </summary>
		/// <returns></returns>
		private string GetProfileDir()
		{
			string moz_path = Utility.ReplacePathSymbols(ApiSettings.Default.Firefox3DataFolder);
			string profile_path = System.IO.Path.Combine(moz_path, ApiSettings.Default.Firefox3ProfilesIniFileName);

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

			string profiles = Utility.ReplacePathSymbols(ApiSettings.Default.Firefox3ProfieFolders);
			return System.IO.Directory.GetDirectories(profiles);

		}

		protected override System.Net.Cookie DataToCookie(object[] data)
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

		protected override string MakeQuery(Uri url)
		{
			Stack<string> hostStack = new Stack<string>(url.Host.Split('.'));
			StringBuilder hostBuilder = new StringBuilder('.' + hostStack.Pop());
			string[] pathes = url.Segments;

			StringBuilder sb = new StringBuilder();
			sb.Append(SELECT_QUERY);
			sb.Append(" WHERE (");

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

		protected override string MakeQuery(Uri url, string key)
		{
			string baseQuery = MakeQuery(url);
			return string.Format("{0} AND name = \"{1}\"", baseQuery, key);

		}

	}
}
