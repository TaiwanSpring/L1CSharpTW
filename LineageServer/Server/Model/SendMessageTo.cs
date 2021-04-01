using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Server.Model
{
	class SendMessageTo : ISendMessageTo
	{
		public void Send(string message, L1PcInstance pc)
		{
			pc.sendPackets(new S_SystemMessage(message));
		}

		public void SendToAll(string message)
		{
			foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
			{
				pc.sendPackets(new S_SystemMessage(message));
			}
		}
	}
}
