using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hal.OpenCommentViewer.Control
{
	public partial class MainForm : Form, IMainView
	{

		Control.ICore _core = null;

		public MainForm()
		{
			InitializeComponent();
			idBox.TextBox.ContextMenuStrip = idBoxContextMenu;
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

		public void Initialize(Hal.NCSPlugin.IPluginHost host)
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

		public void OnComment(Hal.NCSPlugin.IChat chat)
		{
			chatGridView1.Add((NicoApiSharp.Live.Chat)chat);

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

		public void RegisterCellFormattingCallback(Hal.NCSPlugin.CellFormattingCallback callback)
		{
			chatGridView1.AddCellFormattingCallback(callback);
		}

		public void RegisterColumnExtention(Hal.NCSPlugin.IColumnExtention columnExtention)
		{
			this.chatGridView1.AddColumnExtention(columnExtention);
		}

		public void RegisterContextMenuExtention(System.Windows.Forms.ToolStripMenuItem menuItem)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RegisterMenuStripExtention(string category, System.Windows.Forms.ToolStripMenuItem menuItem)
		{
			ToolStripMenuItem target = null;
			foreach (ToolStripMenuItem item in menuStrip1.Items) {
				if (item.Text.Equals(category)) {
					target = item;
					break;
				}
			}

			if(target == null){
				target = new ToolStripMenuItem(category);
				menuStrip1.Items.Add(target);
			}
			
			target.DropDownItems.Add(menuItem);
		}


		

		#endregion
		
		private void StartLive()
		{
			lock (this) {
				string liveId = Utility.GetLiveIdFromUrl(idBox.Text);
				if (liveId != null) {
					if (_core.ConnectLive(liveId)) {
						startButton.Enabled = false;
					}
				}
			}
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			StartLive();
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

		private void idBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				StartLive();
			}
		}

		private void cutIdToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (idBox.SelectionLength != 0) { 
				Utility.SetTxetToClipboard(idBox.SelectedText);
				int selectedStart = idBox.SelectionStart;
				idBox.Text = idBox.Text.Remove(selectedStart, idBox.SelectionLength);

				idBox.SelectionStart = selectedStart;
			}
		}

		private void copyIdToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (idBox.SelectionLength != 0) {
				Utility.SetTxetToClipboard(idBox.SelectedText);
			}
		}

		private void copyIdAsIdToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string id = Utility.GetLiveIdFromUrl(idBox.Text);
			Utility.SetTxetToClipboard(id);
		}

		private void copyIdAsURLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string id = Utility.GetLiveIdFromUrl(idBox.Text);
			Utility.SetTxetToClipboard(string.Format(ApiSettings.Default.LiveWatchUrlFormat, id));
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Clipboard.ContainsText()){
				int selectedStart = idBox.SelectionStart;
				string txt = idBox.Text.Remove(selectedStart, idBox.SelectionLength);
				string pasteTxt = Clipboard.GetText();
				txt = txt.Insert(selectedStart, pasteTxt);
				idBox.Text = txt;

				idBox.SelectionStart = selectedStart + pasteTxt.Length;
			}
		}

		private void pasteAndStartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText()) {
				idBox.Text = Clipboard.GetText();
				StartLive();
			}
		}

		

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			idBox.SelectionStart = 0;
			idBox.SelectionLength = idBox.Text.Length;
		}

		private void idBoxContextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool st = (idBox.SelectionLength != 0);
			bool ct = Utility.GetLiveIdFromUrl(idBox.Text) != null;
			bool pt = Clipboard.ContainsText();
			bool ptag = (pt && Utility.GetLiveIdFromUrl(Clipboard.GetText()) != null);

			cutIdToolStripMenuItem.Enabled = st;
			copyIdToolStripMenuItem.Enabled = st;

			copyIdAsIdToolStripMenuItem.Enabled = ct;
			copyIdAsURLToolStripMenuItem.Enabled = ct;

			pasteToolStripMenuItem.Enabled = pt;
			pasteAndStartToolStripMenuItem.Enabled = ptag;
		}

	}
}
