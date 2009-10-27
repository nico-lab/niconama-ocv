using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OpenCommentViewer.Control
{

	/// <summary>
	/// チャットを表示するためのコントロール
	/// </summary>
	public partial class ChatGridView : UserControl
	{

		protected List<NCSPlugin.IColumnExtention> _columnExtentions = new List<NCSPlugin.IColumnExtention>();
		List<NCSPlugin.IChat> _chats = new List<NCSPlugin.IChat>();
		List<System.Drawing.Size> _sizeList = new List<System.Drawing.Size>();
		int _messageColumnWidth = 40;

		protected int _columnIndexOffset = 0;
		public ChatGridView()
		{
			InitializeComponent();
			_columnIndexOffset = dataGridView1.Columns.Count;
			_messageColumnWidth = dataGridView1.Columns[1].Width;

		}

		public void LoadSettings(UserSettings settings)
		{
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

		public void Add(NCSPlugin.IChat chat)
		{
			dataGridView1.SuspendLayout();
			_chats.Add(chat);
			_sizeList.Add(System.Windows.Forms.TextRenderer.MeasureText(chat.Message, dataGridView1.Font));
			dataGridView1.RowCount = _chats.Count;

			if (IsAttachBottom()) {
				if (dataGridView1.RowCount != 0) {
					dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
				}
			}


			dataGridView1.ResumeLayout();
		}

		public void AddRange(NCSPlugin.IChat[] chats)
		{

			dataGridView1.SuspendLayout();


			foreach (NCSPlugin.IChat c in chats) {
				_chats.Add(c);
				_sizeList.Add(System.Windows.Forms.TextRenderer.MeasureText(c.Message, dataGridView1.Font));
			}
			dataGridView1.RowCount = _chats.Count;

			if (dataGridView1.RowCount != 0) {
				dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
			}

			dataGridView1.ResumeLayout();
		}

		protected bool IsAttachBottom()
		{
			int las = dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Displayed);
			return dataGridView1.RowCount - 2 <= las;
		}

		public void AddColumn(NCSPlugin.IColumnExtention ext) {
			if (ext != null && ext.Column != null) {
				if (ext.Column.SortMode == DataGridViewColumnSortMode.Automatic) {
					ext.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
				}
				dataGridView1.Columns.Add(ext.Column);
				_columnExtentions.Add(ext);
			}
		}

		public void Clear()
		{
			dataGridView1.RowCount = 0;
			_chats.Clear();
			_sizeList.Clear();
			
		}

		protected void dataGridView1_CellValueNeeded(object sender, System.Windows.Forms.DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex < 0 || _chats.Count <= e.RowIndex) {
				e.Value = "";
				return;
			}

			NCSPlugin.IChat chat = _chats[e.RowIndex];
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

			int index = e.ColumnIndex - _columnIndexOffset;
			if (0 <= index && index < _columnExtentions.Count) {
				try {
					e.Value = _columnExtentions[index].OnCellValueNeeded(chat);
				} catch (Exception ex) {
					Logger.Default.LogErrorMessage("Column extention error on " + _columnExtentions[index].Column.Name);
					Logger.Default.LogException(ex);
					e.Value = "Error";
				}
			}


		}

		protected void dataGridView1_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex < 0 || _chats.Count <= e.RowIndex) {
				return;
			}

			NCSPlugin.IChat chat = _chats[e.RowIndex];
			if (e.ColumnIndex == 1 && chat.IsOwnerComment) {
				e.CellStyle.ForeColor = System.Drawing.Color.OrangeRed;
			}
		}

		protected void dataGridView1_RowPrePaint(object sender, System.Windows.Forms.DataGridViewRowPrePaintEventArgs e)
		{
			//if (e.RowIndex <= 0 || _chats.Count < e.RowIndex) {
			//  return;
			//}

			//NCSPlugin.IChat chat = _chats[e.RowIndex];

			//System.Drawing.Size s = System.Windows.Forms.TextRenderer.MeasureText(chat.Message, dataGridView1.Font);
			//if (dataGridView1.Columns[1].Width < s.Width) {
			//  int line = s.Width / dataGridView1.Columns[1].Width + 1;
			//  dataGridView1.Rows[e.RowIndex].Height = s.Height * line + 5;
			//}
		}

		protected void dataGridView1_CellToolTipTextNeeded(object sender, System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs e)
		{
			if (e.RowIndex < 0 || _chats.Count <= e.RowIndex) {
				return;
			}

			NCSPlugin.IChat chat = _chats[e.RowIndex];
			e.ToolTipText = chat.Message;
		}

		protected void dataGridView1_RowHeightInfoNeeded(object sender, System.Windows.Forms.DataGridViewRowHeightInfoNeededEventArgs e)
		{

			if (e.RowIndex < 0 || _sizeList.Count <= e.RowIndex) {
				return;
			}

			System.Drawing.Size s = _sizeList[e.RowIndex];

			if (_messageColumnWidth < s.Width) {
				int line = s.Width / _messageColumnWidth + 1;
				e.Height = Math.Min(s.Height * line + 5, 50);
			}
		}

		private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			_messageColumnWidth = dataGridView1.Columns[1].Width;
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
