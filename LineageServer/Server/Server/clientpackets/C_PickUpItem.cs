using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來撿起道具的封包
	/// </summary>
	class C_PickUpItem : ClientBasePacket
	{

		private const string C_PICK_UP_ITEM = "[C] C_PickUpItem";
		public C_PickUpItem(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if ((pc == null) || pc.Dead || pc.Ghost)
			{
				return;
			}

			int x = readH();
			int y = readH();
			int objectId = readD();
			int pickupCount = readD();

			if (objectId == pc.Id)
			{
				return;
			}

			if (pc.Invisble)
			{ // 隱身狀態
				return;
			}
			if (pc.InvisDelay)
			{ // 還在解除隱身的延遲
				return;
			}

			L1Inventory groundInventory = L1World.Instance.getInventory(x, y, pc.MapId);
			L1Object @object = groundInventory.getItem(objectId);

			if ((@object != null) && !pc.Dead)
			{
				L1ItemInstance item = (L1ItemInstance) @object;
				if ((item.ItemOwnerId != 0) && (pc.Id != item.ItemOwnerId))
				{
					pc.sendPackets(new S_ServerMessage(623)); // アイテムが拾えませんでした。
					return;
				}
				if (pc.Location.getTileLineDistance(item.Location) > 3)
				{
					return;
				}

				if (item.Item.ItemId == L1ItemId.ADENA)
				{
					L1ItemInstance inventoryItem = pc.Inventory.findItemId(L1ItemId.ADENA);
					int inventoryItemCount = 0;
					if (inventoryItem != null)
					{
						inventoryItemCount = inventoryItem.Count;
					}
					// 超過20億
					if ((long) inventoryItemCount + (long) pickupCount > 2000000000L)
					{
						pc.sendPackets(new S_ServerMessage(166, "你身上的金幣已經超過", "2,000,000,000了，所以不能撿取金幣。"));
						return;
					}
				}

				if (pc.Inventory.checkAddItem(item, pickupCount) == L1Inventory.OK)
				{
					if ((item.X != 0) && (item.Y != 0))
					{ // ワールドマップ上のアイテム
						groundInventory.tradeItem(item, pickupCount, pc.Inventory);
						pc.turnOnOffLight();

						S_AttackPacket s_attackPacket = new S_AttackPacket(pc, objectId, ActionCodes.ACTION_Pickup);
						pc.sendPackets(s_attackPacket);
						if (!pc.GmInvis)
						{
							pc.broadcastPacket(s_attackPacket);
						}
					}
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_PICK_UP_ITEM;
			}
		}
	}

}