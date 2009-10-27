using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Control {

  public interface IMainView : NCSPlugin.IPlugin {

		string IdBoxText { get; set; }
		string Text { get; set; }
		void ShowStatusMessage(string message);
		void ShowFatalMessage(string message);

		void RegisterColumnExtention(NCSPlugin.IColumnExtention columnExtention);
		void RegisterContextMenuExtention(NCSPlugin.IContextMenuExtention contextMenuExtention);
		void RegisterMenuStripExtention(NCSPlugin.IMenuStripExtention menuStripExtention);

  }
}
