using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Server.Model
{
    class L1Cube : TimerTask
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1Cube));

        private int _timeCounter = 0;

        private readonly L1Character _effect;

        private readonly L1Character _cha;

        private readonly int _skillId;

        public L1Cube(L1Character effect, L1Character cha, int skillId)
        {
            _effect = effect;
            _cha = cha;
            _skillId = skillId;
        }

        public override void run()
        {
            try
            {
                if (_cha.Dead)
                {
                    stop();
                    return;
                }
                if (!_cha.hasSkillEffect(_skillId))
                {
                    stop();
                    return;
                }
                _timeCounter++;
                giveEffect();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        public virtual void begin()
        {
            // 効果時間が8秒のため、4秒毎のスキルの場合処理時間を考慮すると実際には1回しか効果が現れない
            // よって開始時間を0.9秒後に設定しておく
            Container.Instance.Resolve<ITaskController>().execute(this, 900, 1000);
        }

        public virtual void stop()
        {
            cancel();
        }

        public virtual void giveEffect()
        {
            if (_skillId == L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY)
            {
                if (_timeCounter % 4 != 0)
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.STATUS_FREEZE))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.ICE_LANCE))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.FREEZING_BREATH))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.EARTH_BIND))
                {
                    return;
                }

                if (_cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)_cha;
                    pc.sendPackets(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
                    pc.broadcastPacket(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
                    pc.receiveDamage(_effect, 10, false);
                }
                else if (_cha is L1MonsterInstance)
                {
                    L1MonsterInstance mob = (L1MonsterInstance)_cha;
                    mob.broadcastPacket(new S_DoActionGFX(mob.Id, ActionCodes.ACTION_Damage));
                    mob.receiveDamage(_effect, 10);
                }
            }
            else if (_skillId == L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY)
            {
                if (_timeCounter % 4 != 0)
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.STATUS_FREEZE))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.ICE_LANCE))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.FREEZING_BREATH))
                {
                    return;
                }
                if (_cha.hasSkillEffect(L1SkillId.EARTH_BIND))
                {
                    return;
                }

                if (_cha is L1PcInstance)
                {
                    L1PcInstance pc = (L1PcInstance)_cha;
                    pc.setSkillEffect(L1SkillId.STATUS_FREEZE, 1000);
                    pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, true));
                }
                else if (_cha is L1MonsterInstance)
                {
                    L1MonsterInstance mob = (L1MonsterInstance)_cha;
                    mob.setSkillEffect(L1SkillId.STATUS_FREEZE, 1000);
                    mob.Paralyzed = true;
                }
            }
            else if (_skillId == L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY)
            {
                // if (_timeCounter % 5 != 0) {
                // return;
                // }
                // _cha.addMr(-10);
                // if (_cha instanceof L1PcInstance) {
                // L1PcInstance pc = (L1PcInstance) _cha;
                // pc.sendPackets(new S_SPMR(pc));
                // }
                _cha.setSkillEffect(L1SkillId.STATUS_MR_REDUCTION_BY_CUBE_SHOCK, 4000);
            }
            else if (_skillId == L1SkillId.STATUS_CUBE_BALANCE)
            {
                if (_timeCounter % 4 == 0)
                {
                    int newMp = _cha.CurrentMp + 5;
                    if (newMp < 0)
                    {
                        newMp = 0;
                    }
                    _cha.CurrentMp = newMp;
                }
                if (_timeCounter % 5 == 0)
                {
                    if (_cha is L1PcInstance)
                    {
                        L1PcInstance pc = (L1PcInstance)_cha;
                        pc.receiveDamage(_effect, 25, false);
                    }
                    else if (_cha is L1MonsterInstance)
                    {
                        L1MonsterInstance mob = (L1MonsterInstance)_cha;
                        mob.receiveDamage(_effect, 25);
                    }
                }
            }
        }

    }

}