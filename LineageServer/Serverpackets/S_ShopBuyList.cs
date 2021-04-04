using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.shop;
using LineageServer.william;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_ShopBuyList : ServerBasePacket
    {

        private const string S_SHOP_BUY_LIST = "[S] S_ShopBuyList";

        public S_ShopBuyList(int objid, L1PcInstance pc)
        {
            GameObject gameObject = Container.Instance.Resolve<IGameWorld>().findObject(objid);
            if (gameObject is L1NpcInstance npc)
            {
                int npcId = npc.NpcTemplate.get_npcId();
                // 全道具販賣 
                if (Config.ALL_ITEM_SELL)
                {
                    int tax_rate = L1CastleLocation.getCastleTaxRateByNpcId(npcId);

                    List<L1ItemInstance> sellItems = new List<L1ItemInstance>();
                    for (IEnumerator<L1ItemInstance> iterator = pc.Inventory.Items.GetEnumerator(); iterator.MoveNext();)
                    {
                        object iObject = iterator.Current;
                        L1ItemInstance itm = (L1ItemInstance)iObject;
                        if (itm != null && !itm.Equipped && itm.ItemId != 40308 && L1WilliamItemPrice.getItemId(itm.Item.ItemId) != 0)
                        {
                            sellItems.Add(itm);
                        }
                    }

                    int sell = sellItems.Count;
                    if (sell > 0)
                    {
                        WriteC(Opcodes.S_OPCODE_SHOWSHOPSELLLIST);
                        WriteD(objid);
                        WriteH(sell);
                        foreach (object itemObj in sellItems)
                        {
                            L1ItemInstance item = (L1ItemInstance)itemObj;
                            int getPrice = L1WilliamItemPrice.getItemId(item.Item.ItemId);
                            int price = 0;
                            if (getPrice > 0)
                            {
                                price = getPrice;
                            }
                            else
                            {
                                price = 0;
                            }
                            if (tax_rate != 0)
                            {
                                double tax = (100 + tax_rate) / 100.0;
                                price = (int)(price * tax);
                            }
                            WriteD(item.Id);
                            WriteD(price / 2);
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_NoSell(npc));
                    }
                }
                else
                {
                    L1Shop shop = ShopTable.Instance.get(npcId);
                    if (shop == null)
                    {
                        pc.sendPackets(new S_NoSell(npc));
                        return;
                    }

                    IList<L1AssessedItem> assessedItems = shop.assessItems(pc.Inventory as L1PcInventory);
                    if (assessedItems.Count == 0)
                    {
                        pc.sendPackets(new S_NoSell(npc));
                        return;
                    }

                    WriteC(Opcodes.S_OPCODE_SHOWSHOPSELLLIST);
                    WriteD(objid);
                    WriteH(assessedItems.Count);

                    foreach (L1AssessedItem item in assessedItems)
                    {
                        WriteD(item.TargetId);
                        WriteD(item.AssessedPrice);
                    }
                }
                // 全道具販賣  end
                WriteH(0x0007); // 7 = 金幣為單位 顯示總金額
            }
        }

        public override string Type
        {
            get
            {
                return S_SHOP_BUY_LIST;
            }
        }
    }
}