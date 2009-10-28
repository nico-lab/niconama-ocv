using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenCommentViewer.Control
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
			this.DialogResult = DialogResult.Cancel;
		}

		private void LoginForm_Load(object sender, EventArgs e)
		{
			switch (UserSettings.Default.BrowserType) {
				case OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.IEComponent:
					radioButton1.Checked = true;
					break;
				case OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.IESafeMode:
					radioButton2.Checked = true;
					break;
				case OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.Firefox:
					radioButton3.Checked = true;
					break;
				case OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.Chrome:
					radioButton4.Checked = true;
					break;
			}

			checkBox1.Checked = !UserSettings.Default.ShowAccountForm;

			if (!string.IsNullOrEmpty(UserSettings.Default.CookieFilePath)) {
				checkBox2.Checked = true;
				textBox1.Text = UserSettings.Default.CookieFilePath;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (radioButton1.Checked) {
				UserSettings.Default.BrowserType = OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.IEComponent;
			} else if (radioButton2.Checked) {
				UserSettings.Default.BrowserType = OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.IESafeMode;
			} else if (radioButton3.Checked) {
				UserSettings.Default.BrowserType = OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.Firefox;
			} else if (radioButton4.Checked) {
				UserSettings.Default.BrowserType = OpenCommentViewer.Cookie.CookieGetter.BROWSER_TYPE.Chrome;
			}

			UserSettings.Default.ShowAccountForm = !checkBox1.Checked;
			if (checkBox2.Checked) {
				UserSettings.Default.CookieFilePath = textBox1.Text;
			} else {
				UserSettings.Default.CookieFilePath = null;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			textBox1.Enabled = checkBox2.Checked;
			button2.Enabled = checkBox2.Checked;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				textBox1.Text = openFileDialog1.FileName;
			}
		}


	}
}
