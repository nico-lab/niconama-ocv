using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hal.OpenCommentViewer.Control
{

	/// <summary>
	/// ログイン画面を構成するクラス
	/// </summary>
	public partial class LoginForm : Form
	{
		/// <summary>
		/// ログインフォームを生成します
		/// </summary>
		public LoginForm()
		{
			InitializeComponent();
			this.DialogResult = DialogResult.Cancel;

			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(Hal.NicoApiSharp.LoginManager.GetAvailableBrowserName());

			if (comboBox1.Items.Count != 0) {
				comboBox1.SelectedIndex = 0;
			}
		}

		private void LoginForm_Load(object sender, EventArgs e)
		{
			foreach (string name in comboBox1.Items) {
				if (name.Equals(UserSettings.Default.ShareBrowserName)) {
					comboBox1.SelectedItem = name;
					break;
				}
			}

			switch (UserSettings.Default.BrowserType) {
				case Hal.CookieGetterSharp.BrowserType.IE:
					radioButton1.Checked = true;
					break;
				case Hal.CookieGetterSharp.BrowserType.Opera10:
					radioButton2.Checked = true;
					break;
				case Hal.CookieGetterSharp.BrowserType.Firefox3:
					radioButton3.Checked = true;
					break;
				case Hal.CookieGetterSharp.BrowserType.GoogleChrome3:
					radioButton4.Checked = true;
					break;
			}
			
			if (!string.IsNullOrEmpty(UserSettings.Default.CookieFilePath)) {
				checkBox2.Checked = true;
				textBox1.Text = UserSettings.Default.CookieFilePath;
			}

			radioButton5.Checked = UserSettings.Default.LoginMode == LoginMode.AvailableBrowser;
			comboBox1.Enabled = UserSettings.Default.LoginMode == LoginMode.AvailableBrowser;

			radioButton6.Checked = UserSettings.Default.LoginMode == LoginMode.Custom;
			groupBox1.Enabled = UserSettings.Default.LoginMode == LoginMode.Custom;
			

			checkBox1.Checked = !UserSettings.Default.ShowAccountForm;

			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (radioButton5.Checked) {
				UserSettings.Default.LoginMode = LoginMode.AvailableBrowser;
				string name = comboBox1.SelectedItem as string;
				if (name != null) {
					UserSettings.Default.ShareBrowserName = name;
					this.DialogResult = DialogResult.OK;
				} else {
					this.DialogResult = DialogResult.Cancel;					
				}

			} else if (radioButton6.Checked) {
				UserSettings.Default.LoginMode = LoginMode.Custom;
				this.DialogResult = DialogResult.OK;

				if (radioButton1.Checked) {
					UserSettings.Default.BrowserType = Hal.CookieGetterSharp.BrowserType.IE;
				} else if (radioButton2.Checked) {
					UserSettings.Default.BrowserType = Hal.CookieGetterSharp.BrowserType.Opera10;
				} else if (radioButton3.Checked) {
					UserSettings.Default.BrowserType = Hal.CookieGetterSharp.BrowserType.Firefox3;
				} else if (radioButton4.Checked) {
					UserSettings.Default.BrowserType = Hal.CookieGetterSharp.BrowserType.GoogleChrome3;
				} else {
					this.DialogResult = DialogResult.Cancel;
				}

				if (checkBox2.Checked) {
					UserSettings.Default.CookieFilePath = textBox1.Text;
				} else {
					UserSettings.Default.CookieFilePath = null;
				}

			} else {
				this.DialogResult = DialogResult.Cancel;
			}

			UserSettings.Default.ShowAccountForm = !checkBox1.Checked;
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

		private void radioButton5_CheckedChanged(object sender, EventArgs e)
		{
			comboBox1.Enabled = radioButton5.Checked;
		}

		private void radioButton6_CheckedChanged(object sender, EventArgs e)
		{
			groupBox1.Enabled = radioButton6.Checked;
		}


	}
}
