using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.npc.action;
using LineageServer.Server.Model.poison;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Server.Types;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;

namespace LineageServer.Server.Model
{
    class L1Attack
    {
        private L1PcInstance _pc = null;

        private L1PcInventory pcInventory;

        private L1Character _target = null;

        private L1PcInstance _targetPc = null;

        private L1PcInventory targetPcInventory;

        private L1NpcInstance _npc = null;

        private L1NpcInstance _targetNpc = null;

        private readonly int _targetId;

        private int _targetX;

        private int _targetY;

        private int _statusDamage = 0;

        private int _hitRate = 0;

        private int _calcType;

        private const int PC_PC = 1;

        private const int PC_NPC = 2;

        private const int NPC_PC = 3;

        private const int NPC_NPC = 4;

        private bool _isHit = false;

        private int _damage = 0;

        private int _drainMana = 0;

        private int _drainHp = 0;

        private sbyte _effectId = 0;

        private int _attckGrfxId = 0;

        private int _attckActId = 0;

        // 攻撃者がプレイヤーの場合の武器情報
        private L1ItemInstance weapon = null;

        private int _weaponId = 0;

        private int _weaponType = 0;

        private int _weaponType2 = 0;

        private int _weaponAddHit = 0;

        private int _weaponAddDmg = 0;

        private int _weaponSmall = 0;

        private int _weaponLarge = 0;

        private int _weaponRange = 1;

        private int _weaponBless = 1;

        private int _weaponEnchant = 0;

        private int _weaponMaterial = 0;

        private int _weaponDoubleDmgChance = 0;

        private int _weaponAttrEnchantKind = 0;

        private int _weaponAttrEnchantLevel = 0;

        private L1ItemInstance _arrow = null;

        private L1ItemInstance _sting = null;

        private int _leverage = 10; // 1/10倍で表現する。

        private int _skillId;

        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unused") private double _skillDamage = 0;
        private double _skillDamage = 0;

        public virtual int Leverage
        {
            set
            {
                _leverage = value;
            }
            get
            {
                return _leverage;
            }
        }


        // 攻撃者がプレイヤーの場合のステータスによる補正
        // private static final int[] strHit = { -2, -2, -2, -2, -2, -2, -2, -2, -2,
        // -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9,
        // 9, 10, 10, 11, 11, 12, 12, 13, 13, 14 };

        // private static final int[] dexHit = { -2, -2, -2, -2, -2, -2, -2, -2, -2,
        // -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
        // 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14 };

        /*
		 * private static final int[] strHit = { -2, -2, -2, -2, -2, -2, -2, //
		 * 0～7まで -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, //
		 * 8～26まで 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 11, 11, 12, 12, 12, //
		 * 27～44まで 13, 13, 13, 14, 14, 14, 15, 15, 15, 16, 16, 16, 17, 17, 17}; //
		 * 45～59まで
		 * 
		 * private static final int[] dexHit = { -2, -2, -2, -2, -2, -2, -1, -1, 0,
		 * 0, // 1～10まで 1, 1, 2, 2, 3, 3, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
		 * 15, 16, // 11～30まで 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
		 * 30, 31, // 31～45まで 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44,
		 * 45, 46 }; // 46～60まで
		 * 
		 * private static final int[] strDmg = new int[128];
		 * 
		 * static { // ＳＴＲダメージ補正 int dmg = -6; for (int str = 0; str <= 22; str++) {
		 * // ０～２２は２毎に＋１ if (str % 2 == 1) { dmg++; } strDmg[str] = dmg; } for (int
		 * str = 23; str <= 28; str++) { // ２３～２８は３毎に＋１ if (str % 3 == 2) { dmg++; }
		 * strDmg[str] = dmg; } for (int str = 29; str <= 32; str++) { //
		 * ２９～３２は２毎に＋１ if (str % 2 == 1) { dmg++; } strDmg[str] = dmg; } for (int
		 * str = 33; str <= 39; str++) { // ３３～３９は１毎に＋１ dmg++; strDmg[str] = dmg; }
		 * for (int str = 40; str <= 46; str++) { // ４０～４６は１毎に＋２ dmg += 2;
		 * strDmg[str] = dmg; } for (int str = 47; str <= 127; str++) { //
		 * ４７～１２７は１毎に＋１ dmg++; strDmg[str] = dmg; } }
		 * 
		 * private static final int[] dexDmg = new int[128];
		 * 
		 * static { // ＤＥＸダメージ補正 for (int dex = 0; dex <= 14; dex++) { // ０～１４は０
		 * dexDmg[dex] = 0; } dexDmg[15] = 1; dexDmg[16] = 2; dexDmg[17] = 3;
		 * dexDmg[18] = 4; dexDmg[19] = 4; dexDmg[20] = 4; dexDmg[21] = 5;
		 * dexDmg[22] = 5; dexDmg[23] = 5; int dmg = 5; for (int dex = 24; dex <=
		 * 127; dex++) { // ２４～１２７は１毎に＋１ dmg++; dexDmg[dex] = dmg; } }
		 */

        private static readonly int[] strHit = new int[] { -2, -2, -2, -2, -2, -2, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 11, 11, 12, 12, 12, 13, 13, 13, 14, 14, 14, 15, 15, 15, 16, 16, 16, 17, 17, 17 };

        private static readonly int[] dexHit = new int[] { -2, -2, -2, -2, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 19, 19, 20, 20, 20, 21, 21, 21, 22, 22, 22, 23, 23, 23, 24, 24, 24, 25, 25, 25, 26, 26, 26, 27, 27, 27, 28 };

        private static readonly int[] strDmg = new int[128];

        static L1Attack()
        {
            // STRダメージ補正
            int dmg = -6;
            for (int str = 0; str <= 22; str++)
            { // 0～22は2毎に+1
                if (str % 2 == 1)
                {
                    dmg++;
                }
                strDmg[str] = dmg;
            }
            for (int str = 23; str <= 28; str++)
            { // 23～28は3毎に+1
                if (str % 3 == 2)
                {
                    dmg++;
                }
                strDmg[str] = dmg;
            }
            for (int str = 29; str <= 32; str++)
            { // 29～32は2毎に+1
                if (str % 2 == 1)
                {
                    dmg++;
                }
                strDmg[str] = dmg;
            }
            for (int str = 33; str <= 34; str++)
            { // 33～34は1毎に+1
                dmg++;
                strDmg[str] = dmg;
            }
            for (int str = 35; str <= 127; str++)
            { // 35～127は4毎に+1
                if (str % 4 == 1)
                {
                    dmg++;
                }
                strDmg[str] = dmg;
            }
            // DEXダメージ補正
            for (int dex = 0; dex <= 14; dex++)
            {
                // 0～14は0
                dexDmg[dex] = 0;
            }
            dexDmg[15] = 1;
            dexDmg[16] = 2;
            dexDmg[17] = 3;
            dexDmg[18] = 4;
            dexDmg[19] = 4;
            dexDmg[20] = 4;
            dexDmg[21] = 5;
            dexDmg[22] = 5;
            dexDmg[23] = 5;
            dmg = 5;
            for (int dex = 24; dex <= 35; dex++)
            { // 24～35は3毎に+1
                if (dex % 3 == 1)
                {
                    dmg++;
                }
                dexDmg[dex] = dmg;
            }
            for (int dex = 36; dex <= 127; dex++)
            { // 36～127は4毎に1
                if (dex % 4 == 1)
                {
                    dmg++;
                }
                dexDmg[dex] = dmg;
            }
        }

        private static readonly int[] dexDmg = new int[128];


        public virtual int ActId
        {
            set
            {
                _attckActId = value;
            }
            get
            {
                return _attckActId;
            }
        }

        public virtual int GfxId
        {
            set
            {
                _attckGrfxId = value;
            }
            get
            {
                return _attckGrfxId;
            }
        }



        public L1Attack(L1Character attacker, L1Character target) : this(attacker, target, 0)
        {
        }

        public L1Attack(L1Character attacker, L1Character target, int skillId)
        {
            _skillId = skillId;
            if (_skillId != 0)
            {
                L1Skills skills = SkillsTable.Instance.getTemplate(_skillId);
                _skillDamage = skills.DamageValue;
            }
            if (attacker is L1PcInstance)
            {
                _pc = (L1PcInstance)attacker;
                pcInventory = this.pcInventory as L1PcInventory;
                if (target is L1PcInstance)
                {
                    _targetPc = (L1PcInstance)target;
                    _calcType = PC_PC;
                    targetPcInventory = _targetPc.Inventory as L1PcInventory;
                }
                else if (target is L1NpcInstance)
                {
                    _targetNpc = (L1NpcInstance)target;
                    _calcType = PC_NPC;
                }
                // 武器情報の取得
                weapon = _pc.Weapon;
                if (weapon != null)
                {
                    _weaponId = weapon.Item.ItemId;
                    _weaponType = weapon.Item.Type1;
                    _weaponType2 = weapon.Item.Type;
                    _weaponAddHit = weapon.Item.HitModifier + weapon.HitByMagic;
                    _weaponAddDmg = weapon.Item.DmgModifier + weapon.DmgByMagic;
                    _weaponSmall = weapon.Item.DmgSmall;
                    _weaponLarge = weapon.Item.DmgLarge;
                    _weaponRange = weapon.Item.Range;
                    _weaponBless = weapon.Item.Bless;
                    _weaponEnchant = weapon.EnchantLevel;
                    _weaponMaterial = weapon.Item.Material;
                    _statusDamage = dexDmg[_pc.getDex()]; // 傷害預設用敏捷補正

                    if (_weaponType == 20)
                    { // 弓箭
                        _arrow = this.pcInventory.Arrow;
                        if (_arrow != null)
                        {
                            _weaponBless = _arrow.Item.Bless;
                            _weaponMaterial = _arrow.Item.Material;
                        }
                    }
                    else if (_weaponType == 62)
                    { // 鐵手甲
                        _sting = this.pcInventory.Sting;
                        if (_sting != null)
                        {
                            _weaponBless = _sting.Item.Bless;
                            _weaponMaterial = _sting.Item.Material;
                        }
                    }
                    else
                    { // 近戰類武器
                        _weaponEnchant = weapon.EnchantLevel - weapon.get_durability(); // 計算武器損傷
                        _statusDamage = strDmg[_pc.getStr()]; // 傷害用力量補正
                    }
                    _weaponDoubleDmgChance = weapon.Item.DoubleDmgChance;
                    _weaponAttrEnchantKind = weapon.AttrEnchantKind;
                    _weaponAttrEnchantLevel = weapon.AttrEnchantLevel;
                }
            }
            else if (attacker is L1NpcInstance)
            {
                _npc = (L1NpcInstance)attacker;
                if (target is L1PcInstance)
                {
                    _targetPc = (L1PcInstance)target;
                    _calcType = NPC_PC;
                }
                else if (target is L1NpcInstance)
                {
                    _targetNpc = (L1NpcInstance)target;
                    _calcType = NPC_NPC;
                }
            }
            _target = target;
            _targetId = target.Id;
            _targetX = target.X;
            _targetY = target.Y;
        }

