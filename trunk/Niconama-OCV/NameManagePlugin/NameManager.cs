using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NameManagePlugin
{
	public class NameManager : Hal.NCSPlugin.IPlugin, Hal.NCSPlugin.IColumnExtention, Hal.NCSPlugin.ICellFormatter
	{

		#region [変数]

		Hal.NCSPlugin.IPluginHost _host;
		UserDatabase _dataBase = null;
		Dictionary<string, User> _users = new Dictionary<string,User>();
		System.Windows.Forms.DataGridViewColumn _column;

		NCSPlugin.IChat _selectedChat = null;
		User _selectedUser = null;
		bool _canceled = false;

		#endregion


		#region 非同期プロフィール名取得

		private void AddNameTask(string id){
			_canceled = false;
			System.Threading.ThreadPool.QueueUserWorkItem(_backgroundWorker_DoWork, id);
			
		}
		
		void _backgroundWorker_DoWork(object obj)
		{
			if (_canceled) {
				return;
			}

			string id = (string)obj;

			NicoApiSharp.AccountInfomation ac = NicoApiSharp.AccountInfomation.GetUserAccountInfomation(id);

			if (_canceled) {
				return;
			}

			SetUserName(id, ac.UserName);

		}
		

		#endregion

		#region ユーザー情報関係

		private void SetUserName(string id, string name) {
			lock (_users) {
				if (!_users.ContainsKey(id)) {
					_users.Add(id, new User(id));
				}

				User u = _users[id];
				u.Name = name;
				if (u.State != User.UserState.New) {
					u.State = User.UserState.Update;
				}
			}

			_host.UpdateCellValues();
		}

		private void SetUserColor(string id, System.Drawing.Color color) {
			lock (_users) {
				if (!_users.ContainsKey(id)) {
					_users.Add(id, new User(id));
				}

				User u = _users[id];
				u.Color = color;
				if (u.State != User.UserState.New) {
					u.State = User.UserState.Update;
				}
			}
			_host.UpdateCellValues();

		}

		private void SetUserDate(string id, DateTime date) {
			lock (_users) {
				if (_users.ContainsKey(id)) {
					User u = _users[id];
					if (u.LastCommentDate < date) {
						u.LastCommentDate = date;
						if (u.State != User.UserState.New) {
							u.State = User.UserState.Update;
						}
					}
				}
			}
		}

		private void ResetUserData(string id) {
			if (_users.ContainsKey(id)) {
				User u = _users[id];
				if (u.State == User.UserState.New) {
					_users.Remove(id);
				} else {
					u.Name = null;
					u.Color = System.Drawing.Color.Transparent;
					u.State = User.UserState.Delete;
				}
			}
			_host.UpdateCellValues();

		}


		#endregion

		private void CreateContextMenu()
		{
			System.Windows.Forms.ToolStripMenuItem root = new System.Windows.Forms.ToolStripMenuItem("ユーザー管理(&U)");
			System.Windows.Forms.ToolStripMenuItem getProfileName = new System.Windows.Forms.ToolStripMenuItem("プロフィールから名前を取得");
			System.Windows.Forms.ToolStripMenuItem changeName = new System.Windows.Forms.ToolStripMenuItem("名前をつける");
			System.Windows.Forms.ToolStripMenuItem autoChangeColor = new System.Windows.Forms.ToolStripMenuItem("適当な色をつける");
			System.Windows.Forms.ToolStripMenuItem changeColor = new System.Windows.Forms.ToolStripMenuItem("色を指定する");
			System.Windows.Forms.ToolStripMenuItem reset = new System.Windows.Forms.ToolStripMenuItem("設定をリセットする");

			getProfileName.Click += new EventHandler(getProfileName_Click);
			changeName.Click += new EventHandler(changeName_Click);
			autoChangeColor.Click += new EventHandler(autoChangeColor_Click);
			changeColor.Click += new EventHandler(changeColor_Click);
			reset.Click += new EventHandler(reset_Click);

			root.DropDownItems.Add(getProfileName);
			root.DropDownItems.Add(changeName);
			root.DropDownItems.Add(autoChangeColor);
			root.DropDownItems.Add(changeColor);
			root.DropDownItems.Add(reset);

			// コンテクストメニューへの追加と、メニューが開かれた際の動作を指定
			_host.AddContextMenuItem(root, delegate(object sender, EventArgs e)
			{

				if (_host.IsConnected) {
					_selectedChat = _host.GetSelectedChat();
					if (_selectedChat != null && _users.ContainsKey(_selectedChat.UserId)) {
						_selectedUser = _users[_selectedChat.UserId];
					} else {
						_selectedUser = null;
					}
				}

				root.Enabled = (_selectedChat != null);
				getProfileName.Enabled = (_selectedChat != null && !_selectedChat.Anonymity);
				changeName.Enabled = (_selectedChat != null);
				autoChangeColor.Enabled = (_selectedChat != null);
				changeColor.Enabled = (_selectedChat != null);
				reset.Enabled = (_selectedChat != null && _selectedUser != null);

			});
		}


		
		#region IPlugin メンバ

		public string Name
		{
			get { return "ユーザー管理プラグイン"; }
		}

		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string Description
		{
			get { return "ユーザーに名前や色をつけて識別するためのプラグインです。"; }
		}

		public void Initialize(Hal.NCSPlugin.IPluginHost host)
		{
			_host = host;
			_dataBase = UserDatabase.GetInstance("test.sqlite");
			_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
			_column.Name = "nameColumn";
			_column.HeaderText = "Name";
			_column.Width = 50;
			_column.ReadOnly = true;

			CreateContextMenu();
		}

		
		public void Run()
		{
			

		}

		void getProfileName_Click(object sender, EventArgs e)
		{
			if (_selectedChat != null) {
				AddNameTask(_selectedChat.UserId);
			}
		}

		void changeName_Click(object sender, EventArgs e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void autoChangeColor_Click(object sender, EventArgs e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void changeColor_Click(object sender, EventArgs e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void reset_Click(object sender, EventArgs e)
		{
			if (_selectedChat != null) {
				ResetUserData(_selectedChat.UserId);
			}
		}

		public void OnLiveStart(string liveId, DateTime startTime, int commentCount)
		{
			_users.Clear();
			if (!string.IsNullOrEmpty(_host.CommunityId)) {
				try {
					List<User> users = _dataBase.GetUsers(_host.CommunityId);
					foreach (User user in users) {
						_users.Add(user.Id, user);
					}
				} catch (Exception ex){
					_host.ShowFaitalMessage("データベースの読み込みに失敗しました。" + ex.Message);
				}
			}
			
		}

		public void OnLiveStop()
		{
			_dataBase.UpdateUsers(_host.CommunityId, _users.Values);
		}

		public void OnComment(Hal.NCSPlugin.IChat chat)
		{
			if (!string.IsNullOrEmpty(chat.UserId) && !chat.Anonymity && !_users.ContainsKey(chat.UserId)) {
				AddNameTask(chat.UserId);
			}
		}

		#endregion

		#region IDisposable メンバ

		public void Dispose()
		{
			_host = null;
			_dataBase.Dispose();
			_dataBase = null;
			_selectedChat = null;
			_selectedUser = null;
			_users = null;
		}

		#endregion

		#region IColumnExtention メンバ

		public System.Windows.Forms.DataGridViewColumn Column
		{
			get { return _column; }
		}

		public object OnCellValueNeeded(Hal.NCSPlugin.IChat chat)
		{
			if (!string.IsNullOrEmpty(chat.UserId) && _users.ContainsKey(chat.UserId)) {
				return _users[chat.UserId].Name;
			}

			return "";
		}

		#endregion

		#region IComparer<IChat> メンバ

		public int Compare(Hal.NCSPlugin.IChat x, Hal.NCSPlugin.IChat y)
		{
			return 0;
		}

		#endregion

		#region ICellFormatter メンバ

		public void OnCellFormatting(Hal.NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
		{
		}

		#endregion

	}
}
