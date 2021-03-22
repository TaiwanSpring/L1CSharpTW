﻿
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 收到由客戶端傳來取得好友名單的封包
	/// </summary>
	class C_Buddy : ClientBasePacket
	{

		private const string C_BUDDY = "[C] C_Buddy";

		public C_Buddy(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}
			L1Buddy buddy = BuddyTable.Instance.getBuddyTable(pc.Id);
			pc.sendPackets(new S_Buddy(pc.Id, buddy));
		}

		public override string Type
		{
			get
			{
				return C_BUDDY;
			}
		}
	}

}