        /* ■■■■■■■■■■■■■■■■ 命中判定 ■■■■■■■■■■■■■■■■ */

        // 擁有這些狀態的, 不會受到傷害(無敵)
        private static readonly int[] INVINCIBLE = new int[] { L1SkillId.ABSOLUTE_BARRIER, L1SkillId.ICE_LANCE, L1SkillId.FREEZING_BLIZZARD, L1SkillId.FREEZING_BREATH, L1SkillId.EARTH_BIND, L1SkillId.ICE_LANCE_COCKATRICE, L1SkillId.ICE_LANCE_BASILISK };

        public virtual bool calcHit()
        {
            // 檢查無敵狀態
            foreach (int skillId in INVINCIBLE)
            {
                if (_target.hasSkillEffect(skillId))
                {
                    _isHit = false;
                    return _isHit;
                }
            }

            if ((_calcType == PC_PC) || (_calcType == PC_NPC))
            {
                if (_weaponRange != -1)
                {
                    if (_pc.Location.getTileLineDistance(_target.Location) > _weaponRange + 1)
                    { // BIGのモンスターに対応するため射程範囲+1
                        _isHit = false; // 射程範囲外
                        return _isHit;
                    }
                }
                else
                {
                    if (!_pc.Location.isInScreen(_target.Location))
                    {
                        _isHit = false; // 射程範囲外
                        return _isHit;
                    }
                }
                if ((_weaponType == 20) && (_weaponId != 190) && (_arrow == null))
                {
                    _isHit = false; // 沒有箭
                }
                else if ((_weaponType == 62) && (_sting == null))
                {
                    _isHit = false; // 沒有飛刀
                }
                else if (_weaponRange != 1 && !_pc.glanceCheck(_targetX, _targetY))
                {
                    _isHit = false; // 兩格以上武器 直線距離上有障礙物
                }
                else if ((_weaponId == 247) || (_weaponId == 248) || (_weaponId == 249))
                {
                    _isHit = false; // 試練の剣B～C 攻撃無効
                }
                else if (_calcType == PC_PC)
                {
                    _isHit = calcPcPcHit();
                }
                else if (_calcType == PC_NPC)
                {
                    _isHit = calcPcNpcHit();
                }
            }
            else if (_calcType == NPC_PC)
            {
                _isHit = calcNpcPcHit();
            }
            else if (_calcType == NPC_NPC)
            {
                _isHit = calcNpcNpcHit();
            }
            return _isHit;
        }

        private int calShortRageHit(int hitRate)
        {
            int shortHit = hitRate + _pc.Hitup + _pc.OriginalHitup;
            // 防具增加命中
            shortHit += _pc.HitModifierByArmor;

            if (_pc.hasSkillEffect(L1SkillId.COOKING_2_0_N) || _pc.hasSkillEffect(L1SkillId.COOKING_2_0_S))
            {
                shortHit += 1;
            }
            if (_pc.hasSkillEffect(L1SkillId.COOKING_3_2_N) || _pc.hasSkillEffect(L1SkillId.COOKING_3_2_S))
            {
                shortHit += 2;
            }
            return shortHit;
        }

        private int calLongRageHit(int hitRate)
        {
            int longHit = hitRate + _pc.BowHitup + _pc.OriginalBowHitup;
            // 防具增加命中
            longHit += _pc.BowHitModifierByArmor;

            if (_pc.hasSkillEffect(L1SkillId.COOKING_2_3_N) || _pc.hasSkillEffect(L1SkillId.COOKING_2_3_S) || _pc.hasSkillEffect(L1SkillId.COOKING_3_0_N) || _pc.hasSkillEffect(L1SkillId.COOKING_3_0_S))
            {
                longHit += 1;
            }
            return longHit;
        }

        // ●●●● プレイヤー から プレイヤー への命中判定 ●●●●
        /*
		 * ＰＣへの命中率 ＝（PCのLv＋クラス補正＋STR補正＋DEX補正＋武器補正＋DAIの枚数/2＋魔法補正）×0.68－10
		 * これで算出された数値は自分が最大命中(95%)を与える事のできる相手側PCのAC そこから相手側PCのACが1良くなる毎に自命中率から1引いていく
		 * 最小命中率5% 最大命中率95%
		 */
        private bool calcPcPcHit()
        {
            _hitRate = _pc.Level;

            if (_pc.getStr() > 59)
            {
                _hitRate += strHit[58];
            }
            else
            {
                _hitRate += strHit[_pc.getStr() - 1];
            }

            if (_pc.getDex() > 60)
            {
                _hitRate += dexHit[59];
            }
            else
            {
                _hitRate += dexHit[_pc.getDex() - 1];
            }

            // 命中計算 與魔法、食物buff
            _hitRate += _weaponAddHit + (_weaponEnchant / 2);
            if (_weaponType == 20 || _weaponType == 62)
            {
                _hitRate = calLongRageHit(_hitRate);
            }
            else
            {
                _hitRate = calShortRageHit(_hitRate);
            }

            if ((80 < this.pcInventory.Weight242) && (121 >= this.pcInventory.Weight242))
            {
                _hitRate -= 1;
            }
            else if ((122 <= this.pcInventory.Weight242) && (160 >= this.pcInventory.Weight242))
            {
                _hitRate -= 3;
            }
            else if ((161 <= this.pcInventory.Weight242) && (200 >= this.pcInventory.Weight242))
            {
                _hitRate -= 5;
            }

            int attackerDice = RandomHelper.Next(20) + 1 + _hitRate - 10;

            // 閃避率
            attackerDice -= _targetPc.Dodge;
            attackerDice += _targetPc.Ndodge;

            int defenderDice = 0;

            int defenderValue = (int)(_targetPc.Ac * 1.5) * -1;

            if (_targetPc.Ac >= 0)
            {
                defenderDice = 10 - _targetPc.Ac;
            }
            else if (_targetPc.Ac < 0)
            {
                defenderDice = 10 + RandomHelper.Next(defenderValue) + 1;
            }

            int fumble = _hitRate - 9;
            int critical = _hitRate + 10;

            if (attackerDice <= fumble)
            {
                _hitRate = 0;
            }
            else if (attackerDice >= critical)
            {
                _hitRate = 100;
            }
            else
            {
                if (attackerDice > defenderDice)
                {
                    _hitRate = 100;
                }
                else if (attackerDice <= defenderDice)
                {
                    _hitRate = 0;
                }
            }

            if (_weaponType2 == 17 || _weaponType2 == 19)
            {
                _hitRate = 100; // 奇古獸命中率100%
            }

            // TODO 魔法娃娃效果 - 傷害迴避
            else if (L1MagicDoll.getDamageEvasionByDoll(_targetPc) > 0)
            {
                _hitRate = 0;
            }

            int rnd = RandomHelper.Next(100) + 1;
            if ((_weaponType == 20) && (_hitRate > rnd))
            { // 弓の場合、ヒットした場合でもERでの回避を再度行う。
                return calcErEvasion();
            }

            return _hitRate >= rnd;

            /*
			 * final int MIN_HITRATE = 5;
			 * 
			 * _hitRate = _pc.getLevel();
			 * 
			 * if (_pc.getStr() > 39) { _hitRate += strHit[39]; } else { _hitRate +=
			 * strHit[_pc.getStr()]; }
			 * 
			 * if (_pc.getDex() > 39) { _hitRate += dexHit[39]; } else { _hitRate +=
			 * dexHit[_pc.getDex()]; }
			 * 
			 * if (_weaponType != 20 && _weaponType != 62) { _hitRate +=
			 * _weaponAddHit + _pc.getHitup() + _pc.getOriginalHitup() +
			 * (_weaponEnchant / 2); } else { _hitRate += _weaponAddHit +
			 * _pc.getBowHitup() + _pc .getOriginalBowHitup() + (_weaponEnchant /
			 * 2); }
			 * 
			 * if (_weaponType != 20 && _weaponType != 62) { // 防具による追加命中 _hitRate
			 * += _pc.getHitModifierByArmor(); } else { _hitRate +=
			 * _pc.getBowHitModifierByArmor(); }
			 * 
			 * int hitAc = (int) (_hitRate * 0.68 - 10) * -1;
			 * 
			 * if (hitAc <= _targetPc.getAc()) { _hitRate = 95; } else { _hitRate =
			 * 95 - (hitAc - _targetPc.getAc()); }
			 * 
			 * if (_targetPc.hasSkillEffect(L1SkillId.UNCANNY_DODGE)) { _hitRate -= 20; }
			 * 
			 * if (_targetPc.hasSkillEffect(L1SkillId.MIRROR_IMAGE)) { _hitRate -= 20; }
			 * 
			 * if (_pc.hasSkillEffect(L1SkillId.COOKING_2_0_N) // 料理による追加命中 ||
			 * _pc.hasSkillEffect(L1SkillId.COOKING_2_0_S)) { if (_weaponType != 20 &&
			 * _weaponType != 62) { _hitRate += 1; } } if
			 * (_pc.hasSkillEffect(L1SkillId.COOKING_3_2_N) // 料理による追加命中 ||
			 * _pc.hasSkillEffect(L1SkillId.COOKING_3_2_S)) { if (_weaponType != 20 &&
			 * _weaponType != 62) { _hitRate += 2; } } if
			 * (_pc.hasSkillEffect(L1SkillId.COOKING_2_3_N) // 料理による追加命中 ||
			 * _pc.hasSkillEffect(L1SkillId.COOKING_2_3_S) ||
			 * _pc.hasSkillEffect(L1SkillId.COOKING_3_0_N) ||
			 * _pc.hasSkillEffect(L1SkillId.COOKING_3_0_S)) { if (_weaponType == 20 ||
			 * _weaponType == 62) { _hitRate += 1; } }
			 * 
			 * if (_hitRate < MIN_HITRATE) { _hitRate = MIN_HITRATE; }
			 * 
			 * if (_weaponType2 == 17) { _hitRate = 100; // キーリンクの命中率は100% }
			 * 
			 * if (_targetPc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER)) { _hitRate = 0; } if
			 * (_targetPc.hasSkillEffect(L1SkillId.ICE_LANCE)) { _hitRate = 0; } if
			 * (_targetPc.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD)) { _hitRate = 0; } if
			 * (_targetPc.hasSkillEffect(L1SkillId.FREEZING_BREATH)) { _hitRate = 0; } if
			 * (_targetPc.hasSkillEffect(L1SkillId.EARTH_BIND)) { _hitRate = 0; } int rnd =
			 * RandomHelper.NextInt(100) + 1; if (_weaponType == 20 && _hitRate > rnd) {
			 * // 弓の場合、ヒットした場合でもERでの回避を再度行う。 return calcErEvasion(); }
			 * 
			 * return _hitRate >= rnd;
			 */
        }

