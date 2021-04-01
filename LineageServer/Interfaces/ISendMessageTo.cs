using LineageServer.Server.Model.Instance;

namespace LineageServer.Interfaces
{
    interface ISendMessageTo
	{
		void Send(string message, L1PcInstance pc);
		void SendToAll(string message);
	}
}
