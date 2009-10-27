using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NCSPlugin
{

	/// <summary>
	/// ビューの項目を追加するためのインターフェース
	/// 実装は任意
	/// PluginHost側もこれを扱えるかどうかは任意
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
		/// <param name="chat"></param>
		/// <returns></returns>
		object OnCellValueNeeded(IChat chat);

		

	}
}
