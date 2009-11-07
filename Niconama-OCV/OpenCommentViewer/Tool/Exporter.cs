using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Tool
{

	/// <summary>
	/// XML形式のコメントファイルをエクスポートするためのクラス
	/// </summary>
	class Exporter : NCSPlugin.IPlugin
	{
		NCSPlugin.IPluginHost _host;
		System.Windows.Forms.ToolStripMenuItem _menuItem;

		#region エクスポート処理

		void menuitem_Click(object sender, EventArgs e)
		{
			if (_host.Chats.Length != 0) {
				System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
				sf.DefaultExt = ".xml";
				sf.Filter = "XMLファイル(*.xml)|*.xml";
				sf.FileName = "export_" + _host.LiveId + ".xml";
				if (sf.ShowDialog((System.Windows.Forms.IWin32Window)_host.Win32WindowOwner) == System.Windows.Forms.DialogResult.OK) {
					string xml = buildNicoXML();
					if (xml != null) {
						try {
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sf.FileName)) {
								sw.WriteLine(xml);
								sw.Close();
							}

							_host.ShowStatusMessage("エクスポートに成功しました。");

						} catch (Exception ex) {
							Logger.Default.LogException(ex);
							_host.ShowFaitalMessage("エクスポートに失敗しました。詳しい情報はログファイルを参照してください");
						}
					}
				}
			}
		}

		void contextemenuOpeningCallback(object sender, EventArgs e)
		{
			_menuItem.Enabled = _host.Chats.Length != 0;
		}

		private string buildNicoXML()
		{
			NCSPlugin.IChat[] chats = _host.Chats;
			if (chats.Length != 0) { 
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
				sb.AppendLine("<packet>");

				sb.Append("<thread last_res=\"");
				sb.Append(chats[chats.Length - 1].No);
				sb.Append("\" resultcode=\"0\" revision=\"1\" server_time=\"");
				sb.Append(Utility.DateTimeToUnixTime(DateTime.Now));
				sb.Append("\" thread=\"");
				sb.Append(chats[0].Thread);
				sb.AppendLine("\" ticket=\"0x0000000\" />");

				foreach (NCSPlugin.IChat chat in chats) {

					sb.Append("<chat");

					addAttributeString(sb, "no", chat.No);
					addAttributeString(sb, "vpos", chat.Vpos);
					addAttributeString(sb, "mail", chat.Mail);
					addAttributeString(sb, "user_id", chat.UserId);
					addAttributeString(sb, "date", Utility.DateTimeToUnixTime(chat.Date));
					addAttributeString(sb, "thread", chat.Thread);

					if (chat.Anonymity) {
						addAttributeString(sb, "anonymity", 1);
					}

					if (chat.Premium != 0) {
						addAttributeString(sb, "premium", chat.Premium);
					}

					sb.Append('>');
					sb.Append(Utility.Sanitizing(chat.Message));
					sb.AppendLine("</chat>");
				}

				sb.AppendLine("</packet>");
				return sb.ToString();
			}

			return null;
		}

		/// <summary>
		/// ストリングビルダーに対してXMLのAttribute文字列を追加する
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		private void addAttributeString(System.Text.StringBuilder sb, string name, object value)
		{
			sb.Append(' ');
			sb.Append(name);
			sb.Append("=\"");
			sb.Append(value);
			sb.Append('"');
		}

		#endregion

		#region IPlugin メンバ

		public string Name
		{
			get { return "エクスポート"; }
		}

		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string Description
		{
			get { return "コメントをエクスポートします"; }
		}

		public void Initialize(Hal.NCSPlugin.IPluginHost host)
		{
			_host = host;

			_menuItem = new System.Windows.Forms.ToolStripMenuItem("エクスポート(&E)");
			_menuItem.Click += new EventHandler(menuitem_Click);
			_host.AddMenuStripItem("ファイル(&F)", _menuItem, contextemenuOpeningCallback);

		}

		

		public void Run()
		{
		}

		public void OnLiveStart(string liveId, DateTime startTime, int commentCount)
		{
		}

		public void OnLiveStop()
		{
		}

		public void OnComment(Hal.NCSPlugin.IChat chat)
		{
		}

		#endregion

		#region IDisposable メンバ

		public void Dispose()
		{
			_host = null;
		}

		#endregion
	}
}
