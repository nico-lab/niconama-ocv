using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	/// <summary>
	/// コメントサーバーと通信を開始した直後のスレッドの状態を表すクラス
	/// </summary>
	public class ThreadHeader
	{

		System.Xml.XmlNode _xnode = null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="threadXML"></param>
		public ThreadHeader(string threadXML)
		{
			System.Diagnostics.Debug.Assert(threadXML != null, "new ThreadHeader threadXML is Null!");

			_xnode = new System.Xml.XmlDocument();
			((System.Xml.XmlDocument)_xnode).LoadXml(threadXML);

		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="node"></param>
		public ThreadHeader(System.Xml.XmlNode node)
		{
			_xnode = node;
		}

		/// <summary>
		/// 最終コメント番号
		/// </summary>
		public int LastRes
		{
			get { return Utility.SelectInt(_xnode, "thread/@last_res", 0); }
		}

		/// <summary>
		/// 接続結果　0が成功
		/// </summary>
		public int RresultCode
		{
			get { return Utility.SelectInt(_xnode, "thread/@resultcode", -1); }
		}

		/// <summary>
		/// リビジョン
		/// </summary>
		public int Revision
		{
			get { return Utility.SelectInt(_xnode, "thread/@revision", 0); }
		}

		/// <summary>
		/// このデータが発行されたサーバー時間
		/// </summary>
		public DateTime ServerTime
		{
			get { return Utility.SelectDateTime(_xnode, "thread/@server_time"); }
		}

		/// <summary>
		/// スレッド番号
		/// </summary>
		public int Thread
		{
			get { return Utility.SelectInt(_xnode, "thread/@thread", 0); }
		}

		/// <summary>
		/// コメント投稿用のチケット
		/// </summary>
		public string Ticket
		{
			get { return Utility.SelectString(_xnode, "thread/@ticket"); }
		}


	}
}
