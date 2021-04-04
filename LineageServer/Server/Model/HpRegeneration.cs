using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.Types;
using System;
using LineageServer.Utils;

namespace LineageServer.Server.Model
{

    class HpRegeneration : PcInstanceRunnableBase
    {
        private int _regenMax = 0;

        private int _regenPoint = 0;

        private int _curPoint = 4;

        public HpRegeneration(L1PcInstance pc) : base(pc)
        {
            updateLevel();
        }

        public virtual int State
        {
            set
            {
                if (_curPoint < value)
                {
                    return;
                }

                _curPoint = value;
            }
        }
        public virtual void updateLevel()
        {
            int[] lvlTable = new int[] { 30, 25, 20, 16, 14, 12, 11, 10, 9, 3, 2 };

            int regenLvl = Math.Min(10, _pc.Level);
            if (30 <= _pc.Level && _pc.Knight)
            {
                regenLvl = 11;
            }

            lock (this)
            {
                _regenMax = lvlTable[regenLvl - 1] * 4;
            }
        }

        public virtual void regenHp()
        {
            if (_pc.Dead)
            {
                return;
            }

            int maxBonus = 1;

            // CONボーナス
            if (11 < _pc.Level && 14 <= _pc.BaseCon)
            {
                maxBonus = _pc.BaseCon - 12;
                if (25 < _pc.BaseCon)
                {
                    maxBonus = 14;
                }
            }

            int equipHpr = pcInventory.hpRegenPerTick();
            equipHpr += _pc.Hpr;
            int bonus = RandomHelper.Next(maxBonus) + 1;

            if (_pc.hasSkillEffect(L1SkillId.NATURES_TOUCH))
            {
                bonus += 15;
            }
            if (L1HouseLocation.isInHouse(_pc.X, _pc.Y, _pc.MapId))
            {
                bonus += 5;
            }
            if (_pc.MapId == 16384 || _pc.MapId == 16896 || _pc.MapId == 17408 || _pc.MapId == 17920 || _pc.MapId == 18432 || _pc.MapId == 18944 || _pc.MapId == 19968 || _pc.MapId == 19456 || _pc.MapId == 20480 || _pc.MapId == 20992 || _pc.MapId == 21504 || _pc.MapId == 22016 || _pc.MapId == 22528 || _pc.MapId == 23040 || _pc.MapId == 23552 || _pc.MapId == 24064 || _pc.MapId == 24576 || _pc.MapId == 25088)
            { // 宿屋
                bonus += 5;
            }
            if ((_pc.Location.isInScreen(new Point(33055, 32336)) && _pc.MapId == 4 && _pc.Elf))
            {
                bonus += 5;
            }
            if (_pc.hasSkillEffect(L1SkillId.COOKING_1_5_N) || _pc.hasSkillEffect(L1SkillId.COOKING_1_5_S))
            {
                bonus += 3;
            }
            if (_pc.hasSkillEffect(L1SkillId.COOKING_2_4_N) || _pc.hasSkillEffect(L1SkillId.COOKING_2_4_S) || _pc.hasSkillEffect(L1SkillId.COOKING_3_6_N) || _pc.hasSkillEffect(L1SkillId.COOKING_3_6_S))
            {
                bonus += 2;
            }
            if (_pc.OriginalHpr > 0)
            { // オリジナルCON HPR補正
                bonus += _pc.OriginalHpr;
            }

            bool inLifeStream = false;
            if (isPlayerInLifeStream(_pc))
            {
                inLifeStream = true;
                // 古代の空間、魔族の神殿ではHPR+3はなくなる？
                bonus += 3;
            }

            // 空腹と重量のチェック
            if (_pc.get_food() < 3 || isOverWeight(_pc) || _pc.hasSkillEffect(L1SkillId.BERSERKERS))
            {
                bonus = 0;
                // 装備によるＨＰＲ増加は満腹度、重量によってなくなるが、 減少である場合は満腹度、重量に関係なく効果が残る
                if (equipHpr > 0)
                {
                    equipHpr = 0;
                }
            }
            //		貼入下方 就可以免登出把能力值點完了

            //		免登出可以把能力值點完         chinaabc

            if (_pc.Level >= 51 && _pc.Level - 50 > _pc.BonusStats)
            {

                if ((_pc.BaseStr + _pc.BaseDex + _pc.BaseCon + _pc.BaseInt + _pc.BaseWis + _pc.BaseCha) < 150)
                {

                    _pc.sendPackets(new S_bonusstats(_pc.Id, 1));

                }

            }

            //		END
            int newHp = _pc.CurrentHp;
            newHp += bonus + equipHpr;

            if (newHp < 1)
            {
                newHp = 1; // ＨＰＲ減少装備によって死亡はしない
            }
            // 水中での減少処理
            // ライフストリームで減少をなくせるか不明
            if (isUnderwater(_pc))
            {
                newHp -= 20;
                if (newHp < 1)
                {
                    if (_pc.Gm)
                    {
                        newHp = 1;
                    }
                    else
                    {
                        _pc.death(null); // 窒息によってＨＰが０になった場合は死亡する。
                    }
                }
            }
            // Lv50クエストの古代の空間1F2Fでの減少処理
            if (isLv50Quest(_pc) && !inLifeStream)
            {
                newHp -= 10;
                if (newHp < 1)
                {
                    if (_pc.Gm)
                    {
                        newHp = 1;
                    }
                    else
                    {
                        _pc.death(null); // ＨＰが０になった場合は死亡する。
                    }
                }
            }
            // 魔族の神殿での減少処理
            if (_pc.MapId == 410 && !inLifeStream)
            {
                newHp -= 10;
                if (newHp < 1)
                {
                    if (_pc.Gm)
                    {
                        newHp = 1;
                    }
                    else
                    {
                        _pc.death(null); // ＨＰが０になった場合は死亡する。
                    }
                }
            }

            if (!_pc.Dead)
            {
                _pc.CurrentHp = Math.Min(newHp, _pc.MaxHp);
            }
        }

