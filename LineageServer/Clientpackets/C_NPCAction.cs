using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Game;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Npc;
using LineageServer.Server.Model.Npc.Action;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// TODO: 翻譯，好多 處理收到由客戶端傳來NPC動作的封包
    /// </summary>
    class C_NPCAction : ClientBasePacket
    {

        private const string C_NPC_ACTION = "[C] C_NPCAction";
        private static ILogger _log = Logger.GetLogger(nameof(C_NPCAction));
        public C_NPCAction(byte[] abyte0, ClientThread client) : base(abyte0)
        {
            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }
            L1PcInventory l1PcInventory = pc.Inventory as L1PcInventory;

            int objid = ReadD();
            string s = ReadS();

            string s2 = string.Empty;

            GameObject obj = Container.Instance.Resolve<IGameWorld>().findObject(objid);

            if (s == "select" ||
                s == "map" ||
                s == "apply")
            { // 參加拍賣
                s2 = ReadS();
            }
            else if (s == "ent")
            {
                if (obj is L1NpcInstance l1NpcInstance)
                {
                    if (l1NpcInstance.NpcTemplate.get_npcId() == 80088)
                    {
                        s2 = ReadS();
                    }
                }
            }

            int[] materials = null;
            int[] counts = null;
            int[] createitem = null;
            int[] createcount = null;

            string htmlid = null;
            string success_htmlid = null;
            string failure_htmlid = null;
            string[] htmldata = null;

            int questid = 0;
            int questvalue = 0;
            int contribution = 0;

            if (obj != null)
            {
                if (obj is L1NpcInstance npc)
                {
                    int difflocx = Math.Abs(pc.X - npc.X);
                    int difflocy = Math.Abs(pc.Y - npc.Y);
                    if (!(obj is L1PetInstance) && !(obj is L1SummonInstance))
                    {
                        if ((difflocx > 5) || (difflocy > 5))
                        { // 5格以上的距離對話無效
                            return;
                        }
                    }
                    npc.onFinalAction(pc, s);
                }
                else if (obj is L1PcInstance target)
                {
                    int awakeSkillId = target.AwakeSkillId;
                    if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) || (awakeSkillId == L1SkillId.AWAKEN_FAFURION) || (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
                    {
                        target.sendPackets(new S_ServerMessage(1384)); // 現在の状態では変身できません。
                        return;
                    }
                    if (target.ShapeChange)
                    {
                        L1PolyMorph.handleCommands(target, s);
                        target.ShapeChange = false;
                    }
                    else
                    {
                        L1PolyMorph poly = PolyTable.Instance.getTemplate(s);
                        if (poly != null || s == "none")
                        {
                            if (target.Inventory.checkItem(40088) && usePolyScroll(target, 40088, s))
                            {
                            }
                            if (target.Inventory.checkItem(40096) && usePolyScroll(target, 40096, s))
                            {
                            }
                            if (target.Inventory.checkItem(140088) && usePolyScroll(target, 140088, s))
                            {
                            }
                        }
                    }
                    return;
                }
            }
            else
            {
                // _log.warning("object not found, oid " + i);
            }

            // XML化されたアクション
            INpcAction action = NpcActionTable.Instance.get(s, pc, obj);
            if (action != null)
            {
                L1NpcHtml result = action.Execute(s, pc, obj, ReadByte());
                if (result != null)
                {
                    pc.sendPackets(new S_NPCTalkReturn(obj.Id, result));
                }
                return;
            }

            /*
			 * 個別處理行動
			 */
            if (s == "buy")
            {
                L1NpcInstance npc = (L1NpcInstance)obj;
                // sell 應該指給 NPC 檢查
                if (isNpcSellOnly(npc))
                {
                    return;
                }

                // 販賣清單
                pc.sendPackets(new S_ShopSellList(objid, pc));
            }
            else if (s == "sell")
            {
                int npcid = ((L1NpcInstance)obj).NpcTemplate.get_npcId();
                if ((npcid == 70523) || (npcid == 70805))
                { // ラダー or ジュリー
                    htmlid = "ladar2";
                }
                else if ((npcid == 70537) || (npcid == 70807))
                { // ファーリン or フィン
                    htmlid = "farlin2";
                }
                else if ((npcid == 70525) || (npcid == 70804))
                { // ライアン or ジョエル
                    htmlid = "lien2";
                }
                else if ((npcid == 50527) ||
                    (npcid == 50505) ||
                    (npcid == 50519) ||
                    (npcid == 50545) ||
                    (npcid == 50531) ||
                    (npcid == 50529) ||
                    (npcid == 50516) ||
                    (npcid == 50538) ||
                    (npcid == 50518) ||
                    (npcid == 50509) ||
                    (npcid == 50536) ||
                    (npcid == 50520) ||
                    (npcid == 50543) ||
                    (npcid == 50526) ||
                    (npcid == 50512) ||
                    (npcid == 50510) ||
                    (npcid == 50504) ||
                    (npcid == 50525) ||
                    (npcid == 50534) ||
                    (npcid == 50540) ||
                    (npcid == 50515) ||
                    (npcid == 50513) ||
                    (npcid == 50528) ||
                    (npcid == 50533) ||
                    (npcid == 50542) ||
                    (npcid == 50511) ||
                    (npcid == 50501) ||
                    (npcid == 50503) ||
                    (npcid == 50508) ||
                    (npcid == 50514) ||
                    (npcid == 50532) ||
                    (npcid == 50544) ||
                    (npcid == 50524) ||
                    (npcid == 50535) ||
                    (npcid == 50521) ||
                    (npcid == 50517) ||
                    (npcid == 50537) ||
                    (npcid == 50539) ||
                    (npcid == 50507) ||
                    (npcid == 50530) ||
                    (npcid == 50502) ||
                    (npcid == 50506) ||
                    (npcid == 50522) ||
                    (npcid == 50541) ||
                    (npcid == 50523) ||
                    (npcid == 50620) ||
                    (npcid == 50623) ||
                    (npcid == 50619) ||
                    (npcid == 50621) ||
                    (npcid == 50622) ||
                    (npcid == 50624) ||
                    (npcid == 50617) ||
                    (npcid == 50614) ||
                    (npcid == 50618) ||
                    (npcid == 50616) ||
                    (npcid == 50615) ||
                    (npcid == 50626) ||
                    (npcid == 50627) ||
                    (npcid == 50628) ||
                    (npcid == 50629) ||
                    (npcid == 50630) ||
                    (npcid == 50631))
                { // アジトのNPC
                    string sellHouseMessage = sellHouse(pc, objid, npcid);
                    if (!string.ReferenceEquals(sellHouseMessage, null))
                    {
                        htmlid = sellHouseMessage;
                    }
                }
                else
                { // 一般商人

                    // 可以買的物品清單
                    pc.sendPackets(new S_ShopBuyList(objid, pc));
                }
            }
            else if ((((L1NpcInstance)obj).NpcTemplate.get_npcId() == 91002) && s == "ent")
            {
                L1PolyRace.Instance.enterGame(pc);
            }
            else if (s == "retrieve")
            { // 「個人倉庫：領取物品」
                if (pc.Level >= 5)
                {
                    if (client.Account.WarePassword > 0)
                    {
                        pc.sendPackets(new S_ServerMessage(834));
                    }
                    else
                    {
                        pc.sendPackets(new S_RetrieveList(objid, pc));
                    }
                }
            }
            else if (s == "retrieve-elven")
            { // 「妖精倉庫：領取物品」
                if ((pc.Level >= 5) && pc.Elf)
                {
                    if (pc.Elf && (pc.Level > 4))
                    {
                        if (client.Account.WarePassword > 0)
                        {
                            pc.sendPackets(new S_ServerMessage(834));
                        }
                        else
                        {
                            pc.sendPackets(new S_RetrieveElfList(objid, pc));
                        }
                    }
                }
            }
            else if (s == "retrieve-pledge")
            { // 「血盟倉庫：領取物品」
                if (pc.Level >= 5)
                {
                    if (pc.Clanid == 0)
                    {
                        // \f1血盟倉庫を使用するには血盟に加入していなくてはなりません。
                        pc.sendPackets(new S_ServerMessage(208));
                        return;
                    }
                    int rank = pc.ClanRank;
                    if ((rank != L1Clan.CLAN_RANK_PROBATION) &&
                        (rank != L1Clan.CLAN_RANK_GUARDIAN) &&
                        (rank != L1Clan.CLAN_RANK_PRINCE) &&
                        (rank != L1Clan.CLAN_RANK_LEAGUE_PUBLIC) &&
                        (rank != L1Clan.CLAN_RANK_LEAGUE_PROBATION) &&
                        (rank != L1Clan.CLAN_RANK_LEAGUE_GUARDIAN) &&
                        (rank != L1Clan.CLAN_RANK_LEAGUE_VICEPRINCE) &&
                        (rank != L1Clan.CLAN_RANK_LEAGUE_PRINCE))
                    {
                        // タイトルのない血盟員もしくは、見習い血盟員の場合は、血盟倉庫を利用することはできません。
                        pc.sendPackets(new S_ServerMessage(728));
                        return;
                    }
                    if (client.Account.WarePassword > 0)
                    {
                        pc.sendPackets(new S_ServerMessage(834));
                    }
                    else
                    {
                        pc.sendPackets(new S_RetrievePledgeList(objid, pc));
                    }
                }
            }
            else if (s == "history")
            { // 確認血盟倉庫使用記錄
                pc.sendPackets(new S_PledgeWarehouseHistory(pc.Clanid));
            }
            else if (s == "get")
            {
                L1NpcInstance npc = (L1NpcInstance)obj;
                int npcId = npc.NpcTemplate.get_npcId();
                // クーパー or ダンハム
                if ((npcId == 70099) || (npcId == 70796))
                {
                    L1ItemInstance item = pc.Inventory.storeItem(20081, 1); // オイルスキンマント
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    pc.Quest.set_end(L1Quest.QUEST_OILSKINMANT);
                    htmlid = ""; // ウィンドウを消す
                }
                // HomeTown 村莊管理人 支付福利金
                else if ((npcId == 70528) ||
                    (npcId == 70546) ||
                    (npcId == 70567) ||
                    (npcId == 70594) ||
                    (npcId == 70654) ||
                    (npcId == 70748) ||
                    (npcId == 70774) ||
                    (npcId == 70799) ||
                    (npcId == 70815) ||
                    (npcId == 70860))
                {
                    int townId = pc.HomeTownId;
                    int pay = pc.Pay;
                    int cb = pc.Contribution; // 貢獻度
                    htmlid = "";
                    if (pay < 1)
                    {
                        pc.sendPackets(new S_ServerMessage(767)); // 沒有村莊支援費，請在下個月再來。
                    }
                    else if ((pay > 0) && (cb < 500))
                    {
                        pc.sendPackets(new S_ServerMessage(766)); // 貢獻度不足而無法得到補償金
                    }
                    else if (townId > 0)
                    {
                        double payBonus = 1.0; // cb > 499 && cb < 1000
                        bool isLeader = TownTable.Instance.isLeader(pc, townId); // 村長
                        L1ItemInstance item = pc.Inventory.findItemId(L1ItemId.ADENA);
                        if ((cb > 999) && (cb < 1500))
                        {
                            payBonus = 1.5;
                        }
                        else if ((cb > 1499) && (cb < 2000))
                        {
                            payBonus = 2.0;
                        }
                        else if ((cb > 1999) && (cb < 2500))
                        {
                            payBonus = 2.5;
                        }
                        else if ((cb > 2499) && (cb < 3000))
                        {
                            payBonus = 3.0;
                        }
                        else if (cb > 2999)
                        {
                            payBonus = 4.0;
                        }
                        if (isLeader)
                        {
                            payBonus++;
                        }
                        if ((item != null) && (item.Count + (pay * payBonus) > 2000000000))
                        {
                            pc.sendPackets(new S_ServerMessage(166, "所持有的金幣超過2,000,000,000。"));
                            htmlid = "";
                        }
                        else if ((item != null) && (item.Count + (pay * payBonus) < 2000000001))
                        {
                            pay = (int)(HomeTownTimeController.getPay(pc.Id) * payBonus);
                            pc.Inventory.storeItem(L1ItemId.ADENA, pay);
                            pc.sendPackets(new S_ServerMessage(761, pay.ToString()));
                            pc.Pay = 0;
                        }
                    }
                }
            }
            else if (s == "townscore")
            { // 確認目前貢獻度
                L1NpcInstance npc = (L1NpcInstance)obj;
                int npcId = npc.NpcTemplate.get_npcId();
                if ((npcId == 70528) ||
                    (npcId == 70546) ||
                    (npcId == 70567) ||
                    (npcId == 70594) ||
                    (npcId == 70654) ||
                    (npcId == 70748) ||
                    (npcId == 70774) ||
                    (npcId == 70799) ||
                    (npcId == 70815) ||
                    (npcId == 70860))
                {
                    if (pc.HomeTownId > 0)
                    {
                        pc.sendPackets(new S_ServerMessage(1569, pc.Contribution.ToString()));
                    }
                }
            }
            else if (s == "fix")
            { // 武器的修理

            }
            else if (s == "room")
            { // 租房間
                L1NpcInstance npc = (L1NpcInstance)obj;
                int npcId = npc.NpcTemplate.get_npcId();
                bool canRent = false;
                bool findRoom = false;
                bool isRent = false;
                bool isHall = false;
                int roomNumber = 0;
                sbyte roomCount = 0;
                DateTime now = DateTime.Now;
                for (int i = 0; i < 16; i++)
                {
                    L1Inn inn = InnTable.Instance.getTemplate(npcId, i);
                    if (inn != null)
                    { // 此旅館NPC資訊不為空值
                        //DateTime cal = DateTime.Now;
                        long checkDueTime = (now - inn.DueTime).Ticks;
                        if (inn.LodgerId == pc.Id && checkDueTime < 0)
                        { // 出租時間未到的房間租用人判斷
                            if (inn.Hall)
                            { // 租用的是會議室
                                isHall = true;
                            }
                            isRent = true; // 已租用
                            break;
                        }
                        else if (!findRoom && !isRent)
                        { // 未租用且尚未找到可租用的房間
                            if (checkDueTime >= 0)
                            { // 租用時間已到
                                canRent = true;
                                findRoom = true;
                                roomNumber = inn.RoomNumber;
                            }
                            else
                            { // 計算出租時間未到的數量
                                if (!inn.Hall)
                                { // 一般房間
                                    roomCount++;
                                }
                            }
                        }
                    }
                }

                if (isRent)
                {
                    if (isHall)
                    {
                        htmlid = "inn15"; // 真是抱歉，你已經租借過會議廳了。
                    }
                    else
                    {
                        htmlid = "inn5"; // 對不起，你已經有租房間了。
                    }
                }
                else if (roomCount >= 12)
                {
                    htmlid = "inn6"; // 真不好意思，現在沒有房間了。
                }
                else if (canRent)
                {
                    pc.InnRoomNumber = roomNumber; // 房間編號
                    pc.Hall = false; // 一般房間
                    pc.sendPackets(new S_HowManyKey(npc, 300, 1, 8, "inn2"));
                }
            }
            else if (s == "hall" && (obj is L1MerchantInstance))
            { // 租會議廳
                if (pc.Crown)
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    int npcId = npc.NpcTemplate.get_npcId();
                    bool canRent = false;
                    bool findRoom = false;
                    bool isRent = false;
                    bool isHall = false;
                    int roomNumber = 0;
                    sbyte roomCount = 0;
                    DateTime now = DateTime.Now;
                    for (int i = 0; i < 16; i++)
                    {
                        L1Inn inn = InnTable.Instance.getTemplate(npcId, i);
                        if (inn != null)
                        { // 此旅館NPC資訊不為空值
                            //Timestamp dueTime = inn.DueTime;
                            //DateTime cal = new DateTime();
                            long checkDueTime = (now - inn.DueTime).Ticks;
                            if (inn.LodgerId == pc.Id && checkDueTime < 0)
                            { // 出租時間未到的房間租用人判斷
                                if (inn.Hall)
                                { // 租用的是會議室
                                    isHall = true;
                                }
                                isRent = true; // 已租用
                                break;
                            }
                            else if (!findRoom && !isRent)
                            { // 未租用且尚未找到可租用的房間
                                if (checkDueTime >= 0)
                                { // 租用時間已到
                                    canRent = true;
                                    findRoom = true;
                                    roomNumber = inn.RoomNumber;
                                }
                                else
                                { // 計算出租時間未到的數量
                                    if (inn.Hall)
                                    { // 會議室
                                        roomCount++;
                                    }
                                }
                            }
                        }
                    }

                    if (isRent)
                    {
                        if (isHall)
                        {
                            htmlid = "inn15"; // 真是抱歉，你已經租借過會議廳了。
                        }
                        else
                        {
                            htmlid = "inn5"; // 對不起，你已經有租房間了。
                        }
                    }
                    else if (roomCount >= 4)
                    {
                        htmlid = "inn16"; // 不好意思，目前正好沒有空的會議廳。
                    }
                    else if (canRent)
                    {
                        pc.InnRoomNumber = roomNumber; // 房間編號
                        pc.Hall = true; // 會議室
                        pc.sendPackets(new S_HowManyKey(npc, 300, 1, 8, "inn12"));
                    }
                }
                else
                {
                    // 王子和公主才能租用會議廳。
                    htmlid = "inn10";
                }
            }
            else if (s == "return")
            { // 退租
                L1NpcInstance npc = (L1NpcInstance)obj;
                int npcId = npc.NpcTemplate.get_npcId();
                int price = 0;
                bool isBreak = false;
                // 退租判斷
                for (int i = 0; i < 16; i++)
                {
                    L1Inn inn = InnTable.Instance.getTemplate(npcId, i);
                    if (inn != null)
                    { // 此旅館NPC房間資訊不為空值
                        if (inn.LodgerId == pc.Id)
                        { // 欲退租的租用人
                            DateTime now = DateTime.Now;
                            DateTime dueTime = inn.DueTime;
                            //if (dueTime != null)
                            { // 時間不為空值
                                //DateTime cal = new DateTime();
                                if ((now - inn.DueTime).TotalSeconds < 0)
                                { // 租用時間未到
                                    isBreak = true;
                                    price += 60; // 退 20%租金
                                }
                            }
                            //Timestamp ts = new Timestamp(DateTimeHelper.CurrentUnixTimeMillis()); // 目前時間
                            inn.DueTime = now; // 退租時間
                            inn.LodgerId = 0; // 租用人
                            inn.KeyId = 0; // 旅館鑰匙
                            inn.Hall = false;
                            // DB更新
                            InnTable.Instance.updateInn(inn);
                            break;
                        }
                    }
                }
                // 刪除鑰匙判斷
                foreach (L1ItemInstance item in pc.Inventory.Items)
                {
                    if (item.InnNpcId == npcId)
                    { // 鑰匙與退租的NPC相符
                        price += 20 * item.Count; // 鑰匙的價錢 20 * 鑰匙數量
                        InnKeyTable.DeleteKey(item); // 刪除鑰匙紀錄
                        pc.Inventory.removeItem(item); // 刪除鑰匙
                        isBreak = true;
                    }
                }

                if (isBreak)
                {
                    htmldata = new string[] { npc.Name, price.ToString() };
                    htmlid = "inn20";
                    pc.Inventory.storeItem(L1ItemId.ADENA, price); // 取得金幣
                }
                else
                {
                    htmlid = "";
                }
            }
            else if (s == "enter")
            { // 進入房間或會議廳
                L1NpcInstance npc = (L1NpcInstance)obj;
                int npcId = npc.NpcTemplate.get_npcId();
                DateTime now = DateTime.Now;
                foreach (L1ItemInstance item in pc.Inventory.Items)
                {
                    if (item.InnNpcId == npcId)
                    { // 鑰匙與NPC相符
                        for (int i = 0; i < 16; i++)
                        {
                            L1Inn inn = InnTable.Instance.getTemplate(npcId, i);
                            if (inn.KeyId == item.KeyId)
                            {
                                //Timestamp dueTime = item.DueTime;
                                //if (dueTime != null)
                                { // 時間不為空值
                                    //DateTime cal = new DateTime();
                                    if ((now - item.DueTime).Ticks < 0)
                                    { // 鑰匙租用時間未到
                                        int[] data = null;
                                        switch (npcId)
                                        {
                                            case 70012: // 說話之島 - 瑟琳娜
                                                data = new int[] { 32745, 32803, 16384, 32743, 32808, 16896 };
                                                break;
                                            case 70019: // 古魯丁 - 羅利雅
                                                data = new int[] { 32743, 32803, 17408, 32744, 32807, 17920 };
                                                break;
                                            case 70031: // 奇岩 - 瑪理
                                                data = new int[] { 32744, 32803, 18432, 32744, 32807, 18944 };
                                                break;
                                            case 70065: // 歐瑞 - 小安安
                                                data = new int[] { 32744, 32803, 19456, 32744, 32807, 19968 };
                                                break;
                                            case 70070: // 風木 - 維萊莎
                                                data = new int[] { 32744, 32803, 20480, 32744, 32807, 20992 };
                                                break;
                                            case 70075: // 銀騎士 - 米蘭德
                                                data = new int[] { 32744, 32803, 21504, 32744, 32807, 22016 };
                                                break;
                                            case 70084: // 海音 - 伊莉
                                                data = new int[] { 32744, 32803, 22528, 32744, 32807, 23040 };
                                                break;
                                            default:
                                                break;
                                        }

                                        pc.InnKeyId = item.KeyId; // 登入鑰匙編號

                                        if (!item.checkRoomOrHall())
                                        { // 房間
                                            L1Teleport.teleport(pc, data[0], data[1], (short)data[2], 6, false);
                                        }
                                        else
                                        { // 會議室
                                            L1Teleport.teleport(pc, data[3], data[4], (short)data[5], 6, false);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (s == "openigate")
            { // ゲートキーパー / 城門を開ける
                L1NpcInstance npc = (L1NpcInstance)obj;
                openCloseGate(pc, npc.NpcTemplate.get_npcId(), true);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "closeigate")
            { // ゲートキーパー / 城門を閉める
                L1NpcInstance npc = (L1NpcInstance)obj;
                openCloseGate(pc, npc.NpcTemplate.get_npcId(), false);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "askwartime")
            { // 近衛兵 / 次の攻城戦いの時間をたずねる
                L1NpcInstance npc = (L1NpcInstance)obj;
                if (npc.NpcTemplate.get_npcId() == 60514)
                { // ケント城近衛兵
                    htmldata = makeWarTimeStrings(L1CastleLocation.KENT_CASTLE_ID);
                    htmlid = "ktguard7";
                }
                else if (npc.NpcTemplate.get_npcId() == 60560)
                { // オーク近衛兵
                    htmldata = makeWarTimeStrings(L1CastleLocation.OT_CASTLE_ID);
                    htmlid = "orcguard7";
                }
                else if (npc.NpcTemplate.get_npcId() == 60552)
                { // ウィンダウッド城近衛兵
                    htmldata = makeWarTimeStrings(L1CastleLocation.WW_CASTLE_ID);
                    htmlid = "wdguard7";
                }
                else if ((npc.NpcTemplate.get_npcId() == 60524) || (npc.NpcTemplate.get_npcId() == 60525) || (npc.NpcTemplate.get_npcId() == 60529))
                { // ギラン城近衛兵
                    htmldata = makeWarTimeStrings(L1CastleLocation.GIRAN_CASTLE_ID);
                    htmlid = "grguard7";
                }
                else if (npc.NpcTemplate.get_npcId() == 70857)
                { // ハイネ城ハイネガード
                    htmldata = makeWarTimeStrings(L1CastleLocation.HEINE_CASTLE_ID);
                    htmlid = "heguard7";
                }
                else if ((npc.NpcTemplate.get_npcId() == 60530) || (npc.NpcTemplate.get_npcId() == 60531))
                {
                    htmldata = makeWarTimeStrings(L1CastleLocation.DOWA_CASTLE_ID);
                    htmlid = "dcguard7";
                }
                else if ((npc.NpcTemplate.get_npcId() == 60533) || (npc.NpcTemplate.get_npcId() == 60534))
                {
                    htmldata = makeWarTimeStrings(L1CastleLocation.ADEN_CASTLE_ID);
                    htmlid = "adguard7";
                }
                else if (npc.NpcTemplate.get_npcId() == 81156)
                { // アデン偵察兵（ディアド要塞）
                    htmldata = makeWarTimeStrings(L1CastleLocation.DIAD_CASTLE_ID);
                    htmlid = "dfguard3";
                }
            }
            else if (s == "inex")
            { // 収入/支出の報告を受ける
              // 暫定的に公金をチャットウィンドウに表示させる。
              // メッセージは適当。
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    int castle_id = clan.CastleId;
                    if (castle_id != 0)
                    { // 城主クラン
                        L1Castle l1castle = CastleTable.Instance.getCastleTable(castle_id);
                        pc.sendPackets(new S_ServerMessage(309, l1castle.Name, l1castle.PublicMoney.ToString()));
                        htmlid = ""; // ウィンドウを消す
                    }
                }
            }
            else if (s == "tax")
            { // 税率を調節する
                pc.sendPackets(new S_TaxRate(pc.Id));
            }
            else if (s == "withdrawal")
            { // 資金を引き出す
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    int castle_id = clan.CastleId;
                    if (castle_id != 0)
                    { // 城主クラン
                        L1Castle l1castle = CastleTable.Instance.getCastleTable(castle_id);
                        pc.sendPackets(new S_Drawal(pc.Id, l1castle.PublicMoney));
                    }
                }
            }
            else if (s == "cdeposit")
            { // 資金を入金する
                pc.sendPackets(new S_Deposit(pc.Id));
            }
            else if (s == "employ")
            { // 傭兵の雇用

            }
            else if (s == "arrange")
            { // 雇用した傭兵の配置

            }
            else if (s == "arrange")
            { // 配置弓箭手在城牆上

            }
            else if (s == "castlegate")
            { // 城門を管理する
                repairGate(pc);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "encw")
            { // 武器専門家 / 武器の強化魔法を受ける
                if (pc.Weapon == null)
                {
                    pc.sendPackets(new S_ServerMessage(79));
                }
                else
                {
                    foreach (L1ItemInstance item in pc.Inventory.Items)
                    {
                        if (pc.Weapon == item)
                        {
                            L1SkillUse l1skilluse = new L1SkillUse();
                            l1skilluse.handleCommands(pc, L1SkillId.ENCHANT_WEAPON, item.Id, 0, 0, null, 0, L1SkillUse.TYPE_SPELLSC);
                            break;
                        }
                    }
                }
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "enca")
            { // 防具専門家 / 防具の強化魔法を受ける
                L1ItemInstance item = l1PcInventory.getItemEquipped(2, 2);
                if (item != null)
                {
                    L1SkillUse l1skilluse = new L1SkillUse();
                    l1skilluse.handleCommands(pc, L1SkillId.BLESSED_ARMOR, item.Id, 0, 0, null, 0, L1SkillUse.TYPE_SPELLSC);
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(79));
                }
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "depositnpc")
            { // 寄託寵物
                foreach (L1NpcInstance petNpc in pc.PetList.Values)
                {
                    if (petNpc is L1PetInstance)
                    { // ペット
                        L1PetInstance pet = (L1PetInstance)petNpc;
                        pc.sendPackets(new S_PetCtrlMenu(pc, petNpc, false)); // 關閉寵物控制圖形介面
                                                                              // 停止飽食度計時
                        pet.stopFoodTimer(pet);
                        pet.collect(true);
                        pc.PetList.Remove(pet.Id);
                        pet.deleteMe();
                    }
                }
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "withdrawnpc")
            { // 領取寵物
                pc.sendPackets(new S_PetList(objid, pc));
            }
            else if (s == "aggressive")
            { // 攻撃態勢
                if (obj is L1PetInstance)
                {
                    L1PetInstance l1pet = (L1PetInstance)obj;
                    l1pet.CurrentPetStatus = 1;
                }
            }
            else if (s == "defensive")
            { // 防禦型態
                if (obj is L1PetInstance)
                {
                    L1PetInstance l1pet = (L1PetInstance)obj;
                    l1pet.CurrentPetStatus = 2;
                }
            }
            else if (s == "stay")
            { // 休憩
                if (obj is L1PetInstance)
                {
                    L1PetInstance l1pet = (L1PetInstance)obj;
                    l1pet.CurrentPetStatus = 3;
                }
            }
            else if (s == "extend")
            { // 配備
                if (obj is L1PetInstance)
                {
                    L1PetInstance l1pet = (L1PetInstance)obj;
                    l1pet.CurrentPetStatus = 4;
                }
            }
            else if (s == "alert")
            { // 警戒
                if (obj is L1PetInstance)
                {
                    L1PetInstance l1pet = (L1PetInstance)obj;
                    l1pet.CurrentPetStatus = 5;
                }
            }
            else if (s == "dismiss")
            { // 解散
                if (obj is L1PetInstance)
                {
                    L1PetInstance l1pet = (L1PetInstance)obj;
                    l1pet.CurrentPetStatus = 6;
                }
            }
            else if (s == "changename")
            { // 「名前を決める」
                pc.TempID = objid; // ペットのオブジェクトIDを保存しておく
                pc.sendPackets(new S_Message_YN(325, "")); // 動物の名前を決めてください：
            }
            else if (s == "attackchr")
            {
                if (obj is L1Character)
                {
                    L1Character cha = (L1Character)obj;
                    pc.sendPackets(new S_SelectTarget(cha.Id));
                }
            }
            else if (s == "select")
            { // 競売掲示板のリストをクリック
                pc.sendPackets(new S_AuctionBoardRead(objid, s2));
            }
            else if (s == "map")
            { // アジトの位置を確かめる
                pc.sendPackets(new S_HouseMap(objid, s2));
            }
            else if (s == "apply")
            { // 競売に参加する
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    if (pc.Crown && (pc.Id == clan.LeaderId))
                    { // 君主、かつ、血盟主
                        if (pc.Level >= 15)
                        {
                            if (clan.HouseId == 0)
                            {
                                pc.sendPackets(new S_ApplyAuction(objid, s2));
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(521)); // すでに家を所有しています。
                                htmlid = ""; // ウィンドウを消す
                            }
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(519)); // レベル15未満の君主は競売に参加できません。
                            htmlid = ""; // ウィンドウを消す
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(518)); // この命令は血盟の君主のみが利用できます。
                        htmlid = ""; // ウィンドウを消す
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(518)); // この命令は血盟の君主のみが利用できます。
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "open" || s == "close")
            { // ドアを閉める
                L1NpcInstance npc = (L1NpcInstance)obj;
                openCloseDoor(pc, npc, s);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "expel")
            { // 外部の人間を追い出す
                L1NpcInstance npc = (L1NpcInstance)obj;
                expelOtherClan(pc, npc.NpcTemplate.get_npcId());
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "pay")
            { // 税金を納める
                L1NpcInstance npc = (L1NpcInstance)obj;
                htmldata = makeHouseTaxStrings(pc, npc);
                htmlid = "agpay";
            }
            else if (s == "payfee")
            { // 税金を納める
                L1NpcInstance npc = (L1NpcInstance)obj;
                htmldata = new string[] { npc.NpcTemplate.get_name(), "2000" };
                htmlid = "";
                if (payFee(pc, npc))
                {
                    htmlid = "agpayfee";
                }
            }
            else if (s == "name")
            { // 家の名前を決める
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    int houseId = clan.HouseId;
                    if (houseId != 0)
                    {
                        L1House house = HouseTable.Instance.getHouseTable(houseId);
                        int keeperId = house.KeeperId;
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        if (npc.NpcTemplate.get_npcId() == keeperId)
                        {
                            pc.TempID = houseId; // アジトIDを保存しておく
                            pc.sendPackets(new S_Message_YN(512, "")); // 家の名前は？
                        }
                    }
                }
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "rem")
            { // 家の中の家具をすべて取り除く
            }
            else if (s == "tel0" || s == "tel1" || s == "tel2" || s == "tel3")
            { // テレポートする(ギラン市場)
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    int houseId = clan.HouseId;
                    if (houseId != 0)
                    {
                        L1House house = HouseTable.Instance.getHouseTable(houseId);
                        int keeperId = house.KeeperId;
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        if (npc.NpcTemplate.get_npcId() == keeperId)
                        {
                            int[] loc = new int[3];
                            if (s == "tel0")
                            {
                                loc = L1HouseLocation.getHouseTeleportLoc(houseId, 0);
                            }
                            else if (s == "tel1")
                            {
                                loc = L1HouseLocation.getHouseTeleportLoc(houseId, 1);
                            }
                            else if (s == "tel2")
                            {
                                loc = L1HouseLocation.getHouseTeleportLoc(houseId, 2);
                            }
                            else if (s == "tel3")
                            {
                                loc = L1HouseLocation.getHouseTeleportLoc(houseId, 3);
                            }
                            L1Teleport.teleport(pc, loc[0], loc[1], (short)loc[2], 5, true);
                        }
                    }
                }
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "upgrade")
            { // 地下アジトを作る
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    int houseId = clan.HouseId;
                    if (houseId != 0)
                    {
                        L1House house = HouseTable.Instance.getHouseTable(houseId);
                        int keeperId = house.KeeperId;
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        if (npc.NpcTemplate.get_npcId() == keeperId)
                        {
                            if (pc.Crown && (pc.Id == clan.LeaderId))
                            { // 君主、かつ、血盟主
                                if (house.PurchaseBasement)
                                {
                                    // 既に地下アジトを所有しています。
                                    pc.sendPackets(new S_ServerMessage(1135));
                                }
                                else
                                {
                                    if (pc.Inventory.consumeItem(L1ItemId.ADENA, 5000000))
                                    {
                                        house.PurchaseBasement = true;
                                        HouseTable.Instance.updateHouse(house); // DBに書き込み
                                                                                // 地下アジトが生成されました。
                                        pc.sendPackets(new S_ServerMessage(1099));
                                    }
                                    else
                                    {
                                        // \f1アデナが不足しています。
                                        pc.sendPackets(new S_ServerMessage(189));
                                    }
                                }
                            }
                            else
                            {
                                // この命令は血盟の君主のみが利用できます。
                                pc.sendPackets(new S_ServerMessage(518));
                            }
                        }
                    }
                }
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "hall" && (obj is L1HousekeeperInstance))
            { // 地下アジトにテレポートする
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    int houseId = clan.HouseId;
                    if (houseId != 0)
                    {
                        L1House house = HouseTable.Instance.getHouseTable(houseId);
                        int keeperId = house.KeeperId;
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        if (npc.NpcTemplate.get_npcId() == keeperId)
                        {
                            if (house.PurchaseBasement)
                            {
                                int[] loc = new int[3];
                                loc = L1HouseLocation.getBasementLoc(houseId);
                                L1Teleport.teleport(pc, loc[0], loc[1], (short)(loc[2]), 5, true);
                            }
                            else
                            {
                                // 地下アジトがないため、テレポートできません。
                                pc.sendPackets(new S_ServerMessage(1098));
                            }
                        }
                    }
                }
                htmlid = ""; // ウィンドウを消す
            }

            // ElfAttr:0.無属性,1.地属性,2.火属性,4.水属性,8.風属性
            else if (s == "fire") // エルフの属性変更「火の系列を習う」
            {
                if (pc.Elf)
                {
                    if (pc.ElfAttr != 0)
                    {
                        return;
                    }
                    pc.ElfAttr = 2;
                    pc.Save(); // DBにキャラクター情報を書き込む
                    pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_ELF, 1)); // 忽然全身充滿了火的靈力。
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "water")
            { // エルフの属性変更「水の系列を習う」
                if (pc.Elf)
                {
                    if (pc.ElfAttr != 0)
                    {
                        return;
                    }
                    pc.ElfAttr = 4;
                    pc.Save(); // DBにキャラクター情報を書き込む
                    pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_ELF, 2)); // 忽然全身充滿了水的靈力。
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "air")
            { // エルフの属性変更「風の系列を習う」
                if (pc.Elf)
                {
                    if (pc.ElfAttr != 0)
                    {
                        return;
                    }
                    pc.ElfAttr = 8;
                    pc.Save(); // DBにキャラクター情報を書き込む
                    pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_ELF, 3)); // 忽然全身充滿了風的靈力。
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "earth")
            { // エルフの属性変更「地の系列を習う」
                if (pc.Elf)
                {
                    if (pc.ElfAttr != 0)
                    {
                        return;
                    }
                    pc.ElfAttr = 1;
                    pc.Save(); // DBにキャラクター情報を書き込む
                    pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_ELF, 4)); // 忽然全身充滿了地的靈力。
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "init")
            { // エルフの属性変更「精霊力を除去する」
                if (pc.Elf)
                {
                    if (pc.ElfAttr == 0)
                    {
                        return;
                    }
                    for (int cnt = 129; cnt <= 176; cnt++) // 全エルフ魔法をチェック
                    {
                        L1Skills l1skills1 = SkillsTable.Instance.getTemplate(cnt);
                        int skill_attr = l1skills1.Attr;
                        if (skill_attr != 0) // 無属性魔法以外のエルフ魔法をDBから削除する
                        {
                            SkillsTable.Instance.spellLost(pc.Id, l1skills1.SkillId);
                        }
                    }
                    // エレメンタルプロテクションによって上昇している属性防御をリセット
                    if (pc.hasSkillEffect(L1SkillId.ELEMENTAL_PROTECTION))
                    {
                        pc.removeSkillEffect(L1SkillId.ELEMENTAL_PROTECTION);
                    }
                    pc.sendPackets(new S_DelSkill(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 248, 252, 252, 255, 0, 0, 0, 0, 0, 0)); // 無属性魔法以外のエルフ魔法を魔法ウィンドウから削除する
                    pc.ElfAttr = 0;
                    pc.Save(); // DBにキャラクター情報を書き込む
                    pc.sendPackets(new S_ServerMessage(678));
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "exp")
            { // 「経験値を回復する」
                if (pc.ExpRes == 1)
                {
                    int cost = 0;
                    int level = pc.Level;
                    int lawful = pc.Lawful;
                    if (level < 45)
                    {
                        cost = level * level * 100;
                    }
                    else
                    {
                        cost = level * level * 200;
                    }
                    if (lawful >= 0)
                    {
                        cost = (cost / 2);
                    }
                    pc.sendPackets(new S_Message_YN(738, cost.ToString())); // 経験値を回復するには%0のアデナが必要です。経験値を回復しますか？
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(739)); // 今は経験値を回復することができません。
                    htmlid = ""; // ウィンドウを消す
                }
            }
            else if (s == "pk")
            { // 「贖罪する」
                if (pc.Lawful < 30000)
                {
                    pc.sendPackets(new S_ServerMessage(559)); // \f1まだ罪晴らしに十分な善行を行っていません。
                }
                else if (pc.get_PKcount() < 5)
                {
                    pc.sendPackets(new S_ServerMessage(560)); // \f1まだ罪晴らしをする必要はありません。
                }
                else
                {
                    if (pc.Inventory.consumeItem(L1ItemId.ADENA, 700000))
                    {
                        pc.set_PKcount(pc.get_PKcount() - 5);
                        pc.sendPackets(new S_ServerMessage(561, pc.get_PKcount().ToString())); // PK回数が%0になりました。
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                    }
                }
                // ウィンドウを消す
                htmlid = "";
            }
            else if (s == "ent")
            {
                // 「お化け屋敷に入る」
                // 「アルティメット バトルに参加する」または
                // 「観覧モードで闘技場に入る」
                // 「ステータス再分配」
                int npcId = ((L1NpcInstance)obj).NpcId;
                if (npcId == 80085)
                {
                    htmlid = enterHauntedHouse(pc);
                }
                else if (npcId == 80088)
                {
                    htmlid = enterPetMatch(pc, Convert.ToInt32(s2));
                }
                else if ((npcId == 50038) || (npcId == 50042) || (npcId == 50029) || (npcId == 50019) || (npcId == 50062))
                { // 副管理人の場合は観戦
                    htmlid = watchUb(pc, npcId);
                }
                else if (npcId == 71251)
                { // ロロ
                    if (!pc.Inventory.checkItem(49142))
                    { // 希望のロウソク
                        pc.sendPackets(new S_ServerMessage(1290)); // ステータス初期化に必要なアイテムがありません。
                        return;
                    }
                    L1SkillUse l1skilluse = new L1SkillUse();
                    l1skilluse.handleCommands(pc, L1SkillId.CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
                    l1PcInventory.takeoffEquip(945); // 牛のpolyIdで装備を全部外す。
                    L1Teleport.teleport(pc, 32737, 32789, (short)997, 4, false);
                    int initStatusPoint = 75 + pc.ElixirStats;
                    int pcStatusPoint = pc.BaseStr + pc.BaseInt + pc.BaseWis + pc.BaseDex + pc.BaseCon + pc.BaseCha;
                    if (pc.Level > 50)
                    {
                        pcStatusPoint += (pc.Level - 50 - pc.BonusStats);
                    }
                    int diff = pcStatusPoint - initStatusPoint;
                    /// <summary>
                    /// [50級以上]
                    /// 
                    /// 目前點數 - 初始點數 = 人物應有等級 - 50 -> 人物應有等級 = 50 + (目前點數 - 初始點數)
                    /// </summary>
                    int maxLevel = 1;

                    if (diff > 0)
                    {
                        // 最高到99級:也就是?不支援轉生
                        maxLevel = Math.Min(50 + diff, 99);
                    }
                    else
                    {
                        maxLevel = pc.Level;
                    }

                    pc.TempMaxLevel = maxLevel;
                    pc.TempLevel = 1;
                    pc.InCharReset = true;
                    pc.sendPackets(new S_CharReset(pc));
                }
                else
                {
                    htmlid = enterUb(pc, npcId);
                }
            }
            else if (s == "par")
            { // UB関連「アルティメット バトルに参加する」 副管理人経由
                htmlid = enterUb(pc, ((L1NpcInstance)obj).NpcId);
            }
            else if (s == "info")
            { // 「情報を確認する」「競技情報を確認する」
                htmlid = "colos2";
            }
            else if (s == "sco")
            { // UB関連「高得点者一覧を確認する」
                htmldata = new string[10];
                htmlid = "colos3";
            }

            else if (s == "haste")
            { // ヘイスト師
                L1NpcInstance l1npcinstance = (L1NpcInstance)obj;
                int npcid = l1npcinstance.NpcTemplate.get_npcId();
                if (npcid == 70514)
                {
                    pc.sendPackets(new S_ServerMessage(183));
                    pc.sendPackets(new S_SkillHaste(pc.Id, 1, 1600));
                    pc.broadcastPacket(new S_SkillHaste(pc.Id, 1, 0));
                    pc.sendPackets(new S_SkillSound(pc.Id, 755));
                    pc.broadcastPacket(new S_SkillSound(pc.Id, 755));
                    pc.MoveSpeed = 1;
                    pc.setSkillEffect(L1SkillId.STATUS_HASTE, 1600 * 1000);
                    htmlid = ""; // ウィンドウを消す
                }
            }
            // 変身専門家
            else if (s == "skeleton nbmorph")
            {
                poly(client, 2374);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "lycanthrope nbmorph")
            {
                poly(client, 3874);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "shelob nbmorph")
            {
                poly(client, 95);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "ghoul nbmorph")
            {
                poly(client, 3873);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "ghast nbmorph")
            {
                poly(client, 3875);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "atuba orc nbmorph")
            {
                poly(client, 3868);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "skeleton axeman nbmorph")
            {
                poly(client, 2376);
                htmlid = ""; // ウィンドウを消す
            }
            else if (s == "troll nbmorph")
            {
                poly(client, 3878);
                htmlid = ""; // ウィンドウを消す
            }

            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71095)
            { // 墮落的靈魂
                if (s == "teleport evil-dungeon")
                { // 往邪念地監
                    bool find = false;
                    foreach (object objs in Container.Instance.Resolve<IGameWorld>().getVisibleObjects(306).Values)
                    {
                        if (objs is L1PcInstance)
                        {
                            L1PcInstance _pc = (L1PcInstance)objs;
                            if (_pc != null)
                            {
                                find = true;
                                htmlid = "csoulqn"; // 你的邪念還不夠！
                                break;
                            }
                        }
                    }
                    if (!find)
                    {
                        L1Quest quest = pc.Quest;
                        int lv50_step = quest.get_step(L1Quest.QUEST_LEVEL50);
                        if (lv50_step == L1Quest.QUEST_END)
                        {
                            htmlid = "csoulq3";
                        }
                        else if (lv50_step >= 3)
                        {
                            L1Teleport.teleport(pc, 32747, 32799, (short)306, 6, true);
                        }
                        else
                        {
                            htmlid = "csoulq2";
                        }
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81279)
            { // 卡瑞
              // -
              // 卡瑞的祝福
                if (s == "a")
                {
                    // 卡瑞的祝福已經環繞整個身軀
                    L1BuffUtil.effectBlessOfDragonSlayer(pc, L1SkillId.EFFECT_BLESS_OF_CRAY, 2400, 7681);
                    htmlid = "grayknight2";
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81292)
            { // 受咀咒的巫女莎爾
              // -
              // 莎爾的祝福
                if (s == "a")
                {
                    // 巫女莎爾的祝福纏繞著整個身體。
                    L1BuffUtil.effectBlessOfDragonSlayer(pc, L1SkillId.EFFECT_BLESS_OF_SAELL, 2400, 7680);
                    htmlid = "";
                }
            }
            // 長老 ノナメ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71038)
            {
                // 「手紙を受け取る」
                if (s == "A")
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    L1ItemInstance item = pc.Inventory.storeItem(41060, 1); // ノナメの推薦書
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    htmlid = "orcfnoname9";
                }
                // 「調査をやめます」
                else if (s == "Z")
                {
                    if (pc.Inventory.consumeItem(41060, 1))
                    {
                        htmlid = "orcfnoname11";
                    }
                }
            }
            // ドゥダ-マラ ブウ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71039)
            {
                // 「わかりました、その場所に送ってください」
                if (s == "teleportURL")
                {
                    htmlid = "orcfbuwoo2";
                }
            }
            // 調査団長 アトゥバ ノア
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71040)
            {
                // 「やってみます」
                if (s == "A")
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    L1ItemInstance item = pc.Inventory.storeItem(41065, 1); // 調査団の証書
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    htmlid = "orcfnoa4";
                }
                // 「調査をやめます」
                else if (s == "Z")
                {
                    if (pc.Inventory.consumeItem(41065, 1))
                    {
                        htmlid = "orcfnoa7";
                    }
                }
            }
            // ネルガ フウモ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71041)
            {
                // 「調査をします」
                if (s == "A")
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    L1ItemInstance item = pc.Inventory.storeItem(41064, 1); // 調査団の証書
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    htmlid = "orcfhuwoomo4";
                }
                // 「調査をやめます」
                else if (s == "Z")
                {
                    if (pc.Inventory.consumeItem(41064, 1))
                    {
                        htmlid = "orcfhuwoomo6";
                    }
                }
            }
            // ネルガ バクモ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71042)
            {
                // 「調査をします」
                if (s == "A")
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    L1ItemInstance item = pc.Inventory.storeItem(41062, 1); // 調査団の証書
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    htmlid = "orcfbakumo4";
                }
                // 「調査をやめます」
                else if (s == "Z")
                {
                    if (pc.Inventory.consumeItem(41062, 1))
                    {
                        htmlid = "orcfbakumo6";
                    }
                }
            }
            // ドゥダ-マラ ブカ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71043)
            {
                // 「調査をします」
                if (s == "A")
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    L1ItemInstance item = pc.Inventory.storeItem(41063, 1); // 調査団の証書
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    htmlid = "orcfbuka4";
                }
                // 「調査をやめます」
                else if (s == "Z")
                {
                    if (pc.Inventory.consumeItem(41063, 1))
                    {
                        htmlid = "orcfbuka6";
                    }
                }
            }
            // ドゥダ-マラ カメ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71044)
            {
                // 「調査をします」
                if (s == "A")
                {
                    L1NpcInstance npc = (L1NpcInstance)obj;
                    L1ItemInstance item = pc.Inventory.storeItem(41061, 1); // 調査団の証書
                    string npcName = npc.NpcTemplate.get_name();
                    string itemName = item.Item.Name;
                    pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                    htmlid = "orcfkame4";
                }
                // 「調査をやめます」
                else if (s == "Z")
                {
                    if (pc.Inventory.consumeItem(41061, 1))
                    {
                        htmlid = "orcfkame6";
                    }
                }
            }
            // ポワール
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71078)
            {
                // 「入ってみる」
                if (s == "teleportURL")
                {
                    htmlid = "usender2";
                }
            }
            // 治安団長アミス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71080)
            {
                // 「私がお手伝いしましょう」
                if (s == "teleportURL")
                {
                    htmlid = "amisoo2";
                }
            }
            // 空間の歪み
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80048)
            {
                // 「やめる」
                if (s == "2")
                {
                    htmlid = ""; // ウィンドウを消す
                }
            }
            // 揺らぐ者
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80049)
            {
                // 「バルログの意志を迎え入れる」
                if (s == "1")
                {
                    if (pc.Karma <= -10000000)
                    {
                        pc.Karma = 1000000;
                        // バルログの笑い声が脳裏を強打します。
                        pc.sendPackets(new S_ServerMessage(1078));
                        htmlid = "betray13";
                    }
                }
            }
            // ヤヒの執政官
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80050)
            {
                // 「私の霊魂はヤヒ様へ…」
                if (s == "1")
                {
                    htmlid = "meet105";
                }
                // 「私の霊魂をかけてヤヒ様に忠誠を誓います…」
                else if (s == "2")
                {
                    if (pc.Inventory.checkItem(40718))
                    { // ブラッドクリスタルの欠片
                        htmlid = "meet106";
                    }
                    else
                    {
                        htmlid = "meet110";
                    }
                }
                // 「ブラッドクリスタルの欠片を1個捧げます」
                else if (s == "a")
                {
                    if (pc.Inventory.consumeItem(40718, 1))
                    {
                        pc.addKarma((int)(-100 * Config.RATE_KARMA));
                        // ヤヒの姿がだんだん近くに感じられます。
                        pc.sendPackets(new S_ServerMessage(1079));
                        htmlid = "meet107";
                    }
                    else
                    {
                        htmlid = "meet104";
                    }
                }
                // 「ブラッドクリスタルの欠片を10個捧げます」
                else if (s == "b")
                {
                    if (pc.Inventory.consumeItem(40718, 10))
                    {
                        pc.addKarma((int)(-1000 * Config.RATE_KARMA));
                        // ヤヒの姿がだんだん近くに感じられます。
                        pc.sendPackets(new S_ServerMessage(1079));
                        htmlid = "meet108";
                    }
                    else
                    {
                        htmlid = "meet104";
                    }
                }
                // 「ブラッドクリスタルの欠片を100個捧げます」
                else if (s == "c")
                {
                    if (pc.Inventory.consumeItem(40718, 100))
                    {
                        pc.addKarma((int)(-10000 * Config.RATE_KARMA));
                        // ヤヒの姿がだんだん近くに感じられます。
                        pc.sendPackets(new S_ServerMessage(1079));
                        htmlid = "meet109";
                    }
                    else
                    {
                        htmlid = "meet104";
                    }
                }
                // 「ヤヒ様に会わせてください」
                else if (s == "d")
                {
                    if (pc.Inventory.checkItem(40615) || pc.Inventory.checkItem(40616))
                    { // 影の神殿3階の鍵
                        htmlid = "";
                    }
                    else
                    {
                        L1Teleport.teleport(pc, 32683, 32895, (short)608, 5, true);
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80052)
            { // 火焰之影的軍師
                if (s == "a")
                { // 請賜給我力量
                    if (pc.hasSkillEffect(L1SkillId.STATUS_CURSE_BARLOG))
                    { // 火焰之影的烙印
                        pc.killSkillEffectTimer(L1SkillId.STATUS_CURSE_BARLOG);
                    }
                    pc.sendPackets(new S_SkillSound(pc.Id, 750));
                    pc.broadcastPacket(new S_SkillSound(pc.Id, 750));
                    pc.sendPackets(new S_SkillIconAura(221, 1020, 2)); // 火焰之影的烙印
                    pc.setSkillEffect(L1SkillId.STATUS_CURSE_BARLOG, 1020 * 1000);
                    pc.sendPackets(new S_ServerMessage(1127));
                    htmlid = "";
                }
            }
            // ヤヒの鍛冶屋
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80053)
            {
                // 「材料すべてを用意できました」
                if (s == "a")
                {
                    // バルログのツーハンド ソード / ヤヒの鍛冶屋
                    int aliceMaterialId = 0;
                    int karmaLevel = 0;
                    int[] material = null;
                    int[] count = null;
                    int createItem = 0;
                    string successHtmlId = null;
                    string htmlId = null;

                    int[] aliceMaterialIdList = new int[] { 40991, 196, 197, 198, 199, 200, 201, 202 };
                    int[] karmaLevelList = new int[] { -1, -2, -3, -4, -5, -6, -7, -8 };
                    int[][] materialsList = new int[][]
                    {
                        new int[] {40995, 40718, 40991},
                        new int[] {40997, 40718, 196},
                        new int[] {40990, 40718, 197},
                        new int[] {40994, 40718, 198},
                        new int[] {40993, 40718, 199},
                        new int[] {40998, 40718, 200},
                        new int[] {40996, 40718, 201},
                        new int[] {40992, 40718, 202}
                    };
                    int[][] countList = new int[][]
                    {
                        new int[] {100, 100, 1},
                        new int[] {100, 100, 1},
                        new int[] {100, 100, 1},
                        new int[] {50, 100, 1},
                        new int[] {50, 100, 1},
                        new int[] {50, 100, 1},
                        new int[] {10, 100, 1},
                        new int[] {10, 100, 1}
                    };
                    int[] createItemList = new int[] { 196, 197, 198, 199, 200, 201, 202, 203 };
                    string[] successHtmlIdList = new string[] { "alice_1", "alice_2", "alice_3", "alice_4", "alice_5", "alice_6", "alice_7", "alice_8" };
                    string[] htmlIdList = new string[] { "aliceyet", "alice_1", "alice_2", "alice_3", "alice_4", "alice_5", "alice_5", "alice_7" };

                    for (int i = 0; i < aliceMaterialIdList.Length; i++)
                    {
                        if (pc.Inventory.checkItem(aliceMaterialIdList[i]))
                        {
                            aliceMaterialId = aliceMaterialIdList[i];
                            karmaLevel = karmaLevelList[i];
                            material = materialsList[i];
                            count = countList[i];
                            createItem = createItemList[i];
                            successHtmlId = successHtmlIdList[i];
                            htmlId = htmlIdList[i];
                            break;
                        }
                    }

                    if (aliceMaterialId == 0)
                    {
                        htmlid = "alice_no";
                    }
                    else if (aliceMaterialId == 203)
                    {
                        htmlid = "alice_8";
                    }
                    else
                    {
                        if (pc.KarmaLevel <= karmaLevel)
                        {
                            materials = material;
                            counts = count;
                            createitem = new int[] { createItem };
                            createcount = new int[] { 1 };
                            success_htmlid = successHtmlId;
                            failure_htmlid = "alice_no";
                        }
                        else
                        {
                            htmlid = htmlId;
                        }
                    }
                }
            }
            // ヤヒの補佐官
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80055)
            {
                L1NpcInstance npc = (L1NpcInstance)obj;
                htmlid = getYaheeAmulet(pc, npc, s);
            }
            // 業の管理者
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80056)
            {
                L1NpcInstance npc = (L1NpcInstance)obj;
                if (pc.Karma <= -10000000)
                {
                    getBloodCrystalByKarma(pc, npc, s);
                }
                htmlid = "";
            }
            // 次元の扉(バルログの部屋)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80063)
            {
                // 「中に入る」
                if (s == "a")
                {
                    if (pc.Inventory.checkItem(40921))
                    { // 元素の支配者
                        L1Teleport.teleport(pc, 32674, 32832, (short)603, 2, true);
                    }
                    else
                    {
                        htmlid = "gpass02";
                    }
                }
            }
            // バルログの執政官
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80064)
            {
                // 「私の永遠の主はバルログ様だけです…」
                if (s == "1")
                {
                    htmlid = "meet005";
                }
                // 「私の霊魂をかけてバルログ様に忠誠を誓います…」
                else if (s == "2")
                {
                    if (pc.Inventory.checkItem(40678))
                    { // ソウルクリスタルの欠片
                        htmlid = "meet006";
                    }
                    else
                    {
                        htmlid = "meet010";
                    }
                }
                // 「ソウルクリスタルの欠片を1個捧げます」
                else if (s == "a")
                {
                    if (pc.Inventory.consumeItem(40678, 1))
                    {
                        pc.addKarma((int)(100 * Config.RATE_KARMA));
                        // バルログの笑い声が脳裏を強打します。
                        pc.sendPackets(new S_ServerMessage(1078));
                        htmlid = "meet007";
                    }
                    else
                    {
                        htmlid = "meet004";
                    }
                }
                // 「ソウルクリスタルの欠片を10個捧げます」
                else if (s == "b")
                {
                    if (pc.Inventory.consumeItem(40678, 10))
                    {
                        pc.addKarma((int)(1000 * Config.RATE_KARMA));
                        // バルログの笑い声が脳裏を強打します。
                        pc.sendPackets(new S_ServerMessage(1078));
                        htmlid = "meet008";
                    }
                    else
                    {
                        htmlid = "meet004";
                    }
                }
                // 「ソウルクリスタルの欠片を100個捧げます」
                else if (s == "c")
                {
                    if (pc.Inventory.consumeItem(40678, 100))
                    {
                        pc.addKarma((int)(10000 * Config.RATE_KARMA));
                        // バルログの笑い声が脳裏を強打します。
                        pc.sendPackets(new S_ServerMessage(1078));
                        htmlid = "meet009";
                    }
                    else
                    {
                        htmlid = "meet004";
                    }
                }
                // 「バルログ様に会わせてください」
                else if (s == "d")
                {
                    if (pc.Inventory.checkItem(40909) || pc.Inventory.checkItem(40910) || pc.Inventory.checkItem(40911) || pc.Inventory.checkItem(40912) || pc.Inventory.checkItem(40913) || pc.Inventory.checkItem(40914) || pc.Inventory.checkItem(40915) || pc.Inventory.checkItem(40916) || pc.Inventory.checkItem(40917) || pc.Inventory.checkItem(40918) || pc.Inventory.checkItem(40919) || pc.Inventory.checkItem(40920) || pc.Inventory.checkItem(40921))
                    { // 元素の支配者
                        htmlid = "";
                    }
                    else
                    {
                        L1Teleport.teleport(pc, 32674, 32832, (short)602, 2, true);
                    }
                }
            }
            // 揺らめく者
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80066)
            {
                // 「カヘルの意志を受け入れる」
                if (s == "1")
                {
                    if (pc.Karma >= 10000000)
                    {
                        pc.Karma = -1000000;
                        // ヤヒの姿がだんだん近くに感じられます。
                        pc.sendPackets(new S_ServerMessage(1079));
                        htmlid = "betray03";
                    }
                }
            }
            // バルログの補佐官
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80071)
            {
                L1NpcInstance npc = (L1NpcInstance)obj;
                htmlid = getBarlogEarring(pc, npc, s);
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80073)
            { // 炎魔的軍師
                if (s == "a")
                { // 請給我力量
                    if (pc.hasSkillEffect(L1SkillId.STATUS_CURSE_YAHEE))
                    { // 炎魔的烙印
                        pc.killSkillEffectTimer(L1SkillId.STATUS_CURSE_YAHEE);
                    }
                    pc.sendPackets(new S_SkillSound(pc.Id, 750));
                    pc.broadcastPacket(new S_SkillSound(pc.Id, 750));
                    pc.sendPackets(new S_SkillIconAura(221, 1020, 1)); // 炎魔的烙印
                    pc.setSkillEffect(L1SkillId.STATUS_CURSE_YAHEE, 1020 * 1000);
                    pc.sendPackets(new S_ServerMessage(1127));
                    htmlid = "";
                }
            }
            // バルログの鍛冶屋
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80072)
            {
                string sEquals = null;
                int karmaLevel = 0;
                int[] material = null;
                int[] count = null;
                int createItem = 0;
                string failureHtmlId = null;
                string htmlId = null;

                string[] sEqualsList = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "a", "b", "c", "d", "e", "f", "g", "h" };
                string[] htmlIdList = new string[] { "lsmitha", "lsmithb", "lsmithc", "lsmithd", "lsmithe", "", "lsmithf", "lsmithg", "lsmithh" };
                int[] karmaLevelList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                int[][] materialsList = new int[][]
                {
                    new int[] {20158, 40669, 40678},
                    new int[] {20144, 40672, 40678},
                    new int[] {20075, 40671, 40678},
                    new int[] {20183, 40674, 40678},
                    new int[] {20190, 40674, 40678},
                    new int[] {20078, 40674, 40678},
                    new int[] {20078, 40670, 40678},
                    new int[] {40719, 40673, 40678}
                };
                int[][] countList = new int[][]
                {
                    new int[] {1, 50, 100},
                    new int[] {1, 50, 100},
                    new int[] {1, 50, 100},
                    new int[] {1, 20, 100},
                    new int[] {1, 40, 100},
                    new int[] {1, 5, 100},
                    new int[] {1, 1, 100},
                    new int[] {1, 1, 100}
                };
                int[] createItemList = new int[] { 20083, 20131, 20069, 20179, 20209, 20290, 20261, 20031 };
                string[] failureHtmlIdList = new string[] { "lsmithaa", "lsmithbb", "lsmithcc", "lsmithdd", "lsmithee", "lsmithff", "lsmithgg", "lsmithhh" };

                for (int i = 0; i < sEqualsList.Length; i++)
                {
                    if (s == sEqualsList[i])
                    {
                        sEquals = sEqualsList[i];
                        if (i <= 8)
                        {
                            htmlId = htmlIdList[i];
                        }
                        else if (i > 8)
                        {
                            karmaLevel = karmaLevelList[i - 9];
                            material = materialsList[i - 9];
                            count = countList[i - 9];
                            createItem = createItemList[i - 9];
                            failureHtmlId = failureHtmlIdList[i - 9];
                        }
                        break;
                    }
                }
                if (s == sEquals)
                {
                    if ((karmaLevel != 0) && (pc.KarmaLevel >= karmaLevel))
                    {
                        materials = material;
                        counts = count;
                        createitem = new int[] { createItem };
                        createcount = new int[] { 1 };
                        success_htmlid = "";
                        failure_htmlid = failureHtmlId;
                    }
                    else
                    {
                        htmlid = htmlId;
                    }
                }
            }
            // 業の管理者
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80074)
            {
                L1NpcInstance npc = (L1NpcInstance)obj;
                if (pc.Karma >= 10000000)
                {
                    getSoulCrystalByKarma(pc, npc, s);
                }
                htmlid = "";
            }
            // アルフォンス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80057)
            {
                htmlid = karmaLevelToHtmlId(pc.KarmaLevel);
                htmldata = new string[] { pc.KarmaPercent.ToString() };
            }
            // 次元の扉(土風水火)
            else if ((((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80059) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80060) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80061) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80062))
            {
                htmlid = talkToDimensionDoor(pc, (L1NpcInstance)obj, s);
            }
            // ジャック オ ランタン
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81124)
            {
                if (s == "1")
                {
                    poly(client, 4002);
                    htmlid = ""; // ウィンドウを消す
                }
                else if (s == "2")
                {
                    poly(client, 4004);
                    htmlid = ""; // ウィンドウを消す
                }
                else if (s == "3")
                {
                    poly(client, 4950);
                    htmlid = ""; // ウィンドウを消す
                }
            }

            // クエスト関連
            // 一般クエスト / ライラ
            else if (s == "contract1")
            {
                pc.Quest.set_step(L1Quest.QUEST_LYRA, 1);
                htmlid = "lyraev2";
            }
            else if (s == "contract1yes" || s == "contract1no")
            { // ライラ No

                if (s == "contract1yes")
                {
                    htmlid = "lyraev5";
                }
                else if (s == "contract1no")
                {
                    pc.Quest.set_step(L1Quest.QUEST_LYRA, 0);
                    htmlid = "lyraev4";
                }
                int totem = 0;
                if (pc.Inventory.checkItem(40131))
                {
                    totem++;
                }
                if (pc.Inventory.checkItem(40132))
                {
                    totem++;
                }
                if (pc.Inventory.checkItem(40133))
                {
                    totem++;
                }
                if (pc.Inventory.checkItem(40134))
                {
                    totem++;
                }
                if (pc.Inventory.checkItem(40135))
                {
                    totem++;
                }
                if (totem != 0)
                {
                    materials = new int[totem];
                    counts = new int[totem];
                    createitem = new int[totem];
                    createcount = new int[totem];

                    totem = 0;
                    if (pc.Inventory.checkItem(40131))
                    {
                        L1ItemInstance l1iteminstance = pc.Inventory.findItemId(40131);
                        int i1 = l1iteminstance.Count;
                        materials[totem] = 40131;
                        counts[totem] = i1;
                        createitem[totem] = L1ItemId.ADENA;
                        createcount[totem] = i1 * 50;
                        totem++;
                    }
                    if (pc.Inventory.checkItem(40132))
                    {
                        L1ItemInstance l1iteminstance = pc.Inventory.findItemId(40132);
                        int i1 = l1iteminstance.Count;
                        materials[totem] = 40132;
                        counts[totem] = i1;
                        createitem[totem] = L1ItemId.ADENA;
                        createcount[totem] = i1 * 100;
                        totem++;
                    }
                    if (pc.Inventory.checkItem(40133))
                    {
                        L1ItemInstance l1iteminstance = pc.Inventory.findItemId(40133);
                        int i1 = l1iteminstance.Count;
                        materials[totem] = 40133;
                        counts[totem] = i1;
                        createitem[totem] = L1ItemId.ADENA;
                        createcount[totem] = i1 * 50;
                        totem++;
                    }
                    if (pc.Inventory.checkItem(40134))
                    {
                        L1ItemInstance l1iteminstance = pc.Inventory.findItemId(40134);
                        int i1 = l1iteminstance.Count;
                        materials[totem] = 40134;
                        counts[totem] = i1;
                        createitem[totem] = L1ItemId.ADENA;
                        createcount[totem] = i1 * 30;
                        totem++;
                    }
                    if (pc.Inventory.checkItem(40135))
                    {
                        L1ItemInstance l1iteminstance = pc.Inventory.findItemId(40135);
                        int i1 = l1iteminstance.Count;
                        materials[totem] = 40135;
                        counts[totem] = i1;
                        createitem[totem] = L1ItemId.ADENA;
                        createcount[totem] = i1 * 200;
                        totem++;
                    }
                }
            }
            // 最近物價倍率
            else if (s == "pandora6" || s == "cold6" || s == "balsim3" || s == "arieh6" || s == "andyn3" || s == "ysorya3" || s == "luth3" || s == "catty3" || s == "mayer3" || s == "vergil3" || s == "stella6" || s == "ralf6" || s == "berry6" || s == "jin6" || s == "defman3" || s == "mellisa3" || s == "mandra3" || s == "bius3" || s == "momo6" || s == "ashurEv7" || s == "elmina3" || s == "glen3" || s == "mellin3" || s == "orcm6" || s == "jackson3" || s == "britt3" || s == "old6" || s == "shivan3")
            { // 70083: 須凡(海音 武器商)
                htmlid = s;
                int npcid = ((L1NpcInstance)obj).NpcTemplate.get_npcId();
                int taxRatesCastle = L1CastleLocation.getCastleTaxRateByNpcId(npcid);
                htmldata = new string[] { taxRatesCastle.ToString() };
            }
            // タウンマスター（この村の住民に登録する）
            else if (s == "set")
            {
                if (obj is L1NpcInstance)
                {
                    int npcid = ((L1NpcInstance)obj).NpcTemplate.get_npcId();
                    int town_id = L1TownLocation.getTownIdByNpcid(npcid);

                    if ((town_id >= 1) && (town_id <= 10))
                    {
                        if (pc.HomeTownId == -1)
                        {
                            // \f1新しく住民登録を行なうには時間がかかります。時間を置いてからまた登録してください。
                            pc.sendPackets(new S_ServerMessage(759));
                            htmlid = "";
                        }
                        else if (pc.HomeTownId > 0)
                        {
                            // 既に登録してる
                            if (pc.HomeTownId != town_id)
                            {
                                L1Town town = TownTable.Instance.getTownTable(pc.HomeTownId);
                                if (town != null)
                                {
                                    // 現在、あなたが住民登録している場所は%0です。
                                    pc.sendPackets(new S_ServerMessage(758, town.get_name()));
                                }
                                htmlid = "";
                            }
                            else
                            {
                                // ありえない？
                                htmlid = "";
                            }
                        }
                        else if (pc.HomeTownId == 0)
                        {
                            // 登録
                            if (pc.Level < 10)
                            {
                                // \f1住民登録ができるのはレベル10以上のキャラクターです。
                                pc.sendPackets(new S_ServerMessage(757));
                                htmlid = "";
                            }
                            else
                            {
                                int level = pc.Level;
                                int cost = level * level * 10;
                                if (pc.Inventory.consumeItem(L1ItemId.ADENA, cost))
                                {
                                    pc.HomeTownId = town_id;
                                    pc.Contribution = 0; // 念のため
                                    pc.Save();
                                }
                                else
                                {
                                    // アデナが不足しています。
                                    pc.sendPackets(new S_ServerMessage(337, "$4"));
                                }
                                htmlid = "";
                            }
                        }
                    }
                }
            }
            // タウンマスター（住民登録を取り消す）
            else if (s == "clear")
            {
                if (obj is L1NpcInstance)
                {
                    int npcid = ((L1NpcInstance)obj).NpcTemplate.get_npcId();
                    int town_id = L1TownLocation.getTownIdByNpcid(npcid);
                    if (town_id > 0)
                    {
                        if (pc.HomeTownId > 0)
                        {
                            if (pc.HomeTownId == town_id)
                            {
                                pc.HomeTownId = -1;
                                pc.Contribution = 0; // 貢献度クリア
                                pc.Save();
                            }
                            else
                            {
                                // \f1あなたは他の村の住民です。
                                pc.sendPackets(new S_ServerMessage(756));
                            }
                        }
                        htmlid = "";
                    }
                }
            }
            // タウンマスター（村の村長が誰かを聞く）
            else if (s == "ask")
            {
                if (obj is L1NpcInstance)
                {
                    int npcid = ((L1NpcInstance)obj).NpcTemplate.get_npcId();
                    int town_id = L1TownLocation.getTownIdByNpcid(npcid);

                    if ((town_id >= 1) && (town_id <= 10))
                    {
                        L1Town town = TownTable.Instance.getTownTable(town_id);
                        string leader = town.get_leader_name();
                        if ((!string.ReferenceEquals(leader, null)) && (leader.Length != 0))
                        {
                            htmlid = "owner";
                            htmldata = new string[] { leader };
                        }
                        else
                        {
                            htmlid = "noowner";
                        }
                    }
                }
            }
            // HomeTown 各村莊 副村長 (取消副村長 for 3.3C)
            else if ((((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70534) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70556) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70572) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70631) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70663) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70761) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70788) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70806) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70830) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70876))
            {
                // タウンアドバイザー（収入に関する報告）
                if (s == "r")
                {
                }
                // タウンアドバイザー（税率変更）
                else if (s == "t")
                {

                }
                // タウンアドバイザー（報酬をもらう）
                else if (s == "c")
                {

                }
            }
            // ドロモンド
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70997)
            {
                // ありがとう、旅立ちます
                if (s == "0")
                {
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_ids = { 41146, 4, 20322, 173, 40743};
                    int[] item_ids = new int[] { 41146, 4, 20322, 173, 40743 };
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_amounts = { 1, 1, 1, 1, 500};
                    int[] item_amounts = new int[] { 1, 1, 1, 1, 500 };
                    for (int i = 0; i < item_ids.Length; i++)
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                        pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.LogName));
                    }
                    pc.Quest.set_step(L1Quest.QUEST_DOROMOND, 1);
                    htmlid = "jpe0015";
                }
            }
            // アレックス(歌う島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70999)
            {
                // ドロモンドの紹介状を渡す
                if (s == "1")
                {
                    if (pc.Inventory.consumeItem(41146, 1))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 23, 20219, 20193};
                        int[] item_ids = new int[] { 23, 20219, 20193 };
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_amounts = { 1, 1, 1};
                        int[] item_amounts = new int[] { 1, 1, 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.LogName));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_DOROMOND, 2);
                        htmlid = "";
                    }
                }
                else if (s == "2")
                {
                    L1ItemInstance item = pc.Inventory.storeItem(41227, 1); // アレックスの紹介状
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.LogName));
                    pc.Quest.set_step(L1Quest.QUEST_AREX, L1Quest.QUEST_END);
                    htmlid = "";
                }
            }
            // ポピレア(歌う島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71005)
            {
                // アイテムを受け取る
                if (s == "0")
                {
                    if (!pc.Inventory.checkItem(41209))
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(41209, 1);
                        pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        htmlid = ""; // ウィンドウを消す
                    }
                }
                // アイテムを受け取る
                else if (s == "1")
                {
                    if (pc.Inventory.consumeItem(41213, 1))
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(40029, 20);
                        pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name + " (" + 20 + ")"));
                        htmlid = ""; // ウィンドウを消す
                    }
                }
            }
            // ティミー(歌う島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71006)
            {
                if (s == "0")
                {
                    if (pc.Level > 25)
                    {
                        htmlid = "jpe0057";
                    }
                    else if (pc.Inventory.checkItem(41213))
                    { // ティミーのバスケット
                        htmlid = "jpe0056";
                    }
                    else if (pc.Inventory.checkItem(41210) || pc.Inventory.checkItem(41211))
                    { // 研磨材、ハーブ
                        htmlid = "jpe0055";
                    }
                    else if (pc.Inventory.checkItem(41209))
                    { // ポピリアの依頼書
                        htmlid = "jpe0054";
                    }
                    else if (pc.Inventory.checkItem(41212))
                    { // 特製キャンディー
                        htmlid = "jpe0056";
                        materials = new int[] { 41212 }; // 特製キャンディー
                        counts = new int[] { 1 };
                        createitem = new int[] { 41213 }; // ティミーのバスケット
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "jpe0057";
                    }
                }
            }
            // 治療師（歌う島の中：ＨＰのみ回復）
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70512)
            {
                // 治療を受ける("fullheal"でリクエストが来ることはあるのか？)
                if (s == "0" || s == "fullheal")
                {
                    int hp = RandomHelper.Next(21) + 70;
                    pc.CurrentHp = pc.CurrentHp + hp;
                    pc.sendPackets(new S_ServerMessage(77));
                    pc.sendPackets(new S_SkillSound(pc.Id, 830));
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    htmlid = ""; // ウィンドウを消す
                }
            }
            // 治療師（訓練場：HPMP回復）
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71037)
            {
                if (s == "0")
                {
                    pc.CurrentHp = pc.MaxHp;
                    pc.CurrentMp = pc.MaxMp;
                    pc.sendPackets(new S_ServerMessage(77));
                    pc.sendPackets(new S_SkillSound(pc.Id, 830));
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                }
            }
            // 治療師（西部）
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71030)
            {
                if (s == "fullheal")
                {
                    if (pc.Inventory.checkItem(L1ItemId.ADENA, 5))
                    { // check
                        pc.Inventory.consumeItem(L1ItemId.ADENA, 5); // del
                        pc.CurrentHp = pc.MaxHp;
                        pc.CurrentMp = pc.MaxMp;
                        pc.sendPackets(new S_ServerMessage(77));
                        pc.sendPackets(new S_SkillSound(pc.Id, 830));
                        pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                        pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                        if (pc.InParty)
                        { // パーティー中
                            pc.Party.updateMiniHP(pc);
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(337, "$4")); // アデナが不足しています。
                    }
                }
            }
            // キャンセレーション師
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71002)
            {
                // キャンセレーション魔法をかけてもらう
                if (s == "0")
                {
                    if (pc.Level <= 13)
                    {
                        L1SkillUse skillUse = new L1SkillUse();
                        skillUse.handleCommands(pc, L1SkillId.CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_NPCBUFF, (L1NpcInstance)obj);
                        htmlid = ""; // ウィンドウを消す
                    }
                }
            }
            // ケスキン(歌う島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71025)
            {
                if (s == "0")
                {
                    L1ItemInstance item = pc.Inventory.storeItem(41225, 1); // ケスキンの発注書
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                    htmlid = "jpe0083";
                }
            }
            // ルケイン(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71055)
            {
                // アイテムを受け取る
                if (s == "0")
                {
                    L1ItemInstance item = pc.Inventory.storeItem(40701, 1); // 小さな宝の地図
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                    pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 1);
                    htmlid = "lukein8";
                }
                else if (s == "2")
                {
                    htmlid = "lukein12";
                    pc.Quest.set_step(L1Quest.QUEST_RESTA, 3);
                }
            }
            // 小さな箱-1番目
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71063)
            {
                if (s == "0")
                {
                    materials = new int[] { 40701 }; // 小さな宝の地図
                    counts = new int[] { 1 };
                    createitem = new int[] { 40702 }; // 小さな袋
                    createcount = new int[] { 1 };
                    htmlid = "maptbox1";
                    pc.Quest.set_end(L1Quest.QUEST_TBOX1);
                    int[] nextbox = new int[] { 1, 2, 3 };
                    int pid = RandomHelper.Next(nextbox.Length);
                    int nb = nextbox[pid];
                    if (nb == 1)
                    { // b地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 2);
                    }
                    else if (nb == 2)
                    { // c地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 3);
                    }
                    else if (nb == 3)
                    { // d地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 4);
                    }
                }
            }
            // 小さな箱-2番目
            else if ((((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71064) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71065) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71066))
            {
                if (s == "0")
                {
                    materials = new int[] { 40701 }; // 小さな宝の地図
                    counts = new int[] { 1 };
                    createitem = new int[] { 40702 }; // 小さな袋
                    createcount = new int[] { 1 };
                    htmlid = "maptbox1";
                    pc.Quest.set_end(L1Quest.QUEST_TBOX2);
                    int[] nextbox2 = new int[] { 1, 2, 3, 4, 5, 6 };
                    int pid = RandomHelper.Next(nextbox2.Length);
                    int nb2 = nextbox2[pid];
                    if (nb2 == 1)
                    { // e地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 5);
                    }
                    else if (nb2 == 2)
                    { // f地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 6);
                    }
                    else if (nb2 == 3)
                    { // g地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 7);
                    }
                    else if (nb2 == 4)
                    { // h地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 8);
                    }
                    else if (nb2 == 5)
                    { // i地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 9);
                    }
                    else if (nb2 == 6)
                    { // j地点
                        pc.Quest.set_step(L1Quest.QUEST_LUKEIN1, 10);
                    }
                }
            }
            // シミズ(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71056)
            {
                // 息子を捜す
                if (s == "a")
                {
                    pc.Quest.set_step(L1Quest.QUEST_SIMIZZ, 1);
                    htmlid = "SIMIZZ7";
                }
                else if (s == "b")
                {
                    if (pc.Inventory.checkItem(40661) && pc.Inventory.checkItem(40662) && pc.Inventory.checkItem(40663))
                    {
                        htmlid = "SIMIZZ8";
                        pc.Quest.set_step(L1Quest.QUEST_SIMIZZ, 2);
                        materials = new int[] { 40661, 40662, 40663 };
                        counts = new int[] { 1, 1, 1 };
                        createitem = new int[] { 20044 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "SIMIZZ9";
                    }
                }
                else if (s == "d")
                {
                    htmlid = "SIMIZZ12";
                    pc.Quest.set_step(L1Quest.QUEST_SIMIZZ, L1Quest.QUEST_END);
                }
            }
            // ドイル(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71057)
            {
                // ラッシュについて聞く
                if (s == "3")
                {
                    htmlid = "doil4";
                }
                else if (s == "6")
                {
                    htmlid = "doil6";
                }
                else if (s == "1")
                {
                    if (pc.Inventory.checkItem(40714))
                    {
                        htmlid = "doil8";
                        materials = new int[] { 40714 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 40647 };
                        createcount = new int[] { 1 };
                        pc.Quest.set_step(L1Quest.QUEST_DOIL, L1Quest.QUEST_END);
                    }
                    else
                    {
                        htmlid = "doil7";
                    }
                }
            }
            // ルディアン(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71059)
            {
                // ルディアンの頼みを受け入れる
                if (s == "A")
                {
                    htmlid = "rudian6";
                    L1ItemInstance item = pc.Inventory.storeItem(40700, 1);
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                    pc.Quest.set_step(L1Quest.QUEST_RUDIAN, 1);
                }
                else if (s == "B")
                {
                    if (pc.Inventory.checkItem(40710))
                    {
                        htmlid = "rudian8";
                        materials = new int[] { 40700, 40710 };
                        counts = new int[] { 1, 1 };
                        createitem = new int[] { 40647 };
                        createcount = new int[] { 1 };
                        pc.Quest.set_step(L1Quest.QUEST_RUDIAN, L1Quest.QUEST_END);
                    }
                    else
                    {
                        htmlid = "rudian9";
                    }
                }
            }
            // レスタ(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71060)
            {
                // 仲間たちについて
                if (s == "A")
                {
                    if (pc.Quest.get_step(L1Quest.QUEST_RUDIAN) == L1Quest.QUEST_END)
                    {
                        htmlid = "resta6";
                    }
                    else
                    {
                        htmlid = "resta4";
                    }
                }
                else if (s == "B")
                {
                    htmlid = "resta10";
                    pc.Quest.set_step(L1Quest.QUEST_RESTA, 2);
                }
            }
            // カドムス(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71061)
            {
                // 地図を組み合わせてください
                if (s == "A")
                {
                    if (pc.Inventory.checkItem(40647, 3))
                    {
                        htmlid = "cadmus6";
                        pc.Inventory.consumeItem(40647, 3);
                        pc.Quest.set_step(L1Quest.QUEST_CADMUS, 2);
                    }
                    else
                    {
                        htmlid = "cadmus5";
                        pc.Quest.set_step(L1Quest.QUEST_CADMUS, 1);
                    }
                }
            }
            // カミーラ(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71036)
            {
                if (s == "a")
                {
                    htmlid = "kamyla7";
                    pc.Quest.set_step(L1Quest.QUEST_KAMYLA, 1);
                }
                else if (s == "c")
                {
                    htmlid = "kamyla10";
                    pc.Inventory.consumeItem(40644, 1);
                    pc.Quest.set_step(L1Quest.QUEST_KAMYLA, 3);
                }
                else if (s == "e")
                {
                    htmlid = "kamyla13";
                    pc.Inventory.consumeItem(40630, 1);
                    pc.Quest.set_step(L1Quest.QUEST_KAMYLA, 4);
                }
                else if (s == "i")
                {
                    htmlid = "kamyla25";
                }
                else if (s == "b")
                { // カーミラ（フランコの迷宮）
                    if (pc.Quest.get_step(L1Quest.QUEST_KAMYLA) == 1)
                    {
                        L1Teleport.teleport(pc, 32679, 32742, (short)482, 5, true);
                    }
                }
                else if (s == "d")
                { // カーミラ（ディエゴの閉ざされた牢）
                    if (pc.Quest.get_step(L1Quest.QUEST_KAMYLA) == 3)
                    {
                        L1Teleport.teleport(pc, 32736, 32800, (short)483, 5, true);
                    }
                }
                else if (s == "f")
                { // カーミラ（ホセ地下牢）
                    if (pc.Quest.get_step(L1Quest.QUEST_KAMYLA) == 4)
                    {
                        L1Teleport.teleport(pc, 32746, 32807, (short)484, 5, true);
                    }
                }
            }
            // フランコ(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71089)
            {
                // カミーラにあなたの潔白を証明しましょう
                if (s == "a")
                {
                    htmlid = "francu10";
                    L1ItemInstance item = pc.Inventory.storeItem(40644, 1);
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                    pc.Quest.set_step(L1Quest.QUEST_KAMYLA, 2);
                }
            }
            // 試練のクリスタル2(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71090)
            {
                // はい、武器とスクロールをください
                if (s == "a")
                {
                    htmlid = "";
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_ids = { 246, 247, 248, 249, 40660 };
                    int[] item_ids = new int[] { 246, 247, 248, 249, 40660 };
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_amounts = { 1, 1, 1, 1, 5 };
                    int[] item_amounts = new int[] { 1, 1, 1, 1, 5 };
                    for (int i = 0; i < item_ids.Length; i++)
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                        pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        pc.Quest.set_step(L1Quest.QUEST_CRYSTAL, 1);
                    }
                }
                else if (s == "b")
                {
                    if (l1PcInventory.checkEquipped(246) || l1PcInventory.checkEquipped(247) || l1PcInventory.checkEquipped(248) || l1PcInventory.checkEquipped(249))
                    {
                        htmlid = "jcrystal5";
                    }
                    else if (pc.Inventory.checkItem(40660))
                    {
                        htmlid = "jcrystal4";
                    }
                    else
                    {
                        pc.Inventory.consumeItem(246, 1);
                        pc.Inventory.consumeItem(247, 1);
                        pc.Inventory.consumeItem(248, 1);
                        pc.Inventory.consumeItem(249, 1);
                        pc.Inventory.consumeItem(40620, 1);
                        pc.Quest.set_step(L1Quest.QUEST_CRYSTAL, 2);
                        L1Teleport.teleport(pc, 32801, 32895, (short)483, 4, true);
                    }
                }
                else if (s == "c")
                {
                    if (l1PcInventory.checkEquipped(246) || l1PcInventory.checkEquipped(247) || l1PcInventory.checkEquipped(248) || l1PcInventory.checkEquipped(249))
                    {
                        htmlid = "jcrystal5";
                    }
                    else
                    {
                        pc.Inventory.checkItem(40660);
                        L1ItemInstance l1iteminstance = pc.Inventory.findItemId(40660);
                        int sc = l1iteminstance.Count;
                        if (sc > 0)
                        {
                            pc.Inventory.consumeItem(40660, sc);
                        }
                        else
                        {
                        }
                        pc.Inventory.consumeItem(246, 1);
                        pc.Inventory.consumeItem(247, 1);
                        pc.Inventory.consumeItem(248, 1);
                        pc.Inventory.consumeItem(249, 1);
                        pc.Inventory.consumeItem(40620, 1);
                        pc.Quest.set_step(L1Quest.QUEST_CRYSTAL, 0);
                        L1Teleport.teleport(pc, 32736, 32800, (short)483, 4, true);
                    }
                }
            }
            // 試練のクリスタル2(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71091)
            {
                // さらば！！
                if (s == "a")
                {
                    htmlid = "";
                    pc.Inventory.consumeItem(40654, 1);
                    pc.Quest.set_step(L1Quest.QUEST_CRYSTAL, L1Quest.QUEST_END);
                    L1Teleport.teleport(pc, 32744, 32927, (short)483, 4, true);
                }
            }
            // リザードマンの長老(海賊島)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71074)
            {
                // その戦士は今どこらへんにいるんですか？
                if (s == "A")
                {
                    htmlid = "lelder5";
                    pc.Quest.set_step(L1Quest.QUEST_LIZARD, 1);
                    // 宝を取り戻してきます
                }
                else if (s == "B")
                {
                    htmlid = "lelder10";
                    pc.Inventory.consumeItem(40633, 1);
                    pc.Quest.set_step(L1Quest.QUEST_LIZARD, 3);
                }
                else if (s == "C")
                {
                    htmlid = "lelder13";
                    if (pc.Quest.get_step(L1Quest.QUEST_LIZARD) == L1Quest.QUEST_END)
                    {
                    }
                    materials = new int[] { 40634 };
                    counts = new int[] { 1 };
                    createitem = new int[] { 20167 }; // リザードマングローブ
                    createcount = new int[] { 1 };
                    pc.Quest.set_step(L1Quest.QUEST_LIZARD, L1Quest.QUEST_END);
                }
            }
            // 傭兵団長 ティオン
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71198)
            {
                if (s == "A")
                {
                    if ((pc.Quest.get_step(71198) != 0) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(41339, 5))
                    { // 亡者のメモ
                        L1ItemInstance item = ItemTable.Instance.createItem(41340); // 傭兵団長
                                                                                    // ティオンの紹介状
                        if (item != null)
                        {
                            if (pc.Inventory.checkAddItem(item, 1) == 0)
                            {
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            }
                        }
                        pc.Quest.set_step(71198, 1);
                        htmlid = "tion4";
                    }
                    else
                    {
                        htmlid = "tion9";
                    }
                }
                else if (s == "B")
                {
                    if ((pc.Quest.get_step(71198) != 1) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(41341, 1))
                    { // ジェロンの教本
                        pc.Quest.set_step(71198, 2);
                        htmlid = "tion5";
                    }
                    else
                    {
                        htmlid = "tion10";
                    }
                }
                else if (s == "C")
                {
                    if ((pc.Quest.get_step(71198) != 2) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(41343, 1))
                    { // パプリオンの血痕
                        L1ItemInstance item = ItemTable.Instance.createItem(21057); // 訓練騎士のマント1
                        if (item != null)
                        {
                            if (pc.Inventory.checkAddItem(item, 1) == 0)
                            {
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            }
                        }
                        pc.Quest.set_step(71198, 3);
                        htmlid = "tion6";
                    }
                    else
                    {
                        htmlid = "tion12";
                    }
                }
                else if (s == "D")
                {
                    if ((pc.Quest.get_step(71198) != 3) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(41344, 1))
                    { // 水の精粋
                        L1ItemInstance item = ItemTable.Instance.createItem(21058); // 訓練騎士のマント2
                        if (item != null)
                        {
                            pc.Inventory.consumeItem(21057, 1); // 訓練騎士のマント1
                            if (pc.Inventory.checkAddItem(item, 1) == 0)
                            {
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            }
                        }
                        pc.Quest.set_step(71198, 4);
                        htmlid = "tion7";
                    }
                    else
                    {
                        htmlid = "tion13";
                    }
                }
                else if (s == "E")
                {
                    if ((pc.Quest.get_step(71198) != 4) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(41345, 1))
                    { // 酸性の乳液
                        L1ItemInstance item = ItemTable.Instance.createItem(21059); // ポイズン
                                                                                    // サーペント
                                                                                    // クローク
                        if (item != null)
                        {
                            pc.Inventory.consumeItem(21058, 1); // 訓練騎士のマント2
                            if (pc.Inventory.checkAddItem(item, 1) == 0)
                            {
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            }
                        }
                        pc.Quest.set_step(71198, 0);
                        pc.Quest.set_step(71199, 0);
                        htmlid = "tion8";
                    }
                    else
                    {
                        htmlid = "tion15";
                    }
                }
            }
            // ジェロン
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71199)
            {
                if (s == "A")
                {
                    if ((pc.Quest.get_step(71199) != 0) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.checkItem(41340, 1))
                    { // 傭兵団長 ティオンの紹介状
                        pc.Quest.set_step(71199, 1);
                        htmlid = "jeron2";
                    }
                    else
                    {
                        htmlid = "jeron10";
                    }
                }
                else if (s == "B")
                {
                    if ((pc.Quest.get_step(71199) != 1) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(L1ItemId.ADENA, 1000000))
                    {
                        L1ItemInstance item = ItemTable.Instance.createItem(41341); // ジェロンの教本
                        if (item != null)
                        {
                            if (pc.Inventory.checkAddItem(item, 1) == 0)
                            {
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            }
                        }
                        pc.Inventory.consumeItem(41340, 1);
                        pc.Quest.set_step(71199, 255);
                        htmlid = "jeron6";
                    }
                    else
                    {
                        htmlid = "jeron8";
                    }
                }
                else if (s == "C")
                {
                    if ((pc.Quest.get_step(71199) != 1) || pc.Inventory.checkItem(21059, 1))
                    {
                        return;
                    }
                    if (pc.Inventory.consumeItem(41342, 1))
                    { // メデューサの血
                        L1ItemInstance item = ItemTable.Instance.createItem(41341); // ジェロンの教本
                        if (item != null)
                        {
                            if (pc.Inventory.checkAddItem(item, 1) == 0)
                            {
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            }
                        }
                        pc.Inventory.consumeItem(41340, 1);
                        pc.Quest.set_step(71199, 255);
                        htmlid = "jeron5";
                    }
                    else
                    {
                        htmlid = "jeron9";
                    }
                }
            }
            // 占星術師ケプリシャ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80079)
            {
                // ケプリシャと魂の契約を結ぶ
                if (s == "0")
                {
                    if (!pc.Inventory.checkItem(41312))
                    { // 占星術師の壺
                        L1ItemInstance item = pc.Inventory.storeItem(41312, 1);
                        if (item != null)
                        {
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                            pc.Quest.set_step(L1Quest.QUEST_KEPLISHA, L1Quest.QUEST_END);
                        }
                        htmlid = "keplisha7";
                    }
                }
                // 援助金を出して運勢を見る
                else if (s == "1")
                {
                    if (!pc.Inventory.checkItem(41314))
                    { // 占星術師のお守り
                        if (pc.Inventory.checkItem(L1ItemId.ADENA, 1000))
                        {
                            materials = new int[] { L1ItemId.ADENA, 41313 }; // アデナ、占星術師の玉
                            counts = new int[] { 1000, 1 };
                            createitem = new int[] { 41314 }; // 占星術師のお守り
                            createcount = new int[] { 1 };
                            int htmlA = RandomHelper.Next(3) + 1;
                            int htmlB = RandomHelper.Next(100) + 1;
                            switch (htmlA)
                            {
                                case 1:
                                    htmlid = "horosa" + htmlB; // horosa1 ~
                                                               // horosa100
                                    break;
                                case 2:
                                    htmlid = "horosb" + htmlB; // horosb1 ~
                                                               // horosb100
                                    break;
                                case 3:
                                    htmlid = "horosc" + htmlB; // horosc1 ~
                                                               // horosc100
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            htmlid = "keplisha8";
                        }
                    }
                }
                // ケプリシャから祝福を受ける
                else if (s == "2")
                {
                    if (pc.TempCharGfx != pc.ClassId)
                    {
                        htmlid = "keplisha9";
                    }
                    else
                    {
                        if (pc.Inventory.checkItem(41314))
                        { // 占星術師のお守り
                            pc.Inventory.consumeItem(41314, 1); // 占星術師のお守り
                            int html = RandomHelper.Next(9) + 1;
                            int PolyId = 6180 + RandomHelper.Next(64);
                            polyByKeplisha(client, PolyId);
                            switch (html)
                            {
                                case 1:
                                    htmlid = "horomon11";
                                    break;
                                case 2:
                                    htmlid = "horomon12";
                                    break;
                                case 3:
                                    htmlid = "horomon13";
                                    break;
                                case 4:
                                    htmlid = "horomon21";
                                    break;
                                case 5:
                                    htmlid = "horomon22";
                                    break;
                                case 6:
                                    htmlid = "horomon23";
                                    break;
                                case 7:
                                    htmlid = "horomon31";
                                    break;
                                case 8:
                                    htmlid = "horomon32";
                                    break;
                                case 9:
                                    htmlid = "horomon33";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                // 壺を割って契約を破棄する
                else if (s == "3")
                {
                    if (pc.Inventory.checkItem(41312))
                    { // 占星術師の壺
                        pc.Inventory.consumeItem(41312, 1);
                        htmlid = "";
                    }
                    if (pc.Inventory.checkItem(41313))
                    { // 占星術師の玉
                        pc.Inventory.consumeItem(41313, 1);
                        htmlid = "";
                    }
                    if (pc.Inventory.checkItem(41314))
                    { // 占星術師のお守り
                        pc.Inventory.consumeItem(41314, 1);
                        htmlid = "";
                    }
                }
            }
            // 釣魚小童 波爾 (進入釣魚池)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80082)
            {
                if (s == "a")
                {
                    if (pc.Level < 15)
                    {
                        htmlid = "fk_in_lv"; // 魔法釣魚池只對15等級以上的冒險家開放。
                    }
                    else if (pc.Inventory.consumeItem(L1ItemId.ADENA, 1000))
                    {
                        L1PolyMorph.undoPoly(pc);
                        L1Teleport.teleport(pc, 32742, 32799, (short)5300, 4, true);
                    }
                    else
                    {
                        htmlid = "fk_in_0";
                    }
                }
            }
            // 怪しいオーク商人 パルーム
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80084)
            {
                // 「資源リストをもらう」
                if (s == "q")
                {
                    if (pc.Inventory.checkItem(41356, 1))
                    {
                        htmlid = "rparum4";
                    }
                    else
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(41356, 1);
                        if (item != null)
                        {
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name)); // \f1%0が%1をくれました。
                        }
                        htmlid = "rparum3";
                    }
                }
            }
            // アデン騎馬団員
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80105)
            {
                // 「新たな力をくださいる」
                if (s == "c")
                {
                    if (pc.Crown)
                    {
                        if (pc.Inventory.checkItem(20383, 1))
                        {
                            if (pc.Inventory.checkItem(L1ItemId.ADENA, 100000))
                            {
                                L1ItemInstance item = pc.Inventory.findItemId(20383);
                                if ((item != null) && (item.ChargeCount != 50))
                                {
                                    item.ChargeCount = 50;
                                    pc.Inventory.updateItem(item, L1PcInventory.COL_CHARGE_COUNT);
                                    pc.Inventory.consumeItem(L1ItemId.ADENA, 100000);
                                    htmlid = "";
                                }
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(337, "$4")); // アデナが不足しています。
                            }
                        }
                    }
                }
            }
            // 補佐官イリス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71126)
            {
                // 「はい。私がご協力しましょう」
                if (s == "B")
                {
                    if (pc.Inventory.checkItem(41007, 1))
                    { // イリスの命令書：霊魂の安息
                        htmlid = "eris10";
                    }
                    else
                    {
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        L1ItemInstance item = pc.Inventory.storeItem(41007, 1);
                        string npcName = npc.NpcTemplate.get_name();
                        string itemName = item.Item.Name;
                        pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                        htmlid = "eris6";
                    }
                }
                else if (s == "C")
                {
                    if (pc.Inventory.checkItem(41009, 1))
                    { // イリスの命令書：同盟の意思
                        htmlid = "eris10";
                    }
                    else
                    {
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        L1ItemInstance item = pc.Inventory.storeItem(41009, 1);
                        string npcName = npc.NpcTemplate.get_name();
                        string itemName = item.Item.Name;
                        pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                        htmlid = "eris8";
                    }
                }
                else if (s == "A")
                {
                    if (pc.Inventory.checkItem(41007, 1))
                    { // イリスの命令書：霊魂の安息
                        if (pc.Inventory.checkItem(40969, 20))
                        { // ダークエルフ魂の結晶体
                            htmlid = "eris18";
                            materials = new int[] { 40969, 41007 };
                            counts = new int[] { 20, 1 };
                            createitem = new int[] { 41008 }; // イリスのバック
                            createcount = new int[] { 1 };
                        }
                        else
                        {
                            htmlid = "eris5";
                        }
                    }
                    else
                    {
                        htmlid = "eris2";
                    }
                }
                else if (s == "E")
                {
                    if (pc.Inventory.checkItem(41010, 1))
                    { // イリスの推薦書
                        htmlid = "eris19";
                    }
                    else
                    {
                        htmlid = "eris7";
                    }
                }
                else if (s == "D")
                {
                    if (pc.Inventory.checkItem(41010, 1))
                    { // イリスの推薦書
                        htmlid = "eris19";
                    }
                    else
                    {
                        if (pc.Inventory.checkItem(41009, 1))
                        { // イリスの命令書：同盟の意思
                            if (pc.Inventory.checkItem(40959, 1))
                            { // 冥法軍王の印章
                                htmlid = "eris17";
                                materials = new int[] { 40959, 41009 }; // 冥法軍王の印章
                                counts = new int[] { 1, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40960, 1))
                            { // 魔霊軍王の印章
                                htmlid = "eris16";
                                materials = new int[] { 40960, 41009 }; // 魔霊軍王の印章
                                counts = new int[] { 1, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40961, 1))
                            { // 魔獣霊軍王の印章
                                htmlid = "eris15";
                                materials = new int[] { 40961, 41009 }; // 魔獣軍王の印章
                                counts = new int[] { 1, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40962, 1))
                            { // 暗殺軍王の印章
                                htmlid = "eris14";
                                materials = new int[] { 40962, 41009 }; // 暗殺軍王の印章
                                counts = new int[] { 1, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40635, 10))
                            { // 魔霊軍のバッジ
                                htmlid = "eris12";
                                materials = new int[] { 40635, 41009 }; // 魔霊軍のバッジ
                                counts = new int[] { 10, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40638, 10))
                            { // 魔獣軍のバッジ
                                htmlid = "eris11";
                                materials = new int[] { 40638, 41009 }; // 魔霊軍のバッジ
                                counts = new int[] { 10, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40642, 10))
                            { // 冥法軍のバッジ
                                htmlid = "eris13";
                                materials = new int[] { 40642, 41009 }; // 冥法軍のバッジ
                                counts = new int[] { 10, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else if (pc.Inventory.checkItem(40667, 10))
                            { // 暗殺軍のバッジ
                                htmlid = "eris13";
                                materials = new int[] { 40667, 41009 }; // 暗殺軍のバッジ
                                counts = new int[] { 10, 1 };
                                createitem = new int[] { 41010 }; // イリスの推薦書
                                createcount = new int[] { 1 };
                            }
                            else
                            {
                                htmlid = "eris8";
                            }
                        }
                        else
                        {
                            htmlid = "eris7";
                        }
                    }
                }
            }
            // 倒れた航海士
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80076)
            {
                if (s == "A")
                {
                    int[] diaryno = new int[] { 49082, 49083 };
                    int pid = RandomHelper.Next(diaryno.Length);
                    int di = diaryno[pid];
                    if (di == 49082)
                    { // 奇数ページ抜け
                        htmlid = "voyager6a";
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        L1ItemInstance item = pc.Inventory.storeItem(di, 1);
                        string npcName = npc.NpcTemplate.get_name();
                        string itemName = item.Item.Name;
                        pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                    }
                    else if (di == 49083)
                    { // 偶数ページ抜け
                        htmlid = "voyager6b";
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        L1ItemInstance item = pc.Inventory.storeItem(di, 1);
                        string npcName = npc.NpcTemplate.get_name();
                        string itemName = item.Item.Name;
                        pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                    }
                }
            }
            // 錬金術師 ペリター
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71128)
            {
                if (s == "A")
                {
                    if (pc.Inventory.checkItem(41010, 1))
                    { // イリスの推薦書
                        htmlid = "perita2";
                    }
                    else
                    {
                        htmlid = "perita3";
                    }
                }
                else if (s == "p")
                {
                    // 呪われたブラックイアリング判別
                    if (pc.Inventory.checkItem(40987, 1) && pc.Inventory.checkItem(40988, 1) && pc.Inventory.checkItem(40989, 1))
                    { // ウォーリアクラス
                        htmlid = "perita43";
                    }
                    else if (pc.Inventory.checkItem(40987, 1) && pc.Inventory.checkItem(40989, 1))
                    { // ウォーリアクラス
                        htmlid = "perita44";
                    }
                    else if (pc.Inventory.checkItem(40987, 1) && pc.Inventory.checkItem(40988, 1))
                    { // ナイトクラス
                        htmlid = "perita45";
                    }
                    else if (pc.Inventory.checkItem(40988, 1) && pc.Inventory.checkItem(40989, 1))
                    { // ウォーリアクラス
                        htmlid = "perita47";
                    }
                    else if (pc.Inventory.checkItem(40987, 1))
                    { // ウィザードクラス
                        htmlid = "perita46";
                    }
                    else if (pc.Inventory.checkItem(40988, 1))
                    { // ナイトクラス
                        htmlid = "perita49";
                    }
                    else if (pc.Inventory.checkItem(40987, 1))
                    { // ウォーリアクラス
                        htmlid = "perita48";
                    }
                    else
                    {
                        htmlid = "perita50";
                    }
                }
                else if (s == "q")
                {
                    // ブラックイアリング判別
                    if (pc.Inventory.checkItem(41173, 1) && pc.Inventory.checkItem(41174, 1) && pc.Inventory.checkItem(41175, 1))
                    { // ウォーリアクラス
                        htmlid = "perita54";
                    }
                    else if (pc.Inventory.checkItem(41173, 1) && pc.Inventory.checkItem(41175, 1))
                    { // ウォーリアクラス
                        htmlid = "perita55";
                    }
                    else if (pc.Inventory.checkItem(41173, 1) && pc.Inventory.checkItem(41174, 1))
                    { // ナイトクラス
                        htmlid = "perita56";
                    }
                    else if (pc.Inventory.checkItem(41174, 1) && pc.Inventory.checkItem(41175, 1))
                    { // ウォーリアクラス
                        htmlid = "perita58";
                    }
                    else if (pc.Inventory.checkItem(41174, 1))
                    { // ウィザードクラス
                        htmlid = "perita57";
                    }
                    else if (pc.Inventory.checkItem(41175, 1))
                    { // ナイトクラス
                        htmlid = "perita60";
                    }
                    else if (pc.Inventory.checkItem(41176, 1))
                    { // ウォーリアクラス
                        htmlid = "perita59";
                    }
                    else
                    {
                        htmlid = "perita61";
                    }
                }
                else if (s == "s")
                {
                    // ミステリアス ブラックイアリング判別
                    if (pc.Inventory.checkItem(41161, 1) && pc.Inventory.checkItem(41162, 1) && pc.Inventory.checkItem(41163, 1))
                    { // ウォーリアクラス
                        htmlid = "perita62";
                    }
                    else if (pc.Inventory.checkItem(41161, 1) && pc.Inventory.checkItem(41163, 1))
                    { // ウォーリアクラス
                        htmlid = "perita63";
                    }
                    else if (pc.Inventory.checkItem(41161, 1) && pc.Inventory.checkItem(41162, 1))
                    { // ナイトクラス
                        htmlid = "perita64";
                    }
                    else if (pc.Inventory.checkItem(41162, 1) && pc.Inventory.checkItem(41163, 1))
                    { // ウォーリアクラス
                        htmlid = "perita66";
                    }
                    else if (pc.Inventory.checkItem(41161, 1))
                    { // ウィザードクラス
                        htmlid = "perita65";
                    }
                    else if (pc.Inventory.checkItem(41162, 1))
                    { // ナイトクラス
                        htmlid = "perita68";
                    }
                    else if (pc.Inventory.checkItem(41163, 1))
                    { // ウォーリアクラス
                        htmlid = "perita67";
                    }
                    else
                    {
                        htmlid = "perita69";
                    }
                }
                else if (s == "B")
                {
                    // 浄化のポーション
                    if (pc.Inventory.checkItem(40651, 10) && pc.Inventory.checkItem(40643, 10) && pc.Inventory.checkItem(40618, 10) && pc.Inventory.checkItem(40645, 10) && pc.Inventory.checkItem(40676, 10) && pc.Inventory.checkItem(40442, 5) && pc.Inventory.checkItem(40051, 1))
                    { // 高級エメラルド
                        htmlid = "perita7";
                        materials = new int[] { 40651, 40643, 40618, 40645, 40676, 40442, 40051 };
                        counts = new int[] { 10, 10, 10, 10, 20, 5, 1 };
                        createitem = new int[] { 40925 }; // 浄化のポーション
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita8";
                    }
                }
                else if (s == "G" || s == "h" || s == "i")
                {
                    // ミステリアス ポーション：１段階
                    if (pc.Inventory.checkItem(40651, 5) && pc.Inventory.checkItem(40643, 5) && pc.Inventory.checkItem(40618, 5) && pc.Inventory.checkItem(40645, 5) && pc.Inventory.checkItem(40676, 5) && pc.Inventory.checkItem(40675, 5) && pc.Inventory.checkItem(40049, 3) && pc.Inventory.checkItem(40051, 1))
                    { // 高級エメラルド
                        htmlid = "perita27";
                        materials = new int[] { 40651, 40643, 40618, 40645, 40676, 40675, 40049, 40051 };
                        counts = new int[] { 5, 5, 5, 5, 10, 10, 3, 1 };
                        createitem = new int[] { 40926 }; // ミステリアスポーション：１段階
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita28";
                    }
                }
                else if (s == "H" || s == "j" || s == "k")
                {
                    // ミステリアス ポーション：２段階
                    if (pc.Inventory.checkItem(40651, 10) && pc.Inventory.checkItem(40643, 10) && pc.Inventory.checkItem(40618, 10) && pc.Inventory.checkItem(40645, 10) && pc.Inventory.checkItem(40676, 20) && pc.Inventory.checkItem(40675, 10) && pc.Inventory.checkItem(40048, 3) && pc.Inventory.checkItem(40051, 1))
                    { // 高級エメラルド
                        htmlid = "perita29";
                        materials = new int[] { 40651, 40643, 40618, 40645, 40676, 40675, 40048, 40051 };
                        counts = new int[] { 10, 10, 10, 10, 20, 10, 3, 1 };
                        createitem = new int[] { 40927 }; // ミステリアスポーション：２段階
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita30";
                    }
                }
                else if (s == "I" || s == "l" || s == "m")
                {
                    // ミステリアス ポーション：３段階
                    if (pc.Inventory.checkItem(40651, 20) && pc.Inventory.checkItem(40643, 20) && pc.Inventory.checkItem(40618, 20) && pc.Inventory.checkItem(40645, 20) && pc.Inventory.checkItem(40676, 30) && pc.Inventory.checkItem(40675, 10) && pc.Inventory.checkItem(40050, 3) && pc.Inventory.checkItem(40051, 1))
                    { // 高級エメラルド
                        htmlid = "perita31";
                        materials = new int[] { 40651, 40643, 40618, 40645, 40676, 40675, 40050, 40051 };
                        counts = new int[] { 20, 20, 20, 20, 30, 10, 3, 1 };
                        createitem = new int[] { 40928 }; // ミステリアスポーション：３段階
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita32";
                    }
                }
                else if (s == "J" || s == "n" || s == "o")
                {
                    // ミステリアス ポーション：４段階
                    if (pc.Inventory.checkItem(40651, 30) && pc.Inventory.checkItem(40643, 30) && pc.Inventory.checkItem(40618, 30) && pc.Inventory.checkItem(40645, 30) && pc.Inventory.checkItem(40676, 30) && pc.Inventory.checkItem(40675, 20) && pc.Inventory.checkItem(40052, 1) && pc.Inventory.checkItem(40051, 1))
                    { // 高級エメラルド
                        htmlid = "perita33";
                        materials = new int[] { 40651, 40643, 40618, 40645, 40676, 40675, 40052, 40051 };
                        counts = new int[] { 30, 30, 30, 30, 30, 20, 1, 1 };
                        createitem = new int[] { 40928 }; // ミステリアスポーション：４段階
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita34";
                    }
                }
                else if (s == "K")
                { // １段階イアリング(霊魂のイアリング)
                    int earinga = 0;
                    int earingb = 0;
                    if (l1PcInventory.checkEquipped(21014) || l1PcInventory.checkEquipped(21006) || l1PcInventory.checkEquipped(21007))
                    {
                        htmlid = "perita36";
                    }
                    else if (pc.Inventory.checkItem(21014, 1))
                    { // ウィザードクラス
                        earinga = 21014;
                        earingb = 41176;
                    }
                    else if (pc.Inventory.checkItem(21006, 1))
                    { // ナイトクラス
                        earinga = 21006;
                        earingb = 41177;
                    }
                    else if (pc.Inventory.checkItem(21007, 1))
                    { // ウォーリアクラス
                        earinga = 21007;
                        earingb = 41178;
                    }
                    else
                    {
                        htmlid = "perita36";
                    }
                    if (earinga > 0)
                    {
                        materials = new int[] { earinga };
                        counts = new int[] { 1 };
                        createitem = new int[] { earingb };
                        createcount = new int[] { 1 };
                    }
                }
                else if (s == "L")
                { // ２段階イアリング(知恵のイアリング)
                    if (l1PcInventory.checkEquipped(21015))
                    {
                        htmlid = "perita22";
                    }
                    else if (pc.Inventory.checkItem(21015, 1))
                    {
                        materials = new int[] { 21015 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 41179 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita22";
                    }
                }
                else if (s == "M")
                { // ３段階イアリング(真実のイアリング)
                    if (l1PcInventory.checkEquipped(21016))
                    {
                        htmlid = "perita26";
                    }
                    else if (pc.Inventory.checkItem(21016, 1))
                    {
                        materials = new int[] { 21016 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 41182 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita26";
                    }
                }
                else if (s == "b")
                { // ２段階イアリング(情熱のイアリング)
                    if (l1PcInventory.checkEquipped(21009))
                    {
                        htmlid = "perita39";
                    }
                    else if (pc.Inventory.checkItem(21009, 1))
                    {
                        materials = new int[] { 21009 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 41180 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita39";
                    }
                }
                else if (s == "d")
                { // ３段階イアリング(名誉のイアリング)
                    if (l1PcInventory.checkEquipped(21012))
                    {
                        htmlid = "perita41";
                    }
                    else if (pc.Inventory.checkItem(21012, 1))
                    {
                        materials = new int[] { 21012 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 41183 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita41";
                    }
                }
                else if (s == "a")
                { // ２段階イアリング(憤怒のイアリング)
                    if (l1PcInventory.checkEquipped(21008))
                    {
                        htmlid = "perita38";
                    }
                    else if (pc.Inventory.checkItem(21008, 1))
                    {
                        materials = new int[] { 21008 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 41181 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita38";
                    }
                }
                else if (s == "c")
                { // ３段階イアリング(勇猛のイアリング)
                    if (l1PcInventory.checkEquipped(21010))
                    {
                        htmlid = "perita40";
                    }
                    else if (pc.Inventory.checkItem(21010, 1))
                    {
                        materials = new int[] { 21010 };
                        counts = new int[] { 1 };
                        createitem = new int[] { 41184 };
                        createcount = new int[] { 1 };
                    }
                    else
                    {
                        htmlid = "perita40";
                    }
                }
            }
            // 宝石細工師 ルームィス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71129)
            {
                if (s == "Z")
                {
                    htmlid = "rumtis2";
                }
                else if (s == "Y")
                {
                    if (pc.Inventory.checkItem(41010, 1))
                    { // イリスの推薦書
                        htmlid = "rumtis3";
                    }
                    else
                    {
                        htmlid = "rumtis4";
                    }
                }
                else if (s == "q")
                {
                    htmlid = "rumtis92";
                }
                else if (s == "A")
                {
                    if (pc.Inventory.checkItem(41161, 1))
                    {
                        // ミステリアスブラックイアリング
                        htmlid = "rumtis6";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "B")
                {
                    if (pc.Inventory.checkItem(41164, 1))
                    {
                        // ミステリアスウィザードイアリング
                        htmlid = "rumtis7";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "C")
                {
                    if (pc.Inventory.checkItem(41167, 1))
                    {
                        // ミステリアスグレーウィザードイアリング
                        htmlid = "rumtis8";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "T")
                {
                    if (pc.Inventory.checkItem(41167, 1))
                    {
                        // ミステリアスホワイトウィザードイアリング
                        htmlid = "rumtis9";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "w")
                {
                    if (pc.Inventory.checkItem(41162, 1))
                    {
                        // ミステリアスブラックイアリング
                        htmlid = "rumtis14";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "x")
                {
                    if (pc.Inventory.checkItem(41165, 1))
                    {
                        // ミステリアスナイトイアリング
                        htmlid = "rumtis15";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "y")
                {
                    if (pc.Inventory.checkItem(41168, 1))
                    {
                        // ミステリアスグレーナイトイアリング
                        htmlid = "rumtis16";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "z")
                {
                    if (pc.Inventory.checkItem(41171, 1))
                    {
                        // ミステリアスホワイトナイトイアリング
                        htmlid = "rumtis17";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "U")
                {
                    if (pc.Inventory.checkItem(41163, 1))
                    {
                        // ミステリアスブラックイアリング
                        htmlid = "rumtis10";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "V")
                {
                    if (pc.Inventory.checkItem(41166, 1))
                    {
                        // ミステリアスウォーリアイアリング
                        htmlid = "rumtis11";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "W")
                {
                    if (pc.Inventory.checkItem(41169, 1))
                    {
                        // ミステリアスグレーウォーリアイアリング
                        htmlid = "rumtis12";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "X")
                {
                    if (pc.Inventory.checkItem(41172, 1))
                    {
                        // ミステリアスホワイウォーリアイアリング
                        htmlid = "rumtis13";
                    }
                    else
                    {
                        htmlid = "rumtis101";
                    }
                }
                else if (s == "D" || s == "E" || s == "F" || s == "G")
                {
                    int insn = 0;
                    int bacn = 0;
                    int me = 0;
                    int mr = 0;
                    int mj = 0;
                    int an = 0;
                    int men = 0;
                    int mrn = 0;
                    int mjn = 0;
                    int ann = 0;
                    if (pc.Inventory.checkItem(40959, 1) && pc.Inventory.checkItem(40960, 1) && pc.Inventory.checkItem(40961, 1) && pc.Inventory.checkItem(40962, 1))
                    { // 暗殺軍王の印章
                        insn = 1;
                        me = 40959;
                        mr = 40960;
                        mj = 40961;
                        an = 40962;
                        men = 1;
                        mrn = 1;
                        mjn = 1;
                        ann = 1;
                    }
                    else if (pc.Inventory.checkItem(40642, 10) && pc.Inventory.checkItem(40635, 10) && pc.Inventory.checkItem(40638, 10) && pc.Inventory.checkItem(40667, 10))
                    { // 暗殺軍のバッジ
                        bacn = 1;
                        me = 40642;
                        mr = 40635;
                        mj = 40638;
                        an = 40667;
                        men = 10;
                        mrn = 10;
                        mjn = 10;
                        ann = 10;
                    }
                    if (pc.Inventory.checkItem(40046, 1) && pc.Inventory.checkItem(40618, 5) && pc.Inventory.checkItem(40643, 5) && pc.Inventory.checkItem(40645, 5) && pc.Inventory.checkItem(40651, 5) && pc.Inventory.checkItem(40676, 5))
                    { // 闇の息吹
                        if ((insn == 1) || (bacn == 1))
                        {
                            htmlid = "rumtis60";
                            materials = new int[] { me, mr, mj, an, 40046, 40618, 40643, 40651, 40676 };
                            counts = new int[] { men, mrn, mjn, ann, 1, 5, 5, 5, 5, 5 };
                            createitem = new int[] { 40926 }; // 加工されたサファイア：１段階
                            createcount = new int[] { 1 };
                        }
                        else
                        {
                            htmlid = "rumtis18";
                        }
                    }
                }
            }
            // アタロゼ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71119)
            {
                // 「ラスタバドの歴史書1章から8章まで全部渡す」
                if (s == "request las history book")
                {
                    materials = new int[] { 41019, 41020, 41021, 41022, 41023, 41024, 41025, 41026 };
                    counts = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
                    createitem = new int[] { 41027 };
                    createcount = new int[] { 1 };
                    htmlid = "";
                }
            }
            // 長老随行員クロレンス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71170)
            {
                // 「ラスタバドの歴史書を渡す」
                if (s == "request las weapon manual")
                {
                    materials = new int[] { 41027 };
                    counts = new int[] { 1 };
                    createitem = new int[] { 40965 };
                    createcount = new int[] { 1 };
                    htmlid = "";
                }
            }
            // 真冥王 ダンテス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71168)
            {
                // 「異界の魔物がいる場所へ送ってください」
                if (s == "a")
                {
                    if (pc.Inventory.checkItem(41028, 1))
                    {
                        L1Teleport.teleport(pc, 32648, 32921, (short)535, 6, true);
                        pc.Inventory.consumeItem(41028, 1);
                    }
                }
            }
            // 諜報員(欲望の洞窟側)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80067)
            {
                // 「動揺しつつも承諾する」
                if (s == "n")
                {
                    htmlid = "";
                    poly(client, 6034);
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_ids = { 41132, 41133, 41134 };
                    int[] item_ids = new int[] { 41132, 41133, 41134 };
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_amounts = { 1, 1, 1 };
                    int[] item_amounts = new int[] { 1, 1, 1 };
                    for (int i = 0; i < item_ids.Length; i++)
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                        pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        pc.Quest.set_step(L1Quest.QUEST_DESIRE, 1);
                    }
                    // 「そんな任務はやめる」
                }
                else if (s == "d")
                {
                    htmlid = "minicod09";
                    pc.Inventory.consumeItem(41130, 1);
                    pc.Inventory.consumeItem(41131, 1);
                    // 「初期化する」
                }
                else if (s == "k")
                {
                    htmlid = "";
                    pc.Inventory.consumeItem(41132, 1); // 血痕の堕落した粉
                    pc.Inventory.consumeItem(41133, 1); // 血痕の無力した粉
                    pc.Inventory.consumeItem(41134, 1); // 血痕の我執した粉
                    pc.Inventory.consumeItem(41135, 1); // カヘルの堕落した精髄
                    pc.Inventory.consumeItem(41136, 1); // カヘルの無力した精髄
                    pc.Inventory.consumeItem(41137, 1); // カヘルの我執した精髄
                    pc.Inventory.consumeItem(41138, 1); // カヘルの精髄
                    pc.Quest.set_step(L1Quest.QUEST_DESIRE, 0);
                    // 精髄を渡す
                }
                else if (s == "e")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_DESIRE) == L1Quest.QUEST_END) || (pc.KarmaLevel >= 1))
                    {
                        htmlid = "";
                    }
                    else
                    {
                        if (pc.Inventory.checkItem(41138))
                        {
                            htmlid = "";
                            pc.addKarma((int)(1600 * Config.RATE_KARMA));
                            pc.Inventory.consumeItem(41130, 1); // 血痕の契約書
                            pc.Inventory.consumeItem(41131, 1); // 血痕の指令書
                            pc.Inventory.consumeItem(41138, 1); // カヘルの精髄
                            pc.Quest.set_step(L1Quest.QUEST_DESIRE, L1Quest.QUEST_END);
                        }
                        else
                        {
                            htmlid = "minicod04";
                        }
                    }
                    // プレゼントをもらう
                }
                else if (s == "g")
                {
                    htmlid = "";
                    L1ItemInstance item = pc.Inventory.storeItem(41130, 1); // 血痕の契約書
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                }
            }
            // 諜報員(影の神殿側)
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81202)
            {
                // 「頭にくるが承諾する」
                if (s == "n")
                {
                    htmlid = "";
                    poly(client, 6035);
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_ids = { 41123, 41124, 41125 };
                    int[] item_ids = new int[] { 41123, 41124, 41125 };
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final int[] item_amounts = { 1, 1, 1 };
                    int[] item_amounts = new int[] { 1, 1, 1 };
                    for (int i = 0; i < item_ids.Length; i++)
                    {
                        L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                        pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        pc.Quest.set_step(L1Quest.QUEST_SHADOWS, 1);
                    }
                    // 「そんな任務はやめる」
                }
                else if (s == "d")
                {
                    htmlid = "minitos09";
                    pc.Inventory.consumeItem(41121, 1);
                    pc.Inventory.consumeItem(41122, 1);
                    // 「初期化する」
                }
                else if (s == "k")
                {
                    htmlid = "";
                    pc.Inventory.consumeItem(41123, 1); // カヘルの堕落した粉
                    pc.Inventory.consumeItem(41124, 1); // カヘルの無力した粉
                    pc.Inventory.consumeItem(41125, 1); // カヘルの我執した粉
                    pc.Inventory.consumeItem(41126, 1); // 血痕の堕落した精髄
                    pc.Inventory.consumeItem(41127, 1); // 血痕の無力した精髄
                    pc.Inventory.consumeItem(41128, 1); // 血痕の我執した精髄
                    pc.Inventory.consumeItem(41129, 1); // 血痕の精髄
                    pc.Quest.set_step(L1Quest.QUEST_SHADOWS, 0);
                    // 精髄を渡す
                }
                else if (s == "e")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_SHADOWS) == L1Quest.QUEST_END) || (pc.KarmaLevel >= 1))
                    {
                        htmlid = "";
                    }
                    else
                    {
                        if (pc.Inventory.checkItem(41129))
                        {
                            htmlid = "";
                            pc.addKarma((int)(-1600 * Config.RATE_KARMA));
                            pc.Inventory.consumeItem(41121, 1); // カヘルの契約書
                            pc.Inventory.consumeItem(41122, 1); // カヘルの指令書
                            pc.Inventory.consumeItem(41129, 1); // 血痕の精髄
                            pc.Quest.set_step(L1Quest.QUEST_SHADOWS, L1Quest.QUEST_END);
                        }
                        else
                        {
                            htmlid = "minitos04";
                        }
                    }
                    // 素早く受取る
                }
                else if (s == "g")
                {
                    htmlid = "";
                    L1ItemInstance item = pc.Inventory.storeItem(41121, 1); // カヘルの契約書
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                }
            }
            // ゾウのストーンゴーレム
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71252)
            {
                int weapon1 = 0;
                int weapon2 = 0;
                int newWeapon = 0;
                if (s == "A")
                {
                    weapon1 = 5; // +7エルヴンダガー
                    weapon2 = 6; // +7ラスタバドダガー
                    newWeapon = 259; // マナバーラード
                    htmlid = "joegolem9";
                }
                else if (s == "B")
                {
                    weapon1 = 145; // +7バーサーカーアックス
                    weapon2 = 148; // +7グレートアックス
                    newWeapon = 260; // レイジングウィンド
                    htmlid = "joegolem10";
                }
                else if (s == "C")
                {
                    weapon1 = 52; // +7ツーハンドソード
                    weapon2 = 64; // +7グレートソード
                    newWeapon = 262; // ディストラクション
                    htmlid = "joegolem11";
                }
                else if (s == "D")
                {
                    weapon1 = 125; // +7ソーサリースタッフ
                    weapon2 = 129; // +7メイジスタッフ
                    newWeapon = 261; // アークメイジスタッフ
                    htmlid = "joegolem12";
                }
                else if (s == "E")
                {
                    weapon1 = 99; // +7エルブンスピアー
                    weapon2 = 104; // +7フォチャード
                    newWeapon = 263; // フリージングランサー
                    htmlid = "joegolem13";
                }
                else if (s == "F")
                {
                    weapon1 = 32; // +7グラディウス
                    weapon2 = 42; // +7レイピア
                    newWeapon = 264; // ライトニングエッジ
                    htmlid = "joegolem14";
                }
                if (pc.Inventory.checkEnchantItem(weapon1, 7, 1) && pc.Inventory.checkEnchantItem(weapon2, 7, 1) && pc.Inventory.checkItem(41246, 1000) && pc.Inventory.checkItem(49143, 10))
                { // 勇気の結晶
                    pc.Inventory.consumeEnchantItem(weapon1, 7, 1);
                    pc.Inventory.consumeEnchantItem(weapon2, 7, 1);
                    pc.Inventory.consumeItem(41246, 1000);
                    pc.Inventory.consumeItem(49143, 10);
                    L1ItemInstance item = pc.Inventory.storeItem(newWeapon, 1);
                    pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                }
                else
                {
                    htmlid = "joegolem15";
                    if (!pc.Inventory.checkEnchantItem(weapon1, 7, 1))
                    {
                        pc.sendPackets(new S_ServerMessage(337, "+7 " + ItemTable.Instance.getTemplate(weapon1).Name)); // \f1%0が不足しています。
                    }
                    if (!pc.Inventory.checkEnchantItem(weapon2, 7, 1))
                    {
                        pc.sendPackets(new S_ServerMessage(337, "+7 " + ItemTable.Instance.getTemplate(weapon2).Name)); // \f1%0が不足しています。
                    }
                    if (!pc.Inventory.checkItem(41246, 1000))
                    {
                        int itemCount = 0;
                        itemCount = 1000 - pc.Inventory.countItems(41246);
                        pc.sendPackets(new S_ServerMessage(337, ItemTable.Instance.getTemplate(41246).Name + "(" + itemCount + ")")); // \f1%0が不足しています。
                    }
                    if (!pc.Inventory.checkItem(49143, 10))
                    {
                        int itemCount = 0;
                        itemCount = 10 - pc.Inventory.countItems(49143);
                        pc.sendPackets(new S_ServerMessage(337, ItemTable.Instance.getTemplate(49143).Name + "(" + itemCount + ")")); // \f1%0が不足しています。
                    }
                }
            }
            // ゾウのストーンゴーレム テーベ砂漠
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71253)
            {
                // 「歪みのコアを作る」
                if (s == "A")
                {
                    if (pc.Inventory.checkItem(49101, 100))
                    {
                        materials = new int[] { 49101 };
                        counts = new int[] { 100 };
                        createitem = new int[] { 49092 };
                        createcount = new int[] { 1 };
                        htmlid = "joegolem18";
                    }
                    else
                    {
                        htmlid = "joegolem19";
                    }
                }
                else if (s == "B")
                {
                    if (pc.Inventory.checkItem(49101, 1))
                    {
                        pc.Inventory.consumeItem(49101, 1);
                        L1Teleport.teleport(pc, 33966, 33253, (short)4, 5, true);
                        htmlid = "";
                    }
                    else
                    {
                        htmlid = "joegolem20";
                    }
                }
            }
            // テーベ オシリス祭壇のキーパー
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71255)
            {
                // 「テーベオシリス祭壇の鍵を持っているなら、オシリスの祭壇にお送りしましょう。」
                if (s == "e")
                {
                    if (pc.Inventory.checkItem(49242, 1))
                    { // 鍵のチェック(20人限定/時の歪みが現れてから2h30は未実装)
                        pc.Inventory.consumeItem(49242, 1);
                        L1Teleport.teleport(pc, 32735, 32831, (short)782, 2, true);
                        htmlid = "";
                    }
                    else
                    {
                        htmlid = "tebegate3";
                        // 「上限人数に達している場合は」
                        // htmlid = "tebegate4";
                    }
                }
            }
            // ロビンフッド
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71256)
            {
                if (s == "E")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_MOONOFLONGBOW) == 8) && pc.Inventory.checkItem(40491, 30) && pc.Inventory.checkItem(40495, 40) && pc.Inventory.checkItem(100, 1) && pc.Inventory.checkItem(40509, 12) && pc.Inventory.checkItem(40052, 1) && pc.Inventory.checkItem(40053, 1) && pc.Inventory.checkItem(40054, 1) && pc.Inventory.checkItem(40055, 1) && pc.Inventory.checkItem(41347, 1) && pc.Inventory.checkItem(41350, 1))
                    {
                        pc.Inventory.consumeItem(40491, 30);
                        pc.Inventory.consumeItem(40495, 40);
                        pc.Inventory.consumeItem(100, 1);
                        pc.Inventory.consumeItem(40509, 12);
                        pc.Inventory.consumeItem(40052, 1);
                        pc.Inventory.consumeItem(40053, 1);
                        pc.Inventory.consumeItem(40054, 1);
                        pc.Inventory.consumeItem(40055, 1);
                        pc.Inventory.consumeItem(41347, 1);
                        pc.Inventory.consumeItem(41350, 1);
                        htmlid = "robinhood12";
                        pc.Inventory.storeItem(205, 1);
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, L1Quest.QUEST_END);
                    }
                }
                else if (s == "C")
                {
                    if (pc.Quest.get_step(L1Quest.QUEST_MOONOFLONGBOW) == 7)
                    {
                        if (pc.Inventory.checkItem(41352, 4) && pc.Inventory.checkItem(40618, 30) && pc.Inventory.checkItem(40643, 30) && pc.Inventory.checkItem(40645, 30) && pc.Inventory.checkItem(40651, 30) && pc.Inventory.checkItem(40676, 30) && pc.Inventory.checkItem(40514, 20) && pc.Inventory.checkItem(41351, 1) && pc.Inventory.checkItem(41346, 1))
                        {
                            pc.Inventory.consumeItem(41352, 4);
                            pc.Inventory.consumeItem(40618, 30);
                            pc.Inventory.consumeItem(40643, 30);
                            pc.Inventory.consumeItem(40645, 30);
                            pc.Inventory.consumeItem(40651, 30);
                            pc.Inventory.consumeItem(40676, 30);
                            pc.Inventory.consumeItem(40514, 20);
                            pc.Inventory.consumeItem(41351, 1);
                            pc.Inventory.consumeItem(41346, 1);
                            pc.Inventory.storeItem(41347, 1);
                            pc.Inventory.storeItem(41350, 1);
                            htmlid = "robinhood10";
                            pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 8);
                        }
                    }
                }
                else if (s == "B")
                {
                    if (pc.Inventory.checkItem(41348) && pc.Inventory.checkItem(41346))
                    {
                        htmlid = "robinhood13";
                    }
                    else
                    {
                        pc.Inventory.storeItem(41348, 1);
                        pc.Inventory.storeItem(41346, 1);
                        htmlid = "robinhood13";
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 2);
                    }
                }
                else if (s == "A")
                {
                    if (pc.Inventory.checkItem(40028))
                    {
                        pc.Inventory.consumeItem(40028, 1);
                        htmlid = "robinhood4";
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 1);
                    }
                    else
                    {
                        htmlid = "robinhood19";
                    }
                }
            }
            // ジブリル
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71257)
            {
                if (s == "D")
                {
                    if (pc.Inventory.checkItem(41349))
                    {
                        htmlid = "zybril10";
                        pc.Inventory.storeItem(41351, 1);
                        pc.Inventory.consumeItem(41349, 1);
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 7);
                    }
                    else
                    {
                        htmlid = "zybril14";
                    }
                }
                else if (s == "C")
                {
                    if (pc.Inventory.checkItem(40514, 10) && pc.Inventory.checkItem(41353))
                    {
                        pc.Inventory.consumeItem(40514, 10);
                        pc.Inventory.consumeItem(41353, 1);
                        pc.Inventory.storeItem(41354, 1);
                        htmlid = "zybril9";
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 6);
                    }
                }
                else if (pc.Inventory.checkItem(41353) && pc.Inventory.checkItem(40514, 10))
                {
                    htmlid = "zybril8";
                }
                else if (s == "B")
                {
                    if (pc.Inventory.checkItem(40048, 10) && pc.Inventory.checkItem(40049, 10) && pc.Inventory.checkItem(40050, 10) && pc.Inventory.checkItem(40051, 10))
                    {
                        pc.Inventory.consumeItem(40048, 10);
                        pc.Inventory.consumeItem(40049, 10);
                        pc.Inventory.consumeItem(40050, 10);
                        pc.Inventory.consumeItem(40051, 10);
                        pc.Inventory.storeItem(41353, 1);
                        htmlid = "zybril15";
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 5);
                    }
                    else
                    {
                        htmlid = "zybril12";
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 4);
                    }
                }
                else if (s == "A")
                {
                    if (pc.Inventory.checkItem(41348) && pc.Inventory.checkItem(41346))
                    {
                        htmlid = "zybril3";
                        pc.Quest.set_step(L1Quest.QUEST_MOONOFLONGBOW, 3);
                    }
                    else
                    {
                        htmlid = "zybril11";
                    }
                }
            }
            // マルバ
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71258)
            {
                if (pc.Inventory.checkItem(40665))
                {
                    htmlid = "marba17";
                    if (s == "B")
                    {
                        htmlid = "marba7";
                        if (pc.Inventory.checkItem(214) && pc.Inventory.checkItem(20389) && pc.Inventory.checkItem(20393) && pc.Inventory.checkItem(20401) && pc.Inventory.checkItem(20406) && pc.Inventory.checkItem(20409))
                        {
                            htmlid = "marba15";
                        }
                    }
                }
                else if (s == "A")
                {
                    if (pc.Inventory.checkItem(40637))
                    {
                        htmlid = "marba20";
                    }
                    else
                    {
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        L1ItemInstance item = pc.Inventory.storeItem(40637, 1);
                        string npcName = npc.NpcTemplate.get_name();
                        string itemName = item.Item.Name;
                        pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                        htmlid = "marba6";
                    }
                }
            }
            // アラス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 71259)
            {
                if (pc.Inventory.checkItem(40665))
                {
                    htmlid = "aras8";
                }
                else if (pc.Inventory.checkItem(40637))
                {
                    htmlid = "aras1";
                    if (s == "A")
                    {
                        if (pc.Inventory.checkItem(40664))
                        {
                            htmlid = "aras6";
                            if (pc.Inventory.checkItem(40679) || pc.Inventory.checkItem(40680) || pc.Inventory.checkItem(40681) || pc.Inventory.checkItem(40682) || pc.Inventory.checkItem(40683) || pc.Inventory.checkItem(40684) || pc.Inventory.checkItem(40693) || pc.Inventory.checkItem(40694) || pc.Inventory.checkItem(40695) || pc.Inventory.checkItem(40697) || pc.Inventory.checkItem(40698) || pc.Inventory.checkItem(40699))
                            {
                                htmlid = "aras3";
                            }
                            else
                            {
                                htmlid = "aras6";
                            }
                        }
                        else
                        {
                            L1NpcInstance npc = (L1NpcInstance)obj;
                            L1ItemInstance item = pc.Inventory.storeItem(40664, 1);
                            string npcName = npc.NpcTemplate.get_name();
                            string itemName = item.Item.Name;
                            pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                            htmlid = "aras6";
                        }
                    }
                    else if (s == "B")
                    {
                        if (pc.Inventory.checkItem(40664))
                        {
                            pc.Inventory.consumeItem(40664, 1);
                            L1NpcInstance npc = (L1NpcInstance)obj;
                            L1ItemInstance item = pc.Inventory.storeItem(40665, 1);
                            string npcName = npc.NpcTemplate.get_name();
                            string itemName = item.Item.Name;
                            pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                            htmlid = "aras13";
                        }
                        else
                        {
                            htmlid = "aras14";
                            L1NpcInstance npc = (L1NpcInstance)obj;
                            L1ItemInstance item = pc.Inventory.storeItem(40665, 1);
                            string npcName = npc.NpcTemplate.get_name();
                            string itemName = item.Item.Name;
                            pc.sendPackets(new S_ServerMessage(143, npcName, itemName));
                        }
                    }
                    else
                    {
                        if (s == "7")
                        {
                            if (pc.Inventory.checkItem(40693) && pc.Inventory.checkItem(40694) && pc.Inventory.checkItem(40695) && pc.Inventory.checkItem(40697) && pc.Inventory.checkItem(40698) && pc.Inventory.checkItem(40699))
                            {
                                htmlid = "aras10";
                            }
                            else
                            {
                                htmlid = "aras9";
                            }
                        }
                    }
                }
                else
                {
                    htmlid = "aras7";
                }
            }
            // 治安団長ラルソン
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80099)
            {
                if (s == "A")
                {
                    if (pc.Inventory.checkItem(L1ItemId.ADENA, 300))
                    {
                        pc.Inventory.consumeItem(L1ItemId.ADENA, 300);
                        pc.Inventory.storeItem(41315, 1);
                        pc.Quest.set_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT, 1);
                        htmlid = "rarson16";
                    }
                    else if (!pc.Inventory.checkItem(L1ItemId.ADENA, 300))
                    {
                        htmlid = "rarson7";
                    }
                }
                else if (s == "B")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) == 1) && (pc.Inventory.checkItem(41325, 1)))
                    {
                        pc.Inventory.consumeItem(41325, 1);
                        pc.Inventory.storeItem(L1ItemId.ADENA, 2000);
                        pc.Inventory.storeItem(41317, 1);
                        pc.Quest.set_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT, 2);
                        htmlid = "rarson9";
                    }
                    else
                    {
                        htmlid = "rarson10";
                    }
                }
                else if (s == "C")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) == 4) && (pc.Inventory.checkItem(41326, 1)))
                    {
                        pc.Inventory.storeItem(L1ItemId.ADENA, 30000);
                        pc.Inventory.consumeItem(41326, 1);
                        htmlid = "rarson12";
                        pc.Quest.set_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT, 5);
                    }
                    else
                    {
                        htmlid = "rarson17";
                    }
                }
                else if (s == "D")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) <= 1) || (pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) == 5))
                    {
                        if (pc.Inventory.checkItem(L1ItemId.ADENA, 300))
                        {
                            pc.Inventory.consumeItem(L1ItemId.ADENA, 300);
                            pc.Inventory.storeItem(41315, 1);
                            pc.Quest.set_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT, 1);
                            htmlid = "rarson16";
                        }
                        else if (!pc.Inventory.checkItem(L1ItemId.ADENA, 300))
                        {
                            htmlid = "rarson7";
                        }
                    }
                    else if ((pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) >= 2) && (pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) <= 4))
                    {
                        if (pc.Inventory.checkItem(L1ItemId.ADENA, 300))
                        {
                            pc.Inventory.consumeItem(L1ItemId.ADENA, 300);
                            pc.Inventory.storeItem(41315, 1);
                            htmlid = "rarson16";
                        }
                        else if (!pc.Inventory.checkItem(L1ItemId.ADENA, 300))
                        {
                            htmlid = "rarson7";
                        }
                    }
                }
            }
            // クエン
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80101)
            {
                if (s == "request letter of kuen")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) == 2) && (pc.Inventory.checkItem(41317, 1)))
                    {
                        pc.Inventory.consumeItem(41317, 1);
                        pc.Inventory.storeItem(41318, 1);
                        pc.Quest.set_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT, 3);
                        htmlid = "";
                    }
                    else
                    {
                        htmlid = "";
                    }
                }
                else if (s == "request holy mithril dust")
                {
                    if ((pc.Quest.get_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT) == 3) && (pc.Inventory.checkItem(41315, 1)) && pc.Inventory.checkItem(40494, 30) && pc.Inventory.checkItem(41318, 1))
                    {
                        pc.Inventory.consumeItem(41315, 1);
                        pc.Inventory.consumeItem(41318, 1);
                        pc.Inventory.consumeItem(40494, 30);
                        pc.Inventory.storeItem(41316, 1);
                        pc.Quest.set_step(L1Quest.QUEST_GENERALHAMELOFRESENTMENT, 4);
                        htmlid = "";
                    }
                    else
                    {
                        htmlid = "";
                    }
                }
            }

            // 長老 普洛凱爾
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80136)
            {
                int lv15_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL15);
                int lv30_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL30);
                int lv45_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL45);
                int lv50_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL50);
                if (pc.DragonKnight)
                {
                    // 執行普洛凱爾的課題
                    if (s == "a" && (lv15_step == 0))
                    {
                        L1NpcInstance npc = (L1NpcInstance)obj;
                        L1ItemInstance item = pc.Inventory.storeItem(49210, 1); // 普洛凱爾的第一次指令書
                        string npcName = npc.NpcTemplate.get_name();
                        string itemName = item.Item.Name;
                        pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL15, 1);
                        htmlid = "prokel3";
                        // 執行普洛凱爾的第二次課題
                    }
                    else if (s == "c" && (lv30_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49211, 49215};
                        int[] item_ids = new int[] { 49211, 49215 }; // 普洛凱爾的第二次指令書、普洛凱爾的礦物袋
                                                                     //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                                     //ORIGINAL LINE: final int[] item_amounts = { 1, 1};
                        int[] item_amounts = new int[] { 1, 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL30, 1);
                        htmlid = "prokel9";
                        // 需要普洛凱爾的礦物袋
                    }
                    else if (s == "e")
                    {
                        if (pc.Inventory.checkItem(49215, 1))
                        {
                            htmlid = "prokel35";
                        }
                        else
                        {
                            L1NpcInstance npc = (L1NpcInstance)obj;
                            L1ItemInstance item = pc.Inventory.storeItem(49215, 1); // 普洛凱爾的礦物袋
                            string npcName = npc.NpcTemplate.get_name();
                            string itemName = item.Item.Name;
                            pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                            htmlid = "prokel13";
                        }
                        // 執行普洛凱爾的第三次課題
                    }
                    else if (s == "f" && (lv45_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49209, 49212, 49226};
                        int[] item_ids = new int[] { 49209, 49212, 49226 }; // 長老普洛凱爾的信件、普洛凱爾的第三次指令書、結盟瞬間移動卷軸
                                                                            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                                            //ORIGINAL LINE: final int[] item_amounts = { 1, 1, 1};
                        int[] item_amounts = new int[] { 1, 1, 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL45, 1);
                        htmlid = "prokel16";
                        // 執行普洛凱爾的第四次課題
                    }
                    else if (s == "h" && (lv50_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49287};
                        int[] item_ids = new int[] { 49287 }; // 普洛凱爾的第四次指令書
                                                              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                              //ORIGINAL LINE: final int[] item_amounts = { 1};
                        int[] item_amounts = new int[] { 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 1);
                        htmlid = "prokel22";
                        // 重新接收時空裂痕邪念碎片、普洛凱爾的護身符
                    }
                    else if (s == "k" && (lv50_step >= 2))
                    {
                        if (pc.Inventory.checkItem(49202, 1) || pc.Inventory.checkItem(49216, 1))
                        {
                            htmlid = "prokel29";
                        }
                        else
                        {
                            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                            //ORIGINAL LINE: final int[] item_ids = { 49202, 49216};
                            int[] item_ids = new int[] { 49202, 49216 };
                            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                            //ORIGINAL LINE: final int[] item_amounts = { 1, 1};
                            int[] item_amounts = new int[] { 1, 1 };
                            for (int i = 0; i < item_ids.Length; i++)
                            {
                                L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                            }
                            htmlid = "prokel28";
                        }
                    }
                }
            }

            /*
			 * // 長老 シルレイン else if (((L1NpcInstance)
			 * obj).getNpcTemplate().get_npcId() == 80145) {// 併到 幻術士 試煉 if
			 * (pc.isDragonKnight()) { int lv45_step =
			 * pc.getQuest().get_step(L1Quest.QUEST_LEVEL45); // 「プロケルの手紙を渡す」 if
			 * (s.equalsIgnoreCase("l") && (lv45_step == 1)) { if
			 * (pc.getInventory().checkItem(49209, 1)) { // check
			 * pc.getInventory().consumeItem(49209, 1); // del
			 * pc.getQuest().set_step(L1Quest.QUEST_LEVEL45, 2); htmlid =
			 * "silrein38"; } } else if (s.equalsIgnoreCase("m") && (lv45_step ==
			 * 2)) { pc.getQuest().set_step(L1Quest.QUEST_LEVEL45, 3); htmlid =
			 * "silrein39"; } } }
			 */

            // エルラス
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80135)
            {
                if (pc.DragonKnight)
                {
                    // 「オーク密使変身スクロールを受け取る」
                    if (s == "a")
                    {
                        if (pc.Inventory.checkItem(49220, 1))
                        {
                            htmlid = "elas5";
                        }
                        else
                        {
                            L1NpcInstance npc = (L1NpcInstance)obj;
                            L1ItemInstance item = pc.Inventory.storeItem(49220, 1); // オーク密使変身スクロール
                            string npcName = npc.NpcTemplate.get_name();
                            string itemName = item.Item.Name;
                            pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                            htmlid = "elas4";
                        }
                    }
                }
            }

            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81245)
            { // オーク密使(HC3)
                if (pc.DragonKnight)
                {
                    if (s == "request flute of spy")
                    {
                        if (pc.Inventory.checkItem(49223, 1))
                        { // check
                            pc.Inventory.consumeItem(49223, 1); // del
                            L1NpcInstance npc = (L1NpcInstance)obj;
                            L1ItemInstance item = pc.Inventory.storeItem(49222, 1); // オーク密使の笛
                            string npcName = npc.NpcTemplate.get_name();
                            string itemName = item.Item.Name;
                            pc.sendPackets(new S_ServerMessage(143, npcName, itemName)); // \f1%0が%1をくれました。
                            htmlid = "";
                        }
                        else
                        {
                            htmlid = "";
                        }
                    }
                }
            }

            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81246)
            { // シャルナ
                if (s == "0")
                {
                    materials = new int[] { L1ItemId.ADENA };
                    counts = new int[] { 2500 };
                    if (pc.Level < 30)
                    {
                        htmlid = "sharna4";
                    }
                    else if ((pc.Level >= 30) && (pc.Level <= 39))
                    {
                        createitem = new int[] { 49149 }; // シャルナの変身スクロール（レベル30）
                        createcount = new int[] { 1 };
                    }
                    else if ((pc.Level >= 40) && (pc.Level <= 51))
                    {
                        createitem = new int[] { 49150 }; // シャルナの変身スクロール（レベル40）
                        createcount = new int[] { 1 };
                    }
                    else if ((pc.Level >= 52) && (pc.Level <= 54))
                    {
                        createitem = new int[] { 49151 }; // シャルナの変身スクロール（レベル52）
                        createcount = new int[] { 1 };
                    }
                    else if ((pc.Level >= 55) && (pc.Level <= 59))
                    {
                        createitem = new int[] { 49152 }; // シャルナの変身スクロール（レベル55）
                        createcount = new int[] { 1 };
                    }
                    else if ((pc.Level >= 60) && (pc.Level <= 64))
                    {
                        createitem = new int[] { 49153 }; // シャルナの変身スクロール（レベル60）
                        createcount = new int[] { 1 };
                    }
                    else if ((pc.Level >= 65) && (pc.Level <= 69))
                    {
                        createitem = new int[] { 49154 }; // シャルナの変身スクロール（レベル65）
                        createcount = new int[] { 1 };
                    }
                    else if (pc.Level >= 70)
                    {
                        createitem = new int[] { 49155 }; // シャルナの変身スクロール（レベル70）
                        createcount = new int[] { 1 };
                    }
                    success_htmlid = "sharna3";
                    failure_htmlid = "sharna5";
                }
            }
            else if ((((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70035) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70041) || (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70042))
            { // ギランレース管理人　セシル　ポーリー　パーキン
                if (s == "status")
                { // status
                    htmldata = new string[15];
                    for (int i = 0; i < 5; i++)
                    {
                        htmldata[i * 3] = (Container.Instance.Resolve<INpcController>().getTemplate(LineageServer.Server.Model.Game.L1BugBearRace.Instance.getRunner(i).NpcId).get_nameid());
                        string condition; // 610 普通
                        if (LineageServer.Server.Model.Game.L1BugBearRace.Instance.getCondition(i) == 0)
                        {
                            condition = "$610";
                        }
                        else
                        {
                            if (LineageServer.Server.Model.Game.L1BugBearRace.Instance.getCondition(i) > 0)
                            { // 368
                              // 良い
                                condition = "$368";
                            }
                            else
                            { // 370 悪い
                                condition = "$370";
                            }
                        }
                        htmldata[i * 3 + 1] = condition;
                        htmldata[i * 3 + 2] = (LineageServer.Server.Model.Game.L1BugBearRace.Instance.getWinningAverage(i)).ToString();
                    }
                    htmlid = "maeno4";
                }
            }
            // 然柳寵物商
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70077 || ((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81290)
            { // 班酷
                int consumeItem = 0;
                int consumeItemCount = 0;
                int petNpcId = 0;
                int petItemId = 0; // 40314 低等寵物項圈
                int upLv = 0; // 等級
                long lvExp = 0; // LV.upLv 經驗值
                string msg = "";
                if (s == "buy 1")
                {
                    petNpcId = 45042; // 杜賓狗
                    consumeItem = L1ItemId.ADENA;
                    consumeItemCount = 50000;
                    petItemId = 40314;
                    upLv = 5;
                    lvExp = ExpTable.getExpByLevel(upLv);
                    msg = "金幣";
                }
                else if (s == "buy 2")
                {
                    petNpcId = 45034; // 牧羊犬
                    consumeItem = L1ItemId.ADENA;
                    consumeItemCount = 50000;
                    petItemId = 40314;
                    upLv = 5;
                    lvExp = ExpTable.getExpByLevel(upLv);
                    msg = "金幣";
                }
                else if (s == "buy 3")
                {
                    petNpcId = 45046; // 小獵犬
                    consumeItem = L1ItemId.ADENA;
                    consumeItemCount = 50000;
                    petItemId = 40314;
                    upLv = 5;
                    lvExp = ExpTable.getExpByLevel(upLv);
                    msg = "金幣";
                }
                else if (s == "buy 4")
                {
                    petNpcId = 45047; // 聖伯納犬
                    consumeItem = L1ItemId.ADENA;
                    consumeItemCount = 50000;
                    petItemId = 40314;
                    upLv = 5;
                    lvExp = ExpTable.getExpByLevel(upLv);
                    msg = "金幣";
                }
                else if (s == "buy 7")
                {
                    petNpcId = 97023; // 淘氣龍
                    consumeItem = 47011;
                    consumeItemCount = 1;
                    petItemId = 40314;
                    upLv = 5;
                    lvExp = ExpTable.getExpByLevel(upLv);
                    msg = "淘氣幼龍蛋";
                }
                else if (s == "buy 8")
                {
                    petNpcId = 97022; // 頑皮龍
                    consumeItem = 47012;
                    consumeItemCount = 1;
                    petItemId = 40314;
                    upLv = 5;
                    lvExp = ExpTable.getExpByLevel(upLv);
                    msg = "頑皮幼龍蛋";
                }
                if (petNpcId > 0)
                {
                    if (!pc.Inventory.checkItem(consumeItem, consumeItemCount))
                    {
                        pc.sendPackets(new S_ServerMessage(337, msg));
                    }
                    else if (pc.Inventory.Size > 180)
                    {
                        pc.sendPackets(new S_ServerMessage(337, "身上空間"));
                    }
                    else if (pc.Inventory.checkItem(consumeItem, consumeItemCount))
                    {
                        pc.Inventory.consumeItem(consumeItem, consumeItemCount);
                        L1ItemInstance petamu = pc.Inventory.storeItem(petItemId, 1);
                        if (petamu != null)
                        {
                            PetTable.Instance.buyNewPet(petNpcId, petamu.Id + 1, petamu.Id, upLv, lvExp);
                            pc.sendPackets(new S_ItemName(petamu));
                            pc.sendPackets(new S_ServerMessage(403, petamu.Name));
                        }
                    }
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage("對話檔版本不符，請下載更新"));
                }
                htmlid = "";
            }

            // 幻術士 試練任務
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 80145)
            { // 長老 希蓮恩
                int lv15_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL15);
                int lv30_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL30);
                int lv45_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL45);
                int lv50_step = pc.Quest.get_step(L1Quest.QUEST_LEVEL50);
                if (pc.DragonKnight)
                {
                    if (s == "l" && (lv45_step == 1))
                    {
                        if (pc.Inventory.checkItem(49209, 1))
                        { // check
                            pc.Inventory.consumeItem(49209, 1); // del
                            pc.Quest.set_step(L1Quest.QUEST_LEVEL45, 2);
                            htmlid = "silrein38";
                        }
                    }
                    else if (s == "m" && (lv45_step == 2))
                    {
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL45, 3);
                        htmlid = "silrein39";
                    }
                }
                if (pc.Illusionist)
                {
                    // 希蓮恩的第一次課題
                    if (s == "a" && (lv15_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49172, 49182};
                        int[] item_ids = new int[] { 49172, 49182 }; // 希蓮恩的第一次信件、妖精森林瞬間移動卷軸
                                                                     //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                                     //ORIGINAL LINE: final int[] item_amounts = { 1, 1};
                        int[] item_amounts = new int[] { 1, 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL15, 1);
                        htmlid = "silrein3";
                        // 執行希蓮恩的第二課題
                    }
                    else if (s == "c" && (lv30_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49173, 49179};
                        int[] item_ids = new int[] { 49173, 49179 }; // 希蓮恩的第二次信件、希蓮恩之袋
                                                                     // 獲得【歐瑞村莊瞬間移動卷軸、生鏽的笛子】
                                                                     //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                                     //ORIGINAL LINE: final int[] item_amounts = { 1, 1};
                        int[] item_amounts = new int[] { 1, 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL30, 1);
                        htmlid = "silrein12";
                        // 重新接收生鏽的笛子
                    }
                    else if (s == "o" && (lv30_step == 1))
                    {
                        if (pc.Inventory.checkItem(49186, 1) || pc.Inventory.checkItem(49179, 1))
                        {
                            htmlid = "silrein17"; // 已經有 希蓮恩之袋、生鏽的笛子 不可再取得
                        }
                        else
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(49186, 1); // 生鏽的笛子
                            pc.sendPackets(new S_ServerMessage(143, item.Item.Name));
                            htmlid = "silrein16";
                        }
                        // 執行希蓮恩的第三課題
                    }
                    else if (s == "e" && (lv45_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49174, 49180};
                        int[] item_ids = new int[] { 49174, 49180 }; // 希蓮恩的第三次信件、希蓮恩之袋
                                                                     // 獲得【風木村莊瞬間移動卷軸、時空裂痕水晶(綠色
                                                                     // 3個)】
                                                                     //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                                     //ORIGINAL LINE: final int[] item_amounts = { 1, 1};
                        int[] item_amounts = new int[] { 1, 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL45, 1);
                        htmlid = "silrein19";
                        // 執行希蓮恩的第四課題
                    }
                    else if (s == "h" && (lv50_step == 0))
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49176};
                        int[] item_ids = new int[] { 49176 }; // 希蓮恩的第五次信件
                                                              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                                                              //ORIGINAL LINE: final int[] item_amounts = { 1};
                        int[] item_amounts = new int[] { 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 1);
                        htmlid = "silrein28";
                        // 重新接收時空裂痕邪念碎片、希蓮恩的護身符
                    }
                    else if (s == "k" && (lv50_step >= 2))
                    {
                        if (pc.Inventory.checkItem(49202, 1) || pc.Inventory.checkItem(49178, 1))
                        {
                            htmlid = "silrein32";
                        }
                        else
                        {
                            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                            //ORIGINAL LINE: final int[] item_ids = { 49202, 49178};
                            int[] item_ids = new int[] { 49202, 49178 };
                            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                            //ORIGINAL LINE: final int[] item_amounts = { 1, 1};
                            int[] item_amounts = new int[] { 1, 1 };
                            for (int i = 0; i < item_ids.Length; i++)
                            {
                                L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                                pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                            }
                            htmlid = "silrein32";
                        }
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 70739)
            { // 迪嘉勒廷
                if (pc.Crown)
                {
                    if (s == "e")
                    {
                        if (pc.Inventory.checkItem(49159, 1))
                        {
                            htmlid = "dicardingp5";
                            pc.Inventory.consumeItem(49159, 1);
                            pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 2);
                        }
                        else
                        {
                            htmlid = "dicardingp4a";
                        }
                    }
                    else if (s == "d")
                    {
                        htmlid = "dicardingp7";
                        L1PolyMorph.doPoly(pc, 6035, 900, 1, true);
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 3);
                    }
                    else if (s == "c")
                    {
                        htmlid = "dicardingp9";
                        L1PolyMorph.undoPoly(pc);
                        L1PolyMorph.doPoly(pc, 6035, 900, 1, true);
                    }
                    else if (s == "b")
                    {
                        htmlid = "dicardingp12";
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 4);
                        if (pc.Inventory.checkItem(49165))
                        {
                            pc.Inventory.consumeItem(49165, pc.Inventory.countItems(49165));
                        }
                        if (pc.Inventory.checkItem(49166))
                        {
                            pc.Inventory.consumeItem(49166, pc.Inventory.countItems(49166));
                        }
                        if (pc.Inventory.checkItem(49167))
                        {
                            pc.Inventory.consumeItem(49167, pc.Inventory.countItems(49167));
                        }
                        if (pc.Inventory.checkItem(49168))
                        {
                            pc.Inventory.consumeItem(49168, pc.Inventory.countItems(49168));
                        }
                        if (pc.Inventory.checkItem(49239))
                        {
                            pc.Inventory.consumeItem(49239, pc.Inventory.countItems(49239));
                        }
                    }
                }
                if (pc.Knight)
                {
                    if (s == "h")
                    {
                        if (pc.Inventory.checkItem(49160, 1))
                        {
                            htmlid = "dicardingk5";
                            pc.Inventory.consumeItem(49160, 1);
                            pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 2);
                        }
                    }
                    else if (s == "j")
                    {
                        htmlid = "dicardingk10";
                        pc.Inventory.consumeItem(49161, 10);
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 4);
                    }
                    else if (s == "k")
                    {
                        htmlid = "dicardingk13";
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 4);
                        if (pc.Inventory.checkItem(49165))
                        {
                            pc.Inventory.consumeItem(49165, pc.Inventory.countItems(49165));
                        }
                        if (pc.Inventory.checkItem(49166))
                        {
                            pc.Inventory.consumeItem(49166, pc.Inventory.countItems(49166));
                        }
                        if (pc.Inventory.checkItem(49167))
                        {
                            pc.Inventory.consumeItem(49167, pc.Inventory.countItems(49167));
                        }
                        if (pc.Inventory.checkItem(49168))
                        {
                            pc.Inventory.consumeItem(49168, pc.Inventory.countItems(49168));
                        }
                        if (pc.Inventory.checkItem(49239))
                        {
                            pc.Inventory.consumeItem(49239, pc.Inventory.countItems(49239));
                        }
                    }
                }
                if (pc.Elf)
                {
                    if (s == "n")
                    {
                        if (pc.Inventory.checkItem(49162, 1))
                        {
                            htmlid = "dicardinge5";
                            pc.Inventory.consumeItem(49162, 1);
                            pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 2);
                        }
                    }
                    else if (s == "p")
                    {
                        htmlid = "dicardinge10";
                        pc.Inventory.consumeItem(49163, 1);
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 5);
                    }
                    else if (s == "q")
                    {
                        htmlid = "dicardinge14";
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 5);
                        if (pc.Inventory.checkItem(49165))
                        {
                            pc.Inventory.consumeItem(49165, pc.Inventory.countItems(49165));
                        }
                        if (pc.Inventory.checkItem(49166))
                        {
                            pc.Inventory.consumeItem(49166, pc.Inventory.countItems(49166));
                        }
                        if (pc.Inventory.checkItem(49167))
                        {
                            pc.Inventory.consumeItem(49167, pc.Inventory.countItems(49167));
                        }
                        if (pc.Inventory.checkItem(49168))
                        {
                            pc.Inventory.consumeItem(49168, pc.Inventory.countItems(49168));
                        }
                        if (pc.Inventory.checkItem(49239))
                        {
                            pc.Inventory.consumeItem(49239, pc.Inventory.countItems(49239));
                        }
                    }
                }
                if (pc.Wizard)
                {
                    if (s == "u")
                    {
                        if (pc.Inventory.checkItem(49164, 1))
                        {
                            htmlid = "dicardingw6";
                            pc.Inventory.consumeItem(49164, 1);
                            pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 3);
                        }
                    }
                    else if (s == "w")
                    {
                        htmlid = "dicardingw12";
                        pc.Quest.set_step(L1Quest.QUEST_LEVEL50, 4);
                        if (pc.Inventory.checkItem(49165))
                        {
                            pc.Inventory.consumeItem(49165, pc.Inventory.countItems(49165));
                        }
                        if (pc.Inventory.checkItem(49166))
                        {
                            pc.Inventory.consumeItem(49166, pc.Inventory.countItems(49166));
                        }
                        if (pc.Inventory.checkItem(49167))
                        {
                            pc.Inventory.consumeItem(49167, pc.Inventory.countItems(49167));
                        }
                        if (pc.Inventory.checkItem(49168))
                        {
                            pc.Inventory.consumeItem(49168, pc.Inventory.countItems(49168));
                        }
                        if (pc.Inventory.checkItem(49239))
                        {
                            pc.Inventory.consumeItem(49239, pc.Inventory.countItems(49239));
                        }
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81334)
            { // 被遺棄的肉身
                if (s == "a")
                {
                    if (pc.Inventory.checkItem(49239, 1))
                    {
                        htmlid = "rtf06";
                    }
                    else
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_ids = { 49239};
                        int[] item_ids = new int[] { 49239 };
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final int[] item_amounts = { 1};
                        int[] item_amounts = new int[] { 1 };
                        for (int i = 0; i < item_ids.Length; i++)
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(item_ids[i], item_amounts[i]);
                            pc.sendPackets(new S_ServerMessage(143, ((L1NpcInstance)obj).NpcTemplate.get_name(), item.Item.Name));
                        }
                    }
                }
            }
            else if ((((L1NpcInstance)obj).NpcTemplate.get_npcId() >= 81353) && (((L1NpcInstance)obj).NpcTemplate.get_npcId() <= 81363))
            { // 魔法商人- 仿正設定
                int[] skills = new int[10];
                char s1 = s[0];
                switch (s1)
                {
                    case 'b':
                        skills = new int[] { 43, 79, 151, 158, 160, 206, 211, 216, 115, 149 };
                        break;
                    case 'a':
                        skills = new int[] { 43, 79, 151, 158, 160, 206, 211, 216, 115, 148 };
                        break;
                }
                if (s == "a" || s == "b")
                {
                    if (pc.Inventory.consumeItem(L1ItemId.ADENA, 3000))
                    {
                        L1SkillUse l1skilluse = new L1SkillUse();
                        for (int i = 0; i < skills.Length; i++)
                        {
                            l1skilluse.handleCommands(pc, skills[i], pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_GMBUFF);
                        }
                        htmlid = "bs_done";
                    }
                    else
                    {
                        htmlid = "bs_adena";
                    }
                }
                if (s == "0")
                {
                    htmlid = "bs_01";
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 50016)
            { // 傑諾
                if (s == "0")
                {
                    if (pc.Level < 13)
                    { // lv < 13 傳送隱藏之谷
                        L1Teleport.teleport(pc, 32682, 32874, (short)2005, 2, true);
                    }
                    else
                    {
                        htmlid = "zeno1";
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 50065)
            { // 魯比恩
                if (s == "teleport valley-in")
                {
                    if (pc.Level < 13)
                    { // lv < 13 傳送隱藏之谷
                        L1Teleport.teleport(pc, 32682, 32874, (short)2005, 2, true);
                    }
                    else
                    {
                        htmlid = "";
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 50055)
            { // 德瑞斯特
                if (s == "teleport hidden-valley")
                {
                    if (pc.Level < 13)
                    { // lv < 13 傳送隱藏之谷
                        L1Teleport.teleport(pc, 32682, 32874, (short)2005, 2, true);
                    }
                    else
                    {
                        htmlid = "drist1";
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81255)
            { // 新手導師
              //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
              //ORIGINAL LINE: @SuppressWarnings("unused") int quest_step = pc.getQuest().get_step(l1j.server.server.model.L1Quest.QUEST_TUTOR);
                int quest_step = pc.Quest.get_step(L1Quest.QUEST_TUTOR); // 任務編號階段
                int level = pc.Level; // 角色等級
                char s1 = s[0];
                if (level < 13)
                {
                    switch (s1)
                    {
                        case 'A':
                        case 'a': // isCrown
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutorp1"; // 指引
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutorp2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutorp3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 12))
                            { // lv10 ~ lv11
                                htmlid = "tutorp4"; // 傳送服務
                            }
                            else if ((level > 11) && (level < 13))
                            { // lv12
                                htmlid = "tutorp5"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutorp6"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'B':
                        case 'b': // isKnight
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutork1"; // 接受幫助
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutork2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutork3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 13))
                            { // lv10 ~ lv12
                                htmlid = "tutork4"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutork5"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'C':
                        case 'c': // isElf
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutore1"; // 接受幫助
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutore2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutore3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 12))
                            { // lv10 ~ lv11
                                htmlid = "tutore4"; // 傳送服務
                            }
                            else if ((level > 11) && (level < 13))
                            { // lv12
                                htmlid = "tutore5"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutore6"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'D':
                        case 'd': // isWizard
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutorm1"; // 接受幫助
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutorm2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutorm3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 12))
                            { // lv10 ~ lv11
                                htmlid = "tutorm4"; // 傳送服務
                            }
                            else if ((level > 11) && (level < 13))
                            { // lv12
                                htmlid = "tutorm5"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutorm6"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'E':
                        case 'e': // isDarkelf
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutord1"; // 接受幫助
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutord2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutord3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 12))
                            { // lv10 ~ lv11
                                htmlid = "tutord4"; // 傳送服務
                            }
                            else if ((level > 11) && (level < 13))
                            { // lv12
                                htmlid = "tutord5"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutord6"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'F':
                        case 'f': // isDragonKnight
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutordk1"; // 接受幫助
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutordk2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutordk3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 13))
                            { // lv10 ~ lv12
                                htmlid = "tutordk4"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutordk5"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'G':
                        case 'g': // isIllusionist
                            if ((level > 1) && (level < 5))
                            { // lv2 ~ lv4
                                htmlid = "tutori1"; // 接受幫助
                            }
                            else if ((level > 4) && (level < 8))
                            { // lv5 ~ lv7
                                htmlid = "tutori2"; // 傳送服務
                            }
                            else if ((level > 7) && (level < 10))
                            { // lv8 ~ lv9
                                htmlid = "tutori3"; // 傳送服務
                            }
                            else if ((level > 9) && (level < 13))
                            { // lv10 ~ lv12
                                htmlid = "tutori4"; // 傳送服務
                            }
                            else if (level > 12)
                            { // lv13
                                htmlid = "tutori5"; // 離開隱藏之谷
                            }
                            else
                            {
                                htmlid = "tutorend";
                            }
                            break;
                        case 'H':
                        case 'h':
                            L1Teleport.teleport(pc, 32575, 32945, (short)0, 5, true); // 說話之島倉庫管理員
                            htmlid = "";
                            break;
                        case 'I':
                        case 'i':
                            L1Teleport.teleport(pc, 32579, 32923, (short)0, 5, true); // 血盟執行人
                            htmlid = "";
                            break;
                        case 'J':
                        case 'j':
                            createitem = new int[] { 42099 };
                            createcount = new int[] { 1 };
                            L1Teleport.teleport(pc, 32676, 32813, (short)2005, 5, true); // 隱藏之谷地下洞穴
                            htmlid = "";
                            break;
                        case 'K':
                        case 'k':
                            L1Teleport.teleport(pc, 32562, 33082, (short)0, 5, true); // 魔法師吉倫小屋
                            htmlid = "";
                            break;
                        case 'L':
                        case 'l':
                            L1Teleport.teleport(pc, 32792, 32820, (short)75, 5, true); // 象牙塔
                            htmlid = "";
                            break;
                        case 'M':
                        case 'm':
                            L1Teleport.teleport(pc, 32877, 32904, (short)304, 5, true); // 黑暗魔法師賽帝亞
                            htmlid = "";
                            break;
                        case 'N':
                        case 'n':
                            L1Teleport.teleport(pc, 32759, 32884, (short)1000, 5, true); // 幻術士史菲爾
                            htmlid = "";
                            break;
                        case 'O':
                        case 'o':
                            L1Teleport.teleport(pc, 32605, 32837, (short)2005, 5, true); // 村莊西郊
                            htmlid = "";
                            break;
                        case 'P':
                        case 'p':
                            L1Teleport.teleport(pc, 32733, 32902, (short)2005, 5, true); // 村莊東郊
                            htmlid = "";
                            break;
                        case 'Q':
                        case 'q':
                            L1Teleport.teleport(pc, 32559, 32843, (short)2005, 5, true); // 村莊南部狩獵場
                            htmlid = "";
                            break;
                        case 'R':
                        case 'r':
                            L1Teleport.teleport(pc, 32677, 32982, (short)2005, 5, true); // 村莊東南部狩獵場
                            htmlid = "";
                            break;
                        case 'S':
                        case 's':
                            L1Teleport.teleport(pc, 32781, 32854, (short)2005, 5, true); // 村莊東北部狩獵場
                            htmlid = "";
                            break;
                        case 'T':
                        case 't':
                            L1Teleport.teleport(pc, 32674, 32739, (short)2005, 5, true); // 村莊西北部狩獵場
                            htmlid = "";
                            break;
                        case 'U':
                        case 'u':
                            L1Teleport.teleport(pc, 32578, 32737, (short)2005, 5, true); // 村莊西部狩獵場
                            htmlid = "";
                            break;
                        case 'V':
                        case 'v':
                            L1Teleport.teleport(pc, 32542, 32996, (short)2005, 5, true); // 村莊南部狩獵場
                            htmlid = "";
                            break;
                        case 'W':
                        case 'w':
                            L1Teleport.teleport(pc, 32794, 32973, (short)2005, 5, true); // 村莊東部狩獵場
                            htmlid = "";
                            break;
                        case 'X':
                        case 'x':
                            L1Teleport.teleport(pc, 32803, 32789, (short)2005, 5, true); // 村莊北部狩獵場
                            htmlid = "";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81256)
            { // 修練場管理員
                int quest_step = pc.Quest.get_step(L1Quest.QUEST_TUTOR2); // 任務編號階段
                int level = pc.Level; // 角色等級           
                if (s == "A")
                {
                    if ((level > 4) && (quest_step == 2))
                    {
                        createitem = new int[] { 20028, 20126, 20173, 20206, 20232, 40029, 40030, 40098, 40099, 42099 };
                        createcount = new int[] { 1, 1, 1, 1, 1, 50, 5, 20, 30, 5 };
                        questid = L1Quest.QUEST_TUTOR2;
                        questvalue = 3;
                    }
                }
                htmlid = "";
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81257)
            { // 旅人諮詢員
                int level = pc.Level; // 角色等級
                char s1 = s[0];
                if (level < 46)
                {
                    switch (s1)
                    {
                        case 'A':
                        case 'a':
                            L1Teleport.teleport(pc, 32562, 33082, (short)0, 5, true); // 魔法師吉倫小屋
                            htmlid = "";
                            break;
                        case 'B':
                        case 'b':
                            L1Teleport.teleport(pc, 33119, 32933, (short)4, 5, true); // 正義神殿
                            htmlid = "";
                            break;
                        case 'C':
                        case 'c':
                            L1Teleport.teleport(pc, 32887, 32652, (short)4, 5, true); // 邪惡神殿
                            htmlid = "";
                            break;
                        case 'D':
                        case 'd':
                            L1Teleport.teleport(pc, 32792, 32820, (short)75, 5, true); // 販售妖精精靈魔法的琳達
                            htmlid = "";
                            break;
                        case 'E':
                        case 'e':
                            L1Teleport.teleport(pc, 32789, 32851, (short)76, 5, true); // 象牙塔的精靈魔法修煉室
                            htmlid = "";
                            break;
                        case 'F':
                        case 'f':
                            L1Teleport.teleport(pc, 32750, 32847, (short)76, 5, true); // 象牙塔的艾利溫
                            htmlid = "";
                            break;
                        case 'G':
                        case 'g':
                            if (pc.Darkelf)
                            {
                                L1Teleport.teleport(pc, 32877, 32904, (short)304, 5, true); // 黑暗魔法師賽帝亞
                                htmlid = "";
                            }
                            else
                            {
                                htmlid = "lowlv40";
                            }
                            break;
                        case 'H':
                        case 'h':
                            if (pc.DragonKnight)
                            {
                                L1Teleport.teleport(pc, 32811, 32873, (short)1001, 5, true); // 販售龍騎士技能的森帕爾處
                                htmlid = "";
                            }
                            else
                            {
                                htmlid = "lowlv41";
                            }
                            break;
                        case 'I':
                        case 'i':
                            if (pc.Illusionist)
                            {
                                L1Teleport.teleport(pc, 32759, 32884, (short)1000, 5, true); // 販售幻術士魔法的史菲爾處
                                htmlid = "";
                            }
                            else
                            {
                                htmlid = "lowlv42";
                            }
                            break;
                        case 'J':
                        case 'j':
                            L1Teleport.teleport(pc, 32509, 32867, (short)0, 5, true); // 說話之島的甘特處
                            htmlid = "";
                            break;
                        case 'K':
                        case 'k':
                            {
                                if ((level > 34))
                                {
                                    createitem = new int[] { 20282, 21139 }; // 補充象牙塔飾品
                                    createcount = new int[] { 0, 0 };
                                    bool isOK = false;
                                    for (int i = 0; i < createitem.Length; i++)
                                    {
                                        if (!pc.Inventory.checkItem(createitem[i], 1))
                                        { // check
                                            createcount[i] = 1;
                                            isOK = true;
                                        }
                                    }
                                    if (isOK)
                                    {
                                        success_htmlid = "lowlv43";
                                    }
                                    else
                                    {
                                        htmlid = "lowlv45";
                                    }
                                }
                                else
                                {
                                    htmlid = "lowlv44";
                                }
                            }
                            break;
                        case '0':
                            if (level < 13)
                            {
                                htmlid = "lowlvS1";
                            }
                            else if ((level > 12) && (level < 46))
                            {
                                htmlid = "lowlvS2";
                            }
                            else
                            {
                                htmlid = "lowlvno";
                            }
                            break;
                        case '1':
                            if (level < 13)
                            {
                                htmlid = "lowlv14";
                            }
                            else if ((level > 12) && (level < 46))
                            {
                                htmlid = "lowlv15";
                            }
                            else
                            {
                                htmlid = "lowlvno";
                            }
                            break;
                        case '2':
                            {
                                createitem = new int[] { 20028, 20126, 20173, 20206, 20232, 21138, 49310 };
                                createcount = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                                bool isOK = false;
                                for (int i = 0; i < createitem.Length; i++)
                                {
                                    if (createitem[i] == 49310)
                                    {
                                        L1ItemInstance item = pc.Inventory.findItemId(createitem[i]);
                                        if (item != null)
                                        {
                                            if (item.Count < 1000)
                                            {
                                                createcount[i] = 1000 - item.Count;
                                                isOK = true;
                                            }
                                        }
                                        else
                                        {
                                            createcount[i] = 1000;
                                            isOK = true;
                                        }
                                    }
                                    else if (!pc.Inventory.checkItem(createitem[i], 1))
                                    { // check
                                        createcount[i] = 1;
                                        isOK = true;
                                    }
                                }
                                if (isOK)
                                {
                                    success_htmlid = "lowlv16";
                                }
                                else
                                {
                                    htmlid = "lowlv17";
                                }
                            }
                            break;
                        case '6':
                            if (!pc.Inventory.checkItem(49313, 1) && !pc.Inventory.checkItem(49314, 1))
                            {
                                createitem = new int[] { 49313 }; // 象牙塔魔法袋
                                createcount = new int[] { 2 };
                                materials = new int[] { L1ItemId.ADENA };
                                counts = new int[] { 2000 };
                                success_htmlid = "lowlv22";
                                failure_htmlid = "lowlv20";
                            }
                            else if (pc.Inventory.checkItem(49313, 1) || pc.Inventory.checkItem(49314, 1))
                            {
                                htmlid = "lowlv23";
                            }
                            else
                            {
                                htmlid = "lowlvno";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81260)
            { // 村莊福利員
                int townid = pc.HomeTownId; // 角色所屬村莊
                char s1 = s[0];
                if ((pc.Level > 9) && (townid > 0) && (townid < 11))
                {
                    switch (s1)
                    {
                        case '0':
                            createitem = new int[] { 49305 }; // 製作 福利勇敢藥水
                                                              // addContribution + 2
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40014 };
                            counts = new int[] { 1000, 3 };
                            contribution = 2;
                            htmlid = "";
                            break;
                        case '1':
                            createitem = new int[] { 49304 }; // 製作 福利森林藥水
                                                              // addContribution + 4
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40068 };
                            counts = new int[] { 1000, 3 };
                            contribution = 4;
                            htmlid = "";
                            break;
                        case '2':
                            createitem = new int[] { 49307 }; // 製作 福利慎重藥水
                                                              // addContribution + 2
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40016 };
                            counts = new int[] { 500, 3 };
                            contribution = 2;
                            htmlid = "";
                            break;
                        case '3':
                            createitem = new int[] { 49306 }; // 製作 福利藍色藥水
                                                              // addContribution + 2
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40015 };
                            counts = new int[] { 1000, 3 };
                            contribution = 2;
                            htmlid = "";
                            break;
                        case '4':
                            createitem = new int[] { 49302 }; // 製作 福利加速藥水
                                                              // addContribution + 1
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40013 };
                            counts = new int[] { 500, 3 };
                            contribution = 1;
                            htmlid = "";
                            break;
                        case '5':
                            createitem = new int[] { 49303 }; // 製作 福利呼吸藥水
                                                              // addContribution + 1
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40032 };
                            counts = new int[] { 500, 3 };
                            contribution = 1;
                            htmlid = "";
                            break;
                        case '6':
                            createitem = new int[] { 49308 }; // 製作 福利變形藥水
                                                              // addContribution + 3
                            createcount = new int[] { 1 };
                            materials = new int[] { L1ItemId.ADENA, 40088 };
                            counts = new int[] { 1000, 3 };
                            contribution = 3;
                            htmlid = "";
                            break;
                        case 'A':
                        case 'a':
                            switch (townid)
                            {
                                case 1:
                                    createitem = new int[] { 49292 }; // 購買 福利傳送卷軸：說話之島
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 2:
                                    createitem = new int[] { 49297 }; // 購買 福利傳送卷軸：銀騎士
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 3:
                                    createitem = new int[] { 49293 }; // 購買 福利傳送卷軸：古魯丁
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 4:
                                    createitem = new int[] { 49296 }; // 購買 福利傳送卷軸：燃柳
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 5:
                                    createitem = new int[] { 49295 }; // 購買 福利傳送卷軸：風木
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 6:
                                    createitem = new int[] { 49294 }; // 購買 福利傳送卷軸：肯特
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 7:
                                    createitem = new int[] { 49298 }; // 購買 福利傳送卷軸：奇岩
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 8:
                                    createitem = new int[] { 49299 }; // 購買 福利傳送卷軸：海音
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 9:
                                    createitem = new int[] { 49301 }; // 購買 福利傳送卷軸：威頓
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                case 10:
                                    createitem = new int[] { 49300 }; // 購買 福利傳送卷軸：歐瑞
                                    createcount = new int[] { 1 };
                                    materials = new int[] { L1ItemId.ADENA };
                                    counts = new int[] { 400 };
                                    htmlid = "";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            // 多魯嘉貝爾
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81278)
            { // 多魯嘉之袋
                if (s == "0")
                {
                    if (pc.Inventory.checkItem(46000, 1))
                    { // 檢查身上是否有多魯嘉之袋
                        htmlid = "veil3"; // 已經有袋子了
                    }
                    else if (pc.Inventory.checkItem(L1ItemId.ADENA, 1000000))
                    { // 檢查身上金幣是否足夠
                        pc.Inventory.consumeItem(L1ItemId.ADENA, 1000000);
                        pc.Inventory.storeItem(46000, 1);
                        htmlid = "veil7"; // 購買成功顯示
                    }
                    else if (!pc.Inventory.checkItem(L1ItemId.ADENA, 1000000))
                    { // 檢查身上金幣是否足夠
                        htmlid = "veil4"; // 錢不夠顯示 我們還是不要約定了
                    }
                }
                else if (s == "1")
                {
                    htmlid = "veil9"; // 聽取建議
                }
            }
            else if (((L1NpcInstance)obj).NpcTemplate.get_npcId() == 81277)
            { // 隱匿的巨龍谷入口
                int level = pc.Level; // 角色等級
                char s1 = s[0];
                if (s == "0")
                {
                    if (level >= 30 && level <= 51)
                    {
                        L1Teleport.teleport(pc, 32820, 32904, (short)1002, 5, true); // 前往侏儒部落
                        htmlid = "";
                    }
                    else
                    {
                        htmlid = "dsecret3";
                    }
                }
                else if (level >= 52)
                {
                    switch (s1)
                    {
                        case '1':
                            L1Teleport.teleport(pc, 32904, 32627, (short)1002, 5, true); // 前往造化之地(地)
                            break;
                        case '2':
                            L1Teleport.teleport(pc, 32793, 32593, (short)1002, 5, true); // 前往造化之地(火)
                            break;
                        case '3':
                            L1Teleport.teleport(pc, 32874, 32785, (short)1002, 5, true); // 前往造化之地(水)
                            break;
                        case '4':
                            L1Teleport.teleport(pc, 32993, 32716, (short)1002, 4, true); // 前往造化之地(風)
                            break;
                        case '5':
                            L1Teleport.teleport(pc, 32698, 32664, (short)1002, 6, true); // 前往龍之墓(北邊)
                            break;
                        case '6':
                            L1Teleport.teleport(pc, 32710, 32759, (short)1002, 6, true); // 前往龍之墓(南邊)
                            break;
                        case '7':
                            L1Teleport.teleport(pc, 32986, 32630, (short)1002, 4, true); // 前往蒼空之谷
                            break;
                    }
                    htmlid = "";
                }
                else
                {
                    htmlid = "dsecret3";
                }
            }

            // else System.out.println("C_NpcAction: " + s);
            if ((!string.ReferenceEquals(htmlid, null)) && htmlid == "colos2")
            {
                htmldata = makeUbInfoStrings(((L1NpcInstance)obj).NpcTemplate.get_npcId());
            }
            if (createitem != null)
            { // アイテム精製
                bool isCreate = true;
                if (materials != null)
                {
                    for (int j = 0; j < materials.Length; j++)
                    {
                        if (!pc.Inventory.checkItemNotEquipped(materials[j], counts[j]))
                        {
                            L1Item temp = ItemTable.Instance.getTemplate(materials[j]);
                            pc.sendPackets(new S_ServerMessage(337, temp.Name)); // \f1%0が不足しています。
                            isCreate = false;
                        }
                    }
                }

                if (isCreate)
                {
                    // 容量と重量の計算
                    int create_count = 0; // アイテムの個数（纏まる物は1個）
                    int create_weight = 0;
                    for (int k = 0; k < createitem.Length; k++)
                    {
                        if ((createitem[k] > 0) && (createcount[k] > 0))
                        {
                            L1Item temp = ItemTable.Instance.getTemplate(createitem[k]);
                            if (temp != null)
                            {
                                if (temp.Stackable)
                                {
                                    if (!pc.Inventory.checkItem(createitem[k]))
                                    {
                                        create_count += 1;
                                    }
                                }
                                else
                                {
                                    create_count += createcount[k];
                                }
                                create_weight += temp.Weight * createcount[k] / 1000;
                            }
                        }
                    }
                    // 容量確認
                    if (pc.Inventory.Size + create_count > 180)
                    {
                        pc.sendPackets(new S_ServerMessage(263)); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
                        return;
                    }
                    // 重量確認
                    if (pc.MaxWeight < pc.Inventory.Weight + create_weight)
                    {
                        pc.sendPackets(new S_ServerMessage(82)); // アイテムが重すぎて、これ以上持てません。
                        return;
                    }

                    if (materials != null)
                    {
                        for (int j = 0; j < materials.Length; j++)
                        {
                            // 材料消費
                            pc.Inventory.consumeItem(materials[j], counts[j]);
                        }
                    }
                    for (int k = 0; k < createitem.Length; k++)
                    {
                        if ((createitem[k] > 0) && (createcount[k] > 0))
                        {
                            L1ItemInstance item = pc.Inventory.storeItem(createitem[k], createcount[k]);
                            if (item != null)
                            {
                                string itemName = ItemTable.Instance.getTemplate(createitem[k]).Name;
                                string createrName = "";
                                if (obj is L1NpcInstance)
                                {
                                    createrName = ((L1NpcInstance)obj).NpcTemplate.get_name();
                                }
                                if (createcount[k] > 1)
                                {
                                    pc.sendPackets(new S_ServerMessage(143, createrName, itemName + " (" + createcount[k] + ")")); // \f1%0が%1をくれました。
                                }
                                else
                                {
                                    pc.sendPackets(new S_ServerMessage(143, createrName, itemName)); // \f1%0が%1をくれました。
                                }
                            }
                        }
                    }
                    if (!string.ReferenceEquals(success_htmlid, null))
                    { // html指定がある場合は表示
                        pc.sendPackets(new S_NPCTalkReturn(objid, success_htmlid, htmldata));
                    }
                    if (questid > 0)
                    {
                        pc.Quest.set_step(questid, questvalue);
                    }
                    if (contribution > 0)
                    {
                        pc.addContribution(contribution);
                    }
                }
                else
                { // 精製失敗
                    if (!string.ReferenceEquals(failure_htmlid, null))
                    { // html指定がある場合は表示
                        pc.sendPackets(new S_NPCTalkReturn(objid, failure_htmlid, htmldata));
                    }
                }
            }

            if (!string.ReferenceEquals(htmlid, null))
            { // html指定がある場合は表示
                pc.sendPackets(new S_NPCTalkReturn(objid, htmlid, htmldata));
            }
        }

        private string karmaLevelToHtmlId(int level)
        {
            if ((level == 0) || (level < -7) || (7 < level))
            {
                return "";
            }
            string htmlid = "";
            if (0 < level)
            {
                htmlid = "vbk" + level;
            }
            else if (level < 0)
            {
                htmlid = "vyk" + Math.Abs(level);
            }
            return htmlid;
        }

        private string watchUb(L1PcInstance pc, int npcId)
        {
            L1UltimateBattle ub = UBTable.Instance.getUbForNpcId(npcId);
            L1Location loc = ub.Location;
            if (pc.Inventory.consumeItem(L1ItemId.ADENA, 100))
            {
                try
                {
                    pc.Save();
                    pc.beginGhost(loc.X, loc.Y, (short)loc.MapId, true);
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
            }
            return "";
        }

        private string enterUb(L1PcInstance pc, int npcId)
        {
            L1UltimateBattle ub = UBTable.Instance.getUbForNpcId(npcId);
            if (!ub.Active || !ub.canPcEnter(pc))
            { // 時間外
                return "colos2";
            }
            if (ub.NowUb)
            { // 競技中
                return "colos1";
            }
            if (ub.MembersCount >= ub.MaxPlayer)
            { // 定員オーバー
                return "colos4";
            }

            ub.addMember(pc); // メンバーに追加
            L1Location loc = ub.Location.randomLocation(10, false);
            L1Teleport.teleport(pc, loc.X, loc.Y, ub.MapId, 5, true);
            return "";
        }

        private string enterHauntedHouse(L1PcInstance pc)
        {
            if (L1HauntedHouse.Instance.HauntedHouseStatus == L1HauntedHouse.STATUS_PLAYING)
            { // 競技中
                pc.sendPackets(new S_ServerMessage(1182)); // もうゲームは始まってるよ。
                return "";
            }
            if (L1HauntedHouse.Instance.MembersCount >= 10)
            { // 定員オーバー
                pc.sendPackets(new S_ServerMessage(1184)); // お化け屋敷は人でいっぱいだよ。
                return "";
            }

            L1HauntedHouse.Instance.addMember(pc); // メンバーに追加
            L1Teleport.teleport(pc, 32722, 32830, (short)5140, 2, true);
            return "";
        }

        private string enterPetMatch(L1PcInstance pc, int objid2)
        {
            if (pc.PetList.Values.Count > 0)
            {
                pc.sendPackets(new S_ServerMessage(1187)); // ペットのアミュレットが使用中です。
                return "";
            }
            if (!L1PetMatch.Instance.enterPetMatch(pc, objid2))
            {
                pc.sendPackets(new S_ServerMessage(1182)); // もうゲームは始まってるよ。
            }
            return "";
        }

        private void poly(ClientThread clientthread, int polyId)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) || (awakeSkillId == L1SkillId.AWAKEN_FAFURION) || (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 現在の状態では変身できません。
                return;
            }

            if (pc.Inventory.checkItem(L1ItemId.ADENA, 100))
            { // check
                pc.Inventory.consumeItem(L1ItemId.ADENA, 100); // del

                L1PolyMorph.doPoly(pc, polyId, 1800, L1PolyMorph.MORPH_BY_NPC);
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(337, "$4")); // アデナが不足しています。
            }
        }

        private void polyByKeplisha(ClientThread clientthread, int polyId)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 現在の状態では変身できません。
                return;
            }

            if (pc.Inventory.checkItem(L1ItemId.ADENA, 100))
            { // check
                pc.Inventory.consumeItem(L1ItemId.ADENA, 100); // del

                L1PolyMorph.doPoly(pc, polyId, 1800, L1PolyMorph.MORPH_BY_KEPLISHA);
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(337, "$4")); // アデナが不足しています。
            }
        }

        private string sellHouse(L1PcInstance pc, int objectId, int npcId)
        {
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan == null)
            {
                return ""; // ウィンドウを消す
            }
            int houseId = clan.HouseId;
            if (houseId == 0)
            {
                return ""; // ウィンドウを消す
            }
            L1House house = HouseTable.Instance.getHouseTable(houseId);
            int keeperId = house.KeeperId;
            if (npcId != keeperId)
            {
                return ""; // ウィンドウを消す
            }
            if (!pc.Crown)
            {
                pc.sendPackets(new S_ServerMessage(518)); // この命令は血盟の君主のみが利用できます。
                return ""; // ウィンドウを消す
            }
            if (pc.Id != clan.LeaderId)
            {
                pc.sendPackets(new S_ServerMessage(518)); // この命令は血盟の君主のみが利用できます。
                return ""; // ウィンドウを消す
            }
            if (house.OnSale)
            {
                return "agonsale";
            }

            pc.sendPackets(new S_SellHouse(objectId, houseId.ToString()));
            return null;
        }

        private void openCloseDoor(L1PcInstance pc, L1NpcInstance npc, string s)
        {
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan != null)
            {
                int houseId = clan.HouseId;
                if (houseId != 0)
                {
                    L1House house = HouseTable.Instance.getHouseTable(houseId);
                    int keeperId = house.KeeperId;
                    if (npc.NpcTemplate.get_npcId() == keeperId)
                    {
                        L1DoorInstance door1 = null;
                        L1DoorInstance door2 = null;
                        L1DoorInstance door3 = null;
                        L1DoorInstance door4 = null;
                        foreach (L1DoorInstance door in Container.Instance.Resolve<IDoorController>().DoorList)
                        {
                            if (door.KeeperId == keeperId)
                            {
                                if (door1 == null)
                                {
                                    door1 = door;
                                    continue;
                                }
                                if (door2 == null)
                                {
                                    door2 = door;
                                    continue;
                                }
                                if (door3 == null)
                                {
                                    door3 = door;
                                    continue;
                                }
                                if (door4 == null)
                                {
                                    door4 = door;
                                    break;
                                }
                            }
                        }
                        if (door1 != null)
                        {
                            if (s == "open")
                            {
                                door1.open();
                            }
                            else if (s == "close")
                            {
                                door1.close();
                            }
                        }
                        if (door2 != null)
                        {
                            if (s == "open")
                            {
                                door2.open();
                            }
                            else if (s == "close")
                            {
                                door2.close();
                            }
                        }
                        if (door3 != null)
                        {
                            if (s == "open")
                            {
                                door3.open();
                            }
                            else if (s == "close")
                            {
                                door3.close();
                            }
                        }
                        if (door4 != null)
                        {
                            if (s == "open")
                            {
                                door4.open();
                            }
                            else if (s == "close")
                            {
                                door4.close();
                            }
                        }
                    }
                }
            }
        }

        private void openCloseGate(L1PcInstance pc, int keeperId, bool isOpen)
        {
            bool isNowWar = false;
            int pcCastleId = 0;
            if (pc.Clanid != 0)
            {
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    pcCastleId = clan.CastleId;
                }
            }
            if ((keeperId == 70656) || (keeperId == 70549) || (keeperId == 70985))
            { // ケント城
                if (isExistDefenseClan(L1CastleLocation.KENT_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.KENT_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.KENT_CASTLE_ID);
            }
            else if (keeperId == 70600)
            { // OT
                if (isExistDefenseClan(L1CastleLocation.OT_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.OT_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.OT_CASTLE_ID);
            }
            else if ((keeperId == 70778) || (keeperId == 70987) || (keeperId == 70687))
            { // WW城
                if (isExistDefenseClan(L1CastleLocation.WW_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.WW_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.WW_CASTLE_ID);
            }
            else if ((keeperId == 70817) || (keeperId == 70800) || (keeperId == 70988) || (keeperId == 70990) || (keeperId == 70989) || (keeperId == 70991))
            { // ギラン城
                if (isExistDefenseClan(L1CastleLocation.GIRAN_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.GIRAN_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.GIRAN_CASTLE_ID);
            }
            else if ((keeperId == 70863) || (keeperId == 70992) || (keeperId == 70862))
            { // ハイネ城
                if (isExistDefenseClan(L1CastleLocation.HEINE_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.HEINE_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.HEINE_CASTLE_ID);
            }
            else if ((keeperId == 70995) || (keeperId == 70994) || (keeperId == 70993))
            { // ドワーフ城
                if (isExistDefenseClan(L1CastleLocation.DOWA_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.DOWA_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.DOWA_CASTLE_ID);
            }
            else if (keeperId == 70996)
            { // アデン城
                if (isExistDefenseClan(L1CastleLocation.ADEN_CASTLE_ID))
                {
                    if (pcCastleId != L1CastleLocation.ADEN_CASTLE_ID)
                    {
                        return;
                    }
                }
                isNowWar = WarTimeController.Instance.isNowWar(L1CastleLocation.ADEN_CASTLE_ID);
            }

            foreach (L1DoorInstance door in Container.Instance.Resolve<IDoorController>().DoorList)
            {
                if (door.KeeperId == keeperId)
                {
                    if (isNowWar && (door.MaxHp > 1))
                    { // 戦争中は城門開閉不可
                    }
                    else
                    {
                        if (isOpen)
                        { // 開
                            door.open();
                        }
                        else
                        { // 閉
                            door.close();
                        }
                    }
                }
            }
        }

        private bool isExistDefenseClan(int castleId)
        {
            bool isExistDefenseClan = false;
            foreach (L1Clan clan in Container.Instance.Resolve<IGameWorld>().AllClans)
            {
                if (castleId == clan.CastleId)
                {
                    isExistDefenseClan = true;
                    break;
                }
            }
            return isExistDefenseClan;
        }

        private void expelOtherClan(L1PcInstance clanPc, int keeperId)
        {
            int houseId = 0;
            foreach (L1House house in HouseTable.Instance.HouseTableList)
            {
                if (house.KeeperId == keeperId)
                {
                    houseId = house.HouseId;
                }
            }
            if (houseId == 0)
            {
                return;
            }

            int[] loc = new int[3];
            foreach (GameObject @object in Container.Instance.Resolve<IGameWorld>().Object)
            {
                if (@object is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)@object;
                    if (L1HouseLocation.isInHouseLoc(houseId, pc.X, pc.Y, pc.MapId) && (clanPc.Clanid != pc.Clanid))
                    {
                        loc = L1HouseLocation.getHouseTeleportLoc(houseId, 0);
                        if (pc != null)
                        {
                            L1Teleport.teleport(pc, loc[0], loc[1], (short)loc[2], 5, true);
                        }
                    }
                }
            }
        }

        private void repairGate(L1PcInstance pc)
        {
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan != null)
            {
                int castleId = clan.CastleId;
                if (castleId != 0)
                { // 城主クラン
                    if (!WarTimeController.Instance.isNowWar(castleId))
                    {
                        // 城門を元に戻す
                        foreach (L1DoorInstance door in Container.Instance.Resolve<IDoorController>().DoorList)
                        {
                            if (L1CastleLocation.checkInWarArea(castleId, door))
                            {
                                door.repairGate();
                            }
                        }
                        pc.sendPackets(new S_ServerMessage(990)); // 城門自動修理を命令しました。
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(991)); // 城門自動修理命令を取り消しました。
                    }
                }
            }
        }

        private bool payFee(L1PcInstance pc, L1NpcInstance npc)
        {
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan != null)
            {
                int houseId = clan.HouseId;
                if (houseId != 0)
                {
                    L1House house = HouseTable.Instance.getHouseTable(houseId);
                    int keeperId = house.KeeperId;
                    if (npc.NpcTemplate.get_npcId() == keeperId)
                    {
                        DateTime cal = DateTime.Now; // 目前時間
                        DateTime deadlineCal = house.TaxDeadline; // 盟屋到期時間

                        int remainingTime = (int)((deadlineCal - cal).Ticks / (1000 * 60 * 60 * 24));
                        // 租期剩餘時間大於一半 不用繳房租
                        if (remainingTime >= Config.HOUSE_TAX_INTERVAL / 2)
                        {
                            return true;
                        }
                        else if (pc.Inventory.checkItem(L1ItemId.ADENA, 2000))
                        {
                            pc.Inventory.consumeItem(L1ItemId.ADENA, 2000);
                            // 支付後 deadline延期
                            deadlineCal.AddDays(Config.HOUSE_TAX_INTERVAL);
                            house.TaxDeadline = deadlineCal;
                            HouseTable.Instance.updateHouse(house); // DBに書き込み
                            return true;
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
                        }
                    }
                }
            }
            return false;
        }

        private string[] makeHouseTaxStrings(L1PcInstance pc, L1NpcInstance npc)
        {
            string name = npc.NpcTemplate.get_name();
            string[] result;
            result = new string[] { name, "2000", "1", "1", "00" };
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
            if (clan != null)
            {
                int houseId = clan.HouseId;
                if (houseId != 0)
                {
                    L1House house = HouseTable.Instance.getHouseTable(houseId);
                    int keeperId = house.KeeperId;
                    if (npc.NpcTemplate.get_npcId() == keeperId)
                    {
                        DateTime cal = house.TaxDeadline;
                        int month = cal.Month + 1;
                        int day = cal.Day;
                        int hour = cal.Hour;
                        result = new string[] { name, "2000", month.ToString(), day.ToString(), hour.ToString() };
                    }
                }
            }
            return result;
        }

        private string[] makeWarTimeStrings(int castleId)
        {
            L1Castle castle = CastleTable.Instance.getCastleTable(castleId);
            if (castle == null)
            {
                return null;
            }
            DateTime warTime = castle.WarTime;
            int year = warTime.Year;
            int month = warTime.Month + 1;
            int day = warTime.Day;
            int hour = warTime.Hour;
            int minute = warTime.Minute;
            string[] result;
            if (castleId == L1CastleLocation.OT_CASTLE_ID)
            {
                result = new string[] { year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString() };
            }
            else
            {
                result = new string[] { "", year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString() };
            }
            return result;
        }

        private string getYaheeAmulet(L1PcInstance pc, L1NpcInstance npc, string s)
        {
            int[] amuletIdList = new int[] { 20358, 20359, 20360, 20361, 20362, 20363, 20364, 20365 };
            int amuletId = 0;
            L1ItemInstance item = null;
            string htmlid = null;
            if (s == "1")
            {
                amuletId = amuletIdList[0];
            }
            else if (s == "2")
            {
                amuletId = amuletIdList[1];
            }
            else if (s == "3")
            {
                amuletId = amuletIdList[2];
            }
            else if (s == "4")
            {
                amuletId = amuletIdList[3];
            }
            else if (s == "5")
            {
                amuletId = amuletIdList[4];
            }
            else if (s == "6")
            {
                amuletId = amuletIdList[5];
            }
            else if (s == "7")
            {
                amuletId = amuletIdList[6];
            }
            else if (s == "8")
            {
                amuletId = amuletIdList[7];
            }
            if (amuletId != 0)
            {
                item = pc.Inventory.storeItem(amuletId, 1);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                foreach (int id in amuletIdList)
                {
                    if (id == amuletId)
                    {
                        break;
                    }
                    if (pc.Inventory.checkItem(id))
                    {
                        pc.Inventory.consumeItem(id, 1);
                    }
                }
                htmlid = "";
            }
            return htmlid;
        }

        private string getBarlogEarring(L1PcInstance pc, L1NpcInstance npc, string s)
        {
            int[] earringIdList = new int[] { 21020, 21021, 21022, 21023, 21024, 21025, 21026, 21027 };
            int earringId = 0;
            L1ItemInstance item = null;
            string htmlid = null;
            if (s == "1")
            {
                earringId = earringIdList[0];
            }
            else if (s == "2")
            {
                earringId = earringIdList[1];
            }
            else if (s == "3")
            {
                earringId = earringIdList[2];
            }
            else if (s == "4")
            {
                earringId = earringIdList[3];
            }
            else if (s == "5")
            {
                earringId = earringIdList[4];
            }
            else if (s == "6")
            {
                earringId = earringIdList[5];
            }
            else if (s == "7")
            {
                earringId = earringIdList[6];
            }
            else if (s == "8")
            {
                earringId = earringIdList[7];
            }
            if (earringId != 0)
            {
                item = pc.Inventory.storeItem(earringId, 1);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                foreach (int id in earringIdList)
                {
                    if (id == earringId)
                    {
                        break;
                    }
                    if (pc.Inventory.checkItem(id))
                    {
                        pc.Inventory.consumeItem(id, 1);
                    }
                }
                htmlid = "";
            }
            return htmlid;
        }

        private string[] makeUbInfoStrings(int npcId)
        {
            L1UltimateBattle ub = UBTable.Instance.getUbForNpcId(npcId);
            return ub.makeUbInfoStrings();
        }

        private string talkToDimensionDoor(L1PcInstance pc, L1NpcInstance npc, string s)
        {
            string htmlid = "";
            int protectionId = 0;
            int sealId = 0;
            int locX = 0;
            int locY = 0;
            short mapId = 0;
            if (npc.NpcTemplate.get_npcId() == 80059)
            { // 次元の扉(土)
                protectionId = 40909;
                sealId = 40913;
                locX = 32773;
                locY = 32835;
                mapId = 607;
            }
            else if (npc.NpcTemplate.get_npcId() == 80060)
            { // 次元の扉(風)
                protectionId = 40912;
                sealId = 40916;
                locX = 32757;
                locY = 32842;
                mapId = 606;
            }
            else if (npc.NpcTemplate.get_npcId() == 80061)
            { // 次元の扉(水)
                protectionId = 40910;
                sealId = 40914;
                locX = 32830;
                locY = 32822;
                mapId = 604;
            }
            else if (npc.NpcTemplate.get_npcId() == 80062)
            { // 次元の扉(火)
                protectionId = 40911;
                sealId = 40915;
                locX = 32835;
                locY = 32822;
                mapId = 605;
            }

            // 「中に入ってみる」「元素の支配者を近づけてみる」「通行証を使う」「通過する」
            if (s == "a")
            {
                L1Teleport.teleport(pc, locX, locY, mapId, 5, true);
                htmlid = "";
            }
            // 「絵から突出部分を取り除く」
            else if (s == "b")
            {
                L1ItemInstance item = pc.Inventory.storeItem(protectionId, 1);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                htmlid = "";
            }
            // 「通行証を捨てて、この地をあきらめる」
            else if (s == "c")
            {
                htmlid = "wpass07";
            }
            // 「続ける」
            else if (s == "d")
            {
                if (pc.Inventory.checkItem(sealId))
                { // 地の印章
                    L1ItemInstance item = pc.Inventory.findItemId(sealId);
                    pc.Inventory.consumeItem(sealId, item.Count);
                }
            }
            // 「そのままにする」「慌てて拾う」
            else if (s == "e")
            {
                htmlid = "";
            }
            // 「消えるようにする」
            else if (s == "f")
            {
                if (pc.Inventory.checkItem(protectionId))
                { // 地の通行証
                    pc.Inventory.consumeItem(protectionId, 1);
                }
                if (pc.Inventory.checkItem(sealId))
                { // 地の印章
                    L1ItemInstance item = pc.Inventory.findItemId(sealId);
                    pc.Inventory.consumeItem(sealId, item.Count);
                }
                htmlid = "";
            }
            return htmlid;
        }

        private bool isNpcSellOnly(L1NpcInstance npc)
        {
            int npcId = npc.NpcTemplate.get_npcId();
            string npcName = npc.NpcTemplate.get_name();
            if ((npcId == 70027) || "亞丁商團" == npcName)
            {
                return true;
            }
            return false;
        }

        private void getBloodCrystalByKarma(L1PcInstance pc, L1NpcInstance npc, string s)
        {
            L1ItemInstance item = null;

            // 「ブラッドクリスタルの欠片を1個ください」
            if (s == "1")
            {
                pc.addKarma((int)(500 * Config.RATE_KARMA));
                item = pc.Inventory.storeItem(40718, 1);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                // ヤヒの姿を記憶するのが難しくなります。
                pc.sendPackets(new S_ServerMessage(1081));
            }
            // 「ブラッドクリスタルの欠片を10個ください」
            else if (s == "2")
            {
                pc.addKarma((int)(5000 * Config.RATE_KARMA));
                item = pc.Inventory.storeItem(40718, 10);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                // ヤヒの姿を記憶するのが難しくなります。
                pc.sendPackets(new S_ServerMessage(1081));
            }
            // 「ブラッドクリスタルの欠片を100個ください」
            else if (s == "3")
            {
                pc.addKarma((int)(50000 * Config.RATE_KARMA));
                item = pc.Inventory.storeItem(40718, 100);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                // ヤヒの姿を記憶するのが難しくなります。
                pc.sendPackets(new S_ServerMessage(1081));
            }
        }

        private void getSoulCrystalByKarma(L1PcInstance pc, L1NpcInstance npc, string s)
        {
            L1ItemInstance item = null;

            // 「ソウルクリスタルの欠片を1個ください」
            if (s == "1")
            {
                pc.addKarma((int)(-500 * Config.RATE_KARMA));
                item = pc.Inventory.storeItem(40678, 1);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                // バルログの冷笑を感じ悪寒が走ります。
                pc.sendPackets(new S_ServerMessage(1080));
            }
            // 「ソウルクリスタルの欠片を10個ください」
            else if (s == "2")
            {
                pc.addKarma((int)(-5000 * Config.RATE_KARMA));
                item = pc.Inventory.storeItem(40678, 10);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                // バルログの冷笑を感じ悪寒が走ります。
                pc.sendPackets(new S_ServerMessage(1080));
            }
            // 「ソウルクリスタルの欠片を100個ください」
            else if (s == "3")
            {
                pc.addKarma((int)(-50000 * Config.RATE_KARMA));
                item = pc.Inventory.storeItem(40678, 100);
                if (item != null)
                {
                    pc.sendPackets(new S_ServerMessage(143, npc.NpcTemplate.get_name(), item.LogName)); // \f1%0が%1をくれました。
                }
                // バルログの冷笑を感じ悪寒が走ります。
                pc.sendPackets(new S_ServerMessage(1080));
            }
        }

        private bool usePolyScroll(L1PcInstance pc, int itemId, string s)
        {
            int time = 0;
            if ((itemId == 40088) || (itemId == 40096))
            { // 変身スクロール、象牙の塔の変身スクロール
                time = 1800;
            }
            else if (itemId == 140088)
            { // 祝福された変身スクロール
                time = 2100;
            }

            L1PolyMorph poly = PolyTable.Instance.getTemplate(s);
            L1ItemInstance item = pc.Inventory.findItemId(itemId);
            bool isUseItem = false;
            if ((poly != null) || s == "none")
            {
                if (s == "none")
                {
                    if ((pc.TempCharGfx == 6034) || (pc.TempCharGfx == 6035))
                    {
                        isUseItem = true;
                    }
                    else
                    {
                        pc.removeSkillEffect(L1SkillId.SHAPE_CHANGE);
                        isUseItem = true;
                    }
                }
                else if ((poly.MinLevel <= pc.Level) || pc.Gm)
                {
                    L1PolyMorph.doPoly(pc, poly.PolyId, time, L1PolyMorph.MORPH_BY_ITEMMAGIC);
                    isUseItem = true;
                }
            }
            if (isUseItem)
            {
                pc.Inventory.removeItem(item, 1);
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(181)); // \f1そのようなモンスターには変身できません。
            }
            return isUseItem;
        }

        public override string Type
        {
            get
            {
                return C_NPC_ACTION;
            }
        }

    }

}