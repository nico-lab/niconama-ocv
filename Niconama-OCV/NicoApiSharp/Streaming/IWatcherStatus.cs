using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming
{

	/// <summary>
	/// 放送を視聴する人の情報
	/// </summary>
	public interface IWatcherStatus : IAccountInfomation
	{
		/// <summary>
		/// 座席名を取得する
		/// </summary>
		string RoomLabel { get; }

		/// <summary>
		/// 座席番号
		/// </summary>
		int SheetNo { get; }

		/// <summary>
		/// 放送主かどうか
		/// </summary>
		bool IsOwner { get; }

	}
}
