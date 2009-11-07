using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Hal.OpenCommentViewer.Control
{
	/// <summary>
	/// チャットを表示するためのコントロール
	/// </summary>
	public partial class ChatGridView : UserControl
	{
		/// <summary>
		/// カラムを拡張するプラグインを格納する配列
		/// </summary>
		protected List<Hal.NCSPlugin.IColumnExtention> _columnExtention = new List<Hal.NCSPlugin.IColumnExtention>();

		protected List<Hal.NCSPlugin.CellFormattingCallback> _cellFormattingCallbacks = new List<Hal.NCSPlugin.CellFormattingCallback>();

		/// <summary>
		/// プラグイン追加前のカラムの量
		/// </summary>
		protected int _defaultColmunCount = 0;
		
		List<Hal.NCSPlugin.IChat> _chats = new List<Hal.NCSPlugin.IChat>();
		List<int> _widthList = new List<int>();
		int _dgHeight = 0;

		int _messageColumnWidth = 40;

		
		/// <summary>
		/// チャットビューを生成します
		/// </summary>
		public ChatGridView()
		{
			InitializeComponent();

			// プラグイン追加前のカラムの量を保存しておく
			_defaultColmunCount = dataGridView1.Columns.Count;

			// 現在のフォントの高さを取得しておく
			_dgHeight = System.Windows.Forms.TextRenderer.MeasureText("measure height", dataGridView1.Font).Height;

		}

		/// <summary>
		/// ビューの設定を読み込む
		/// </summary>
		/// <param name="settings"></param>
		public void LoadSettings(UserSettings settings)
		{
			// 同じ名前のカラムの設定を反映させる
			foreach (UserSettings.ColumnStatus cs in settings.ColumnStates) {
				if (dataGridView1.Columns.Contains(cs.Name)) {
					try {
						DataGridViewColumn col = dataGridView1.Columns[cs.Name];
						col.Width = cs.Width;
						col.Visible = cs.Visible;
						if (cs.DisplayIndex < dataGridView1.Columns.Count) {
							col.DisplayIndex = cs.DisplayIndex;
						}
					} catch (Exception ex) {
						Logger.Default.LogException(ex);
					}
				}
			}

		}

		/// <summary>
		/// ビューの設定を保存する
		/// </summary>
		/// <param name="settings"></param>
		public void SaveSettings(UserSettings settings)
		{
			foreach (DataGridViewColumn column in dataGridView1.Columns) {
				UserSettings.ColumnStatus colState = null;

				foreach (UserSettings.ColumnStatus cs in settings.ColumnStates) {
					if (cs.Name == column.Name) {
						colState = cs;
						break;
					}
				}

				if (colState == null) {
					colState = new UserSettings.ColumnStatus();
					colState.Name = column.Name;
					settings.ColumnStates.Add(colState);

				}

				colState.Visible = column.Visible;
				colState.Width = column.Width;
				colState.DisplayIndex = column.DisplayIndex;

			}
		}

		/// <summary>
		/// チャットを追加する
		/// </summary>
		/// <param name="chat"></param>
		public void Add(Hal.NCSPlugin.IChat chat)
		{
			// 現在のフォントでの文字列の長さを取得する
			_widthList.Add(System.Windows.Forms.TextRenderer.MeasureText(chat.Message, dataGridView1.Font).Width);
			
			dataGridView1.SuspendLayout();
			_chats.Add(chat);
			dataGridView1.RowCount = _chats.Count;

			if (IsAttachBottom()) {
				if (dataGridView1.RowCount != 0) {
					dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
				}
			}


			dataGridView1.ResumeLayout();
		}

		/// <summary>
		/// チャット配列を追加する
		/// </summary>
		/// <param name="chats"></param>
		public void AddRange(Hal.NCSPlugin.IChat[] chats)
		{

			dataGridView1.SuspendLayout();


			foreach (Hal.NCSPlugin.IChat c in chats) {
				_chats.Add(c);
				// 現在のフォントでの文字列の長さを取得する
				_widthList.Add(System.Windows.Forms.TextRenderer.MeasureText(c.Message, dataGridView1.Font).Width);
			}
			dataGridView1.RowCount = _chats.Count;

			if (dataGridView1.RowCount != 0) {
				dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
			}

			dataGridView1.ResumeLayout();
		}

		/// <summary>
		/// 一番下のチャットが表示されているかどうか
		/// </summary>
		/// <returns></returns>
		protected bool IsAttachBottom()
		{
			int las = dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Displayed);
			return dataGridView1.RowCount - 2 <= las;
		}

		/// <summary>
		/// カラムを追加する
		/// </summary>
		/// <param name="columnExtention"></param>
		public void AddColumnExtention(NCSPlugin.IColumnExtention columnExtention)
		{
			if (columnExtention != null && columnExtention.Column != null) {
				if (columnExtention.Column.SortMode == DataGridViewColumnSortMode.Automatic) {
					columnExtention.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
				}
				dataGridView1.Columns.Add(columnExtention.Column);
				_columnExtention.Add(columnExtention);
			}
		}

		/// <summary>
		/// カラムを追加する
		/// </summary>
		/// <param name="cfm"></param>
		public void AddCellFormattingCallback(Hal.NCSPlugin.CellFormattingCallback callback)
		{
			if (callback != null) {
				_cellFormattingCallbacks.Add(callback);
			}
		}


		/// <summary>
		/// チャットをすべて取り除く
		/// </summary>
		public void Clear()
		{
			dataGridView1.RowCount = 0;
			_chats.Clear();
			_widthList.Clear();
			
		}

		/// <summary>
		/// 現在選択されているチャットを返す
		/// </summary>
		/// <returns></returns>
		public NCSPlugin.IChat GetSelectedChat() {
			if (dataGridView1.SelectedRows.Count == 1) {
				int index = dataGridView1.SelectedRows[0].Index;
				if (0 <= index && index < _chats.Count) {
					return _chats[index];
				}
			}

			return null;
		}

		/// <summary>
		/// 指定した番号のコメントを選択状態にする
		/// </summary>
		/// <param name="no"></param>
		/// <returns></returns>
		public bool SelectChat(int no) {
			for (int i = 0; i < _chats.Count; i++) {
				if (_chats[i].No == no) {
					if (i < dataGridView1.RowCount) {
						dataGridView1.Rows[i].Selected = true;
						dataGridView1.FirstDisplayedScrollingRowIndex = i;
						return true;
					} else {
						break;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// ビューの値が必要になったときに呼び出される
		/// 
		/// メモリ使用量の削減、大量のコメントがある場合のビュー構築の時間短縮のために
		/// データグリッドビューは仮想モードで実行させる
		/// 
		/// （なぜかMONOだとこのイベントが呼び出されない。原因がわかる方は連絡お願いします）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void dataGridView1_CellValueNeeded(object sender, System.Windows.Forms.DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex < 0 || _chats.Count <= e.RowIndex) {
				e.Value = "";
				return;
			}

			Hal.NCSPlugin.IChat chat = _chats[e.RowIndex];
			switch (e.ColumnIndex) {
				case 0:
					e.Value = chat.No;
					return;
				case 1:
					e.Value = chat.Message;
					return;
				case 2:
					e.Value = chat.Mail;
					return;
				case 3:
					e.Value = chat.UserId;
					return;
				case 4:
					e.Value = chat.Date;
					return;
			}

			// 拡張カラムの値を処理する
			int index = e.ColumnIndex - _defaultColmunCount;
			if (0 <= index && index < _columnExtention.Count) {
				try {
					e.Value = _columnExtention[index].OnCellValueNeeded(chat);
				} catch (Exception ex) {
					Logger.Default.LogErrorMessage("Column extention error on " + dataGridView1.Columns[e.ColumnIndex].Name);
					Logger.Default.LogException(ex);
					e.Value = "Error";
				}
			}


		}

		/// <summary>
		/// セルのスタイルを決定する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void dataGridView1_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex < 0 || _chats.Count <= e.RowIndex) {
				return;
			}

			Hal.NCSPlugin.IChat chat = _chats[e.RowIndex];
			
			for (int i = 0; i < _cellFormattingCallbacks.Count; i++) {
				_cellFormattingCallbacks[i](chat, e);
			}
		}

		/// <summary>
		/// ツールチップテキストにチャットのメッセージを表示させる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void dataGridView1_CellToolTipTextNeeded(object sender, System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs e)
		{
			if (e.RowIndex < 0 || _chats.Count <= e.RowIndex) {
				return;
			}

			Hal.NCSPlugin.IChat chat = _chats[e.RowIndex];
			e.ToolTipText = chat.Message;
		}

		/// <summary>
		/// メッセージの高さを調整する
		/// AutoResizeを使わずに計算
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void dataGridView1_RowHeightInfoNeeded(object sender, System.Windows.Forms.DataGridViewRowHeightInfoNeededEventArgs e)
		{

			if (e.RowIndex < 0 || _widthList.Count <= e.RowIndex) {
				return;
			}

			int w = _widthList[e.RowIndex];

			if (_messageColumnWidth < w) {
				int line = w / _messageColumnWidth + 1;
				e.Height = Math.Min(_dgHeight * line + 5, 50);
			}
		}
		
		/// <summary>
		/// メッセージカラムの幅が変わった場合、折り返しの発生でビューのレイアウトが崩れてしまうので
		/// ビューを再構築する
		/// （ビュー再構築のやり方が間違っているかもしれません。）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (e.Column.Index == 1) {
				_messageColumnWidth = dataGridView1.Columns[1].Width;

				if (dataGridView1.RowCount != 0) {
					dataGridView1.SuspendLayout();
					int c = dataGridView1.RowCount;
					int d = dataGridView1.FirstDisplayedScrollingRowIndex;
					dataGridView1.RowCount = 0;

					if (c != -1) {
						dataGridView1.RowCount = c;
					}

					if (d != -1) {
						dataGridView1.FirstDisplayedScrollingRowIndex = d;
					}

					dataGridView1.ResumeLayout();
				}
			}
		}
	}
}
