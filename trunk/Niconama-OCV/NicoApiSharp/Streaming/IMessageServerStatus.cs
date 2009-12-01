using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming
{

	/// <summary>
	/// メッセージサーバーにアクセスするための情報を表すインターフェース
	/// </summary>
	public interface IMessageServerStatus
	{

		/// <summary>
		/// メッセージサーバーのアドレス
		/// </summary>
		string Address { get; }

		/// <summary>
		/// メッセージサーバーのポート番号
		/// </summary>
		int Port { get; }

		/// <summary>
		/// スレッド番号
		/// </summary>
		int Thread { get; }
	}
}
