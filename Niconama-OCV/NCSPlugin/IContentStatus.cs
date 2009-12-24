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
		/// IDを取得します
		/// </summary>
		string Id { get; }

		/// <summary>
		/// タイトルを取得します
		/// </summary>
		string Title { get; }

		/// <summary>
		/// サムネイルのURLを取得します
		/// </summary>
		string ThumbnailUrl { get; }

		/// <summary>
		/// 説明文を取得します
		/// </summary>
		string Description { get; }
	}
}
