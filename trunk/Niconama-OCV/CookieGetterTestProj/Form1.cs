using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Hal.CookieGetterSharp;

namespace CookieGetterTestProj
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		
private void Form1_Load(object sender, EventArgs e)
{

	ICookieGetter[] status = CookieGetter.CreateInstances(true);
	comboBox1.Items.Clear();
	comboBox1.Items.AddRange(status);

	if (comboBox1.Items.Count != 0) {
		comboBox1.SelectedIndex = 0;
	}
}

private void button1_Click(object sender, EventArgs e)
{
	if (comboBox1.SelectedItem != null) {
		ICookieGetter s = comboBox1.SelectedItem as ICookieGetter;
		Uri uri;

		if (s != null && Uri.TryCreate(textBox1.Text, UriKind.Absolute, out uri)) {
			try {
				cookieBindingSource.DataSource = s.GetCookieCollection(uri);
			} catch (CookieGetterException ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}

}

		

		
	}
}