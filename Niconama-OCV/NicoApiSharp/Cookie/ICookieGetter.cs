using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// ブラウザからクッキーを取得するためのインターフェース
	/// </summary>
	interface ICookieGetter
	{

		/// <summary>
		/// 対象URL上の名前がKeyであるクッキーを取得する
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		string[] GetCookieValues(string url, string key);

	}
}
