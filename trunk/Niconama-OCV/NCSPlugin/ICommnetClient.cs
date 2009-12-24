using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	
	/// <summary>
	/// 入力されたIDから該当するコメントを取得する機能を表すインターフェース
	/// 複数のICommnetClientが定義されている場合はPriorityが高いものが実際の処理を担当する
	/// </summary>
	public interface ICommnetClient
	{
		/// <summary>
		/// コメント取得が完了して表示する準備ができたときに発生するイベント
		/// </summary>
		event EventHandler ConnectedServer;

		/// <summary>
		/// コメント取得が終了したときに発生するイベント
		/// </summary>
		event EventHandler DisconnectedServer;

		/// <summary>
		/// コメントを取得したときに発生するイベント
		/// </summary>
		event EventHandler<ReceiveChatEventArgs> ReceiveChat;

		/// <summary>
		/// 複数のコメントを取得したときに発生するイベント
		/// コメントが多い場合はReceiveChatイベントよりも高いパフォーマンスが出る
		/// </summary>
		event EventHandler<ReceiveChatsEventArgs> ReceiveChats;

		/// <summary>
		/// コンテンツに関連する情報が取得できた際に発生するイベント
		/// </summary>
		event EventHandler<ReceiveContentStatusEventArgs> ReceiveContentStatus;

		/// <summary>
		/// 数値が大きいほど優先的に処理される
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// 指定されたIDを処理することが出来るか
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool CanHandle(string id);

		/// <summary>
		/// 接続
		/// </summary>
		/// <param name="id"></param>
		void Connect(string id);

		/// <summary>
		/// 切断
		/// </summary>
		void Disconnect();
	}
}
