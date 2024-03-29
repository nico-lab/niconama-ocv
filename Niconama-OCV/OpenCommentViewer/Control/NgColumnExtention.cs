﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Hal.OpenCommentViewer.Control
{
	class NgColumnExtention : Hal.NCSPlugin.IColumnExtention, Hal.NCSPlugin.ICellFormatter
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

		public object OnCellValueNeeded(Hal.NCSPlugin.IChat chat)
		{
			Hal.NCSPlugin.IFilterdChat f = chat as Hal.NCSPlugin.IFilterdChat;
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

		public int Compare(Hal.NCSPlugin.IChat x, Hal.NCSPlugin.IChat y)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion


		#region ICellFormatter メンバ

		public void OnCellFormatting(Hal.NCSPlugin.IChat chat, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 1) {
				NCSPlugin.IFilterdChat f = chat as NCSPlugin.IFilterdChat;
				if (f != null) {
					if (f.NgType == NCSPlugin.NGType.Word) {
						e.CellStyle.ForeColor = System.Drawing.Color.Crimson;
					} else if (f.NgType == NCSPlugin.NGType.Id) {
						e.CellStyle.ForeColor = System.Drawing.Color.Blue;
					} else if (f.NgType == NCSPlugin.NGType.Command) {
						e.CellStyle.ForeColor = System.Drawing.Color.Green;
					}
				}
			}
		}

		#endregion
	}
}
