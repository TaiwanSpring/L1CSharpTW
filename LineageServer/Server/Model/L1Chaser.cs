using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;

namespace LineageServer.Server.Model
{
    class L1Chaser : TimerTask
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1Chaser));
        private int _timeCounter = 0;
        private readonly int _attr;
        private readonly int _gfxid;
        private readonly L1PcInstance _pc;
        private readonly L1Character _cha;

        public L1Chaser(L1PcInstance pc, L1Character cha, int attr, int gfxid)
        {
            _cha = cha;
            _pc = pc;
            _attr = attr;
            _gfxid = gfxid;
        }

        public override void run()
        {
            try
            {
                if (_cha == null || _cha.Dead)
                {
                    stop();
                    return;
                }
                attack();
                _timeCounter++;
                if (_timeCounter >= 3)
                {
                    stop();
                    return;
                }
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
            Container.Instance.Resolve<ITaskController>().execute(this, 0, 1000);
        }

        public virtual void stop()
        {
            cancel();
        }

        public virtual void attack()
        {
            double damage = getDamage(_pc, _cha);
            if (_cha.CurrentHp - (int)damage <= 0 && _cha.CurrentHp != 1)
            {
                damage = _cha.CurrentHp - 1;
            }
            else if (_cha.CurrentHp == 1)
            {
                damage = 0;
            }
            S_EffectLocation packet = new S_EffectLocation(_cha.X, _cha.Y, _gfxid);
            if (_pc.Weapon == null)
            { // 修正空手會出錯的問題
                damage = 0;
            }
            else if (_pc.Weapon.Item.ItemId == 265 || _pc.Weapon.Item.ItemId == 266 || _pc.Weapon.Item.ItemId == 267 || _pc.Weapon.Item.ItemId == 268)
            {
                packet = new S_EffectLocation(_cha.X, _cha.Y, 7025);
            }
            else if (_pc.Weapon.Item.ItemId == 276 || _pc.Weapon.Item.ItemId == 277)
            {
                packet = new S_EffectLocation(_cha.X, _cha.Y, 7224);
            }
            else if (_pc.Weapon.Item.ItemId == 304 || _pc.Weapon.Item.ItemId == 307 || _pc.Weapon.Item.ItemId == 308)
            {
                packet = new S_EffectLocation(_cha.X, _cha.Y, 8150);
            }
            else if (_pc.Weapon.Item.ItemId == 305 || _pc.Weapon.Item.ItemId == 306 || _pc.Weapon.Item.ItemId == 309)
            {
                packet = new S_EffectLocation(_cha.X, _cha.Y, 8152);
            }
            else
            { // 更換為其他武器 附加特效傷害歸零
                damage = 0;
            }
            _pc.sendPackets(packet);
            _pc.broadcastPacket(packet);
            if (_cha is L1PcInstance)
            {
                L1PcInstance pc = (L1PcInstance)_cha;
                pc.sendPackets(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
                pc.broadcastPacket(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
                pc.receiveDamage(_pc, damage, false);
            }
            else if (_cha is L1NpcInstance)
            {
                L1NpcInstance npc = (L1NpcInstance)_cha;
                npc.broadcastPacket(new S_DoActionGFX(npc.Id, ActionCodes.ACTION_Damage));
                npc.receiveDamage(_pc, (int)damage);
            }
        }

        public virtual double getDamage(L1PcInstance pc, L1Character cha)
        {
            double dmg = 0;
            int spByItem = pc.Sp - pc.TrueSp;
            int intel = pc.Int;
            int charaIntelligence = pc.Int + spByItem - 12;
            double coefficientA = 1.0 + 3.0 / 32.0 * charaIntelligence;
            if (coefficientA < 1)
            {
                coefficientA = 1.0;
            }
            double coefficientB = 0;
            if (intel > 18)
            {
                coefficientB = (intel + 2.0) / intel;
            }
            else if (intel <= 12)
            {
                coefficientB = 12.0 * 0.065;
            }
            else
            {
                coefficientB = intel * 0.065;
            }
            double coefficientC = 0;
            if (intel <= 12)
            {
                coefficientC = 12;
            }
            else
            {
                coefficientC = intel;
            }
            dmg = (RandomHelper.Next(6) + 1 + 7) * coefficientA * coefficientB / 10.5 * coefficientC * 2.0;
            dmg = L1WeaponSkill.calcDamageReduction(pc, cha, dmg, _attr);
            if (cha.hasSkillEffect(L1SkillId.IMMUNE_TO_HARM))
            {
                dmg /= 2.0;
            }
            return dmg;
        }

    }

}