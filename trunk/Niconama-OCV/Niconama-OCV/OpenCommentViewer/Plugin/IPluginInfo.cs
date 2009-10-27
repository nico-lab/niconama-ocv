using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Plugin
{
	interface IPluginInfo
	{
		string ClassName { get; }
		string Location { get; }
		NCSPlugin.IPlugin CreateInstance();
	}
}
