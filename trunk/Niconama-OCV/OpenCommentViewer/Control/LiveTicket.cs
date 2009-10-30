using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{

	/// <summary>
	/// コメントサーバーに再接続するためのデータを格納するためのクラス
	/// </summary>
	public class LiveTicket : NicoAPI.ILiveBasicStatus, NicoAPI.IMessageServerStatus, NicoAPI.ILiveDescription
	{

		string _liveId;
		string _communityId;
		DateTime _startTime;
		DateTime _localStartTime;
		string _roomLabel;
		string _address;
		int _port;
		int _thread;
		string _liveName;
		string _communityName;
		string _caster;

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public LiveTicket()
		{

		}

		/// <summary>
		/// チケットの生成に必要な情報をもとにチケットを生成します
		/// </summary>
		/// <param name="basicStatus"></param>
		/// <param name="messageStatus"></param>
		/// <param name="liveDescription"></param>
		public LiveTicket(NicoAPI.ILiveBasicStatus basicStatus, NicoAPI.IMessageServerStatus messageStatus, NicoAPI.ILiveDescription liveDescription)
		{
			this._liveId = basicStatus.LiveId;
			this._communityId = basicStatus.CommunityId;
			this._startTime = basicStatus.StartTime;
			this._localStartTime = basicStatus.LocalStartTime;
			this._roomLabel = basicStatus.RoomLabel;
			this._address = messageStatus.Address;
			this._port = messageStatus.Port;
			this._thread = messageStatus.Thread;
			this._liveName = liveDescription.LiveName;
			this._communityName = liveDescription.CommunityName;
			this._caster = liveDescription.Caster;
		}

		#region ILiveBasicStatus メンバ

		/// <summary>
		/// 放送IDを取得、設定します
		/// </summary>
		public string LiveId
		{
			get { return _liveId; }
			set { _liveId = value; }
		}

		/// <summary>
		/// コミュニティIDを取得、設定します
		/// </summary>
		public string CommunityId
		{
			get { return _communityId; }
			set { _communityId = value; }
		}

		/// <summary>
		/// サーバー上での開始時間を取得、設定します
		/// </summary>
		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		/// <summary>
		/// ローカルPC上での放送開始時間を取得、設定します
		/// </summary>
		public DateTime LocalStartTime
		{
			get { return _localStartTime; }
			set { _localStartTime = value; }
		}

		/// <summary>
		/// 座席名を取得、設定します
		/// </summary>
		public string RoomLabel
		{
			get { return _roomLabel; }
			set { _roomLabel = value; }
		}

		#endregion

		#region IMessageServerStatus メンバ

		/// <summary>
		/// メッセージサーバーのアドレスを取得、設定します
		/// </summary>
		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}

		/// <summary>
		/// メッセージサーバーのポート番号を取得、設定します
		/// </summary>
		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}

		/// <summary>
		/// スレッド番号を取得、設定します
		/// </summary>
		public int Thread
		{
			get { return _thread; }
			set { _thread = value; }
		}

		#endregion

		#region ILiveDescription メンバ

		/// <summary>
		/// 番組名を取得、設定します
		/// </summary>
		public string LiveName
		{
			get { return _liveName; }
			set { _liveName = value; }
		}

		/// <summary>
		/// コミュニティ名を取得、設定します
		/// </summary>
		public string CommunityName
		{
			get { return _communityName; }
			set { _communityName = value; }
		}

		/// <summary>
		/// 放送者を取得、設定します
		/// </summary>
		public string Caster
		{
			get { return _caster; }
			set { _caster = value; }
		}

		#endregion
	}
}
