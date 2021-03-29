
using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.item.Action;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class CharBuffTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterBuff);
        private CharBuffTable()
        {
        }
        private static readonly int[] buffSkill = new int[]
        {
            L1SkillId.LIGHT,L1SkillId.SHAPE_CHANGE,L1SkillId.SHIELD,L1SkillId.SHADOW_ARMOR,L1SkillId.EARTH_SKIN,L1SkillId.EARTH_BLESS,L1SkillId.IRON_SKIN,L1SkillId.HOLY_WALK,L1SkillId.MOVING_ACCELERATION,L1SkillId.WIND_WALK,L1SkillId.PHYSICAL_ENCHANT_DEX,L1SkillId.PHYSICAL_ENCHANT_STR,L1SkillId.DRESS_MIGHTY,L1SkillId.DRESS_DEXTERITY,L1SkillId.GLOWING_AURA,L1SkillId.SHINING_AURA,L1SkillId.BRAVE_AURA,L1SkillId.FIRE_WEAPON,L1SkillId.FIRE_BLESS,L1SkillId.BURNING_WEAPON,L1SkillId.WIND_SHOT,L1SkillId.STORM_EYE,L1SkillId.STORM_SHOT,L1SkillId.DRESS_EVASION,L1SkillId.RESIST_FEAR,L1SkillId.MIRROR_IMAGE,L1SkillId.UNCANNY_DODGE,L1SkillId.HASTE,L1SkillId.GREATER_HASTE,L1SkillId.STATUS_HASTE,L1SkillId.STATUS_BRAVE,L1SkillId.STATUS_ELFBRAVE,L1SkillId.STATUS_RIBRAVE,L1SkillId.STATUS_BRAVE2,L1SkillId.STATUS_THIRD_SPEED,L1SkillId.STATUS_BLUE_POTION,L1SkillId.STATUS_CHAT_PROHIBITED,L1SkillId.COOKING_1_0_N,L1SkillId.COOKING_1_0_S,L1SkillId.COOKING_1_1_N,L1SkillId.COOKING_1_1_S,L1SkillId.COOKING_1_2_N,L1SkillId.COOKING_1_2_S,L1SkillId.COOKING_1_3_N,L1SkillId.COOKING_1_3_S,L1SkillId.COOKING_1_4_N,L1SkillId.COOKING_1_4_S,L1SkillId.COOKING_1_5_N,L1SkillId.COOKING_1_5_S,L1SkillId.COOKING_1_6_N,L1SkillId.COOKING_1_6_S,L1SkillId.COOKING_2_0_N,L1SkillId.COOKING_2_0_S,L1SkillId.COOKING_2_1_N,L1SkillId.COOKING_2_1_S,L1SkillId.COOKING_2_2_N,L1SkillId.COOKING_2_2_S,L1SkillId.COOKING_2_3_N,L1SkillId.COOKING_2_3_S,L1SkillId.COOKING_2_4_N,L1SkillId.COOKING_2_4_S,L1SkillId.COOKING_2_5_N,L1SkillId.COOKING_2_5_S,L1SkillId.COOKING_2_6_N,L1SkillId.COOKING_2_6_S,L1SkillId.COOKING_3_0_N,L1SkillId.COOKING_3_0_S,L1SkillId.COOKING_3_1_N,L1SkillId.COOKING_3_1_S,L1SkillId.COOKING_3_2_N,L1SkillId.COOKING_3_2_S,L1SkillId.COOKING_3_3_N,L1SkillId.COOKING_3_3_S,L1SkillId.COOKING_3_4_N,L1SkillId.COOKING_3_4_S,L1SkillId.COOKING_3_5_N,L1SkillId.COOKING_3_5_S,L1SkillId.COOKING_3_6_N,L1SkillId.COOKING_3_6_S,L1SkillId.EFFECT_POTION_OF_EXP_150,L1SkillId.EFFECT_POTION_OF_EXP_175,L1SkillId.EFFECT_POTION_OF_EXP_200,L1SkillId.EFFECT_POTION_OF_EXP_225,L1SkillId.EFFECT_POTION_OF_EXP_250,L1SkillId.EFFECT_POTION_OF_BATTLE,L1SkillId.EFFECT_BLESS_OF_MAZU,L1SkillId.EFFECT_ENCHANTING_BATTLE,L1SkillId.EFFECT_STRENGTHENING_HP,L1SkillId.EFFECT_STRENGTHENING_MP,L1SkillId.COOKING_WONDER_DRUG,L1SkillId.EFFECT_BLOODSTAIN_OF_ANTHARAS,L1SkillId.EFFECT_BLOODSTAIN_OF_FAFURION,L1SkillId.EFFECT_MAGIC_STONE_A_1,L1SkillId.EFFECT_MAGIC_STONE_A_2,L1SkillId.EFFECT_MAGIC_STONE_A_3,L1SkillId.EFFECT_MAGIC_STONE_A_4,L1SkillId.EFFECT_MAGIC_STONE_A_5,L1SkillId.EFFECT_MAGIC_STONE_A_6,L1SkillId.EFFECT_MAGIC_STONE_A_7,L1SkillId.EFFECT_MAGIC_STONE_A_8,L1SkillId.EFFECT_MAGIC_STONE_A_9,L1SkillId.EFFECT_MAGIC_STONE_B_1,L1SkillId.EFFECT_MAGIC_STONE_B_2,L1SkillId.EFFECT_MAGIC_STONE_B_3,L1SkillId.EFFECT_MAGIC_STONE_B_4,L1SkillId.EFFECT_MAGIC_STONE_B_5,L1SkillId.EFFECT_MAGIC_STONE_B_6,L1SkillId.EFFECT_MAGIC_STONE_B_7,L1SkillId.EFFECT_MAGIC_STONE_B_8,L1SkillId.EFFECT_MAGIC_STONE_B_9,L1SkillId.EFFECT_MAGIC_STONE_C_1,L1SkillId.EFFECT_MAGIC_STONE_C_2,L1SkillId.EFFECT_MAGIC_STONE_C_3,L1SkillId.EFFECT_MAGIC_STONE_C_4,L1SkillId.EFFECT_MAGIC_STONE_C_5,L1SkillId.EFFECT_MAGIC_STONE_C_6,L1SkillId.EFFECT_MAGIC_STONE_C_7,L1SkillId.EFFECT_MAGIC_STONE_C_8,L1SkillId.EFFECT_MAGIC_STONE_C_9,L1SkillId.EFFECT_MAGIC_STONE_D_1,L1SkillId.EFFECT_MAGIC_STONE_D_2,L1SkillId.EFFECT_MAGIC_STONE_D_3,L1SkillId.EFFECT_MAGIC_STONE_D_4,L1SkillId.EFFECT_MAGIC_STONE_D_5,L1SkillId.EFFECT_MAGIC_STONE_D_6,L1SkillId.EFFECT_MAGIC_STONE_D_7,L1SkillId.EFFECT_MAGIC_STONE_D_8,L1SkillId.EFFECT_MAGIC_STONE_D_9,L1SkillId.EFFECT_MAGIC_EYE_OF_AHTHARTS,L1SkillId.EFFECT_MAGIC_EYE_OF_FAFURION,L1SkillId.EFFECT_MAGIC_EYE_OF_LINDVIOR,L1SkillId.EFFECT_MAGIC_EYE_OF_VALAKAS,L1SkillId.EFFECT_MAGIC_EYE_OF_BIRTH,L1SkillId.EFFECT_MAGIC_EYE_OF_FIGURE,L1SkillId.EFFECT_MAGIC_EYE_OF_LIFE,L1SkillId.EFFECT_BLESS_OF_CRAY,L1SkillId.EFFECT_BLESS_OF_SAELL
        };

        private static void StoreBuff(int objId, int skillId, int time, int polyId)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(CharacterBuff.Column_char_obj_id, objId)
            .Set(CharacterBuff.Column_skill_id, skillId)
            .Set(CharacterBuff.Column_remaining_time, time)
            .Set(CharacterBuff.Column_poly_id, polyId)
            .Execute();
        }

        public static void DeleteBuff(L1PcInstance pc)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(CharacterBuff.Column_char_obj_id, pc.Id)
            .Execute();
        }

        public static void SaveBuff(L1PcInstance pc)
        {
            foreach (int skillId in buffSkill)
            {
                int timeSec = pc.getSkillEffectTimeSec(skillId);
                if (0 < timeSec)
                {
                    int polyId = 0;
                    if (skillId == L1SkillId.SHAPE_CHANGE)
                    {
                        polyId = pc.TempCharGfx;
                    }
                    StoreBuff(pc.Id, skillId, timeSec, polyId);
                }
            }
        }

        public static void buffRemainingTime(L1PcInstance pc)
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int skillid = dataSourceRow.getInt(CharacterBuff.Column_skill_id);
                int remaining_time = dataSourceRow.getInt(CharacterBuff.Column_remaining_time);
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