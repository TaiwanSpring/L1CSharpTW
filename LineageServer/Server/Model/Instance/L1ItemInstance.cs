using LineageServer.Server.DataTables;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Text;
using LineageServer.Models;
using LineageServer.Interfaces;

namespace LineageServer.Server.Model.Instance
{
    class L1ItemInstance : GameObject
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            _lastStatus = new DataLastStatus(this);
        }

        private const long serialVersionUID = 1L;

        private int _count;

        private int _itemId;

        private L1Item _item;

        private bool _isEquipped = false;

        private int _enchantLevel;

        private bool _isIdentified = false;

        private int _durability;

        private int _chargeCount;

        private int _remainingTime;

        private DateTime _lastUsed = default(DateTime);

        private int _lastWeight;

        private DataLastStatus _lastStatus;

        private L1PcInstance _pc;

        private bool _isRunning = false;

        private EnchantTimer _timer;

        private int _bless;

        private int _attrEnchantKind;

        private int _attrEnchantLevel;

        public L1ItemInstance()
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
            _count = 1;
            _enchantLevel = 0;
        }

        public L1ItemInstance(L1Item item, int count) : this()
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
            Item = item;
            Count = count;
        }

        /// <summary>
        /// アイテムが確認(鑑定)済みであるかを返す。
        /// </summary>
        /// <returns> 確認済みならtrue、未確認ならfalse。 </returns>
        public virtual bool Identified
        {
            get
            {
                return _isIdentified;
            }
            set
            {
                _isIdentified = value;
            }
        }


        public virtual string Name
        {
            get
            {
                return _item.Name;
            }
        }

        /// <summary>
        /// アイテムの個数を返す。
        /// </summary>
        /// <returns> アイテムの個数 </returns>
        public virtual int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }


        /// <summary>
        /// アイテムが装備されているかを返す。
        /// </summary>
        /// <returns> アイテムが装備されていればtrue、装備されていなければfalse。 </returns>
        public virtual bool Equipped
        {
            get
            {
                return _isEquipped;
            }
            set
            {
                _isEquipped = value;
            }
        }


        public virtual L1Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                _itemId = value.ItemId;
            }
        }


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


        public virtual bool Stackable
        {
            get
            {
                return _item.Stackable;
            }
        }

        public override void onAction(L1PcInstance player)
        {
        }

        public virtual int EnchantLevel
        {
            get
            {
                return _enchantLevel;
            }
            set
            {
                _enchantLevel = value;
            }
        }


        public virtual int get_gfxid()
        {
            return _item.GfxId;
        }

        public virtual int ChargeCount
        {
            get
            {
                return _chargeCount;
            }
            set
            {
                _chargeCount = value;
            }
        }


        public virtual int RemainingTime
        {
            get
            {
                return _remainingTime;
            }
            set
            {
                _remainingTime = value;
            }
        }


        public virtual DateTime LastUsed
        {
            set
            {
                _lastUsed = value;
            }
            get
            {
                return _lastUsed;
            }
        }


        public virtual int LastWeight
        {
            get
            {
                return _lastWeight;
            }
            set
            {
                _lastWeight = value;
            }
        }


        public virtual int Bless
        {
            set
            {
                _bless = value;
            }
            get
            {
                return _bless;
            }
        }


        public virtual int AttrEnchantKind
        {
            set
            {
                _attrEnchantKind = value;
            }
            get
            {
                return _attrEnchantKind;
            }
        }


        public virtual int AttrEnchantLevel
        {
            set
            {
                _attrEnchantLevel = value;
            }
            get
            {
                return _attrEnchantLevel;
            }
        }


        public virtual int Mr
        {
            get
            {
                int mr = _item.get_mdef();
                if ((ItemId == L1ArmorId.HELMET_OF_MAGIC_RESISTANCE) || (ItemId == L1ArmorId.CHAIN_MAIL_OF_MAGIC_RESISTANCE) || (ItemId >= L1ArmorId.ELITE_PLATE_MAIL_OF_LINDVIOR && ItemId <= L1ArmorId.ELITE_SCALE_MAIL_OF_LINDVIOR) || (ItemId == L1ArmorId.B_HELMET_OF_MAGIC_RESISTANCE))
                { // 受祝福的 抗魔法頭盔
                    mr += EnchantLevel;
                }
                if ((ItemId == L1ArmorId.CLOAK_OF_MAGIC_RESISTANCE) || (ItemId == L1ArmorId.B_CLOAK_OF_MAGIC_RESISTANCE) || (ItemId == L1ArmorId.C_CLOAK_OF_MAGIC_RESISTANCE))
                { // 受咀咒的 抗魔法斗篷
                    mr += EnchantLevel * 2;
                }
                // 飾品強化效果
                if (M_Def != 0)
                {
                    mr += M_Def;
                }
                return mr;
            }
        }

        public virtual int get_durability()
        {
            return _durability;
        }
        /*
		 * 耐久性、0~127まで -の値は許可しない。
		 */
        public virtual void set_durability(int i)
        {
            if (i < 0)
            {
                i = 0;
            }

            if (i > 127)
            {
                i = 127;
            }
            _durability = i;
        }

        public virtual int Weight
        {
            get
            {
                if (Item.Weight == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Max(Count * Item.Weight / 1000, 1);
                }
            }
        }

        /// <summary>
        /// 前回DBへ保存した際のアイテムのステータスを格納するクラス
        /// </summary>
        public class DataLastStatus
        {
            private readonly L1ItemInstance outerInstance;

            public DataLastStatus(L1ItemInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public int count;

            public int itemId;

            public bool isEquipped = false;

            public int enchantLevel;

            public bool isIdentified = true;

            public int durability;

            public int chargeCount;

            public int remainingTime;

            public DateTime lastUsed = default(DateTime);

            public int bless;

            public int attrEnchantKind;

            public int attrEnchantLevel;

            public int firemr; // Scroll of Enchant Accessory

            public int watermr;

            public int earthmr;

            public int windmr;

            public int addhp;

            public int addmp;

            public int hpr;

            public int mpr;

            public int addsp;

            public int m_def;

            public virtual void updateAll()
            {
                count = outerInstance.Count;
                itemId = outerInstance.ItemId;
                isEquipped = outerInstance.Equipped;
                isIdentified = outerInstance.Identified;
                enchantLevel = outerInstance.EnchantLevel;
                durability = outerInstance.get_durability();
                chargeCount = outerInstance.ChargeCount;
                remainingTime = outerInstance.RemainingTime;
                lastUsed = outerInstance.LastUsed;
                bless = outerInstance.Bless;
                attrEnchantKind = outerInstance.AttrEnchantKind;
                attrEnchantLevel = outerInstance.AttrEnchantLevel;
                firemr = outerInstance.FireMr;
                watermr = outerInstance.WaterMr;
                earthmr = outerInstance.EarthMr;
                windmr = outerInstance.WindMr;
                addhp = outerInstance.getaddHp();
                addmp = outerInstance.getaddMp();
                addsp = outerInstance.getaddSp();
                hpr = outerInstance.Hpr;
                mpr = outerInstance.Mpr;
                m_def = outerInstance.M_Def;
            }

            public virtual void updateCount()
            {
                count = outerInstance.Count;
            }

            public virtual void updateItemId()
            {
                itemId = outerInstance.ItemId;
            }

            public virtual void updateEquipped()
            {
                isEquipped = outerInstance.Equipped;
            }

            public virtual void updateIdentified()
            {
                isIdentified = outerInstance.Identified;
            }

            public virtual void updateEnchantLevel()
            {
                enchantLevel = outerInstance.EnchantLevel;
            }

            public virtual void updateDuraility()
            {
                durability = outerInstance.get_durability();
            }

            public virtual void updateChargeCount()
            {
                chargeCount = outerInstance.ChargeCount;
            }

            public virtual void updateRemainingTime()
            {
                remainingTime = outerInstance.RemainingTime;
            }

            public virtual void updateLastUsed()
            {
                lastUsed = outerInstance.LastUsed;
            }

            public virtual void updateBless()
            {
                bless = outerInstance.Bless;
            }

            public virtual void updateAttrEnchantKind()
            {
                attrEnchantKind = outerInstance.AttrEnchantKind;
            }

            public virtual void updateAttrEnchantLevel()
            {
                attrEnchantLevel = outerInstance.AttrEnchantLevel;
            }

            public virtual void updateFireMr()
            {
                firemr = outerInstance.FireMr;
            }

            public virtual void updateWaterMr()
            {
                watermr = outerInstance.WaterMr;
            }

            public virtual void updateEarthMr()
            {
                earthmr = outerInstance.EarthMr;
            }

            public virtual void updateWindMr()
            {
                windmr = outerInstance.WindMr;
            }

            public virtual void updateSp()
            {
                addsp = outerInstance.getaddSp();
            }

            public virtual void updateaddHp()
            {
                addhp = outerInstance.getaddHp();
            }

            public virtual void updateaddMp()
            {
                addmp = outerInstance.getaddMp();
            }

            public virtual void updateHpr()
            {
                hpr = outerInstance.Hpr;
            }

            public virtual void updateMpr()
            {
                mpr = outerInstance.Mpr;
            }

            public virtual void updateM_Def()
            {
                m_def = outerInstance.M_Def;
            }
        }

        public virtual DataLastStatus LastStatus
        {
            get
            {
                return _lastStatus;
            }
        }

        /// <summary>
        /// 前回DBに保存した時から変化しているカラムをビット集合として返す。
        /// </summary>
        public virtual int RecordingColumns
        {
            get
            {
                int column = 0;

                if (Count != _lastStatus.count)
                {
                    column += L1PcInventory.COL_COUNT;
                }
                if (ItemId != _lastStatus.itemId)
                {
                    column += L1PcInventory.COL_ITEMID;
                }
                if (Equipped != _lastStatus.isEquipped)
                {
                    column += L1PcInventory.COL_EQUIPPED;
                }
                if (EnchantLevel != _lastStatus.enchantLevel)
                {
                    column += L1PcInventory.COL_ENCHANTLVL;
                }
                if (get_durability() != _lastStatus.durability)
                {
                    column += L1PcInventory.COL_DURABILITY;
                }
                if (ChargeCount != _lastStatus.chargeCount)
                {
                    column += L1PcInventory.COL_CHARGE_COUNT;
                }
                if (LastUsed != _lastStatus.lastUsed)
                {
                    column += L1PcInventory.COL_DELAY_EFFECT;
                }
                if (Identified != _lastStatus.isIdentified)
                {
                    column += L1PcInventory.COL_IS_ID;
                }
                if (RemainingTime != _lastStatus.remainingTime)
                {
                    column += L1PcInventory.COL_REMAINING_TIME;
                }
                if (Bless != _lastStatus.bless)
                {
                    column += L1PcInventory.COL_BLESS;
                }
                if (AttrEnchantKind != _lastStatus.attrEnchantKind)
                {
                    column += L1PcInventory.COL_ATTR_ENCHANT_KIND;
                }
                if (AttrEnchantLevel != _lastStatus.attrEnchantLevel)
                {
                    column += L1PcInventory.COL_ATTR_ENCHANT_LEVEL;
                }

                return column;
            }
        }

        public virtual int RecordingColumnsEnchantAccessory
        {
            get
            {
                int column = 0;

                if (getaddHp() != _lastStatus.addhp)
                {
                    column += L1PcInventory.COL_ADDHP;
                }
                if (getaddMp() != _lastStatus.addmp)
                {
                    column += L1PcInventory.COL_ADDMP;
                }
                if (Hpr != _lastStatus.hpr)
                {
                    column += L1PcInventory.COL_HPR;
                }
                if (Mpr != _lastStatus.mpr)
                {
                    column += L1PcInventory.COL_MPR;
                }
                if (getaddSp() != _lastStatus.addsp)
                {
                    column += L1PcInventory.COL_ADDSP;
                }
                if (M_Def != _lastStatus.m_def)
                {
                    column += L1PcInventory.COL_M_DEF;
                }
                if (EarthMr != _lastStatus.earthmr)
                {
                    column += L1PcInventory.COL_EARTHMR;
                }
                if (FireMr != _lastStatus.firemr)
                {
                    column += L1PcInventory.COL_FIREMR;
                }
                if (WaterMr != _lastStatus.watermr)
                {
                    column += L1PcInventory.COL_WATERMR;
                }
                if (WindMr != _lastStatus.windmr)
                {
                    column += L1PcInventory.COL_WINDMR;
                }

                return column;
            }
        }

        /// <summary>
        /// 鞄や倉庫で表示される形式の名前を個数を指定して取得する。<br>
        /// </summary>
        public virtual string getNumberedViewName(int count)
        {
            StringBuilder name = new StringBuilder(getNumberedName(count));
            int itemType2 = Item.Type2;
            int itemId = Item.ItemId;

            if ((itemId == 40314) || (itemId == 40316))
            { // ペットのアミュレット
                L1Pet pet = PetTable.Instance.getTemplate(Id);
                if (pet != null)
                {
                    L1Npc npc = Container.Instance.Resolve<INpcController>().getTemplate(pet.get_npcid());
                    // name.append("[Lv." + pet.get_level() + " "
                    // + npc.get_nameid() + "]");
                    name.Append("[Lv." + pet.get_level() + " " + pet.get_name() + "]HP" + pet.get_hp() + " " + npc.get_nameid());
                }
            }

            if ((Item.Type2 == 0) && (Item.Type == 2))
            { // light系アイテム
                if (NowLighting)
                {
                    name.Append(" ($10)");
                }
                if ((itemId == 40001) || (itemId == 40002))
                { // ランプorランタン
                    if (RemainingTime <= 0)
                    {
                        name.Append(" ($11)");
                    }
                }
            }

            if (Equipped)
            {
                if (itemType2 == 1)
                {
                    name.Append(" ($9)"); // 装備(Armed)
                }
                else if (itemType2 == 2)
                {
                    name.Append(" ($117)"); // 装備(Worn)
                }
            }
            return name.ToString();
        }

        /// <summary>
        /// 鞄や倉庫で表示される形式の名前を返す。<br>
        /// 例:+10 カタナ (装備)
        /// </summary>
        public virtual string ViewName
        {
            get
            {
                return getNumberedViewName(_count);
            }
        }

        /// <summary>
        /// ログに表示される形式の名前を返す。<br>
        /// 例:アデナ(250) / +6 ダガー
        /// </summary>
        public virtual string LogName
        {
            get
            {
                return getNumberedName(_count);
            }
        }

        /// <summary>
        /// ログに表示される形式の名前を、個数を指定して取得する。
        /// </summary>
        public virtual string getNumberedName(int count)
        {
            StringBuilder name = new StringBuilder();

            if (Identified)
            {
                if (Item.Type2 == 1)
                { // 武器
                    int attrEnchantLevel = AttrEnchantLevel;
                    if (attrEnchantLevel > 0)
                    {
                        string attrStr = null;
                        switch (AttrEnchantKind)
                        {
                            case 1: // 地
                                if (attrEnchantLevel == 1)
                                {
                                    attrStr = "$6124";
                                }
                                else if (attrEnchantLevel == 2)
                                {
                                    attrStr = "$6125";
                                }
                                else if (attrEnchantLevel == 3)
                                {
                                    attrStr = "$6126";
                                }
                                break;
                            case 2: // 火
                                if (attrEnchantLevel == 1)
                                {
                                    attrStr = "$6115";
                                }
                                else if (attrEnchantLevel == 2)
                                {
                                    attrStr = "$6116";
                                }
                                else if (attrEnchantLevel == 3)
                                {
                                    attrStr = "$6117";
                                }
                                break;
                            case 4: // 水
                                if (attrEnchantLevel == 1)
                                {
                                    attrStr = "$6118";
                                }
                                else if (attrEnchantLevel == 2)
                                {
                                    attrStr = "$6119";
                                }
                                else if (attrEnchantLevel == 3)
                                {
                                    attrStr = "$6120";
                                }
                                break;
                            case 8: // 風
                                if (attrEnchantLevel == 1)
                                {
                                    attrStr = "$6121";
                                }
                                else if (attrEnchantLevel == 2)
                                {
                                    attrStr = "$6122";
                                }
                                else if (attrEnchantLevel == 3)
                                {
                                    attrStr = "$6123";
                                }
                                break;
                            default:
                                break;
                        }
                        name.Append(attrStr + " ");
                    }
                }

                if ((Item.Type2 == 1) || (Item.Type2 == 2))
                { // 武器・防具
                    if (EnchantLevel >= 0)
                    {
                        name.Append("+" + EnchantLevel + " ");
                    }
                    else if (EnchantLevel < 0)
                    {
                        name.Append(EnchantLevel.ToString() + " ");
                    }
                }
            }
            if (Identified)
            {
                name.Append(_item.IdentifiedNameId);
            }
            else
            {
                name.Append(_item.UnidentifiedNameId);
            }
            if (Identified)
            {
                if (Item.MaxChargeCount > 0)
                {
                    name.Append(" (" + ChargeCount + ")");
                }
                if (Item.ItemId == 20383)
                { // 騎馬用ヘルム
                    name.Append(" (" + ChargeCount + ")");
                }
                if ((Item.MaxUseTime > 0) && (Item.Type2 != 0))
                { // 武器防具で使用時間制限あり
                    name.Append(" [" + RemainingTime + "]");
                }
            }

            // 旅館鑰匙
            if (Item.ItemId == 40312 && KeyId != 0)
            {
                name.Append(InnKeyName);
            }

            if (count > 1)
            {
                name.Append(" (" + count + ")");
            }

            return name.ToString();
        }

        // 旅館鑰匙
        public virtual string InnKeyName
        {
            get
            {
                StringBuilder name = new StringBuilder();
                name.Append(" #");
                string chatText = KeyId.ToString();
                string s1 = "";
                string s2 = "";
                for (int i = 0; i < chatText.Length; i++)
                {
                    if (i >= 5)
                    {
                        break;
                    }
                    s1 = s1 + chatText[i].ToString();
                }
                name.Append(s1);
                for (int i = 0; i < chatText.Length; i++)
                {
                    if ((i % 2) == 0)
                    {
                        s1 = chatText[i].ToString();
                    }
                    else
                    {
                        s2 = s1 + chatText[i].ToString();
                        name.Append(Convert.ToInt32(s2).ToString("x").ToLower()); // 轉成16進位
                    }
                }
                return name.ToString();
            }
        }

        /// <summary>
        /// アイテムの状態からサーバーパケットで利用する形式のバイト列を生成し、返す。
        /// </summary>
        public virtual byte[] StatusBytes
        {
            get
            {
                int itemType2 = Item.Type2;
                int itemId = ItemId;
                BinaryOutputStream os = new BinaryOutputStream();
                L1PetItem petItem = PetItemTable.Instance.getTemplate(itemId);

                if (petItem != null)
                { // 寵物裝備
                    if (petItem.UseType == 1)
                    { // 牙齒
                        os.writeC(7); // 可使用職業：
                        os.writeC(128); // [高等寵物]
                        os.writeC(23); // 材質
                        os.writeC(Item.Material);
                        os.writeD(Weight);
                    }
                    else
                    { // 盔甲
                      // AC
                        os.writeC(19);
                        int ac = petItem.AddAc;
                        if (ac < 0)
                        {
                            ac = ac - ac - ac;
                        }
                        os.writeC(ac);
                        os.writeC(Item.Material);
                        os.writeC(-1); // 飾品級別 - 0:上等 1:中等 2:初級 3:特等
                        os.writeD(Weight);

                        os.writeC(7); // 可使用職業：
                        os.writeC(128); // [高等寵物]

                        // STR~CHA
                        if (petItem.AddStr != 0)
                        {
                            os.writeC(8);
                            os.writeC(petItem.AddStr);
                        }
                        if (petItem.AddDex != 0)
                        {
                            os.writeC(9);
                            os.writeC(petItem.AddDex);
                        }
                        if (petItem.AddCon != 0)
                        {
                            os.writeC(10);
                            os.writeC(petItem.AddCon);
                        }
                        if (petItem.AddWis != 0)
                        {
                            os.writeC(11);
                            os.writeC(petItem.AddWis);
                        }
                        if (petItem.AddInt != 0)
                        {
                            os.writeC(12);
                            os.writeC(petItem.AddInt);
                        }
                        // HP, MP
                        if (petItem.AddHp != 0)
                        {
                            os.writeC(14);
                            os.writeH(petItem.AddHp);
                        }
                        if (petItem.AddMp != 0)
                        {
                            os.writeC(32);
                            os.writeC(petItem.AddMp);
                        }
                        // MR
                        if (petItem.AddMr != 0)
                        {
                            os.writeC(15);
                            os.writeH(petItem.AddMr);
                        }
                        // SP(魔力)
                        if (petItem.AddSp != 0)
                        {
                            os.writeC(17);
                            os.writeC(petItem.AddSp);
                        }
                    }
                }
                else if (itemType2 == 0)
                { // etcitem
                    switch (Item.Type)
                    {
                        case 2: // light
                            os.writeC(22); // 明るさ
                            os.writeH(Item.LightRange);
                            break;
                        case 7: // food
                            os.writeC(21);
                            // 栄養
                            os.writeH(Item.FoodVolume);
                            break;
                        case 0: // arrow
                        case 15: // sting
                            os.writeC(1); // 打撃値
                            os.writeC(Item.DmgSmall);
                            os.writeC(Item.DmgLarge);
                            break;
                        default:
                            os.writeC(23); // 材質
                            break;
                    }
                    os.writeC(Item.Material);
                    os.writeD(Weight);
                }
                else if ((itemType2 == 1) || (itemType2 == 2))
                { // weapon | armor
                    if (itemType2 == 1)
                    { // weapon
                      // 打撃値
                        os.writeC(1);
                        os.writeC(Item.DmgSmall);
                        os.writeC(Item.DmgLarge);
                        os.writeC(Item.Material);
                        os.writeD(Weight);
                    }
                    else if (itemType2 == 2)
                    { // armor
                      // AC
                        os.writeC(19);
                        int ac = ((L1Armor)Item).get_ac();
                        if (ac < 0)
                        {
                            ac = ac - ac - ac;
                        }
                        os.writeC(ac);
                        os.writeC(Item.Material);
                        os.writeC(Item.Grade); // 飾品級別 - 0:上等 1:中等 2:初級 3:特等
                        os.writeD(Weight);
                    }
                    /// <summary>
                    /// 強化數判斷 </summary>
                    if (EnchantLevel != 0)
                    {
                        os.writeC(2);
                        /// <summary>
                        /// 飾品強化卷軸 </summary>
                        if (Item.Type2 == 2 && Item.Type >= 8 && Item.Type <= 12)
                        { // 8:項鍊 9:戒指1 10:腰帶
                          // 11:戒指2 12:耳環
                            os.writeC(0);
                        }
                        else
                        {
                            os.writeC(EnchantLevel);
                        }
                    }
                    // 損傷度
                    if (get_durability() != 0)
                    {
                        os.writeC(3);
                        os.writeC(get_durability());
                    }
                    // 両手武器
                    if (Item.TwohandedWeapon)
                    {
                        os.writeC(4);
                    }
                    // 攻撃成功
                    if (itemType2 == 1)
                    { // weapon
                        if (Item.HitModifier != 0)
                        {
                            os.writeC(5);
                            os.writeC(Item.HitModifier);
                        }
                    }
                    else if (itemType2 == 2)
                    { // armor
                        if (Item.HitModifierByArmor != 0)
                        {
                            os.writeC(5);
                            os.writeC(Item.HitModifierByArmor);
                        }
                    }
                    // 追加打撃
                    if (itemType2 == 1)
                    { // weapon
                        if (Item.DmgModifier != 0)
                        {
                            os.writeC(6);
                            os.writeC(Item.DmgModifier);
                        }
                    }
                    else if (itemType2 == 2)
                    { // armor
                        if (Item.DmgModifierByArmor != 0)
                        {
                            os.writeC(6);
                            os.writeC(Item.DmgModifierByArmor);
                        }
                    }
                    // 使用可能
                    int bit = 0;
                    bit |= Item.UseRoyal ? 1 : 0;
                    bit |= Item.UseKnight ? 2 : 0;
                    bit |= Item.UseElf ? 4 : 0;
                    bit |= Item.UseMage ? 8 : 0;
                    bit |= Item.UseDarkelf ? 16 : 0;
                    bit |= Item.UseDragonknight ? 32 : 0;
                    bit |= Item.UseIllusionist ? 64 : 0;
                    // bit |= getItem().isUseHiPet() ? 64 : 0; // ハイペット
                    os.writeC(7);
                    os.writeC(bit);
                    // 弓の命中率補正
                    if (Item.BowHitModifierByArmor != 0)
                    {
                        os.writeC(24);
                        os.writeC(Item.BowHitModifierByArmor);
                    }
                    // 弓のダメージ補正
                    if (Item.BowDmgModifierByArmor != 0)
                    {
                        os.writeC(35);
                        os.writeC(Item.BowDmgModifierByArmor);
                    }
                    // MP吸収
                    if ((itemId == 126) || (itemId == 127))
                    { // マナスタッフ、鋼鉄のマナスタッフ
                        os.writeC(16);
                    }
                    // HP吸収
                    if (itemId == 262)
                    { // ディストラクション
                        os.writeC(34);
                    }
                    // STR~CHA
                    if (Item.get_addstr() != 0)
                    {
                        os.writeC(8);
                        os.writeC(Item.get_addstr());
                    }
                    if (Item.get_adddex() != 0)
                    {
                        os.writeC(9);
                        os.writeC(Item.get_adddex());
                    }
                    if (Item.get_addcon() != 0)
                    {
                        os.writeC(10);
                        os.writeC(Item.get_addcon());
                    }
                    if (Item.get_addwis() != 0)
                    {
                        os.writeC(11);
                        os.writeC(Item.get_addwis());
                    }
                    if (Item.get_addint() != 0)
                    {
                        os.writeC(12);
                        os.writeC(Item.get_addint());
                    }
                    if (Item.get_addcha() != 0)
                    {
                        os.writeC(13);
                        os.writeC(Item.get_addcha());
                    }
                    // HP, MP
                    if (Item.get_addhp() != 0 || getaddHp() != 0)
                    {
                        os.writeC(14);
                        os.writeH(Item.get_addhp() + getaddHp());
                    }
                    if (Item.get_addmp() != 0 || getaddMp() != 0)
                    {
                        os.writeC(32);
                        os.writeC(Item.get_addmp() + getaddMp());
                    }
                    // SP(魔力)
                    if (Item.get_addsp() != 0 || getaddSp() != 0)
                    {
                        os.writeC(17);
                        os.writeC(Item.get_addsp() + getaddSp());
                    }
                    // ヘイスト
                    if (Item.HasteItem)
                    {
                        os.writeC(18);
                    }
                    // 火の属性
                    if (Item.get_defense_fire() != 0 || FireMr != 0)
                    {
                        os.writeC(27);
                        os.writeC(Item.get_defense_fire() + FireMr);
                    }
                    // 水の属性
                    if (Item.get_defense_water() != 0 || WaterMr != 0)
                    {
                        os.writeC(28);
                        os.writeC(Item.get_defense_water() + WaterMr);
                    }
                    // 風の属性
                    if (Item.get_defense_wind() != 0 || WindMr != 0)
                    {
                        os.writeC(29);
                        os.writeC(Item.get_defense_wind() + WindMr);
                    }
                    // 地の属性
                    if (Item.get_defense_earth() != 0 || EarthMr != 0)
                    {
                        os.writeC(30);
                        os.writeC(Item.get_defense_earth() + EarthMr);
                    }
                    // 凍結耐性
                    if (Item.get_regist_freeze() != 0)
                    {
                        os.writeC(15);
                        os.writeH(Item.get_regist_freeze());
                        os.writeC(33);
                        os.writeC(1);
                    }
                    // 石化耐性
                    if (Item.get_regist_stone() != 0)
                    {
                        os.writeC(15);
                        os.writeH(Item.get_regist_stone());
                        os.writeC(33);
                        os.writeC(2);
                    }
                    // 睡眠耐性
                    if (Item.get_regist_sleep() != 0)
                    {
                        os.writeC(15);
                        os.writeH(Item.get_regist_sleep());
                        os.writeC(33);
                        os.writeC(3);
                    }
                    // 暗闇耐性
                    if (Item.get_regist_blind() != 0)
                    {
                        os.writeC(15);
                        os.writeH(Item.get_regist_blind());
                        os.writeC(33);
                        os.writeC(4);
                    }
                    // スタン耐性
                    if (Item.get_regist_stun() != 0)
                    {
                        os.writeC(15);
                        os.writeH(Item.get_regist_stun());
                        os.writeC(33);
                        os.writeC(5);
                    }
                    // ホールド耐性
                    if (Item.get_regist_sustain() != 0)
                    {
                        os.writeC(15);
                        os.writeH(Item.get_regist_sustain());
                        os.writeC(33);
                        os.writeC(6);
                    }
                    // MR
                    if (Mr != 0)
                    {
                        os.writeC(15);
                        os.writeH(Mr);
                    }
                    // 體力回復率
                    if (Item.get_addhpr() != 0 || Hpr != 0)
                    {
                        os.writeC(37);
                        os.writeC(Item.get_addhpr() + Hpr);
                    }
                    // 魔力回復率
                    if (Item.get_addmpr() != 0 || Mpr != 0)
                    {
                        os.writeC(26); // 3.70C
                        os.writeC(Item.get_addmpr() + Mpr);
                    }
                    // 幸運
                    // if (getItem.getLuck() != 0) {
                    // os.writeC(20);
                    // os.writeC(val);
                    // }
                    // 種類
                    // if (getItem.getDesc() != 0) {
                    // os.writeC(25);
                    // os.writeH(val); // desc.tbl ID
                    // }
                    // レベル
                    // if (getItem.getLevel() != 0) {
                    // os.writeC(26);
                    // os.writeH(val);
                    // }
                }
                byte[] result = os.Bytes;

                return result;
            }
        }

        internal class EnchantTimer : TimerTask
        {
            private readonly L1ItemInstance outerInstance;


            public EnchantTimer(L1ItemInstance outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void run()
            {
                try
                {
                    int type = outerInstance.Item.Type;
                    int type2 = outerInstance.Item.Type2;
                    int itemId = outerInstance.Item.ItemId;
                    if ((outerInstance._pc != null) && outerInstance._pc.Inventory.checkItem(itemId))
                    {
                        if ((type == 2) && (type2 == 2) && outerInstance.Equipped)
                        {
                            outerInstance._pc.addAc(3);
                            outerInstance._pc.sendPackets(new S_OwnCharStatus(outerInstance._pc));
                        }
                    }
                    outerInstance.AcByMagic = 0;
                    outerInstance.DmgByMagic = 0;
                    outerInstance.HolyDmgByMagic = 0;
                    outerInstance.HitByMagic = 0;
                    outerInstance._pc.sendPackets(new S_ServerMessage(308, outerInstance.LogName));
                    outerInstance._isRunning = false;
                    outerInstance._timer = null;
                }
                catch (Exception)
                {
                }
            }
        }

        private int _acByMagic = 0;

        public virtual int AcByMagic
        {
            get
            {
                return _acByMagic;
            }
            set
            {
                _acByMagic = value;
            }
        }


        private int _dmgByMagic = 0;

        public virtual int DmgByMagic
        {
            get
            {
                return _dmgByMagic;
            }
            set
            {
                _dmgByMagic = value;
            }
        }


        private int _holyDmgByMagic = 0;

        public virtual int HolyDmgByMagic
        {
            get
            {
                return _holyDmgByMagic;
            }
            set
            {
                _holyDmgByMagic = value;
            }
        }


        private int _hitByMagic = 0;

        public virtual int HitByMagic
        {
            get
            {
                return _hitByMagic;
            }
            set
            {
                _hitByMagic = value;
            }
        }


        private int _FireMr = 0;

        public virtual int FireMr
        {
            get
            {
                return _FireMr;
            }
            set
            {
                _FireMr = value;
            }
        }


        private int _WaterMr = 0;

        public virtual int WaterMr
        {
            get
            {
                return _WaterMr;
            }
            set
            {
                _WaterMr = value;
            }
        }


        private int _EarthMr = 0;

        public virtual int EarthMr
        {
            get
            {
                return _EarthMr;
            }
            set
            {
                _EarthMr = value;
            }
        }


        private int _WindMr = 0;

        public virtual int WindMr
        {
            get
            {
                return _WindMr;
            }
            set
            {
                _WindMr = value;
            }
        }


        private int _M_Def = 0;

        public virtual int M_Def
        {
            get
            {
                return _M_Def;
            }
            set
            {
                _M_Def = value;
            }
        }


        private int _Mpr = 0;

        public virtual int Mpr
        {
            get
            {
                return _Mpr;
            }
            set
            {
                _Mpr = value;
            }
        }


        private int _Hpr = 0;

        public virtual int Hpr
        {
            get
            {
                return _Hpr;
            }
            set
            {
                _Hpr = value;
            }
        }


        private int _addHp = 0;

        public virtual int getaddHp()
        {
            return _addHp;
        }

        public virtual void setaddHp(int i)
        {
            _addHp = i;
        }

        private int _addMp = 0;

        public virtual int getaddMp()
        {
            return _addMp;
        }

        public virtual void setaddMp(int i)
        {
            _addMp = i;
        }

        private int _addSp = 0;

        public virtual int getaddSp()
        {
            return _addSp;
        }

        public virtual void setaddSp(int i)
        {
            _addSp = i;
        }

        public virtual void setSkillArmorEnchant(L1PcInstance pc, int skillId, int skillTime)
        {
            int type = Item.Type;
            int type2 = Item.Type2;
            if (_isRunning)
            {
                _timer.cancel();
                int itemId = Item.ItemId;
                if ((pc != null) && pc.Inventory.checkItem(itemId))
                {
                    if ((type == 2) && (type2 == 2) && Equipped)
                    {
                        pc.addAc(3);
                        pc.sendPackets(new S_OwnCharStatus(pc));
                    }
                }
                AcByMagic = 0;
                _isRunning = false;
                _timer = null;
            }

            if ((type == 2) && (type2 == 2) && Equipped)
            {
                pc.addAc(-3);
                pc.sendPackets(new S_OwnCharStatus(pc));
            }
            AcByMagic = 3;
            _pc = pc;
            _timer = new EnchantTimer(this);
			Container.Instance.Resolve<ITaskController>().execute((Interfaces.IRunnable)this._timer, skillTime);
            _isRunning = true;
        }

        public virtual void setSkillWeaponEnchant(L1PcInstance pc, int skillId, int skillTime)
        {
            if (Item.Type2 != 1)
            {
                return;
            }
            if (_isRunning)
            {
                _timer.cancel();
                DmgByMagic = 0;
                HolyDmgByMagic = 0;
                HitByMagic = 0;
                _isRunning = false;
                _timer = null;
            }

            switch (skillId)
            {
                case L1SkillId.HOLY_WEAPON:
                    HolyDmgByMagic = 1;
                    HitByMagic = 1;
                    break;

                case L1SkillId.ENCHANT_WEAPON:
                    DmgByMagic = 2;
                    break;

                case L1SkillId.BLESS_WEAPON:
                    DmgByMagic = 2;
                    HitByMagic = 2;
                    break;

                case L1SkillId.SHADOW_FANG:
                    DmgByMagic = 5;
                    break;

                default:
                    break;
            }

            _pc = pc;
            _timer = new EnchantTimer(this);
			Container.Instance.Resolve<ITaskController>().execute((Interfaces.IRunnable)this._timer, skillTime);
            _isRunning = true;
        }

        private int _itemOwnerId = 0;

        public virtual int ItemOwnerId
        {
            get
            {
                return _itemOwnerId;
            }
            set
            {
                _itemOwnerId = value;
            }
        }


        public virtual void startItemOwnerTimer(L1PcInstance pc)
        {
            ItemOwnerId = pc.Id;
            L1ItemOwnerTimer timer = new L1ItemOwnerTimer(this, 10000);
            timer.begin();
        }

        private L1EquipmentTimer _equipmentTimer;

        public virtual void startEquipmentTimer(L1PcInstance pc)
        {
            if (RemainingTime > 0)
            {
                _equipmentTimer = new L1EquipmentTimer(pc, this);
                Container.Instance.Resolve<ITaskController>().execute(_equipmentTimer, 1000, 1000);
            }
        }

        public virtual void stopEquipmentTimer(L1PcInstance pc)
        {
            if (RemainingTime > 0)
            {
                _equipmentTimer.cancel();
                _equipmentTimer = null;
            }
        }

        private bool _isNowLighting = false;

        public virtual bool NowLighting
        {
            get
            {
                return _isNowLighting;
            }
            set
            {
                _isNowLighting = value;
            }
        }

        //3.63
        private int _ringId;

        public virtual int RingID
        {
            get
            {
                return _ringId;
            }
            set
            {
                _ringId = value;
            }
        }

        //3.63
        // 旅館鑰匙
        private int _keyId = 0;

        public virtual int KeyId
        {
            get
            {
                return _keyId;
            }
            set
            {
                _keyId = value;
            }
        }


        private int _innNpcId = 0;

        public virtual int InnNpcId
        {
            get
            {
                return _innNpcId;
            }
            set
            {
                _innNpcId = value;
            }
        }


        private bool _isHall;

        public virtual bool checkRoomOrHall()
        {
            return _isHall;
        }

        public virtual bool Hall
        {
            set
            {
                _isHall = value;
            }
        }

        private DateTime _dueTime;

        public virtual DateTime DueTime
        {
            get
            {
                return _dueTime;
            }
            set
            {
                _dueTime = value;
            }
        }


        public virtual int ItemStatusX
        {
            get
            {
                if (!Identified)
                {
                    return 0;
                }
                int statusX = 1;

                if (!Item.Tradable)
                {
                    statusX |= 2; // 無法交易
                }

                if (Item.CantDelete)
                {
                    statusX |= 4; // 無法刪除
                }

                if (Item.get_safeenchant() < 0)
                {
                    statusX |= 8; // 無法強化
                }

                if (Item.get_safeenchant() < 0)
                {
                    statusX |= 16; // 倉庫保管功能
                }

                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final int bless = getBless();
                int bless = Bless;
                if (bless >= 128 && bless <= 131)
                {
                    statusX |= 2; // 無法交易
                    statusX |= 4; // 無法刪除
                    statusX |= 8; // 無法強化
                    statusX |= 32; // 封印狀態
                }
                else if (bless > 131)
                {
                    statusX |= 64; // 特殊封印狀態
                }

                if (Item.Stackable)
                {
                    statusX |= 128; // 可以堆疊
                }
                return statusX;
            }
        }

        public L1Map Map { get; internal set; }
    }

}