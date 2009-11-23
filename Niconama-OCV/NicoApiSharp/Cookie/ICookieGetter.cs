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
		/// <returns></returns>
		System.Net.Cookie[] GetCookies(Uri url, string key);

		/// <summary>
		/// クッキーが保存されているPathを指定して対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		System.Net.Cookie[] GetCookies(Uri url, string key, string path);

		/// <summary>
		/// urlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		System.Net.CookieCollection[] GetCookieCollection(Uri url);

		/// <summary>
		/// urlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		System.Net.CookieCollection[] GetCookieCollection(Uri url, string path);

	}
}
