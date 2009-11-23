using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// IEやトライデントエンジンを利用しているブラウザのクッキーを取得する
	/// </summary>
	class IEComponentCookieGetter : ICookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
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
		/// <returns></returns>
		public virtual System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{
			List<System.Net.Cookie> cookies = new List<System.Net.Cookie>();
			string[] files = SelectFiles(url, path);

			foreach (string filepath in files) {
				System.Net.CookieCollection collection = new System.Net.CookieCollection();
				PickCookiesFromFile(filepath, url, collection);
				if (collection[key] != null) {
					cookies.Add(collection[key]);
				}
			}

			return cookies.ToArray();
		}

		/// <summary>
		/// urlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public virtual System.Net.CookieCollection[] GetCookieCollection(Uri url)
		{
			string cookieFolder = Utility.ReplacePathSymbols(ApiSettings.Default.IECookieFolderPath);
			return GetCookieCollection(url, cookieFolder);
		}

		/// <summary>
		/// urlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public virtual System.Net.CookieCollection[] GetCookieCollection(Uri url, string path)
		{
			System.Net.CookieCollection collection = new System.Net.CookieCollection();
			string[] files = SelectFiles(url, path);

			foreach (string filepath in files) {
				PickCookiesFromFile(filepath, url, collection);
			}

			return new System.Net.CookieCollection[] { collection };
		}

		/// <summary>
		/// urlで指定されたサイトで使用されるクッキーが保存されているファイルを選択する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		private string[] SelectFiles(Uri url, string path)
		{
			List<string> results = new List<string>();
			
			// VISTA用などのLOWサブフォルダも検索範囲に含める
			Stack<string> cookieFolders = new Stack<string>(System.IO.Directory.GetDirectories(path));
			cookieFolders.Push(path);

			// クッキーのファイル名はユーザー名+トップレベルドメインを除いたホスト名+識別番号となっている
			string hostName = RemoveTopLevelDomain(url.Host);
			
			foreach (string folder in cookieFolders) {
				if (System.IO.Directory.Exists(folder)) {

					foreach (string filePath in System.IO.Directory.GetFiles(folder)) {
						string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
						string fileHostName = GetFileHostName(fileName);

						if (fileHostName != null && hostName.EndsWith(fileHostName)) {
							results.Add(filePath);
						}
					}
				}
			}

			return results.ToArray();
		}

		/// <summary>
		/// 指定されたファイルから該当するクッキーを拾い上げる
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		private void PickCookiesFromFile(string filepath, Uri url, System.Net.CookieCollection collection)
		{
			try {
				string data = System.IO.File.ReadAllText(filepath, Encoding.GetEncoding("Shift_JIS"));
				string[] blocks = data.Split('*');
				foreach (string block in blocks) {
					string[] lines = block.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

					if (7 < lines.Length) {
						System.Net.Cookie cookie = new System.Net.Cookie();
						cookie.Name = lines[0];
						cookie.Value = lines[1];
						cookie.Domain = lines[2].Split('/')[0];
						cookie.Path = lines[2].Substring(lines[2].IndexOf('/'));

						// ドメインの最初に.をつける
						if (!cookie.Domain.StartsWith("www") && !cookie.Domain.StartsWith(".")) {
							cookie.Domain = '.' + cookie.Domain;
						}
						
						// 有効期限を取得する
						int uexp = 0, lexp = 0;
						if (int.TryParse(lines[4], out lexp) && int.TryParse(lines[5], out uexp)) {
							cookie.Expires = FileTimeToDateTime(lexp, uexp);	
						}

						// 同じものがあった場合は有効期限が先のものを優先する
						if (collection[cookie.Name] == null || collection[cookie.Name].Expires < cookie.Expires) {
							collection.Add(cookie);
						}
					}

				}
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

		}

		/// <summary>
		/// ホスト名からトップレベルドメインを取り除く
		/// </summary>
		/// <param name="host"></param>
		/// <returns></returns>
		private string RemoveTopLevelDomain(string host) {
			List<string> hosts = new List<string>(host.Split('.'));
			if (hosts.Count != 1) {
				hosts.RemoveAt(hosts.Count - 1);
			}
			return string.Join(".", hosts.ToArray());

		}

		/// <summary>
		/// ファイル名からホスト名を取得する
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private string GetFileHostName(string fileName) { 
			int start = fileName.IndexOf('@')+1;
			int end = fileName.LastIndexOf('[');
			if (start < end) {
				return fileName.Substring(start, end - start);
			} else {
				return null;
			}
		}

		/// <summary>
		/// ファイルタイムを日付に直す
		/// http://wisdom.sakura.ne.jp/system/winapi/win32/win112.html
		/// </summary>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <returns></returns>
		private DateTime FileTimeToDateTime(int low, int high) {
			long ticks = ((long)high << 32) + low;
			return new DateTime(ticks).AddYears(1600);
		}

	}
}

/*

 * クッキーファイルの中身
 * クッキーは * で区切られている
 * 上から名前、値、URL、？、有効期限１、有効期限2、生成日１、生成日２となっている
 * 日付はWindows32APIのFiletime
 * クッキーの名前はユーザー名＠トップドメインを除いたホスト名[識別番号].txt

user_session
user_session_460838_-------------------
nicovideo.jp/
1536
423150080
30049020
4054468736
30042984
*
__utma
---------.---------.---------.---------.---------.1
nicovideo.jp/
1600
2350186496
32111674
2683696384
30043046
*
__utmz
8292653.--------.1.1.utmccn=(direct)|utmcsr=(direct)|utmcmd=(none)
nicovideo.jp/
1600
1543438336
30079759
2684186384
30043046
*

 
 
 */