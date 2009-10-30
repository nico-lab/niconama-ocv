using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{
	public class OcvChat : NicoApiSharp.Live.Chat, NCSPlugin.IFilterdChat
	{

		private NCSPlugin.NGType _ngType = NCSPlugin.NGType.None;
		private string _ngSource = null;

		/// <summary>
		/// NCSPlugin.IChatにFilteredChatインターフェースを追加する
		/// </summary>
		/// <param name="chat"></param>
		public OcvChat(NCSPlugin.IChat chat) : base(chat) { 
		
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

		#endregion
	}
}
