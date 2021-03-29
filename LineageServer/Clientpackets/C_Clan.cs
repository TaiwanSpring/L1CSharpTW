using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來的要求血盟徽章封包
	/// </summary>
	class C_Clan : ClientBasePacket
	{

		private const string C_CLAN = "[C] C_Clan";

		public C_Clan(byte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int clanId = ReadD();
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