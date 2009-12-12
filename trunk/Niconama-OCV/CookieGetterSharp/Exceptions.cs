using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{

	/// <summary>
	/// �N�b�L�[�擾�Ɋւ����O
	/// </summary>
	[global::System.Serializable]
	public class CookieGetterException : Exception
	{
		/// <summary>
		/// �N���X�̐V�����C���X�^���X�����������܂��B
		/// </summary>
		public CookieGetterException() { }

		/// <summary>
		/// �w�肵���G���[ ���b�Z�[�W���g�p���āASystem.Exception �N���X�̐V�����C���X�^���X�����������܂��B
		/// </summary>
		/// <param name="message">�G���[��������郁�b�Z�[�W�B</param>
		public CookieGetterException(string message) : base(message) { }

		/// <summary>
		/// �w�肵���G���[ ���b�Z�[�W�ƁA���̗�O�̌����ł��������O�ւ̎Q�Ƃ��g�p���āASystem.Exception �N���X�̐V�����C���X�^���X�����������܂��B
		/// </summary>
		/// <param name="message">��O�̌������������G���[ ���b�Z�[�W�B</param>
		/// <param name="inner">���݂̗�O�̌����ł����O�B������O���w�肳��Ă��Ȃ��ꍇ�́Anull �Q�� (Visual Basic �̏ꍇ�� Nothing)�B</param>
		public CookieGetterException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// �V���A���������f�[�^���g�p���āASystem.Exception �N���X�̐V�����C���X�^���X�����������܂��B
		/// </summary>
		/// <param name="info">�X���[����Ă����O�Ɋւ���V���A�����ς݃I�u�W�F�N�g �f�[�^��ێ����Ă��� System.Runtime.Serialization.SerializationInfo�B</param>
		/// <param name="context">�]�����܂��͓]����Ɋւ���R���e�L�X�g�����܂�ł��� System.Runtime.Serialization.StreamingContext�B</param>
		protected CookieGetterException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}


}
