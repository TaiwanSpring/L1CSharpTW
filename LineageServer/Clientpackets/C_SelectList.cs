using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using System;
namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來選擇清單的封包
	/// </summary>
	class C_SelectList : ClientBasePacket
	{

		private const string C_SELECT_LIST = "[C] C_SelectList";

		public C_SelectList(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			// アイテム毎にリクエストが来る。
			int itemObjectId = ReadD();
			int npcObjectId = ReadD();


			if (npcObjectId != 0)
			{ // 武器的修理
				GameObject obj = Container.Instance.Resolve<IGameWorld>().findObject(npcObjectId);
				if (obj != null)
				{
					if (obj is L1NpcInstance)
					{
						L1NpcInstance npc = (L1NpcInstance) obj;
						int difflocx = Math.Abs(pc.X - npc.X);
						int difflocy = Math.Abs(pc.Y - npc.Y);
						// 3格以上的距離視為無效請求
						if ((difflocx > 3) || (difflocy > 3))
						{
							return;
						}
					}
				}

				L1PcInventory pcInventory = pc.Inventory as L1PcInventory;
				L1ItemInstance item = pcInventory.getItem(itemObjectId);
				int cost = item.get_durability() * 200;
				if (!pc.Inventory.consumeItem(L1ItemId.ADENA, cost))
				{
					return;
				}
				item.set_durability(0);
				pcInventory.updateItem(item, L1PcInventory.COL_DURABILITY);
			}
		}

		public override string Type
		{
			get
			{
				return C_SELECT_LIST;
			}
		}
	}

}