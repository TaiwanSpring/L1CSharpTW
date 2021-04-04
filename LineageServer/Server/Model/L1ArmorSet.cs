using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model
{
    abstract class L1ArmorSet
    {
        public abstract void giveEffect(L1PcInstance pc);

        public abstract void cancelEffect(L1PcInstance pc);

        public abstract bool isValid(L1PcInstance pc);

        public abstract bool isPartOfSet(int id);

        public abstract bool isEquippedRingOfArmorSet(L1PcInstance pc);

        public static IList<L1ArmorSet> AllSet
        {
            get
            {
                return _allSet;
            }
        }

        private static IList<L1ArmorSet> _allSet = ListFactory.NewList<L1ArmorSet>();

        /*
		 * ここで初期化してしまうのはいかがなものか・・・美しくない気がする
		 */
        static L1ArmorSet()
        {
            L1ArmorSetImpl impl;

            foreach (L1ArmorSets armorSets in ArmorSetTable.Instance.AllList)
            {
                try
                {
                    impl = new L1ArmorSetImpl(getArray(armorSets.Sets, ","));
                    if (armorSets.PolyId != -1)
                    {
                        impl.addEffect(new PolymorphEffect(armorSets.PolyId));
                    }
                    impl.addEffect(new AcHpMpBonusEffect(armorSets.Ac, armorSets.Hp, armorSets.Mp, armorSets.Hpr, armorSets.Mpr, armorSets.Mr));
                    impl.addEffect(new StatBonusEffect(armorSets.Str, armorSets.Dex, armorSets.Con, armorSets.Wis, armorSets.Cha, armorSets.Intl));
                    impl.addEffect(new DefenseBonusEffect(armorSets.DefenseWater, armorSets.DefenseWind, armorSets.DefenseFire, armorSets.DefenseWind));
                    impl.addEffect(new HitDmgModifierEffect(armorSets.HitModifier, armorSets.DmgModifier, armorSets.BowHitModifier, armorSets.BowDmgModifier, armorSets.Sp));
                    _allSet.Add(impl);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    System.Console.Write(ex.StackTrace);
                }
            }
        }

        private static int[] getArray(string s, string sToken)
        {
            StringTokenizer st = new StringTokenizer(s, sToken);
            int size = st.countTokens();
            string temp = null;
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                temp = st.nextToken();
                array[i] = int.Parse(temp);
            }
            return array;
        }
    }

    internal interface IL1ArmorSetEffect
    {
        void giveEffect(L1PcInstance pc);

        void cancelEffect(L1PcInstance pc);
    }

    internal class L1ArmorSetImpl : L1ArmorSet
    {
        private readonly int[] _ids;

        private readonly IList<IL1ArmorSetEffect> _effects;

        protected internal L1ArmorSetImpl(int[] ids)
        {
            _ids = ids;
            _effects = ListFactory.NewList<IL1ArmorSetEffect>();
        }

        public virtual void addEffect(IL1ArmorSetEffect effect)
        {
            _effects.Add(effect);
        }

        public virtual void removeEffect(IL1ArmorSetEffect effect)
        {
            _effects.Remove(effect);
        }

        public override void cancelEffect(L1PcInstance pc)
        {
            foreach (IL1ArmorSetEffect effect in _effects)
            {
                effect.cancelEffect(pc);
            }
        }

        public override void giveEffect(L1PcInstance pc)
        {
            foreach (IL1ArmorSetEffect effect in _effects)
            {
                effect.giveEffect(pc);
            }
        }

        public override sealed bool isValid(L1PcInstance pc)
        {
            return (pc.Inventory as L1PcInventory).checkEquipped(_ids);
        }

        public override bool isPartOfSet(int id)
        {
            foreach (int i in _ids)
            {
                if (id == i)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool isEquippedRingOfArmorSet(L1PcInstance pc)
        {
            L1PcInventory pcInventory = pc.Inventory as L1PcInventory;
            L1ItemInstance armor = null;
            bool isSetContainRing = false;

            // セット装備にリングが含まれているか調べる
            foreach (int id in _ids)
            {
                armor = pcInventory.findItemId(id);
                if ((armor.Item.Type2 == 2) && (armor.Item.Type == 9))
                { // ring
                    isSetContainRing = true;
                    break;
                }
            }

            // リングを2つ装備していて、それが両方セット装備か調べる
            if ((armor != null) && isSetContainRing)
            {
                int itemId = armor.Item.ItemId;
                if (pcInventory.getTypeEquipped(2, 9) == 2)
                {
                    L1ItemInstance[] ring = new L1ItemInstance[2];
                    ring = pcInventory.RingEquipped;
                    if ((ring[0].Item.ItemId == itemId) && (ring[1].Item.ItemId == itemId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }

    internal class AcHpMpBonusEffect : IL1ArmorSetEffect
    {
        private readonly int _ac;

        private readonly int _addHp;

        private readonly int _addMp;

        private readonly int _regenHp;

        private readonly int _regenMp;

        private readonly int _addMr;

        public AcHpMpBonusEffect(int ac, int addHp, int addMp, int regenHp, int regenMp, int addMr)
        {
            _ac = ac;
            _addHp = addHp;
            _addMp = addMp;
            _regenHp = regenHp;
            _regenMp = regenMp;
            _addMr = addMr;
        }

        public virtual void giveEffect(L1PcInstance pc)
        {
            pc.addAc(_ac);
            pc.addMaxHp(_addHp);
            pc.addMaxMp(_addMp);
            pc.addHpr(_regenHp);
            pc.addMpr(_regenMp);
            pc.addMr(_addMr);
        }

        public virtual void cancelEffect(L1PcInstance pc)
        {
            pc.addAc(-_ac);
            pc.addMaxHp(-_addHp);
            pc.addMaxMp(-_addMp);
            pc.addHpr(-_regenHp);
            pc.addMpr(-_regenMp);
            pc.addMr(-_addMr);
        }
    }

    internal class StatBonusEffect : IL1ArmorSetEffect
    {
        private readonly int _str;

        private readonly int _dex;

        private readonly int _con;

        private readonly int _wis;

        private readonly int _cha;

        private readonly int _intl;

        public StatBonusEffect(int str, int dex, int con, int wis, int cha, int intl)
        {
            _str = str;
            _dex = dex;
            _con = con;
            _wis = wis;
            _cha = cha;
            _intl = intl;
        }

        public virtual void giveEffect(L1PcInstance pc)
        {
            pc.addStr(_str);
            pc.addDex(_dex);
            pc.addCon(_con);
            pc.addWis(_wis);
            pc.addCha(_cha);
            pc.addInt(_intl);
        }

        public virtual void cancelEffect(L1PcInstance pc)
        {
            pc.addStr(-_str);
            pc.addDex(-_dex);
            pc.addCon(-_con);
            pc.addWis(-_wis);
            pc.addCha(-_cha);
            pc.addInt(-_intl);
        }
    }

    // 水、風、火、地屬性
    internal class DefenseBonusEffect : IL1ArmorSetEffect
    {
        private readonly int _defenseWater;

        private readonly int _defenseWind;

        private readonly int _defenseFire;

        private readonly int _defenseEarth;

        public DefenseBonusEffect(int defenseWater, int defenseWind, int defenseFire, int defenseEarth)
        {
            _defenseWater = defenseWater;
            _defenseWind = defenseWind;
            _defenseFire = defenseFire;
            _defenseEarth = defenseEarth;
        }

        // @Override
        public virtual void giveEffect(L1PcInstance pc)
        {
            pc.addWater(_defenseWater);
            pc.addWind(_defenseWind);
            pc.addFire(_defenseFire);
            pc.addEarth(_defenseEarth);
        }

        // @Override
        public virtual void cancelEffect(L1PcInstance pc)
        {
            pc.addWater(-_defenseWater);
            pc.addWind(-_defenseWind);
            pc.addFire(-_defenseFire);
            pc.addEarth(-_defenseEarth);
        }
    }

    // 命中率、額外攻擊力、魔攻
    internal class HitDmgModifierEffect : IL1ArmorSetEffect
    {
        private readonly int _hitModifier;

        private readonly int _dmgModifier;

        private readonly int _bowHitModifier;

        private readonly int _bowDmgModifier;

        private readonly int _sp;

        public HitDmgModifierEffect(int hitModifier, int dmgModifier, int bowHitModifier, int bowDmgModifier, int sp)
        {
            _hitModifier = hitModifier;
            _dmgModifier = dmgModifier;
            _bowHitModifier = bowHitModifier;
            _bowDmgModifier = bowDmgModifier;
            _sp = sp;
        }

        // @Override
        public virtual void giveEffect(L1PcInstance pc)
        {
            pc.addHitModifierByArmor(_hitModifier);
            pc.addDmgModifierByArmor(_dmgModifier);
            pc.addBowHitModifierByArmor(_bowHitModifier);
            pc.addBowDmgModifierByArmor(_bowDmgModifier);
            pc.addSp(_sp);
        }

        // @Override
        public virtual void cancelEffect(L1PcInstance pc)
        {
            pc.addHitModifierByArmor(-_hitModifier);
            pc.addDmgModifierByArmor(-_dmgModifier);
            pc.addBowHitModifierByArmor(-_bowHitModifier);
            pc.addBowDmgModifierByArmor(-_bowDmgModifier);
            pc.addSp(-_sp);
        }
    }

    internal class PolymorphEffect : IL1ArmorSetEffect
    {
        private int _gfxId;

        public PolymorphEffect(int gfxId)
        {
            _gfxId = gfxId;
        }

        public virtual void giveEffect(L1PcInstance pc)
        {
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 現在の状態では変身できません。
                return;
            }
            if ((_gfxId == 6080) || (_gfxId == 6094))
            {
                if (pc.get_sex() == 0)
                {
                    _gfxId = 6094;
                }
                else
                {
                    _gfxId = 6080;
                }
                if (!isRemainderOfCharge(pc))
                { // 残チャージ数なし
                    return;
                }
            }
            L1PolyMorph.doPoly(pc, _gfxId, 7200, L1PolyMorph.MORPH_BY_ITEMMAGIC);
        }

        public virtual void cancelEffect(L1PcInstance pc)
        {
            int awakeSkillId = pc.AwakeSkillId;
            if ((awakeSkillId == L1SkillId.AWAKEN_ANTHARAS) ||
                (awakeSkillId == L1SkillId.AWAKEN_FAFURION) ||
                (awakeSkillId == L1SkillId.AWAKEN_VALAKAS))
            {
                pc.sendPackets(new S_ServerMessage(1384)); // 現在の状態では変身できません。
                return;
            }
            if (_gfxId == 6080)
            {
                if (pc.get_sex() == 0)
                {
                    _gfxId = 6094;
                }
            }
            if (pc.TempCharGfx != _gfxId)
            {
                return;
            }
            L1PolyMorph.undoPoly(pc);
        }

        private bool isRemainderOfCharge(L1PcInstance pc)
        {
            bool isRemainderOfCharge = false;
            if (pc.Inventory.checkItem(20383, 1))
            {
                L1ItemInstance item = pc.Inventory.findItemId(20383);
                if (item != null)
                {
                    if (item.ChargeCount != 0)
                    {
                        isRemainderOfCharge = true;
                    }
                }
            }
            return isRemainderOfCharge;
        }

    }

}