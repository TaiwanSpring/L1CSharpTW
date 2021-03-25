
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.item.action;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.DataSources
{
    class CharBuffTable
    {
        private CharBuffTable()
        {
        }
        private static readonly int[] buffSkill = new int[] { 2, 67, 3, 99, 151, 159, 168, 52, 101, 150, 26, 42, 109, 110, 114, 115, 117, 148, 155, 163, 149, 156, 166, DRESS_EVASION, RESIST_FEAR, MIRROR_IMAGE, UNCANNY_DODGE, 43, 54, STATUS_HASTE, STATUS_BRAVE, STATUS_ELFBRAVE, STATUS_RIBRAVE, STATUS_BRAVE2, STATUS_THIRD_SPEED, STATUS_BLUE_POTION, STATUS_CHAT_PROHIBITED, COOKING_1_0_N, COOKING_1_0_S, COOKING_1_1_N, COOKING_1_1_S, COOKING_1_2_N, COOKING_1_2_S, COOKING_1_3_N, COOKING_1_3_S, COOKING_1_4_N, COOKING_1_4_S, COOKING_1_5_N, COOKING_1_5_S, COOKING_1_6_N, COOKING_1_6_S, COOKING_2_0_N, COOKING_2_0_S, COOKING_2_1_N, COOKING_2_1_S, COOKING_2_2_N, COOKING_2_2_S, COOKING_2_3_N, COOKING_2_3_S, COOKING_2_4_N, COOKING_2_4_S, COOKING_2_5_N, COOKING_2_5_S, COOKING_2_6_N, COOKING_2_6_S, COOKING_3_0_N, COOKING_3_0_S, COOKING_3_1_N, COOKING_3_1_S, COOKING_3_2_N, COOKING_3_2_S, COOKING_3_3_N, COOKING_3_3_S, COOKING_3_4_N, COOKING_3_4_S, COOKING_3_5_N, COOKING_3_5_S, COOKING_3_6_N, COOKING_3_6_S, EFFECT_POTION_OF_EXP_150, EFFECT_POTION_OF_EXP_175, EFFECT_POTION_OF_EXP_200, EFFECT_POTION_OF_EXP_225, EFFECT_POTION_OF_EXP_250, EFFECT_POTION_OF_BATTLE, EFFECT_BLESS_OF_MAZU, EFFECT_ENCHANTING_BATTLE, EFFECT_STRENGTHENING_HP, EFFECT_STRENGTHENING_MP, COOKING_WONDER_DRUG, EFFECT_BLOODSTAIN_OF_ANTHARAS, EFFECT_BLOODSTAIN_OF_FAFURION, EFFECT_MAGIC_STONE_A_1, EFFECT_MAGIC_STONE_A_2, EFFECT_MAGIC_STONE_A_3, EFFECT_MAGIC_STONE_A_4, EFFECT_MAGIC_STONE_A_5, EFFECT_MAGIC_STONE_A_6, EFFECT_MAGIC_STONE_A_7, EFFECT_MAGIC_STONE_A_8, EFFECT_MAGIC_STONE_A_9, EFFECT_MAGIC_STONE_B_1, EFFECT_MAGIC_STONE_B_2, EFFECT_MAGIC_STONE_B_3, EFFECT_MAGIC_STONE_B_4, EFFECT_MAGIC_STONE_B_5, EFFECT_MAGIC_STONE_B_6, EFFECT_MAGIC_STONE_B_7, EFFECT_MAGIC_STONE_B_8, EFFECT_MAGIC_STONE_B_9, EFFECT_MAGIC_STONE_C_1, EFFECT_MAGIC_STONE_C_2, EFFECT_MAGIC_STONE_C_3, EFFECT_MAGIC_STONE_C_4, EFFECT_MAGIC_STONE_C_5, EFFECT_MAGIC_STONE_C_6, EFFECT_MAGIC_STONE_C_7, EFFECT_MAGIC_STONE_C_8, EFFECT_MAGIC_STONE_C_9, EFFECT_MAGIC_STONE_D_1, EFFECT_MAGIC_STONE_D_2, EFFECT_MAGIC_STONE_D_3, EFFECT_MAGIC_STONE_D_4, EFFECT_MAGIC_STONE_D_5, EFFECT_MAGIC_STONE_D_6, EFFECT_MAGIC_STONE_D_7, EFFECT_MAGIC_STONE_D_8, EFFECT_MAGIC_STONE_D_9, EFFECT_MAGIC_EYE_OF_AHTHARTS, EFFECT_MAGIC_EYE_OF_FAFURION, EFFECT_MAGIC_EYE_OF_LINDVIOR, EFFECT_MAGIC_EYE_OF_VALAKAS, EFFECT_MAGIC_EYE_OF_BIRTH, EFFECT_MAGIC_EYE_OF_FIGURE, EFFECT_MAGIC_EYE_OF_LIFE, EFFECT_BLESS_OF_CRAY, EFFECT_BLESS_OF_SAELL };

        private static void StoreBuff(int objId, int skillId, int time, int polyId)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("INSERT INTO character_buff SET char_obj_id=?, skill_id=?, remaining_time=?, poly_id=?");
                pstm.setInt(1, objId);
                pstm.setInt(2, skillId);
                pstm.setInt(3, time);
                pstm.setInt(4, polyId);
                pstm.execute();
            }
            catch (Exception e)
            {
            }
        }

        public static void DeleteBuff(L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("DELETE FROM character_buff WHERE char_obj_id=?");
                pstm.setInt(1, pc.Id);
                pstm.execute();
            }
            catch (Exception e)
            {
            }
        }

        public static void SaveBuff(L1PcInstance pc)
        {
            foreach (int skillId in buffSkill)
            {
                int timeSec = pc.getSkillEffectTimeSec(skillId);
                if (0 < timeSec)
                {
                    int polyId = 0;
                    if (skillId == SHAPE_CHANGE)
                    {
                        polyId = pc.TempCharGfx;
                    }
                    StoreBuff(pc.Id, skillId, timeSec, polyId);
                }
            }
        }

        public static void buffRemainingTime(L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            con = L1DatabaseFactory.Instance.Connection;
            pstm = con.prepareStatement("SELECT * FROM character_buff WHERE char_obj_id=?");
            pstm.setInt(1, pc.Id);
            rs = pstm.executeQuery();
            while (rs.next())
            {
                int skillid = rs.getInt("skill_id");
                int remaining_time = rs.getInt("remaining_time");
                switch (skillid)
                {
                    case L1SkillId.STATUS_RIBRAVE: // 生命之樹果實
                    case L1SkillId.DRESS_EVASION: // 迴避提升
                        remaining_time = remaining_time / 4;
                        pc.setSkillEffect(skillid, remaining_time * 4 * 1000);
                        break;
                    case L1SkillId.COOKING_WONDER_DRUG: // 象牙塔妙藥
                        pc.addHpr(10);
                        pc.addMpr(2);
                        remaining_time = remaining_time / 4;
                        pc.setSkillEffect(skillid, remaining_time * 4 * 1000);
                        break;
                    case L1SkillId.EFFECT_BLESS_OF_MAZU: // 媽祖的祝福
                    case L1SkillId.EFFECT_ENCHANTING_BATTLE: // 強化戰鬥卷軸
                    case L1SkillId.EFFECT_STRENGTHENING_HP: // 體力增強卷軸
                    case L1SkillId.EFFECT_STRENGTHENING_MP: // 魔力增強卷軸
                        remaining_time = remaining_time / 16;
                        Effect.useEffect(pc, skillid, remaining_time * 16);
                        break;
                    case L1SkillId.EFFECT_POTION_OF_BATTLE: // 戰鬥藥水
                    case L1SkillId.EFFECT_POTION_OF_EXP_150: // 神力藥水
                    case L1SkillId.EFFECT_POTION_OF_EXP_175:
                    case L1SkillId.EFFECT_POTION_OF_EXP_200:
                    case L1SkillId.EFFECT_POTION_OF_EXP_225:
                    case L1SkillId.EFFECT_POTION_OF_EXP_250:
                        remaining_time = remaining_time / 16;
                        pc.setSkillEffect(skillid, remaining_time * 16 * 1000);
                        break;
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_AHTHARTS: // 魔眼
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_FAFURION:
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_LINDVIOR:
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_VALAKAS:
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_BIRTH:
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_FIGURE:
                    case L1SkillId.EFFECT_MAGIC_EYE_OF_LIFE:
                        remaining_time = remaining_time / 32;
                        Effect.useEffect(pc, skillid, remaining_time * 32);
                        break;
                    case L1SkillId.RESIST_FEAR: // 恐懼無助
                        remaining_time = remaining_time / 4;
                        pc.addNdodge((sbyte)5); // 閃避率 - 50%
                                                // 更新閃避率顯示
                        pc.sendPackets(new S_PacketBox(101, pc.Ndodge));
                        pc.setSkillEffect(skillid, remaining_time * 4 * 1000);
                        break;
                    case L1SkillId.EFFECT_BLESS_OF_CRAY: // 卡瑞、莎爾的祝福
                    case L1SkillId.EFFECT_BLESS_OF_SAELL:
                        remaining_time = remaining_time / 32;
                        L1BuffUtil.effectBlessOfDragonSlayer(pc, skillid, remaining_time * 32, 0);
                        break;
                    default:
                        if (skillid >= L1SkillId.EFFECT_MAGIC_STONE_A_1 && skillid <= L1SkillId.EFFECT_MAGIC_STONE_D_9)
                        { // 附魔石
                            remaining_time = remaining_time / 32;
                            Effect.magicStoneEffect(pc, skillid, remaining_time * 32);
                            break;
                        }
                        break;
                }
            }
        }

    }

}