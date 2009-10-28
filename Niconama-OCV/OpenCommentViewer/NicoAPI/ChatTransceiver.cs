using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{

	/// <summary>
	/// メッセージサーバーと相互通信するためのクラス
	/// </summary
	public class ChatTransceiver : ChatReceiver
	{

		/// <summary>
		/// 送信結果を受け取った際に発生します
		/// </summary>
		public event EventHandler<ReceiveSendResultEventArgs> ReceiveSendResult;

		PostKey _key;
		Chat _tempchat;

		protected override void OnConnectServer(ThreadHeader threadHeader)
		{
			base.OnConnectServer(threadHeader);
			_key = null;
			_tempchat = null;
		}

		public bool PostComment(Chat chat, System.Net.CookieContainer cookies) {

			if(this.IsConnected && _threadHeader != null){
				_tempchat = chat;

				if (_key == null) {
					_key = PostKey.GetInstance(_threadHeader.Thread, this.LastRes, cookies);
				}

				if (_key != null && !string.IsNullOrEmpty(_key.Value)) {
					return this.SendMessge(makeSendCommandXML(chat, _threadHeader, _key));
				}
			}

			return false;
		}

		private string makeSendCommandXML(Chat chat, ThreadHeader threadHeader, PostKey postkey)
		{
			string message = Utility.Sanitizing(chat.Message.Replace("\r\n", "\n"));
			string command = Utility.Sanitizing(chat.Mail);

			return String.Format(
					 "<chat thread=\"{0}\" ticket=\"{1}\" vpos=\"{2}\" postkey=\"{3}\" mail=\"{4}\" user_id=\"{5}\" premium=\"{6}\">{7}</chat>\0",
					 threadHeader.Thread,
					 threadHeader.Ticket,
					 chat.Vpos,
					 postkey.Value,
					 command,
					 chat.UserId,
					 chat.Premium,
					 message);
		}

		

	}

	/// <summary>
	/// コメントの送信結果を受け取った際に発生するイベントの引数
	/// </summary>
	public class ReceiveSendResultEventArgs : EventArgs
	{
		private readonly ChatResult chatResult;

		public ReceiveSendResultEventArgs(ChatResult chatResult)
		{
			this.chatResult = chatResult;
		}

		/// <summary>
		/// コメントの送信結果を取得します
		/// </summary>
		public ChatResult ChatResult
		{
			get { return this.chatResult; }
		}

	}

}
