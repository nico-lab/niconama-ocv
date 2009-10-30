using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// コメントを表現するインターフェース
	/// </summary>
	public interface IChat
	{
		/// <summary>
		///  匿名性
		/// </summary>
		bool Anonymity { get; }

		/// <summary>
		/// 投稿時刻
		/// </summary>
		DateTime Date { get; }

		/// <summary>
		///  コマンド
		/// </summary>
		string Mail { get; }

		/// <summary>
		///  コメント
		/// </summary>
		string Message { get; }

		/// <summary>
		/// コメント番号
		/// </summary>
		int No { get; }

		/// <summary>
		/// 投稿者の属性をあらわす数値
		/// </summary>
		int Premium { get; }

		/// <summary>
		/// 
		/// </summary>
		int Thread { get; }

		/// <summary>
		/// ユーザーID
		/// </summary>
		string UserId { get; }

		/// <summary>
		///  コメント位置
		/// </summary>
		int Vpos { get; }

		/// <summary>
		/// 放送主のコメントかどうか
		/// </summary>
		bool IsOwnerComment { get; }
	}
}
