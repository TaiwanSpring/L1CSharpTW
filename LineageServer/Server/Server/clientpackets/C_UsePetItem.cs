/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.Clientpackets
{
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using PetItemTable = LineageServer.Server.Server.datatables.PetItemTable;
	using PetTypeTable = LineageServer.Server.Server.datatables.PetTypeTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using S_PetEquipment = LineageServer.Server.Server.serverpackets.S_PetEquipment;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using L1PetItem = LineageServer.Server.Server.Templates.L1PetItem;
	using L1PetType = LineageServer.Server.Server.Templates.L1PetType;

	// Referenced classes of package l1j.server.server.clientpackets:
	// ClientBasePacket

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

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public C_UsePetItem(byte abyte0[], l1j.server.server.ClientThread clientthread) throws Exception
		public C_UsePetItem(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int data = readC();
			int petId = readD();
			int listNo = readC();

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