        // ●●●● プレイヤー から ＮＰＣ への命中判定 ●●●●
        private bool calcPcNpcHit()
        {
            // ＮＰＣへの命中率
            // ＝（PCのLv＋クラス補正＋STR補正＋DEX補正＋武器補正＋DAIの枚数/2＋魔法補正）×5－{NPCのAC×（-5）}
            _hitRate = _pc.Level;

            if (_pc.getStr() > 59)
            {
                _hitRate += strHit[58];
            }
            else
            {
                _hitRate += strHit[_pc.getStr() - 1];
            }

            if (_pc.getDex() > 60)
            {
                _hitRate += dexHit[59];
            }
            else
            {
                _hitRate += dexHit[_pc.getDex() - 1];
            }

            // 命中計算 與魔法、食物buff
            _hitRate += _weaponAddHit + (_weaponEnchant / 2);
            if (_weaponType == 20 || _weaponType == 62)
            {
                _hitRate = calLongRageHit(_hitRate);
            }
            else
            {
                _hitRate = calShortRageHit(_hitRate);
            }

            if ((80 < this.pcInventory.Weight242) && (121 >= this.pcInventory.Weight242))
            {
                _hitRate -= 1;
            }
            else if ((122 <= this.pcInventory.Weight242) && (160 >= this.pcInventory.Weight242))
            {
                _hitRate -= 3;
            }
            else if ((161 <= this.pcInventory.Weight242) && (200 >= this.pcInventory.Weight242))
            {
                _hitRate -= 5;
            }

            int attackerDice = RandomHelper.Next(20) + 1 + _hitRate - 10;

            // 閃避率
            attackerDice -= _targetNpc.Dodge;
            attackerDice += _targetNpc.Ndodge;

            int defenderDice = 10 - _targetNpc.Ac;

            int fumble = _hitRate - 9;
            int critical = _hitRate + 10;

            if (attackerDice <= fumble)
            {
                _hitRate = 0;
            }
            else if (attackerDice >= critical)
            {
                _hitRate = 100;
            }
            else
            {
                if (attackerDice > defenderDice)
                {
                    _hitRate = 100;
                }
                else if (attackerDice <= defenderDice)
                {
                    _hitRate = 0;
                }
            }

            if (_weaponType2 == 17 || _weaponType2 == 19)
            {
                _hitRate = 100; // 奇古獸 命中率 100%
            }

            // 特定狀態下才可攻擊 NPC
            if (_pc.isAttackMiss(_pc, _targetNpc.NpcTemplate.get_npcId()))
            {
                _hitRate = 0;
            }

            int rnd = RandomHelper.Next(100) + 1;

            return _hitRate >= rnd;
        }

        // ●●●● ＮＰＣ から プレイヤー への命中判定 ●●●●
        private bool calcNpcPcHit()
        {

            _hitRate += _npc.Level;

            if (_npc is L1PetInstance)
            { // ペットの武器による追加命中
                _hitRate += ((L1PetInstance)_npc).HitByWeapon;
            }

            _hitRate += _npc.Hitup;

            int attackerDice = RandomHelper.Next(20) + 1 + _hitRate - 1;

            // 閃避率
            attackerDice -= _targetPc.Dodge;
            attackerDice += _targetPc.Ndodge;

            int defenderDice = 0;

            int defenderValue = (_targetPc.Ac) * -1;

            if (_targetPc.Ac >= 0)
            {
                defenderDice = 10 - _targetPc.Ac;
            }
            else if (_targetPc.Ac < 0)
            {
                defenderDice = 10 + RandomHelper.Next(defenderValue) + 1;
            }

            int fumble = _hitRate;
            int critical = _hitRate + 19;

            if (attackerDice <= fumble)
            {
                _hitRate = 0;
            }
            else if (attackerDice >= critical)
            {
                _hitRate = 100;
            }
            else
            {
                if (attackerDice > defenderDice)
                {
                    _hitRate = 100;
                }
                else if (attackerDice <= defenderDice)
                {
                    _hitRate = 0;
                }
            }

            if ((_npc is L1PetInstance) || (_npc is L1SummonInstance))
            {
                // 目標在安區、攻擊者在安區、NOPVP
                if ((_targetPc.ZoneType == 1) || (_npc.ZoneType == 1) || (_targetPc.checkNonPvP(_targetPc, _npc)))
                {
                    _hitRate = 0;
                }
            }
            // TODO 魔法娃娃效果 - 傷害迴避
            else if (L1MagicDoll.getDamageEvasionByDoll(_targetPc) > 0)
            {
                _hitRate = 0;
            }

            int rnd = RandomHelper.Next(100) + 1;

            // NPCの攻撃レンジが10以上の場合で、2以上離れている場合弓攻撃とみなす
            if ((_npc.AtkRanged >= 10) && (_hitRate > rnd) && (_npc.Location.getTileLineDistance(new Point(_targetX, _targetY)) >= 2))
            {
                return calcErEvasion();
            }
            return _hitRate >= rnd;
        }

        // ●●●● ＮＰＣ から ＮＰＣ への命中判定 ●●●●
        private bool calcNpcNpcHit()
        {

            _hitRate += _npc.Level;

            if (_npc is L1PetInstance)
            { // ペットの武器による追加命中
                _hitRate += ((L1PetInstance)_npc).HitByWeapon;
            }

            _hitRate += _npc.Hitup;

            int attackerDice = RandomHelper.Next(20) + 1 + _hitRate - 1;

            // 閃避率
            attackerDice -= _targetNpc.Dodge;
            attackerDice += _targetNpc.Ndodge;

            int defenderDice = 0;

            int defenderValue = (_targetNpc.Ac) * -1;

            if (_targetNpc.Ac >= 0)
            {
                defenderDice = 10 - _targetNpc.Ac;
            }
            else if (_targetNpc.Ac < 0)
            {
                defenderDice = 10 + RandomHelper.Next(defenderValue) + 1;
            }

            int fumble = _hitRate;
            int critical = _hitRate + 19;

            if (attackerDice <= fumble)
            {
                _hitRate = 0;
            }
            else if (attackerDice >= critical)
            {
                _hitRate = 100;
            }
            else
            {
                if (attackerDice > defenderDice)
                {
                    _hitRate = 100;
                }
                else if (attackerDice <= defenderDice)
                {
                    _hitRate = 0;
                }
            }
            if (((_npc is L1PetInstance) || (_npc is L1SummonInstance)) && ((_targetNpc is L1PetInstance) || (_targetNpc is L1SummonInstance)))
            {
                // 目標在安區、攻擊者在安區、NOPVP
                if ((_targetNpc.ZoneType == 1) || (_npc.ZoneType == 1))
                {
                    _hitRate = 0;
                }
            }

            int rnd = RandomHelper.Next(100) + 1;
            return _hitRate >= rnd;
        }

        // ●●●● ＥＲによる回避判定 ●●●●
        private bool calcErEvasion()
        {
            int er = _targetPc.Er;

            int rnd = RandomHelper.Next(100) + 1;
            return er < rnd;
        }

        /* ■■■■■■■■■■■■■■■ ダメージ算出 ■■■■■■■■■■■■■■■ */

        public virtual int calcDamage()
        {
            if (_calcType == PC_PC)
            {
                _damage = calcPcPcDamage();
            }
            else if (_calcType == PC_NPC)
            {
                _damage = calcPcNpcDamage();
            }
            else if (_calcType == NPC_PC)
            {
                _damage = calcNpcPcDamage();
            }
            else if (_calcType == NPC_NPC)
            {
                _damage = calcNpcNpcDamage();
            }
            return _damage;
        }

