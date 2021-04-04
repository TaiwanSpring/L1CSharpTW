using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_RetrievePledgeList : ServerBasePacket
    {
        public S_RetrievePledgeList(int objid, L1PcInstance pc)
        {
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan == null)
            {
                return;
            }

            if (clan.WarehouseUsingChar != 0 && clan.WarehouseUsingChar != pc.Id) // 自キャラ以外がクラン倉庫使用中
            {
                pc.sendPackets(new S_ServerMessage(209)); // \f1他の血盟員が倉庫を使用中です。しばらく経ってから利用してください。
                return;
            }

            if (pc.Inventory.Size < 180)
            {
                int size = clan.DwarfForClanInventory.Size;
                if (size > 0)
                {
                    clan.WarehouseUsingChar = pc.Id; // クラン倉庫をロック
                    WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
                    WriteD(objid);
                    WriteH(size);
                    WriteC(5); // 血盟倉庫
                    foreach (object itemObject in clan.DwarfForClanInventory.Items)
                    {
                        L1ItemInstance item = (L1ItemInstance)itemObject;
                        WriteD(item.Id);
                        WriteC(item.Item.UseType);
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