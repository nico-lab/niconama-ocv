using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace Hal.NicoApiSharp.Live
{

	/// <summary>
	/// ニコ生のメッセージサーバーと通信するためのクラス
	/// 受信したデータを処理するにはParseReceivedDataをオーバーライドする
	/// </summary>
	public class ChatClient
	{

		#region [イベント]

		/// <summary>
		/// コメント取得が開始されたときに発生するイベント
		/// </summary>
		public event EventHandler<ConnectServerEventArgs> ConnectServer;

		/// <summary>
		/// 接続に失敗したときに発生するイベント
		/// </summary>
		public event EventHandler<ConnectionErrorEventArgs> ConnectionError;

		/// <summary>
		/// サーバーから切断したときに発生するイベント
		/// </summary>
		public event EventHandler DisconnectServer;

		#endregion [イベント]

		#region [変数]

		/// <summary>
		/// スレッド情報
		/// </summary>
		protected ThreadHeader _threadHeader = null;

		private bool _disconnecting = false;
		private object _disconnectingSync = new object();
		private System.Threading.ManualResetEvent _cancelEvent;

		private TcpClient _tcpClient;
		protected System.ComponentModel.AsyncOperation _asyncOperation = null;

		#endregion [変数]


		#region [処理]

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ChatClient()
		{
			_cancelEvent = new System.Threading.ManualResetEvent(true);
			_asyncOperation = null;
		}

		/// <summary>
		/// サーバーに接続中かどうかを取得します
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return _tcpClient != null && _tcpClient.Connected;
			}
		}

		/// <summary>
		/// 通信が中断状態にあるかどうかを取得・設定する
		/// </summary>
		private bool IsDisconnecting
		{
			get
			{
				lock (_disconnectingSync) {
					return _disconnecting;
				}
			}

			set
			{
				//非接続状態ではtrueにしない
				if (!value || this.IsConnected) {

					lock (_disconnectingSync) {
						_disconnecting = value;
					}

				}

			}
		}

		#region 非同期受信

		/// <summary>
		/// コメント取得を開始する
		/// </summary>
		/// <param name="data"></param>
		/// <param name="resFrom"></param>
		/// <returns>成功・失敗</returns>
		public bool Connect(IMessageServerStatus data, int resFrom)
		{
			this.Initialize();

			lock (this) {

				try {

					bool timeouted = false;
					using (System.Threading.ManualResetEvent m = new System.Threading.ManualResetEvent(false)) {

						_tcpClient = new TcpClient();
						_tcpClient.SendTimeout = 1000;
						_tcpClient.ReceiveTimeout = 1000;

						AsyncCallback ac = (AsyncCallback)delegate(IAsyncResult ar)
						{
							if (!timeouted) {
								m.Set();
								TcpClient tcp = (TcpClient)ar.AsyncState;
								tcp.EndConnect(ar);
							}

						};

						// 既定時間以上応答が無かった場合はタイムアウト
						_tcpClient.BeginConnect(data.Address, data.Port, ac, _tcpClient);

						if (!m.WaitOne(ApiSettings.Default.DefaultConnectionTimeout, false)) {
							timeouted = true;
							throw new Exception("コメント受信：サーバーコネクションタイムアウト");
						}
					}

					string msg = String.Format(ApiSettings.Default.ThreadStartMessageFormat, data.Thread, resFrom);
					SendMessge(msg);

					NetworkStream ns = _tcpClient.GetStream();
					StateObject obj = new StateObject(ApiSettings.Default.ReceiveBufferSize, ns);
					ns.BeginRead(obj.Buffer, 0, obj.BufferSize, new AsyncCallback(ReadCallback), obj);

					// キャンセルイベントをリセット
					// 切断されるまでセットされない
					_cancelEvent.Reset();

					return true;

				} catch (Exception ex) {
					Logger.Default.LogException(ex);
				}
			}

			return false;
		}


		/// <summary>
		/// 接続を中断する
		/// </summary>
		public void Disconnect()
		{

			if (_tcpClient != null) {
				IsDisconnecting = true;

				lock (this) {
					try {
						_tcpClient.Close();

					} catch (Exception ex) {
						Logger.Default.LogException(ex);
						throw;
					}

					_tcpClient = null;
				}

				// ReadCallBackが終わるまで待つ
				_cancelEvent.WaitOne();
				IsDisconnecting = false;
				OnDisconnectServer();
			}
		}

		/// <summary>
		/// 状態を初期化する
		/// </summary>
		protected virtual void Initialize()
		{

			// 既に接続中なら中止する
			if (this.IsConnected) {
				Disconnect();
			}

			_asyncOperation = System.ComponentModel.AsyncOperationManager.CreateOperation(null);

		}

		/// <summary>
		/// サーバーからデータを受け取ったときに実行される
		/// </summary>
		/// <param name="ar"></param>
		private void ReadCallback(IAsyncResult ar)
		{
			StateObject obj = (StateObject)ar.AsyncState;

			try {

				lock (this) {

					if (_tcpClient != null && _tcpClient.Connected) {

						if (obj.WorkingStream.CanRead && !IsDisconnecting) {
							int readBytes = 0;

							readBytes = obj.WorkingStream.EndRead(ar) + obj.Remainder;

							int proccessedByte = 0;

							// 受け取ったデータを解析する
							if (!IsDisconnecting) {
								proccessedByte = AnalayzeResponse(obj.Buffer, readBytes);
							}

							//バッファーいっぱいまでデータを受信した場合
							if (proccessedByte != readBytes) {

								//処理しきれていない部分を次回に持ち越す
								obj.Remainder = readBytes - proccessedByte;
								Array.Copy(obj.Buffer, proccessedByte, obj.Buffer, 0, obj.Remainder);

							} else {
								obj.Remainder = 0;
							}

							// キャンセル状態でなければ次のデータを待つ
							if (!IsDisconnecting) {
								obj.WorkingStream.BeginRead(obj.Buffer, obj.Remainder, obj.BufferSize - obj.Remainder, new AsyncCallback(ReadCallback), obj);
								return;
							}
						}
					}
				}

			} catch (Exception ex) {
				Logger.Default.LogErrorMessage(ex.Message);
				OnConnectionError(ex);
			}

			// エラーが発生したか接続がキャンセルされた場合ここに到達する

			obj.WorkingStream.Close();
			obj.WorkingStream = null;
			obj.Buffer = null;

			// サーバーからの読み込みが終了したことを通知する
			_cancelEvent.Set();

			// まだ接続状態であれば切断処理をする
			if (this.IsConnected) {
				Disconnect();
			}

		}

		/// <summary>
		/// サーバーから受け取ったデータをブロックごとに分割して解析する
		/// </summary>
		/// <param name="buffer">読み込んだデータが格納されているバッファー</param>
		/// <param name="length">処理が必要なバイト数</param>
		/// <returns>処理したバイト数</returns>
		private int AnalayzeResponse(byte[] buffer, int length)
		{
			string block;
			int cursor = 0;

			for (int i = 0; i < length; i++) {

				//データは\0で区切られているので\0区切りで処理していく
				if (buffer[i] == '\0') {

					block = System.Text.Encoding.UTF8.GetString(buffer, cursor, i - cursor);
					cursor = i + 1;

					System.Xml.XmlDocument xdoc = new XmlDocument();
					try {
						xdoc.LoadXml(block);
					} catch (Exception ex) {
						Logger.Default.LogException(ex);
						continue;
					}

					ParseReceivedData(xdoc);
				}
			}

			return cursor;
		}

		/// <summary>
		/// サーバーから受け取ったXMLデータを解析する
		/// オーバーライドする際は基底を呼び出さないとOnConnectServerイベントが発生しなくなる
		/// </summary>
		/// <param name="node"></param>
		protected virtual void ParseReceivedData(System.Xml.XmlNode node)
		{

			if (node.FirstChild.Name.Equals("thread")) {
				// スレッドデータであった場合（接続直後に送られてくる）
				_threadHeader = new ThreadHeader(node);
				this.OnConnectServer(_threadHeader);

			}

		}

		#endregion

		#region 同期送信

		/// <summary>
		/// サーバーにメッセージを送る
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		protected bool SendMessge(string message)
		{

			if (this.IsConnected && !this.IsDisconnecting) {

				byte[] wdata = System.Text.Encoding.UTF8.GetBytes(message + "\0");
				try {
					NetworkStream ns = _tcpClient.GetStream();
					if (ns != null && ns.CanWrite) {
						ns.Write(wdata, 0, wdata.Length);
						return true;
					}

				} catch (Exception ex) {
					Logger.Default.LogException(ex);
				}

			}

			return false;
		}

		#endregion

		

		/// <summary>
		/// メッセージサーバーに接続したことを通知します。
		/// </summary>
		/// <param name="threadHeader"></param>
		protected virtual void OnConnectServer(ThreadHeader threadHeader)
		{
			if (this.ConnectServer != null) {
				if (_asyncOperation != null) {
					_asyncOperation.Post(postConnectServerEvent, new ConnectServerEventArgs(threadHeader));
				} else {
					postConnectServerEvent(new ConnectServerEventArgs(threadHeader));
				}
				
			}
		}
		
		private void postConnectServerEvent(object obj) {
			System.Diagnostics.Debug.Assert(obj is ConnectServerEventArgs, "objはConnectServerEventArgsである必要があります。");
			this.ConnectServer(this, (ConnectServerEventArgs)obj);
		}

		/// <summary>
		/// メッセージサーバーから切断したことを通知します。
		/// </summary>
		protected virtual void OnDisconnectServer()
		{
			if (this.DisconnectServer != null) {
				if (_asyncOperation != null) {
					_asyncOperation.PostOperationCompleted(postDisconnectEvent, EventArgs.Empty);
				} else {
					postDisconnectEvent(EventArgs.Empty);
				}
			}
		}

		private void postDisconnectEvent(object obj) {
			this.DisconnectServer(this, EventArgs.Empty);
		}

		/// <summary>
		/// 接続に失敗したことを通知します。
		/// </summary>
		/// <param name="ex"></param>
		protected virtual void OnConnectionError(Exception ex)
		{
			if (this.ConnectionError != null) {
				if (_asyncOperation != null) {
					_asyncOperation.Post(postConnectionErrorEvent, new ConnectionErrorEventArgs(ex));
				} else {
					postConnectionErrorEvent(new ConnectionErrorEventArgs(ex));
				}
			}
		}

		private void postConnectionErrorEvent(object obj) {
			System.Diagnostics.Debug.Assert(obj is ConnectionErrorEventArgs, "objはConnectionErrorEventArgsである必要があります。");
			this.ConnectionError(this, (ConnectionErrorEventArgs)obj);
		}

		/// <summary>
		/// サーバーに接続した際に発生するイベントの引数
		/// </summary>
		public class ConnectServerEventArgs : EventArgs
		{
			readonly private ThreadHeader _th;

			/// <summary>
			///ThreadHeaderを取得します
			/// </summary>
			public ThreadHeader ThreadHeader
			{
				get { return _th; }
			}

			/// <summary>
			/// コメントを受信した際に発生するイベントの引数
			/// </summary>
			/// <param name="threadHeader"></param>
			internal ConnectServerEventArgs(ThreadHeader threadHeader)
			{
				_th = threadHeader;
			}

		}

		/// <summary>
		/// 接続失敗時に発生するイベントの引数
		/// </summary>
		public class ConnectionErrorEventArgs : EventArgs
		{
			readonly private Exception _ex;

			/// <summary>
			/// 受信時点での最後のコメント番号
			/// </summary>
			public Exception Exception
			{
				get { return _ex; }
			}

			/// <summary>
			/// コメントを受信した際に発生するイベントの引数
			/// </summary>
			/// <param name="ex"></param>
			internal ConnectionErrorEventArgs(Exception ex)
			{
				_ex = ex;
			}

		}

		/// <summary>
		/// 非同期通信用のオブジェクト
		/// </summary>
		private class StateObject
		{
			public NetworkStream WorkingStream = null;
			public readonly int BufferSize;
			public byte[] Buffer;

			public int Remainder = 0;


			public StateObject(int bufferSize)
			{
				BufferSize = bufferSize;
				Buffer = new byte[bufferSize];
			}

			public StateObject(int bufferSize, NetworkStream ns)
				: this(bufferSize)
			{
				WorkingStream = ns;
			}
		}

		#endregion [処理]


		

	}
}
