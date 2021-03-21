using System.Collections.Generic;

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
namespace LineageServer.Server.Server.serverpackets
{

	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_PetInventory : ServerBasePacket
	{

		private const string S_PET_INVENTORY = "[S] S_PetInventory";

		private byte[] _byte = null;

		public S_PetInventory(L1PetInstance pet)
		{
			IList<L1ItemInstance> itemList = pet.Inventory.Items;

			writeC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
			writeD(pet.Id);
			writeH(itemList.Count);
			writeC(0x0b);

			foreach (object itemObject in itemList)
			{
				L1ItemInstance petItem = (L1ItemInstance) itemObject;
				if (petItem == null)
				{
					continue;
				}
				writeD(petItem.Id);
				writeC(0x02); // 值:0x00  無、0x01:武器類、0x02:防具類、0x16:牙齒類 、0x33:藥水類
				writeH(petItem.get_gfxid());
				writeC(petItem.Bless);
				writeD(petItem.Count);

				// 顯示裝備中的寵物裝備
				if (petItem.Item.Type2 == 0 && petItem.Item.Type == 11 && petItem.Equipped)
				{
					writeC(petItem.Identified ? 3 : 2);
				}
				else
				{
					writeC(petItem.Identified ? 1 : 0);
				}
				writeS(petItem.ViewName);

			}
			writeC(pet.Ac); // 寵物防禦
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_PET_INVENTORY;
			}
		}
	}

}