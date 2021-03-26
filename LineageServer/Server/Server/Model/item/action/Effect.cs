using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Model.item.action
{
    class Effect
    {

        public static void useEffectItem(L1PcInstance pc, L1ItemInstance item)
        {
            bool isMagicStone = false;
            bool deteleItem = true;

            if (pc.hasSkillEffect(L1SkillId.DECAY_POTION))
            { // 藥水霜化術狀態
                pc.sendPackets(new S_ServerMessage(698)); // 喉嚨灼熱，無法喝東西。
                return;
            }

            int itemId = item.Item.ItemId;
            int skillId = 0;
            int time = 0;
            int gfxid = 0;
            switch (itemId)
            {
                case L1ItemId.POTION_OF_EXP_150: // 150%神力藥水
                case L1ItemId.POTION_OF_EXP_175: // 175%神力藥水
                case L1ItemId.POTION_OF_EXP_200: // 200%神力藥水
                case L1ItemId.POTION_OF_EXP_225: // 225%神力藥水
                case L1ItemId.POTION_OF_EXP_250: // 250%神力藥水
                    skillId = itemId - 42999;
                    time = 900;
                    gfxid = itemId - 39699;
                    deleteRepeatedSkills(pc, skillId); // 與戰鬥藥水等相衝
                    pc.sendPackets(new S_ServerMessage(1292)); // 狩獵的經驗值將會增加。
                    break;
                case L1ItemId.BLESS_OF_MAZU: // 媽祖祝福平安符
                    skillId = L1SkillId.EFFECT_BLESS_OF_MAZU;
                    time = 2400;
                    gfxid = 7321;
                    deleteRepeatedSkills(pc, skillId); // 與妖精屬性魔法相衝！
                    break;
                case L1ItemId.POTION_OF_BATTLE: // 戰鬥藥水
                    skillId = L1SkillId.EFFECT_POTION_OF_BATTLE;
                    time = 3600;
                    gfxid = 7013;
                    deleteRepeatedSkills(pc, skillId); // 與神力藥水等相衝
                    break;
                case L1ItemId.SCROLL_FOR_STRENGTHENING_HP: // 體力增強卷軸
                case L1ItemId.SCROLL_FOR_STRENGTHENING_MP: // 魔力增強卷軸
                case L1ItemId.SCROLL_FOR_ENCHANTING_BATTLE: // 強化戰鬥卷軸
                    skillId = itemId - 42999;
                    time = 3600;
                    gfxid = itemId - 40014;
                    deleteRepeatedSkills(pc, skillId);
                    break;
                case 47017: // 地龍之魔眼
                case 47018: // 水龍之魔眼
                case 47019: // 風龍之魔眼
                case 47020: // 火龍之魔眼
                case 47021: // 誕生之魔眼
                case 47022: // 形象之魔眼
                case 47023: // 生命之魔眼
                    skillId = itemId - 42968;
                    time = 600;
                    gfxid = itemId - 39346; // gfxid = 7671 ~ 7676、7678
                    if (itemId == 47023)
                    {
                        gfxid = 7678;
                    }
                    deteleItem = false;
                    deleteRepeatedSkills(pc, skillId);
                    break;
                default:
                    if (itemId >= 47064 && itemId <= 47072)
                    { // 附魔石(近戰)
                        skillId = itemId - 43051;
                        gfxid = itemId - 38125;
                        time = 600;
                        isMagicStone = true;
                        deleteRepeatedSkills(pc, skillId); // 附魔石不可共存
                    }
                    else if (itemId >= 47074 && itemId <= 47082)
                    { // 附魔石(遠攻)
                        skillId = itemId - 43052;
                        gfxid = itemId - 38126;
                        time = 600;
                        isMagicStone = true;
                        deleteRepeatedSkills(pc, skillId); // 附魔石不可共存
                    }
                    else if (itemId >= 47084 && itemId <= 47092)
                    { // 附魔石(恢復)
                        skillId = itemId - 43053;
                        gfxid = itemId - 38127;
                        time = 600;
                        isMagicStone = true;
                        deleteRepeatedSkills(pc, skillId); // 附魔石不可共存
                    }
                    else if (itemId >= 47094 && itemId <= 47102)
                    { // 附魔石(防禦)
                        skillId = itemId - 43054;
                        gfxid = itemId - 38128;
                        time = 600;
                        isMagicStone = true;
                        deleteRepeatedSkills(pc, skillId); // 附魔石不可共存
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(79)); // 沒有任何事情發生。
                        return;
                    }
                    break;
            }
            pc.sendPackets(new S_SkillSound(pc.Id, gfxid));
            pc.broadcastPacket(new S_SkillSound(pc.Id, gfxid));

            if (isMagicStone)
            {
                magicStoneEffect(pc, skillId, time);

            }
            else
            {
                useEffect(pc, skillId, time);
                if (deteleItem)
                { // 刪除道具
                    pc.Inventory.removeItem(item, 1);
                }
            }
        }

        public static void useEffect(L1PcInstance pc, int skillId, int time)
        {
            if (!pc.hasSkillEffect(skillId))
            {
                switch (skillId)
                {
                    case L1SkillId.EFFECT_BLESS_OF_MAZU: // 媽祖的祝福
                        pc.addHitup(3); // 攻擊成功 +3
                        pc.addDmgup(3); // 額外攻擊點數 +3
                        pc.addMpr(2);
                        break;
                    case L1SkillId.EFFECT_ENCHANTING_BATTLE: // 強化戰鬥卷軸
                        pc.addHitup(3); // 攻擊成功 +3
                        pc.addDmgup(3); // 額外攻擊點數 +3
                        pc.addBowHitup(3); // 遠距離命中率 +3
                        pc.addBowDmgup(3); // 遠距離攻擊力 +3
                        pc.addSp(3); // 魔攻 +3
                        pc.sendPackets(new S_SPMR(pc));
                        break;
                    case L1SkillId.EFFECT_STRENGTHENING_HP: // 體力增強卷軸
                        pc.addMaxHp(50);
                        pc.addHpr(4);
                        pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                        if (pc.InParty)
                        { // 組隊中
                            pc.Party.updateMiniHP(pc);
                        }
                        break;
                    case L1SkillId.EFFECT_STRENGTHENING_MP: // 魔力增強卷軸
                        pc.addMaxMp(40);
                        pc.addMpr(4);
                        pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_AHTHARTS: // 地龍之魔眼
                        pc.addRegistStone(3); // 石化耐性 +3

                        pc.addDodge(1); // 閃避率 + 10%
                                        // 更新閃避率顯示
                        pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_FAFURION: // 水龍之魔眼
                        pc.add_regist_freeze(3); // 寒冰耐性 +3
                                                 // 魔法傷害減免 +50
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_LINDVIOR: // 風龍之魔眼
                        pc.addRegistSleep(3); // 睡眠耐性 +3
                                              // 魔法暴擊率 +1
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_VALAKAS: // 火龍之魔眼
                        pc.addRegistStun(3); // 昏迷耐性 +3
                        pc.addDmgup(2); // 額外攻擊點數 +2
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_BIRTH: // 誕生之魔眼
                        pc.addRegistBlind(3); // 闇黑耐性 +3
                                              // 魔法傷害減免 +50

                        pc.addDodge(1); // 閃避率 + 10%
                                        // 更新閃避率顯示
                        pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_FIGURE: // 形象之魔眼
                        pc.addRegistSustain(3); // 支撐耐性 +3
                                                // 魔法傷害減免 +50
                                                // 魔法暴擊率 +1

                        pc.addDodge(1); // 閃避率 + 10%
                                        // 更新閃避率顯示
                        pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_LIFE: // 生命之魔眼
                        pc.addDmgup(2); // 額外攻擊點數 +2
                                        // 魔法傷害減免 +50
                                        // 魔法暴擊率 +1
                                        // 防護中毒狀態

                        pc.addDodge(1); // 閃避率 + 10%
                                        // 更新閃避率顯示
                        pc.sendPackets(new S_PacketBox(88, pc.Dodge));
                        break;
                    default:
                        break;
                }
            }
            pc.setSkillEffect(skillId, time * 1000);
        }

        // 設定不可重複的魔法狀態 
        public static void deleteRepeatedSkills(L1PcInstance pc, int skillId)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int[][] repeatedSkills = { { EFFECT_POTION_OF_EXP_150, EFFECT_POTION_OF_EXP_175, EFFECT_POTION_OF_EXP_200, EFFECT_POTION_OF_EXP_225, EFFECT_POTION_OF_EXP_250, EFFECT_POTION_OF_BATTLE }, { EFFECT_STRENGTHENING_HP, EFFECT_STRENGTHENING_MP, EFFECT_ENCHANTING_BATTLE }, { FIRE_WEAPON, WIND_SHOT, FIRE_BLESS, STORM_EYE, BURNING_WEAPON, STORM_SHOT, EFFECT_BLESS_OF_MAZU }, { EFFECT_MAGIC_STONE_A_1, EFFECT_MAGIC_STONE_A_2, EFFECT_MAGIC_STONE_A_3, EFFECT_MAGIC_STONE_A_4, EFFECT_MAGIC_STONE_A_5, EFFECT_MAGIC_STONE_A_6, EFFECT_MAGIC_STONE_A_7, EFFECT_MAGIC_STONE_A_8, EFFECT_MAGIC_STONE_A_9, EFFECT_MAGIC_STONE_B_1, EFFECT_MAGIC_STONE_B_2, EFFECT_MAGIC_STONE_B_3, EFFECT_MAGIC_STONE_B_4, EFFECT_MAGIC_STONE_B_5, EFFECT_MAGIC_STONE_B_6, EFFECT_MAGIC_STONE_B_7, EFFECT_MAGIC_STONE_B_8, EFFECT_MAGIC_STONE_B_9, EFFECT_MAGIC_STONE_C_1, EFFECT_MAGIC_STONE_C_2, EFFECT_MAGIC_STONE_C_3, EFFECT_MAGIC_STONE_C_4, EFFECT_MAGIC_STONE_C_5, EFFECT_MAGIC_STONE_C_6, EFFECT_MAGIC_STONE_C_7, EFFECT_MAGIC_STONE_C_8, EFFECT_MAGIC_STONE_C_9, EFFECT_MAGIC_STONE_D_1, EFFECT_MAGIC_STONE_D_2, EFFECT_MAGIC_STONE_D_3, EFFECT_MAGIC_STONE_D_4, EFFECT_MAGIC_STONE_D_5, EFFECT_MAGIC_STONE_D_6, EFFECT_MAGIC_STONE_D_7, EFFECT_MAGIC_STONE_D_8, EFFECT_MAGIC_STONE_D_9 }, {EFFECT_MAGIC_EYE_OF_AHTHARTS, EFFECT_MAGIC_EYE_OF_FAFURION, EFFECT_MAGIC_EYE_OF_LINDVIOR, EFFECT_MAGIC_EYE_OF_VALAKAS, EFFECT_MAGIC_EYE_OF_BIRTH, EFFECT_MAGIC_EYE_OF_FIGURE, EFFECT_MAGIC_EYE_OF_LIFE} };
            int[][] repeatedSkills = new int[][]
            {
                new int[] {EFFECT_POTION_OF_EXP_150, EFFECT_POTION_OF_EXP_175, EFFECT_POTION_OF_EXP_200, EFFECT_POTION_OF_EXP_225, EFFECT_POTION_OF_EXP_250, EFFECT_POTION_OF_BATTLE},
                new int[] {EFFECT_STRENGTHENING_HP, EFFECT_STRENGTHENING_MP, EFFECT_ENCHANTING_BATTLE},
                new int[] {FIRE_WEAPON, WIND_SHOT, FIRE_BLESS, STORM_EYE, BURNING_WEAPON, STORM_SHOT, EFFECT_BLESS_OF_MAZU},
                new int[] {EFFECT_MAGIC_STONE_A_1, EFFECT_MAGIC_STONE_A_2, EFFECT_MAGIC_STONE_A_3, EFFECT_MAGIC_STONE_A_4, EFFECT_MAGIC_STONE_A_5, EFFECT_MAGIC_STONE_A_6, EFFECT_MAGIC_STONE_A_7, EFFECT_MAGIC_STONE_A_8, EFFECT_MAGIC_STONE_A_9, EFFECT_MAGIC_STONE_B_1, EFFECT_MAGIC_STONE_B_2, EFFECT_MAGIC_STONE_B_3, EFFECT_MAGIC_STONE_B_4, EFFECT_MAGIC_STONE_B_5, EFFECT_MAGIC_STONE_B_6, EFFECT_MAGIC_STONE_B_7, EFFECT_MAGIC_STONE_B_8, EFFECT_MAGIC_STONE_B_9, EFFECT_MAGIC_STONE_C_1, EFFECT_MAGIC_STONE_C_2, EFFECT_MAGIC_STONE_C_3, EFFECT_MAGIC_STONE_C_4, EFFECT_MAGIC_STONE_C_5, EFFECT_MAGIC_STONE_C_6, EFFECT_MAGIC_STONE_C_7, EFFECT_MAGIC_STONE_C_8, EFFECT_MAGIC_STONE_C_9, EFFECT_MAGIC_STONE_D_1, EFFECT_MAGIC_STONE_D_2, EFFECT_MAGIC_STONE_D_3, EFFECT_MAGIC_STONE_D_4, EFFECT_MAGIC_STONE_D_5, EFFECT_MAGIC_STONE_D_6, EFFECT_MAGIC_STONE_D_7, EFFECT_MAGIC_STONE_D_8, EFFECT_MAGIC_STONE_D_9},
                new int[] {EFFECT_MAGIC_EYE_OF_AHTHARTS, EFFECT_MAGIC_EYE_OF_FAFURION, EFFECT_MAGIC_EYE_OF_LINDVIOR, EFFECT_MAGIC_EYE_OF_VALAKAS, EFFECT_MAGIC_EYE_OF_BIRTH, EFFECT_MAGIC_EYE_OF_FIGURE, EFFECT_MAGIC_EYE_OF_LIFE}
            };

            foreach (int[] skills in repeatedSkills)
            {
                foreach (int id in skills)
                {
                    if (id == skillId)
                    {
                        stopSkillList(pc, skillId, skills);
                    }
                }
            }
        }

        // 將重複的狀態刪除
        private static void stopSkillList(L1PcInstance pc, int _skillId, int[] repeat_skill)
        {
            foreach (int skillId in repeat_skill)
            {
                if (skillId != _skillId)
                {
                    pc.removeSkillEffect(skillId);
                }
            }
        }

        public static void magicStoneEffect(L1PcInstance pc, int skillId, int time)
        {
            sbyte type = 0;
            if (!pc.hasSkillEffect(skillId))
            {
                switch (skillId)
                {
                    case L1SkillId.EFFECT_MAGIC_STONE_A_1:
                        pc.addMaxHp(10);
                        type = 84;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_2:
                        pc.addMaxHp(20);
                        type = 85;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_3:
                        pc.addMaxHp(30);
                        type = 86;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_4:
                        pc.addMaxHp(40);
                        type = 87;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_5:
                        pc.addMaxHp(50);
                        pc.addHpr(1);
                        type = 88;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_6:
                        pc.addMaxHp(60);
                        pc.addHpr(2);
                        type = 89;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_7:
                        pc.addMaxHp(70);
                        pc.addHpr(3);
                        type = 90;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_8:
                        pc.addMaxHp(80);
                        pc.addHpr(4);
                        pc.addHitup(1);
                        type = 91;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_A_9:
                        pc.addMaxHp(100);
                        pc.addHpr(5);
                        pc.addHitup(2);
                        pc.addDmgup(2);
                        pc.addStr((sbyte)1);
                        pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                        type = 92;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_1:
                        pc.addMaxHp(5);
                        pc.addMaxMp(3);
                        type = 93;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_2:
                        pc.addMaxHp(10);
                        pc.addMaxMp(6);
                        type = 94;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_3:
                        pc.addMaxHp(15);
                        pc.addMaxMp(10);
                        type = 95;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_4:
                        pc.addMaxHp(20);
                        pc.addMaxMp(15);
                        type = 96;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_5:
                        pc.addMaxHp(25);
                        pc.addMaxMp(20);
                        type = 97;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_6:
                        pc.addMaxHp(30);
                        pc.addMaxMp(20);
                        pc.addHpr(1);
                        type = 98;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_7:
                        pc.addMaxHp(35);
                        pc.addMaxMp(20);
                        pc.addHpr(1);
                        pc.addMpr(1);
                        type = 99;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_8:
                        pc.addMaxHp(40);
                        pc.addMaxMp(25);
                        pc.addHpr(2);
                        pc.addMpr(1);
                        type = 100;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_B_9:
                        pc.addMaxHp(50);
                        pc.addMaxMp(30);
                        pc.addHpr(2);
                        pc.addMpr(2);
                        pc.addBowDmgup(2);
                        pc.addBowHitup(2);
                        pc.addDex((sbyte)1);
                        pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                        type = 101;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_1:
                        pc.addMaxMp(5);
                        type = 102;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_2:
                        pc.addMaxMp(10);
                        type = 103;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_3:
                        pc.addMaxMp(15);
                        type = 104;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_4:
                        pc.addMaxMp(20);
                        type = 105;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_5:
                        pc.addMaxMp(25);
                        pc.addMpr(1);
                        type = 106;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_6:
                        pc.addMaxMp(30);
                        pc.addMpr(2);
                        type = 107;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_7:
                        pc.addMaxMp(35);
                        pc.addMpr(3);
                        type = 108;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_8:
                        pc.addMaxMp(40);
                        pc.addMpr(4);
                        type = 109;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_C_9:
                        pc.addMaxMp(50);
                        pc.addMpr(5);
                        pc.addInt((sbyte)1);
                        pc.addSp(1);
                        pc.sendPackets(new S_SPMR(pc));
                        pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                        type = 110;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_1:
                        pc.addMr(2);
                        type = 111;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_2:
                        pc.addMr(4);
                        type = 112;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_3:
                        pc.addMr(6);
                        type = 113;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_4:
                        pc.addMr(8);
                        type = 114;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_5:
                        pc.addMr(10);
                        pc.addAc(-1);
                        type = 115;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_6:
                        pc.addMr(10);
                        pc.addAc(-2);
                        type = 116;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_7:
                        pc.addMr(10);
                        pc.addAc(-3);
                        type = 117;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_8:
                        pc.addMr(15);
                        pc.addAc(-4);
                        pc.addDamageReductionByArmor(1);
                        type = 118;
                        break;
                    case L1SkillId.EFFECT_MAGIC_STONE_D_9:
                        pc.addMr(20);
                        pc.addAc(-5);
                        pc.addCon((sbyte)1);
                        pc.addDamageReductionByArmor(3);
                        type = 119;
                        break;
                    default:
                        break;
                }

                if (type >= 84 && type <= 92)
                { // (近戰)
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                }
                else if (type >= 93 && type <= 101)
                { // (遠攻)
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                }
                else if (type >= 102 && type <= 110)
                { // 恢復
                    pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
                }
                else if (type >= 111 && type <= 119)
                { // 防禦
                    pc.sendPackets(new S_SPMR(pc));
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                    pc.sendPackets(new S_OwnCharStatus2(pc, 0));

                }
            }
            pc.MagicStoneLevel = type;
            pc.setSkillEffect(skillId, time * 1000);
        }

    }

}