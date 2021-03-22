using LineageServer.Interfaces;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來登入到伺服器的封包
    /// </summary>
    class C_LoginToServer : ClientBasePacket
    {
        private const string C_LOGIN_TO_SERVER = "[C] C_LoginToServer";
        private static ILogger _log = Logger.getLogger(nameof(C_LoginToServer));
        public C_LoginToServer(sbyte[] abyte0, ClientThread client) : base(abyte0)
        {

            string login = client.AccountName;

            string charName = readS();

            if (client.ActiveChar != null)
            {
                _log.info("同一個角色重複登入，強制切斷 " + client.Hostname + ") 的連結");
                client.close();
                return;
            }

            L1PcInstance pc = L1PcInstance.load(charName);
            Account account = Account.load(pc.AccountName);
            if (account.OnlineStatus)
            {
                _log.info("同一個帳號雙重角色登入，強制切斷 " + client.Hostname + ") 的連結");
                client.close();
                return;
            }
            if ((pc == null) || !login.Equals(pc.AccountName))
            {
                _log.info("無效的角色名稱: char=" + charName + " account=" + login + " host=" + client.Hostname);
                client.close();
                return;
            }

            if (Config.LEVEL_DOWN_RANGE != 0)
            {
                if (pc.HighLevel - pc.Level >= Config.LEVEL_DOWN_RANGE)
                {
                    _log.info("登錄請求超出了容忍的等級下降的角色: char=" + charName + " account=" + login + " host=" + client.Hostname);
                    client.kick();
                    return;
                }
            }

            _log.info("角色登入到伺服器中: char=" + charName + " account=" + login + " host=" + client.Hostname);

            int currentHpAtLoad = pc.CurrentHp;
            int currentMpAtLoad = pc.CurrentMp;
            pc.clearSkillMastery();
            pc.OnlineStatus = 1;
            CharacterTable.updateOnlineStatus(pc);
            L1World.Instance.storeObject(pc);

            pc.NetConnection = client;
            pc.PacketOutput = client;
            client.ActiveChar = pc;

            pc.sendPackets(new S_LoginGame(pc));

            Account.OnlineStatus(account, true); // OnlineStatus = 1

            // 如果設定檔中設定自動回村的話
            GetBackRestartTable gbrTable = GetBackRestartTable.Instance;
            L1GetBackRestart[] gbrList = gbrTable.GetBackRestartTableList;
            foreach (L1GetBackRestart gbr in gbrList)
            {
                if (pc.MapId == gbr.Area)
                {
                    pc.X = gbr.LocX;
                    pc.Y = gbr.LocY;
                    pc.Map = L1WorldMap.Instance.getMap(gbr.MapId);
                    break;
                }
            }

            // altsettings.properties 中 GetBack 設定為 true 就自動回村
            if (Config.GET_BACK)
            {
                int[] loc = Getback.GetBack_Location(pc, true);
                pc.X = loc[0];
                pc.Y = loc[1];
                pc.Map = L1WorldMap.Instance.getMap((short)loc[2]);
            }

            // 如果標記是在戰爭期間，如果不是血盟成員回到城堡。
            int castle_id = L1CastleLocation.getCastleIdByArea(pc);
            if (0 < castle_id)
            {
                if (WarTimeController.Instance.isNowWar(castle_id))
                {
                    L1Clan clan = L1World.Instance.getClan(pc.Clanname);
                    if (clan != null)
                    {
                        if (clan.CastleId != castle_id)
                        {
                            // 沒有城堡
                            int[] loc = new int[3];
                            loc = L1CastleLocation.getGetBackLoc(castle_id);
                            pc.X = loc[0];
                            pc.Y = loc[1];
                            pc.Map = L1WorldMap.Instance.getMap((short)loc[2]);
                        }
                    }
                    else
                    {
                        // 有城堡就回到城堡
                        int[] loc = new int[3];
                        loc = L1CastleLocation.getGetBackLoc(castle_id);
                        pc.X = loc[0];
                        pc.Y = loc[1];
                        pc.Map = L1WorldMap.Instance.getMap((short)loc[2]);
                    }
                }
            }
            L1World.Instance.addVisibleObject(pc);

            if (Config.CHARACTER_CONFIG_IN_SERVER_SIDE)
            {
                pc.sendPackets(new S_CharacterConfig(pc.Id));
            }

            items(pc);

            pc.sendPackets(new S_Mail(pc, 0));
            pc.sendPackets(new S_Mail(pc, 1));
            pc.sendPackets(new S_Mail(pc, 2));

            pc.beginGameTimeCarrier();

            pc.sendPackets(new S_RuneSlot(S_RuneSlot.RUNE_CLOSE_SLOT, 3)); // 符文關閉欄位數
            pc.sendPackets(new S_RuneSlot(S_RuneSlot.RUNE_OPEN_SLOT, 1)); // 符文開放欄位數

            pc.setEquipped(pc, true); //3.63

            pc.sendPackets(new S_ActiveSpells(pc));

            pc.sendPackets(new S_Bookmarks(pc));

            pc.sendPackets(new S_OwnCharStatus(pc));

            pc.sendPackets(new S_MapID(pc.MapId, pc.Map.Underwater));

            pc.sendPackets(new S_OwnCharPack(pc));

            pc.sendPackets(new S_SPMR(pc));

            S_CharTitle s_charTitle = new S_CharTitle(pc.Id, pc.Title);
            pc.sendPackets(s_charTitle);
            pc.broadcastPacket(s_charTitle);

            pc.sendVisualEffectAtLogin(); // 皇冠，毒，水和其他視覺效果顯示

            pc.sendPackets(new S_OwnCharStatus2(pc, 1)); // 角色初始素質

            pc.sendPackets(new S_InitialAbilityGrowth(pc)); // 角色狀態獎勵

            pc.sendPackets(new S_Weather(L1World.Instance.Weather));

            skills(pc);

            buff(client, pc);

            pc.turnOnOffLight();
            // TODO 殷海薩的祝福
            int ainOutTime = Config.RATE_AIN_OUTTIME;
            int ainMaxPercent = Config.RATE_MAX_CHARGE_PERCENT;

            if (pc.Level >= 49)
            { // TODO 49級以上 殷海薩的祝福紀錄
                if (pc.Map.isSafetyZone(pc.Location))
                {
                    pc.AinZone = 1;
                }
                else
                {
                    pc.AinZone = 0;
                }

                if (pc.AinPoint >= 1)
                {
                    pc.sendPackets(new S_SkillIconExp(pc.AinPoint)); // TODO 角色登入時點數大於1則送出
                }
                if (pc.AinZone == 1)
                {
                    DateTime cal = DateTime.Now;
                    long startTime = (cal - pc.LastActive).Ticks / 60000;

                    if (startTime >= ainOutTime)
                    {
                        long outTime = startTime / ainOutTime;
                        long saveTime = outTime + pc.AinPoint;
                        if (saveTime >= 1 && saveTime <= ainMaxPercent)
                        {
                            pc.AinPoint = (int)saveTime;
                        }
                        else if (saveTime > ainMaxPercent)
                        {
                            pc.AinPoint = ainMaxPercent;
                        }
                    }
                }
            }
            pc.sendPackets(new S_Karma(pc)); // 友好度

            pc.sendPackets(new S_PacketBox(S_PacketBox.DODGE_RATE_PLUS, pc.Dodge)); // 閃避率 正
            pc.sendPackets(new S_PacketBox(S_PacketBox.DODGE_RATE_MINUS, pc.Ndodge)); // 閃避率 負

            CheckPledgeRecommendation(pc);

            if (pc.CurrentHp > 0)
            {
                pc.Dead = false;
                pc.Status = 0;
            }
            else
            {
                pc.Dead = true;
                pc.Status = ActionCodes.ACTION_Die;
            }
            SerchSummon(pc);

            WarTimeController.Instance.checkCastleWar(pc);

            if (pc.Clanid != 0)
            { // 有血盟
                L1Clan clan = L1World.Instance.getClan(pc.Clanname);
                if (clan != null)
                {
                    if ((pc.Clanid == clan.ClanId) && pc.Clanname.ToLower().Equals(clan.ClanName.ToLower()))
                    {
                        L1PcInstance[] clanMembers = clan.OnlineClanMember;
                        foreach (L1PcInstance clanMember in clanMembers)
                        {
                            if (clanMember.Id != pc.Id)
                            {
                                clanMember.sendPackets(new S_ServerMessage(843, pc.Name)); // 只今、血盟員の%0%sがゲームに接続しました。
                            }
                        }

                        // 取得所有的盟戰
                        foreach (L1War war in L1World.Instance.WarList)
                        {
                            bool ret = war.CheckClanInWar(pc.Clanname);
                            if (ret)
                            { // 盟戰中
                                string enemy_clan_name = war.GetEnemyClanName(pc.Clanname);
                                if (!string.ReferenceEquals(enemy_clan_name, null))
                                {
                                    // あなたの血盟が現在_血盟と交戦中です。
                                    pc.sendPackets(new S_War(8, pc.Clanname, enemy_clan_name));
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        pc.Clanid = 0;
                        pc.Clanname = "";
                        pc.ClanRank = 0;
                        pc.Save(); // 儲存玩家的資料到資料庫中
                    }
                }
            }

            if (pc.PartnerId != 0)
            { // 結婚中
                L1PcInstance partner = (L1PcInstance)L1World.Instance.findObject(pc.PartnerId);
                if ((partner != null) && (partner.PartnerId != 0))
                {
                    if ((pc.PartnerId == partner.Id) && (partner.PartnerId == pc.Id))
                    {
                        pc.sendPackets(new S_ServerMessage(548)); // あなたのパートナーは今ゲーム中です。
                        partner.sendPackets(new S_ServerMessage(549)); // あなたのパートナーはたった今ログインしました。
                    }
                }
            }

            if (currentHpAtLoad > pc.CurrentHp)
            {
                pc.CurrentHp = currentHpAtLoad;
            }
            if (currentMpAtLoad > pc.CurrentMp)
            {
                pc.CurrentMp = currentMpAtLoad;
            }
            pc.startHpRegeneration();
            pc.startMpRegeneration();
            pc.startObjectAutoUpdate();
            pc.setCryOfSurvivalTime();
            client.CharReStart(false);
            pc.beginExpMonitor();
            pc.Save(); // 儲存玩家的資料到資料庫中

            if (pc.HellTime > 0)
            {
                pc.beginHell(false);
            }

            // 處理新手保護系統(遭遇的守護)狀態資料的變動
            pc.checkNoviceType();
        }

        private void items(L1PcInstance pc)
        {
            // 從資料庫中讀取角色的道具
            CharacterTable.Instance.restoreInventory(pc);

            pc.sendPackets(new S_InvList(pc.Inventory.Items));
        }

        private void skills(L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {

                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM character_skills WHERE char_obj_id=?");
                pstm.setInt(1, pc.Id);
                rs = pstm.executeQuery();
                int i = 0;
                int lv1 = 0;
                int lv2 = 0;
                int lv3 = 0;
                int lv4 = 0;
                int lv5 = 0;
                int lv6 = 0;
                int lv7 = 0;
                int lv8 = 0;
                int lv9 = 0;
                int lv10 = 0;
                int lv11 = 0;
                int lv12 = 0;
                int lv13 = 0;
                int lv14 = 0;
                int lv15 = 0;
                int lv16 = 0;
                int lv17 = 0;
                int lv18 = 0;
                int lv19 = 0;
                int lv20 = 0;
                int lv21 = 0;
                int lv22 = 0;
                int lv23 = 0;
                int lv24 = 0;
                int lv25 = 0;
                int lv26 = 0;
                int lv27 = 0;
                int lv28 = 0;
                while (rs.next())
                {
                    int skillId = rs.getInt("skill_id");
                    L1Skills l1skills = SkillsTable.Instance.getTemplate(skillId);
                    if (l1skills.SkillLevel == 1)
                    {
                        lv1 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 2)
                    {
                        lv2 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 3)
                    {
                        lv3 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 4)
                    {
                        lv4 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 5)
                    {
                        lv5 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 6)
                    {
                        lv6 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 7)
                    {
                        lv7 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 8)
                    {
                        lv8 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 9)
                    {
                        lv9 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 10)
                    {
                        lv10 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 11)
                    {
                        lv11 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 12)
                    {
                        lv12 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 13)
                    {
                        lv13 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 14)
                    {
                        lv14 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 15)
                    {
                        lv15 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 16)
                    {
                        lv16 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 17)
                    {
                        lv17 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 18)
                    {
                        lv18 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 19)
                    {
                        lv19 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 20)
                    {
                        lv20 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 21)
                    {
                        lv21 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 22)
                    {
                        lv22 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 23)
                    {
                        lv23 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 24)
                    {
                        lv24 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 25)
                    {
                        lv25 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 26)
                    {
                        lv26 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 27)
                    {
                        lv27 |= l1skills.Id;
                    }
                    if (l1skills.SkillLevel == 28)
                    {
                        lv28 |= l1skills.Id;
                    }
                    i = lv1 + lv2 + lv3 + lv4 + lv5 + lv6 + lv7 + lv8 + lv9 + lv10 + lv11 + lv12 + lv13 + lv14 + lv15 + lv16 + lv17 + lv18 + lv19 + lv20 + lv21 + lv22 + lv23 + lv24 + lv25 + lv26 + lv27 + lv28;
                    pc.SkillMastery = skillId;
                }
                if (i > 0)
                {
                    pc.sendPackets(new S_AddSkill(lv1, lv2, lv3, lv4, lv5, lv6, lv7, lv8, lv9, lv10, lv11, lv12, lv13, lv14, lv15, lv16, lv17, lv18, lv19, lv20, lv21, lv22, lv23, lv24, lv25, lv26, lv27, lv28));
                }
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

        private void SerchSummon(L1PcInstance pc)
        {
            foreach (L1SummonInstance summon in L1World.Instance.AllSummons)
            {
                if (summon.Master.Id == pc.Id)
                {
                    summon.Master = pc;
                    pc.addPet(summon);
                    foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(summon))
                    {
                        visiblePc.sendPackets(new S_SummonPack(summon, visiblePc));
                    }
                }
            }
        }

        private void buff(ClientThread clientthread, L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {

                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM character_buff WHERE char_obj_id=?");
                pstm.setInt(1, pc.Id);
                rs = pstm.executeQuery();
                while (rs.next())
                {
                    int skillid = rs.getInt("skill_id");
                    int remaining_time = rs.getInt("remaining_time");
                    int time = 0;
                    switch (skillid)
                    {
                        case L1SkillId.SHAPE_CHANGE: // 變身
                            int poly_id = rs.getInt("poly_id");
                            L1PolyMorph.doPoly(pc, poly_id, remaining_time, L1PolyMorph.MORPH_BY_LOGIN);
                            break;
                        case L1SkillId.STATUS_BRAVE: // 勇敢藥水
                            pc.sendPackets(new S_SkillBrave(pc.Id, 1, remaining_time));
                            pc.broadcastPacket(new S_SkillBrave(pc.Id, 1, 0));
                            pc.BraveSpeed = 1;
                            pc.setSkillEffect(skillid, remaining_time * 1000);
                            break;
                        case L1SkillId.STATUS_ELFBRAVE: // 精靈餅乾
                            pc.sendPackets(new S_SkillBrave(pc.Id, 3, remaining_time));
                            pc.broadcastPacket(new S_SkillBrave(pc.Id, 3, 0));
                            pc.BraveSpeed = 3;
                            pc.setSkillEffect(skillid, remaining_time * 1000);
                            break;
                        case L1SkillId.STATUS_BRAVE2: // 超級加速
                            pc.sendPackets(new S_SkillBrave(pc.Id, 5, remaining_time));
                            pc.broadcastPacket(new S_SkillBrave(pc.Id, 5, 0));
                            pc.BraveSpeed = 5;
                            pc.setSkillEffect(skillid, remaining_time * 1000);
                            break;
                        case L1SkillId.STATUS_HASTE: // 加速
                            pc.sendPackets(new S_SkillHaste(pc.Id, 1, remaining_time));
                            pc.broadcastPacket(new S_SkillHaste(pc.Id, 1, 0));
                            pc.MoveSpeed = 1;
                            pc.setSkillEffect(skillid, remaining_time * 1000);
                            break;
                        case L1SkillId.STATUS_BLUE_POTION: // 藍色藥水
                            pc.sendPackets(new S_SkillIconGFX(34, remaining_time));
                            pc.setSkillEffect(skillid, remaining_time * 1000);
                            break;
                        case L1SkillId.STATUS_CHAT_PROHIBITED: // 禁言
                            pc.sendPackets(new S_SkillIconGFX(36, remaining_time));
                            pc.setSkillEffect(skillid, remaining_time * 1000);
                            break;
                        case L1SkillId.STATUS_THIRD_SPEED: // 三段加速
                            time = remaining_time / 4;
                            pc.sendPackets(new S_Liquor(pc.Id, 8)); // 人物 *
                                                                    // 1.15
                            pc.broadcastPacket(new S_Liquor(pc.Id, 8)); // 人物 *
                                                                        // 1.15
                            pc.sendPackets(new S_SkillIconThirdSpeed(time));
                            pc.setSkillEffect(skillid, time * 4 * 1000);
                            break;
                        case L1SkillId.MIRROR_IMAGE: // 鏡像
                        case L1SkillId.UNCANNY_DODGE: // 暗影閃避
                            time = remaining_time / 16;
                            pc.addDodge((sbyte)5); // 閃避率 + 50%
                                                   // 更新閃避率顯示
                            pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                            pc.sendPackets(new S_PacketBox(21, time));
                            pc.setSkillEffect(skillid, time * 16 * 1000);
                            break;
                        case L1SkillId.EFFECT_BLOODSTAIN_OF_ANTHARAS: // 安塔瑞斯的血痕
                            remaining_time = remaining_time / 60;
                            if (remaining_time != 0)
                            {
                                L1BuffUtil.bloodstain(pc, (sbyte)0, remaining_time, false);
                            }
                            break;
                        case L1SkillId.EFFECT_BLOODSTAIN_OF_FAFURION: // 法利昂的血痕
                            remaining_time = remaining_time / 60;
                            if (remaining_time != 0)
                            {
                                L1BuffUtil.bloodstain(pc, (sbyte)1, remaining_time, false);
                            }
                            break;
                        default:
                            // 魔法料理
                            if (((skillid >= L1SkillId.COOKING_1_0_N) && (skillid <= L1SkillId.COOKING_1_6_N)) || ((skillid >= L1SkillId.COOKING_1_0_S) && (skillid <= L1SkillId.COOKING_1_6_S)) || ((skillid >= L1SkillId.COOKING_2_0_N) && (skillid <= L1SkillId.COOKING_2_6_N)) || ((skillid >= L1SkillId.COOKING_2_0_S) && (skillid <= L1SkillId.COOKING_2_6_S)) || ((skillid >= L1SkillId.COOKING_3_0_N) && (skillid <= L1SkillId.COOKING_3_6_N)) || ((skillid >= L1SkillId.COOKING_3_0_S) && (skillid <= L1SkillId.COOKING_3_6_S)))
                            {
                                L1Cooking.eatCooking(pc, skillid, remaining_time);
                            }
                            // 生命之樹果實、商城道具
                            else if (skillid == L1SkillId.STATUS_RIBRAVE || (skillid >= L1SkillId.EFFECT_BEGIN && skillid <= L1SkillId.EFFECT_END) || skillid == L1SkillId.COOKING_WONDER_DRUG)
                            {
                                ;
                            }
                            else
                            {
                                L1SkillUse l1skilluse = new L1SkillUse();
                                l1skilluse.handleCommands(clientthread.ActiveChar, skillid, pc.Id, pc.X, pc.Y, null, remaining_time, L1SkillUse.TYPE_LOGIN);
                            }
                            break;
                    }
                }
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

        private void CheckPledgeRecommendation(L1PcInstance pc)
        {
            if (pc.Clanid > 0)
            {
                pc.sendPackets(new S_ClanAttention());
                pc.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, pc.Clan.EmblemStatus));
                if (pc.ClanRank == L1Clan.CLAN_RANK_PRINCE || pc.ClanRank == L1Clan.CLAN_RANK_GUARDIAN || pc.ClanRank == L1Clan.CLAN_RANK_LEAGUE_GUARDIAN || pc.ClanRank == L1Clan.CLAN_RANK_LEAGUE_VICEPRINCE || pc.ClanRank == L1Clan.CLAN_RANK_LEAGUE_PRINCE)
                {
                    // 有登錄，結束
                    if (ClanRecommendTable.Instance.isRecorded(pc.Clanid))
                    {
                        // 是否有人申請
                        if (ClanRecommendTable.Instance.isClanApplyByPlayer(pc.Clanid))
                        {
                            pc.sendPackets(new S_ServerMessage(3248));
                        }
                    }
                    else
                    {
                        // 無登錄
                        pc.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, pc.Clan.EmblemStatus));
                        pc.sendPackets(new S_ClanAttention());
                        pc.sendPackets(new S_ServerMessage(3246));
                    }
                }
            }
            else
            {
                if (pc.Crown)
                {
                    pc.sendPackets(new S_ServerMessage(3247));
                }
                else
                {
                    // 如果有登記 不發送
                    if (ClanRecommendTable.Instance.isApplied(pc.Name))
                    {

                    }
                    else
                    {
                        // 沒有登記
                        pc.sendPackets(new S_ServerMessage(3245));
                    }
                }
            }

        }


        public override string Type
        {
            get
            {
                return C_LOGIN_TO_SERVER;
            }
        }
    }

}