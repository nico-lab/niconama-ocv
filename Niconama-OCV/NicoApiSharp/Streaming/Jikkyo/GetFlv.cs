using System;
using System.Collections.Generic;
using System.Text;
using Hal.NicoApiSharp.Streaming;

namespace Hal.NicoApiSharp.Streaming.Jikkyo
{
	/// <summary>
	/// �����R�����g���擾���邽�߂̏���\���N���X
	/// </summary>
	public class GetFlv : IBasicStatus, IMessageServerStatus, IErrorData
	{

		/// <summary>
		/// API:GetFlv��������擾����
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static GetFlv GetInstance(string liveId, System.Net.CookieContainer cookies)
		{

			try {

				string url = string.Format(ApiSettings.Default.GetJikkyoFlvUrlFormat, liveId);
				string data = Utility.GetResponseText(url, cookies, 1000);
				GetFlv status = new GetFlv();
				status._params = toMap(data);
				status._localGetTime = DateTime.Now;
				status._liveId = liveId;
				return status;

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}

		/// <summary>
		/// API:GetFlv��������擾����
		/// </summary>
		/// <param name="liveId"></param>
		/// <returns></returns>
		public static GetFlv GetInstance(string liveId)
		{
			if (LoginManager.DefaultCookies != null) {
				return GetInstance(liveId, LoginManager.DefaultCookies);
			}

			return null;
		}

		private static Dictionary<string, string> toMap(string data)
		{

			Dictionary<string, string> results = new Dictionary<string, string>();

			foreach (string segment in data.Split(new char[]{'&'},  StringSplitOptions.RemoveEmptyEntries)) {
				string[] parts = segment.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2) {
					string name = parts[0];
					string value = parts[1];

					if (!results.ContainsKey(name)) {
						results.Add(name, value);
					}
				}
			}

			return results;
		}

		Dictionary<string, string> _params = null;

		DateTime _localGetTime;
		//DateTime _serverGetTime;
		string _liveId;
		//DateTime _startTime;
		//DateTime _baseTime;
		//string _address;
		//int _port;
		//int _thread;
		//int _userId;
		//bool _isPremium;
		//string _errorMessage = null;


		/// <summary>
		/// �擾���̃T�[�o�[��ł̎���
		/// </summary>
		public DateTime ServerGetTime
		{
			get { return GetDateTime(_params["base_time"]); }
		}

		/// <summary>
		/// �擾���̃��[�J���ł̎���
		/// </summary>
		public DateTime LocalGetTime
		{
			get { return _localGetTime; }
		}

		/// <summary>
		/// ���[�J��PC�ƃT�[�o�[�̎��v�̂���
		/// </summary>
		public TimeSpan ServerTimeDelay
		{
			get { return this.LocalGetTime - this.ServerGetTime; }
		}


		#region IBasicStatus �����o

		/// <summary>
		/// ����ID���擾����
		/// </summary>
		public string Id
		{
			get { return _liveId; }
		}

		/// <summary>
		/// �R�~���j�e�B�����擾����
		/// </summary>
		public string CommunityId
		{
			get { return ""; }
		}

		/// <summary>
		/// �T�[�o�[��ł̕����J�n���Ԃ��擾����
		/// </summary>
		public DateTime StartTime
		{
			get { return GetDateTime(_params["start_time"]); }
		}

		/// <summary>
		/// PC��ł̕����J�n���Ԃ��擾����
		/// </summary>
		public DateTime LocalStartTime
		{
			get { return this.StartTime + this.ServerTimeDelay; }
		}

		/// <summary>
		/// ���Ȗ����擾����
		/// </summary>
		public string RoomLabel
		{
			get { return ""; }
		}

		#endregion

		#region IMessageServerStatus �����o

		/// <summary>
		/// ���b�Z�[�W�T�[�o�[�̃A�h���X
		/// </summary>
		public string Address
		{
			get { return _params["ms"]; }
		}

		/// <summary>
		/// ���b�Z�[�W�T�[�o�[�̃|�[�g�ԍ�
		/// </summary>
		public int Port
		{
			get { return GetInt(_params["ms_port"], 0); }
		}

		/// <summary>
		/// �X���b�h�ԍ�
		/// </summary>
		public int Thread
		{
			get { return GetInt(_params["thread_id"], 0); }
		}

		#endregion

		#region IAccountInfomation �����o

		/// <summary>
		/// ���[�U�[ID���擾����
		/// </summary>
		public int UserId
		{
			get { return GetInt(_params["user_id"], 0); }
		}

		/// <summary>
		/// ���[�U�[�����擾����
		/// </summary>
		public string UserName
		{
			get { return ""; }
		}

		/// <summary>
		/// �v���~�A��������ǂ���
		/// </summary>
		public bool IsPremium
		{
			get { return GetInt(_params["is_premium"], 0) == 1; }
		}

		#endregion

		#region IErrorData �����o

		/// <summary>
		/// �T�[�o�[���瑗���Ă����G���[�R�[�h
		/// </summary>
		public string ErrorCode
		{
			get {
				if (this.HasError) { 
					return _params["error"]; 
				}
				return null;
			}
		}

		/// <summary>
		/// �G���[�R�[�h�̈Ӗ�
		/// </summary>
		public string ErrorMessage
		{
			get {
				switch (ErrorCode) { 
					case "":
						return "�f�[�^����M�ł��܂���ł����B";
					case "channel_is_deleted":
						return "���̃`�����l���͏I�����܂����B";
					case "invalid_thread":
						return "���̃`�����l���͑��݂��܂���B";
				}

				return "�G���[�͂���܂���B";
			}
		}

		/// <summary>
		/// �G���[�����邩�ǂ���
		/// </summary>
		public bool HasError
		{
			get { return _params.ContainsKey("error"); }
		}

		#endregion

		private static int GetInt(string data, int defaultValue)
		{
			if (data != null) {
				int result;
				if (int.TryParse(data, out result)) {
					return result;
				}
			}

			return defaultValue;
		}

		private static DateTime GetDateTime(string data)
		{
			int unixTime = GetInt(data, 0);
			return Utility.UnixTimeToDateTime(unixTime);
		}
	}
}
