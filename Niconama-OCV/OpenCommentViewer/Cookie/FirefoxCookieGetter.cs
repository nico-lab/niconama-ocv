using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Cookie
{

	/// <summary>
	/// Firefoxからクッキーを取得する
	/// </summary>
	class FirefoxCookieGetter : SqliteCookieGetter
	{
		const string PROFILEFOLDER = "Mozilla\\Firefox\\";
		const string PROFILEINI_NAME = "profiles.ini";
		const string DATABASEFILE_NAME = "cookies.sqlite";

		const string TEMP_SQLITE_FILE_NAME = "temp_firefox_cookies.sqlite";
		const string QUERY_FORMAT = "SELECT value FROM moz_cookies WHERE host LIKE '{0}' AND name LIKE '{1}'";

		private string _path = null;

		/// <summary>
		/// 既定のファイルからクッキーを取得する場合
		/// </summary>
		public FirefoxCookieGetter()
		{
			string profileDir = GetProfileDir();
			if (profileDir != null) {
				_path = System.IO.Path.Combine(profileDir, DATABASEFILE_NAME);
			}
		}

		/// <summary>
		/// 指定したファイルからクッキーを取得する場合
		/// </summary>
		/// <param name="path"></param>
		public FirefoxCookieGetter(string path)
		{
			_path = path;
		}

		public override string GetCookieValue(string url, string key)
		{
			Logger.Default.LogMessage("クッキー取得開始");
			Logger.Default.LogMessage(_path);
			if (_path == null || !System.IO.File.Exists(_path)) {
				Logger.Default.LogErrorMessage("パスが存在しない");
				return null;
			}

			try {

				// FireFox3.5以上からDBがロックされるようになったのでコピーしてこれを回避する
				string tempdbpath = System.IO.Path.GetFullPath(TEMP_SQLITE_FILE_NAME);
				System.IO.File.Copy(_path, tempdbpath);
				Logger.Default.LogMessage("データベースシャドウィング");
				string query = string.Format(QUERY_FORMAT, url, key);

				string res = base.getDatabaseValue(tempdbpath, query);
				Logger.Default.LogMessage(res);
				// SqliteCookieGetterに処理を投げる
				return res;

			} catch (Exception ex) {

				Logger.Default.LogException(ex);

			} finally {
				if (System.IO.File.Exists(TEMP_SQLITE_FILE_NAME)) {
					System.IO.File.Delete(TEMP_SQLITE_FILE_NAME);
				}
				Logger.Default.LogMessage("クッキー取得終了");
			}

			return null;
		}

		/// <summary>
		/// 既定のプロファイルが保存されているディレクトリーを取得する
		/// </summary>
		/// <returns></returns>
		private string GetProfileDir()
		{

			string moz_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROFILEFOLDER);
			string profile_path = System.IO.Path.Combine(moz_path, PROFILEINI_NAME);
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

			Logger.Default.LogMessage("パス取得" + path);
			return path;

		}

	}
}
