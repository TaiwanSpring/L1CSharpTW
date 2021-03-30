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
namespace LineageServer.Serverpackets
{

	using Opcodes = LineageServer.Server.Opcodes;
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using ListFactory = LineageServer.Utils.ListFactory;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_SystemMessage

	public class S_FixWeaponList : ServerBasePacket
	{

		private const string S_FIX_WEAPON_LIST = "[S] S_FixWeaponList";

		public S_FixWeaponList(L1PcInstance pc)
		{
			buildPacket(pc);
		}

		private void buildPacket(L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_SELECTLIST);
			WriteD(0x000000c8); // Price

			IList<L1ItemInstance> weaponList = ListFactory.NewList();
			IList<L1ItemInstance> itemList = pc.Inventory.Items;
			foreach (L1ItemInstance item in itemList)
			{

				// Find Weapon
				switch (item.Item.Type2)
				{
					case 1:
						if (item.get_durability() > 0)
						{
							weaponList.Add(item);
						}
						break;
				}
			}

			WriteH(weaponList.Count); // Weapon Amount

			foreach (L1ItemInstance weapon in weaponList)
			{

				WriteD(weapon.Id); // Item ID
				WriteC(weapon.get_durability()); // Fix Level
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_FIX_WEAPON_LIST;
			}
		}
	}
}