using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.NicoAPI
{
	/// <summary>
	/// 現在の総来場者数、コメント数をあらわすインターフェース
	/// </summary>
	public interface ILiveCountStatus
	{
		/// <summary>
		/// 総来場者数
		/// </summary>
		int WatchCount { get;}

		/// <summary>
		/// コメント数
		/// </summary>
		int CommentCount { get;}
	}
}
