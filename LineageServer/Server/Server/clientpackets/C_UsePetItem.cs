
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來使用寵物道具的封包
	/// </summary>
	class C_UsePetItem : ClientBasePacket
	{

		/// <summary>
		/// 【Client】 id:60 size:8 time:1302335819781
		/// 0000	3c 00 04 bd 54 00 00 00                            <...T...
		/// 
		/// 【Server】 id:82 size:16 time:1302335819812
		/// 0000	52 25 00 04 bd 54 00 00 0a 37 80 08 7e ec d0 46    R%...T...7..~..F
		/// </summary>

		private const string C_USE_PET_ITEM = "[C] C_UsePetItem";

		public C_UsePetItem(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int data = ReadC();
			int petId = ReadD();
			int listNo = ReadC();

			L1PetInstance pet = (L1PetInstance) L1World.Instance.findObject(petId);
			if (pet == null)
			{
				return;
			}
			L1ItemInstance item = pet.Inventory.Items[listNo];
			if (item == null)
			{
				return;
			}

			if ((item.Item.Type2 == 0) && (item.Item.Type == 11))
			{ // 寵物道具
				L1PetType petType = PetTypeTable.Instance.get(pet.NpcTemplate.get_npcId());
				// 判斷是否可用寵物裝備
				if (!petType.canUseEquipment())
				{
					pc.sendPackets(new S_ServerMessage(74, item.LogName));
					return;
				}
				int itemId = item.Item.ItemId;
				L1PetItem petItem = PetItemTable.Instance.getTemplate(itemId);
				if (petItem.UseType == 1)
				{ // 牙齒
					pet.usePetWeapon(pet, item);
					pc.sendPackets(new S_PetEquipment(data, pet, listNo)); // 裝備時更新寵物資訊
				}
				else if (petItem.UseType == 0)
				{ // 盔甲
					pet.usePetArmor(pet, item);
					pc.sendPackets(new S_PetEquipment(data, pet, listNo)); // 裝備時更新寵物資訊
				}
				else
				{
					pc.sendPackets(new S_ServerMessage(74, item.LogName));
				}
			}
			else
			{
				pc.sendPackets(new S_ServerMessage(74, item.LogName));
			}
		}

		public override string Type
		{
			get
			{
				return C_USE_PET_ITEM;
			}
		}
	}

}