using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// �`���b�g����M�����ۂɔ�������C�x���g�̈���
	/// </summary>
	[Serializable]
	public class ReceiveChatEventArgs : EventArgs
	{
		readonly IChat _chat;

		/// <summary>
		/// ��M�����`���b�g
		/// </summary>
		public IChat Chat
		{
			get { return _chat; }
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="chat"></param>
		public ReceiveChatEventArgs(IChat chat)
		{
			_chat = chat;
		}
	}

	/// <summary>
	/// �����̃R�����g���󂯎�����Ƃ��ɔ�������C�x���g�̈���
	/// </summary>
	[Serializable]
	public class ReceiveChatsEventArgs : EventArgs
	{
		readonly IChat[] _chats;

		/// <summary>
		/// ��M�����`���b�g
		/// </summary>
		public IChat[] Chats
		{
			get { return _chats; }
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="chats"></param>
		public ReceiveChatsEventArgs(IChat[] chats)
		{
			_chats = chats;
		}
	}

	/// <summary>
	/// �R���e���c�f�[�^���󂯎�����Ƃ��ɔ�������C�x���g
	/// </summary>
	[Serializable]
	public class ReceiveContentStatusEventArgs : EventArgs
	{
		readonly IContentStatus _status;

		/// <summary>
		/// �R���e���c�f�[�^
		/// </summary>
		public IContentStatus Status
		{
			get { return _status; }
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="status"></param>
		public ReceiveContentStatusEventArgs(IContentStatus status)
		{
			_status = status;
		}
	}
	
}
