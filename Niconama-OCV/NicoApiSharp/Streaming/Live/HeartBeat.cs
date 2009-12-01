using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming.Live
{
	/// <summary>
	/// ハートビートを取得する
	/// </summary>
	public class HeartBeat : ICountStatus, IErrorData
	{
		System.Xml.XmlNode _xnode;

		/// <summary>
		/// サーバーからハートビートを取得する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static HeartBeat GetInstance(string liveId, System.Net.CookieContainer cookies)
		{

			try {
				HeartBeat hb = new HeartBeat();
				string url = string.Format(ApiSettings.Default.GetHeartBeatUrlFormat, liveId);
				hb._xnode = new ExXMLDocument();
				((ExXMLDocument)hb._xnode).Load(url, cookies);

				return hb;
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}

		/// <summary>
		/// サーバーからハートビートを取得する
		/// </summary>
		/// <param name="liveId"></param>
		/// <returns></returns>
		public static HeartBeat GetInstance(string liveId) {
			if (LoginManager.DefaultCookies != null) {
				return GetInstance(liveId, LoginManager.DefaultCookies);
			}

			return null;
		}

		#region ILiveCountStatus メンバ

		/// <summary>
		/// 総来場者数を取得する
		/// </summary>
		public int WatchCount
		{
			get { return Utility.SelectInt(_xnode, "heartbeat/watchCount", 0); }
		}

		/// <summary>
		/// コメント数を取得する
		/// </summary>
		public int CommentCount
		{
			get { return Utility.SelectInt(_xnode, "heartbeat/commentCount", 0); }
		}

		#endregion

		#region IErrorData メンバ

		/// <summary>
		/// エラーコードを取得する
		/// </summary>
		public string ErrorCode
		{
			get { return Utility.SelectString(_xnode, "heartbeat/error/code"); }
		}

		/// <summary>
		/// エラーコードの説明を取得する
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				if (this.HasError) {
					switch (this.ErrorCode) {
						case null:
							return "XMLを取得できませんでした。";
						case "NOTFOUND_STREAM":
						case "NOTEXIST_SLOT":
							return "放送が見つかりませんでした。";
						case "NOTFOUND_USERLIVESLOT":
							return "座席を確保できていません。";
						case "NOTLOGIN":
							return "ログインが完了していません。";
						default:
							Logger.Default.LogErrorMessage("HeartBeat UnknownErrorCode:" + this.ErrorCode);
							return "未定義のエラーが発生しました";
					}
				} else {
					return "エラーはありません";
				}
			}
		}

		/// <summary>
		/// エラーが発生しているか調べる
		/// </summary>
		public bool HasError
		{
			get
			{
				string status = Utility.SelectString(_xnode, "heartbeat/@status");
				return status == null || status != "ok";
			}
		}

		#endregion


	}
}
