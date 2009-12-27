using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// コンテンツ情報
	/// </summary>
	public interface IContentStatus
	{
		/// <summary>
		/// 放送に接続中かどうかを取得します。
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// 放送主かどうかを取得します。
		/// </summary>
		bool IsOwner { get; }

		/// <summary>
		/// プレミアム会員かどうかを取得します。
		/// </summary>
		bool IsPremium { get; }

		/// <summary>
		/// 放送IDを取得します
		/// </summary>
		string Id { get;}

		/// <summary>
		/// 放送名を取得します
		/// </summary>
		string Title { get;}

		/// <summary>
		/// コミュニティIDを取得します
		/// </summary>
		string CommunityId { get;}

		/// <summary>
		/// コミュニティ名を取得します
		/// </summary>
		string CommunityName { get;}

		/// <summary>
		/// ローカルPC上での放送開始時刻
		/// </summary>
		DateTime LocalStartTime { get; }

		/// <summary>
		/// サーバー上での放送開始時刻
		/// </summary>
		DateTime ServerStartTime { get; }
	}
}
