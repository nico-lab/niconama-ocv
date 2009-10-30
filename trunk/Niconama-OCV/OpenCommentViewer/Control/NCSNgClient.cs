using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer.Control
{
	public class NCSNgClient : NicoApiSharp.Live.NgClient, NCSPlugin.INgClient
	{

		public NCSNgClient(NCSPlugin.NGType type, string source, DateTime regTime) : base(PluginTypeToApiType(type), source, regTime)
		{

		}

		#region INgClient ÉÅÉìÉo

		Hal.NCSPlugin.NGType Hal.NCSPlugin.INgClient.Type
		{
			get {
				return ApiTypeToPluginType(this.Type);
			}
		}

		

		private static Hal.NCSPlugin.NGType ApiTypeToPluginType(Hal.NicoApiSharp.Live.NGType value)
		{
			switch (value) {
				case Hal.NicoApiSharp.Live.NGType.None:
					return Hal.NCSPlugin.NGType.None;
				case Hal.NicoApiSharp.Live.NGType.Id:
					return Hal.NCSPlugin.NGType.Id;
				case Hal.NicoApiSharp.Live.NGType.Command:
					return Hal.NCSPlugin.NGType.Command;
				case Hal.NicoApiSharp.Live.NGType.Word:
					return Hal.NCSPlugin.NGType.Word;
				default:
					return Hal.NCSPlugin.NGType.None;
			}
		}

		private static Hal.NicoApiSharp.Live.NGType PluginTypeToApiType(Hal.NCSPlugin.NGType value)
		{
			switch (value) {
				case Hal.NCSPlugin.NGType.None:
					return Hal.NicoApiSharp.Live.NGType.None;
				case Hal.NCSPlugin.NGType.Id:
					return Hal.NicoApiSharp.Live.NGType.Id;
				case Hal.NCSPlugin.NGType.Command:
					return Hal.NicoApiSharp.Live.NGType.Command;
				case Hal.NCSPlugin.NGType.Word:
					return Hal.NicoApiSharp.Live.NGType.Word;
				default:
					return Hal.NicoApiSharp.Live.NGType.None;
			}
		}

		
		#endregion
	}
}