        private int calcWeponDamage(int weaponMaxDamage)
        {
            int weaponDamage = RandomHelper.Next(weaponMaxDamage) + 1;
            // 判斷魔法輔助
            if (_pc.hasSkillEffect(L1SkillId.SOUL_OF_FLAME))
            {
                weaponDamage = weaponMaxDamage;
            }

            // 判斷武器類型
            bool darkElfWeapon = false;
            if (_pc.Darkelf && (_weaponType == 58))
            { // 鋼爪 (追加判斷持有者為黑妖，避免與幻術師奇谷獸相衝)
                darkElfWeapon = true;
                if ((RandomHelper.Next(100) + 1) <= _weaponDoubleDmgChance)
                { // 出現最大值的機率
                    weaponDamage = weaponMaxDamage;
                }
                if (weaponDamage == weaponMaxDamage)
                { // 出現最大值時 - 爪痕
                    _effectId = 2;
                }
            }
            else if (_weaponType == 20 || _weaponType == 62)
            { // 弓、鐵手甲 不算武器傷害
                weaponDamage = 0;
            }

            weaponDamage += _weaponAddDmg + _weaponEnchant; // 加上武器(額外點數+祝福魔法武器)跟武卷數

            if (_calcType == PC_NPC)
            {
                weaponDamage += calcMaterialBlessDmg(); // 銀祝福武器加傷害
            }
            if (_weaponType == 54)
            {
                darkElfWeapon = true;
                if ((RandomHelper.Next(100) + 1) <= _weaponDoubleDmgChance)
                { // 雙刀雙擊
                    weaponDamage *= 2;
                    _effectId = 4;
                }
            }
            weaponDamage += calcAttrEnchantDmg(); // 属性強化傷害

            if (darkElfWeapon && _pc.hasSkillEffect(L1SkillId.DOUBLE_BRAKE))
            {
                if ((RandomHelper.Next(100) + 1) <= 33)
                {
                    weaponDamage *= 2;
                }
            }

            return weaponDamage;
        }

        private double calLongRageDamage(double dmg)
        {
            double longdmg = dmg + _pc.BowDmgup + _pc.OriginalBowDmgup;

            int add_dmg = 1;
            if (_weaponType == 20)
            { // 弓
                if (_arrow != null)
                {
                    add_dmg = _arrow.Item.DmgSmall;
                    if (_calcType == PC_NPC)
                    {
                        if (_targetNpc.NpcTemplate.get_size() == "large")
                        {
                            add_dmg = _arrow.Item.DmgLarge;
                        }
                        if (_targetNpc.NpcTemplate.is_hard())
                        {
                            add_dmg /= 2;
                        }
                    }
                }
                else if (_weaponId == 190) // 沙哈之弓
                {
                    add_dmg = 15;
                }
            }
            else if (_weaponType == 62)
            { // 鐵手甲
                add_dmg = _sting.Item.DmgSmall;
                if (_calcType == PC_NPC)
                {
                    if (_targetNpc.NpcTemplate.get_size() == "large")
                    {
                        add_dmg = _sting.Item.DmgLarge;
                    }
                }
            }

            if (add_dmg > 0)
            {
                longdmg += RandomHelper.Next(add_dmg) + 1;
            }

            // 防具增傷
            longdmg += _pc.DmgModifierByArmor;

            if (_pc.hasSkillEffect(L1SkillId.COOKING_2_3_N) || _pc.hasSkillEffect(L1SkillId.COOKING_2_3_S) || _pc.hasSkillEffect(L1SkillId.COOKING_3_0_N) || _pc.hasSkillEffect(L1SkillId.COOKING_3_0_S))
            {
                longdmg += 1;
            }

            return longdmg;
        }

        private double calShortRageDamage(double dmg)
        {
            double shortdmg = dmg + _pc.Dmgup + _pc.OriginalDmgup;
            // 弱點曝光發動判斷
            WeaknessExposure();
            // 近戰魔法增傷
            shortdmg = calcBuffDamage(shortdmg);
            // 防具增傷
            shortdmg += _pc.BowDmgModifierByArmor;

            if (_weaponType == 0) // 空手
            {
                shortdmg = (RandomHelper.Next(5) + 4) / 4;
            }
            else if (_weaponType2 == 17 || _weaponType2 == 19) // 奇古獸
            {
                shortdmg = L1WeaponSkill.getKiringkuDamage(_pc, _target);
            }

            if (_pc.hasSkillEffect(L1SkillId.COOKING_2_0_N) || _pc.hasSkillEffect(L1SkillId.COOKING_2_0_S) || _pc.hasSkillEffect(L1SkillId.COOKING_3_2_N) || _pc.hasSkillEffect(L1SkillId.COOKING_3_2_S))
            {
                shortdmg += 1;
            }

            return shortdmg;
        }

        // ●●●● プレイヤー から プレイヤー へのダメージ算出 ●●●●
        public virtual int calcPcPcDamage()
        {
            // 計算武器總傷害
            int weaponTotalDamage = calcWeponDamage(_weaponSmall);

            if ((_weaponId == 262) && (RandomHelper.Next(100) + 1 <= 75))
            { // ディストラクション装備かつ成功確率(暫定)75%
                weaponTotalDamage += calcDestruction(weaponTotalDamage);
            }

            // 計算 遠程 或 近戰武器 傷害 與魔法、食物buff
            double dmg = weaponTotalDamage + _statusDamage;
            if (_weaponType == 20 || _weaponType == 62)
            {
                dmg = calLongRageDamage(dmg);
            }
            else
            {
                dmg = calShortRageDamage(dmg);
            }

            if (_weaponId == 124 || _weaponId == 289 || _weaponId == 290 || _weaponId == 291 || _weaponId == 292 || _weaponId == 293 || _weaponId == 294 || _weaponId == 295 || _weaponId == 296 || _weaponId == 297 || _weaponId == 298 || _weaponId == 299 || _weaponId == 300 || _weaponId == 301 || _weaponId == 302 || _weaponId == 303)
            { // バフォメットスタッフ
                dmg += L1WeaponSkill.getBaphometStaffDamage(_pc, _target);
            }
            else if (_weaponId == 2 || _weaponId == 200002)
            { // ダイスダガー
                dmg += L1WeaponSkill.getDiceDaggerDamage(_pc, _targetPc, weapon);
            }
            else if (_weaponId == 204 || _weaponId == 100204)
            { // 真紅のクロスボウ
                L1WeaponSkill.giveFettersEffect(_pc, _targetPc);
            }
            else if (_weaponId == 264 || _weaponId == 288)
            { // ライトニングエッジ
                dmg += L1WeaponSkill.getLightningEdgeDamage(_pc, _target);
            }
            else if (_weaponId == 260 || _weaponId == 263 || _weaponId == 287)
            { // レイジングウィンド、フリージングランサー
                dmg += L1WeaponSkill.getAreaSkillWeaponDamage(_pc, _target, _weaponId);
            }
            else if (_weaponId == 261)
            { // アークメイジスタッフ
                L1WeaponSkill.giveArkMageDiseaseEffect(_pc, _target);
            }
            else
            {
                dmg += L1WeaponSkill.getWeaponSkillDamage(_pc, _target, _weaponId);
            }

            dmg -= _targetPc.DamageReductionByArmor; // 防具によるダメージ軽減

            // 魔法娃娃效果 - 傷害減免
            dmg -= L1MagicDoll.getDamageReductionByDoll(_targetPc);

            if (_targetPc.hasSkillEffect(L1SkillId.COOKING_1_0_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_1_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_2_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_3_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_4_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_5_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_6_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_0_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_1_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_2_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_3_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_4_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_5_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_6_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_0_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_1_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_2_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_3_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_4_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_5_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_6_S))
            {
                dmg -= 5;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.COOKING_1_7_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_7_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_7_S))
            {
                dmg -= 5;
            }

