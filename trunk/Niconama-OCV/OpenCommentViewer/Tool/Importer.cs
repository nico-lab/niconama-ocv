using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Tool
{

	/// <summary>
	/// XML形式のコメントファイルをインポートするためのクラス
	/// </summary>
	class Importer : NCSPlugin.IPlugin
	{
		NCSPlugin.IPluginHost _host;
		System.Windows.Forms.ToolStripMenuItem _menuItem;

		#region インポート処理

		void menuitem_Click(object sender, EventArgs e)
		{

			System.Windows.Forms.OpenFileDialog of = new System.Windows.Forms.OpenFileDialog();
			of.DefaultExt = ".xml";
			of.Filter = "XMLファイル(*.xml)|*.xml";
			if (of.ShowDialog((System.Windows.Forms.IWin32Window)_host.Win32WindowOwner) == System.Windows.Forms.DialogResult.OK) {
				try {
					System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
					xdoc.Load(of.FileName);

					// 擬似的に放送に接続した状態にする
					_host.StartMockLive("lv0", System.IO.Path.GetFileNameWithoutExtension(of.FileName), DateTime.Now);
					
					// ファイル内のコメントをホストに登録する
					foreach (System.Xml.XmlNode node in xdoc.SelectNodes("packet/chat")) {
						NicoApiSharp.Live.Chat chat = new Hal.NicoApiSharp.Live.Chat(node);
						_host.InsertPluginChat(chat);
					}
					_host.ShowStatusMessage("インポートに成功しました。");

				}catch(Exception ex){
					Logger.Default.LogException(ex);
					_host.ShowStatusMessage("インポートに失敗しました。");

				}
			}
			
		}

		#endregion

		#region IPlugin メンバ

		public string Name
		{
			get { return "インポート"; }
		}

		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string Description
		{
			get { return "コメントをインポートします"; }
		}

		public void Initialize(Hal.NCSPlugin.IPluginHost host)
		{
			_host = host;

			_menuItem = new System.Windows.Forms.ToolStripMenuItem("インポート(&I)");
			_menuItem.Click += new EventHandler(menuitem_Click);
			_host.AddMenuStripItem("ファイル(&F)", _menuItem);

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
