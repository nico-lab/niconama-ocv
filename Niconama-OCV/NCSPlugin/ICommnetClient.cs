using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	
	/// <summary>
	/// ���͂��ꂽID����Y������R�����g���擾����@�\��\���C���^�[�t�F�[�X
	/// ������ICommnetClient����`����Ă���ꍇ��Priority���������̂����ۂ̏�����S������
	/// </summary>
	public interface ICommnetClient
	{
		/// <summary>
		/// �R�����g�擾���������ĕ\�����鏀�����ł����Ƃ��ɔ�������C�x���g
		/// </summary>
		event EventHandler ConnectedServer;

		/// <summary>
		/// �R�����g�擾���I�������Ƃ��ɔ�������C�x���g
		/// </summary>
		event EventHandler DisconnectedServer;

		/// <summary>
		/// �R�����g���擾�����Ƃ��ɔ�������C�x���g
		/// </summary>
		event EventHandler<ReceiveChatEventArgs> ReceiveChat;

		/// <summary>
		/// �����̃R�����g���擾�����Ƃ��ɔ�������C�x���g
		/// �R�����g�������ꍇ��ReceiveChat�C�x���g���������p�t�H�[�}���X���o��
		/// </summary>
		event EventHandler<ReceiveChatsEventArgs> ReceiveChats;

		/// <summary>
		/// �R���e���c�Ɋ֘A�����񂪎擾�ł����ۂɔ�������C�x���g
		/// </summary>
		event EventHandler<ReceiveContentStatusEventArgs> ReceiveContentStatus;

		/// <summary>
		/// ���l���傫���قǗD��I�ɏ��������
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// �w�肳�ꂽID���������邱�Ƃ��o���邩
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool CanHandle(string id);

		/// <summary>
		/// �ڑ�
		/// </summary>
		/// <param name="id"></param>
		void Connect(string id);

		/// <summary>
		/// �ؒf
		/// </summary>
		void Disconnect();
	}
}
