using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenCommentViewer.Control
{
	public partial class MainForm : Form, IMainView
	{

		Control.ICore _core = null;

		public MainForm()
		{
			InitializeComponent();
		}

		#region IPlugin メンバ


		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string Description
		{
			get { return "SimpleViewerのメインフォーム"; }
		}

		public void Initialize(NCSPlugin.IPluginHost host)
		{
			_core = host as Control.ICore;
			if (_core == null) {
				throw new ApplicationException("ホストが正しくありません。");
			}
		}

		public void Run()
		{
			if (!this.Visible) {
				this.Show();
			}

			this.Activate();
		}

		public void OnLiveStart(string liveId, DateTime startTime, int commentCount)
		{

			chatGridView1.Clear();
			chatGridView1.AddRange(_core.Chats);
			startButton.Enabled = true;

			string label = (_core.SeetType == SeetType.Arena ? "【アリーナ】" : "【立ち見】");
			idBox.Text = string.Format("{0} - {1}", liveId, label);
			this.Text = string.Format("{0}{1}", label, _core.LiveName);

		}

		public void OnLiveStop()
		{
			startButton.Enabled = true;

		}

		public void OnComment(NCSPlugin.IChat chat)
		{
			chatGridView1.Add((NicoAPI.Chat)chat);

		}

		#endregion

		#region IMainForm メンバ

		public string IdBoxText
		{
			get { return idBox.Text; }
			set { idBox.Text = value; }
		}

		public void ShowStatusMessage(string message)
		{
			if (statusLabel.ForeColor != ApplicationSettings.Default.StatusMessageColor) {
				statusLabel.ForeColor = ApplicationSettings.Default.StatusMessageColor;
			}

			statusLabel.Text = message;
		}

		public void ShowFatalMessage(string message)
		{
			if (statusLabel.ForeColor != ApplicationSettings.Default.FatalMessageColor) {
				statusLabel.ForeColor = ApplicationSettings.Default.FatalMessageColor;
			}

			statusLabel.Text = message;
		}

		public void RegisterColumnExtention(NCSPlugin.IColumnExtention columnExtention)
		{
			this.chatGridView1.AddColumn(columnExtention);
		}

		public void RegisterContextMenuExtention(NCSPlugin.IContextMenuExtention contextMenuExtention)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RegisterMenuStripExtention(NCSPlugin.IMenuStripExtention menuStripExtention)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		private void startButton_Click(object sender, EventArgs e)
		{
			string liveId = Utility.GetLiveIdFromUrl(idBox.Text);
			if (liveId != null) {
				if (_core.ConnectLive(liveId)) {
					startButton.Enabled = false;
				}
			}
		}

		private void stopButton_Click(object sender, EventArgs e)
		{
			_core.Disconnect();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			idBox.Text = UserSettings.Default.LastAccessLiveId;
			UserSettings.Default.MainformWindowState.Load(this);
			chatGridView1.LoadSettings(UserSettings.Default);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			UserSettings.Default.MainformWindowState.Save(this);
			chatGridView1.SaveSettings(UserSettings.Default);
			UserSettings.Default.Save();
		}


		private void saveTicketToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Control.LiveTicket log = _core.GetLiveTicket();
			if (log != null) {
				saveFileDialog1.InitialDirectory = ApplicationSettings.Default.LiveTicketsFolder;
				saveFileDialog1.FileName = log.LiveId + ".xml";
				if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
					Utility.Serialize(saveFileDialog1.FileName, log, typeof(Control.LiveTicket));
				}
			}
		}

		private void loadTicketToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = ApplicationSettings.Default.LiveTicketsFolder;
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				Control.LiveTicket log = Utility.Deserialize(openFileDialog1.FileName, typeof(Control.LiveTicket)) as Control.LiveTicket;
				if (log != null) {
					_core.ConnectByLiveTicket(log);
				}
			}

		}


		private void loginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LoginForm f = new LoginForm();
			if (f.ShowDialog() == DialogResult.OK) {
				_core.Login(UserSettings.Default.BrowserType, UserSettings.Default.CookieFilePath);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
#if DEBUG
			_core.CallTestMethod();
#else
menuStrip1.Visible = !menuStrip1.Visible;
statusStrip1.Visible = !statusStrip1.Visible;
#endif
		}

	}
}
