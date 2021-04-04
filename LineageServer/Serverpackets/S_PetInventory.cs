using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_PetInventory : ServerBasePacket
    {

        private const string S_PET_INVENTORY = "[S] S_PetInventory";

        private byte[] _byte = null;

        public S_PetInventory(L1PetInstance pet)
        {
            IList<L1ItemInstance> itemList = pet.Inventory.Items;

            WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
            WriteD(pet.Id);
            WriteH(itemList.Count);
            WriteC(0x0b);

            foreach (object itemObject in itemList)
            {
                L1ItemInstance petItem = (L1ItemInstance)itemObject;
                if (petItem == null)
                {
                    continue;
                }
                WriteD(petItem.Id);
                WriteC(0x02); // 值:0x00  無、0x01:武器類、0x02:防具類、0x16:牙齒類 、0x33:藥水類
                WriteH(petItem.get_gfxid());
                WriteC(petItem.Bless);
                WriteD(petItem.Count);

                // 顯示裝備中的寵物裝備
                if (petItem.Item.Type2 == 0 && petItem.Item.Type == 11 && petItem.Equipped)
                {
                    WriteC(petItem.Identified ? 3 : 2);
                }
                else
                {
                    WriteC(petItem.Identified ? 1 : 0);
                }
                WriteS(petItem.ViewName);

            }
            WriteC(pet.Ac); // 寵物防禦
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