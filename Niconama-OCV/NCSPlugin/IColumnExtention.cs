using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// ビューの項目を追加するためのインターフェース
	/// </summary>
	public interface IColumnExtention : IComparer<IChat>
	{

		/// <summary>
		/// 追加されるデータグリッドビューのカラム
		/// </summary>
		DataGridViewColumn Column { get; }

		/// <summary>
		/// セルの値が必要になったときに呼び出されます
		/// 対応する文字列などを返してください
		/// なおビューアは仮想モードで動くことを前提にしてあります
		/// </summary>
		/// <param name="chat">該当するセルが含まれている行に割り当てられているチャット</param>
		/// <returns>セルに代入される値</returns>
		object OnCellValueNeeded(IChat chat);
	}
}
