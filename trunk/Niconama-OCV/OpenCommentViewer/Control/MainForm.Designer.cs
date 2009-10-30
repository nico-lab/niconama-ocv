namespace OpenCommentViewer.Control {
  partial class MainForm {
    /// <summary>
    /// 必要なデザイナ変数です。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// 使用中のリソースをすべてクリーンアップします。
    /// </summary>
    /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows フォーム デザイナで生成されたコード

    /// <summary>
    /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
    /// コード エディタで変更しないでください。
    /// </summary>
    private void InitializeComponent() {
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.loadTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.idBox = new OpenCommentViewer.CustomControl.ToolStripSpringTextBox();
		this.startButton = new System.Windows.Forms.ToolStripButton();
		this.stopButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.chatGridView1 = new OpenCommentViewer.Control.ChatGridView();
		this.idBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.cutIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
		this.copyIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyIdAsIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyIdAsURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
		this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pasteAndStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
		this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStrip1.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.idBoxContextMenu.SuspendLayout();
		this.SuspendLayout();
		// 
		// statusStrip1
		// 
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
		this.statusStrip1.Location = new System.Drawing.Point(0, 311);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(490, 23);
		this.statusStrip1.TabIndex = 0;
		this.statusStrip1.Text = "statusStrip1";
		// 
		// statusLabel
		// 
		this.statusLabel.Name = "statusLabel";
		this.statusLabel.Size = new System.Drawing.Size(68, 18);
		this.statusLabel.Text = "ステータス";
		// 
		// menuStrip1
		// 
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem});
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(490, 26);
		this.menuStrip1.TabIndex = 1;
		this.menuStrip1.Text = "menuStrip1";
		// 
		// ファイルFToolStripMenuItem
		// 
		this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTicketToolStripMenuItem,
            this.loadTicketToolStripMenuItem,
            this.toolStripSeparator1,
            this.loginToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
		this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
		this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
		this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
		// 
		// saveTicketToolStripMenuItem
		// 
		this.saveTicketToolStripMenuItem.Name = "saveTicketToolStripMenuItem";
		this.saveTicketToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.saveTicketToolStripMenuItem.Text = "チケット保存";
		this.saveTicketToolStripMenuItem.Click += new System.EventHandler(this.saveTicketToolStripMenuItem_Click);
		// 
		// loadTicketToolStripMenuItem
		// 
		this.loadTicketToolStripMenuItem.Name = "loadTicketToolStripMenuItem";
		this.loadTicketToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.loadTicketToolStripMenuItem.Text = "チケット読込";
		this.loadTicketToolStripMenuItem.Click += new System.EventHandler(this.loadTicketToolStripMenuItem_Click);
		// 
		// toolStripSeparator1
		// 
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
		// 
		// loginToolStripMenuItem
		// 
		this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
		this.loginToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.loginToolStripMenuItem.Text = "ログイン";
		this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
		// 
		// toolStripSeparator2
		// 
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
		// 
		// exitToolStripMenuItem
		// 
		this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
		this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.exitToolStripMenuItem.Text = "終了";
		this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
		// 
		// toolStrip2
		// 
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.idBox,
            this.startButton,
            this.stopButton,
            this.toolStripButton1});
		this.toolStrip2.Location = new System.Drawing.Point(0, 26);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(490, 25);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		// 
		// toolStripLabel1
		// 
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(46, 22);
		this.toolStripLabel1.Text = "放送ID";
		// 
		// idBox
		// 
		this.idBox.MaxSize = 300;
		this.idBox.Name = "idBox";
		this.idBox.Size = new System.Drawing.Size(300, 25);
		this.idBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.idBox_KeyDown);
		// 
		// startButton
		// 
		this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
		this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.startButton.Name = "startButton";
		this.startButton.Size = new System.Drawing.Size(23, 22);
		this.startButton.Text = "放送に接続";
		this.startButton.Click += new System.EventHandler(this.startButton_Click);
		// 
		// stopButton
		// 
		this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
		this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.stopButton.Name = "stopButton";
		this.stopButton.Size = new System.Drawing.Size(23, 22);
		this.stopButton.Text = "放送から切断";
		this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
		// 
		// toolStripButton1
		// 
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
		this.toolStripButton1.Text = "メニューを消す";
		this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
		// 
		// openFileDialog1
		// 
		this.openFileDialog1.DefaultExt = "xml";
		this.openFileDialog1.FileName = "openFileDialog1";
		this.openFileDialog1.Filter = "チケットファイル(*.xml)|*.xml";
		// 
		// saveFileDialog1
		// 
		this.saveFileDialog1.DefaultExt = "xml";
		this.saveFileDialog1.Filter = "チケットファイル(*.xml)|*.xml";
		// 
		// chatGridView1
		// 
		this.chatGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.chatGridView1.Location = new System.Drawing.Point(0, 51);
		this.chatGridView1.Name = "chatGridView1";
		this.chatGridView1.Size = new System.Drawing.Size(490, 260);
		this.chatGridView1.TabIndex = 3;
		// 
		// idBoxContextMenu
		// 
		this.idBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutIdToolStripMenuItem,
            this.toolStripSeparator11,
            this.copyIdToolStripMenuItem,
            this.copyIdAsIdToolStripMenuItem,
            this.copyIdAsURLToolStripMenuItem,
            this.toolStripSeparator10,
            this.pasteToolStripMenuItem,
            this.pasteAndStartToolStripMenuItem,
            this.toolStripSeparator9,
            this.selectAllToolStripMenuItem});
		this.idBoxContextMenu.Name = "idBoxContextMenu";
		this.idBoxContextMenu.Size = new System.Drawing.Size(192, 176);
		this.idBoxContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.idBoxContextMenu_Opening);
		// 
		// cutIdToolStripMenuItem
		// 
		this.cutIdToolStripMenuItem.Name = "cutIdToolStripMenuItem";
		this.cutIdToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.cutIdToolStripMenuItem.Text = "切り取り(&T)";
		this.cutIdToolStripMenuItem.Click += new System.EventHandler(this.cutIdToolStripMenuItem_Click);
		// 
		// toolStripSeparator11
		// 
		this.toolStripSeparator11.Name = "toolStripSeparator11";
		this.toolStripSeparator11.Size = new System.Drawing.Size(188, 6);
		// 
		// copyIdToolStripMenuItem
		// 
		this.copyIdToolStripMenuItem.Name = "copyIdToolStripMenuItem";
		this.copyIdToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.copyIdToolStripMenuItem.Text = "コピー(&C)";
		this.copyIdToolStripMenuItem.Click += new System.EventHandler(this.copyIdToolStripMenuItem_Click);
		// 
		// copyIdAsIdToolStripMenuItem
		// 
		this.copyIdAsIdToolStripMenuItem.Name = "copyIdAsIdToolStripMenuItem";
		this.copyIdAsIdToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.copyIdAsIdToolStripMenuItem.Text = "IDをコピー(&I)";
		this.copyIdAsIdToolStripMenuItem.Click += new System.EventHandler(this.copyIdAsIdToolStripMenuItem_Click);
		// 
		// copyIdAsURLToolStripMenuItem
		// 
		this.copyIdAsURLToolStripMenuItem.Name = "copyIdAsURLToolStripMenuItem";
		this.copyIdAsURLToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.copyIdAsURLToolStripMenuItem.Text = "URLをコピー(&U)";
		this.copyIdAsURLToolStripMenuItem.Click += new System.EventHandler(this.copyIdAsURLToolStripMenuItem_Click);
		// 
		// toolStripSeparator10
		// 
		this.toolStripSeparator10.Name = "toolStripSeparator10";
		this.toolStripSeparator10.Size = new System.Drawing.Size(188, 6);
		// 
		// pasteToolStripMenuItem
		// 
		this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
		this.pasteToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.pasteToolStripMenuItem.Text = "貼り付け(&P)";
		this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
		// 
		// pasteAndStartToolStripMenuItem
		// 
		this.pasteAndStartToolStripMenuItem.Name = "pasteAndStartToolStripMenuItem";
		this.pasteAndStartToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.pasteAndStartToolStripMenuItem.Text = "貼り付けして接続(&O)";
		this.pasteAndStartToolStripMenuItem.Click += new System.EventHandler(this.pasteAndStartToolStripMenuItem_Click);
		// 
		// toolStripSeparator9
		// 
		this.toolStripSeparator9.Name = "toolStripSeparator9";
		this.toolStripSeparator9.Size = new System.Drawing.Size(188, 6);
		// 
		// selectAllToolStripMenuItem
		// 
		this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
		this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
		this.selectAllToolStripMenuItem.Text = "すべて選択(&A)";
		this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
		// 
		// MainForm
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(490, 334);
		this.Controls.Add(this.chatGridView1);
		this.Controls.Add(this.toolStrip2);
		this.Controls.Add(this.statusStrip1);
		this.Controls.Add(this.menuStrip1);
		this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
		this.MainMenuStrip = this.menuStrip1;
		this.Name = "MainForm";
		this.Text = "OpenCommentViewer";
		this.Load += new System.EventHandler(this.MainForm_Load);
		this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.idBoxContextMenu.ResumeLayout(false);
		this.ResumeLayout(false);
		this.PerformLayout();

    }

    #endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private OpenCommentViewer.Control.ChatGridView chatGridView1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private OpenCommentViewer.CustomControl.ToolStripSpringTextBox idBox;
		private System.Windows.Forms.ToolStripButton startButton;
		private System.Windows.Forms.ToolStripButton stopButton;
		private System.Windows.Forms.ToolStripMenuItem saveTicketToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadTicketToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		protected System.Windows.Forms.MenuStrip menuStrip1;
	  protected System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
	  private System.Windows.Forms.ToolStrip toolStrip2;
	  private System.Windows.Forms.ContextMenuStrip idBoxContextMenu;
	  private System.Windows.Forms.ToolStripMenuItem copyIdToolStripMenuItem;
	  private System.Windows.Forms.ToolStripMenuItem copyIdAsIdToolStripMenuItem;
	  private System.Windows.Forms.ToolStripMenuItem copyIdAsURLToolStripMenuItem;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
	  private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
	  private System.Windows.Forms.ToolStripMenuItem pasteAndStartToolStripMenuItem;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
	  private System.Windows.Forms.ToolStripMenuItem cutIdToolStripMenuItem;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
	  private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
  }
}
