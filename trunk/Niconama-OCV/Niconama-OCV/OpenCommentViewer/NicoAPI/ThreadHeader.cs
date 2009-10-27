using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	public class ThreadHeader
	{

		System.Xml.XmlNode _xnode = null;

		public ThreadHeader(string threadXML)
		{
			System.Diagnostics.Debug.Assert(threadXML != null, "new ThreadHeader threadXML is Null!");

			_xnode = new System.Xml.XmlDocument();
			((System.Xml.XmlDocument)_xnode).LoadXml(threadXML);

		}

		public ThreadHeader(System.Xml.XmlNode node)
		{
			_xnode = node;
		}

		public int LastRes
		{
			get { return Utility.SelectInt(_xnode, "thread/@last_res", 0); }
		}

		public int RresultCode
		{
			get { return Utility.SelectInt(_xnode, "thread/@resultcode", -1); }
		}

		public int Revision
		{
			get { return Utility.SelectInt(_xnode, "thread/@revision", 0); }
		}

		public DateTime ServerTime
		{
			get { return Utility.SelectDateTime(_xnode, "thread/@server_time"); }
		}

		public int Thread
		{
			get { return Utility.SelectInt(_xnode, "thread/@thread", 0); }
		}

		public string Ticket
		{
			get { return Utility.SelectString(_xnode, "thread/@ticket"); }
		}


	}
}
