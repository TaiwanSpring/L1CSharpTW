using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來刪除身上道具的封包
	/// </summary>
	class C_DeleteInventoryItem : ClientBasePacket
	{

		private const string C_DELETE_INVENTORY_ITEM = "[C] C_DeleteInventoryItem";

		public C_DeleteInventoryItem(byte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int itemObjectId = ReadD();
			int deleteCount = 0;
			L1ItemInstance item = pc.Inventory.getItem(itemObjectId);

			// 沒有要刪除的道具
			if (item == null)
			{
				return;
			}

			if (item.Item.CantDelete)
			{
				// \f1你不能夠放棄此樣物品。
				pc.sendPackets(new S_ServerMessage(125));
				return;
			}

			// 使用中的寵物項鍊 - 無法刪除
			foreach (L1NpcInstance petNpc in pc.PetList.Values)
			{
				if (petNpc is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance) petNpc;
					if (item.Id == pet.ItemObjId)
					{
						pc.sendPackets(new S_ServerMessage(1187)); // 寵物項鍊正在使用中。
						return;
					}
				}
			}
			// 使用中的魔法娃娃 - 無法刪除
			foreach (L1DollInstance doll in pc.DollList.Values)
			{
				if (doll.ItemObjId == item.Id)
				{
					pc.sendPackets(new S_ServerMessage(1181)); // 這個魔法娃娃目前正在使用中。
					return;
				}
			}

			if (item.Equipped)
			{
				// \f1削除できないアイテムや装備しているアイテムは捨てられません。
				pc.sendPackets(new S_ServerMessage(125));
				return;
			}
			if (item.Bless >= 128)
			{ // 封印された装備
				// \f1%0は捨てたりまたは他人に讓ることができません。
				pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
				return;
			}

			if (item.Count > 1)
			{
				deleteCount = ReadD();
				pc.Inventory.removeItem(item, deleteCount);
			}
			else
			{
				pc.Inventory.removeItem(item, item.Count);
			}
			pc.turnOnOffLight();
		}

		public override string Type
		{
			get
			{
				return C_DELETE_INVENTORY_ITEM;
			}
		}
	}

}