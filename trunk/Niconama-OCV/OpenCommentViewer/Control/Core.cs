﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{
	/// <summary>
	/// コア
	/// </summary>
	public class Core : Hal.NCSPlugin.IPluginHost, ICore
	{

		/// <summary>
		/// 番組の基本情報
		/// </summary>
		protected NicoApiSharp.Live.ILiveBasicStatus _liveBasicStatus = null;

		/// <summary>
		/// メッセージサーバー接続用の情報
		/// </summary>
		protected NicoApiSharp.Live.IMessageServerStatus _messageServerStatus = null;

		/// <summary>
		/// 視聴者に関する情報
		/// </summary>
		protected NicoApiSharp.Live.ILiveWatcherStatus _liveWatcherStatus = null;

		/// <summary>
		/// 放送の詳細情報
		/// </summary>
		protected NicoApiSharp.Live.ILiveDescription _liveDescription = null;

		/// <summary>
		/// アカウント情報
		/// </summary>
		protected NicoApiSharp.IAccountInfomation _accountInfomation = null;

		protected List<OcvChat> _chats;
		protected Control.IMainView _mainview = null;
		protected System.Windows.Forms.Form _form = null;
		protected SeetType _seetType = SeetType.Arena;

		string _reservedId = null;
		NicoApiSharp.Live.ChatReceiver _chatReceiver;
		System.Net.CookieContainer _cookies;
		NgChecker _ngChecker;

		protected List<Hal.NCSPlugin.IPlugin> _plugins = null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Core()
		{
			_chats = new List<OcvChat>();
			_cookies = new System.Net.CookieContainer();
			_chatReceiver = new NicoApiSharp.Live.ChatReceiver();
			_chatReceiver.ConnectServer += new EventHandler<NicoApiSharp.Live.ChatReceiver.ConnectServerEventArgs>(_chatReceiver_ConnectServer);
			_chatReceiver.ReceiveChat += new EventHandler<NicoApiSharp.Live.ChatReceiver.ChatReceiveEventArgs>(_chatReceiver_ReceiveChat);
			_chatReceiver.DisconnectServer += new EventHandler(_chatReceiver_DisconnectServer);

			_ngChecker = new NgChecker();
			_ngChecker.Initialize(this);

			_plugins = new List<Hal.NCSPlugin.IPlugin>();


		}

		/// <summary>
		/// フォームがロードされたときに呼び出されます
		/// </summary>
		protected virtual void Initialize()
		{
			// プラグインの読み込み
			LoadPlugins();

			// ログイン　必要があればログイン画面の表示
			if (UserSettings.Default.ShowAccountForm) {
				LoginForm f = new LoginForm();
				if (f.ShowDialog(this._form) == System.Windows.Forms.DialogResult.OK) {
					Login(UserSettings.Default.BrowserType, UserSettings.Default.CookieFilePath);
				}
			} else {
				Login(UserSettings.Default.BrowserType, UserSettings.Default.CookieFilePath);
			}

			// コマンドライン引数で指定された放送がある場合はそれに接続する
			if (_reservedId != null) {
				_mainview.IdBoxText = _reservedId;
				ConnectLive(_reservedId);
			}
		}

		/// <summary>
		/// プラグインを動的に読み込みます
		/// </summary>
		private void LoadPlugins()
		{
			Plugin.IPluginInfo[] infos = Plugin.PluginInfo.FindPlugins();
			foreach (Plugin.IPluginInfo pinfo in infos) {
				try {
					Hal.NCSPlugin.IPlugin pins = pinfo.CreateInstance();
					pins.Initialize(this);
					_plugins.Add(pins);

					// カラム拡張プラグイン
					if (pins is Hal.NCSPlugin.IColumnExtention) {
						Hal.NCSPlugin.IColumnExtention ext = (Hal.NCSPlugin.IColumnExtention)pins;
						_mainview.RegisterColumnExtention(ext);
					}

					if (pins is Hal.NCSPlugin.ICellFormatter) {
						Hal.NCSPlugin.ICellFormatter ext = (Hal.NCSPlugin.ICellFormatter)pins;
						_mainview.RegisterCellFormattingCallback(ext.OnCellFormatting);
					}

				} catch (Exception ex) {
					_mainview.ShowFatalMessage(pinfo.ClassName + "の登録に失敗しました。");
					Logger.Default.LogErrorMessage(pinfo.ClassName + "の登録に失敗しました。");
					Logger.Default.LogException(ex);
				}
			}

		}

		/// <summary>
		/// LiveIdから放送に接続する
		/// </summary>
		/// <param name="liveId"></param>
		/// <returns></returns>
		private bool connectLiveID(string liveId)
		{
			if (_accountInfomation == null) {
				_mainview.ShowFatalMessage("ログインが完了していません");
				return false;
			}

			NicoApiSharp.Live.PlayerStatus playerStatus = NicoApiSharp.Live.PlayerStatus.GetInstance(liveId, _cookies);

			if (playerStatus != null) {
				if (!playerStatus.HasError) {
					_liveBasicStatus = playerStatus;
					_liveWatcherStatus = playerStatus;
					_messageServerStatus = playerStatus;
					_liveDescription = NicoApiSharp.Live.LiveDescription.GetInstance(liveId, _cookies);
					_seetType = _liveBasicStatus.RoomLabel != "立ち見席" ? SeetType.Arena : SeetType.Standing;

					return ConnectServer(_accountInfomation, _liveDescription, _messageServerStatus);
				} else {
					_mainview.ShowFatalMessage(playerStatus.ErrorMessage);
				}
			}

			return false;
		}

		/// <summary>
		/// 与えられた情報を元にメッセージサーバーに接続を試みる
		/// </summary>
		/// <param name="accountInfomation">アカウント情報</param>
		/// <param name="liveDescription">放送の詳細情報</param>
		/// <param name="messageServerStatus">メッセージサーバにアクセスするための情報</param>
		/// <returns>接続に成功したか</returns>
		protected bool ConnectServer(NicoApiSharp.IAccountInfomation accountInfomation, NicoApiSharp.Live.ILiveDescription liveDescription, NicoApiSharp.Live.IMessageServerStatus messageServerStatus)
		{

			if (accountInfomation == null) {
				_mainview.ShowFatalMessage("ログインが完了していません");
				return false;
			}

			_chatReceiver.Disconnect();

			_chats.Clear();
			_ngChecker.Initialize(this);

			UserSettings.Default.LastAccessLiveId = liveDescription.LiveId;
			UserSettings.Default.Save();

			NicoApiSharp.Live.Chat[] chats = NicoApiSharp.Live.ChatReceiver.ReceiveAllLog(messageServerStatus, _cookies, accountInfomation.UserId);
			foreach (NicoApiSharp.Live.Chat chat in chats) {
				_chats.Add(new OcvChat(chat));
			}

			_chatReceiver.Connect(messageServerStatus, chats.Length + 1);
			return true;

		}

		#region ICore メンバ

		/// <summary>
		/// アリーナか立ち見かを返します
		/// </summary>
		public SeetType SeetType
		{
			get { return _seetType; }
		}

		/// <summary>
		/// ビューアで使用するメインフォームを指定する
		/// （拡張の際、別のフォームを使用できるように）
		/// </summary>
		/// <param name="mainform"></param>
		public void SetMainView(IMainView mainview)
		{
			_form = mainview as System.Windows.Forms.Form;
			System.Diagnostics.Debug.Assert(_form != null, "mainviewはSystem.Windows.Forms.Formの派生クラスである必要があります。");

			Hal.NCSPlugin.IPlugin plugin = mainview as Hal.NCSPlugin.IPlugin;
			_mainview = mainview;
			plugin.Initialize(this);
			_form.Load += new EventHandler(_form_Load);
			_form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(_form_FormClosing);

			_plugins.Add(plugin);

			ExtendForm();
			
		}

		/// <summary>
		/// フォームを拡張する
		/// </summary>
		protected virtual void ExtendForm()
		{
			// VPOSを数値で表示するカラムを追加する
			_mainview.RegisterColumnExtention(new VposColumnExtention());

			// NGソースを表示するカラムを追加する
			_mainview.RegisterColumnExtention(new NgColumnExtention());

			// 主コメをオレンジ色にするフォーマッターを追加する
			_mainview.RegisterCellFormattingCallback(ColoringOwnerComment);

		}

		private void ColoringOwnerComment(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e) {
			if (e.ColumnIndex == 1 && chat.IsOwnerComment) {
				e.CellStyle.ForeColor = System.Drawing.Color.OrangeRed;
			}
		}

		/// <summary>
		/// 開始直後に接続する放送IDを指定する
		/// </summary>
		/// <param name="id"></param>
		public void Reserve(string id)
		{
			_reservedId = id;
		}

		/// <summary>
		/// MessageServerStatusで指定されたコメントをすべて取得する
		/// （用途は未定、なんとなく作った）
		/// </summary>
		/// <param name="messageServerStatus"></param>
		/// <returns></returns>
		public bool GetLogComment(NicoApiSharp.Live.IMessageServerStatus messageServerStatus)
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
					NicoApiSharp.Live.Chat[] chats = NicoApiSharp.Live.ChatReceiver.ReceiveAllLog(_messageServerStatus, _cookies, _accountInfomation.UserId);
					foreach (NicoApiSharp.Live.Chat chat in chats) {
						_chats.Add(new OcvChat(chat));
					}

					// プラグインには開始通知の直後に終了通知する
					OnStartLive(null, null);
					OnDisconnectServer(null, EventArgs.Empty);

				}
				return true;
			}

			return false;
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
				_seetType = ticket.RoomLabel != "立ち見席" ? SeetType.Arena : SeetType.Standing;

				return ConnectServer(_accountInfomation, ticket, ticket);
			}

			return false;
		}



		/// <summary>
		/// 放送再接続用の情報を取得します
		/// </summary>
		/// <returns></returns>
		public Control.LiveTicket GetLiveTicket()
		{
			if (_liveBasicStatus != null && _liveDescription != null && _messageServerStatus != null) {
				return new LiveTicket(_liveBasicStatus, _messageServerStatus, _liveDescription);
			}

			return null;

		}

		/// <summary>
		/// ログインする
		/// </summary>
		/// <param name="browserType"></param>
		/// <param name="cookieFilePath">クッキーが保存されているファイル、nullの場合既定のファイルを対称にする</param>
		/// <returns></returns>
		public virtual bool Login(Cookie.CookieGetter.BROWSER_TYPE browserType, string cookieFilePath)
		{
			string[] userSessions = Cookie.CookieGetter.GetCookies("nicovideo.jp", "user_session", browserType, cookieFilePath);

			if (userSessions == null) {
				_mainview.ShowFatalMessage("指定されたブラウザからクッキーを取得できませんでした。");
				Logger.Default.LogMessage(string.Format("Login not found: type-{0}", browserType.ToString()));
				return false;
			}
			
			Logger.Default.LogMessage(string.Format("Login: type-{0}, cookies-{1}", browserType.ToString(), userSessions.Length));

			foreach (string session in userSessions) {
				
				System.Net.Cookie cuid = new System.Net.Cookie("user_session", session, "/", ".nicovideo.jp");
				_cookies.Add(cuid);

				_accountInfomation = NicoApiSharp.AccountInfomation.GetMyAccountInfomation(_cookies);
				if (_accountInfomation != null) {
					_mainview.ShowStatusMessage(string.Format("ログイン成功 : ユーザー名【{0}】", _accountInfomation.UserName));
					return true;
				} else {
					
				}
			}
			
			_mainview.ShowFatalMessage("ログインに失敗しました。");
			return false;

			
		}

		// デバッグ用
		public virtual void CallTestMethod()
		{
			NicoApiSharp.AccountInfomation ac = NicoApiSharp.AccountInfomation.GetUserAccountInfomation("9417784", _cookies);
			_mainview.ShowStatusMessage(ac.UserName);
		}




		#endregion

		#region イベント処理

		void _form_Load(object sender, EventArgs e)
		{

			Initialize();
		}

		void _form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			this.Disconnect();
			_mainview = null;

			// プラグインを破棄
			foreach (Hal.NCSPlugin.IPlugin plugin in _plugins) {
				plugin.Dispose();
			}

			_plugins.Clear();
			_plugins = null;
		}

		private void _chatReceiver_ConnectServer(object sender, NicoApiSharp.Live.ChatReceiver.ConnectServerEventArgs e)
		{
			if (_mainview != null) {
				if (_form.InvokeRequired) {
					_form.BeginInvoke(new EventHandler<NicoApiSharp.Live.ChatReceiver.ConnectServerEventArgs>(OnStartLive), new object[] { sender, e });
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
		protected virtual void OnStartLive(object sender, NicoApiSharp.Live.ChatReceiver.ConnectServerEventArgs e)
		{
			_mainview.ShowStatusMessage("メッセージサーバーに接続しました");
			_ngChecker.Check(_chats);

			// プラグインに通知
			foreach (Hal.NCSPlugin.IPlugin plugin in _plugins) {
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

		private void _chatReceiver_ReceiveChat(object sender, NicoApiSharp.Live.ChatReceiver.ChatReceiveEventArgs e)
		{
			if (_mainview != null) {
				if (_form.InvokeRequired) {
					_form.BeginInvoke(new EventHandler<NicoApiSharp.Live.ChatReceiver.ChatReceiveEventArgs>(OnReceivedChat), new object[] { sender, e });
				} else {
					OnReceivedChat(sender, e);
				}
			}


		}

		/// <summary>
		/// メッセージサーバーからチャットを受け取ったときに実行される
		/// ただし、過去ログ分については呼び出されない
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnReceivedChat(object sender, NicoApiSharp.Live.ChatReceiver.ChatReceiveEventArgs e)
		{
			OcvChat chat = new OcvChat(e.Chat);
			_chats.Add(chat);
			_ngChecker.Check(chat);

			//プラグインに通知
			foreach (Hal.NCSPlugin.IPlugin plugin in _plugins) {
				plugin.OnComment(chat);
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
		protected virtual void OnDisconnectServer(object sender, EventArgs e)
		{
			_mainview.ShowStatusMessage("メッセージサーバーから切断しました");
			foreach (Hal.NCSPlugin.IPlugin plugin in _plugins) {
				plugin.OnLiveStop();
			}
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

		NicoApiSharp.Live.Chat[] __chatsCache = null;
		public Hal.NCSPlugin.IChat[] Chats
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
					return NicoApiSharp.Live.OwnerCommentPoster.Post(_liveBasicStatus.LiveId, message, command, _cookies);
				}
			}

			return false;

		}

		public bool AddNG(Hal.NCSPlugin.NGType type, string source)
		{
			if (this.IsConnected && this.IsOwner && _liveBasicStatus != null) {
				return NicoApiSharp.Live.NgClient.AddNg(_liveBasicStatus.LiveId, type, source, _cookies);
			}

			return false;
		}

		public bool DeleteNG(Hal.NCSPlugin.NGType type, string source)
		{
			if (this.IsConnected && this.IsOwner && _liveBasicStatus != null) {
				return NicoApiSharp.Live.NgClient.DeleteNg(_liveBasicStatus.LiveId, type, source, _cookies);
			}

			return false;
		}

		public bool ConnectLive(string liveId)
		{
			return connectLiveID(liveId);
		}


		public void Disconnect()
		{
			if (this.IsConnected) {
				_chatReceiver.Disconnect();
			}
		}

		public Hal.NCSPlugin.INgClient[] GetNgClients()
		{
			if (this.LiveId != null) {
				return NicoApiSharp.Live.NgClient.GetNgClients(this.LiveId, _cookies);
			}

			return null;
		}

		public Hal.NCSPlugin.IChat GetSelectedChat()
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

		public bool InsertPluginChat(Hal.NCSPlugin.IChat chat)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion


		#region IPluginHost メンバ


		public bool SupportAddCellFormattingCallBack
		{
			get { return true; }
		}

		public bool SupportAddColumn
		{
			get { return true; }
		}

		public bool SupportAddContextMenuItem
		{
			get { return true; }
		}

		public bool SupportAddMenuStripItem
		{
			get { return true; }
		}

		public void AddCellFormattingCallBack(Hal.NCSPlugin.CellFormattingCallback callback)
		{
			System.Diagnostics.Debug.Assert(_mainview != null, "フォームの初期化が完了していません。");

			_mainview.RegisterCellFormattingCallback(callback);
		}

		public void AddColumnExtention(Hal.NCSPlugin.IColumnExtention columnExtention)
		{
			System.Diagnostics.Debug.Assert(_mainview != null, "フォームの初期化が完了していません。");

			_mainview.RegisterColumnExtention(columnExtention);
		}

		public void AddContextMenuItem(System.Windows.Forms.ToolStripMenuItem menuItem)
		{
			System.Diagnostics.Debug.Assert(_mainview != null, "フォームの初期化が完了していません。");
			
			_mainview.RegisterContextMenuExtention(menuItem);
		}

		public void AddMenuStripItem(string category, System.Windows.Forms.ToolStripMenuItem menuItem)
		{
			System.Diagnostics.Debug.Assert(_mainview != null, "フォームの初期化が完了していません。");
			
			_mainview.RegisterMenuStripExtention(category, menuItem);
		}


		#endregion
	}
}