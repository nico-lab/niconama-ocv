using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Cookie
{

	/// <summary>
	/// Vista・IEの保護モードクッキーを取得する
	/// </summary>
	class IESafeModeCookieGetter : ICookieGetter
	{
		public string GetCookieValue(string url, string key)
		{

			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			cookieFolder = System.IO.Path.Combine(cookieFolder, "low");

			//URLの最後の部分（.jpとか）を取り除く
			int l = url.LastIndexOf('.');
			string fileNameKey = "";
			if (l != -1) {
				fileNameKey = url.Substring(0, l);
			}

			if (System.IO.Directory.Exists(cookieFolder)) {

				foreach (string path in System.IO.Directory.GetFiles(cookieFolder)) {
					string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

					if (fileName != null && fileName.Contains(fileNameKey)) {
						string candidate = GetCookie(path, url, key);
						if (candidate != null) {
							return candidate;
						}
					}
				}

			}

			return null;
		}

		/// <summary>
		/// 指定されたファイルからクッキーを検索する
		/// </summary>
		/// <param name="path"></param>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private string GetCookie(string path, string url, string key)
		{

			try {
				string data = System.IO.File.ReadAllText(path, Encoding.GetEncoding("Shift_JIS"));
				string[] blocks = data.Split('*');
				foreach (string block in blocks) {
					string[] lines = block.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

					if (3 < lines.Length && lines[0] != null && lines[2] != null && lines[0].Equals(key) && lines[2].StartsWith(url)) {
						return lines[1];
					}

				}
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}


	}
}
