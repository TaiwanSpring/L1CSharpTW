using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System.Collections.Generic;
namespace LineageServer.Serverpackets
{
    class S_PrivateShop : ServerBasePacket
    {

        public S_PrivateShop(L1PcInstance pc, int objectId, int type)
        {
            L1PcInstance shopPc = (L1PcInstance)Container.Instance.Resolve<IGameWorld>().findObject(objectId);

            if (shopPc == null)
            {
                return;
            }

            WriteC(Opcodes.S_OPCODE_PRIVATESHOPLIST);
            WriteC(type);
            WriteD(objectId);

            if (type == 0)
            {
                IList<L1PrivateShopSellList> list = shopPc.SellList;
                int size = list.Count;
                pc.PartnersPrivateShopItemCount = size;
                WriteH(size);
                for (int i = 0; i < size; i++)
                {
                    L1PrivateShopSellList pssl = list[i];
                    int itemObjectId = pssl.ItemObjectId;
                    int count = pssl.SellTotalCount - pssl.SellCount;
                    int price = pssl.SellPrice;
                    L1ItemInstance item = shopPc.Inventory.getItem(itemObjectId);
                    if (item != null)
                    {
                        WriteC(i);
                        WriteC(item.Bless);
                        WriteH(item.Item.GfxId);
                        WriteD(count);
                        WriteD(price);
                        WriteS(item.getNumberedViewName(count));
                        WriteC(0);
                    }
                }
            }
            else if (type == 1)
            {
                IList<L1PrivateShopBuyList> list = shopPc.BuyList;
                int size = list.Count;
                WriteH(size);
                for (int i = 0; i < size; i++)
                {
                    L1PrivateShopBuyList psbl = list[i];
                    int itemObjectId = psbl.ItemObjectId;
                    int count = psbl.BuyTotalCount;
                    int price = psbl.BuyPrice;
                    L1ItemInstance item = shopPc.Inventory.getItem(itemObjectId);
                    foreach (L1ItemInstance pcItem in pc.Inventory.Items)
                    {
                        if ((item.ItemId == pcItem.ItemId) && (item.EnchantLevel == pcItem.EnchantLevel))
                        {
                            WriteC(i);
                            WriteD(pcItem.Id);
                            WriteD(count);
                            WriteD(price);
                        }
                    }
                }
            }
        }
    }

}