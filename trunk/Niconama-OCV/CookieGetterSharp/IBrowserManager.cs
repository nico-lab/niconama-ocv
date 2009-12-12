using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// CookieGetter�𐶐����邽�߂̃C���^�[�t�F�[�X
	/// </summary>
	interface IBrowserManager
	{
		/// <summary>
		/// �u���E�U�̎��
		/// </summary>
		BrowserType BrowserType { get; }

		/// <summary>
		/// �����CookieGetter���擾���܂�
		/// </summary>
		/// <returns></returns>
		ICookieGetter CreateDefaultCookieGetter();

		/// <summary>
		/// ���p�\�Ȃ��ׂĂ�CookieGetter���擾���܂�
		/// </summary>
		/// <returns></returns>
		ICookieGetter[] CreateCookieGetters();
		
	}
}
