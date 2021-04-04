using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.shop;
using LineageServer.Server.Templates;
using System.Collections.Generic;
namespace LineageServer.Serverpackets
{
    class S_ShopSellList : ServerBasePacket
    {

        /// <summary>
        /// 商店販賣的物品清單
        /// 店の品物リストを表示する。キャラクターがBUYボタンを押した時に送る。
        /// </summary>
        public S_ShopSellList(int objId, L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_SHOWSHOPBUYLIST);
            WriteD(objId);

            GameObject npcObj = Container.Instance.Resolve<IGameWorld>().findObject(objId);
            if (!(npcObj is L1NpcInstance))
            {
                WriteH(0);
                return;
            }
            int npcId = ((L1NpcInstance)npcObj).NpcTemplate.get_npcId();

            L1TaxCalculator calc = new L1TaxCalculator(npcId);
            L1Shop shop = ShopTable.Instance.get(npcId);
            IList<L1ShopItem> shopItems = shop.SellingItems;

            WriteH(shopItems.Count);

            // L1ItemInstanceのgetStatusBytesを利用するため
            L1ItemInstance dummy = new L1ItemInstance();

            for (int i = 0; i < shopItems.Count; i++)
            {
                L1ShopItem shopItem = shopItems[i];
                L1Item item = shopItem.Item;
                int price = calc.layTax((int)(shopItem.Price * Config.RATE_SHOP_SELLING_PRICE));
                WriteD(i);
                WriteH(shopItem.Item.GfxId);
                WriteD(price);

                if (shopItem.PackCount > 1)
                {
                    WriteS($"{item.Name} ({shopItem.PackCount})");
                }
                else
                {
                    if (item.ItemId == 40309)
                    {
                        // 食人妖精RaceTicket
                        string[] temp = item.Name.Split(' ');
                        string buf = temp[temp.Length - 1];
                        temp = buf.Split('-');
                        WriteS(buf + " $" + (1212 + int.Parse(temp[temp.Length - 1])));
                    }
                    else
                    {
                        WriteS(item.Name);
                    }
                }

                L1Item template = ItemTable.Instance.getTemplate(item.ItemId);
                if (template == null)
                {
                    WriteC(0);
                }
                else
                {
                    dummy.Item = template;
                    byte[] status = dummy.StatusBytes;
                    WriteC(status.Length);
                    foreach (byte b in status)
                    {
                        WriteC(b);
                    }
                }
            }
            WriteH(0x07); // 0x00:kaimo 0x01:pearl 0x07:adena
        }
    }

}