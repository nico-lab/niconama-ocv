using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// �r���[�̍��ڂ�ǉ����邽�߂̃C���^�[�t�F�[�X
	/// </summary>
	public interface IColumnExtention : IComparer<IChat>
	{

		/// <summary>
		/// �ǉ������f�[�^�O���b�h�r���[�̃J����
		/// </summary>
		DataGridViewColumn Column { get; }

		/// <summary>
		/// �Z���̒l���K�v�ɂȂ����Ƃ��ɌĂяo����܂�
		/// �Ή����镶����Ȃǂ�Ԃ��Ă�������
		/// �Ȃ��r���[�A�͉��z���[�h�œ������Ƃ�O��ɂ��Ă���܂�
		/// </summary>
		/// <param name="chat">�Y������Z�����܂܂�Ă���s�Ɋ��蓖�Ă��Ă���`���b�g</param>
		/// <returns>�Z���ɑ�������l</returns>
		object OnCellValueNeeded(IChat chat);
	}
}
