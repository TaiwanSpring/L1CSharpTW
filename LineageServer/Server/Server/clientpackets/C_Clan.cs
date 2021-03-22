using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來的要求血盟徽章封包
	/// </summary>
	class C_Clan : ClientBasePacket
	{

		private const string C_CLAN = "[C] C_Clan";

		public C_Clan(sbyte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int clanId = readD();
			L1Clan clan = ClanTable.Instance.getTemplate(clanId);
			pc.sendPackets(new S_Emblem(clan.ClanId));
		}

		public override string Type
		{
			get
			{
				return C_CLAN;
			}
		}

	}

}