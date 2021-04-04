using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_RetrieveElfList : ServerBasePacket
    {
        public S_RetrieveElfList(int objid, L1PcInstance pc)
        {
            if (pc.Inventory.Size < 180)
            {
                int size = pc.DwarfForElfInventory.Size;
                if (size > 0)
                {
                    WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
                    WriteD(objid);
                    WriteH(size);
                    WriteC(9); // エルフ倉庫
                    foreach (object itemObject in pc.DwarfForElfInventory.Items)
                    {
                        L1ItemInstance item = (L1ItemInstance)itemObject;
                        WriteD(item.Id);
                        WriteC(0);
                        WriteH(item.get_gfxid());
                        WriteC(item.Bless);
                        WriteD(item.Count);
                        WriteC(item.Identified ? 1 : 0);
                        WriteS(item.ViewName);
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(1625));
                }
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(263)); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
            }
        }
    }
}