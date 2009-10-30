using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{

	/// <summary>
	/// メインフォームのインターフェース
	/// コアに対して公開される
	/// </summary>
	public interface IMainView : Hal.NCSPlugin.IPlugin
	{

		/// <summary>
		/// 放送IDを入力するボックスのテキスト
		/// </summary>
		string IdBoxText { get; set; }

		/// <summary>
		/// ステータスメッセージを表示させる
		/// </summary>
		/// <param name="message"></param>
		void ShowStatusMessage(string message);

		/// <summary>
		/// エラーメッセージを表示させる
		/// </summary>
		/// <param name="message"></param>
		void ShowFatalMessage(string message);

		/// <summary>
		/// カラム拡張を登録する
		/// </summary>
		/// <param name="columnExtention"></param>
		void RegisterColumnExtention(Hal.NCSPlugin.IColumnExtention columnExtention);

		/// <summary>
		/// コンテクストメニュー拡張を登録する
		/// </summary>
		/// <param name="contextMenuExtention"></param>
		void RegisterContextMenuExtention(Hal.NCSPlugin.IContextMenuExtention contextMenuExtention);

		/// <summary>
		/// メニューストリップ拡張を登録する
		/// </summary>
		/// <param name="menuStripExtention"></param>
		void RegisterMenuStripExtention(Hal.NCSPlugin.IMenuStripExtention menuStripExtention);

	}
}
