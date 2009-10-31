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
		/// セルフォーマット時の外部操作を登録する
		/// </summary>
		/// <param name="callback"></param>
		void RegisterCellFormattingCallback(Hal.NCSPlugin.CellFormattingCallback callback);

		/// <summary>
		/// 拡張カラムを登録する
		/// </summary>
		/// <param name="columnExtention"></param>
		void RegisterColumnExtention(Hal.NCSPlugin.IColumnExtention columnExtention);

		/// <summary>
		/// コンテクストメニューを拡張する
		/// </summary>
		/// <param name="menuItem"></param>
		void RegisterContextMenuExtention(System.Windows.Forms.ToolStripMenuItem menuItem);

		/// <summary>
		/// メニューストリップを拡張する
		/// </summary>
		/// <param name="category"></param>
		/// <param name="menuItem"></param>
		void RegisterMenuStripExtention(string category, System.Windows.Forms.ToolStripMenuItem menuItem);

	}
}
