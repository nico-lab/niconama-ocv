using System;
using System.Collections.Generic;
using System.Text;

namespace NCSPlugin
{

	/// <summary>
	/// NGのチェック結果をあらわすインターフェース
	/// NG判定を行うビューアはIChatインターフェースと一緒にこのインターフェースも実装して、キャストできるようにしておく。
	/// ビューアがNG判定を持たない場合は実装する必要は無い
	/// </summary>
	public interface IFilterdChat: IChat
	{
		/// <summary>
		/// NGの種類
		/// </summary>
		NGType NgType { get; }

		/// <summary>
		/// NGの原因
		/// </summary>
		string NgSource { get; }
	}
}
