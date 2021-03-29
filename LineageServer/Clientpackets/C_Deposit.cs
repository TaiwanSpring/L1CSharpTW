using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// TODO: 處理收到由客戶端傳來納稅的封包(?)
	/// </summary>
	class C_Deposit : ClientBasePacket
	{

		private const string C_DEPOSIT = "[C] C_Deposit";
		public C_Deposit(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance player = clientthread.ActiveChar;
			if (player == null)
			{
				return;
			}

			int i = ReadD();
			int j = ReadD();

			if (i == player.Id)
			{
				L1Clan clan = L1World.Instance.getClan(player.Clanname);
				if (clan != null)
				{
					int castle_id = clan.CastleId;
					if (castle_id != 0)
					{ // 有城堡的盟主
						L1Castle l1castle = CastleTable.Instance.getCastleTable(castle_id);
						lock (l1castle)
						{
							int money = l1castle.PublicMoney;
							if (player.Inventory.consumeItem(L1ItemId.ADENA, j))
							{
								money += j;
								l1castle.PublicMoney = money;
								CastleTable.Instance.updateCastle(l1castle);
							}
						}
					}
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_DEPOSIT;
			}
		}

	}

}