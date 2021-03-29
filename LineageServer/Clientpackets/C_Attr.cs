using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using System.IO;
using LineageServer.Server;

namespace LineageServer.Clientpackets
{
    class C_Attr : ClientBasePacket
    {
        private const string C_ATTR = "[C] C_Attr";

        private static readonly int[] HEADING_TABLE_X = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };

        private static readonly int[] HEADING_TABLE_Y = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };

        public C_Attr(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int i = ReadH(); // 3.51C未知的功能
            int attrcode;

            if (i == 479)
            {
                attrcode = i;
            }
            else
            {
                //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
                //ORIGINAL LINE: @SuppressWarnings("unused") int count = readD();
                int count = ReadD(); // 紀錄世界中發送YesNo的次數
                attrcode = ReadH();
            }

            string name;
            int c;
            string clanName = pc.Clanname;
            switch (attrcode)
            {
                case 97: // \f3%0%s 想加入你的血盟。你接受嗎。(Y/N)
                    c = ReadH();
                    L1PcInstance joinPc = (L1PcInstance)L1World.Instance.findObject(pc.TempID);
                    pc.TempID = 0;
                    if (joinPc != null)
                    {
                        if (c == 0)
                        { // No
                            joinPc.sendPackets(new S_ServerMessage(96, pc.Name)); //  拒絕你的請求。
                        }
                        else if (c == 1)
                        { // Yes
                            int clan_id = pc.Clanid;
                            L1Clan clan = L1World.Instance.getClan(clanName);
                            if (clan != null)
                            {
                                int maxMember = 0;
                                int charisma = pc.Cha;
                                // 公式
                                maxMember = charisma * 3 * (2 + pc.Level / 50);
                                // 未過45 人數/3
                                if (!pc.Quest.isEnd(L1Quest.QUEST_LEVEL45))
                                {
                                    maxMember /= 3;
                                }

                                if (Config.MAX_CLAN_MEMBER > 0)
                                { // 設定檔中如果有設定血盟的人數上限
                                    maxMember = Config.MAX_CLAN_MEMBER;
                                }

                                if (joinPc.Clanid == 0)
                                { // 加入玩家未加入血盟
                                    string[] clanMembersName = clan.AllMembers;
                                    if (maxMember <= clanMembersName.Length)
                                    { // 血盟還有空間可以讓玩家加入
                                        joinPc.sendPackets(new S_ServerMessage(188, pc.Name));
                                        return;
                                    }
                                    if (joinPc.Crown)
                                    { // 如果是王加入，判定收人方是否通過45試煉
                                        if (!pc.Quest.isEnd(L1Quest.QUEST_LEVEL45))
                                        {
                                            return;
                                        }
                                    }
                                    foreach (L1PcInstance clanMembers in clan.OnlineClanMember)
                                    {
                                        clanMembers.sendPackets(new S_ServerMessage(94, joinPc.Name)); // \f1你接受%0當你的血盟成員。
                                    }
                                    joinPc.Clanid = clan_id;
                                    joinPc.Clanname = clanName;
                                    joinPc.ClanRank = L1Clan.CLAN_RANK_PUBLIC;
                                    joinPc.ClanMemberNotes = "";
                                    joinPc.Title = "";
                                    joinPc.sendPackets(new S_CharTitle(joinPc.Id, ""));
                                    joinPc.broadcastPacket(new S_CharTitle(joinPc.Id, ""));
                                    joinPc.Save(); // 儲存加入的玩家資料
                                    clan.addMemberName(joinPc.Name);
                                    ClanMembersTable.Instance.newMember(joinPc);
                                    joinPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_PUBLIC, joinPc.Name)); // 你的階級變更為
                                    joinPc.sendPackets(new S_ServerMessage(95, clanName)); // \f1加入%0血盟。
                                    joinPc.sendPackets(new S_ClanName(joinPc, true));
                                    joinPc.sendPackets(new S_CharReset(joinPc.Id, clan.ClanId));
                                    joinPc.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, pc.Clan.EmblemStatus)); // TODO
                                    joinPc.sendPackets(new S_ClanAttention());
                                    foreach (L1PcInstance player in clan.OnlineClanMember)
                                    {
                                        player.sendPackets(new S_CharReset(joinPc.Id, joinPc.Clan.EmblemId));
                                        player.broadcastPacket(new S_CharReset(player.Id, joinPc.Clan.EmblemId));
                                    }
                                }
                                else
                                { // 如果是有血盟的聯盟王加入（聯合血盟）
                                    if (Config.CLAN_ALLIANCE && pc.Quest.isEnd(L1Quest.QUEST_LEVEL45))
                                    {
                                        changeClan(clientthread, pc, joinPc, maxMember);
                                    }
                                    else
                                    {
                                        joinPc.sendPackets(new S_ServerMessage(89)); // \f1你已經有血盟了。
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 217: // %0 血盟向你的血盟宣戰。是否接受？(Y/N)
                case 221: // %0 血盟要向你投降。是否接受？(Y/N)
                case 222: // %0 血盟要結束戰爭。是否接受？(Y/N)
                    c = ReadH();
                    L1PcInstance enemyLeader = (L1PcInstance)L1World.Instance.findObject(pc.TempID);
                    if (enemyLeader == null)
                    {
                        return;
                    }
                    pc.TempID = 0;
                    string enemyClanName = enemyLeader.Clanname;
                    if (c == 0)
                    { // No
                        if (i == 217)
                        {
                            enemyLeader.sendPackets(new S_ServerMessage(236, clanName)); // %0
                                                                                         // 血盟拒絕你的宣戰。
                        }
                        else if ((i == 221) || (i == 222))
                        {
                            enemyLeader.sendPackets(new S_ServerMessage(237, clanName)); // %0
                                                                                         // 血盟拒絕你的提案。
                        }
                    }
                    else if (c == 1)
                    { // Yes
                        if (i == 217)
                        {
                            L1War war = new L1War();
                            war.handleCommands(2, enemyClanName, clanName); // 盟戰開始
                        }
                        else if ((i == 221) || (i == 222))
                        {
                            // 取得線上所有的盟戰
                            foreach (L1War war in L1World.Instance.WarList)
                            {
                                if (war.CheckClanInWar(clanName))
                                { // 如果有現在的血盟
                                    if (i == 221)
                                    {
                                        war.SurrenderWar(enemyClanName, clanName); // 投降
                                    }
                                    else if (i == 222)
                                    {
                                        war.CeaseWar(enemyClanName, clanName); // 結束
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case 252: // \f2%0%s 要與你交易。願不願交易？ (Y/N)
                    c = ReadH();
                    L1PcInstance trading_partner = (L1PcInstance)L1World.Instance.findObject(pc.TradeID);
                    if (trading_partner != null)
                    {
                        if (c == 0) // No
                        {
                            trading_partner.sendPackets(new S_ServerMessage(253, pc.Name)); // %0%d
                                                                                            // 拒絕與你交易。
                            pc.TradeID = 0;
                            trading_partner.TradeID = 0;
                        }
                        else if (c == 1) // Yes
                        {
                            pc.sendPackets(new S_Trade(trading_partner.Name));
                            trading_partner.sendPackets(new S_Trade(pc.Name));
                        }
                    }
                    break;

                case 321: // 是否要復活？ (Y/N)
                    c = ReadH();
                    L1PcInstance resusepc1 = (L1PcInstance)L1World.Instance.findObject(pc.TempID);
                    pc.TempID = 0;
                    if (resusepc1 != null)
                    { // 如果有這個人
                        if (c == 0)
                        { // No

                        }
                        else if (c == 1)
                        { // Yes
                            resurrection(pc, resusepc1, (short)(pc.MaxHp / 2));
                        }
                    }
                    break;

                case 322: // 是否要復活？ (Y/N)
                    c = ReadH();
                    L1PcInstance resusepc2 = (L1PcInstance)L1World.Instance.findObject(pc.TempID);
                    pc.TempID = 0;
                    if (resusepc2 != null)
                    { // 祝福された 復活スクロール、リザレクション、グレーター リザレクション
                        if (c == 0)
                        { // No

                        }
                        else if (c == 1)
                        { // Yes
                            resurrection(pc, resusepc2, pc.MaxHp);
                            // EXPロストしている、G-RESを掛けられた、EXPロストした死亡
                            // 全てを満たす場合のみEXP復旧
                            if ((pc.ExpRes == 1) && pc.Gres && pc.GresValid)
                            {
                                pc.resExp();
                                pc.ExpRes = 0;
                                pc.Gres = false;
                            }
                        }
                    }
                    break;

                case 325: // 你想叫牠什麼名字？
                    c = ReadC(); // ?
                    name = ReadS();
                    L1PetInstance pet = (L1PetInstance)L1World.Instance.findObject(pc.TempID);
                    pc.TempID = 0;
                    renamePet(pet, name);
                    break;

                case 512: // 請輸入血盟小屋名稱?
                    c = ReadH(); // ?
                    name = ReadS();
                    int houseId = pc.TempID;
                    pc.TempID = 0;
                    if (name.Length <= 16)
                    {
                        L1House house = HouseTable.Instance.getHouseTable(houseId);
                        house.HouseName = name;
                        HouseTable.Instance.updateHouse(house); // 更新到資料庫中
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(513)); // 血盟小屋名稱太長。
                    }
                    break;

                case 630: // %0%s 要與你決鬥。你是否同意？(Y/N)
                    c = ReadH();
                    L1PcInstance fightPc = (L1PcInstance)L1World.Instance.findObject(pc.FightId);
                    if (c == 0)
                    {
                        pc.FightId = 0;
                        fightPc.FightId = 0;
                        fightPc.sendPackets(new S_ServerMessage(631, pc.Name)); // %0%dがあなたとの決闘を断りました。
                    }
                    else if (c == 1)
                    {
                        fightPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_DUEL, fightPc.FightId, fightPc.Id));
                        pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_DUEL, pc.FightId, pc.Id));
                    }
                    break;

                case 653: // 若你離婚，你的結婚戒指將會消失。你決定要離婚嗎？(Y/N)
                    c = ReadH();
                    L1PcInstance target653 = (L1PcInstance)L1World.Instance.findObject(pc.PartnerId);
                    if (c == 0)
                    { // No
                        return;
                    }
                    else if (c == 1)
                    { // Yes
                        if (target653 != null)
                        {
                            target653.PartnerId = 0;
                            target653.Save();
                            target653.sendPackets(new S_ServerMessage(662)); // \f1你(妳)目前未婚。
                        }
                        else
                        {
                            CharacterTable.updatePartnerId(pc.PartnerId);
                        }
                    }
                    pc.PartnerId = 0;
                    pc.Save(); // 將玩家資料儲存到資料庫中
                    pc.sendPackets(new S_ServerMessage(662)); // \f1你(妳)目前未婚。
                    break;

                case 654: // %0 向你(妳)求婚，你(妳)答應嗎?
                    c = ReadH();
                    L1PcInstance partner = (L1PcInstance)L1World.Instance.findObject(pc.TempID);
                    pc.TempID = 0;
                    if (partner != null)
                    {
                        if (c == 0)
                        { // No
                            partner.sendPackets(new S_ServerMessage(656, pc.Name)); // %0 拒絕你(妳)的求婚。
                        }
                        else if (c == 1)
                        { // Yes
                            pc.PartnerId = partner.Id;
                            pc.Save();
                            pc.sendPackets(new S_ServerMessage(790)); // 倆人的結婚在所有人的祝福下完成
                            pc.sendPackets(new S_ServerMessage(655, partner.Name)); // 恭喜!! %0  已接受你(妳)的求婚。

                            partner.PartnerId = pc.Id;
                            partner.Save();
                            partner.sendPackets(new S_ServerMessage(790)); // 恭喜!! %0 已接受你(妳)的求婚。
                            partner.sendPackets(new S_ServerMessage(655, pc.Name)); // 恭喜!! %0 已接受你(妳)的求婚。
                        }
                    }
                    break;

                // コールクラン
                case 729: // 盟主正在呼喚你，你要接受他的呼喚嗎？(Y/N)
                    c = ReadH();
                    if (c == 0)
                    { // No

                    }
                    else if (c == 1)
                    { // Yes
                        callClan(pc);
                    }
                    break;

                case 738: // 恢復經驗值需消耗%0金幣。想要恢復經驗值嗎?
                    c = ReadH();
                    if ((c == 1) && (pc.ExpRes == 1))
                    { // Yes
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
                        if (pc.Inventory.consumeItem(L1ItemId.ADENA, cost))
                        {
                            pc.resExp();
                            pc.ExpRes = 0;
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(189)); // \f1金幣不足。
                        }
                    }
                    break;

                case 951: // 您要接受玩家 %0%s 提出的隊伍對話邀請嗎？(Y/N)
                    c = ReadH();
                    L1PcInstance chatPc = (L1PcInstance)L1World.Instance.findObject(pc.PartyID);
                    if (chatPc != null)
                    {
                        if (c == 0)
                        { // No
                            chatPc.sendPackets(new S_ServerMessage(423, pc.Name)); // %0%s
                                                                                   // 拒絕了您的邀請。
                            pc.PartyID = 0;
                        }
                        else if (c == 1)
                        { // Yes
                            if (chatPc.InChatParty)
                            {
                                if (chatPc.ChatParty.Vacancy || chatPc.Gm)
                                {
                                    chatPc.ChatParty.addMember(pc);
                                }
                                else
                                {
                                    chatPc.sendPackets(new S_ServerMessage(417)); // 你的隊伍已經滿了，無法再接受隊員。
                                }
                            }
                            else
                            {
                                L1ChatParty chatParty = new L1ChatParty();
                                chatParty.addMember(chatPc);
                                chatParty.addMember(pc);
                                chatPc.sendPackets(new S_ServerMessage(424, pc.Name)); // %0%s加入了您的隊伍。
                            }
                        }
                    }
                    break;

                case 953: // 玩家 %0%s 邀請您加入隊伍？(Y/N)
                    c = ReadH();
                    L1PcInstance target = (L1PcInstance)L1World.Instance.findObject(pc.PartyID);
                    if (target != null)
                    {
                        if (c == 0) // No
                        {
                            target.sendPackets(new S_ServerMessage(423, pc.Name)); // %0%s 拒絕了您的邀請。
                            pc.PartyID = 0;
                        }
                        else if (c == 1) // Yes
                        {
                            if (target.InParty)
                            {
                                // 隊長組隊中
                                if (target.Party.Vacancy || target.Gm)
                                {
                                    // 組隊是空的
                                    target.Party.addMember(pc);
                                }
                                else
                                {
                                    // 組隊滿了
                                    target.sendPackets(new S_ServerMessage(417)); // 你的隊伍已經滿了，無法再接受隊員。
                                }
                            }
                            else
                            {
                                // 還沒有組隊，建立一個新組隊
                                L1Party party = new L1Party();
                                party.addMember(target);
                                party.addMember(pc);
                                target.sendPackets(new S_ServerMessage(424, pc.Name)); // %0%s
                                                                                       // 加入了您的隊伍。
                            }
                        }
                    }
                    break;

                case 954: // 玩家 %0%s 邀請您加入自動分配隊伍？(Y/N)
                    c = ReadH();
                    L1PcInstance target2 = (L1PcInstance)L1World.Instance.findObject(pc.PartyID);
                    if (target2 != null)
                    {
                        if (c == 0)
                        { // No
                            target2.sendPackets(new S_ServerMessage(423, pc.Name)); // %0%s
                                                                                    // 拒絕了您的邀請。
                            pc.PartyID = 0;
                        }
                        else if (c == 1)
                        { // Yes
                            if (target2.InParty)
                            {
                                // 隊長組隊中
                                if (target2.Party.Vacancy || target2.Gm)
                                {
                                    // 組隊是空的
                                    target2.Party.addMember(pc);
                                }
                                else
                                {
                                    // 組隊滿了
                                    target2.sendPackets(new S_ServerMessage(417)); // 你的隊伍已經滿了，無法再接受隊員。
                                }
                            }
                            else
                            {
                                // 還沒有組隊，建立一個新組隊
                                L1Party party = new L1Party();
                                party.addMember(target2);
                                party.addMember(pc);
                                target2.sendPackets(new S_ServerMessage(424, pc.Name)); // %0%s
                                                                                        // 加入了您的隊伍。
                            }
                        }
                    }
                    break;

                case 479: // 提昇能力值？（str、dex、int、con、wis、cha）
                    if (ReadC() == 1)
                    {
                        string s = ReadS();
                        if (!(pc.Level - 50 > pc.BonusStats))
                        {
                            return;
                        }
                        if (s.ToLower().Equals("str".ToLower()))
                        {
                            if (pc.BaseStr < Config.BONUS_STATS1)
                            { // 調整能力值上限
                                pc.addBaseStr((sbyte)1); // 素のSTR値に+1
                                pc.BonusStats = pc.BonusStats + 1;
                                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                                pc.sendPackets(new S_CharVisualUpdate(pc));
                                pc.Save(); // 將玩家資料儲存到資料庫中
                            }
                            else
                            {
                                //TODO 修正提示訊息
                                pc.sendPackets(new S_SystemMessage("屬性最大值只能到" + Config.BONUS_STATS1 + "。 請重試一次。"));
                            }
                        }
                        else if (s.ToLower().Equals("dex".ToLower()))
                        {
                            if (pc.BaseDex < Config.BONUS_STATS1)
                            { // 調整能力值上限
                                pc.addBaseDex((sbyte)1); // 素のDEX値に+1
                                pc.resetBaseAc();
                                pc.BonusStats = pc.BonusStats + 1;
                                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                                pc.sendPackets(new S_CharVisualUpdate(pc));
                                pc.Save(); // 將玩家資料儲存到資料庫中
                            }
                            else
                            {
                                pc.sendPackets(new S_SystemMessage("屬性最大值只能到" + Config.BONUS_STATS1 + "。 請重試一次。"));
                            }
                        }
                        else if (s.ToLower().Equals("con".ToLower()))
                        {
                            if (pc.BaseCon < Config.BONUS_STATS1)
                            { // 調整能力值上限
                                pc.addBaseCon((sbyte)1); // 素のCON値に+1
                                pc.BonusStats = pc.BonusStats + 1;
                                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                                pc.sendPackets(new S_CharVisualUpdate(pc));
                                pc.Save(); // 將玩家資料儲存到資料庫中
                            }
                            else
                            {
                                pc.sendPackets(new S_SystemMessage("屬性最大值只能到" + Config.BONUS_STATS1 + "。 請重試一次。"));
                            }
                        }
                        else if (s.ToLower().Equals("int".ToLower()))
                        {
                            if (pc.BaseInt < Config.BONUS_STATS1)
                            { // 調整能力值上限
                                pc.addBaseInt((sbyte)1); // 素のINT値に+1
                                pc.BonusStats = pc.BonusStats + 1;
                                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                                pc.sendPackets(new S_CharVisualUpdate(pc));
                                pc.Save(); // 將玩家資料儲存到資料庫中
                            }
                            else
                            {
                                pc.sendPackets(new S_SystemMessage("屬性最大值只能到" + Config.BONUS_STATS1 + "。 請重試一次。"));
                            }
                        }
                        else if (s.ToLower().Equals("wis".ToLower()))
                        {
                            if (pc.BaseWis < Config.BONUS_STATS1)
                            { // 調整能力值上限
                                pc.addBaseWis((sbyte)1); // 素のWIS値に+1
                                pc.resetBaseMr();
                                pc.BonusStats = pc.BonusStats + 1;
                                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                                pc.sendPackets(new S_CharVisualUpdate(pc));
                                pc.Save(); // 將玩家資料儲存到資料庫中
                            }
                            else
                            {
                                pc.sendPackets(new S_SystemMessage("屬性最大值只能到" + Config.BONUS_STATS1 + "。 請重試一次。"));
                            }
                        }
                        else if (s.ToLower().Equals("cha".ToLower()))
                        {
                            if (pc.BaseCha < Config.BONUS_STATS1)
                            { // 調整能力值上限
                                pc.addBaseCha((sbyte)1); // 素のCHA値に+1
                                pc.BonusStats = pc.BonusStats + 1;
                                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                                pc.sendPackets(new S_CharVisualUpdate(pc));
                                pc.Save(); // 將玩家資料儲存到資料庫中
                            }
                            else
                            {
                                pc.sendPackets(new S_SystemMessage("屬性最大值只能到" + Config.BONUS_STATS1 + "。 請重試一次。"));
                            }
                        }
                    }
                    break;
                case 1256: // 寵物競速 預約名單回應
                    LineageServer.Server.Model.Game.L1PolyRace.Instance.requsetAttr(pc, ReadC());
                    break;
                default:
                    break;
            }
        }

        private void resurrection(L1PcInstance pc, L1PcInstance resusepc, int resHp)
        {
            // 由其他角色復活
            pc.sendPackets(new S_SkillSound(pc.Id, '\x00E6'));
            pc.broadcastPacket(new S_SkillSound(pc.Id, '\x00E6'));
            pc.resurrect(resHp);
            pc.CurrentHp = resHp;
            pc.startHpRegeneration();
            pc.startMpRegeneration();
            pc.startHpRegenerationByDoll();
            pc.startMpRegenerationByDoll();
            pc.stopPcDeleteTimer();
            pc.sendPackets(new S_Resurrection(pc, resusepc, 0));
            pc.broadcastPacket(new S_Resurrection(pc, resusepc, 0));
            pc.sendPackets(new S_CharVisualUpdate(pc)); // 3.80C可能已經不需要
            pc.broadcastPacket(new S_CharVisualUpdate(pc)); // 3.80C可能已經不需要
        }

        private void changeClan(ClientThread clientthread, L1PcInstance pc, L1PcInstance joinPc, int maxMember)
        {
            int clanId = pc.Clanid;
            string clanName = pc.Clanname;
            L1Clan clan = L1World.Instance.getClan(clanName);

            string oldClanName = joinPc.Clanname;
            L1Clan oldClan = L1World.Instance.getClan(oldClanName);

            if ((clan != null) && (oldClan != null) && joinPc.Crown && (joinPc.Id == oldClan.LeaderId))
            {
                if (maxMember < clan.AllMembers.Length + oldClan.AllMembers.Length)
                { // 沒有空缺
                    joinPc.sendPackets(new S_ServerMessage(188, pc.Name));
                    return;
                }

                foreach (L1PcInstance element in clan.OnlineClanMember)
                {
                    element.sendPackets(new S_ServerMessage(94, joinPc.Name)); // \f1你接受%0當你的血盟成員。
                }

                /// <summary>
                /// 變更為聯盟王 </summary>
                pc.ClanRank = L1Clan.CLAN_RANK_LEAGUE_PRINCE;
                pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_LEAGUE_PRINCE, pc.Name)); // 你的階級變更為
                try
                {
                    pc.Save();
                }
                catch (Exception e1)
                {
                    System.Console.WriteLine(e1.ToString());
                    System.Console.Write(e1.StackTrace);
                }

                foreach (string element in oldClan.AllMembers)
                {
                    L1PcInstance oldClanMember = L1World.Instance.getPlayer(element);
                    if (oldClanMember != null)
                    { // 舊血盟成員在線上
                        ClanMembersTable.Instance.deleteMember(oldClanMember.Id);
                        oldClanMember.Clanid = clanId;
                        oldClanMember.Clanname = clanName;
                        oldClanMember.ClanRank = L1Clan.CLAN_RANK_LEAGUE_PUBLIC;
                        try
                        {
                            // 儲存玩家資料到資料庫中
                            oldClanMember.Save();
                        }
                        catch (Exception e)
                        {
                            // _log.log(Enum.Level.Server, e.Message, e);
                        }
                        clan.addMemberName(oldClanMember.Name);
                        ClanMembersTable.Instance.newMember(oldClanMember); // 加入成員資料
                        oldClanMember.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_PUBLIC, oldClanMember.Name)); // 你的階級變更為
                        oldClanMember.sendPackets(new S_ServerMessage(95, clanName)); // \f1加入%0血盟。
                        oldClanMember.sendPackets(new S_ClanName(oldClanMember, true));
                        oldClanMember.sendPackets(new S_CharReset(oldClanMember.Id, clan.ClanId));
                        oldClanMember.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, pc.Clan.EmblemStatus));
                        oldClanMember.sendPackets(new S_ClanAttention());
                        foreach (L1PcInstance player in clan.OnlineClanMember)
                        {
                            player.sendPackets(new S_CharReset(oldClanMember.Id, oldClanMember.Clan.EmblemId));
                            player.broadcastPacket(new S_CharReset(player.Id, oldClanMember.Clan.EmblemId));
                        }
                    }
                    else
                    { // 舊血盟成員不在線上
                        try
                        {
                            L1PcInstance offClanMember = CharacterTable.Instance.restoreCharacter(element);
                            ClanMembersTable.Instance.deleteMember(offClanMember.Id);
                            offClanMember.Clanid = clanId;
                            offClanMember.Clanname = clanName;
                            offClanMember.ClanRank = L1Clan.CLAN_RANK_LEAGUE_PUBLIC;
                            offClanMember.Save(); // 儲存玩家資料到資料庫中
                            clan.addMemberName(offClanMember.Name);
                            ClanMembersTable.Instance.newMember(offClanMember); // 加入成員資料
                        }
                        catch (Exception e)
                        {
                            //_log.log(Enum.Level.Server, e.Message, e);
                        }
                    }
                }
                // 刪除舊盟徽
                string emblem_file = oldClan.EmblemId.ToString();
                string fileFullName = "emblem/" + emblem_file;
                if (File.Exists(fileFullName))
                {
                    File.Delete(fileFullName);
                }

                ClanTable.Instance.deleteClan(oldClanName);
            }
        }

        private static void renamePet(L1PetInstance pet, string name)
        {
            if ((pet == null) || (string.ReferenceEquals(name, null)))
            {
                throw new System.NullReferenceException();
            }

            int petItemObjId = pet.ItemObjId;
            L1Pet petTemplate = PetTable.Instance.getTemplate(petItemObjId);
            if (petTemplate == null)
            {
                throw new System.NullReferenceException();
            }

            L1PcInstance pc = (L1PcInstance)pet.Master;
            if (PetTable.isNameExists(name))
            {
                pc.sendPackets(new S_ServerMessage(327)); // 同樣的名稱已經存在。
                return;
            }
            L1Npc l1npc = NpcTable.Instance.getTemplate(pet.NpcId);
            if (!(pet.Name == l1npc.get_name()))
            {
                pc.sendPackets(new S_ServerMessage(326)); // 一旦你已決定就不能再變更。
                return;
            }
            pet.Name = name;
            petTemplate.set_name(name);
            PetTable.Instance.storePet(petTemplate); // 儲存寵物資料到資料庫中
            L1ItemInstance item = pc.Inventory.getItem(pet.ItemObjId);
            pc.Inventory.updateItem(item);
            pc.sendPackets(new S_ChangeName(pet.Id, name));
            pc.broadcastPacket(new S_ChangeName(pet.Id, name));
        }

        private void callClan(L1PcInstance pc)
        {
            L1PcInstance callClanPc = (L1PcInstance)L1World.Instance.findObject(pc.TempID);
            pc.TempID = 0;
            if (callClanPc == null)
            {
                return;
            }
            if (!pc.Map.Escapable && !pc.Gm)
            {
                // 這附近的能量影響到瞬間移動。在此地無法使用瞬間移動。
                pc.sendPackets(new S_ServerMessage(647));
                L1Teleport.teleport(pc, pc.Location, pc.Heading, false);
                return;
            }
            if (pc.Id != callClanPc.CallClanId)
            {
                return;
            }

            bool isInWarArea = false;
            int castleId = L1CastleLocation.getCastleIdByArea(callClanPc);
            if (castleId != 0)
            {
                isInWarArea = true;
                if (WarTimeController.Instance.isNowWar(castleId))
                {
                    isInWarArea = false; // 戰爭也可以在時間的旗
                }
            }
            short mapId = callClanPc.MapId;
            if (((mapId != 0) && (mapId != 4) && (mapId != 304)) || isInWarArea)
            {
                // 沒有任何事情發生。
                pc.sendPackets(new S_ServerMessage(79));
                return;
            }

            L1Map map = callClanPc.Map;
            int locX = callClanPc.X;
            int locY = callClanPc.Y;
            int heading = callClanPc.CallClanHeading;
            locX += HEADING_TABLE_X[heading];
            locY += HEADING_TABLE_Y[heading];
            heading = (heading + 4) % 4;

            bool isExsistCharacter = false;
            foreach (GameObject @object in L1World.Instance.getVisibleObjects(callClanPc, 1))
            {
                if (@object is L1Character)
                {
                    L1Character cha = (L1Character)@object;
                    if ((cha.X == locX) && (cha.Y == locY) && (cha.MapId == mapId))
                    {
                        isExsistCharacter = true;
                        break;
                    }
                }
            }

            if (((locX == 0) && (locY == 0)) || !map.isPassable(locX, locY) || isExsistCharacter)
            {
                // 因你要去的地方有障礙物以致於無法直接傳送到該處。
                pc.sendPackets(new S_ServerMessage(627));
                return;
            }
            L1Teleport.teleport(pc, locX, locY, mapId, heading, true, L1Teleport.CALL_CLAN);
        }

        public override string Type
        {
            get
            {
                return C_ATTR;
            }
        }
    }
}