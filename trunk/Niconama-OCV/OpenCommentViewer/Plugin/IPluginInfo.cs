using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Plugin
{
	interface IPluginInfo
	{
		string ClassName { get; }
		string Location { get; }
		Hal.NCSPlugin.IPlugin CreateInstance();
	}
}
