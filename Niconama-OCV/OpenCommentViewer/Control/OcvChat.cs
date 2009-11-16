using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{
	public class OcvChat : NicoApiSharp.Live.Chat, NCSPlugin.IChat, NCSPlugin.IFilterdChat
	{

		private NCSPlugin.NGType _ngType = NCSPlugin.NGType.None;
		private string _ngSource = null;

		/// <summary>
		/// Hal.NicoApiSharp.Live.ChatからOcvChatを生成する
		/// </summary>
		/// <param name="chat"></param>
		public OcvChat(Hal.NicoApiSharp.Live.IChat chat) : base(chat) { 
		
		}

		/// <summary>
		/// NCSPlugin.IChatからOcvChatを生成する
		/// </summary>
		/// <param name="chat"></param>
		public OcvChat(Hal.NCSPlugin.IChat chat)
			: base(chat.Anonymity, chat.Premium, chat.Date, chat.Mail, chat.Message, chat.No, chat.Thread, chat.UserId, chat.Vpos)
		{

		}

		/// <summary>
		/// System.Xml.XmlNodeからOcvChatを生成する
		/// </summary>
		/// <param name="chat"></param>
		public OcvChat(System.Xml.XmlNode xml) : base(xml) { 
		
		}


		#region IFilterdChat メンバ

		public NCSPlugin.NGType NgType
		{
			get { return _ngType; }
			set { _ngType = value; }
		}

		public string NgSource
		{
			get { return _ngSource; }
			set { _ngSource = value; }
		}
		
		public int NgStart
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int NgLength
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}


		#endregion

	}
}