            if (_targetPc.hasSkillEffect(L1SkillId.REDUCTION_ARMOR))
            {
                int targetPcLvl = _targetPc.Level;
                if (targetPcLvl < 50)
                {
                    targetPcLvl = 50;
                }
                dmg -= (targetPcLvl - 50) / 5 + 1;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.DRAGON_SKIN) || _targetPc.hasSkillEffect(L1SkillId.PATIENCE))
            {
                dmg -= 2;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.IMMUNE_TO_HARM))
            {
                dmg /= 2;
            }
            // 使用暴擊增加15點傷害，而奇古獸固定15點傷害
            if (_skillId == L1SkillId.SMASH)
            {
                dmg += 15;
                if (_weaponType2 == 17 || _weaponType2 == 19)
                {
                    dmg = 15;
                }
            }
            // 使用骷髏毀壞增加10點傷害，而奇古獸固定10點傷害
            else if (_skillId == L1SkillId.BONE_BREAK)
            {
                dmg += 10;
                if (_weaponType2 == 17 || _weaponType2 == 19)
                {
                    dmg = 10;
                }
                // 再次發動判斷
                if (!_targetPc.hasSkillEffect(L1SkillId.BONE_BREAK))
                {
                    int change = RandomHelper.Next(100) + 1;
                    if (change < (30 + RandomHelper.Next(11)))
                    { // 30 ~ 40%
                        L1EffectSpawn.Instance.spawnEffect(93001, 1700, _targetPc.X, _targetPc.Y, _targetPc.MapId);
                        _targetPc.setSkillEffect(L1SkillId.BONE_BREAK, 2 * 1000); // 發動後再次發動間隔
                                                                                  // 2秒
                        _targetPc.setSkillEffect(L1SkillId.BONE_BREAK_START, 700);
                    }
                }
            }
            if (dmg <= 0)
            {
                _isHit = false;
                _drainHp = 0; // ダメージ無しの場合は吸収による回復はしない
            }

            return (int)dmg;
        }

        // ●●●● プレイヤー から ＮＰＣ へのダメージ算出 ●●●●
        private int calcPcNpcDamage()
        {
            int weaponMaxDamage = 0;
            if (_targetNpc.NpcTemplate.get_size() == "small" && (_weaponSmall > 0))
            {
                weaponMaxDamage = _weaponSmall;
            }
            else if (_targetNpc.NpcTemplate.get_size() == "large" && (_weaponLarge > 0))
            {
                weaponMaxDamage = _weaponLarge;
            }

            // 計算武器總傷害
            int weaponTotalDamage = calcWeponDamage(weaponMaxDamage);

            if ((_weaponId == 262) && (RandomHelper.Next(100) + 1 <= 75))
            { // ディストラクション装備かつ成功確率(暫定)75%
                weaponTotalDamage += calcDestruction(weaponTotalDamage);
            }

            // 計算傷害 遠程 或 近戰武器 及buff
            double dmg = weaponTotalDamage + _statusDamage;
            if (_weaponType == 20 || _weaponType == 62)
            {
                dmg = calLongRageDamage(dmg);
            }
            else
            {
                dmg = calShortRageDamage(dmg);
            }

            if (_weaponId == 124 || _weaponId == 289 || _weaponId == 290 || _weaponId == 291 || _weaponId == 292 || _weaponId == 293 || _weaponId == 294 || _weaponId == 295 || _weaponId == 296 || _weaponId == 297 || _weaponId == 298 || _weaponId == 299 || _weaponId == 300 || _weaponId == 301 || _weaponId == 302 || _weaponId == 303)
            {
                dmg += L1WeaponSkill.getBaphometStaffDamage(_pc, _target);
            }
            else if ((_weaponId == 2) || (_weaponId == 200002))
            { // ダイスダガー
                dmg += L1WeaponSkill.getDiceDaggerDamage(_pc, _targetNpc, weapon);
            }
            else if ((_weaponId == 204) || (_weaponId == 100204))
            { // 真紅のクロスボウ
                L1WeaponSkill.giveFettersEffect(_pc, _targetNpc);
                //} else if (_weaponId == 264 || _weaponId == 291) { // ライトニングエッジ
            }
            else if (_weaponId == 264 || _weaponId == 288)
            { // ライトニングエッジ, 天雷劍能發動的修正
                dmg += L1WeaponSkill.getLightningEdgeDamage(_pc, _target);
            }
            else if ((_weaponId == 260) || (_weaponId == 263 || _weaponId == 287))
            { // レイジングウィンド、フリージングランサー
                dmg += L1WeaponSkill.getAreaSkillWeaponDamage(_pc, _target, _weaponId);
            }
            else if (_weaponId == 261)
            { // アークメイジスタッフ
                L1WeaponSkill.giveArkMageDiseaseEffect(_pc, _target);
            }
            else
            {
                dmg += L1WeaponSkill.getWeaponSkillDamage(_pc, _target, _weaponId);
            }

            dmg -= calcNpcDamageReduction();

            // 使用暴擊增加15點傷害，而奇古獸固定15點傷害
            if (_skillId == L1SkillId.SMASH)
            {
                dmg += 15;
                if (_weaponType2 == 17 || _weaponType2 == 19)
                {
                    dmg = 15;
                }
            }
            // 使用骷髏毀壞增加10點傷害，而奇古獸固定10點傷害
            else if (_skillId == L1SkillId.BONE_BREAK)
            {
                dmg += 10;
                if (_weaponType2 == 17 || _weaponType2 == 19)
                {
                    dmg = 10;
                }
                // 再次發動判斷
                if (!_targetNpc.hasSkillEffect(L1SkillId.BONE_BREAK))
                {
                    int change = RandomHelper.Next(100) + 1;
                    if (change < (30 + RandomHelper.Next(11)))
                    { // 30 ~ 40%
                        L1EffectSpawn.Instance.spawnEffect(93001, 1700, _targetNpc.X, _targetNpc.Y, _targetNpc.MapId);
                        _targetNpc.setSkillEffect(L1SkillId.BONE_BREAK, 2 * 1000); // 發動後再次發動間隔
                                                                                   // 2秒
                        _targetNpc.setSkillEffect(L1SkillId.BONE_BREAK_START, 700);
                    }
                }
            }

            // 非攻城區域對寵物、召喚獸傷害減少
            bool isNowWar = false;
            int castleId = L1CastleLocation.getCastleIdByArea(_targetNpc);
            if (castleId > 0)
            {
                isNowWar = Container.Instance.Resolve<IWarController>().isNowWar(castleId);
            }
            if (!isNowWar)
            {
                if (_targetNpc is L1PetInstance)
                {
                    dmg /= 8;
                }
                else if (_targetNpc is L1SummonInstance)
                {
                    L1SummonInstance summon = (L1SummonInstance)_targetNpc;
                    if (summon.ExsistMaster)
                    {
                        dmg /= 8;
                    }
                }
            }
            if (dmg <= 0)
            {
                _isHit = false;
                _drainHp = 0; // ダメージ無しの場合は吸収による回復はしない
            }

            return (int)dmg;
        }

        // ●●●● ＮＰＣ から プレイヤー へのダメージ算出 ●●●●
        private int calcNpcPcDamage()
        {
            int lvl = _npc.Level;
            double dmg = 0D;
            if (lvl < 10)
            {
                dmg = RandomHelper.Next(lvl) + 10D + _npc.Str / 2 + 1;
            }
            else
            {
                dmg = RandomHelper.Next(lvl) + _npc.Str / 2 + 1;
            }

            if (_npc is L1PetInstance)
            {
                dmg += (lvl / 16); // ペットはLV16毎に追加打撃
                dmg += ((L1PetInstance)_npc).DamageByWeapon;
            }

            dmg += _npc.Dmgup;

            if (UndeadDamage)
            {
                dmg *= 1.1;
            }

            dmg = dmg * Leverage / 10;

            dmg -= calcPcDefense();

            if (_npc.WeaponBreaked)
            { // ＮＰＣがウェポンブレイク中。
                dmg /= 2;
            }

            dmg -= _targetPc.DamageReductionByArmor; // 防具によるダメージ軽減

            // 魔法娃娃效果 - 傷害減免
            dmg -= L1MagicDoll.getDamageReductionByDoll(_targetPc);

            if (_targetPc.hasSkillEffect(L1SkillId.COOKING_1_0_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_1_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_2_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_3_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_4_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_5_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_1_6_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_0_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_1_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_2_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_3_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_4_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_5_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_6_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_0_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_1_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_2_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_3_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_4_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_5_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_6_S))
            {
                dmg -= 5;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.COOKING_1_7_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_2_7_S) || _targetPc.hasSkillEffect(L1SkillId.COOKING_3_7_S))
            {
                dmg -= 5;
            }

            if (_targetPc.hasSkillEffect(L1SkillId.REDUCTION_ARMOR))
            {
                int targetPcLvl = _targetPc.Level;
                if (targetPcLvl < 50)
                {
                    targetPcLvl = 50;
                }
                dmg -= (targetPcLvl - 50) / 5 + 1;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.DRAGON_SKIN))
            {
                dmg -= 2;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.PATIENCE))
            {
                dmg -= 2;
            }
            if (_targetPc.hasSkillEffect(L1SkillId.IMMUNE_TO_HARM))
            {
                dmg /= 2;
            }
            // ペット、サモンからプレイヤーに攻撃
            bool isNowWar = false;
            int castleId = L1CastleLocation.getCastleIdByArea(_targetPc);
            if (castleId > 0)
            {
                isNowWar = Container.Instance.Resolve<IWarController>().isNowWar(castleId);
            }
            if (!isNowWar)
            {
                if (_npc is L1PetInstance)
                {
                    dmg /= 8;
                }
                else if (_npc is L1SummonInstance)
                {
                    L1SummonInstance summon = (L1SummonInstance)_npc;
                    if (summon.ExsistMaster)
                    {
                        dmg /= 8;
                    }
                }
            }

            if (dmg <= 0)
            {
                _isHit = false;
            }

            addNpcPoisonAttack(_npc, _targetPc);

            return (int)dmg;
        }

        // ●●●● ＮＰＣ から ＮＰＣ へのダメージ算出 ●●●●
        private int calcNpcNpcDamage()
        {
            int lvl = _npc.Level;
            double dmg = 0;

            if (_npc is L1PetInstance)
            {
                dmg = RandomHelper.Next(_npc.NpcTemplate.get_level()) + _npc.Str / 2 + 1;
                dmg += (lvl / 16); // ペットはLV16毎に追加打撃
                dmg += ((L1PetInstance)_npc).DamageByWeapon;
            }
            else
            {
                dmg = RandomHelper.Next(lvl) + _npc.Str / 2 + 1;
            }

            if (UndeadDamage)
            {
                dmg *= 1.1;
            }

            dmg = dmg * Leverage / 10;

            dmg -= calcNpcDamageReduction();

            if (_npc.WeaponBreaked)
            { // ＮＰＣがウェポンブレイク中。
                dmg /= 2;
            }

            addNpcPoisonAttack(_npc, _targetNpc);

            if (dmg <= 0)
            {
                _isHit = false;
            }

            return (int)dmg;
        }

        // ●●●● 強化魔法近戰用 ●●●●
        private double calcBuffDamage(double dmg)
        {
            // 火武器、バーサーカーのダメージは1.5倍しない
            if (_pc.hasSkillEffect(L1SkillId.BURNING_SPIRIT) || _pc.hasSkillEffect(L1SkillId.ELEMENTAL_FIRE))
            {
                if ((RandomHelper.Next(100) + 1) <= 33)
                {
                    double tempDmg = dmg;
                    if (_pc.hasSkillEffect(L1SkillId.FIRE_WEAPON))
                    {
                        tempDmg -= 4;
                    }
                    else if (_pc.hasSkillEffect(L1SkillId.FIRE_BLESS))
                    {
                        tempDmg -= 5;
                    }
                    else if (_pc.hasSkillEffect(L1SkillId.BURNING_WEAPON))
                    {
                        tempDmg -= 6;
                    }
                    if (_pc.hasSkillEffect(L1SkillId.BERSERKERS))
                    {
                        tempDmg -= 5;
                    }
                    double diffDmg = dmg - tempDmg;
                    dmg = tempDmg * 1.5 + diffDmg;
                }
            }
            // 鎖鏈劍
            if (_weaponType2 == 18)
            {
                // 弱點曝光 - 傷害加成
                if (_pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV3))
                {
                    dmg += 9;
                }
                else if (_pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV2))
                {
                    dmg += 6;
                }
                else if (_pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV1))
                {
                    dmg += 3;
                }
            }
            // 屠宰者 & 弱點曝光LV3 - 傷害 *1.3
            if (_pc.FoeSlayer && _pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV3))
            {
                dmg *= 1.3;
            }
            if (_pc.hasSkillEffect(L1SkillId.BURNING_SLASH))
            { // 燃燒擊砍
                dmg += 10;
                _pc.sendPackets(new S_EffectLocation(_targetX, _targetY, 6591));
                _pc.broadcastPacket(new S_EffectLocation(_targetX, _targetY, 6591));
                _pc.killSkillEffectTimer(L1SkillId.BURNING_SLASH);
            }

            return dmg;
        }

        // ●●●● プレイヤーのＡＣによるダメージ軽減 ●●●●
        private int calcPcDefense()
        {
            int ac = Math.Max(0, 10 - _targetPc.Ac);
            int acDefMax = _targetPc.ClassFeature.GetAcDefenseMax(ac);
            return RandomHelper.Next(acDefMax + 1);
        }

        // ●●●● ＮＰＣのダメージリダクションによる軽減 ●●●●
        private int calcNpcDamageReduction()
        {
            return _targetNpc.NpcTemplate.get_damagereduction();
        }

        // ●●●● 武器の材質と祝福による追加ダメージ算出 ●●●●
        private int calcMaterialBlessDmg()
        {
            int damage = 0;
            int undead = _targetNpc.NpcTemplate.get_undead();
            if (((_weaponMaterial == 14) || (_weaponMaterial == 17) || (_weaponMaterial == 22)) && ((undead == 1) || (undead == 3) || (undead == 5)))
            { // 銀・ミスリル・オリハルコン、かつ、アンデッド系・アンデッド系ボス・銀特効モンスター
                damage += RandomHelper.Next(20) + 1;
            }
            else if (((_weaponMaterial == 17) || (_weaponMaterial == 22)) && (undead == 2))
            { // ミスリル・オリハルコン、かつ、悪魔系
                damage += RandomHelper.Next(3) + 1;
            }
            if ((_weaponBless == 0) && ((undead == 1) || (undead == 2) || (undead == 3)))
            { // 祝福武器、かつ、アンデッド系・悪魔系・アンデッド系ボス
                damage += RandomHelper.Next(4) + 1;
            }
            if ((_pc.Weapon != null) && (_weaponType != 20) && (_weaponType != 62) && (weapon.HolyDmgByMagic != 0) && ((undead == 1) || (undead == 3)))
            {
                damage += weapon.HolyDmgByMagic;
            }
            return damage;
        }

        // ●●●● 武器の属性強化による追加ダメージ算出 ●●●●
        private int calcAttrEnchantDmg()
        {
            int damage = 0;
            // int weakAttr = _targetNpc.getNpcTemplate().get_weakAttr();
            // if ((weakAttr & 1) == 1 && _weaponAttrEnchantKind == 1 // 地
            // || (weakAttr & 2) == 2 && _weaponAttrEnchantKind == 2 // 火
            // || (weakAttr & 4) == 4 && _weaponAttrEnchantKind == 4 // 水
            // || (weakAttr & 8) == 8 && _weaponAttrEnchantKind == 8) { // 風
            // damage = _weaponAttrEnchantLevel;
            // }
            if (_weaponAttrEnchantLevel == 1)
            {
                damage = 1;
            }
            else if (_weaponAttrEnchantLevel == 2)
            {
                damage = 3;
            }
            else if (_weaponAttrEnchantLevel == 3)
            {
                damage = 5;
            }

            // XXX 耐性処理は本来、耐性合計値ではなく、各値を個別に処理して総和する。
            int resist = 0;
            if (_calcType == PC_PC)
            {
                if (_weaponAttrEnchantKind == 1)
                { // 地
                    resist = _targetPc.Earth;
                }
                else if (_weaponAttrEnchantKind == 2)
                { // 火
                    resist = _targetPc.Fire;
                }
                else if (_weaponAttrEnchantKind == 4)
                { // 水
                    resist = _targetPc.Water;
                }
                else if (_weaponAttrEnchantKind == 8)
                { // 風
                    resist = _targetPc.Wind;
                }
            }
            else if (_calcType == PC_NPC)
            {
                int weakAttr = _targetNpc.NpcTemplate.get_weakAttr();
                if (((_weaponAttrEnchantKind == 1) && (weakAttr == 1)) || ((_weaponAttrEnchantKind == 2) && (weakAttr == 2)) || ((_weaponAttrEnchantKind == 4) && (weakAttr == 4)) || ((_weaponAttrEnchantKind == 8) && (weakAttr == 8)))
                { // 風
                    resist = -50;
                }
            }

            int resistFloor = (int)(0.32 * Math.Abs(resist));
            if (resist >= 0)
            {
                resistFloor *= 1;
            }
            else
            {
                resistFloor *= -1;
            }

            double attrDeffence = resistFloor / 32.0;
            double attrCoefficient = 1 - attrDeffence;

            damage *= (int)attrCoefficient;

            return damage;
        }

        // ●●●● ＮＰＣのアンデッドの夜間攻撃力の変化 ●●●●
        private bool UndeadDamage
        {
            get
            {
                bool flag = false;
                int undead = _npc.NpcTemplate.get_undead();
                bool isNight = Container.Instance.Resolve<IGameTimeClock>().CurrentTime().Night;
                if (isNight && ((undead == 1) || (undead == 3) || (undead == 4)))
                { // 18～6時、かつ、アンデッド系・アンデッド系ボス・弱点無効のアンデッド系
                    flag = true;
                }
                return flag;
            }
        }

        // ●●●● ＮＰＣの毒攻撃を付加 ●●●●
        private void addNpcPoisonAttack(L1Character attacker, L1Character target)
        {
            if (_npc.NpcTemplate.get_poisonatk() != 0)
            { // 毒攻撃あり
                if (15 >= RandomHelper.Next(100) + 1)
                { // 15%の確率で毒攻撃
                    if (_npc.NpcTemplate.get_poisonatk() == 1)
                    { // 通常毒
                      // 3秒周期でダメージ5
                        L1DamagePoison.doInfection(attacker, target, 3000, 5);
                    }
                    else if (_npc.NpcTemplate.get_poisonatk() == 2)
                    { // 沈黙毒
                        L1SilencePoison.doInfection(target);
                    }
                    else if (_npc.NpcTemplate.get_poisonatk() == 4)
                    { // 麻痺毒
                      // 20秒後に45秒間麻痺
                        L1ParalysisPoison.doInfection(target, 20000, 45000);
                    }
                }
            }
            else if (_npc.NpcTemplate.get_paralysisatk() != 0)
            { // 麻痺攻撃あり
            }
        }

        // ■■■■ マナスタッフ、鋼鉄のマナスタッフ、マナバーラードのMP吸収量算出 ■■■■
        public virtual void calcStaffOfMana()
        {
            if ((_weaponId == 126) || (_weaponId == 127))
            { // SOMまたは鋼鉄のSOM
                int som_lvl = _weaponEnchant + 3; // 最大MP吸収量を設定
                if (som_lvl < 0)
                {
                    som_lvl = 0;
                }
                // MP吸収量をランダム取得
                _drainMana = RandomHelper.Next(som_lvl) + 1;
                // 最大MP吸収量を9に制限
                if (_drainMana > Config.MANA_DRAIN_LIMIT_PER_SOM_ATTACK)
                {
                    _drainMana = Config.MANA_DRAIN_LIMIT_PER_SOM_ATTACK;
                }
            }
            else if (_weaponId == 259)
            { // マナバーラード
                if (_calcType == PC_PC)
                {
                    if (_targetPc.Mr <= RandomHelper.Next(100) + 1)
                    { // 確率はターゲットのMRに依存
                        _drainMana = 1; // 吸収量は1固定
                    }
                }
                else if (_calcType == PC_NPC)
                {
                    if (_targetNpc.Mr <= RandomHelper.Next(100) + 1)
                    { // 確率はターゲットのMRに依存
                        _drainMana = 1; // 吸収量は1固定
                    }
                }
            }
        }

        // ■■■■ ディストラクションのHP吸収量算出 ■■■■
        private int calcDestruction(int dmg)
        {
            _drainHp = (dmg / 8) + 1;
            return _drainHp > 0 ? _drainHp : 1;
        }

        // ■■■■ ＰＣの毒攻撃を付加 ■■■■
        public virtual void addPcPoisonAttack(L1Character attacker, L1Character target)
        {
            int chance = RandomHelper.Next(100) + 1;
            if (((_weaponId == 13) || (_weaponId == 44) || ((_weaponId != 0) && _pc.hasSkillEffect(L1SkillId.ENCHANT_VENOM))) && (chance <= 10))
            {
                // 通常毒、3秒周期、ダメージHP-5
                L1DamagePoison.doInfection(attacker, target, 3000, 5);
            }
            else
            {
                // 魔法娃娃效果 - 中毒
                if (L1MagicDoll.getEffectByDoll(attacker, 1) == 1)
                {
                    L1DamagePoison.doInfection(attacker, target, 3000, 5);
                }
            }
        }

        // ■■■■ 底比斯武器攻撃付加 ■■■■
        public virtual void addChaserAttack()
        {
            if (5 > RandomHelper.Next(100) + 1)
            {
                if (_weaponId == 265 || _weaponId == 266 || _weaponId == 267 || _weaponId == 268 || _weaponId == 280 || _weaponId == 281)
                {
                    L1Chaser chaser = new L1Chaser(_pc, _target, L1Skills.ATTR_EARTH, 7025);
                    chaser.begin();
                }
                else if (_weaponId == 276 || _weaponId == 277)
                {
                    L1Chaser chaser = new L1Chaser(_pc, _target, L1Skills.ATTR_WATER, 7179);
                    chaser.begin();
                }
                else if (_weaponId == 304 || _weaponId == 307 || _weaponId == 308)
                {
                    L1Chaser chaser = new L1Chaser(_pc, _target, L1Skills.ATTR_WATER, 8150);
                    chaser.begin();
                }
                else if (_weaponId == 305 || _weaponId == 306 || _weaponId == 309)
                {
                    L1Chaser chaser = new L1Chaser(_pc, _target, L1Skills.ATTR_WATER, 8152);
                    chaser.begin();
                }
            }
        }

        /* ■■■■■■■■■■■■■■ 攻撃モーション送信 ■■■■■■■■■■■■■■ */
        public virtual void action()
        {
            if (_calcType == PC_PC || _calcType == PC_NPC)
            {
                actionPc();
            }
            else if (_calcType == NPC_PC || _calcType == NPC_NPC)
            {
                actionNpc();
            }
        }

        // ●●●● ＰＣ攻擊動作 ●●●●
        public virtual void actionPc()
        {
            _attckActId = 1;
            bool isFly = false;
            _pc.Heading = _pc.targetDirection(_targetX, _targetY); // 改變面向

            if (_weaponType == 20 && (_arrow != null || _weaponId == 190))
            { // 弓 有箭或沙哈之弓
                if (_arrow != null)
                { // 弓 - 有箭
                    this.pcInventory.removeItem(_arrow, 1);
                    _attckGrfxId = 66; // 箭
                }
                else if (_weaponId == 190) // 沙哈 - 無箭
                {
                    _attckGrfxId = 2349; // 魔法箭
                }

                if (_pc.TempCharGfx == 8719) // 柑橘
                {
                    _attckGrfxId = 8721; // 橘子籽
                }

                if (_pc.TempCharGfx == 8900) // 海露拜
                {
                    _attckGrfxId = 8904; // 魔法箭
                }

                if (_pc.TempCharGfx == 8913) // 朱里安
                {
                    _attckGrfxId = 8916; // 魔法箭
                }

                isFly = true;
            }
            else if ((_weaponType == 62) && (_sting != null))
            { // 鐵手甲 - 有飛刀
                this.pcInventory.removeItem(_sting, 1);
                _attckGrfxId = 2989; // 飛刀
                isFly = true;
            }

            if (!_isHit)
            { // Miss
                _damage = 0;
            }

            int[] data = null;

            if (isFly)
            { // 遠距離攻擊
                data = new int[] { _attckActId, _damage, _attckGrfxId };
                _pc.sendPackets(new S_UseArrowSkill(_pc, _targetId, _targetX, _targetY, data));
                _pc.broadcastPacket(new S_UseArrowSkill(_pc, _targetId, _targetX, _targetY, data));
            }
            else
            { // 近距離攻擊
                data = new int[] { _attckActId, _damage, _effectId };
                _pc.sendPackets(new S_AttackPacket(_pc, _targetId, data));
                _pc.broadcastPacket(new S_AttackPacket(_pc, _targetId, data));
            }

            if (_isHit)
            {
                _target.broadcastPacketExceptTargetSight(new S_DoActionGFX(_targetId, ActionCodes.ACTION_Damage), _pc);
            }
        }

        // ●●●● ＮＰＣ攻擊動作 ●●●●
        private void actionNpc()
        {
            int bowActId = 0;
            int npcGfxid = _npc.TempCharGfx;
            int actId = Container.Instance.Resolve<IGameActionProvider>().getSpecialAttack(npcGfxid); // 特殊攻擊動作
            double dmg = _damage;
            int[] data = null;

            _npc.Heading = _npc.targetDirection(_targetX, _targetY); // 改變面向

            // 與目標距離2格以上
            bool isLongRange = false;
            if (npcGfxid == 4521 || npcGfxid == 4550 || npcGfxid == 5062 || npcGfxid == 5317 || npcGfxid == 5324 || npcGfxid == 5331 || npcGfxid == 5338 || npcGfxid == 5412)
            {
                isLongRange = (_npc.Location.getTileLineDistance(new Point(_targetX, _targetY)) > 2);
            }
            else
            {
                isLongRange = (_npc.Location.getTileLineDistance(new Point(_targetX, _targetY)) > 1);
            }
            bowActId = _npc.PolyArrowGfx; // 被變身後的遠距圖像
            if (bowActId == 0)
            {
                bowActId = _npc.NpcTemplate.BowActId;
            }
            if (ActId == 0)
            {
                if ((actId != 0) && ((RandomHelper.Next(100) + 1) <= 40))
                {
                    dmg *= 1.2;
                }
                else
                {
                    if (!isLongRange || bowActId == 0)
                    { // 近距離
                        actId = Container.Instance.Resolve<IGameActionProvider>().getDefaultAttack(npcGfxid);
                        if (bowActId > 0)
                        { // 遠距離怪物，近距離時攻擊力加成
                            dmg *= 1.2;
                        }
                    }
                    else
                    { // 遠距離
                        actId = Container.Instance.Resolve<IGameActionProvider>().getRangedAttack(npcGfxid);
                    }
                }
            }
            else
            {
                actId = ActId; // 攻擊動作由 mobskill控制
            }
            _damage = (int)dmg;

            if (!_isHit)
            { // Miss
                _damage = 0;
            }

            // 距離2格以上攻使用 弓 攻擊
            if (isLongRange && (bowActId > 0))
            {
                data = new int[] { actId, _damage, bowActId }; // data = {actid,
                                                               // dmg, spellgfx}
                _npc.broadcastPacket(new S_UseArrowSkill(_npc, _targetId, _targetX, _targetY, data));
            }
            else
            {
                if (GfxId > 0)
                {
                    data = new int[] { actId, _damage, GfxId, 6 }; // data =
                                                                   // {actid,
                                                                   // dmg,
                                                                   // spellgfx,
                                                                   // use_type}
                    _npc.broadcastPacket(new S_UseAttackSkill(_npc, _targetId, _targetX, _targetY, data));
                }
                else
                {
                    data = new int[] { actId, _damage, 0 }; // data = {actid, dmg,
                                                            // effect}
                    _npc.broadcastPacket(new S_AttackPacket(_npc, _targetId, data));
                }
            }
            if (_isHit)
            {
                _target.broadcastPacketExceptTargetSight(new S_DoActionGFX(_targetId, ActionCodes.ACTION_Damage), _npc);
            }
        }

        /*
		 * // 飛び道具（矢、スティング）がミスだったときの軌道を計算 public void calcOrbit(int cx, int cy, int
		 * head) // 起点Ｘ 起点Ｙ 今向いてる方向 { float dis_x = Math.abs(cx - _targetX); //
		 * Ｘ方向のターゲットまでの距離 float dis_y = Math.abs(cy - _targetY); // Ｙ方向のターゲットまでの距離
		 * float dis = Math.max(dis_x, dis_y); // ターゲットまでの距離 float avg_x = 0; float
		 * avg_y = 0; if (dis == 0) { // 目標と同じ位置なら向いてる方向へ真っ直ぐ if (head == 1) { avg_x
		 * = 1; avg_y = -1; } else if (head == 2) { avg_x = 1; avg_y = 0; } else if
		 * (head == 3) { avg_x = 1; avg_y = 1; } else if (head == 4) { avg_x = 0;
		 * avg_y = 1; } else if (head == 5) { avg_x = -1; avg_y = 1; } else if (head
		 * == 6) { avg_x = -1; avg_y = 0; } else if (head == 7) { avg_x = -1; avg_y
		 * = -1; } else if (head == 0) { avg_x = 0; avg_y = -1; } } else { avg_x =
		 * dis_x / dis; avg_y = dis_y / dis; }
		 * 
		 * int add_x = (int) Math.floor((avg_x * 15) + 0.59f); // 上下左右がちょっと優先な丸め int
		 * add_y = (int) Math.floor((avg_y * 15) + 0.59f); // 上下左右がちょっと優先な丸め
		 * 
		 * if (cx > _targetX) { add_x *= -1; } if (cy > _targetY) { add_y *= -1; }
		 * 
		 * _targetX = _targetX + add_x; _targetY = _targetY + add_y; }
		 */

        /* ■■■■■■■■■■■■■■■ 計算結果反映 ■■■■■■■■■■■■■■■ */

        public virtual void commit()
        {
            if (_isHit)
            {
                if ((_calcType == PC_PC) || (_calcType == NPC_PC))
                {
                    commitPc();
                }
                else if ((_calcType == PC_NPC) || (_calcType == NPC_NPC))
                {
                    commitNpc();
                }
            }

            // ダメージ値及び命中率確認用メッセージ
            if (!Config.ALT_ATKMSG)
            {
                return;
            }
            if (Config.ALT_ATKMSG)
            {
                if (((_calcType == PC_PC) || (_calcType == PC_NPC)) && !_pc.Gm)
                {
                    return;
                }
                if (((_calcType == PC_PC) || (_calcType == NPC_PC)) && !_targetPc.Gm)
                {
                    return;
                }
            }
            string msg0 = "";
            string msg1 = " 造成 ";
            string msg2 = "";
            string msg3 = "";
            string msg4 = "";
            if ((_calcType == PC_PC) || (_calcType == PC_NPC))
            { // アタッカーがＰＣの場合
                msg0 = "物攻 對";
            }
            else if (_calcType == NPC_PC)
            { // アタッカーがＮＰＣの場合
                msg0 = _npc.NameId + "(物攻)：";
            }

            if ((_calcType == NPC_PC) || (_calcType == PC_PC))
            { // ターゲットがＰＣの場合
                msg4 = _targetPc.Name;
                msg2 = "，剩餘 " + _targetPc.CurrentHp + "，命中	" + _hitRate + "%";
            }
            else if (_calcType == PC_NPC)
            { // ターゲットがＮＰＣの場合
                msg4 = _targetNpc.NameId;
                msg2 = "，剩餘 " + _targetNpc.CurrentHp + "，命中 " + _hitRate + "%";
            }
            msg3 = _isHit ? _damage + " 傷害" : "  0 傷害";

            // 物攻 對 目標 造成 X 傷害，剩餘 Y，命中 Z %。
            if ((_calcType == PC_PC) || (_calcType == PC_NPC))
            {
                _pc.sendPackets(new S_ServerMessage(166, msg0, msg1, msg2, msg3, msg4));
            }
            // 攻擊者(物攻)： X傷害，剩餘 Y，命中%。
            else if ((_calcType == NPC_PC))
            {
                _targetPc.sendPackets(new S_ServerMessage(166, msg0, null, msg2, msg3, null));
            }
        }

        // ●●●● プレイヤーに計算結果を反映 ●●●●
        private void commitPc()
        {
            if (_calcType == PC_PC)
            {
                if ((_drainMana > 0) && (_targetPc.CurrentMp > 0))
                {
                    if (_drainMana > _targetPc.CurrentMp)
                    {
                        _drainMana = _targetPc.CurrentMp;
                    }
                    short newMp = (short)(_targetPc.CurrentMp - _drainMana);
                    _targetPc.CurrentMp = newMp;
                    newMp = (short)(_pc.CurrentMp + _drainMana);
                    _pc.CurrentMp = newMp;
                }
                if (_drainHp > 0)
                { // HP吸収による回復
                    short newHp = (short)(_pc.CurrentHp + _drainHp);
                    _pc.CurrentHp = newHp;
                }
                damagePcWeaponDurability(); // 武器を損傷させる。
                _targetPc.receiveDamage(_pc, _damage, false);
            }
            else if (_calcType == NPC_PC)
            {
                _targetPc.receiveDamage(_npc, _damage, false);
            }
        }

        // ●●●● ＮＰＣに計算結果を反映 ●●●●
        private void commitNpc()
        {
            if (_calcType == PC_NPC)
            {
                if (_drainMana > 0)
                {
                    int drainValue = _targetNpc.drainMana(_drainMana);
                    int newMp = _pc.CurrentMp + drainValue;
                    _pc.CurrentMp = newMp;
                    if (drainValue > 0)
                    {
                        int newMp2 = _targetNpc.CurrentMp - drainValue;
                        _targetNpc.CurrentMpDirect = newMp2;
                    }
                }
                if (_drainHp > 0)
                { // HP吸収による回復
                    short newHp = (short)(_pc.CurrentHp + _drainHp);
                    _pc.CurrentHp = newHp;
                }
                damageNpcWeaponDurability(); // 武器を損傷させる。
                _targetNpc.receiveDamage(_pc, _damage);
            }
            else if (_calcType == NPC_NPC)
            {
                _targetNpc.receiveDamage(_npc, _damage);
            }
        }

        /* ■■■■■■■■■■■■■■■ カウンターバリア ■■■■■■■■■■■■■■■ */

        // ■■■■ カウンターバリア時の攻撃モーション送信 ■■■■
        public virtual void actionCounterBarrier()
        {
            if (_calcType == PC_PC)
            {
                _pc.Heading = _pc.targetDirection(_targetX, _targetY); // 向きのセット
                _pc.sendPackets(new S_AttackMissPacket(_pc, _targetId));
                _pc.broadcastPacket(new S_AttackMissPacket(_pc, _targetId));
                _pc.sendPackets(new S_DoActionGFX(_pc.Id, ActionCodes.ACTION_Damage));
                _pc.broadcastPacket(new S_DoActionGFX(_pc.Id, ActionCodes.ACTION_Damage));
            }
            else if (_calcType == NPC_PC)
            {
                int actId = 0;
                _npc.Heading = _npc.targetDirection(_targetX, _targetY); // 向きのセット
                if (ActId > 0)
                {
                    actId = ActId;
                }
                else
                {
                    actId = ActionCodes.ACTION_Attack;
                }
                if (GfxId > 0)
                {
                    int[] data = new int[] { actId, 0, GfxId, 6 }; // data = {actId, dmg, getGfxId(), use_type}
                    _npc.broadcastPacket(new S_UseAttackSkill(_target, _npc.Id, _targetX, _targetY, data));
                }
                else
                {
                    _npc.broadcastPacket(new S_AttackMissPacket(_npc, _targetId, actId));
                }
                _npc.broadcastPacket(new S_DoActionGFX(_npc.Id, ActionCodes.ACTION_Damage));
            }
        }

        // ■■■■ 相手の攻撃に対してカウンターバリアが有効かを判別 ■■■■
        public virtual bool ShortDistance
        {
            get
            {
                bool isShortDistance = true;
                if (_calcType == PC_PC)
                {
                    if ((_weaponType == 20) || (_weaponType == 62))
                    { // 弓かガントレット
                        isShortDistance = false;
                    }
                }
                else if (_calcType == NPC_PC)
                {
                    bool isLongRange = (_npc.Location.getTileLineDistance(new Point(_targetX, _targetY)) > 1);
                    int bowActId = _npc.PolyArrowGfx;
                    if (bowActId == 0)
                    {
                        bowActId = _npc.NpcTemplate.BowActId;
                    }
                    // 距離が2以上、攻撃者の弓のアクションIDがある場合は遠攻撃
                    if (isLongRange && (bowActId > 0))
                    {
                        isShortDistance = false;
                    }
                }
                return isShortDistance;
            }
        }

        // ■■■■ カウンターバリアのダメージを反映 ■■■■
        public virtual void commitCounterBarrier()
        {
            int damage = calcCounterBarrierDamage();
            if (damage == 0)
            {
                return;
            }
            if (_calcType == PC_PC)
            {
                _pc.receiveDamage(_targetPc, damage, false);
            }
            else if (_calcType == NPC_PC)
            {
                _npc.receiveDamage(_targetPc, damage);
            }
        }

        // ●●●● カウンターバリアのダメージを算出 ●●●●
        private int calcCounterBarrierDamage()
        {
            int damage = 0;
            L1ItemInstance weapon = null;
            weapon = _targetPc.Weapon;
            if (weapon != null)
            {
                if (weapon.Item.Type == 3)
                { // 両手剣
                  // (BIG最大ダメージ+強化数+追加ダメージ)*2
                    damage = (weapon.Item.DmgLarge + weapon.EnchantLevel + weapon.Item.DmgModifier) * 2;
                }
            }
            return damage;
        }

        /*
		 * 武器を損傷させる。 対NPCの場合、損傷確率は10%とする。祝福武器は3%とする。
		 */
        private void damageNpcWeaponDurability()
        {
            int chance = 10;
            int bchance = 3;

            /*
			 * 損傷しないNPC、素手、損傷しない武器使用、SOF中の場合何もしない。
			 */
            if ((_calcType != PC_NPC) || (_targetNpc.NpcTemplate.is_hard() == false) || (_weaponType == 0) || (weapon.Item.get_canbedmg() == 0) || _pc.hasSkillEffect(L1SkillId.SOUL_OF_FLAME))
            {
                return;
            }
            // 通常の武器・呪われた武器
            if (((_weaponBless == 1) || (_weaponBless == 2)) && ((RandomHelper.Next(100) + 1) < chance))
            {
                // \f1あなたの%0が損傷しました。
                _pc.sendPackets(new S_ServerMessage(268, weapon.LogName));
                this.pcInventory.receiveDamage(weapon);
            }
            // 祝福された武器
            if ((_weaponBless == 0) && ((RandomHelper.Next(100) + 1) < bchance))
            {
                // \f1あなたの%0が損傷しました。
                _pc.sendPackets(new S_ServerMessage(268, weapon.LogName));
                this.pcInventory.receiveDamage(weapon);
            }
        }

        /*
		 * バウンスアタックにより武器を損傷させる。 バウンスアタックの損傷確率は10%
		 */
        private void damagePcWeaponDurability()
        {
            // PvP以外、素手、弓、ガントトレット、相手がバウンスアタック未使用、SOF中の場合何もしない
            if ((_calcType != PC_PC) || (_weaponType == 0) || (_weaponType == 20) || (_weaponType == 62) || (_targetPc.hasSkillEffect(L1SkillId.BOUNCE_ATTACK) == false) || _pc.hasSkillEffect(L1SkillId.SOUL_OF_FLAME))
            {
                return;
            }

            if (RandomHelper.Next(100) + 1 <= 10)
            {
                // \f1あなたの%0が損傷しました。
                _pc.sendPackets(new S_ServerMessage(268, weapon.LogName));
                this.pcInventory.receiveDamage(weapon);
            }
        }

        /// <summary>
        /// 弱點曝光 </summary>
        private void WeaknessExposure()
        {
            if (weapon != null)
            {
                int random = RandomHelper.Next(100) + 1;
                if (_weaponType2 == 18)
                { // 鎖鏈劍
                  // 使用屠宰者...
                    if (_pc.FoeSlayer)
                    {
                        return;
                    }
                    if (_pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV3))
                    { // 目前階段三
                        if (random > 30 && random <= 60)
                        { // 階段三
                            _pc.killSkillEffectTimer(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV3);
                            _pc.setSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV3, 16 * 1000);
                            _pc.sendPackets(new S_SkillIconGFX(75, 3));
                        }
                    }
                    else if (_pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV2))
                    { // 目前階段二
                        if (random <= 30)
                        { // 階段二
                            _pc.killSkillEffectTimer(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV2);
                            _pc.setSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV2, 16 * 1000);
                            _pc.sendPackets(new S_SkillIconGFX(75, 2));
                        }
                        else if (random >= 70)
                        { // 階段三
                            _pc.killSkillEffectTimer(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV2);
                            _pc.setSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV3, 16 * 1000);
                            _pc.sendPackets(new S_SkillIconGFX(75, 3));
                        }
                    }
                    else if (_pc.hasSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV1))
                    { // 目前階段一
                        if (random <= 40)
                        { // 階段一
                            _pc.killSkillEffectTimer(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV1);
                            _pc.setSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV1, 16 * 1000);
                            _pc.sendPackets(new S_SkillIconGFX(75, 1));
                        }
                        else if (random >= 70)
                        { // 階段二
                            _pc.killSkillEffectTimer(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV1);
                            _pc.setSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV2, 16 * 1000);
                            _pc.sendPackets(new S_SkillIconGFX(75, 2));
                        }
                    }
                    else
                    {
                        if (random <= 40)
                        { // 階段一
                            _pc.setSkillEffect(L1SkillId.SPECIAL_EFFECT_WEAKNESS_LV1, 16 * 1000);
                            _pc.sendPackets(new S_SkillIconGFX(75, 1));
                        }
                    }
                }
            }
        }
    }

}