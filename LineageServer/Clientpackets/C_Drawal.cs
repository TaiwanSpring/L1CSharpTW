using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using LineageServer.Server;
using LineageServer.Interfaces;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// TODO: 處理收到由客戶端傳來取得城堡稅收的封包(?)
	/// </summary>
	class C_Drawal : ClientBasePacket
	{

		private const string C_DRAWAL = "[C] C_Drawal";
		public C_Drawal(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			ReadD();
			int j = Math.Abs(ReadD());

			L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
			if (clan != null)
			{
				int castle_id = clan.CastleId;
				if (castle_id != 0)
				{
					L1Castle l1castle = CastleTable.Instance.getCastleTable(castle_id);
					int money = l1castle.PublicMoney;
					money -= j;
					L1ItemInstance item = ItemTable.Instance.createItem(L1ItemId.ADENA);
					if (item != null)
					{
						l1castle.PublicMoney = money;
						CastleTable.Instance.updateCastle(l1castle);
						if (pc.Inventory.checkAddItem(item, j) == L1Inventory.OK)
						{
							pc.Inventory.storeItem(L1ItemId.ADENA, j);
						}
						else
						{
							Container.Instance.Resolve<IGameWorld>().getInventory(pc.X, pc.Y, pc.MapId).storeItem(L1ItemId.ADENA, j);
						}
						pc.sendPackets(new S_ServerMessage(143, "$457", $"$4 ({j})")); // \f1%0%s
																								 // 給你
																								 // %1%o
																								 // 。
					}
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_DRAWAL;
			}
		}

	}

}