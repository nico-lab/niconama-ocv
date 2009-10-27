using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	public interface ILiveCountStatus
	{
		int WatchCount { get;}
		int CommentCount { get;}
	}
}
