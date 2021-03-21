using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.npc;
using LineageServer.Server.Server.Model.npc.action;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.storage;
using LineageServer.Server.Server.Templates;
using System;
namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理客戶端傳來拍賣的封包
    /// </summary>
    class C_Amount : ClientBasePacket
    {

        private const string C_AMOUNT = "[C] C_Amount";
        public C_Amount(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {

            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int objectId = readD();
            int amount = readD();
            readC();
            string s = readS();


            L1NpcInstance npc = (L1NpcInstance)L1World.Instance.findObject(objectId);
            if (npc == null)
            {
                return;
            }

            string s1 = string.Empty;
            string s2 = string.Empty;
            try
            {
                string[] tokens = s.Split(new string[] { "\t", "\n", "\r", "\f" }, StringSplitOptions.RemoveEmptyEntries);
                s1 = tokens[0];
                s2 = tokens[1];
            }
            catch (Exception e)
            {
                s1 = string.Empty;
                s2 = string.Empty;
            }
            if (s1 == "agapply")
            { // 如果你在拍賣競標
                string pcName = pc.Name;
                AuctionBoardTable boardTable = new AuctionBoardTable();
                foreach (L1AuctionBoard item in boardTable.AuctionBoardTableList)
                {
                    if (pcName.Equals(item.Bidder, StringComparison.OrdinalIgnoreCase))
                    {
                        pc.sendPackets(new S_ServerMessage(523)); // すでに他の家の競売に参加しています。
                        return;
                    }
                }
                int houseId = Convert.ToInt32(s2);
                L1AuctionBoard board = boardTable.getAuctionBoardTable(houseId);
                if (board != null)
                {
                    int nowPrice = board.Price;
                    int nowBidderId = board.BidderId;
                    if (pc.Inventory.consumeItem(L1ItemId.ADENA, amount))
                    {
                        // 更新拍賣公告
                        board.Price = amount;
                        board.Bidder = pcName;
                        board.BidderId = pc.Id;
                        boardTable.updateAuctionBoard(board);
                        if (nowBidderId != 0)
                        {
                            // 將金幣退還給投標者
                            L1PcInstance bidPc = (L1PcInstance)L1World.Instance.findObject(nowBidderId);
                            if (bidPc != null)
                            { // 玩家在線上
                                bidPc.Inventory.storeItem(L1ItemId.ADENA, nowPrice);
                                // あなたが提示された金額よりももっと高い金額を提示した方が現れたため、残念ながら入札に失敗しました。%n
                                // あなたが競売に預けた%0アデナをお返しします。%nありがとうございました。%n%n
                                bidPc.sendPackets(new S_ServerMessage(525, nowPrice.ToString()));
                            }
                            else
                            { // 玩家離線中
                                L1ItemInstance item = ItemTable.Instance.createItem(L1ItemId.ADENA);
                                item.Count = nowPrice;
                                CharactersItemStorage storage = CharactersItemStorage.create();
                                storage.storeItem(nowBidderId, item);
                            }
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                    }
                }
            }
            else if (s1 == "agsell")
            { // 出售盟屋
                int houseId = Convert.ToInt32(s2);
                AuctionBoardTable boardTable = new AuctionBoardTable();
                L1AuctionBoard board = new L1AuctionBoard();
                if (board != null)
                {
                    // 新增拍賣公告到拍賣板
                    board.HouseId = houseId;
                    L1House house = HouseTable.Instance.getHouseTable(houseId);
                    board.HouseName = house.HouseName;
                    board.HouseArea = house.HouseArea;
                    DateTime cal = DateTime.Now.Date;
                    cal.AddDays(5); // 5天後
                    board.Deadline = cal;
                    board.Price = amount;
                    board.Location = house.Location;
                    board.OldOwner = pc.Name;
                    board.OldOwnerId = pc.Id;
                    board.Bidder = "";
                    board.BidderId = 0;
                    boardTable.insertAuctionBoard(board);

                    house.OnSale = true; // 設定盟屋為拍賣中
                    house.PurchaseBasement = true; // 地下アジト未購入に設定
                    HouseTable.Instance.updateHouse(house); // 更新到資料庫中
                }
            }
            else
            {
                // 旅館NPC
                int npcId = npc.NpcId;
                if (npcId == 70070 || npcId == 70019 || npcId == 70075 || npcId == 70012 || npcId == 70031 || npcId == 70084 || npcId == 70065 || npcId == 70054 || npcId == 70096)
                {

                    if (pc.Inventory.checkItem(L1ItemId.ADENA, (300 * amount)))
                    { // 所需金幣 = 鑰匙價格(300) * 鑰匙數量(amount)
                        L1Inn inn = InnTable.Instance.getTemplate(npcId, pc.InnRoomNumber);
                        if (inn != null)
                        {
                            //DateTime dueTime = inn.DueTime;
                            //if (dueTime != default(DateTime))
                            { // 再次判斷房間租用時間
                                //DateTime cal = DateTime.Now;
                                if (((DateTime.Now.Ticks - inn.DueTime.Ticks) / 1000) < 0)
                                { // 租用時間未到
                                  // 此房間被搶走了...
                                    pc.sendPackets(new S_NPCTalkReturn(npcId, string.Empty));
                                    return;
                                }
                            }
                            // 租用時間 4小時
                            DateTime ts = DateTime.Now.AddHours(4);
                            // 登入旅館資料
                            L1ItemInstance item = ItemTable.Instance.createItem(40312); // 旅館鑰匙
                            if (item != null)
                            {
                                item.KeyId = item.Id; // 鑰匙編號
                                item.InnNpcId = npcId; // 旅館NPC
                                item.Hall = pc.checkRoomOrHall(); // 判斷租房間 or 會議室
                                item.DueTime = ts; // 租用時間
                                item.Count = amount; // 鑰匙數量

                                inn.KeyId = item.KeyId; // 旅館鑰匙
                                inn.LodgerId = pc.Id; // 租用人
                                inn.Hall = pc.checkRoomOrHall(); // 判斷租房間 or 會議室
                                inn.DueTime = ts; // 租用時間
                                                  // DB更新
                                InnTable.Instance.updateInn(inn);

                                pc.Inventory.consumeItem(L1ItemId.ADENA, (300 * amount)); // 扣除金幣

                                // 給予鑰匙並登入鑰匙資料
                                L1Inventory inventory;
                                if (pc.Inventory.checkAddItem(item, amount) == L1Inventory.OK)
                                {
                                    inventory = pc.Inventory;
                                }
                                else
                                {
                                    inventory = L1World.Instance.getInventory(pc.Location);
                                }
                                inventory.storeItem(item);

                                if (InnKeyTable.checkey(item))
                                {
                                    InnKeyTable.DeleteKey(item);
                                    InnKeyTable.StoreKey(item);
                                }
                                else
                                {
                                    InnKeyTable.StoreKey(item);
                                }

                                string itemName = (item.Item.Name + item.InnKeyName);
                                if (amount > 1)
                                {
                                    itemName = (itemName + " (" + amount + ")");
                                }
                                pc.sendPackets(new S_ServerMessage(143, npc.Name, itemName)); // \f1%0%s 給你 %1%o 。
                                string[] msg = new string[] { npc.Name };
                                pc.sendPackets(new S_NPCTalkReturn(npcId, "inn4", msg)); // 要一起使用房間的話，請把鑰匙給其他人，往旁邊的樓梯上去即可。
                            }
                        }
                    }
                    else
                    {
                        string[] msg = new string[] { npc.Name };
                        pc.sendPackets(new S_NPCTalkReturn(npcId, "inn3", msg)); // 對不起，你手中的金幣不夠哦！
                    }
                }
                else
                {
                    INpcAction action = NpcActionTable.Instance.get(s, pc, npc);
                    if (action != null)
                    {
                        L1NpcHtml result = action.executeWithAmount(s, pc, npc, amount);
                        if (result != null)
                        {
                            pc.sendPackets(new S_NPCTalkReturn(npcId, result));
                        }
                        return;
                    }
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_AMOUNT;
            }
        }
    }

}