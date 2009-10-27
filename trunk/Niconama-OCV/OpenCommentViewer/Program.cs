using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenCommentViewer
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			OpenCommentViewer.Control.Core c = new OpenCommentViewer.Control.Core();
			OpenCommentViewer.Control.MainForm m = new OpenCommentViewer.Control.MainForm();
			c.SetMainView(m);

			for (int i = 0; i < args.Length; i++) {
				string id = OpenCommentViewer.Utility.GetLiveIdFromUrl(args[i]);
				if (id != null) {
					c.Reserve(id);
					break;
				}
			}

			Application.Run(m);

			OpenCommentViewer.Logger.Default.Save("log.txt");
		}
	}
}
