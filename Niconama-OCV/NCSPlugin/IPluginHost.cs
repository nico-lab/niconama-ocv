using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// プラグインホストのインターフェース
	/// version 3
	/// </summary>
	public interface IPluginHost : ICommnetAgent, IContentStatus
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

		#region フォーム操作

		/// <summary>
		/// ウィンドウオーナー
		/// Formをshowする場合にこれをIWin32Windowにキャストして引数に渡すと
		/// Formがメイン画面の裏に隠れなくなる
		/// </summary>
		object Win32WindowOwner { get; }

		/// <summary>
		/// chatで指定したコメントを選択状態にして画面上に表示します。
		/// </summary>
		/// <param name="chat">コメント</param>
		/// <returns></returns>
		bool SelectChat(IChat chat);

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

		#endregion

		#region コメント

		/// <summary>
		/// 外部からチャットを追加します
		/// </summary>
		/// <param name="chat"></param>
		void AddChat(IChat chat);

		/// <summary>
		/// 外部からチャットを追加します
		/// </summary>
		/// <param name="chats"></param>
		void AddChats(IChat[] chats);

		/// <summary>
		/// 保存されているチャット一覧のコピーを取得します。
		/// </summary>
		IChat[] Chats { get;}

		/// <summary>
		/// ビューで選択中のチャットを取得します。
		/// </summary>
		/// <returns>選択されているChat。何も選択されていない場合はnullを返す</returns>
		IChat GetSelectedChat();

		#endregion

		#region 拡張

		/// <summary>
		/// フォームを拡張するためのオブジェクトを取得する
		/// </summary>
		IFormExtender FormExtender { get; }

		/// <summary>
		/// チャットビュー拡張するためのオブジェクトを取得する
		/// </summary>
		IChatviewExtender ChatviewExtender { get; }

		#endregion


	}
}
