using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// �R�����g�T�[�o�[�Ƃ̒ʐM���󂯎���
	/// </summary>
	public interface ICommnetAgent
	{
		/// <summary>
		/// �R�����g���擾�����Ƃ��ɔ�������C�x���g
		/// </summary>
		event EventHandler<ReceiveChatEventArgs> ReceiveChat;

		/// <summary>
		/// �R�����g�擾���������ĕ\�����鏀�����ł����Ƃ��ɔ�������C�x���g
		/// </summary>
		event EventHandler ConnectedServer;

		/// <summary>
		/// �R�����g�擾���I�������Ƃ��ɔ�������C�x���g
		/// </summary>
		event EventHandler DisconnectedServer;

		/// <summary>
		/// �R���e���c�Ɋ֘A�����񂪎擾�ł����ۂɔ�������C�x���g
		/// </summary>
		event EventHandler<ReceiveContentStatusEventArgs> ReceiveContentStatus;

		/// <summary>
		/// ��ʃR�����g�����e�\���ǂ������擾���܂��B
		/// �R�����g���e�@�\����������Ă��Ȃ��ꍇ�͏��false���Ԃ����
		/// </summary>
		bool CanPostComment { get; }

		/// <summary>
		/// �^�c�R�����g�����e�\���ǂ������擾���܂��B
		/// �^�c�R�����g���e�@�\����������Ă��Ȃ��ꍇ�͏��false���Ԃ����
		/// </summary>
		bool CanPostOwnerComment { get; }

		/// <summary>
		/// ��ʃR�����g�𓊍e���e���܂��B
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		void PostComment(string message, string command);

		/// <summary>
		/// �^�c�҃R�����g�𓊍e���܂��B
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		bool PostOwnerComment(string message, string command);

		/// <summary>
		/// �^�c�҃R�����g�𓊍e���܂��B
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		bool PostOwnerComment(string message, string command, string name);

		/// <summary>
		/// �����ɐڑ����܂��B
		/// </summary>
		/// <param name="id">�Ώۂ̕���URL�A�܂��͕���ID</param>
		/// <returns>�ڑ��ł�����</returns>
		bool Connect(string id);

		/// <summary>
		/// �T�[�o�[�ʐM�𒆒f���܂��B
		/// </summary>
		void Disconnect();
	}
}
