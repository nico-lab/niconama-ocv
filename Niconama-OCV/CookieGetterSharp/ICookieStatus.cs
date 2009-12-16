using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// PathがFileとDirectoryのどちらを示しているかを表す
	/// </summary>
	public enum PathType {
 
		/// <summary>
		/// ファイル
		/// </summary>
		File,

		/// <summary>
		/// ディレクトリ
		/// </summary>
		Directory
	}

	/// <summary>
	/// クッキーゲッターの状態を表すインターフェース
	/// </summary>
	public interface ICookieStatus
	{
		/// <summary>
		/// ブラウザの種類を取得する
		/// </summary>
		BrowserType BrowserType { get; }

		/// <summary>
		/// 識別名を取得する
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ToStringで表示される名前。nullにするとNameが表示されるようになる。
		/// </summary>
		string DisplayName { get; set; }

		/// <summary>
		/// 利用可能かどうかを取得する
		/// </summary>
		bool IsAvailable { get; }
		
		/// <summary>
		/// クッキーが保存されているフォルダを取得、設定する
		/// </summary>
		string CookiePath { get; set; }

		/// <summary>
		/// CookiePathがFileを表すのか、Directoryを表すのかを取得する
		/// </summary>
		PathType PathType { get; }

	}
}
