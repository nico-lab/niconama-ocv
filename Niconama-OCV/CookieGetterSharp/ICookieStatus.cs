using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// Path��File��Directory�̂ǂ���������Ă��邩��\��
	/// </summary>
	public enum PathType {
 
		/// <summary>
		/// �t�@�C��
		/// </summary>
		File,

		/// <summary>
		/// �f�B���N�g��
		/// </summary>
		Directory
	}

	/// <summary>
	/// �N�b�L�[�Q�b�^�[�̏�Ԃ�\���C���^�[�t�F�[�X
	/// </summary>
	public interface ICookieStatus
	{
		/// <summary>
		/// �u���E�U�̎�ނ��擾����
		/// </summary>
		BrowserType BrowserType { get; }

		/// <summary>
		/// ���ʖ����擾����
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ���p�\���ǂ������擾����
		/// </summary>
		bool IsAvailable { get; }
		
		/// <summary>
		/// �N�b�L�[���ۑ�����Ă���t�H���_���擾�A�ݒ肷��
		/// </summary>
		string CookiePath { get; set; }

		/// <summary>
		/// CookiePath��File��\���̂��ADirectory��\���̂����擾����
		/// </summary>
		PathType PathType { get; }

	}
}
