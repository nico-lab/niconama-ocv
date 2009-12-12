using System;
using System.Collections.Generic;
using System.Text;
using Hal.NicoApiSharp.Streaming;

namespace Hal.NicoApiSharp.Streaming.Jikkyo
{
	/// <summary>
	/// 実況コメントを取得するための情報を表すクラス
	/// </summary>
	public class GetFlv : IBasicStatus, IMessageServerStatus, IErrorData
	{

		/// <summary>
		/// API:GetFlvから情報を取得する
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
		/// API:GetFlvから情報を取得する
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


		#region IBasicStatus メンバ

		/// <summary>
		/// 実況IDを取得する
		/// </summary>
		public string Id
		{
			get { return _liveId; }
		}

		/// <summary>
		/// コミュニティ情報を取得する
		/// </summary>
		public string CommunityId
		{
			get { return ""; }
		}

		/// <summary>
		/// サーバー上での放送開始時間を取得する
		/// </summary>
		public DateTime StartTime
		{
			get { return GetDateTime(_params["start_time"]); }
		}

		/// <summary>
		/// PC上での放送開始時間を取得する
		/// </summary>
		public DateTime LocalStartTime
		{
			get { return this.StartTime + this.ServerTimeDelay; }
		}

		/// <summary>
		/// 座席名を取得する
		/// </summary>
		public string RoomLabel
		{
			get { return ""; }
		}

		#endregion

		#region IMessageServerStatus メンバ

		/// <summary>
		/// メッセージサーバーのアドレス
		/// </summary>
		public string Address
		{
			get { return _params["ms"]; }
		}

		/// <summary>
		/// メッセージサーバーのポート番号
		/// </summary>
		public int Port
		{
			get { return GetInt(_params["ms_port"], 0); }
		}

		/// <summary>
		/// スレッド番号
		/// </summary>
		public int Thread
		{
			get { return GetInt(_params["thread_id"], 0); }
		}

		#endregion

		#region IAccountInfomation メンバ

		/// <summary>
		/// ユーザーIDを取得する
		/// </summary>
		public int UserId
		{
			get { return GetInt(_params["user_id"], 0); }
		}

		/// <summary>
		/// ユーザー名を取得する
		/// </summary>
		public string UserName
		{
			get { return ""; }
		}

		/// <summary>
		/// プレミアム会員かどうか
		/// </summary>
		public bool IsPremium
		{
			get { return GetInt(_params["is_premium"], 0) == 1; }
		}

		#endregion

		#region IErrorData メンバ

		/// <summary>
		/// サーバーから送られてきたエラーコード
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
		/// エラーコードの意味
		/// </summary>
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

		/// <summary>
		/// エラーがあるかどうか
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
