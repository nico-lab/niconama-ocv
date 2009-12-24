using System;
using System.Windows.Forms;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// コンテクストメニューが開かれた際に呼び出されるデリゲート
	/// </summary>
	/// <param name="chat">開かれたときに選択されていたチャット</param>
	public delegate void ContextmenuOpeningCallback(IChat chat);

	/// <summary>
	/// 外部からフォームを拡張するためのインターフェース
	/// </summary>
	public interface IFormExtender
	{

		/// <summary>
		/// メニューストリップを拡張します。
		/// </summary>
		/// <param name="category">ファイル、編集、表示などのメニューの分類を指定する。なるべく一般的なものにすること</param>
		/// <param name="menuItem"></param>
		/// <param name="openingCallback">コンテクストメニューが開かれた際に実行されるコールバック関数</param> 
		void AddMenuStripItem(string category, System.Windows.Forms.ToolStripMenuItem menuItem, ContextmenuOpeningCallback openingCallback);

		/// <summary>
		/// ツールストリップを拡張します。
		/// </summary>
		/// <param name="toolstripItem"></param>
		void AddToolStripItem(ToolStripItem toolstripItem);
		
		/// <summary>
		/// ステータスストリップを拡張します。
		/// </summary>
		/// <param name="toolstripItem"></param>
		void AddStatusStripItem(ToolStripItem toolstripItem);

		/// <summary>
		/// ユーザーコントロールを追加します
		/// </summary>
		/// <param name="userControl"></param>
		/// <param name="dockStyle"></param>
		void AddUserControl(UserControl userControl, DockStyle dockStyle);

		
	}
}
