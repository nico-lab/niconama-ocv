using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Control
{
	/// <summary>
	/// コア
	/// </summary>
	public class Core : NCSPlugin.IPluginHost, ICore
	{

		protected List<NicoAPI.Chat> _chats;
		protected NicoAPI.ILiveBasicStatus _liveBasicStatus = null;
		protected NicoAPI.IMessageServerStatus _messageServerStatus = null;
		protected NicoAPI.ILiveWatcherStatus _liveWatcherStatus = null;
		protected NicoAPI.ILiveDescription _liveDescription = null;
		protected NicoAPI.IAccountInfomation _accountInfomation = null;
		protected Control.IMainView _mainview = null;
		protected System.Windows.Forms.Form _form = null;
		protected SeetType _seetType = SeetType.Arena;

		string _reservedId = null;
		NicoAPI.ChatReceiver _chatReceiver;
		System.Net.CookieContainer _cookies;
		NgChecker _ngChecker;

		protected List<NCSPlugin.IPlugin> _plugins = null;

		public Core()
		{
			_chats = new List<OpenCommentViewer.NicoAPI.Chat>();
			_cookies = new System.Net.CookieContainer();
			_chatReceiver = new OpenCommentViewer.NicoAPI.ChatReceiver();
			_chatReceiver.ConnectServer += new EventHandler<OpenCommentViewer.NicoAPI.ChatReceiver.ConnectServerEventArgs>(_chatReceiver_ConnectServer);
			_chatReceiver.ReceiveChat += new EventHandler<OpenCommentViewer.NicoAPI.ChatReceiver.ChatReceiveEventArgs>(_chatReceiver_ReceiveChat);
			_chatReceiver.DisconnectServer += new EventHandler(_chatReceiver_DisconnectServer);

			_ngChecker = new NgChecker();
			_ngChecker.Initialize(this);

			_plugins = new List<NCSPlugin.IPlugin>();

		}

		/// <summary>
		/// プラグインを動的に読み込みます
		/// </summary>
		private void LoadPlugins()
		{
			Plugin.IPluginInfo[] infos = Plugin.PluginInfo.FindPlugins();
			foreach (Plugin.IPluginInfo pinfo in infos) {
				try {
					NCSPlugin.IPlugin pins = pinfo.CreateInstance();
					pins.Initialize(this);
					_plugins.Add(pins);

					// カラム拡張プラグイン
					if (pins is NCSPlugin.IColumnExtention) {
						_mainview.RegisterColumnExtention((NCSPlugin.IColumnExtention)pins);
					}

					// コンテクストメニュー拡張プラグイン
					if (pins is NCSPlugin.IContextMenuExtention) {
						_mainview.RegisterContextMenuExtention((NCSPlugin.IContextMenuExtention)pins);
					}

					// メニューストリップ拡張プラグイン
					if (pins is NCSPlugin.IMenuStripExtention) {
						_mainview.RegisterMenuStripExtention((NCSPlugin.IMenuStripExtention)pins);
					}

				} catch (Exception ex) {
					_mainview.ShowFatalMessage(pinfo.ClassName + "の登録に失敗しました。");
					Logger.Default.LogErrorMessage(pinfo.ClassName + "の登録に失敗しました。");
					Logger.Default.LogException(ex);
				}
			}

		}

		#region ICore メンバ

		/// <summary>
		/// ビューアで使用するメインフォームを指定する
		/// （拡張の際、別のフォームを使用できるように）
		/// </summary>
		/// <param name="mainform"></param>
		public void SetMainView(IMainView mainview)
		{
			_form = mainview as System.Windows.Forms.Form;
			System.Diagnostics.Debug.Assert(_form != null, "mainviewはSystem.Windows.Forms.Formの派生クラスである必要があります。");

			NCSPlugin.IPlugin plugin = mainview as NCSPlugin.IPlugin;
			_mainview = mainview;
			plugin.Initialize(this);
			_form.Load += new EventHandler(_form_Load);
			_form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(_form_FormClosing);

			_plugins.Add(plugin);

			// VPOSを数値で表示するカラムを追加する
			_mainview.RegisterColumnExtention(new VposColumnExtention());
			_mainview.RegisterColumnExtention(new NgColumnExtention());
		}

		/// <summary>
		/// 開始直後に接続する放送IDを指定する
		/// </summary>
		/// <param name="id"></param>
		public void Reserve(string id)
		{
			_reservedId = id;
		}

		public SeetType SeetType
		{
			get { return _seetType; }
		}

		public Control.LiveTicket GetLiveTicket()
		{
			if (_liveBasicStatus != null && _liveDescription != null && _messageServerStatus != null) {
				return new LiveTicket(_liveBasicStatus, _messageServerStatus, _liveDescription);
			}

			return null;

		}

		public virtual bool Login()
		{
			Logger.Default.LogMessage("Core.Login start");
			string userSession = Cookie.CookieGetter.GetCookie("nicovideo.jp", "user_session", UserSettings.Default.BrowserType, UserSettings.Default.CookieFilePath);

			Logger.Default.LogMessage("us" + userSession);

			if (string.IsNullOrEmpty(userSession)) {
				_mainview.ShowFatalMessage("指定されたブラウザからクッキーを取得できませんでした。");
				return false;
			}

			Logger.Default.LogMessage("success get cookie");

			System.Net.Cookie cuid = new System.Net.Cookie("user_session", userSession, "/", ".nicovideo.jp");
			_cookies.Add(cuid);

			Logger.Default.LogMessage("test login");

			_accountInfomation = NicoAPI.AccountInfomation.GetInstance(_cookies);
			if (_accountInfomation != null) {
				Logger.Default.LogMessage("login success");
				_mainview.ShowStatusMessage(string.Format("ログイン成功 : ユーザー名【{0}】", _accountInfomation.UserName));
				return true;
			} else {
				Logger.Default.LogMessage("login failed");
				_mainview.ShowFatalMessage("ログインに失敗しました。");
				return false;
			}
		}

		// デバッグ用
		public void CallTestMethod()
		{
			//_chatReceiver.SendMessge("<test />");
		}

		#endregion

		#region イベント処理

		void _form_Load(object sender, EventArgs e)
		{

			// プラグインの読み込み
			LoadPlugins();

			// ログイン　必要があればログイン画面の表示
			if (UserSettings.Default.ShowAccountForm) {
				LoginForm f = new LoginForm();
				if (f.ShowDialog(this._form) == System.Windows.Forms.DialogResult.OK) {
					Login();
				}
			} else {
				Login();
			}

			// コマンドライン引数で指定された放送がある場合はそれに接続する
			if (_reservedId != null) {
				_mainview.IdBoxText = _reservedId;
				ConnectLive(_reservedId);
			}
		}

		void _form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			this.Disconnect();
			_mainview = null;

			// プラグインを破棄
			foreach (NCSPlugin.IPlugin plugin in _plugins) {
				plugin.Dispose();
			}

			_plugins.Clear();
			_plugins = null;
		}

		private void _chatReceiver_ConnectServer(object sender, OpenCommentViewer.NicoAPI.ChatReceiver.ConnectServerEventArgs e)
		{
			if (_mainview != null) {
				if (_form.InvokeRequired) {
					_form.BeginInvoke(new EventHandler<NicoAPI.ChatReceiver.ConnectServerEventArgs>(OnStartLive), new object[] { sender, e });
				} else {
					OnStartLive(sender, e);
				}
			}

		}

		/// <summary>
		/// メッセージサーバーからThreadを受け取ったときに実行される
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnStartLive(object sender, NicoAPI.ChatReceiver.ConnectServerEventArgs e)
		{
			_mainview.ShowStatusMessage("メッセージサーバーに接続しました");
			_ngChecker.Check(_chats);

			// プラグインに通知
			foreach (NCSPlugin.IPlugin plugin in _plugins) {
				plugin.OnLiveStart(this.LiveId, this.ServerStartTime, _chats.Count);
			}

			// デバッグ用にチケットをすべて保存する
			LiveTicket log = GetLiveTicket();
			if (log != null) {
				string fileName = log.LiveId + (_seetType == SeetType.Arena ? "Arena" : "Standing") + ".xml";
				string path = System.IO.Path.Combine(ApplicationSettings.Default.LiveTicketsFolder, fileName);
				Utility.Serialize(path, log, typeof(LiveTicket));
			}
		}

		private void _chatReceiver_ReceiveChat(object sender, OpenCommentViewer.NicoAPI.ChatReceiver.ChatReceiveEventArgs e)
		{
			if (_mainview != null) {
				if (_form.InvokeRequired) {
					_form.BeginInvoke(new EventHandler<NicoAPI.ChatReceiver.ChatReceiveEventArgs>(onReceivedChat), new object[] { sender, e });
				} else {
					onReceivedChat(sender, e);
				}
			}


		}

		/// <summary>
		/// メッセージサーバーからチャットを受け取ったときに実行される
		/// ただし、過去ログ分については呼び出されない
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void onReceivedChat(object sender, OpenCommentViewer.NicoAPI.ChatReceiver.ChatReceiveEventArgs e)
		{
			_chats.Add(e.Chat);
			_ngChecker.Check(e.Chat);

			//プラグインに通知
			foreach (NCSPlugin.IPlugin plugin in _plugins) {
				plugin.OnComment(e.Chat);
			}


		}


		private void _chatReceiver_DisconnectServer(object sender, EventArgs e)
		{
			if (_mainview != null) {
				if (_form.InvokeRequired) {
					_form.BeginInvoke(new EventHandler(OnDisconnectServer), new object[] { sender, e });
				} else {
					OnDisconnectServer(sender, e);
				}
			}

		}

		/// <summary>
		/// サーバーと通信ができなくなったときに実行される
		/// （2009/10/25 /disconnectは検知していない）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDisconnectServer(object sender, EventArgs e)
		{
			_mainview.ShowStatusMessage("メッセージサーバーから切断しました");
			foreach (NCSPlugin.IPlugin plugin in _plugins) {
				plugin.OnLiveStop();
			}
		}

		/// <summary>
		/// チケット情報を元にメッセージサーバーへの接続を試みる
		/// </summary>
		/// <param name="ticket"></param>
		/// <returns></returns>
		public bool ConnectByLiveTicket(LiveTicket ticket)
		{
			if (ticket != null) {
				_liveBasicStatus = ticket;
				_messageServerStatus = ticket;
				_liveDescription = ticket;
				_seetType = _liveBasicStatus.RoomLabel != "立ち見席" ? SeetType.Arena : SeetType.Standing;

				return ConnectServer();
			}

			return false;
		}

		protected bool ConnectServer()
		{
			if (_accountInfomation == null) {
				_mainview.ShowFatalMessage("ログインが完了していません");
				return false;

			}

			_chatReceiver.Disconnect();

			_chats.Clear();
			_ngChecker.Initialize(this);

			switch (_seetType) {
				case SeetType.Arena:
					_mainview.IdBoxText = _liveDescription.LiveId + " - 【アリーナ】";
					_mainview.Text = "【アリーナ】" + _liveDescription.LiveName;
					break;
				case SeetType.Standing:
					_mainview.IdBoxText = _liveDescription.LiveId + " - 【立ち見】";
					_mainview.Text = "【立ち見】" + _liveDescription.LiveName;
					break;
			}

			UserSettings.Default.LastAccessLiveId = _liveDescription.LiveId;
			UserSettings.Default.Save();

			NicoAPI.Chat[] chats = NicoAPI.ChatReceiver.ReceiveAllLog(_messageServerStatus, _cookies, _accountInfomation.UserId);
			_chats.AddRange(chats);
			_chatReceiver.Connect(_messageServerStatus, chats.Length + 1);
			return true;

		}

		public bool GetLogComment(NicoAPI.IMessageServerStatus messageServerStatus)
		{
			if (messageServerStatus != null) {

				if (_accountInfomation == null) {

					_mainview.ShowFatalMessage("ログインが完了していません");
					return false;

				}

				_chats.Clear();
				_ngChecker.Initialize(this);

				_messageServerStatus = messageServerStatus;

				if (_accountInfomation.IsPremium) {
					NicoAPI.Chat[] chats = NicoAPI.ChatReceiver.ReceiveAllLog(_messageServerStatus, _cookies, _accountInfomation.UserId);
					_chats.AddRange(chats);

					OnStartLive(null, null);
					OnDisconnectServer(null, EventArgs.Empty);

				}
				return true;
			}

			return false;
		}


		#endregion


		#region IPluginHost メンバ

		public bool IsConnected
		{
			get { return this._chatReceiver.IsConnected; }
		}

		public bool CanPostComment
		{
			get { return false; }
		}

		public bool CanPostOwnerComment
		{
			get { return this.IsConnected && this.IsOwner; }
		}

		public bool IsOwner
		{
			get { return this._liveWatcherStatus != null && this._liveWatcherStatus.IsOwner; }
		}

		public bool IsPremium
		{
			get { return this._liveWatcherStatus != null && this._liveWatcherStatus.IsPremium; }
		}

		NicoAPI.Chat[] __chatsCache = null;
		public NCSPlugin.IChat[] Chats
		{
			get
			{
				if (__chatsCache == null || __chatsCache.Length != _chats.Count) {
					__chatsCache = _chats.ToArray();
				}

				return __chatsCache;
			}
		}

		public string LiveId
		{
			get
			{
				if (_liveBasicStatus != null) {
					return _liveBasicStatus.LiveId;
				} else {
					return null;
				}
			}
		}

		public string LiveName
		{
			get
			{
				if (_liveDescription != null) {
					return _liveDescription.LiveName;
				} else {
					return null;
				}
			}
		}

		public string CommunityId
		{
			get
			{
				if (_liveDescription != null) {
					return _liveDescription.CommunityId;
				} else {
					return null;
				}
			}
		}

		public string CommunityName
		{
			get
			{
				if (_liveDescription != null) {
					return _liveDescription.CommunityName;
				} else {
					return null;
				}
			}
		}

		public DateTime LocalStartTime
		{
			get
			{
				if (_liveBasicStatus != null) {
					return _liveBasicStatus.LocalStartTime;
				} else {
					return new DateTime();
				}
			}
		}

		public DateTime ServerStartTime
		{
			get
			{
				if (_liveBasicStatus != null) {
					return _liveBasicStatus.StartTime;
				} else {
					return new DateTime();
				}
			}
		}

		public object Win32WindowOwner
		{
			get { return _mainview; }
		}

		public string PluginDataFolder
		{
			get { return ApplicationSettings.Default.PluginsDataFolder; }
		}

		public string PluginsFolder
		{
			get { return ApplicationSettings.Default.PluginsFolder; }
		}

		public int InterfaceVersion
		{
			get { return 0; }
		}

		public string ApplicationName
		{
			get { return System.Windows.Forms.Application.ProductName; }
		}

		public Version ApplicationVersion
		{
			get { return new Version(System.Windows.Forms.Application.ProductVersion); }
		}

		public virtual void PostComment(string message, string command)
		{
			this.ShowFaitalMessage("未実装の機能(PostComment)がプラグインから呼び出されました。");
		}

		public bool PostOwnerComment(string message, string command)
		{
			if (this.CanPostOwnerComment) {
				if (_liveBasicStatus != null && !string.IsNullOrEmpty(message) && command != null) {
					return NicoAPI.OwnerCommentPoster.Post(_liveBasicStatus.LiveId, message, command, _cookies);
				}
			}

			return false;

		}

		public bool AddNG(NCSPlugin.NGType type, string source)
		{
			if (this.IsConnected && this.IsOwner && _liveBasicStatus != null) {
				return NicoAPI.NgClient.AddNg(_liveBasicStatus.LiveId, type, source, _cookies);
			}

			return false;
		}

		public bool DeleteNG(NCSPlugin.NGType type, string source)
		{
			if (this.IsConnected && this.IsOwner && _liveBasicStatus != null) {
				return NicoAPI.NgClient.DeleteNg(_liveBasicStatus.LiveId, type, source, _cookies);
			}

			return false;
		}

		public bool ConnectLive(string liveId)
		{
			if (_accountInfomation == null) {
				_mainview.ShowFatalMessage("ログインが完了していません");
				return false;
			}

			NicoAPI.PlayerStatus playerStatus = NicoAPI.PlayerStatus.GetInstance(liveId, _cookies);

			if (playerStatus != null) {
				if (!playerStatus.HasError) {
					_liveBasicStatus = playerStatus;
					_liveWatcherStatus = playerStatus;
					_messageServerStatus = playerStatus;
					_liveDescription = NicoAPI.LiveDescription.GetInstance(liveId, _cookies);
					_seetType = _liveBasicStatus.RoomLabel != "立ち見席" ? SeetType.Arena : SeetType.Standing;

					return ConnectServer();
				} else {
					_mainview.ShowFatalMessage(playerStatus.ErrorMessage);
				}
			}

			return false;
		}


		public void Disconnect()
		{
			if (this.IsConnected) {
				_chatReceiver.Disconnect();
			}
		}

		public NCSPlugin.INgClient[] GetNgClients()
		{
			if (this.LiveId != null) {
				return NicoAPI.NgClient.GetNgClients(this.LiveId, _cookies);
			}

			return null;
		}

		public NCSPlugin.IChat GetSelectedChat()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool SelectChat(int no)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void ShowStatusMessage(string message)
		{
			_mainview.ShowStatusMessage(message);
		}

		public void ShowFaitalMessage(string message)
		{
			_mainview.ShowFatalMessage(message);
		}

		#endregion



	}
}
