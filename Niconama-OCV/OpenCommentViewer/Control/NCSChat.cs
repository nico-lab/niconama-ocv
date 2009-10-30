using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{
	public class NCSChat : NicoApiSharp.Live.Chat, NCSPlugin.IChat, NCSPlugin.IFilterdChat
	{
		private NCSPlugin.NGType _ngType = NCSPlugin.NGType.None;
		private string _ngSource = null;

		public NCSChat(Hal.NicoApiSharp.Live.IChat chat) : base(chat) { 
		
		}

		#region IFilterdChat ÉÅÉìÉo

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
