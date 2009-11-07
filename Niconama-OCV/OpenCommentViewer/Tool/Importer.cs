using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Tool
{

	/// <summary>
	/// XML�`���̃R�����g�t�@�C�����C���|�[�g���邽�߂̃N���X
	/// </summary>
	class Importer : NCSPlugin.IPlugin
	{
		NCSPlugin.IPluginHost _host;
		System.Windows.Forms.ToolStripMenuItem _menuItem;

		#region �C���|�[�g����

		void menuitem_Click(object sender, EventArgs e)
		{

			System.Windows.Forms.OpenFileDialog of = new System.Windows.Forms.OpenFileDialog();
			of.DefaultExt = ".xml";
			of.Filter = "XML�t�@�C��(*.xml)|*.xml";
			if (of.ShowDialog((System.Windows.Forms.IWin32Window)_host.Win32WindowOwner) == System.Windows.Forms.DialogResult.OK) {
				try {
					System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
					xdoc.Load(of.FileName);

					// �[���I�ɕ����ɐڑ�������Ԃɂ���
					_host.StartMockLive("lv0", System.IO.Path.GetFileNameWithoutExtension(of.FileName), DateTime.Now);
					
					// �t�@�C�����̃R�����g���z�X�g�ɓo�^����
					foreach (System.Xml.XmlNode node in xdoc.SelectNodes("packet/chat")) {
						NicoApiSharp.Live.Chat chat = new Hal.NicoApiSharp.Live.Chat(node);
						_host.InsertPluginChat(chat);
					}
					_host.ShowStatusMessage("�C���|�[�g�ɐ������܂����B");

				}catch(Exception ex){
					Logger.Default.LogException(ex);
					_host.ShowStatusMessage("�C���|�[�g�Ɏ��s���܂����B");

				}
			}
			
		}

		#endregion

		#region IPlugin �����o

		public string Name
		{
			get { return "�C���|�[�g"; }
		}

		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string Description
		{
			get { return "�R�����g���C���|�[�g���܂�"; }
		}

		public void Initialize(Hal.NCSPlugin.IPluginHost host)
		{
			_host = host;

			_menuItem = new System.Windows.Forms.ToolStripMenuItem("�C���|�[�g(&I)");
			_menuItem.Click += new EventHandler(menuitem_Click);
			_host.AddMenuStripItem("�t�@�C��(&F)", _menuItem);

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
