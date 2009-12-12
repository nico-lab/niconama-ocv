using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// ブラウザーから取得可能なクッキーの情報を表すインターフェース
	/// </summary>
	public interface IBrowserStatus
	{
		/// <summary>
		/// ブラウザの識別名を取得する
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// クッキーが保存されているフォルダを取得する
		/// </summary>
		string CookiePath { get; }

		/// <summary>
		/// 対応するブラウザ用のクッキーゲッターを取得する
		/// </summary>
		/// <returns></returns>
		ICookieGetter CookieGetter{ get; }

	}
}
