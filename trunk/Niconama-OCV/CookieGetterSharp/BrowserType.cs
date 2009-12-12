using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// �u���E�U�̎��
	/// </summary>
	public enum BrowserType
	{
		/// <summary>
		/// IE�n�u���E�U(IEComponet + IESafemode)
		/// </summary>
		IE,

		/// <summary>
		/// XP��IE��g���C�f���g�G���W�����g�p���Ă���u���E�U
		/// </summary>
		IEComponet,

		/// <summary>
		/// Vista�ȍ~��IE
		/// </summary>
		IESafemode,

		/// <summary>
		/// Firefox
		/// </summary>
		Firefox3,

		/// <summary>
		/// Google Chrome
		/// </summary>
		GoogleChrome3,

		/// <summary>
		/// Opera10
		/// </summary>
		Opera10,

		/// <summary>
		/// Safari4
		/// </summary>
		Safari4,

		/// <summary>
		/// Lunascape5 Gecko�G���W��
		/// </summary>
		Lunascape5Gecko,

		/// <summary>
		/// Lunascape6 Gecko�G���W��
		/// </summary>
		Lunascape6Gecko
	}
}
