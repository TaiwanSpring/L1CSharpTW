using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Serverpackets
{
    class S_FixWeaponList : ServerBasePacket
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

            IList<L1ItemInstance> weaponList = ListFactory.NewList<L1ItemInstance>();
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
        public override string Type
        {
            get
            {
                return S_FIX_WEAPON_LIST;
            }
        }
    }
}