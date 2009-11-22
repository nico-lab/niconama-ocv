using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// ブラウザからクッキーを取得するためのインターフェース
	/// </summary>
	public interface ICookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns>1件も取得できなかった場合はnullを返す</returns>
		System.Net.Cookie[] GetCookies(Uri url, string key);

		/// <summary>
		/// クッキーが保存されているPathを指定して対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path">1件も取得できなかった場合はnullを返す</param>
		/// <returns></returns>
		System.Net.Cookie[] GetCookies(Uri url, string key, string path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		//System.Net.CookieContainer[] GetCookies(Uri url);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		//System.Net.CookieContainer[] GetCookies(Uri url, string path);

	}
}
