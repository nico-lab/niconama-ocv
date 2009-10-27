using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OpenCommentViewer.Control
{

	/// <summary>
	/// ビューのカラムにNGSourceを追加するためのクラス
	/// </summary>
	class NgColumnExtention : NCSPlugin.IColumnExtention
	{
		DataGridViewColumn _column = null;

		public NgColumnExtention()
		{
			_column = new DataGridViewTextBoxColumn();
			_column.Name = "ngSourceColumn";
			_column.HeaderText = "NgSource";
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
			NCSPlugin.IFilterdChat f = chat as NCSPlugin.IFilterdChat;
			if (f != null) {
				return f.NgSource;
			} else {
				return "";
			}
		}

		//public void OnCellFormatting(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
		//{
		//  NCSPlugin.IFilterdChat f = chat as NCSPlugin.IFilterdChat;
		//  if (f != null) {
		//    if (f.NgType == NCSPlugin.NGType.Word) {
		//      e.CellStyle.ForeColor = System.Drawing.Color.Crimson;
		//    }else if(f.NgType == NCSPlugin.NGType.Id){
		//      e.CellStyle.ForeColor = System.Drawing.Color.Blue;
		//    }else if(f.NgType == NCSPlugin.NGType.Command){
		//      e.CellStyle.ForeColor = System.Drawing.Color.Green;
		//    }
		//  }
		//}

		#endregion

		#region IComparer<IChat> メンバ

		public int Compare(NCSPlugin.IChat x, NCSPlugin.IChat y)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
