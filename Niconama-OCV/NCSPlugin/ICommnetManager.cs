using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	
	/// <summary>
	/// ���͂��ꂽID����Y������R�����g���擾����@�\��\���C���^�[�t�F�[�X
	/// ������ICommnetClient����`����Ă���ꍇ��Priority���������̂����ۂ̏�����S������
	/// </summary>
	public interface ICommnetManager : ICommnetAgent, IContentStatus, IPlugin
	{

		/// <summary>
		/// ���l���傫���قǗD��I�ɏ��������
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// �w�肳�ꂽID���������邱�Ƃ��o���邩
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool CanHandle(string id);

		
	}
}