        private bool isUnderwater(L1PcInstance pc)
        {
            // ウォーターブーツ装備時か、 エヴァの祝福状態、修理された装備セットであれば水中では無いとみなす。
            if (pcInventory.checkEquipped(20207))
            {
                return false;
            }
            if (pc.hasSkillEffect(L1SkillId.STATUS_UNDERWATER_BREATH))
            {
                return false;
            }
            if (pcInventory.checkEquipped(21048) && pcInventory.checkEquipped(21049) && pcInventory.checkEquipped(21050))
            {
                return false;
            }

            return pc.Map.Underwater;
        }

        private bool isOverWeight(L1PcInstance pc)
        {
            // エキゾチックバイタライズ状態、アディショナルファイアー状態か
            // ゴールデンウィング装備時であれば、重量オーバーでは無いとみなす。
            if (pc.hasSkillEffect(L1SkillId.EXOTIC_VITALIZE) || pc.hasSkillEffect(L1SkillId.ADDITIONAL_FIRE))
            {
                return false;
            }
            if (pcInventory.checkEquipped(20049))
            {
                return false;
            }

            return (121 <= pcInventory.Weight242) ? true : false;
        }

        private bool isLv50Quest(L1PcInstance pc)
        {
            int mapId = pc.MapId;
            return (mapId == 2000 || mapId == 2001) ? true : false;
        }

        /// <summary>
        /// 指定したPCがライフストリームの範囲内にいるかチェックする
        /// </summary>
        /// <param name="pc">
        ///            PC </param>
        /// <returns> true PCがライフストリームの範囲内にいる場合 </returns>
        private static bool isPlayerInLifeStream(L1PcInstance pc)
        {
            foreach (GameObject @object in pc.KnownObjects)
            {
                if (@object is L1EffectInstance == false)
                {
                    continue;
                }
                L1EffectInstance effect = (L1EffectInstance)@object;
                if (effect.NpcId == 81169 && effect.Location.getTileLineDistance(pc.Location) < 4)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void DoRun()
        {
            _regenPoint += _curPoint;
            _curPoint = 4;

            lock (this)
            {
                if (_regenMax <= _regenPoint)
                {
                    _regenPoint = 0;
                    regenHp();
                }
            }
        }
    }

}