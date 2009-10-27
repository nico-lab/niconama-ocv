using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{

	/// <summary>
	/// 放送にアクセスするための情報を取得する
	/// 情報はXMLDocumentとして保持され、適時読みだす
	/// </summary>
	public class PlayerStatus : ILiveBasicStatus, IMessageServerStatus, ILiveWatcherStatus, ILiveCountStatus, IErrorData
	{

		System.Xml.XmlNode _xnode = null;

		/// <summary>
		/// API:GetPlayerStatusから情報を取得する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static PlayerStatus GetInstance(string liveId, System.Net.CookieContainer cookies)
		{

			try {
				PlayerStatus status = new PlayerStatus();
				string url = string.Format(ApplicationSettings.Default.GetPlayerStatusUrlFormat, liveId);
				status._xnode = new CustomControl.ExXMLDocument();
				((CustomControl.ExXMLDocument)status._xnode).Load(url, cookies);
				status._localGetTime = DateTime.Now;
				return status;

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}

		DateTime _localGetTime;

		private PlayerStatus()
		{

		}

		public DateTime ServerGetTime
		{
			get { return Utility.SelectDateTime(_xnode, "getplayerstatus/@time"); }
		}

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


		#region ILiveBasicData メンバ

		public string LiveId
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/stream/id"); }
		}

		public string CommunityId
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/stream/default_community"); }
		}

		public DateTime StartTime
		{
			get { return Utility.SelectDateTime(_xnode, "getplayerstatus/stream/start_time"); }
		}

		public DateTime LocalStartTime
		{
			get { return this.StartTime + this.ServerTimeDelay; }
		}

		#endregion

		#region IMessageServerData メンバ

		public string Address
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/ms/addr"); }
		}

		public int Port
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/ms/port", -1); }
		}

		public int Thread
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/ms/thread", -1); }
		}

		#endregion

		#region ILiveWatcherData メンバ

		public string RoomLabel
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/user/room_label"); }
		}

		public int SheetNo
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/user/room_seetno", 0); }
		}

		public bool IsOwner
		{
			get { return this.SheetNo == ApplicationSettings.Default.OwnerSheetNo; }
		}

		public bool IsPremium
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/user/is_premium", 0) == 1; }
		}

		public int UserId
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/user/user_id", 0); }
		}

		public string UserName
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/user/nickname"); }
		}

		#endregion

		#region ILiveCountStatus メンバ

		public int WatchCount
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/stream/watch_count", 0); }
		}

		public int CommentCount
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/stream/comment_count", 0); }
		}

		#endregion

		#region IErrorCode メンバ

		public string ErrorCode
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/error/code"); }
		}

		public string ErrorMessage
		{
			get
			{
				if (this.HasError) {
					switch (this.ErrorCode) {
						case null:
							return "XMLを取得できませんでした。";
						case "closed":
							return "放送は終了しています。";
						case "notfound":
							return "放送が見つかりませんでした。";
						case "notlogin":
							return "ログインが完了していません。";
						case "full":
							return "満員のため放送にアクセスできませんでした。";
						case "require_community_member":
							return "メンバー限定のためアクセスできませんでした。";
						default:
							return "未定義のエラーが発生しました";
					}


				} else {
					return "エラーはありません";
				}

			}
		}

		public bool HasError
		{
			get
			{
				string status = Utility.SelectString(_xnode, "getplayerstatus/@status");
				return status == null || status != "ok";
			}
		}

		#endregion
	}
}
