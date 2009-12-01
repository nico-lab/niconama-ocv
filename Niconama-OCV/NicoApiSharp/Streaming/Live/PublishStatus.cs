using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Streaming.Live
{
	//public class PublishStatus : ILiveCountStatus, IErrorData
	//{
	//    System.Xml.XmlNode _xnode = null;

	//    /// <summary>
	//    /// API:GetPublishStatusから情報を取得する
	//    /// </summary>
	//    /// <param name="liveId"></param>
	//    /// <param name="cookies"></param>
	//    /// <returns></returns>
	//    public static PlayerStatus GetInstance(string liveId, System.Net.CookieContainer cookies)
	//    {

	//        try {
	//            PublishStatus status = new PublishStatus();
	//            string url = string.Format(ApiSettings.Default.GetPublishStatusUrlFormat, liveId);
	//            status._xnode = new ExXMLDocument();
	//            ((ExXMLDocument)status._xnode).Load(url, cookies);
	//            status._localGetTime = DateTime.Now;
	//            return status;

	//        } catch (Exception ex) {
	//            Logger.Default.LogException(ex);
	//        }

	//        return null;
	//    }

	//    /// <summary>
	//    /// 放送にアクセスするための情報を取得する
	//    /// 情報はXMLDocumentとして保持され、適時読みだす
	//    /// </summary>
	//    /// <param name="liveId"></param>
	//    /// <returns></returns>
	//    public static PlayerStatus GetInstance(string liveId)
	//    {
	//        if (LoginManager.DefaultCookies != null) {
	//            return GetInstance(liveId, LoginManager.DefaultCookies);
	//        }

	//        return null;
	//    }

	//    #region ILiveCountStatus メンバ

	//    public int WatchCount
	//    {
	//        get { throw new Exception("The method or operation is not implemented."); }
	//    }

	//    public int CommentCount
	//    {
	//        get { throw new Exception("The method or operation is not implemented."); }
	//    }

	//    #endregion

	//    #region IErrorData メンバ

	//    public string ErrorCode
	//    {
	//        get { throw new Exception("The method or operation is not implemented."); }
	//    }

	//    public string ErrorMessage
	//    {
	//        get { throw new Exception("The method or operation is not implemented."); }
	//    }

	//    public bool HasError
	//    {
	//        get { throw new Exception("The method or operation is not implemented."); }
	//    }

	//    #endregion
	//}
}
