using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.shop;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.william;
using System;
using System.Collections.Generic;
using LineageServer.Server;
using LineageServer.Interfaces;
using System.Extensions;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// TODO 翻譯，好多 處理收到由客戶端傳來取得結果的封包
    /// </summary>
    class C_Result : ClientBasePacket
    {

        private const string C_RESULT = "[C] C_Result";
        public C_Result(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int npcObjectId = ReadD();
            int resultType = ReadC();
            int size = ReadH();

            int level = pc.Level;

            int npcId = 0;
            string npcImpl = "";
            bool isPrivateShop = false;
            bool tradable = true;
            GameObject findObject = Container.Instance.Resolve<IGameWorld>().findObject(npcObjectId);
            if (findObject != null)
            {
                int diffLocX = Math.Abs(pc.X - findObject.X);
                int diffLocY = Math.Abs(pc.Y - findObject.Y);
                // 5格以上的距離視為無效要求
                if ((diffLocX > 5) || (diffLocY > 5))
                {
                    return;
                }
                if (findObject is L1NpcInstance)
                {
                    L1NpcInstance targetNpc = (L1NpcInstance)findObject;
                    npcId = targetNpc.NpcTemplate.get_npcId();
                    npcImpl = targetNpc.NpcTemplate.Impl;
                }
                else if (findObject is L1PcInstance)
                {
                    isPrivateShop = true;
                }
            }

            if ((resultType == 0) && (size != 0) && npcImpl == "L1Merchant")
            {
                // 買道具
                L1Shop shop = ShopTable.Instance.get(npcId);
                L1ShopBuyOrderList orderList = shop.newBuyOrderList();
                for (int i = 0; i < size; i++)
                {
                    orderList.add(ReadD(), ReadD());
                }
                shop.sellItems(pc, orderList);
            }
            else if ((resultType == 1) && (size != 0) && npcImpl == "L1Merchant")
            { // 賣道具
              // 全道具販賣 
                if (Config.ALL_ITEM_SELL)
                {
                    int objectId, count;
                    L1ItemInstance item;
                    int totalPrice = 0;
                    int tax_rate = L1CastleLocation.getCastleTaxRateByNpcId(npcId);
                    for (int i = 0; i < size; i++)
                    {
                        objectId = ReadD();
                        count = ReadD();

                        item = pc.Inventory.getItem(objectId);
                        if (item == null)
                        {
                            continue;
                        }
                        if (item.Equipped)
                        {
                            continue;
                        }
                        count = pc.Inventory.removeItem(item, count); // 削除

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
                        price = price * count / 2;
                        totalPrice += price;
                    }
                    totalPrice = totalPrice.Ensure(0, 2000000000);
                    if (0 < totalPrice)
                    {
                        pc.Inventory.storeItem(L1ItemId.ADENA, totalPrice);
                    }
                }
                else
                {
                    L1Shop shop = ShopTable.Instance.get(npcId);
                    L1ShopSellOrderList orderList = shop.newSellOrderList(pc);
                    for (int i = 0; i < size; i++)
                    {
                        orderList.add(ReadD(), ReadD());
                    }
                    shop.buyItems(orderList);
                }
                // 全道具販賣  end
            }
            else if ((resultType == 2) && (size != 0) && npcImpl == "L1Dwarf" && (level >= 5))
            { // 自己的倉庫
                int objectId, count;
                for (int i = 0; i < size; i++)
                {
                    tradable = true;
                    objectId = ReadD();
                    count = ReadD();
                    GameObject @object = pc.Inventory.getItem(objectId);
                    L1ItemInstance item = (L1ItemInstance)@object;
                    if (!item.Item.Tradable)
                    {
                        tradable = false;
                        pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0は捨てたりまたは他人に讓ることができません。
                    }
                    foreach (L1NpcInstance petNpc in pc.PetList.Values)
                    {
                        if (petNpc is L1PetInstance)
                        {
                            L1PetInstance pet = (L1PetInstance)petNpc;
                            if (item.Id == pet.ItemObjId)
                            {
                                tradable = false;
                                // \f1%0は捨てたりまたは他人に讓ることができません。
                                pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
                                break;
                            }
                        }
                    }
                    foreach (L1DollInstance doll in pc.DollList.Values)
                    {
                        if (item.Id == doll.ItemObjId)
                        {
                            tradable = false;
                            pc.sendPackets(new S_ServerMessage(1181)); // 該当のマジックドールは現在使用中です。
                            break;
                        }
                    }
                    if (pc.DwarfInventory.checkAddItemToWarehouse(item, count, L1Inventory.WAREHOUSE_TYPE_PERSONAL) == L1Inventory.SIZE_OVER)
                    {
                        pc.sendPackets(new S_ServerMessage(75)); // \f1これ以上ものを置く場所がありません。
                        break;
                    }
                    if (tradable)
                    {
                        pc.Inventory.tradeItem(objectId, count, pc.DwarfInventory);
                        pc.turnOnOffLight();
                    }
                }

                // 強制儲存一次身上道具, 避免角色背包內的物品未正常寫入導致物品複製的問題
                pc.saveInventory();
            }
            else if ((resultType == 3) && (size != 0) && npcImpl == "L1Dwarf" && (level >= 5))
            { // 從倉庫取出東西
                int objectId, count;
                L1ItemInstance item;
                for (int i = 0; i < size; i++)
                {
                    objectId = ReadD();
                    count = ReadD();
                    item = pc.DwarfInventory.getItem(objectId);
                    if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK) // 檢查重量與容量
                    {
                        if (pc.Inventory.consumeItem(L1ItemId.ADENA, 30))
                        {
                            pc.DwarfInventory.tradeItem(item, count, pc.Inventory);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                            break;
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(270)); // \f1持っているものが重くて取引できません。
                        break;
                    }
                }
            }
            else if ((resultType == 4) && (size != 0) && npcImpl == "L1Dwarf" && (level >= 5))
            { // 儲存道具到血盟倉庫
                int objectId, count;
                if (pc.Clanid != 0)
                { // 有血盟
                    for (int i = 0; i < size; i++)
                    {
                        tradable = true;
                        objectId = ReadD();
                        count = ReadD();
                        L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                        GameObject @object = pc.Inventory.getItem(objectId);
                        L1ItemInstance item = (L1ItemInstance)@object;
                        if (clan != null)
                        {
                            if (!item.Item.Tradable)
                            {
                                tradable = false;
                                pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0は捨てたりまたは他人に讓ることができません。
                            }
                            if (item.Bless >= 128)
                            { // 被封印的裝備
                                tradable = false;
                                pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0は捨てたりまたは他人に讓ることができません。
                            }
                            foreach (L1NpcInstance petNpc in pc.PetList.Values)
                            {
                                if (petNpc is L1PetInstance)
                                {
                                    L1PetInstance pet = (L1PetInstance)petNpc;
                                    if (item.Id == pet.ItemObjId)
                                    {
                                        tradable = false;
                                        // \f1%0は捨てたりまたは他人に讓ることができません。
                                        pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
                                        break;
                                    }
                                }
                            }
                            foreach (L1DollInstance doll in pc.DollList.Values)
                            {
                                if (item.Id == doll.ItemObjId)
                                {
                                    tradable = false;
                                    pc.sendPackets(new S_ServerMessage(1181)); // 該当のマジックドールは現在使用中です。
                                    break;

                                }
                            }
                            if (clan.DwarfForClanInventory.checkAddItemToWarehouse(item, count, L1Inventory.WAREHOUSE_TYPE_CLAN) == L1Inventory.SIZE_OVER)
                            {
                                pc.sendPackets(new S_ServerMessage(75)); // \f1これ以上ものを置く場所がありません。
                                break;
                            }
                            if (tradable)
                            {
                                pc.Inventory.tradeItem(objectId, count, clan.DwarfForClanInventory);
                                clan.DwarfForClanInventory.writeHistory(pc, item, count, 0); // 血盟倉庫存入紀錄
                                pc.turnOnOffLight();
                            }
                        }
                    }

                    // 強制儲存一次身上道具, 避免角色背包內的物品未正常寫入導致物品複製的問題
                    pc.saveInventory();
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(208)); // \f1血盟倉庫を使用するには血盟に加入していなくてはなりません。
                }
            }
            else if ((resultType == 5) && (size != 0) && npcImpl == "L1Dwarf" && (level >= 5))
            { // 從克萊因血盟倉庫中取出道具
                int objectId, count;
                L1ItemInstance item;

                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    for (int i = 0; i < size; i++)
                    {
                        objectId = ReadD();
                        count = ReadD();
                        item = clan.DwarfForClanInventory.getItem(objectId);
                        if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                        { // 容量重量確認及びメッセージ送信
                            if (pc.Inventory.consumeItem(L1ItemId.ADENA, 30))
                            {
                                clan.DwarfForClanInventory.tradeItem(item, count, pc.Inventory);
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                                break;
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(270)); // \f1持っているものが重くて取引できません。
                            break;
                        }
                        clan.DwarfForClanInventory.writeHistory(pc, item, count, 1); // 血盟倉庫領出紀錄
                    }
                    clan.WarehouseUsingChar = 0; // クラン倉庫のロックを解除
                }
            }
            else if ((resultType == 5) && (size == 0) && npcImpl == "L1Dwarf")
            { // クラン倉庫から取り出し中にCancel、または、ESCキー
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    clan.WarehouseUsingChar = 0; // クラン倉庫のロックを解除
                }
            }
            else if ((resultType == 8) && (size != 0) && npcImpl == "L1Dwarf" && (level >= 5) && pc.Elf)
            { // 自分のエルフ倉庫に格納
                int objectId, count;
                for (int i = 0; i < size; i++)
                {
                    tradable = true;
                    objectId = ReadD();
                    count = ReadD();
                    GameObject @object = pc.Inventory.getItem(objectId);
                    L1ItemInstance item = (L1ItemInstance)@object;
                    if (!item.Item.Tradable)
                    {
                        tradable = false;
                        pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0は捨てたりまたは他人に讓ることができません。
                    }
                    foreach (L1NpcInstance petNpc in pc.PetList.Values)
                    {
                        if (petNpc is L1PetInstance)
                        {
                            L1PetInstance pet = (L1PetInstance)petNpc;
                            if (item.Id == pet.ItemObjId)
                            {
                                tradable = false;
                                // \f1%0は捨てたりまたは他人に讓ることができません。
                                pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
                                break;
                            }
                        }
                    }
                    foreach (L1DollInstance doll in pc.DollList.Values)
                    {
                        if (item.Id == doll.ItemObjId)
                        {
                            tradable = false;
                            pc.sendPackets(new S_ServerMessage(1181)); // 該当のマジックドールは現在使用中です。
                            break;
                        }
                    }
                    if (pc.DwarfForElfInventory.checkAddItemToWarehouse(item, count, L1Inventory.WAREHOUSE_TYPE_PERSONAL) == L1Inventory.SIZE_OVER)
                    {
                        pc.sendPackets(new S_ServerMessage(75)); // \f1これ以上ものを置く場所がありません。
                        break;
                    }
                    if (tradable)
                    {
                        pc.Inventory.tradeItem(objectId, count, pc.DwarfForElfInventory);
                        pc.turnOnOffLight();
                    }
                }

                // 強制儲存一次身上道具, 避免角色背包內的物品未正常寫入導致物品複製的問題
                pc.saveInventory();
            }
            else if ((resultType == 9) && (size != 0) && npcImpl == "L1Dwarf" && (level >= 5) && pc.Elf)
            { // 自分のエルフ倉庫から取り出し
                int objectId, count;
                L1ItemInstance item;
                for (int i = 0; i < size; i++)
                {
                    objectId = ReadD();
                    count = ReadD();
                    item = pc.DwarfForElfInventory.getItem(objectId);
                    if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                    { // 容量重量確認及びメッセージ送信
                        if (pc.Inventory.consumeItem(40494, 2))
                        { // ミスリル
                            pc.DwarfForElfInventory.tradeItem(item, count, pc.Inventory);
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(337, "$767")); // \f1%0が不足しています。
                            break;
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(270)); // \f1持っているものが重くて取引できません。
                        break;
                    }
                }
            }
            else if ((resultType == 0) && (size != 0) && isPrivateShop)
            { // 個人商店からアイテム購入
                if (findObject == null)
                {
                    return;
                }
                if (!(findObject is L1PcInstance))
                {
                    return;
                }
                L1PcInstance targetPc = (L1PcInstance)findObject;

                int order;
                int count;
                int price;
                IList<L1PrivateShopSellList> sellList;
                L1PrivateShopSellList pssl;
                int itemObjectId;
                int sellPrice;
                int sellTotalCount;
                int sellCount;
                L1ItemInstance item;
                bool[] isRemoveFromList = new bool[8];

                if (targetPc.TradingInPrivateShop)
                {
                    return;
                }
                sellList = targetPc.SellList;
                lock (sellList)
                {
                    // 売り切れが発生し、閲覧中のアイテム数とリスト数が異なる
                    if (pc.PartnersPrivateShopItemCount != sellList.Count)
                    {
                        return;
                    }
                    targetPc.TradingInPrivateShop = true;

                    for (int i = 0; i < size; i++)
                    { // 購入予定の商品
                        order = ReadD();
                        count = ReadD();
                        pssl = sellList[order];
                        itemObjectId = pssl.ItemObjectId;
                        sellPrice = pssl.SellPrice;
                        sellTotalCount = pssl.SellTotalCount; // 売る予定の個数
                        sellCount = pssl.SellCount; // 売った累計
                        item = targetPc.Inventory.getItem(itemObjectId);
                        if (item == null)
                        {
                            continue;
                        }
                        if (count > sellTotalCount - sellCount)
                        {
                            count = sellTotalCount - sellCount;
                        }
                        if (count == 0)
                        {
                            continue;
                        }

                        if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                        { // 容量重量確認及びメッセージ送信
                            for (int j = 0; j < count; j++)
                            { // オーバーフローをチェック
                                if (sellPrice * j > 2000000000)
                                {
                                    // 総販売価格は%dアデナを超過できません。
                                    pc.sendPackets(new S_ServerMessage(904, "2000000000"));
                                    targetPc.TradingInPrivateShop = false;
                                    return;
                                }
                            }
                            price = count * sellPrice;
                            if (pc.Inventory.checkItem(L1ItemId.ADENA, price))
                            {
                                L1ItemInstance adena = pc.Inventory.findItemId(L1ItemId.ADENA);
                                if ((targetPc != null) && (adena != null))
                                {
                                    if (targetPc.Inventory.tradeItem(item, count, pc.Inventory) == null)
                                    {
                                        targetPc.TradingInPrivateShop = false;
                                        return;
                                    }
                                    pc.Inventory.tradeItem(adena, price, targetPc.Inventory);
                                    string message = $"{item.Item.Name} ({count})";
                                    targetPc.sendPackets(new S_ServerMessage(877, pc.Name, message));
                                    pssl.SellCount = count + sellCount;
                                    sellList[order] = pssl;
                                    if (pssl.SellCount == pssl.SellTotalCount)
                                    { // 売る予定の個数を売った
                                        isRemoveFromList[order] = true;
                                    }
                                }
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                                break;
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(270)); // \f1持っているものが重くて取引できません。
                            break;
                        }
                    }
                    // 売り切れたアイテムをリストの末尾から削除
                    for (int i = 7; i >= 0; i--)
                    {
                        if (isRemoveFromList[i])
                        {
                            sellList.RemoveAt(i);
                        }
                    }
                    targetPc.TradingInPrivateShop = false;
                }
            }
            else if ((resultType == 1) && (size != 0) && isPrivateShop)
            { // 個人商店にアイテム売却
                int count;
                int order;
                IList<L1PrivateShopBuyList> buyList;
                L1PrivateShopBuyList psbl;
                int itemObjectId;
                L1ItemInstance item;
                int buyPrice;
                int buyTotalCount;
                int buyCount;
                bool[] isRemoveFromList = new bool[8];

                L1PcInstance targetPc = null;
                if (findObject is L1PcInstance)
                {
                    targetPc = (L1PcInstance)findObject;
                }
                if (targetPc.TradingInPrivateShop)
                {
                    return;
                }
                targetPc.TradingInPrivateShop = true;
                buyList = targetPc.BuyList;

                for (int i = 0; i < size; i++)
                {
                    itemObjectId = ReadD();
                    count = ReadCH();
                    order = ReadC();
                    item = pc.Inventory.getItem(itemObjectId);
                    if (item == null)
                    {
                        continue;
                    }
                    psbl = buyList[order];
                    buyPrice = psbl.BuyPrice;
                    buyTotalCount = psbl.BuyTotalCount; // 買う予定の個数
                    buyCount = psbl.BuyCount; // 買った累計
                    if (count > buyTotalCount - buyCount)
                    {
                        count = buyTotalCount - buyCount;
                    }
                    if (item.Equipped)
                    {
                        // pc.sendPackets(new S_ServerMessage(905)); // 無法販賣裝備中的道具。
                        continue;
                    }
                    if (item.Bless >= 128)
                    { // 被封印的裝備
                      // pc.sendPackets(new S_ServerMessage(210, item.getItem().getName())); // \f1%0%d是不可轉移的…
                        continue;
                    }

                    if (targetPc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                    { // 容量重量確認及びメッセージ送信
                        for (int j = 0; j < count; j++)
                        { // オーバーフローをチェック
                            if (buyPrice * j > 2000000000)
                            {
                                targetPc.sendPackets(new S_ServerMessage(904, "2000000000"));
                                return;
                            }
                        }
                        if (targetPc.Inventory.checkItem(L1ItemId.ADENA, count * buyPrice))
                        {
                            L1ItemInstance adena = targetPc.Inventory.findItemId(L1ItemId.ADENA);
                            if (adena != null)
                            {
                                targetPc.Inventory.tradeItem(adena, count * buyPrice, pc.Inventory);
                                pc.Inventory.tradeItem(item, count, targetPc.Inventory);
                                psbl.BuyCount = count + buyCount;
                                buyList[order] = psbl;
                                if (psbl.BuyCount == psbl.BuyTotalCount)
                                { // 買う予定の個数を買った
                                    isRemoveFromList[order] = true;
                                }
                            }
                        }
                        else
                        {
                            targetPc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                            break;
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(271)); // \f1相手が物を持ちすぎていて取引できません。
                        break;
                    }
                }
                // 買い切ったアイテムをリストの末尾から削除
                for (int i = 7; i >= 0; i--)
                {
                    if (isRemoveFromList[i])
                    {
                        buyList.RemoveAt(i);
                    }
                }
                targetPc.TradingInPrivateShop = false;
            }
            else if ((resultType == 12) && (size != 0) && npcImpl == "L1Merchant")
            { // 領取寵物
                int petCost, petCount, divisor, itemObjectId, itemCount = 0;
                bool chackAdena = true;

                for (int i = 0; i < size; i++)
                {
                    petCost = 0;
                    petCount = 0;
                    divisor = 6;
                    itemObjectId = ReadD();
                    itemCount = ReadD();

                    if (itemCount == 0)
                    {
                        continue;
                    }
                    foreach (L1NpcInstance petNpc in pc.PetList.Values)
                    {
                        petCost += petNpc.Petcost;
                    }

                    int charisma = pc.getCha();
                    if (pc.Crown)
                    { // 王族
                        charisma += 6;
                    }
                    else if (pc.Elf)
                    { // 妖精
                        charisma += 12;
                    }
                    else if (pc.Wizard)
                    { // 法師
                        charisma += 6;
                    }
                    else if (pc.Darkelf)
                    { // 黑暗妖精
                        charisma += 6;
                    }
                    else if (pc.DragonKnight)
                    { // 龍騎士
                        charisma += 6;
                    }
                    else if (pc.Illusionist)
                    { // 幻術師
                        charisma += 6;
                    }

                    if (!pc.Inventory.consumeItem(L1ItemId.ADENA, 115))
                    {
                        chackAdena = false;
                    }
                    L1Pet l1pet = PetTable.Instance.getTemplate(itemObjectId);
                    if (l1pet != null && chackAdena)
                    {
                        npcId = l1pet.get_npcid();
                        charisma -= petCost;
                        if ((npcId == 45313) || (npcId == 45710) || (npcId == 45711) || (npcId == 45712))
                        { // 紀州犬の子犬、紀州犬
                            divisor = 12;
                        }
                        else
                        {
                            divisor = 6;
                        }
                        petCount = charisma / divisor;
                        if (petCount <= 0)
                        {
                            pc.sendPackets(new S_ServerMessage(489)); // 你無法一次控制那麼多寵物。
                            return;
                        }
                        L1Npc npcTemp = Container.Instance.Resolve<INpcController>().getTemplate(npcId);
                        L1PetInstance pet = new L1PetInstance(npcTemp, pc, l1pet);
                        pet.Petcost = divisor;
                    }
                }
                if (!chackAdena)
                {
                    pc.sendPackets(new S_ServerMessage(189)); // \f1金幣不足。
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_RESULT;
            }
        }

    }

}