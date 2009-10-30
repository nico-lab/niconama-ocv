using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.NicoAPI
{

	/// <summary>
	/// 放送に関する詳細情報を表すインターフェース
	/// </summary>
	public interface ILiveDescription
	{

		/// <summary>
		/// 放送ID
		/// </summary>
		string LiveId { get; }

		/// <summary>
		/// コミュニティID
		/// </summary>
		string CommunityId { get; }

		/// <summary>
		/// 放送のタイトル
		/// </summary>
		string LiveName { get; }

		/// <summary>
		/// コミュニティ名
		/// </summary>
		string CommunityName { get; }

		/// <summary>
		/// 放送者名
		/// </summary>
		string Caster { get; }

	}
}
