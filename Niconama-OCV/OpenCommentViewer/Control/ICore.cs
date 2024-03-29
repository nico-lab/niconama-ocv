﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{

	/// <summary>
	/// 座席の種類
	/// </summary>
	public enum SeetType
	{
		/// <summary>
		/// アリーナ
		/// </summary>
		Arena,

		/// <summary>
		/// 立ち見
		/// </summary>
		Standing,

		/// <summary>
		/// 実況
		/// </summary>
		Jikkyo
	}

	/// <summary>
	/// コア部分のインターフェース
	/// メインフォームやシステム内部のプラグインに対して公開される
	/// </summary>
	public interface ICore : Hal.NCSPlugin.IPluginHost
	{
		/// <summary>
		/// アリーナか立ち見かを返します
		/// </summary>
		SeetType SeetType { get; }

		/// <summary>
		/// ビューアで使用するメインフォームを指定する
		/// （拡張の際、別のフォームを使用できるように）
		/// </summary>
		/// <param name="mainform"></param>
		void SetMainView(IMainView mainview);

		/// <summary>
		/// 開始直後に接続する放送IDを指定する
		/// </summary>
		/// <param name="id"></param>
		void Reserve(string liveId);

		/// <summary>
		/// MessageServerStatusで指定されたコメントをすべて取得する
		/// </summary>
		/// <param name="messageServerStatus"></param>
		/// <returns></returns>
		bool GetLogComment(NicoApiSharp.Streaming.IMessageServerStatus messageServerStatus);

		/// <summary>
		/// 放送再接続用の情報を取得します
		/// </summary>
		/// <returns></returns>
		LiveTicket GetLiveTicket();

		/// <summary>
		/// チケット情報を元にメッセージサーバーへの接続を試みる
		/// </summary>
		/// <param name="ticket"></param>
		/// <returns></returns>
		bool ConnectByLiveTicket(LiveTicket ticket);

		/// <summary>
		/// UserSettingsの設定に従ってログインを実行する
		/// </summary>
		/// <returns></returns>
		bool Login();

		/// <summary>
		/// デバッグ用
		/// </summary>
		void CallTestMethod();
	}
}
