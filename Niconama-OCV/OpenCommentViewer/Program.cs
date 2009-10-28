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

			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			System.Threading.Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(Program_UnhandledException);

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

			Logger.Default.Save("log.txt");
		}

		/// <summary>
		/// 処理されなかった例外をハンドルする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			Logger.Default.LogException(e.Exception);
			Logger.Default.Save("ThreadException.txt");
			Application.Exit();
		}

		/// <summary>
		/// 上記のメイン・スレッド以外のコンテキスト上で発生した例外をハンドルする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null) {
				Logger.Default.LogException(ex);
				Logger.Default.Save("UnhandledException.txt");
			}
			Application.Exit();
		}
	}
}
