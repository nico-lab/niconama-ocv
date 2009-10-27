using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Control
{

	public enum SeetType
	{
		Arena,
		Standing
	}

	public interface ICore : NCSPlugin.IPluginHost
	{

		void SetMainView(IMainView mainview);
		void Reserve(string liveId);

		bool GetLogComment(NicoAPI.IMessageServerStatus messageServerStatus);

		LiveTicket GetLiveTicket();
		bool ConnectByLiveTicket(LiveTicket ticket);

		SeetType SeetType { get; }

		bool Login();

		void CallTestMethod();
	}
}
