using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hal.OpenCommentViewer
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

			Hal.OpenCommentViewer.Control.Core c = new Hal.OpenCommentViewer.Control.Core();
			Hal.OpenCommentViewer.Control.MainForm m = new Hal.OpenCommentViewer.Control.MainForm();
			c.SetMainView(m);

			for (int i = 0; i < args.Length; i++) {
				string id = Hal.OpenCommentViewer.Utility.GetLiveIdFromUrl(args[i]);
				if (id != null) {
					c.Reserve(id);
					break;
				}
			}

			Application.Run(m);
			if (NicoApiSharp.Logger.Default.HasErrorLog) {
				NicoApiSharp.Logger.Default.Save("log.txt");
			}
		}

		/// <summary>
		/// 処理されなかった例外をハンドルする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			NicoApiSharp.Logger.Default.LogException(e.Exception);
			NicoApiSharp.Logger.Default.Save("ThreadException.txt");
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
				NicoApiSharp.Logger.Default.LogException(ex);
				NicoApiSharp.Logger.Default.Save("UnhandledException.txt");
			}
		}
	}
}
