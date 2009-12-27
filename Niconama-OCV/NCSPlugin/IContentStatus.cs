using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// �R���e���c���
	/// </summary>
	public interface IContentStatus
	{
		/// <summary>
		/// �����ɐڑ������ǂ������擾���܂��B
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// �����傩�ǂ������擾���܂��B
		/// </summary>
		bool IsOwner { get; }

		/// <summary>
		/// �v���~�A��������ǂ������擾���܂��B
		/// </summary>
		bool IsPremium { get; }

		/// <summary>
		/// ����ID���擾���܂�
		/// </summary>
		string Id { get;}

		/// <summary>
		/// ���������擾���܂�
		/// </summary>
		string Title { get;}

		/// <summary>
		/// �R�~���j�e�BID���擾���܂�
		/// </summary>
		string CommunityId { get;}

		/// <summary>
		/// �R�~���j�e�B�����擾���܂�
		/// </summary>
		string CommunityName { get;}

		/// <summary>
		/// ���[�J��PC��ł̕����J�n����
		/// </summary>
		DateTime LocalStartTime { get; }

		/// <summary>
		/// �T�[�o�[��ł̕����J�n����
		/// </summary>
		DateTime ServerStartTime { get; }
	}
}
