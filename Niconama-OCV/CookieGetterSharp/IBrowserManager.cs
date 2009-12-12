using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// クッキーが保存されているパスを検索する能力を表すインターフェース
	/// </summary>
	interface IBrowserManager
	{
		/// <summary>
		/// ブラウザの種類
		/// </summary>
		CookieGetter.BROWSER_TYPE BrowserType { get; }

		/// <summary>
		/// 既定のステータスを取得します
		/// </summary>
		/// <returns></returns>
		IBrowserStatus GetDefaultStatus();

		/// <summary>
		/// 利用可能なすべてのステータスを取得します
		/// </summary>
		/// <returns></returns>
		IBrowserStatus[] GetStatus();
		
	}
}
