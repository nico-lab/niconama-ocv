using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming.Live
{

	/// <summary>
	/// 放送にアクセスするための情報を取得する
	/// 情報はXMLDocumentとして保持され、適時読みだす
	/// </summary>
	public class PlayerStatus : IBasicStatus, IMessageServerStatus, IWatcherStatus, ICountStatus, IErrorData
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
				string url = string.Format(ApiSettings.Default.GetPlayerStatusUrlFormat, liveId);
				status._xnode = new ExXMLDocument();
				((ExXMLDocument)status._xnode).Load(url, cookies);
				status._localGetTime = DateTime.Now;
				return status;

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;
		}

		/// <summary>
		/// 放送にアクセスするための情報を取得する
		/// 情報はXMLDocumentとして保持され、適時読みだす
		/// </summary>
		/// <param name="liveId"></param>
		/// <returns></returns>
		public static PlayerStatus GetInstance(string liveId) {
			if (LoginManager.DefaultCookies != null) {
				return GetInstance(liveId, LoginManager.DefaultCookies);
			}

			return null;
		}

		DateTime _localGetTime;

		private PlayerStatus()
		{

		}

		/// <summary>
		/// 取得時のサーバー上での時間
		/// </summary>
		public DateTime ServerGetTime
		{
			get { return Utility.SelectDateTime(_xnode, "getplayerstatus/@time"); }
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


		#region ILiveBasicData メンバ

		/// <summary>
		/// 放送IDを取得する
		/// </summary>
		public string Id
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/stream/id"); }
		}

		/// <summary>
		/// コミュニティ情報を取得する
		/// </summary>
		public string CommunityId
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/stream/default_community"); }
		}

		/// <summary>
		/// サーバー上での放送開始時間を取得する
		/// </summary>
		public DateTime StartTime
		{
			get { return Utility.SelectDateTime(_xnode, "getplayerstatus/stream/start_time"); }
		}

		/// <summary>
		/// PC上での放送開始時間を取得する
		/// </summary>
		public DateTime LocalStartTime
		{
			get { return this.StartTime + this.ServerTimeDelay; }
		}

		#endregion

		#region IMessageServerData メンバ

		/// <summary>
		/// メッセージサーバーのアドレス
		/// </summary>
		public string Address
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/ms/addr"); }
		}

		/// <summary>
		/// メッセージサーバーのポート番号
		/// </summary>
		public int Port
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/ms/port", -1); }
		}

		/// <summary>
		/// スレッド番号
		/// </summary>
		public int Thread
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/ms/thread", -1); }
		}

		#endregion

		#region ILiveWatcherData メンバ

		/// <summary>
		/// 座席名を取得する
		/// </summary>
		public string RoomLabel
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/user/room_label"); }
		}

		/// <summary>
		/// 座席番号
		/// </summary>
		public int SheetNo
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/user/room_seetno", 0); }
		}

		/// <summary>
		/// 放送主かどうか
		/// </summary>
		public bool IsOwner
		{
			get { return this.SheetNo == ApiSettings.Default.OwnerSheetNo; }
		}

		/// <summary>
		/// プレミアム会員かどうか
		/// </summary>
		public bool IsPremium
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/user/is_premium", 0) == 1; }
		}

		/// <summary>
		/// ユーザーIDを取得する
		/// </summary>
		public int UserId
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/user/user_id", 0); }
		}

		/// <summary>
		/// ユーザー名を取得する
		/// </summary>
		public string UserName
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/user/nickname"); }
		}

		#endregion

		#region ILiveCountStatus メンバ

		/// <summary>
		/// 総来場者数
		/// </summary>
		public int WatchCount
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/stream/watch_count", 0); }
		}

		/// <summary>
		/// コメント数
		/// </summary>
		public int CommentCount
		{
			get { return Utility.SelectInt(_xnode, "getplayerstatus/stream/comment_count", 0); }
		}

		#endregion

		#region IErrorCode メンバ

		/// <summary>
		/// サーバーから送られてきたエラーコード
		/// </summary>
		public string ErrorCode
		{
			get { return Utility.SelectString(_xnode, "getplayerstatus/error/code"); }
		}

		/// <summary>
		/// エラーコードの意味
		/// </summary>
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
							Logger.Default.LogErrorMessage("PlayerStatus UnknownErrorCode:"+this.ErrorCode);
							return "未定義のエラーが発生しました";
					}


				} else {
					return "エラーはありません";
				}

			}
		}

		/// <summary>
		/// エラーがあるかどうか
		/// </summary>
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
