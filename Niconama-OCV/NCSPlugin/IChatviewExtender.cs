using System;
using System.Windows.Forms;

namespace Hal.NCSPlugin
{
	
	/// <summary>
	/// チャットビューのイベントを扱うデリゲート
	/// </summary>
	/// <param name="chat">対象の行に属するチャット</param>
	public delegate void ChatviewEventHandler(IChat chat);

	/// <summary>
	/// チャットビューのイベントを扱うデリゲート
	/// </summary>
	/// <typeparam name="T">データグリッドビューの行の動作を定義するイベント</typeparam>
	/// <param name="chat">対象の行に属するチャット</param>
	/// <param name="e">元イベントの引数</param>
	public delegate void ChatviewEventHandler<T>(IChat chat, T e);

	/// <summary>
	/// セルに値を提供するメソッドを表す
	/// </summary>
	/// <param name="chat"></param>
	/// <returns></returns>
	public delegate object CellValueProvision(IChat chat);

	/// <summary>
	/// チャットビューを拡張するためのインターフェース
	/// </summary>
	public interface IChatviewExtender
	{
		/// <summary>
		/// 行の高さを決定する必要がある際に発生するイベント
		/// </summary>
		event ChatviewEventHandler<DataGridViewRowHeightInfoNeededEventArgs> RowHeightInfoNeeded;

		/// <summary>
		/// ツールストリップテキストが必要になった際に発生するイベント
		/// </summary>
		event ChatviewEventHandler<DataGridViewCellToolTipTextNeededEventArgs> CellToolTipTextNeeded;

		/// <summary>
		/// セルをフォーマッティングする必要がある時に発生するイベント
		/// </summary>
		event ChatviewEventHandler<DataGridViewCellFormattingEventArgs> CellFormatting;

		/// <summary>
		/// 行がダブルクリックされた際に発生するイベント
		/// </summary>
		event ChatviewEventHandler<DataGridViewCellMouseEventArgs> DoubleClicked;
		
		/// <summary>
		/// カラムの幅が変わった際に発生するイベント
		/// </summary>
		event EventHandler<DataGridViewColumnEventArgs> ColumnWidthChanged;

		/// <summary>
		/// ソートされた際に発生するイベント
		/// </summary>
		event EventHandler Sorted;

		/// <summary>
		/// チャットビューに列拡張を追加します。
		/// </summary>
		/// <param name="columnExtention">追加する列拡張</param>
		void AddColumnExtention(IColumnExtention columnExtention);

		/// <summary>
		/// チャットビューに列を追加し、その動作をデリゲートにより制御します。
		/// </summary>
		/// <param name="column">追加する列</param>
		/// <param name="provision">セルの値が必要になったときに呼び出されるメソッド</param>
		/// <param name="comparison">並び替えが必要になったときに呼び出されるメソッド</param>
		void AddColumn(DataGridViewColumn column, CellValueProvision provision, Comparison<IChat> comparison);

		/// <summary>
		/// コンテクストメニューを拡張します
		/// </summary>
		/// <param name="menuItem"></param>
		/// <param name="openingCallback">コンテクストメニューが開かれた際に実行されるコールバック関数</param>
		void AddContextMenu(ToolStripMenuItem menuItem, ContextmenuOpeningCallback openingCallback);

		/// <summary>
		/// カラムエクステンションやセルフォーマッターなどで値が変更され、ビュー全体を更新する必要があるときに呼び出します
		/// </summary>
		void UpdateCellValues();
	}
}
