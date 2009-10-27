using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenCommentViewer.CustomControl
{

  /// <summary>
  /// ダブルバッファーを有効にしてちらつきを押さえたデータグリッドビュー
  /// </summary>
	public partial class BufferedDataGridView : DataGridView
	{
		protected override bool DoubleBuffered
		{
			get
			{
				return true;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}
	}
}
