
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來給道具的封包
    /// </summary>
    class C_GiveItem : ClientBasePacket
	{
		private const string C_GIVE_ITEM = "[C] C_GiveItem";

		public C_GiveItem(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}
			int targetId = readD();
			readH();
			readH();
			int itemId = readD();
			int count = readD();

			L1Object l1Object = L1World.Instance.findObject(targetId);
			if ((l1Object == null) || !(l1Object is L1NpcInstance))
			{
				return;
			}
			L1NpcInstance target = (L1NpcInstance) l1Object;
			if (!isNpcItemReceivable(target.NpcTemplate))
			{
				return;
			}
			L1Inventory targetInv = target.Inventory;

			L1Inventory inv = pc.Inventory;
			L1ItemInstance item = inv.getItem(itemId);
			if (item == null)
			{
				return;
			}
			// 只能給以下物品 by mca 20081013
			if (pc.Gm || item.ItemId == 40010 || item.ItemId == 40011 || item.ItemId == 40012 || item.ItemId == 40013 || item.ItemId == 40018 || item.ItemId == 40056 || item.ItemId == 40057 || item.ItemId == 40070 || item.ItemId == 88 || item.ItemId == 40507 || item.ItemId == 40505 || item.ItemId == 40499 || item.ItemId == 40494 || item.ItemId == 40495 || item.ItemId == 40520 || item.ItemId == 40521 || item.ItemId == 40508 || item.ItemId == 49302 || item.ItemId == 40060 || item.ItemId == 41310)
			{ // 勝利果實
			}
					else
					{
					  pc.sendPackets(new S_ServerMessage(942)); // 對方的負重太重，無法再給予。
					  return;
					} // end
			if (item.Equipped)
			{
				pc.sendPackets(new S_ServerMessage(141)); // \f1你不能夠將轉移已經裝備的物品。
				return;
			}
			if (!item.Item.Tradable)
			{
				pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0%d是不可轉移的…
				return;
			}
			if (item.Bless >= 128)
			{ // 封印的裝備
				// \f1%0%d是不可轉移的…
				pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
				return;
			}
			// 使用中的寵物項鍊 - 無法給予
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
			// 使用中的魔法娃娃 - 無法給予
			foreach (L1DollInstance doll in pc.DollList.Values)
			{
				if (doll.ItemObjId == item.Id)
				{
					pc.sendPackets(new S_ServerMessage(1181)); // 這個魔法娃娃目前正在使用中。
					return;
				}
			}
			if (targetInv.checkAddItem(item, count) != L1Inventory.OK)
			{
				pc.sendPackets(new S_ServerMessage(942)); // 對方的負重太重，無法再給予。
				return;
			}
			item = inv.tradeItem(item, count, targetInv);
			target.onGetItem(item);
			target.turnOnOffLight();
			pc.turnOnOffLight();

			L1PetType petType = PetTypeTable.Instance.get(target.NpcTemplate.get_npcId());
			if ((petType == null) || target.Dead)
			{
				return;
			}

			// 捕抓寵物
			if (item.ItemId == petType.ItemIdForTaming)
			{
				tamePet(pc, target);
			}
			// 進化寵物
			else if (item.ItemId == petType.EvolvItemId)
			{
				evolvePet(pc, target, item.ItemId);
			}

			if (item.Item.Type2 == 0)
			{ // 道具類
				// 食物類
				if (item.Item.Type == 7)
				{
					eatFood(pc, target, item, count);
				}
				// 寵物裝備類
				else if ((item.Item.Type == 11) && (petType.canUseEquipment()))
				{ // 判斷是否可用寵物裝備
					usePetWeaponArmor(target, item);
				}
			}

		}

		private void eatFood(L1PcInstance pc, L1NpcInstance target, L1ItemInstance item, int count)
		{
			if (!(target is L1PetInstance))
			{
				return;
			}
			L1PetInstance pet = (L1PetInstance) target;
			L1Pet _l1pet = PetTable.Instance.getTemplate(item.Id);
			int food = 0;
			int foodCount = 0;
			bool isFull = false;

			if (pet.get_food() == 100)
			{ // 非常飽
				return;
			}
			// 食物營養度判斷
			if (item.Item.FoodVolume != 0)
			{
				// 吃掉食物的數量判斷
				for (int i = 0; i < count; i++)
				{
					food = item.Item.FoodVolume / 10;
					food += pet.get_food();
					if (!isFull)
					{
						if (food >= 100)
						{
							isFull = true;
							pet.set_food(100);
							foodCount++;
						}
						else
						{
							pet.set_food(food);
							foodCount++;
						}
					}
					else
					{
						break;
					}
				}
				if (foodCount != 0)
				{
					pet.Inventory.consumeItem(item.ItemId, foodCount); // 吃掉食物
					// 紀錄寵物飽食度
					_l1pet.set_food(pet.get_food());
					PetTable.Instance.storePetFood(_l1pet);
				}
			}
		}

		private void usePetWeaponArmor(L1NpcInstance target, L1ItemInstance item)
		{
			if (!(target is L1PetInstance))
			{
				return;
			}
			L1PetInstance pet = (L1PetInstance) target;
			L1PetItem petItem = PetItemTable.Instance.getTemplate(item.ItemId);
			if (petItem.UseType == 1)
			{ // 牙齒
				pet.usePetWeapon(pet, item);
			}
			else if (petItem.UseType == 0)
			{ // 盔甲
				pet.usePetArmor(pet, item);
			}
		}

		private static readonly string[] receivableImpls = new string[] {"L1Npc", "L1Monster", "L1Guardian", "L1Teleporter", "L1Guard"};

		private bool isNpcItemReceivable(L1Npc npc)
		{
			foreach (string impl in receivableImpls)
			{
				if (npc.Impl.Equals(impl))
				{
					return true;
				}
			}
			return false;
		}

		private void tamePet(L1PcInstance pc, L1NpcInstance target)
		{
			if ((target is L1PetInstance) || (target is L1SummonInstance))
			{
				return;
			}

			int petcost = 0;
			foreach (L1NpcInstance petNpc in pc.PetList.Values)
			{
				petcost += petNpc.Petcost;
			}
			int charisma = pc.Cha;
			if (pc.Crown)
			{ // 王族
				charisma += 6;
			}
			else if (pc.Elf)
			{ // 妖精
				charisma += 12;
			}
			else if (pc.Wizard)
			{ // 法師
				charisma += 6;
			}
			else if (pc.Darkelf)
			{ // 黑暗妖精
				charisma += 6;
			}
			else if (pc.DragonKnight)
			{ // 龍騎士
				charisma += 6;
			}
			else if (pc.Illusionist)
			{ // 幻術師
				charisma += 6;
			}
			charisma -= petcost;

			L1PcInventory inv = pc.Inventory;
			if ((charisma >= 6) && (inv.Size < 180))
			{
				if (isTamePet(target))
				{
					L1ItemInstance petamu = inv.storeItem(40314, 1); // 漂浮之眼的肉
					if (petamu != null)
					{
						new L1PetInstance(target, pc, petamu.Id);
						pc.sendPackets(new S_ItemName(petamu));
					}
				}
				else
				{
					pc.sendPackets(new S_ServerMessage(324)); // 馴養失敗。
				}
			}
		}

		private void evolvePet(L1PcInstance pc, L1NpcInstance target, int itemId)
		{
			if (!(target is L1PetInstance))
			{
				return;
			}
			L1PcInventory inv = pc.Inventory;
			L1PetInstance pet = (L1PetInstance) target;
			L1ItemInstance petamu = inv.getItem(pet.ItemObjId);
			if (((pet.Level >= 30) || (itemId == 41310)) && (pc == pet.Master) && (petamu != null))
			{
				L1ItemInstance highpetamu = inv.storeItem(40316, 1);
				if (highpetamu != null)
				{
					pet.evolvePet(highpetamu.Id);
					pc.sendPackets(new S_ItemName(highpetamu));
					inv.removeItem(petamu, 1);
				}
			}
		}

		private bool isTamePet(L1NpcInstance npc)
		{
			bool isSuccess = false;
			int npcId = npc.NpcTemplate.get_npcId();
			if (npcId == 45313)
			{ // タイガー
				if ((npc.MaxHp / 3 > npc.CurrentHp) && (RandomHelper.Next(16) == 15))
				{
					isSuccess = true;
				}
			}
			else
			{
				if (npc.MaxHp / 3 > npc.CurrentHp)
				{
					isSuccess = true;
				}
			}

			if ((npcId == 45313) || (npcId == 45044) || (npcId == 45711))
			{ // タイガー、ラクーン、紀州犬の子犬
				if (npc.Resurrect)
				{ // RES後はテイム不可
					isSuccess = false;
				}
			}

			return isSuccess;
		}

		public override string Type
		{
			get
			{
				return C_GIVE_ITEM;
			}
		}
	}

}