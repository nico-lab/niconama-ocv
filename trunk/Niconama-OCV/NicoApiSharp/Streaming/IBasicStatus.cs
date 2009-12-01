using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming
{

	/// <summary>
	/// 放送の基本情報を表すインターフェース
	/// </summary>
	public interface IBasicStatus
	{

		/// <summary>
		/// 放送IDを取得する
		/// </summary>
		string Id { get; }

		/// <summary>
		/// コミュニティ情報を取得する
		/// </summary>
		string CommunityId { get; }

		/// <summary>
		/// サーバー上での放送開始時間を取得する
		/// </summary>
		DateTime StartTime { get; }

		/// <summary>
		/// PC上での放送開始時間を取得する
		/// </summary>
		DateTime LocalStartTime { get; }

		/// <summary>
		/// 座席名を取得する
		/// </summary>
		string RoomLabel { get; }


	}
}
