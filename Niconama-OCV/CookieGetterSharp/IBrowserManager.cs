using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// �N�b�L�[���ۑ�����Ă���p�X����������\�͂�\���C���^�[�t�F�[�X
	/// </summary>
	interface IBrowserManager
	{
		/// <summary>
		/// �u���E�U�̎��
		/// </summary>
		CookieGetter.BROWSER_TYPE BrowserType { get; }

		/// <summary>
		/// ����̃X�e�[�^�X���擾���܂�
		/// </summary>
		/// <returns></returns>
		IBrowserStatus GetDefaultStatus();

		/// <summary>
		/// ���p�\�Ȃ��ׂẴX�e�[�^�X���擾���܂�
		/// </summary>
		/// <returns></returns>
		IBrowserStatus[] GetStatus();
		
	}
}
