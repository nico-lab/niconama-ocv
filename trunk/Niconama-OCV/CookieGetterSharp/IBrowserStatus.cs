using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// �u���E�U�[����擾�\�ȃN�b�L�[�̏���\���C���^�[�t�F�[�X
	/// </summary>
	public interface IBrowserStatus
	{
		/// <summary>
		/// �u���E�U�̎��ʖ����擾����
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// �N�b�L�[���ۑ�����Ă���t�H���_���擾����
		/// </summary>
		string CookiePath { get; }

		/// <summary>
		/// �Ή�����u���E�U�p�̃N�b�L�[�Q�b�^�[���擾����
		/// </summary>
		/// <returns></returns>
		ICookieGetter CookieGetter{ get; }

	}
}
