using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// チャットを受信した際に発生するイベントの引数
	/// </summary>
	[Serializable]
	public class ReceiveChatEventArgs : EventArgs
	{
		readonly IChat _chat;

		/// <summary>
		/// 受信したチャット
		/// </summary>
		public IChat Chat
		{
			get { return _chat; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="chat"></param>
		public ReceiveChatEventArgs(IChat chat)
		{
			_chat = chat;
		}
	}


	/// <summary>
	/// コンテンツデータを受け取ったときに発生するイベント
	/// </summary>
	[Serializable]
	public class ReceiveContentStatusEventArgs : EventArgs
	{
		readonly IContentStatus _status;

		/// <summary>
		/// コンテンツデータ
		/// </summary>
		public IContentStatus Status
		{
			get { return _status; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="status"></param>
		public ReceiveContentStatusEventArgs(IContentStatus status)
		{
			_status = status;
		}
	}
	
}
