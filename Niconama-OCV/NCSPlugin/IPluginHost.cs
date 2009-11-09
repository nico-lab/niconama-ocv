using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// プラグインホストのインターフェース
	/// version 3
	/// </summary>
	public interface IPluginHost
	{

		#region アプリケーション情報

		/// <summary>
		/// プラグイン用データ保存フォルダーへのパスを取得します。
		/// </summary>
		string PluginDataFolder { get; }

		/// <summary>
		/// プラグインが格納されているフォルダーへのパスを取得します。
		/// </summary>
		string PluginsFolder { get; }

		/// <summary>
		/// PluginHostが実装しているインターフェースのバージョンを取得します。
		/// </summary>
		int InterfaceVersion { get; }

		/// <summary>
		/// アプリケーションの名前を取得します。
		/// </summary>
		string ApplicationName { get; }

		/// <summary>
		/// アプリケーションのバージョンを取得します。
		/// </summary>
		System.Version ApplicationVersion { get; }

		#endregion

		#region 放送情報

		/// <summary>
		/// 放送に接続中かどうかを取得します。
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// 放送主かどうかを取得します。
		/// </summary>
		bool IsOwner { get; }

		/// <summary>
		/// プレミアム会員かどうかを取得します。
		/// </summary>
		bool IsPremium { get; }

		/// <summary>
		/// 保存されているチャット一覧のコピーを取得します。
		/// </summary>
		IChat[] Chats { get;}

		/// <summary>
		/// 放送IDを取得します
		/// </summary>
		string LiveId { get;}

		/// <summary>
		/// 放送名を取得します
		/// </summary>
		string LiveName { get;}

		/// <summary>
		/// コミュニティIDを取得します
		/// </summary>
		string CommunityId { get;}

		/// <summary>
		/// コミュニティ名を取得します
		/// </summary>
		string CommunityName { get;}

		/// <summary>
		/// ローカルPC上での放送開始時刻
		/// </summary>
		DateTime LocalStartTime { get; }

		/// <summary>
		/// サーバー上での放送開始時刻
		/// </summary>
		DateTime ServerStartTime { get; }

		#endregion


		


		#region 操作

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
		/// Ngを追加します。
		/// ビューアがNG機能を持たない場合は常にFalseを返す
		/// </summary>
		/// <param name="type">NGのタイプ。単語、ユーザー、コマンド</param>
		/// <param name="source">追加対象となるワード</param>
		/// <returns>成否判定</returns>
		bool AddNG(NGType type, string source);

		/// <summary>
		/// Ngを削除します
		/// ビューアがNG機能を持たない場合は常にFalseを返す
		/// </summary>
		/// <param name="type">NGのタイプ。単語、ユーザー、コマンド</param>
		/// <param name="source">削除対象となるワード</param>
		/// <returns>成否判定</returns>
		bool DeleteNG(NGType type, string source);

		/// <summary>
		/// 放送に接続します。
		/// </summary>
		/// <param name="liveId">対象の放送URL、または放送ID</param>
		/// <returns>接続できたか</returns>
		bool ConnectLive(string liveId);

		/// <summary>
		/// サーバー通信を中断します。
		/// </summary>
		void Disconnect();

		/// <summary>
		/// コメントサーバーに接続しない状態で放送にアクセスしたときのような動作を起こさせる
		/// InsertPluginChatとあわせて使い、コメントサーバーとはまったく別ソースのコメントを
		/// ビューに表示させるときに使う（Ustreamのコメントなど）
		/// 実装は任意
		/// インターフェースバージョン2
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="liveName"></param>
		/// <param name="liveStart"></param>
		/// <returns>成功したかどうか(未実装の場合はfalseを返す)</returns>
		bool StartMockLive(string liveId, string liveName, DateTime liveStart);

		/// <summary>
		/// プラグインからコメントサーバーとは別のコメントを挿入する
		/// 実装は任意
		/// インターフェースバージョン1
		/// </summary>
		/// <param name="chat"></param>
		/// <returns>成功したかどうか(未実装の場合はfalseを返す)</returns>
		bool InsertPluginChat(IChat chat);

		/// <summary>
		/// NG一覧を取得します。
		/// ビューアがNG機能を持たない場合は常にNullを返す
		/// </summary>
		/// <returns></returns>
		INgClient[] GetNgClients();

		/// <summary>
		/// ビューで選択中のチャットを取得します。
		/// </summary>
		/// <returns>選択されているChat。何も選択されていない場合はnullを返す</returns>
		IChat GetSelectedChat();

		#endregion

		


		#region フォーム操作

		/// <summary>
		/// ウィンドウオーナー
		/// Formをshowする場合にこれをIWin32Windowにキャストして引数に渡すと
		/// Formがメイン画面の裏に隠れなくなる
		/// </summary>
		object Win32WindowOwner { get; }

		/// <summary>
		/// noで指定したコメントを選択状態にして画面上に表示します。
		/// </summary>
		/// <param name="no">コメント番号</param>
		/// <returns></returns>
		bool SelectChat(int no);

		/// <summary>
		/// ステータスメッセージを表示させます。
		/// </summary>
		/// <param name="message">表示内容</param>
		void ShowStatusMessage(string message);

		/// <summary>
		/// 警告メッセージを表示させます。
		/// </summary>
		/// <param name="message">表示内容</param>
		void ShowFaitalMessage(string message);

		/// <summary>
		/// ビューフォーマット時の外部操作をサポートするか
		/// インターフェースバージョン1
		/// </summary>
		bool SupportAddCellFormattingCallBack { get; }

		/// <summary>
		/// カラム拡張をサポートするかを取得します
		/// インターフェースバージョン1
		/// </summary>
		bool SupportAddColumn { get; }

		/// <summary>
		/// コンテクストメニュー拡張をサポートするかを取得します
		/// インターフェースバージョン1
		/// </summary>
		bool SupportAddContextMenuItem { get; }

		/// <summary>
		/// メニューストリップ拡張をサポートするか
		/// インターフェースバージョン1
		/// </summary>
		bool SupportAddMenuStripItem { get; }

		/// <summary>
		/// ビューフォーマット時の外部操作を追加します
		/// 実装は任意
		/// インターフェースバージョン1
		/// </summary>
		/// <param name="callback"></param>
		void AddCellFormattingCallBack(CellFormattingCallback callback);

		/// <summary>
		/// ビューのカラムを追加します
		/// 実装は任意
		/// インターフェースバージョン1
		/// </summary>
		/// <param name="columnExtention"></param>
		void AddColumnExtention(IColumnExtention columnExtention);

		/// <summary>
		/// コンテクストメニューを拡張します
		/// 実装は任意
		/// インターフェースバージョン1
		/// </summary>
		/// <param name="menuItem"></param>
		void AddContextMenuItem(System.Windows.Forms.ToolStripMenuItem menuItem);

		/// <summary>
		/// コンテクストメニューを拡張します
		/// 実装は任意
		/// インターフェースバージョン2
		/// </summary>
		/// <param name="menuItem"></param>
		/// <param name="openingCallback">コンテクストメニューが開かれた際に実行されるコールバック関数</param>
		void AddContextMenuItem(System.Windows.Forms.ToolStripMenuItem menuItem, EventHandler openingCallback);

		/// <summary>
		/// メニューストリップを拡張します
		/// 実装は任意
		/// インターフェースバージョン1
		/// </summary>
		/// <param name="category">ファイル、編集、表示などなるべく一般的なもの</param>
		/// <param name="menuItem"></param>
		void AddMenuStripItem(string category, System.Windows.Forms.ToolStripMenuItem menuItem);

		/// <summary>
		/// メニューストリップを拡張します
		/// 実装は任意
		/// インターフェースバージョン2
		/// </summary>
		/// <param name="category">ファイル、編集、表示などなるべく一般的なもの</param>
		/// <param name="menuItem"></param>
		/// <param name="openingCallback">コンテクストメニューが開かれた際に実行されるコールバック関数</param> 
		void AddMenuStripItem(string category, System.Windows.Forms.ToolStripMenuItem menuItem, EventHandler openingCallback);

		/// <summary>
		/// カラムエクステンションやセルフォーマッターなどで値が変更され、ビュー全体を更新する必要があるときに呼び出します
		/// 必要があれば実装
		/// インターフェースバージョン3
		/// </summary>
		void UpdateCellValues();

		#endregion


	}
}
