using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// Firefoxからクッキーを取得する
	/// </summary>
	class FirefoxCookieGetter : SqliteCookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns>1件も取得できなかった場合はnullを返す</returns>
		public override System.Net.Cookie[] GetCookies(Uri url, string key)
		{

			//すべてのプロフィールから取得する
			List<System.Net.Cookie> cookies = new List<System.Net.Cookie>();
			foreach (string dir in GetProfileDirs()) {
				string path = System.IO.Path.Combine(dir, ApiSettings.Default.FirefoxDatabaseName);
				Logger.Default.LogMessage("Firefox取得 " + path);
				System.Net.Cookie cookie = GetCookie(url, key, path);
				if (cookie != null) {
					Logger.Default.LogMessage("Firefox取得成功");
					cookies.Add(cookie);
				}
			}

			if (cookies.Count != 0) {
				return cookies.ToArray();
			}
			
			return null;
		}

		/// <summary>
		/// クッキーの保存先を指定して取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public override System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{
			if (path != null && !System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return null;
			}

			if (path != null) {
				//指定されたパスからクッキーを取得する
				Logger.Default.LogMessage("Firefoxパス指定取得 " + path);
				System.Net.Cookie cookie = GetCookie(url, key, path);
				if (cookie != null) {
					Logger.Default.LogMessage("Firefox取得成功");
					return new System.Net.Cookie[] { cookie };
				}
			}

			return null;
		}

		private System.Net.Cookie GetCookie(Uri url, string key, string path)
		{
			
			string tempdbpath = ApiSettings.Default.FirefoxTempSqliteFileName;
			try {

				// FireFox3.5以上からDBがロックされるようになったのでコピーしてこれを回避する
				System.IO.File.Copy(path, tempdbpath);
				string query = string.Format(ApiSettings.Default.FirefoxQueryFormat, url, key);

				// SqliteCookieGetterに処理を投げる
				object[] data = base.getDatabaseValues(tempdbpath, query);
				if (data != null && data.Length == 5) {
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
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			} finally {
				if (System.IO.File.Exists(tempdbpath)) {
					System.IO.File.Delete(tempdbpath);
				}
			}

			return null;
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



		public override System.Net.CookieCollection[] GetCookieCollection(Uri url)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override System.Net.CookieCollection[] GetCookieCollection(Uri url, string path)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
