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
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			BrowserType type;

			if (radioButton1.Checked) {
				type = BrowserType.IE;
			} else if (radioButton2.Checked) {
				type = BrowserType.Firefox3;
			} else if (radioButton3.Checked) {
				type = BrowserType.Opera10;
			} else if (radioButton4.Checked) {
				type = BrowserType.Safari4;
			} else {
				type = BrowserType.GoogleChrome3;
			}

			ICookieGetter cookieGetter = CookieGetter.CreateInstance(type);
			
			if (checkBox1.Checked) {
				cookieGetter.Status.CookiePath = textBox1.Text;
			}

			Uri uri;
			if(cookieGetter.Status.IsAvailable && Uri.TryCreate(textBox2.Text, UriKind.Absolute, out uri)){
				System.Net.Cookie cookie;
				try {
					cookie = cookieGetter.GetCookie(uri, textBox3.Text);
				} catch (Hal.CookieGetterSharp.CookieGetterException ex) {
					MessageBox.Show(ex.Message);
					label7.Text = "éÊìæé∏îs!ÉGÉâÅ[Ç™î≠ê∂ÇµÇ‹ÇµÇΩÅB";
					return;
				}

				if (cookie != null) {
					textBox4.Text = cookie.Name;
					textBox5.Text = cookie.Value;
					textBox6.Text = cookie.Domain;
					textBox7.Text = cookie.Path;
					label7.Text = "éÊìæê¨å˜";
				} else {
					label7.Text = "éÊìæé∏îs";
				}
			}
		}
		
		private void button2_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				textBox1.Text = openFileDialog1.FileName;
			}
		}
		
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			textBox1.Enabled = checkBox1.Checked;
		}

		
	}
}