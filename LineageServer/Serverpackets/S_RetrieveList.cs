using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_RetrieveList : ServerBasePacket
    {
        public S_RetrieveList(int objid, L1PcInstance pc)
        {
            if (pc.Inventory.Size < 180)
            {
                int size = pc.DwarfInventory.Size;
                if (size > 0)
                {
                    WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
                    WriteD(objid);
                    WriteH(size);
                    WriteC(3); // 個人倉庫
                    foreach (object itemObject in pc.DwarfInventory.Items)
                    {
                        L1ItemInstance item = (L1ItemInstance)itemObject;
                        WriteD(item.Id);
                        WriteC(item.Item.UseType); // 道具:0 武器:1  防具:2...
                        WriteH(item.get_gfxid());
                        WriteC(item.Bless);
                        WriteD(item.Count);
                        WriteC(item.Identified ? 1 : 0);
                        WriteS(item.ViewName);
                    }
                    WriteD(0x0000001e); // 金幣30
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