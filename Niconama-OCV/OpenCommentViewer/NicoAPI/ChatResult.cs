using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI
{
	public class ChatResult
	{
		System.Xml.XmlNode _xnode = null;

		public ChatResult(string chatResultXML)
		{
			System.Diagnostics.Debug.Assert(chatResultXML != null, "new chatResult chatResultXML is Null!");
			
			_xnode = new System.Xml.XmlDocument();
			((System.Xml.XmlDocument)_xnode).LoadXml(chatResultXML);

		}

		
	}
}
