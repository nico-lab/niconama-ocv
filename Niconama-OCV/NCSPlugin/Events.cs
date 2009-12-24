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
	/// 複数のコメントを受け取ったときに発生するイベントの引数
	/// </summary>
	[Serializable]
	public class ReceiveChatsEventArgs : EventArgs
	{
		readonly IChat[] _chats;

		/// <summary>
		/// 受信したチャット
		/// </summary>
		public IChat[] Chats
		{
			get { return _chats; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="chats"></param>
		public ReceiveChatsEventArgs(IChat[] chats)
		{
			_chats = chats;
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
