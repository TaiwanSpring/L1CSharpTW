using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_ActiveSpells : ServerBasePacket
    {
        public S_ActiveSpells(L1PcInstance pc)
        {
            byte[] randBox = new byte[2];
            randBox[0] = RandomHelper.NextByte();
            randBox[1] = RandomHelper.NextByte();

            // 取得技能剩餘時間
            CharBuffTable.buffRemainingTime(pc);

            writeC(Opcodes.S_OPCODE_ACTIVESPELLS);
            writeC(0x14);

            foreach (int i in activeSpells(pc))
            {
                if (i != 76)
                {
                    writeC(i);
                }
                else
                {
                    writeD((int)(DateTimeHelper.CurrentUnixTimeMillis() / 1000));
                }
            }
            writeByte(randBox);
        }

        // 登入時給于角色狀態剩餘時間
        private int[] activeSpells(L1PcInstance pc)
        {
            int[] data = new int[104];
            // 生命之樹果實
            if (pc.hasSkillEffect(L1SkillId.STATUS_RIBRAVE))
            {
                data[61] = pc.getSkillEffectTimeSec(STATUS_RIBRAVE) / 4;
            }
            // 迴避提升
            if (pc.hasSkillEffect(L1SkillId.DRESS_EVASION))
            {
                data[17] = pc.getSkillEffectTimeSec(DRESS_EVASION) / 4;
            }
            // 恐懼無助
            if (pc.hasSkillEffect(L1SkillId.RESIST_FEAR))
            {
                data[57] = pc.getSkillEffectTimeSec(RESIST_FEAR) / 4;
            }
            // 象牙塔妙藥
            if (pc.hasSkillEffect(L1SkillId.COOKING_WONDER_DRUG))
            {
                data[42] = pc.getSkillEffectTimeSec(COOKING_WONDER_DRUG) / 4;
                if (data[42] != 0)
                {
                    data[43] = 54; // 因為妙藥，身心都很輕鬆。提升體力回復量和魔力回復量。
                }
            }
            // 戰鬥藥水
            if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_BATTLE))
            {
                data[45] = pc.getSkillEffectTimeSec(EFFECT_POTION_OF_BATTLE) / 16;
                if (data[45] != 0)
                {
                    data[62] = 20; // 經驗值加成20%。
                }
            }
            // 150% ~ 250% 神力藥水
            for (int i = 0; i < 5; i++)
            {
                if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_EXP_150 + i))
                {
                    data[45] = pc.getSkillEffectTimeSec(EFFECT_POTION_OF_EXP_150 + i) / 16;
                    if (data[45] != 0)
                    {
                        data[62] = 50; // 狩獵經驗值將會增加。
                    }
                }
            }
            // 媽祖的祝福
            if (pc.hasSkillEffect(L1SkillId.EFFECT_BLESS_OF_MAZU))
            {
                data[48] = pc.getSkillEffectTimeSec(EFFECT_BLESS_OF_MAZU) / 16;
                if (data[48] != 0)
                {
                    data[49] = 44; // 感受到媽祖的祝福。
                }
            }
            // 體力增強卷軸、魔力增強卷軸、強化戰鬥卷軸
            for (int i = 0; i < 3; i++)
            {
                if (pc.hasSkillEffect(L1SkillId.EFFECT_STRENGTHENING_HP + i))
                {
                    data[46] = pc.getSkillEffectTimeSec(EFFECT_STRENGTHENING_HP + i) / 16;
                    if (data[46] != 0)
                    {
                        data[47] = i; // 體力上限+50，體力回復+4。
                    }
                }
            }
            // 附魔石
            if (pc.MagicStoneLevel != 0)
            {
                int skillId = pc.MagicStoneLevel + 3929; // skillId = 4013 ~ 4048
                data[102] = pc.getSkillEffectTimeSec(skillId) / 32;
                if (data[102] != 0)
                {
                    data[103] = pc.MagicStoneLevel;
                }
            }
            // 龍之魔眼
            for (int i = 0; i < 7; i++)
            {
                if (pc.hasSkillEffect(L1SkillId.EFFECT_MAGIC_EYE_OF_AHTHARTS + i))
                {
                    data[78] = pc.getSkillEffectTimeSec(EFFECT_MAGIC_EYE_OF_AHTHARTS + i) / 32;
                    if (data[78] != 0)
                    {
                        data[79] = 46 + i;
                    }
                }
            }
            // 卡瑞、莎爾的祝福
            if (pc.hasSkillEffect(L1SkillId.EFFECT_BLESS_OF_CRAY))
            {
                data[76] = pc.getSkillEffectTimeSec(EFFECT_BLESS_OF_CRAY) / 32;
                if (data[76] != 0)
                {
                    data[77] = 45;
                }
            }
            else if (pc.hasSkillEffect(L1SkillId.EFFECT_BLESS_OF_SAELL))
            {
                data[76] = pc.getSkillEffectTimeSec(EFFECT_BLESS_OF_SAELL) / 32;
                if (data[76] != 0)
                {
                    data[77] = 60;
                }
            }

            return data;
        }

        public override sbyte[] Content
        {
            get
            {
                if (_byte == null)
                {
                    _byte = _bao.toByteArray();
                }

                return _byte;
            }
        }
    }

}