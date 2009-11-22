using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// XP上のIEやトライデントエンジンを利用しているブラウザのクッキーを取得する
	/// </summary>
	class IEComponentCookieGetter : ICookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns>1件も取得できなかった場合はnullを返す</returns>
		public virtual System.Net.Cookie[] GetCookies(Uri url, string key)
		{
			string cookieFolder = Utility.ReplacePathSymbols(ApiSettings.Default.IECookieFolderPath);
			return GetCookies(url, key, cookieFolder);
		}

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns>1件も取得できなかった場合はnullを返す</returns>
		public virtual System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{
			//string cookieFolder = path;
			//Stack<string> cookieFolders = new Stack<string>();
			//cookieFolders.Push(cookieFolder);
			//foreach (string subdir in System.IO.Directory.GetDirectories(cookieFolder)) {
			//    cookieFolders.Push(subdir);
			//}
			//List<System.Net.Cookie> results = new List<System.Net.Cookie>();

			////最初の.を取り除く
			//if (1 < url.Length && url[0] == '.') {
			//    url = url.Substring(1);
			//}

			////URLの最後の部分（.jpとか）を取り除く
			//int l = url.LastIndexOf('.');
			//string fileNameKey = "";
			//if (l != -1) {
			//    fileNameKey = url.Substring(0, l);
			//}

			//foreach (string folder in cookieFolders) {
			//    if (System.IO.Directory.Exists(folder)) {

			//        foreach (string filePath in System.IO.Directory.GetFiles(folder)) {
			//            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

			//            if (fileName != null && fileName.Contains(fileNameKey)) {

			//                System.Net.Cookie c = GetCookie(filePath, url, key);
			//                if (c != null) {
			//                    results.Add(c);
			//                }
			//            }
			//        }

			//    }
			//}

			//if (results.Count != 0) {
			//    return results.ToArray();
			//}

			//return null;
		}

		/// <summary>
		/// 指定されたファイルからクッキーを検索する
		/// </summary>
		/// <param name="path"></param>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private System.Net.Cookie GetCookie(string path, string url, string key)
		{

			try {
				string data = System.IO.File.ReadAllText(path, Encoding.GetEncoding("Shift_JIS"));
				string[] blocks = data.Split('*');
				foreach (string block in blocks) {
					string[] lines = block.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

					if (5 < lines.Length && lines[0].Equals(key) && lines[2].StartsWith(url)) {
						System.Net.Cookie cookie = new System.Net.Cookie();
						cookie.Name = lines[0];
						cookie.Value = lines[1];
						cookie.Domain = lines[2].Split('/')[0];
						if (!cookie.Domain.StartsWith("www") && !cookie.Domain.StartsWith(".")) {
							cookie.Domain = '.' + cookie.Domain;
						}
						
						cookie.Path = "/";
						long exp = 0;
						if (long.TryParse(lines[4], out exp)) {
							cookie.Expires = new DateTime(exp);
						}
						return cookie;
					}

				}
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}

	}
}
