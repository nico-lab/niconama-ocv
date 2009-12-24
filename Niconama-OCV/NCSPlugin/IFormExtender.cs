using System;
using System.Windows.Forms;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// �R���e�N�X�g���j���[���J���ꂽ�ۂɌĂяo�����f���Q�[�g
	/// </summary>
	/// <param name="chat">�J���ꂽ�Ƃ��ɑI������Ă����`���b�g</param>
	public delegate void ContextmenuOpeningCallback(IChat chat);

	/// <summary>
	/// �O������t�H�[�����g�����邽�߂̃C���^�[�t�F�[�X
	/// </summary>
	public interface IFormExtender
	{

		/// <summary>
		/// ���j���[�X�g���b�v���g�����܂��B
		/// </summary>
		/// <param name="category">�t�@�C���A�ҏW�A�\���Ȃǂ̃��j���[�̕��ނ��w�肷��B�Ȃ�ׂ���ʓI�Ȃ��̂ɂ��邱��</param>
		/// <param name="menuItem"></param>
		/// <param name="openingCallback">�R���e�N�X�g���j���[���J���ꂽ�ۂɎ��s�����R�[���o�b�N�֐�</param> 
		void AddMenuStripItem(string category, System.Windows.Forms.ToolStripMenuItem menuItem, ContextmenuOpeningCallback openingCallback);

		/// <summary>
		/// �c�[���X�g���b�v���g�����܂��B
		/// </summary>
		/// <param name="toolstripItem"></param>
		void AddToolStripItem(ToolStripItem toolstripItem);
		
		/// <summary>
		/// �X�e�[�^�X�X�g���b�v���g�����܂��B
		/// </summary>
		/// <param name="toolstripItem"></param>
		void AddStatusStripItem(ToolStripItem toolstripItem);

		/// <summary>
		/// ���[�U�[�R���g���[����ǉ����܂�
		/// </summary>
		/// <param name="userControl"></param>
		/// <param name="dockStyle"></param>
		void AddUserControl(UserControl userControl, DockStyle dockStyle);

		
	}
}
