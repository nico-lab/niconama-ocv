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
		/// 指定されたPathにあるクッキーファイルから対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		System.Net.Cookie GetCookie(Uri url, string key, string path);

		/// <summary>
		/// urlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		System.Net.CookieCollection[] GetCookieCollections(Uri url);

		/// <summary>
		/// 指定されたPathにあるクッキーファイルからurlに関連付けられたクッキーを取得します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		System.Net.CookieCollection GetCookieCollection(Uri url, string path);

		/// <summary>
		/// すべてのクッキーを取得する
		/// </summary>
		/// <returns></returns>
		System.Net.CookieContainer[] GetAllCookies();

		/// <summary>
		/// 指定されたPathにあるクッキーファイルからすべてのクッキーを取得する
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		System.Net.CookieContainer GetAllCookies(string path);

	}
}
