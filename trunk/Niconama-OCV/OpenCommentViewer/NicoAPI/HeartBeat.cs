using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	/// <summary>
	/// ハートビートを取得する
	/// </summary>
	public class HeartBeat : ILiveCountStatus, IErrorData
	{
		System.Xml.XmlNode _xnode;

		public static HeartBeat GetInstance(string liveId, System.Net.CookieContainer cookies)
		{

			try {
				HeartBeat hb = new HeartBeat();
				string url = string.Format(ApplicationSettings.Default.GetHeartBeatUrlFormat, liveId);
				hb._xnode = new CustomControl.ExXMLDocument();
				((CustomControl.ExXMLDocument)hb._xnode).Load(url, cookies);

				return hb;
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}

		#region ILiveCountStatus メンバ

		public int WatchCount
		{
			get { return Utility.SelectInt(_xnode, "heartbeat/watchCount", 0); }
		}

		public int CommentCount
		{
			get { return Utility.SelectInt(_xnode, "heartbeat/commentCount", 0); }
		}

		#endregion

		#region IErrorData メンバ

		public string ErrorCode
		{
			get { return Utility.SelectString(_xnode, "heartbeat/error/code"); }
		}

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
