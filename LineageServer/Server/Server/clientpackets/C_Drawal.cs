using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// TODO: 處理收到由客戶端傳來取得城堡稅收的封包(?)
	/// </summary>
	class C_Drawal : ClientBasePacket
	{

		private const string C_DRAWAL = "[C] C_Drawal";
		public C_Drawal(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			readD();
			int j = Math.Abs(readD());

			L1Clan clan = L1World.Instance.getClan(pc.Clanname);
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
							L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(L1ItemId.ADENA, j);
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