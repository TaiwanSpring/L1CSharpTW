using LineageServer.Server.Model.Instance;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
	interface ISendMessageTo
	{
		void Send(string message, L1PcInstance pc);
		void SendToAll(string message);
	}
}
