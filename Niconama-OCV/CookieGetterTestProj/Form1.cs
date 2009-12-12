using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;


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
		
			IBrowserStatus[] status = CookieGetter.GetBrowserStatus();
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(status);

			if (comboBox1.Items.Count != 0) {
				comboBox1.SelectedIndex = 0;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedItem != null) {
				IBrowserStatus s = comboBox1.SelectedItem as IBrowserStatus;
				Uri uri;

				if (s != null) {
					try {
						uri = new Uri(textBox1.Text);
					} catch {
						return;
					}

					System.Net.CookieCollection collection = s.CookieGetter.GetCookieCollection(uri);
					cookieBindingSource.DataSource = collection;
				}
			}

			//System.Net.CookieContainer co = new CookieContainer();
			//GetResponseText("http://hal.fscs.jp/fez", co, 1000);
		}

		/// <summary>
		/// urlè„ÇÃÉyÅ[ÉWÇéÊìæÇ∑ÇÈ
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cookies"></param>
		/// <param name="defaultTimeout"></param>
		/// <returns></returns>
		static public string GetResponseText(string url, CookieContainer cookies, int defaultTimeout)
		{
			HttpWebResponse webRes = null;
			StreamReader sr = null;

			try {
				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);

				webReq.Timeout = defaultTimeout;
				webReq.CookieContainer = cookies;

				try {
					webRes = (HttpWebResponse)webReq.GetResponse();
				} catch (WebException ex) {
					
					webRes = ex.Response as HttpWebResponse;

				}

				if (webRes == null) {
					return null;
				}

				sr = new StreamReader(webRes.GetResponseStream(), System.Text.Encoding.UTF8);
				return sr.ReadToEnd();

			} finally {
				if (webRes != null)
					webRes.Close();
				if (sr != null)
					sr.Close();
			}
		}

		
	}
}