using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// 簡易プラグインのインターフェース
	/// nwhoisのプラグインと同等の能力を備える
	/// </summary>
	public interface ISimplePlugin : IPlugin
	{
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
