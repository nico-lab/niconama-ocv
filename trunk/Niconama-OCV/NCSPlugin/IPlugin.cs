using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// プラグインのインターフェース
	/// version 0
	/// </summary>
	public interface IPlugin : IDisposable
	{

		/// <summary>
		/// プラグインの名前
		/// </summary>
		string Name { get;}

		/// <summary>
		/// プラグインが実装しているインターフェースのバージョン
		/// </summary>
		int InterfaceVersion { get; }

		/// <summary>
		/// プラグインの説明
		/// </summary>
		string Description { get;}

		/// <summary>
		/// ホストアプリケーション開始時に実行される
		/// </summary>
		/// <param name="host"></param>
		void Initialize(IPluginHost host);

		/// <summary>
		/// メニューから項目が選択されると実行されます。
		/// </summary>
		void Run();

		/// <summary>
		/// 取得開始ボタンが押された際に実行されます。
		/// </summary>
		/// <param name="liveId">接続した放送枠のID</param>
		/// <param name="startTime">接続した放送枠の開始時間</param>
		/// <param name="commentCount">接続時までのコメント数</param>
		void OnLiveStart(string liveId, DateTime startTime, int commentCount);

		/// <summary>
		/// 放送が終了したり、停止ボタンが押された際に実行されます。
		/// </summary>
		void OnLiveStop();

		/// <summary>
		/// 受信したチャットを処理する
		/// </summary>
		/// <param name="chat"></param>
		void OnComment(IChat chat);



	}
}
