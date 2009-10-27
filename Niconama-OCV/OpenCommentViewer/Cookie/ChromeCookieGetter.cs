using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Cookie
{

	/// <summary>
	/// GoogleChrome3.0からクッキーを取得する
	/// </summary>
	class ChromeCookieGetter : SqliteCookieGetter
	{

		/// <summary>
		/// GoogleChrome3.0以上
		/// </summary>
		const string DATABASE_PATH = "Google\\Chrome\\User Data\\Default\\cookies";
		const string QUERY_FORMAT = "SELECT value FROM cookies where host_key=='{0}' AND name=='{1}'";

		private string _path = null;

		/// <summary>
		/// 既定のファイルからクッキーを取得する場合
		/// </summary>
		public ChromeCookieGetter()
		{
			_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_PATH);
		}

		/// <summary>
		/// 指定したファイルからクッキーを取得する場合
		/// </summary>
		/// <param name="path"></param>
		public ChromeCookieGetter(string path)
		{
			_path = path;
		}

		public override string GetCookieValue(string url, string key)
		{

			if (!System.IO.File.Exists(_path)) {
				return null;
			}

			try {
				string query = string.Format(QUERY_FORMAT, url, key);

				// SqliteCookieGetterに処理を投げる
				return base.getDatabaseValue(_path, query);

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}


	}
}
