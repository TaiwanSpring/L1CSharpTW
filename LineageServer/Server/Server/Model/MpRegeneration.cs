using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.Types;
namespace LineageServer.Server.Server.Model
{
    class MpRegeneration : PcInstanceRunnableBase
    {
        private int _regenPoint = 0;

        private int _curPoint = 4;

        public MpRegeneration(L1PcInstance pc) : base(pc)
        {
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
        public virtual void regenMp()
        {
            int baseMpr = 1;
            int wis = _pc.Wis;
            if ((wis == 15) || (wis == 16))
            {
                baseMpr = 2;
            }
            else if (wis >= 17)
            {
                baseMpr = 3;
            }

            if (_pc.hasSkillEffect(L1SkillId.STATUS_BLUE_POTION))
            { // ブルーポーション使用中
                if (wis < 11)
                { // WIS11未満でもMPR+1
                    wis = 11;
                }
                baseMpr += wis - 10;
            }
            if (L1HouseLocation.isInHouse(_pc.X, _pc.Y, _pc.MapId))
            {
                baseMpr += 3;
            }
            if ((_pc.MapId == 16384) || (_pc.MapId == 16896) || (_pc.MapId == 17408) || (_pc.MapId == 17920) || (_pc.MapId == 18432) || (_pc.MapId == 18944) || (_pc.MapId == 19968) || (_pc.MapId == 19456) || (_pc.MapId == 20480) || (_pc.MapId == 20992) || (_pc.MapId == 21504) || (_pc.MapId == 22016) || (_pc.MapId == 22528) || (_pc.MapId == 23040) || (_pc.MapId == 23552) || (_pc.MapId == 24064) || (_pc.MapId == 24576) || (_pc.MapId == 25088))
            { // 宿屋
                baseMpr += 3;
            }
            if ((_pc.Location.isInScreen(new Point(33055, 32336)) && (_pc.MapId == 4) && _pc.Elf))
            {
                baseMpr += 3;
            }
            if (_pc.hasSkillEffect(L1SkillId.COOKING_1_2_N) || _pc.hasSkillEffect(L1SkillId.COOKING_1_2_S))
            {
                baseMpr += 3;
            }
            if (_pc.hasSkillEffect(L1SkillId.COOKING_2_4_N) || _pc.hasSkillEffect(L1SkillId.COOKING_2_4_S) || _pc.hasSkillEffect(L1SkillId.COOKING_3_5_N) || _pc.hasSkillEffect(L1SkillId.COOKING_3_5_S))
            {
                baseMpr += 2;
            }
            if (_pc.OriginalMpr > 0)
            { // オリジナルWIS MPR補正
                baseMpr += _pc.OriginalMpr;
            }

            int itemMpr = _pc.Inventory.mpRegenPerTick();
            itemMpr += _pc.Mpr;

            if ((_pc.get_food() < 3) || isOverWeight(_pc))
            {
                baseMpr = 0;
                if (itemMpr > 0)
                {
                    itemMpr = 0;
                }
            }
            int mpr = baseMpr + itemMpr;
            int newMp = _pc.CurrentMp + mpr;
            if (newMp < 0)
            {
                newMp = 0;
            }
            _pc.CurrentMp = newMp;
        }

        private bool isOverWeight(L1PcInstance pc)
        {
            // エキゾチックバイタライズ状態、アディショナルファイアー状態であれば、
            // 重量オーバーでは無いとみなす。
            if (pc.hasSkillEffect(L1SkillId.EXOTIC_VITALIZE) || pc.hasSkillEffect(L1SkillId.ADDITIONAL_FIRE))
            {
                return false;
            }

            return (120 <= pc.Inventory.Weight242) ? true : false;
        }

        protected override void DoRun()
        {
            _regenPoint += _curPoint;
            _curPoint = 4;

            if (64 <= _regenPoint)
            {
                _regenPoint = 0;
                regenMp();
            }
        }
    }

}