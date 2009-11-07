using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Tool
{

	/// <summary>
	/// XML�`���̃R�����g�t�@�C�����G�N�X�|�[�g���邽�߂̃N���X
	/// </summary>
	class Exporter : NCSPlugin.IPlugin
	{
		NCSPlugin.IPluginHost _host;
		System.Windows.Forms.ToolStripMenuItem _menuItem;

		#region �G�N�X�|�[�g����

		void menuitem_Click(object sender, EventArgs e)
		{
			if (_host.Chats.Length != 0) {
				System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
				sf.DefaultExt = ".xml";
				sf.Filter = "XML�t�@�C��(*.xml)|*.xml";
				sf.FileName = "export_" + _host.LiveId + ".xml";
				if (sf.ShowDialog((System.Windows.Forms.IWin32Window)_host.Win32WindowOwner) == System.Windows.Forms.DialogResult.OK) {
					string xml = buildNicoXML();
					if (xml != null) {
						try {
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sf.FileName)) {
								sw.WriteLine(xml);
								sw.Close();
							}

							_host.ShowStatusMessage("�G�N�X�|�[�g�ɐ������܂����B");

						} catch (Exception ex) {
							Logger.Default.LogException(ex);
							_host.ShowFaitalMessage("�G�N�X�|�[�g�Ɏ��s���܂����B�ڂ������̓��O�t�@�C�����Q�Ƃ��Ă�������");
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
		/// �X�g�����O�r���_�[�ɑ΂���XML��Attribute�������ǉ�����
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

		#region IPlugin �����o

		public string Name
		{
			get { return "�G�N�X�|�[�g"; }
		}

		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string Description
		{
			get { return "�R�����g���G�N�X�|�[�g���܂�"; }
		}

		public void Initialize(Hal.NCSPlugin.IPluginHost host)
		{
			_host = host;

			_menuItem = new System.Windows.Forms.ToolStripMenuItem("�G�N�X�|�[�g(&E)");
			_menuItem.Click += new EventHandler(menuitem_Click);
			_host.AddMenuStripItem("�t�@�C��(&F)", _menuItem, contextemenuOpeningCallback);

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

		#region IDisposable �����o

		public void Dispose()
		{
			_host = null;
		}

		#endregion
	}
}
