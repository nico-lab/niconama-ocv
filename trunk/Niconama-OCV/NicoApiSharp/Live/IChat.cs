using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Live
{
	/// <summary>
	/// �R�����g��\������C���^�[�t�F�[�X
	/// </summary>
	public interface IChat
	{
		/// <summary>
		///  ������
		/// </summary>
		bool Anonymity { get; }

		/// <summary>
		/// ���e����
		/// </summary>
		DateTime Date { get; }

		/// <summary>
		///  �R�}���h
		/// </summary>
		string Mail { get; }

		/// <summary>
		///  �R�����g
		/// </summary>
		string Message { get; }

		/// <summary>
		/// �R�����g�ԍ�
		/// </summary>
		int No { get; }

		/// <summary>
		/// ���e�҂̑���������킷���l
		/// </summary>
		int Premium { get; }

		/// <summary>
		/// 
		/// </summary>
		int Thread { get; }

		/// <summary>
		/// ���[�U�[ID
		/// </summary>
		string UserId { get; }

		/// <summary>
		///  �R�����g�ʒu
		/// </summary>
		int Vpos { get; }

		/// <summary>
		/// ������̃R�����g���ǂ���
		/// </summary>
		bool IsOwnerComment { get; }
	}
}
