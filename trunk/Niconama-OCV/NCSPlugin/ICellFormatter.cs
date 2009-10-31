using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// �Z���̃t�H�[�}�b�e�B���O�̍ۂɌĂяo�����f���Q�[�g
	/// </summary>
	/// <param name="chat"></param>
	/// <param name="e"></param>
	public delegate void CellFormattingCallback(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e);

	/// <summary>
	/// �r���[�̃Z���̃f�U�C����ύX���邽�߂̃C���^�[�t�F�[�X
	/// �s�ɐF��������A�Z���̒l��ύX������ł���
	/// </summary>
	public interface ICellFormatter
	{

		/// <summary>
		/// �Z���̃t�H�[�}�b�e�B���O���K�v�ɂȂ����Ƃ��Ăяo�����
		/// </summary>
		/// <param name="chat"></param>
		/// <param name="e"></param>
		void OnCellFormatting(NCSPlugin.IChat chat, System.Windows.Forms.DataGridViewCellFormattingEventArgs e);
	}
}
