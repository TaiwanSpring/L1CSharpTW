using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_PetList : ServerBasePacket
    {

        private const string S_PETLIST = "[S] S_PetList";

        private byte[] _byte = null;

        public S_PetList(int npcObjId, L1PcInstance pc)
        {
            buildPacket(npcObjId, pc);
        }

        private void buildPacket(int npcObjId, L1PcInstance pc)
        {
            IList<L1ItemInstance> amuletList = ListFactory.NewList<L1ItemInstance>();
            // 判斷身上是否有寵物項圈！
            foreach (L1ItemInstance item in pc.Inventory.Items)
            {
                if ((item.Item.ItemId == 40314) || (item.Item.ItemId == 40316))
                {
                    if (!isWithdraw(pc, item))
                    {
                        amuletList.Add(item);
                    }
                }
            }

            if (amuletList.Count != 0)
            {
                WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
                WriteD(npcObjId);
                WriteH(amuletList.Count);
                WriteC(0x0c);
                foreach (L1ItemInstance item in amuletList)
                {
                    WriteD(item.Id);
                    WriteC(0x00);
                    WriteH(item.get_gfxid());
                    WriteC(item.Bless);
                    WriteD(item.Count);
                    WriteC(item.Identified ? 1 : 0);
                    WriteS(item.ViewName);
                }
            }
            else
            {
                return;
            }
            WriteD(0x00000073); // Price
        }

        private bool isWithdraw(L1PcInstance pc, L1ItemInstance item)
        {
            foreach (L1NpcInstance petNpc in pc.PetList.Values)
            {
                if (petNpc is L1PetInstance)
                {
                    L1PetInstance pet = (L1PetInstance)petNpc;
                    if (item.Id == pet.ItemObjId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string Type
        {
            get
            {
                return S_PETLIST;
            }
        }
    }

}