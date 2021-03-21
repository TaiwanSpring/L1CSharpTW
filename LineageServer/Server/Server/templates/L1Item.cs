using System;
namespace LineageServer.Server.Server.Templates
{

    [Serializable]
    public abstract class L1Item : ICloneable
    {

        private const long serialVersionUID = 1L;

        public L1Item()
        {
        }

        public virtual object clone()
        {
            throw new NotImplementedException();
        }

        // ■■■■■■ L1EtcItem,L1Weapon,L1Armor に共通する項目 ■■■■■■

        private int _type2; // ● 0=L1EtcItem, 1=L1Weapon, 2=L1Armor

        /// <returns> 0 if L1EtcItem, 1 if L1Weapon, 2 if L1Armor </returns>
        public virtual int Type2
        {
            get
            {
                return _type2;
            }
            set
            {
                _type2 = value;
            }
        }


        private int _itemId; // ● アイテムＩＤ

        public virtual int ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
            }
        }


        private string _name; // ● アイテム名

        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }


        private string _unidentifiedNameId; // ● 未鑑定アイテムのネームＩＤ

        public virtual string UnidentifiedNameId
        {
            get
            {
                return _unidentifiedNameId;
            }
            set
            {
                _unidentifiedNameId = value;
            }
        }


        private string _identifiedNameId; // ● 鑑定済みアイテムのネームＩＤ

        public virtual string IdentifiedNameId
        {
            get
            {
                return _identifiedNameId;
            }
            set
            {
                _identifiedNameId = value;
            }
        }


        private int _type; // ● 詳細なタイプ

        /// <summary>
        /// アイテムの種類を返す。<br>
        /// 
        /// </para>
        /// </summary>
        /// <returns> <para>
        ///         [etcitem]<br>
        ///         0:arrow, 1:wand, 2:light, 3:gem, 4:totem, 5:firecracker,
        ///         6:potion, 7:food, 8:scroll, 9:questitem, 10:spellbook,
        ///         11:petitem, 12:other, 13:material, 14:event, 15:sting,
        ///		   16:treasure_box, 17:magic_doll, 18:spellscroll, 19:spellwand,
        ///         20:spellicon, 21:protect_scroll
        ///         </p>
        ///         <para>
        ///         [weapon]<br>
        ///         1:sword, 2:twohandsword, 3:dagger, 4:bow, 5:arrow, 6:spear,
        ///		   7:blunt, 8:staff, 9:claw, 10:dualsword, 11:gauntlet, 12:sting,
        ///		   13:chainsword, 14:kiringku
        ///         </para>
        ///         <para>
        ///         [armor]<br>
        ///         1:helm, 2:t_shirts, 3:armor, 4:cloak, 5:glove, 6:boots, 7:shield,
        ///         8:guarder, 10:amulet, 11:ring, 12:earring, 13:belt, 
        ///		   14:pattern_back, 15:pattern_left, 16:pattern_right,
        ///		   17:talisman_left, 18:talisman_right
        ///         </para> </returns>
        public virtual int Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }


        private int _type1; // ● タイプ

        /// <summary>
        /// アイテムの種類を返す。<br>
        /// 
        /// </para>
        /// </summary>
        /// <returns> <para>
        ///         [weapon]<br>
        ///         sword:4, dagger:46, tohandsword:50, bow:20, blunt:11, spear:24,
        ///         staff:40, throwingknife:2922, arrow:66, gauntlet:62, claw:58,
        ///         edoryu:54, singlebow:20, singlespear:24, tohandblunt:11,
        ///         tohandstaff:40, kiringku:58, chainsword:24
        ///         </p> </returns>
        public virtual int Type1
        {
            get
            {
                return _type1;
            }
            set
            {
                _type1 = value;
            }
        }


        private int _material; // ● 素材

        /// <summary>
        /// アイテムの素材を返す
        /// </summary>
        /// <returns> 0:none 1:液体 2:web 3:植物性 4:動物性 5:紙 6:布 7:皮 8:木 9:骨 10:竜の鱗 11:鉄
        ///         12:鋼鉄 13:銅 14:銀 15:金 16:プラチナ 17:ミスリル 18:ブラックミスリル 19:玻璃 20:宝石
        ///         21:鉱物 22:オリハルコン </returns>
        public virtual int Material
        {
            get
            {
                return _material;
            }
            set
            {
                _material = value;
            }
        }


        private int _weight; // ● 重量

        public virtual int Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }


        private int _gfxId; // ● インベントリ内のグラフィックＩＤ

        public virtual int GfxId
        {
            get
            {
                return _gfxId;
            }
            set
            {
                _gfxId = value;
            }
        }


        private int _groundGfxId; // ● 地面に置いた時のグラフィックＩＤ

        public virtual int GroundGfxId
        {
            get
            {
                return _groundGfxId;
            }
            set
            {
                _groundGfxId = value;
            }
        }


        private int _minLevel; // ● 使用、装備可能最小ＬＶ

        private int _itemDescId;

        /// <summary>
        /// 鑑定時に表示されるItemDesc.tblのメッセージIDを返す。
        /// </summary>
        public virtual int ItemDescId
        {
            get
            {
                return _itemDescId;
            }
            set
            {
                _itemDescId = value;
            }
        }


        public virtual int MinLevel
        {
            get
            {
                return _minLevel;
            }
            set
            {
                _minLevel = value;
            }
        }


        private int _maxLevel; // ● 使用、装備可能最大ＬＶ

        public virtual int MaxLevel
        {
            get
            {
                return _maxLevel;
            }
            set
            {
                _maxLevel = value;
            }
        }


        private int _bless; // ● 祝福状態

        public virtual int Bless
        {
            get
            {
                return _bless;
            }
            set
            {
                _bless = value;
            }
        }


        private bool _tradable; // ● トレード可／不可

        public virtual bool Tradable
        {
            get
            {
                return _tradable;
            }
            set
            {
                _tradable = value;
            }
        }


        private bool _cantDelete; // ● 削除不可

        public virtual bool CantDelete
        {
            get
            {
                return _cantDelete;
            }
            set
            {
                _cantDelete = value;
            }
        }


        private bool _save_at_once;

        /// <summary>
        /// アイテムの個数が変化した際にすぐにDBに書き込むべきかを返す。
        /// </summary>
        public virtual bool ToBeSavedAtOnce
        {
            get
            {
                return _save_at_once;
            }
            set
            {
                _save_at_once = value;
            }
        }


        // ■■■■■■ L1EtcItem,L1Weapon に共通する項目 ■■■■■■

        private int _dmgSmall = 0; // ● 最小ダメージ

        public virtual int DmgSmall
        {
            get
            {
                return _dmgSmall;
            }
            set
            {
                _dmgSmall = value;
            }
        }


        private int _dmgLarge = 0; // ● 最大ダメージ

        public virtual int DmgLarge
        {
            get
            {
                return _dmgLarge;
            }
            set
            {
                _dmgLarge = value;
            }
        }


        // ■■■■■■ L1EtcItem,L1Armor に共通する項目 ■■■■■■

        // ■■■■■■ L1Weapon,L1Armor に共通する項目 ■■■■■■

        private int _safeEnchant = 0; // ● ＯＥ安全圏

        public virtual int get_safeenchant()
        {
            return _safeEnchant;
        }

        public virtual void set_safeenchant(int safeenchant)
        {
            _safeEnchant = safeenchant;
        }

        private bool _useRoyal = false; // ● ロイヤルクラスが装備できるか

        public virtual bool UseRoyal
        {
            get
            {
                return _useRoyal;
            }
            set
            {
                _useRoyal = value;
            }
        }


        private bool _useKnight = false; // ● ナイトクラスが装備できるか

        public virtual bool UseKnight
        {
            get
            {
                return _useKnight;
            }
            set
            {
                _useKnight = value;
            }
        }


        private bool _useElf = false; // ● エルフクラスが装備できるか

        public virtual bool UseElf
        {
            get
            {
                return _useElf;
            }
            set
            {
                _useElf = value;
            }
        }


        private bool _useMage = false; // ● メイジクラスが装備できるか

        public virtual bool UseMage
        {
            get
            {
                return _useMage;
            }
            set
            {
                _useMage = value;
            }
        }


        private bool _useDarkelf = false; // ● ダークエルフクラスが装備できるか

        public virtual bool UseDarkelf
        {
            get
            {
                return _useDarkelf;
            }
            set
            {
                _useDarkelf = value;
            }
        }


        private bool _useDragonknight = false; // ● ドラゴンナイト裝備できるか

        public virtual bool UseDragonknight
        {
            get
            {
                return _useDragonknight;
            }
            set
            {
                _useDragonknight = value;
            }
        }


        private bool _useIllusionist = false; // ● イリュージョニスト裝備できるか

        public virtual bool UseIllusionist
        {
            get
            {
                return _useIllusionist;
            }
            set
            {
                _useIllusionist = value;
            }
        }


        private sbyte _addstr = 0; // ● ＳＴＲ補正

        public virtual sbyte get_addstr()
        {
            return _addstr;
        }

        public virtual void set_addstr(sbyte addstr)
        {
            _addstr = addstr;
        }

        private sbyte _adddex = 0; // ● ＤＥＸ補正

        public virtual sbyte get_adddex()
        {
            return _adddex;
        }

        public virtual void set_adddex(sbyte adddex)
        {
            _adddex = adddex;
        }

        private sbyte _addcon = 0; // ● ＣＯＮ補正

        public virtual sbyte get_addcon()
        {
            return _addcon;
        }

        public virtual void set_addcon(sbyte addcon)
        {
            _addcon = addcon;
        }

        private sbyte _addint = 0; // ● ＩＮＴ補正

        public virtual sbyte get_addint()
        {
            return _addint;
        }

        public virtual void set_addint(sbyte addint)
        {
            _addint = addint;
        }

        private sbyte _addwis = 0; // ● ＷＩＳ補正

        public virtual sbyte get_addwis()
        {
            return _addwis;
        }

        public virtual void set_addwis(sbyte addwis)
        {
            _addwis = addwis;
        }

        private sbyte _addcha = 0; // ● ＣＨＡ補正

        public virtual sbyte get_addcha()
        {
            return _addcha;
        }

        public virtual void set_addcha(sbyte addcha)
        {
            _addcha = addcha;
        }

        private int _addhp = 0; // ● ＨＰ補正

        public virtual int get_addhp()
        {
            return _addhp;
        }

        public virtual void set_addhp(int addhp)
        {
            _addhp = addhp;
        }

        private int _addmp = 0; // ● ＭＰ補正

        public virtual int get_addmp()
        {
            return _addmp;
        }

        public virtual void set_addmp(int addmp)
        {
            _addmp = addmp;
        }

        private int _addhpr = 0; // ● ＨＰＲ補正

        public virtual int get_addhpr()
        {
            return _addhpr;
        }

        public virtual void set_addhpr(int addhpr)
        {
            _addhpr = addhpr;
        }

        private int _addmpr = 0; // ● ＭＰＲ補正

        public virtual int get_addmpr()
        {
            return _addmpr;
        }

        public virtual void set_addmpr(int addmpr)
        {
            _addmpr = addmpr;
        }

        private int _addsp = 0; // ● ＳＰ補正

        public virtual int get_addsp()
        {
            return _addsp;
        }

        public virtual void set_addsp(int addsp)
        {
            _addsp = addsp;
        }

        private int _mdef = 0; // ● ＭＲ

        public virtual int get_mdef()
        {
            return _mdef;
        }

        public virtual void set_mdef(int i)
        {
            this._mdef = i;
        }

        private bool _isHasteItem = false; // ● ヘイスト効果の有無

        public virtual bool HasteItem
        {
            get
            {
                return _isHasteItem;
            }
            set
            {
                _isHasteItem = value;
            }
        }


        private int _maxUseTime = 0; // ● 使用可能な時間

        public virtual int MaxUseTime
        {
            get
            {
                return _maxUseTime;
            }
            set
            {
                _maxUseTime = value;
            }
        }


        private int _useType;

        /// <summary>
        /// 使用したときのリアクションを決定するタイプを返す。
        /// </summary>
        public virtual int UseType
        {
            get
            {
                return _useType;
            }
            set
            {
                _useType = value;
            }
        }


        private int _foodVolume;

        /// <summary>
        /// 肉などのアイテムに設定されている満腹度を返す。
        /// </summary>
        public virtual int FoodVolume
        {
            get
            {
                return _foodVolume;
            }
            set
            {
                _foodVolume = value;
            }
        }


        /// <summary>
        /// ランプなどのアイテムに設定されている明るさを返す。
        /// </summary>
        public virtual int LightRange
        {
            get
            {
                if (_itemId == 40001)
                { // ランプ
                    return 11;
                }
                else if (_itemId == 40002)
                { // ランタン
                    return 14;
                }
                else if (_itemId == 40004)
                { // マジックランタン
                    return 14;
                }
                else if (_itemId == 40005)
                { // キャンドル
                    return 8;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// ランプなどの燃料の量を返す。
        /// </summary>
        public virtual int LightFuel
        {
            get
            {
                if (_itemId == 40001)
                { // ランプ
                    return 6000;
                }
                else if (_itemId == 40002)
                { // ランタン
                    return 12000;
                }
                else if (_itemId == 40003)
                { // ランタンオイル
                    return 12000;
                }
                else if (_itemId == 40004)
                { // マジックランタン
                    return 0;
                }
                else if (_itemId == 40005)
                { // キャンドル
                    return 600;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 魔法素材的種類封包
        /// </summary>
        public virtual int MagicCatalystType
        {
            get
            {
                int type = 0;

                switch (ItemId)
                {
                    case 40318: // 魔法寶石
                        type = 166;
                        break;
                    case 40319: // 精靈玉
                        type = 569;
                        break;
                    case 40321: // 二級黑魔石
                        type = 837;
                        break;
                    case 49158: // 生命之樹果實
                        type = 3674;
                        break;
                    case 49157: // 刻印的骨頭片
                        type = 3605;
                        break;
                    case 49156: // 屬性石
                        type = 3606;
                        break;
                }

                return type;
            }
        }

        // ■■■■■■ L1EtcItem でオーバーライドする項目 ■■■■■■
        public virtual bool Stackable
        {
            get
            {
                return false;
            }
        }

        public virtual int get_locx()
        {
            return 0;
        }

        public virtual int get_locy()
        {
            return 0;
        }

        public virtual short get_mapid()
        {
            return 0;
        }

        public virtual int get_delayid()
        {
            return 0;
        }

        public virtual int get_delaytime()
        {
            return 0;
        }

        public virtual int MaxChargeCount
        {
            get
            {
                return 0;
            }
        }


        private bool _isCanSeal; // ● 封印スクロールで封印可能
        public virtual bool CanSeal
        {
            get
            {
                return _isCanSeal;
            }
            set
            {
                _isCanSeal = value;
            }
        }

        // ■■■■■■ L1Weapon でオーバーライドする項目 ■■■■■■
        private int _range = 0; // ● 射程範囲
        public virtual int Range
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value;
            }
        }
        private int _hitModifier = 0; // ● 命中率補正
        public virtual int HitModifier
        {
            get
            {
                return _hitModifier;
            }
            set
            {
                _hitModifier = value;
            }
        }
        private int _dmgModifier = 0; // ● ダメージ補正
        public virtual int DmgModifier
        {
            get
            {
                return _dmgModifier;
            }
            set
            {
                _dmgModifier = value;
            }
        }
        private int _doubleDmgChance; // ● DB、クロウの発動確率
        public virtual int DoubleDmgChance
        {
            get
            {
                return _doubleDmgChance;
            }
            set
            {
                _doubleDmgChance = value;
            }
        }
        private int _magicDmgModifier = 0; // ● 攻撃魔法のダメージ補正
        public virtual int MagicDmgModifier
        {
            get
            {
                return _magicDmgModifier;
            }
            set
            {
                _magicDmgModifier = value;
            }
        }

        public virtual int get_canbedmg()
        {
            return 0;
        }

        public virtual bool TwohandedWeapon
        {
            get
            {
                return false;
            }
        }

        // ■■■■■■ L1Armor でオーバーライドする項目 ■■■■■■
        public virtual int get_ac()
        {
            return 0;
        }
        private int _damageReduction = 0; // ● ダメージ軽減
        public virtual int DamageReduction
        {
            get
            {
                return _damageReduction;
            }
            set
            {
                _damageReduction = value;
            }
        }
        private int _weightReduction = 0; // ● 重量軽減
        public virtual int WeightReduction
        {
            get
            {
                return _weightReduction;
            }
            set
            {
                _weightReduction = value;
            }
        }
        private int _hitModifierByArmor = 0; // ● 命中率補正
        public virtual int HitModifierByArmor
        {
            get
            {
                return _hitModifierByArmor;
            }
            set
            {
                _hitModifierByArmor = value;
            }
        }
        private int _dmgModifierByArmor = 0; // ● ダメージ補正
        public virtual int DmgModifierByArmor
        {
            get
            {
                return _dmgModifierByArmor;
            }
            set
            {
                _dmgModifierByArmor = value;
            }
        }
        private int _bowHitModifierByArmor = 0; // ● 弓の命中率補正

        public virtual int BowHitModifierByArmor
        {
            get
            {
                return _bowHitModifierByArmor;
            }
            set
            {
                _bowHitModifierByArmor = value;
            }
        }
        private int _bowDmgModifierByArmor = 0; // ● 弓のダメージ補正
        public virtual int BowDmgModifierByArmor
        {
            get
            {
                return _bowDmgModifierByArmor;
            }
            set
            {
                _bowDmgModifierByArmor = value;
            }
        }

        public virtual int get_defense_water()
        {
            return 0;
        }

        public virtual int get_defense_fire()
        {
            return 0;
        }

        public virtual int get_defense_earth()
        {
            return 0;
        }

        public virtual int get_defense_wind()
        {
            return 0;
        }

        public virtual int get_regist_stun()
        {
            return 0;
        }

        public virtual int get_regist_stone()
        {
            return 0;
        }

        public virtual int get_regist_sleep()
        {
            return 0;
        }

        public virtual int get_regist_freeze()
        {
            return 0;
        }

        public virtual int get_regist_sustain()
        {
            return 0;
        }

        public virtual int get_regist_blind()
        {
            return 0;
        }

        public object Clone()
        {
            return clone();
        }

        public virtual int Grade
        {
            get
            {
                return 0;
            }
        }

    }

}