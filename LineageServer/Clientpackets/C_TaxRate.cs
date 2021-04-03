using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來稅率的封包
	/// </summary>
	class C_TaxRate : ClientBasePacket
	{

		private const string C_TAX_RATE = "[C] C_TaxRate";
		public C_TaxRate(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance player = clientthread.ActiveChar;
			if (player == null)
			{
				return;
			}

			int i = ReadD();
			int j = ReadC();

			if (i == player.Id)
			{
				L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(player.Clanname);
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