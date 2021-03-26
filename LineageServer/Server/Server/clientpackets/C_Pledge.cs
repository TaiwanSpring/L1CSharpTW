
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來血盟的封包
	/// </summary>
	class C_Pledge : ClientBasePacket
	{

		private const string C_PLEDGE = "[C] C_Pledge";
		public C_Pledge(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			if (pc.Clanid > 0)
			{
				L1Clan clan = L1World.Instance.getClan(pc.Clanname);
				// 血盟公告
				pc.sendPackets(new S_Pledge(clan.ClanId));

				// 血盟成員
				pc.sendPackets(new S_Pledge(pc));

				// 線上血盟成員
				pc.sendPackets(new S_PacketBox(S_PacketBox.HTML_PLEDGE_ONLINE_MEMBERS, clan.OnlineClanMember));
			}
			else
			{
				// 不屬於血盟。
				pc.sendPackets(new S_ServerMessage(1064));
			}
		}

		public override string Type
		{
			get
			{
				return C_PLEDGE;
			}
		}

	}

}