using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來建立血盟的封包
	/// </summary>
	class C_CreateClan : ClientBasePacket
	{

		private const string C_CREATE_CLAN = "[C] C_CreateClan";

		public C_CreateClan(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			string s = ReadS();
			if (pc.Crown)
			{ // 是王族
				if (pc.Clanid == 0)
				{
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{ // 檢查是否有同名的血盟
						if (clan.ClanName.ToLower().Equals(s.ToLower()))
						{
							pc.sendPackets(new S_ServerMessage(99)); // \f1那個血盟名稱已經存在。
							return;
						}
					}
					if (pc.Inventory.checkItem(L1ItemId.ADENA, 30000))
					{ // 身上有金幣3萬
						L1Clan clan = ClanTable.Instance.createClan(pc, s); // 建立血盟
						ClanMembersTable.Instance.newMember(pc);
						if (clan != null)
						{
							pc.Inventory.consumeItem(L1ItemId.ADENA, 30000);
							pc.sendPackets(new S_ServerMessage(84, s)); // 創立\f1%0  血盟。
							pc.sendPackets(new S_ClanName(pc, true));
							pc.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, pc.Clan.EmblemStatus));
							pc.sendPackets(new S_ClanAttention());
						}
					}
					else
					{
						pc.sendPackets(new S_ServerMessage(189)); // \f1金幣不足。
					}
				}
				else
				{
					pc.sendPackets(new S_ServerMessage(86)); // \f1已經創立血盟。
				}
			}
			else
			{
				pc.sendPackets(new S_ServerMessage(85)); // \f1王子和公主才可創立血盟。
			}
		}

		public override string Type
		{
			get
			{
				return C_CREATE_CLAN;
			}
		}

	}

}