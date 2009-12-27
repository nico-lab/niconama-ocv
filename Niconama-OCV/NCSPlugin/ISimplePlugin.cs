using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// �ȈՃv���O�C���̃C���^�[�t�F�[�X
	/// nwhois�̃v���O�C���Ɠ����̔\�͂������
	/// </summary>
	public interface ISimplePlugin : IPlugin
	{
		/// <summary>
		/// ���j���[���獀�ڂ��I�������Ǝ��s����܂��B
		/// </summary>
		void Run();

		/// <summary>
		/// �擾�J�n�{�^���������ꂽ�ۂɎ��s����܂��B
		/// </summary>
		/// <param name="liveId">�ڑ����������g��ID</param>
		/// <param name="startTime">�ڑ����������g�̊J�n����</param>
		/// <param name="commentCount">�ڑ����܂ł̃R�����g��</param>
		void OnLiveStart(string liveId, DateTime startTime, int commentCount);

		/// <summary>
		/// �������I��������A��~�{�^���������ꂽ�ۂɎ��s����܂��B
		/// </summary>
		void OnLiveStop();

		/// <summary>
		/// ��M�����`���b�g����������
		/// </summary>
		/// <param name="chat"></param>
		void OnComment(IChat chat);
	}
}
