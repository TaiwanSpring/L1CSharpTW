using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來稅率的封包
	/// </summary>
	class C_TaxRate : ClientBasePacket
	{

		private const string C_TAX_RATE = "[C] C_TaxRate";
		public C_TaxRate(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance player = clientthread.ActiveChar;
			if (player == null)
			{
				return;
			}

			int i = readD();
			int j = readC();

			if (i == player.Id)
			{
				L1Clan clan = L1World.Instance.getClan(player.Clanname);
				if (clan != null)
				{
					int castle_id = clan.CastleId;
					if (castle_id != 0)
					{ // 有城堡
						L1Castle l1castle = CastleTable.Instance.getCastleTable(castle_id);
						if ((j >= 10) && (j <= 50))
						{
							l1castle.TaxRate = j;
							CastleTable.Instance.updateCastle(l1castle);
						}
					}
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_TAX_RATE;
			}
		}

	}

}