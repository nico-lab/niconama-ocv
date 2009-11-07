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
	/// メッセージサーバーからチャットを取得するためのクラス
	/// </summary>
	public class ChatReceiver : ChatClient
	{

		/// <summary>
		/// チャットを受信したときに発生するイベント
		/// </summary>
		public event EventHandler<ChatReceiveEventArgs> ReceiveChat;

		private int _lastRes = 0;
		private object _lastResSync = new object();

		/// <summary>
		/// 初期化を行います
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			lock (_lastResSync) {
				_lastRes = 0;
			}
		}

		/// <summary>
		/// 最後に受け取ったコメント番号を取得します
		/// </summary>
		public int LastRes
		{
			get { return _lastRes; }
		}

		#region 非同期コメント取得

		/// <summary>
		/// サーバーから受け取ったXMLデータを解析する
		/// </summary>
		/// <param name="node"></param>
		protected override void ParseReceivedData(System.Xml.XmlNode node)
		{
			base.ParseReceivedData(node);

			if (node.FirstChild.Name.Equals("chat")) {
				Chat chat = null;
				try {
					chat = new Chat(node.FirstChild);
				} catch (Exception ex) {
					chat = null;
					Logger.Default.LogErrorMessage(ex.Message);
					return;
				}

				if (_lastRes < chat.No) {
					lock (_lastResSync) {
						_lastRes = chat.No;
					}
				}

				this.OnReceiveChat(chat);
			}
		}

		#endregion



		#region 過去ログ取得

		/// <summary>
		/// 指定された時間以前の過去ログを取得する
		/// </summary>
		/// <param name="data"></param>
		/// <param name="cookies"></param>
		/// <param name="userId"></param>
		/// <param name="when">取得開始時間</param>
		/// <param name="resFrom"></param>
		/// <returns></returns>
		public static Chat[] ReceiveLog(IMessageServerStatus data, System.Net.CookieContainer cookies, int userId, DateTime when, int resFrom)
		{

			List<Chat> results = new List<Chat>(1000);
			TcpClient tcpClient = null;
			try {
				Waybackkey key = Waybackkey.GetInstance(data.Thread, cookies);

				if (key != null) {
					bool timeouted = false;
					using (System.Threading.ManualResetEvent m = new System.Threading.ManualResetEvent(false)) {

						tcpClient = new TcpClient();
						tcpClient.SendTimeout = 1000;
						tcpClient.ReceiveTimeout = 100;

						AsyncCallback ac = (AsyncCallback)delegate(IAsyncResult ar)
						{
							if (!timeouted) {
								m.Set();
								TcpClient tcp = (TcpClient)ar.AsyncState;
								tcp.EndConnect(ar);
							}

						};

						tcpClient.BeginConnect(data.Address, data.Port, ac, tcpClient);

						// 既定時間以内に接続できない場合は失敗とする
						if (!m.WaitOne(ApiSettings.Default.DefaultConnectionTimeout, false)) {
							timeouted = true;
							throw new Exception("過去ログ取得：コネクションタイムアウト");
						}
					}

					// NetworkStreamは自動的に開放されないのでusingで囲む
					using (NetworkStream ns = tcpClient.GetStream()) {

						string msg = String.Format(ApiSettings.Default.WaybackThreadStartMessageFormat, data.Thread, resFrom, Utility.DateTimeToUnixTime(when), key.Value, userId);
						byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(msg + '\0');

						ns.Write(sendBytes, 0, sendBytes.Length);

						byte[] buffer = new byte[ApiSettings.Default.ReceiveBufferSize];
						int last = 0;

						//DataAvailableはすべて取得しきる前にfalseを返してしまうので使えない
						while (ns.CanRead) {
							int b = ns.ReadByte();

							if (b == -1) {
								break;
							}

							int i;
							for (i = 0; b != (int)'\0' && b != -1 && i < buffer.Length; i++, b = ns.ReadByte()) {
								buffer[i] = (byte)b;
							}

							string block = System.Text.Encoding.UTF8.GetString(buffer, 0, i);
							if (block.StartsWith("<chat")) {
								Chat chat = null;
								try {
									chat = new Chat(block);
								} catch (Exception ex) {
									chat = null;
									Logger.Default.LogErrorMessage(ex.Message);
								}

								if (chat != null) {
									results.Add(chat);

									// 指定数読み込んだ場合は終了
									if (chat.No == last) {
										break;
									}
								}
							} else if (block.StartsWith("<thread")) {
								ThreadHeader th = new ThreadHeader(block);
								last = th.LastRes;

							}

						}
					}
				}

			} catch (System.IO.IOException) {

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			} finally {
				if (tcpClient != null) {
					tcpClient.Close();
				}
			}

			return results.ToArray();
		}

		/// <summary>
		/// 放送の過去ログをすべて取得する
		/// </summary>
		/// <param name="data"></param>
		/// <param name="cookies"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static Chat[] ReceiveAllLog(IMessageServerStatus data, System.Net.CookieContainer cookies, int userId)
		{
			List<Chat> results = new List<Chat>();
			DateTime when = DateTime.Now;

			while (true) {
				Chat[] chats = ReceiveLog(data, cookies, userId, when, -1000);
				if (chats.Length != 0) {
					results.InsertRange(0, chats);
					if (1 < chats[0].No) {
						when = chats[0].Date;
					} else {
						break;
					}
				} else {
					break;
				}

			}

			return results.ToArray();
		}

		#endregion


		/// <summary>
		/// チャットを受信したことを通知します。
		/// </summary>
		/// <param name="chat"></param>
		protected virtual void OnReceiveChat(Chat chat)
		{
			if (this.ReceiveChat != null) {
				this.ReceiveChat(this, new ChatReceiveEventArgs(chat));
			}
		}


		/// <summary>
		/// メッセージサーバーに接続したことを通知します。
		/// </summary>
		/// <param name="threadHeader"></param>
		protected override void OnConnectServer(ThreadHeader threadHeader)
		{

			lock (_lastResSync) {
				_lastRes = threadHeader.LastRes;
			}

			base.OnConnectServer(threadHeader);
		}

		/// <summary>
		/// コメントを受信した際に発生するイベントの引数
		/// </summary>
		public class ChatReceiveEventArgs : EventArgs
		{
			private readonly Chat chat;

			/// <summary>
			/// コメントを受信した際に発生するイベントの引数
			/// </summary>
			/// <param name="chat"></param>
			public ChatReceiveEventArgs(Chat chat)
			{
				this.chat = chat;
			}

			/// <summary>
			/// 受信したチャットを取得します
			/// </summary>
			public Chat Chat
			{
				get { return this.chat; }
			}

		}

	}
}
