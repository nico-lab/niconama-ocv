using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{

  /// <summary>
  /// エラー情報を通知するためのインターフェース
  /// </summary>
	public interface IErrorData
	{
		string ErrorCode { get; }
		string ErrorMessage { get; }
		bool HasError { get; }
	}
}
