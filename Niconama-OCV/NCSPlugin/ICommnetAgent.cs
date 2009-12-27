using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// コメントサーバーとの通信を受け持つ
	/// </summary>
	public interface ICommnetAgent
	{
		/// <summary>
		/// コメントを取得したときに発生するイベント
		/// </summary>
		event EventHandler<ReceiveChatEventArgs> ReceiveChat;

		/// <summary>
		/// コメント取得が完了して表示する準備ができたときに発生するイベント
		/// </summary>
		event EventHandler ConnectedServer;

		/// <summary>
		/// コメント取得が終了したときに発生するイベント
		/// </summary>
		event EventHandler DisconnectedServer;

		/// <summary>
		/// コンテンツに関連する情報が取得できた際に発生するイベント
		/// </summary>
		event EventHandler<ReceiveContentStatusEventArgs> ReceiveContentStatus;

		/// <summary>
		/// 一般コメントが投稿可能かどうかを取得します。
		/// コメント投稿機能が実装されていない場合は常にfalseが返される
		/// </summary>
		bool CanPostComment { get; }

		/// <summary>
		/// 運営コメントが投稿可能かどうかを取得します。
		/// 運営コメント投稿機能が実装されていない場合は常にfalseが返される
		/// </summary>
		bool CanPostOwnerComment { get; }

		/// <summary>
		/// 一般コメントを投稿投稿します。
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		void PostComment(string message, string command);

		/// <summary>
		/// 運営者コメントを投稿します。
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		bool PostOwnerComment(string message, string command);

		/// <summary>
		/// 運営者コメントを投稿します。
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		bool PostOwnerComment(string message, string command, string name);

		/// <summary>
		/// 放送に接続します。
		/// </summary>
		/// <param name="id">対象の放送URL、または放送ID</param>
		/// <returns>接続できたか</returns>
		bool Connect(string id);

		/// <summary>
		/// サーバー通信を中断します。
		/// </summary>
		void Disconnect();
	}
}
