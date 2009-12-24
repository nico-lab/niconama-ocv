using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// �R���e���c���
	/// </summary>
	public interface IContentStatus
	{
		/// <summary>
		/// ID���擾���܂�
		/// </summary>
		string Id { get; }

		/// <summary>
		/// �^�C�g�����擾���܂�
		/// </summary>
		string Title { get; }

		/// <summary>
		/// �T���l�C����URL���擾���܂�
		/// </summary>
		string ThumbnailUrl { get; }

		/// <summary>
		/// ���������擾���܂�
		/// </summary>
		string Description { get; }
	}
}
