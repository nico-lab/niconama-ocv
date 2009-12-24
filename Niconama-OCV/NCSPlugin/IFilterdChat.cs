using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// NGの種類
	/// </summary>
	public enum NGType
	{
		/// <summary>
		/// NGなし
		/// </summary>
		None,

		/// <summary>
		/// 単語
		/// </summary>
		Word,

		/// <summary>
		/// ユーザー
		/// </summary>
		Id,

		/// <summary>
		/// コマンド
		/// </summary>
		Command
	}

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
