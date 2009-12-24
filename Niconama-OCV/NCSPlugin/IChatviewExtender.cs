using System;
using System.Windows.Forms;

namespace Hal.NCSPlugin
{
	
	/// <summary>
	/// �`���b�g�r���[�̃C�x���g�������f���Q�[�g
	/// </summary>
	/// <param name="chat">�Ώۂ̍s�ɑ�����`���b�g</param>
	public delegate void ChatviewEventHandler(IChat chat);

	/// <summary>
	/// �`���b�g�r���[�̃C�x���g�������f���Q�[�g
	/// </summary>
	/// <typeparam name="T">�f�[�^�O���b�h�r���[�̍s�̓�����`����C�x���g</typeparam>
	/// <param name="chat">�Ώۂ̍s�ɑ�����`���b�g</param>
	/// <param name="e">���C�x���g�̈���</param>
	public delegate void ChatviewEventHandler<T>(IChat chat, T e);

	/// <summary>
	/// �Z���ɒl��񋟂��郁�\�b�h��\��
	/// </summary>
	/// <param name="chat"></param>
	/// <returns></returns>
	public delegate object CellValueProvision(IChat chat);

	/// <summary>
	/// �`���b�g�r���[���g�����邽�߂̃C���^�[�t�F�[�X
	/// </summary>
	public interface IChatviewExtender
	{
		/// <summary>
		/// �s�̍��������肷��K�v������ۂɔ�������C�x���g
		/// </summary>
		event ChatviewEventHandler<DataGridViewRowHeightInfoNeededEventArgs> RowHeightInfoNeeded;

		/// <summary>
		/// �c�[���X�g���b�v�e�L�X�g���K�v�ɂȂ����ۂɔ�������C�x���g
		/// </summary>
		event ChatviewEventHandler<DataGridViewCellToolTipTextNeededEventArgs> CellToolTipTextNeeded;

		/// <summary>
		/// �Z�����t�H�[�}�b�e�B���O����K�v�����鎞�ɔ�������C�x���g
		/// </summary>
		event ChatviewEventHandler<DataGridViewCellFormattingEventArgs> CellFormatting;

		/// <summary>
		/// �s���_�u���N���b�N���ꂽ�ۂɔ�������C�x���g
		/// </summary>
		event ChatviewEventHandler<DataGridViewCellMouseEventArgs> DoubleClicked;
		
		/// <summary>
		/// �J�����̕����ς�����ۂɔ�������C�x���g
		/// </summary>
		event EventHandler<DataGridViewColumnEventArgs> ColumnWidthChanged;

		/// <summary>
		/// �\�[�g���ꂽ�ۂɔ�������C�x���g
		/// </summary>
		event EventHandler Sorted;

		/// <summary>
		/// �`���b�g�r���[�ɗ�g����ǉ����܂��B
		/// </summary>
		/// <param name="columnExtention">�ǉ������g��</param>
		void AddColumnExtention(IColumnExtention columnExtention);

		/// <summary>
		/// �`���b�g�r���[�ɗ��ǉ����A���̓�����f���Q�[�g�ɂ�萧�䂵�܂��B
		/// </summary>
		/// <param name="column">�ǉ������</param>
		/// <param name="provision">�Z���̒l���K�v�ɂȂ����Ƃ��ɌĂяo����郁�\�b�h</param>
		/// <param name="comparison">���ёւ����K�v�ɂȂ����Ƃ��ɌĂяo����郁�\�b�h</param>
		void AddColumn(DataGridViewColumn column, CellValueProvision provision, Comparison<IChat> comparison);

		/// <summary>
		/// �R���e�N�X�g���j���[���g�����܂�
		/// </summary>
		/// <param name="menuItem"></param>
		/// <param name="openingCallback">�R���e�N�X�g���j���[���J���ꂽ�ۂɎ��s�����R�[���o�b�N�֐�</param>
		void AddContextMenu(ToolStripMenuItem menuItem, ContextmenuOpeningCallback openingCallback);

		/// <summary>
		/// �J�����G�N�X�e���V������Z���t�H�[�}�b�^�[�ȂǂŒl���ύX����A�r���[�S�̂��X�V����K�v������Ƃ��ɌĂяo���܂�
		/// </summary>
		void UpdateCellValues();
	}
}
