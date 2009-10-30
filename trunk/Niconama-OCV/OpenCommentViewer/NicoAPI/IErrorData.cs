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
		/// <summary>
		/// サーバーから送られてきたエラーコード
		/// </summary>
		string ErrorCode { get; }

		/// <summary>
		/// エラーコードの意味
		/// </summary>
		string ErrorMessage { get; }

		/// <summary>
		/// エラーがあるかどうか
		/// </summary>
		bool HasError { get; }
	}
}
