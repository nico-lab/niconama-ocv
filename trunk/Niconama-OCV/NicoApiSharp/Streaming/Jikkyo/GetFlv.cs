using System;
using System.Collections.Generic;
using System.Text;
using Hal.NicoApiSharp.Live;

namespace Hal.NicoApiSharp.Jk
{
	public class GetFlv : ILiveBasicStatus, IMessageServerStatus, IErrorData
	{
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
		/// 取得時のサーバー上での時間
		/// </summary>
		public DateTime ServerGetTime
		{
			get { return GetDateTime(_params["base_time"]); }
		}

		/// <summary>
		/// 取得時のローカルでの時間
		/// </summary>
		public DateTime LocalGetTime
		{
			get { return _localGetTime; }
		}

		/// <summary>
		/// ローカルPCとサーバーの時計のずれ
		/// </summary>
		public TimeSpan ServerTimeDelay
		{
			get { return this.LocalGetTime - this.ServerGetTime; }
		}


		#region ILiveBasicStatus メンバ

		public string LiveId
		{
			get { return _liveId; }
		}

		public string CommunityId
		{
			get { return ""; }
		}

		public DateTime StartTime
		{
			get { return GetDateTime(_params["start_time"]); }
		}

		public DateTime LocalStartTime
		{
			get { return this.StartTime + this.ServerTimeDelay; }
		}

		public string RoomLabel
		{
			get { return ""; }
		}

		#endregion

		#region IMessageServerStatus メンバ

		public string Address
		{
			get { return _params["ms"]; }
		}

		public int Port
		{
			get { return GetInt(_params["ms_port"], 0); }
		}

		public int Thread
		{
			get { return GetInt(_params["thread_id"], 0); }
		}

		#endregion

		#region IAccountInfomation メンバ

		public int UserId
		{
			get { return GetInt(_params["user_id"], 0); }
		}

		public string UserName
		{
			get { return ""; }
		}

		public bool IsPremium
		{
			get { return GetInt(_params["is_premium"], 0) == 1; }
		}

		#endregion

		#region IErrorData メンバ

		public string ErrorCode
		{
			get {
				if (this.HasError) { 
					return _params["error"]; 
				}
				return null;
			}
		}

		public string ErrorMessage
		{
			get {
				switch (ErrorCode) { 
					case "":
						return "データを受信できませんでした。";
					case "channel_is_deleted":
						return "このチャンネルは終了しました。";
					case "invalid_thread":
						return "このチャンネルは存在しません。";
				}

				return "エラーはありません。";
			}
		}

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
