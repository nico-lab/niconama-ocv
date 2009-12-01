using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming
{

	/// <summary>
	/// 放送に関する詳細情報を表すインターフェース
	/// </summary>
	public interface IDescription
	{

		/// <summary>
		/// 放送ID
		/// </summary>
		string Id { get; }
		
		/// <summary>
		/// 放送のタイトル
		/// </summary>
		string Title { get; }

		/// <summary>
		/// コミュニティID
		/// </summary>
		string CommunityId { get; }

		/// <summary>
		/// コミュニティ名
		/// </summary>
		string CommunityName { get; }

		/// <summary>
		/// 放送者名
		/// </summary>
		string Caster { get; }

		/// <summary>
		/// 詳細
		/// </summary>
		string Description { get; }

	}
}
