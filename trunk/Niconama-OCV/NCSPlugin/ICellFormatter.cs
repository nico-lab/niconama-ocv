using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// セルのフォーマッティングの際に呼び出されるデリゲート
	/// </summary>
	/// <param name="chat"></param>
	/// <param name="e"></param>
	public delegate void CellFormattingCallback(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e);

	/// <summary>
	/// ビューのセルのデザインを変更するためのインターフェース
	/// 行に色をつけたり、セルの値を変更したりできる
	/// </summary>
	public interface ICellFormatter
	{

		/// <summary>
		/// セルのフォーマッティングが必要になったとき呼び出される
		/// </summary>
		/// <param name="chat"></param>
		/// <param name="e"></param>
		void OnCellFormatting(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e);
	}
}
