using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Control
{
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

		public LiveTicket()
		{

		}

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
			this._liveName = liveDescription.CommunityName;
			this._communityName = liveDescription.CommunityName;
			this._caster = liveDescription.Caster;
		}

		#region ILiveBasicStatus メンバ

		public string LiveId
		{
			get { return _liveId; }
			set { _liveId = value; }
		}

		public string CommunityId
		{
			get { return _communityId; }
			set { _communityId = value; }
		}

		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		public DateTime LocalStartTime
		{
			get { return _localStartTime; }
			set { _localStartTime = value; }
		}

		public string RoomLabel
		{
			get { return _roomLabel; }
			set { _roomLabel = value; }
		}

		#endregion

		#region IMessageServerStatus メンバ

		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}

		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}

		public int Thread
		{
			get { return _thread; }
			set { _thread = value; }
		}

		#endregion

		#region ILiveDescription メンバ


		public string LiveName
		{
			get { return _liveName; }
			set { _liveName = value; }
		}

		public string CommunityName
		{
			get { return _communityName; }
			set { _communityName = value; }
		}

		public string Caster
		{
			get { return _caster; }
			set { _caster = value; }
		}

		#endregion
	}
}
