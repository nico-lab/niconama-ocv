using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VirtualDatagridViewTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			dataGridView1.RowCount = 100;
		}

		private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = string.Format("{0} - {1}", e.ColumnIndex, e.RowIndex);
		}
	}
}