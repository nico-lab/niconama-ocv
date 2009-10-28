using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OpenCommentViewer.Control
{
	/// <summary>
	/// ビューのカラムに00：00形式の時間を追加するためのクラス
	/// </summary>
	class VposColumnExtention : NCSPlugin.IColumnExtention
	{

		DataGridViewColumn _column = null;

		public VposColumnExtention()
		{
			_column = new DataGridViewTextBoxColumn();
			_column.Name = "vposColumn";
			_column.HeaderText = "Time";
			_column.Width = 50;
			_column.ReadOnly = true;
		}

		#region IColumnExtention メンバ

		public System.Windows.Forms.DataGridViewColumn Column
		{
			get { return _column; }
		}

		public object OnCellValueNeeded(NCSPlugin.IChat chat)
		{
			return VposToTimeString(chat.Vpos);
		}

		public void OnCellFormatting(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
		{
		}

		#endregion

		#region IComparer<IChat> メンバ

		public int Compare(NCSPlugin.IChat x, NCSPlugin.IChat y)
		{
			return x.Vpos - y.Vpos;
		}

		#endregion

		private static string VposToTimeString(int vpos)
		{
			int m = vpos / 6000;
			int s = (vpos % 6000) / 100;

			return m.ToString("00") + ":" + s.ToString("00");
		}
	}
}
