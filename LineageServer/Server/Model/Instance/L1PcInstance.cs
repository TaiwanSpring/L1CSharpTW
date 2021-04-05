using LineageServer.Command.Executors;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Classes;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Model.monitor;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LineageServer.Server.Model.Instance
{
    class L1PcInstance : L1Character
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            _acceleratorChecker = new AcceleratorChecker(this);
        }

        private const long serialVersionUID = 1L;

        public const int CLASSID_KNIGHT_MALE = 61;

        public const int CLASSID_KNIGHT_FEMALE = 48;

        public const int CLASSID_ELF_MALE = 138;

        public const int CLASSID_ELF_FEMALE = 37;

        public const int CLASSID_WIZARD_MALE = 734;

        public const int CLASSID_WIZARD_FEMALE = 1186;

        public const int CLASSID_DARK_ELF_MALE = 2786;

        public const int CLASSID_DARK_ELF_FEMALE = 2796;

        public const int CLASSID_PRINCE = 0;

        public const int CLASSID_PRINCESS = 1;

        public const int CLASSID_DRAGON_KNIGHT_MALE = 6658;

        public const int CLASSID_DRAGON_KNIGHT_FEMALE = 6661;

        public const int CLASSID_ILLUSIONIST_MALE = 6671;

        public const int CLASSID_ILLUSIONIST_FEMALE = 6650;

        private short _hpr = 0;

        private short _trueHpr = 0;

        /// <summary>
        /// 3.3C組隊系統 </summary>
        internal bool _rpActive = false;

        private L1PartyRefresh _rp;

        private int _partyType;

        private static ILogger _log = Logger.GetLogger(nameof(L1PcInstance));
        public virtual short Hpr
        {
            get
            {
                return (short)_hpr;
            }
        }

        public virtual void addHpr(int i)
        {
            _trueHpr += (short)i;
            _hpr = Math.Max((short)0, _trueHpr);
        }

        private short _mpr = 0;

        private short _trueMpr = 0;

        public virtual short Mpr
        {
            get
            {
                return _mpr;
            }
        }

        public virtual void addMpr(int i)
        {
            _trueMpr += (short)i;
            _mpr = Math.Max((short)0, _trueMpr);
        }

        public short _originalHpr = 0; // ● オリジナルCON HPR

        public virtual short OriginalHpr
        {
            get
            {
                return _originalHpr;
            }
        }

        public short _originalMpr = 0; // ● オリジナルWIS MPR

        public virtual short OriginalMpr
        {
            get
            {
                return _originalMpr;
            }
        }

        public virtual void startHpRegeneration()
        {
            const int INTERVAL = 1000;

            if (!_hpRegenActive)
            {
                _hpRegen = new HpRegeneration(this);
                _regenTimer.execute(_hpRegen, INTERVAL, INTERVAL);
                _hpRegenActive = true;
            }
        }

        public virtual void stopHpRegeneration()
        {
            if (_hpRegenActive)
            {
                _hpRegen.cancel();
                _hpRegen = null;
                _hpRegenActive = false;
            }
        }

        public virtual void startMpRegeneration()
        {
            const int INTERVAL = 1000;

            if (!_mpRegenActive)
            {
                _mpRegen = new MpRegeneration(this);
                _regenTimer.execute(_mpRegen, INTERVAL, INTERVAL);
                _mpRegenActive = true;
            }
        }

        public virtual void stopMpRegeneration()
        {
            if (_mpRegenActive)
            {
                _mpRegen.cancel();
                _mpRegen = null;
                _mpRegenActive = false;
            }
        }

        // 獲得道具
        public virtual void startItemMakeByDoll()
        {
            const int INTERVAL_BY_DOLL = 240000;
            bool isExistItemMakeDoll = false;
            if (L1MagicDoll.isItemMake(this))
            {
                isExistItemMakeDoll = true;
            }
            if (!_ItemMakeActiveByDoll && isExistItemMakeDoll)
            {
                _itemMakeByDoll = new ItemMakeByDoll(this);
                _regenTimer.execute(_itemMakeByDoll, INTERVAL_BY_DOLL, INTERVAL_BY_DOLL);
                _ItemMakeActiveByDoll = true;
            }
        }

        // 獲得道具停止
        public virtual void stopItemMakeByDoll()
        {
            if (_ItemMakeActiveByDoll)
            {
                _itemMakeByDoll.cancel();
                _itemMakeByDoll = null;
                _ItemMakeActiveByDoll = false;
            }
        }

        // 回血開始
        public virtual void startHpRegenerationByDoll()
        {
            const int INTERVAL_BY_DOLL = 64000;
            bool isExistHprDoll = false;
            if (L1MagicDoll.isHpRegeneration(this))
            {
                isExistHprDoll = true;
            }
            if (!_hpRegenActiveByDoll && isExistHprDoll)
            {
                _hpRegenByDoll = new HpRegenerationByDoll(this);
                _regenTimer.execute(_hpRegenByDoll, INTERVAL_BY_DOLL, INTERVAL_BY_DOLL);
                _hpRegenActiveByDoll = true;
            }
        }

        // 回血停止
        public virtual void stopHpRegenerationByDoll()
        {
            if (_hpRegenActiveByDoll)
            {
                _hpRegenByDoll.cancel();
                _hpRegenByDoll = null;
                _hpRegenActiveByDoll = false;
            }
        }

        // 回魔開始
        public virtual void startMpRegenerationByDoll()
        {
            const int INTERVAL_BY_DOLL = 64000;
            bool isExistMprDoll = false;
            if (L1MagicDoll.isMpRegeneration(this))
            {
                isExistMprDoll = true;
            }
            if (!_mpRegenActiveByDoll && isExistMprDoll)
            {
                _mpRegenByDoll = new MpRegenerationByDoll(this);
                _regenTimer.execute(_mpRegenByDoll, INTERVAL_BY_DOLL, INTERVAL_BY_DOLL);
                _mpRegenActiveByDoll = true;
            }
        }

        // 回魔停止
        public virtual void stopMpRegenerationByDoll()
        {
            if (_mpRegenActiveByDoll)
            {
                _mpRegenByDoll.cancel();
                _mpRegenByDoll = null;
                _mpRegenActiveByDoll = false;
            }
        }

        public virtual void startMpReductionByAwake()
        {
            const int INTERVAL_BY_AWAKE = 4000;
            if (!_mpReductionActiveByAwake)
            {
                _mpReductionByAwake = new MpReductionByAwake(this);
                _regenTimer.execute(_mpReductionByAwake, INTERVAL_BY_AWAKE, INTERVAL_BY_AWAKE);
                _mpReductionActiveByAwake = true;
            }
        }

        public virtual void stopMpReductionByAwake()
        {
            if (_mpReductionActiveByAwake)
            {
                _mpReductionByAwake.cancel();
                _mpReductionByAwake = null;
                _mpReductionActiveByAwake = false;
            }
        }

        public virtual void startObjectAutoUpdate()
        {
            removeAllKnownObjects();
            _autoUpdateFuture = new L1PcAutoUpdate(Id);
            Container.Instance.Resolve<ITaskController>().execute(_autoUpdateFuture, 0, INTERVAL_AUTO_UPDATE);
        }

        /// <summary>
        /// 各種モニタータスクを停止する。
        /// </summary>
        public virtual void stopEtcMonitor()
        {
            if (_autoUpdateFuture != null)
            {
                _autoUpdateFuture.cancel();
                _autoUpdateFuture = null;
            }
            if (_expMonitorFuture != null)
            {
                _expMonitorFuture.cancel();
                _expMonitorFuture = null;
            }
            if (_ghostFuture != null)
            {
                _ghostFuture.cancel();
                _ghostFuture = null;
            }

            if (_hellFuture != null)
            {
                _hellFuture.cancel();
                _hellFuture = null;
            }

        }

        private const int INTERVAL_AUTO_UPDATE = 300;

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: private java.util.concurrent.ScheduledFuture<?> _autoUpdateFuture;
        private ITimerTask _autoUpdateFuture;

        private const int INTERVAL_EXP_MONITOR = 500;

        private ITimerTask _expMonitorFuture;
        public virtual void onChangeExp()
        {
            int level = ExpTable.getLevelByExp(Exp);
            int char_level = Level;
            int gap = level - char_level;
            if (gap == 0)
            {
                sendPackets(new S_OwnCharStatus(this));
                //sendPackets(new S_Exp(this));
                return;
            }

            // レベルが変化した場合
            if (gap > 0)
            {
                levelUp(gap);
            }
            else if (gap < 0)
            {
                levelDown(gap);
            }
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            // 判斷旅館內是否使用相同鑰匙
            if (perceivedFrom.MapId >= 16384 && perceivedFrom.MapId <= 25088 && perceivedFrom.InnKeyId != InnKeyId)
            {
                return;
            }
            if (GmInvis || Ghost)
            {
                return;
            }
            if (Invisble && !perceivedFrom.hasSkillEffect(L1SkillId.GMSTATUS_FINDINVIS))
            {
                return;
            }

            perceivedFrom.addKnownObject(this);
            perceivedFrom.sendPackets(new S_OtherCharPacks(this, perceivedFrom.hasSkillEffect(L1SkillId.GMSTATUS_FINDINVIS))); // 自分の情報を送る
            if (InParty && Party.isMember(perceivedFrom))
            { // PTメンバーならHPメーターも送る
                perceivedFrom.sendPackets(new S_HPMeter(this));
            }

            if (PrivateShop)
            { // 開個人商店中
                perceivedFrom.sendPackets(new S_DoActionShop(Id, ActionCodes.ACTION_Shop, ShopChat));
            }
            else if (Fishing)
            { // 釣魚中
                perceivedFrom.sendPackets(new S_Fishing(Id, ActionCodes.ACTION_Fishing, FishX, FishY));
            }

            if (Crown)
            { // 君主
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(Clanname);
                if (clan != null)
                {
                    if ((Id == clan.LeaderId) && (clan.CastleId != 0))
                    {
                        perceivedFrom.sendPackets(new S_CastleMaster(clan.CastleId, Id));
                    }
                }
            }
        }

        // 範囲外になった認識済みオブジェクトを除去
        private void removeOutOfRangeObjects()
        {
            foreach (GameObject known in KnownObjects)
            {
                if (known == null)
                {
                    continue;
                }

                if (Config.PC_RECOGNIZE_RANGE == -1)
                {
                    if (!Location.isInScreen(known.Location))
                    { // 画面外
                        removeKnownObject(known);
                        sendPackets(new S_RemoveObject(known));
                    }
                }
                else
                {
                    if (Location.getTileLineDistance(known.Location) > Config.PC_RECOGNIZE_RANGE)
                    {
                        removeKnownObject(known);
                        sendPackets(new S_RemoveObject(known));
                    }
                }
            }
        }

        // 更新範圍內的物件
        public virtual void updateObject()
        {
            removeOutOfRangeObjects();

            if (MapId <= 10000)
            {
                foreach (GameObject visible in Container.Instance.Resolve<IGameWorld>().getVisibleObjects(this, Config.PC_RECOGNIZE_RANGE))
                {
                    if (!knownsObject(visible))
                    {
                        visible.onPerceive(this);
                    }
                    else
                    {
                        if (visible is L1NpcInstance)
                        {
                            L1NpcInstance npc = (L1NpcInstance)visible;
                            if (Location.isInScreen(npc.Location) && (npc.HiddenStatus != 0))
                            {
                                npc.approachPlayer(this);
                            }
                        }
                    }
                    if (hasSkillEffect(L1SkillId.GMSTATUS_HPBAR) && L1HpBar.isHpBarTarget(visible))
                    {
                        sendPackets(new S_HPMeter((L1Character)visible));
                    }
                }
            }
            else
            { // 旅館內判斷
                foreach (GameObject visible in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(this))
                {
                    if (!knownsObject(visible))
                    {
                        visible.onPerceive(this);
                    }
                    if (hasSkillEffect(L1SkillId.GMSTATUS_HPBAR) && L1HpBar.isHpBarTarget(visible))
                    {
                        if (InnKeyId == ((L1Character)visible).InnKeyId)
                        {
                            sendPackets(new S_HPMeter((L1Character)visible));
                        }
                    }
                }
            }
        }

        private void sendVisualEffect()
        {
            int poisonId = 0;
            if (Poison != null)
            { // 毒状態
                poisonId = Poison.EffectId;
            }
            if (Paralysis != null)
            { // 麻痺状態
              // 麻痺エフェクトを優先して送りたい為、poisonIdを上書き。
                poisonId = Paralysis.EffectId;
            }
            if (poisonId != 0)
            { // このifはいらないかもしれない
                sendPackets(new S_Poison(Id, poisonId));
                broadcastPacket(new S_Poison(Id, poisonId));
            }
        }

        public virtual void sendVisualEffectAtLogin()
        {
            foreach (L1Castle ca in CastleTable.Instance.CastleTableList)
            {
                sendPackets(new S_CastleMaster(ca.Id, ca.HeldClanId > 0 ? ca.HeldClanId : 0));
            }

            if (Clanid != 0)
            { // クラン所属
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(Clanname);
                if (clan != null)
                {
                    // プリンスまたはプリンセス、かつ、血盟主で自クランが城主
                    if (Crown && (Id == clan.LeaderId) && (clan.CastleId != 0))
                    {
                        sendPackets(new S_CastleMaster(clan.CastleId, Id));
                    }
                }
            }

            sendVisualEffect();
        }

        public virtual void sendVisualEffectAtTeleport()
        {
            if (Drink)
            { // liquorで酔っている
                sendPackets(new S_Liquor(Id, 1));
            }

            sendVisualEffect();
        }

        private IList<int> skillList = ListFactory.NewList<int>();

        public virtual int SkillMastery
        {
            set
            {
                if (!skillList.Contains(value))
                {
                    skillList.Add(value);
                }
            }
        }

        public virtual void removeSkillMastery(int skillid)
        {
            //if (skillList.Contains(skillid))
            {
                skillList.Remove(skillid);
            }
        }

        public virtual bool isSkillMastery(int skillid)
        {
            return skillList.Contains(skillid);
        }

        public virtual void clearSkillMastery()
        {
            skillList.Clear();
        }

        // 寵物競速
        private int _lap = 1;

        public virtual int Lap
        {
            set
            {
                _lap = value;
            }
            get
            {
                return _lap;
            }
        }


        private int _lapCheck = 0;

        public virtual int LapCheck
        {
            set
            {
                _lapCheck = value;
            }
            get
            {
                return _lapCheck;
            }
        }


        /// <summary>
        /// 只是將總圈數的完程度數量化
        /// </summary>
        public virtual int LapScore
        {
            get
            {
                return _lap * 29 + _lapCheck;
            }
        }

        // 補充
        private bool _order_list = false;

        public virtual bool InOrderList
        {
            get
            {
                return _order_list;
            }
            set
            {
                _order_list = value;
            }
        }


        public L1PcInstance()
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
            _accessLevel = 0;
            _currentWeapon = 0;
            _inventory = new L1PcInventory(this);
            _dwarf = new L1DwarfInventory(this);
            _dwarfForElf = new L1DwarfForElfInventory(this);
            _tradewindow = new L1Inventory();
            _bookmarks = ListFactory.NewList<L1BookMark>();
            _quest = new L1Quest(this);
            _equipSlot = new L1EquipmentSlot(this); // コンストラクタでthisポインタを渡すのは安全だろうか・・・
        }
        public override int CurrentHp
        {
            set
            {
                if (CurrentHp == value)
                {
                    return;
                }
                int currentHp = value;
                if (currentHp >= getMaxHp())
                {
                    currentHp = getMaxHp();
                }
                CurrentHpDirect = currentHp;
                sendPackets(new S_HPUpdate(currentHp, getMaxHp()));
                if (InParty)
                { // パーティー中
                    Party.updateMiniHP(this);
                }
            }
        }

        public override int CurrentMp
        {
            set
            {
                if (CurrentMp == value)
                {
                    return;
                }
                int currentMp = value;
                if ((currentMp >= getMaxMp()) || Gm)
                {
                    currentMp = getMaxMp();
                }
                CurrentMpDirect = currentMp;
                sendPackets(new S_MPUpdate(currentMp, getMaxMp()));
            }
        }

        public override L1Inventory Inventory
        {
            get
            {
                return _inventory;
            }
        }

        public virtual L1DwarfInventory DwarfInventory
        {
            get
            {
                return _dwarf;
            }
        }

        public virtual L1DwarfForElfInventory DwarfForElfInventory
        {
            get
            {
                return _dwarfForElf;
            }
        }

        public virtual L1Inventory TradeWindowInventory
        {
            get
            {
                return _tradewindow;
            }
        }

        public virtual bool GmInvis
        {
            get
            {
                return _gmInvis;
            }
            set
            {
                _gmInvis = value;
            }
        }


        public virtual int CurrentWeapon
        {
            get
            {
                return _currentWeapon;
            }
            set
            {
                _currentWeapon = value;
            }
        }


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


        public virtual short AccessLevel
        {
            get
            {
                return _accessLevel;
            }
            set
            {
                _accessLevel = value;
            }
        }


        public virtual int ClassId
        {
            get
            {
                return _classId;
            }
            set
            {
                _classId = value;
                _classFeature = L1ClassFeature.NewClassFeature(value);
            }
        }


        private L1ClassFeature _classFeature = null;

        public virtual L1ClassFeature ClassFeature
        {
            get
            {
                return _classFeature;
            }
        }

        public override long Exp
        {
            get
            {
                lock (this)
                {
                    return _exp;
                }
            }
            set
            {
                lock (this)
                {
                    _exp = value;
                }
            }
        }


        private int _PKcount; // ● PKカウント

        public virtual int get_PKcount()
        {
            return _PKcount;
        }

        public virtual void set_PKcount(int i)
        {
            _PKcount = i;
        }

        private int _PkCountForElf; // ● PKカウント(エルフ用)

        public virtual int PkCountForElf
        {
            get
            {
                return _PkCountForElf;
            }
            set
            {
                _PkCountForElf = value;
            }
        }


        private int _clanid; // ● クランＩＤ

        public virtual int Clanid
        {
            get
            {
                return _clanid;
            }
            set
            {
                _clanid = value;
            }
        }


        private string clanname; // ● クラン名

        public virtual string Clanname
        {
            get
            {
                return clanname;
            }
            set
            {
                clanname = value;
            }
        }


        // 参照を持つようにしたほうがいいかもしれない
        public virtual L1Clan Clan
        {
            get
            {
                return Container.Instance.Resolve<IGameWorld>().getClan(Clanname);
            }
        }

        private int _clanRank; // ● クラン内のランク(血盟君主、守護騎士、一般、見習)

        public virtual int ClanRank
        {
            get
            {
                return _clanRank;
            }
            set
            {
                _clanRank = value;
            }
        }


        private int _clanMemberId; // 血盟成員Id

        public virtual int ClanMemberId
        {
            get
            {
                return _clanMemberId;
            }
            set
            {
                _clanMemberId = value;
            }
        }


        private string _clanMemberNotes; // 血盟成員備註

        public virtual string ClanMemberNotes
        {
            get
            {
                return _clanMemberNotes;
            }
            set
            {
                _clanMemberNotes = value;
            }
        }



        // 角色生日
        private DateTime _birthday;

        public virtual DateTime Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
            }
        }

        public virtual int SimpleBirthday
        {
            get
            {
                return (_birthday.Year * 10000) + (_birthday.Month * 100) + _birthday.Day;
            }
        }


        public virtual void setBirthday()
        {
            _birthday = DateTime.Now;
        }

        private byte _sex; // ● 性別

        public virtual byte get_sex()
        {
            return _sex;
        }

        public virtual void set_sex(int i)
        {
            _sex = (byte)i;
        }

        public virtual bool Gm
        {
            get
            {
                return _gm;
            }
            set
            {
                _gm = value;
            }
        }


        public virtual bool Monitor
        {
            get
            {
                return _monitor;
            }
            set
            {
                _monitor = value;
            }
        }


        private L1PcInstance Stat
        {
            get
            {
                return null;
            }
        }

        public virtual void reduceCurrentHp(double d, L1Character l1character)
        {
            Stat.reduceCurrentHp(d, l1character);
        }

        /// <summary>
        /// 指定されたプレイヤー群にログアウトしたことを通知する
        /// </summary>
        /// <param name="playersList">
        ///            通知するプレイヤーの配列 </param>
        private void notifyPlayersLogout(IList<L1PcInstance> playersArray)
        {
            foreach (L1PcInstance player in playersArray)
            {
                if (player.knownsObject(this))
                {
                    player.removeKnownObject(this);
                    player.sendPackets(new S_RemoveObject(this));
                }
            }
        }

        public virtual void logout()
        {
            IGameWorld world = Container.Instance.Resolve<IGameWorld>();
            if (Clanid != 0) // クラン所属
            {
                L1Clan clan = world.getClan(Clanname);
                if (clan != null)
                {
                    if (clan.WarehouseUsingChar == Id) // 自キャラがクラン倉庫使用中
                    {
                        clan.WarehouseUsingChar = 0; // クラン倉庫のロックを解除
                    }
                }
            }
            notifyPlayersLogout(KnownPlayers);
            world.removeVisibleObject(this);
            world.removeObject(this);
            notifyPlayersLogout(world.getRecognizePlayer(this));
            _inventory.clearItems();
            _dwarf.clearItems();
            removeAllKnownObjects();
            stopHpRegeneration();
            stopMpRegeneration();
            Dead = true; // 使い方おかしいかもしれないけど、ＮＰＣに消滅したことをわからせるため
            NetConnection = null;
            PacketOutput = null;
        }

        public virtual ClientThread NetConnection
        {
            get
            {
                return _netConnection;
            }
            set
            {
                _netConnection = value;
            }
        }


        public virtual bool InParty
        {
            get
            {
                return Party != null;
            }
        }

        public virtual L1Party Party
        {
            get
            {
                return _party;
            }
            set
            {
                _party = value;
            }
        }


        public virtual bool InChatParty
        {
            get
            {
                return ChatParty != null;
            }
        }

        public virtual L1ChatParty ChatParty
        {
            get
            {
                return _chatParty;
            }
            set
            {
                _chatParty = value;
            }
        }


        public virtual int PartyID
        {
            get
            {
                return _partyID;
            }
            set
            {
                _partyID = value;
            }
        }


        public virtual int TradeID
        {
            get
            {
                return _tradeID;
            }
            set
            {
                _tradeID = value;
            }
        }


        public virtual bool TradeOk
        {
            set
            {
                _tradeOk = value;
            }
            get
            {
                return _tradeOk;
            }
        }


        public virtual int TempID
        {
            get
            {
                return _tempID;
            }
            set
            {
                _tempID = value;
            }
        }


        public virtual bool Teleport
        {
            get
            {
                return _isTeleport;
            }
            set
            {
                _isTeleport = value;
            }
        }


        public virtual bool Drink
        {
            get
            {
                return _isDrink;
            }
            set
            {
                _isDrink = value;
            }
        }


        public virtual bool Gres
        {
            get
            {
                return _isGres;
            }
            set
            {
                _isGres = value;
            }
        }


        public virtual bool PinkName
        {
            get
            {
                return _isPinkName;
            }
            set
            {
                _isPinkName = value;
            }
        }


        private IList<L1PrivateShopSellList> _sellList = ListFactory.NewList<L1PrivateShopSellList>();

        public virtual IList<L1PrivateShopSellList> SellList
        {
            get
            {
                return _sellList;
            }
        }

        private IList<L1PrivateShopBuyList> _buyList = ListFactory.NewList<L1PrivateShopBuyList>();

        public virtual IList<L1PrivateShopBuyList> BuyList
        {
            get
            {
                return _buyList;
            }
        }

        private byte[] _shopChat;

        public virtual byte[] ShopChat
        {
            set
            {
                _shopChat = value;
            }
            get
            {
                return _shopChat;
            }
        }


        private bool _isPrivateShop = false;

        public virtual bool PrivateShop
        {
            get
            {
                return _isPrivateShop;
            }
            set
            {
                _isPrivateShop = value;
            }
        }


        private bool _isTradingInPrivateShop = false;

        public virtual bool TradingInPrivateShop
        {
            get
            {
                return _isTradingInPrivateShop;
            }
            set
            {
                _isTradingInPrivateShop = value;
            }
        }


        private int _partnersPrivateShopItemCount = 0; // 閲覧中の個人商店のアイテム数

        public virtual int PartnersPrivateShopItemCount
        {
            get
            {
                return _partnersPrivateShopItemCount;
            }
            set
            {
                _partnersPrivateShopItemCount = value;
            }
        }


        private IPacketOutput _out;

        public virtual IPacketOutput PacketOutput
        {
            set
            {
                _out = value;
            }
        }

        public virtual void sendPackets(ServerBasePacket serverbasepacket)
        {
            if (_out == null)
            {
                return;
            }

            try
            {
                _out.SendPacket(serverbasepacket);
            }
            catch (Exception)
            {
            }
        }

        public override void onAction(L1PcInstance attacker)
        {
            onAction(attacker, 0);
        }

        public override void onAction(L1PcInstance attacker, int skillId)
        {
            // XXX:NullPointerException回避。onActionの引数の型はL1Characterのほうが良い？
            if (attacker == null)
            {
                return;
            }
            // テレポート処理中
            if (Teleport)
            {
                return;
            }
            // 攻撃される側または攻撃する側がセーフティーゾーン
            if ((ZoneType == 1) || (attacker.ZoneType == 1))
            {
                // 攻撃モーション送信
                L1Attack attack_mortion = new L1Attack(attacker, this, skillId);
                attack_mortion.action();
                return;
            }

            if (checkNonPvP(this, attacker) == true)
            {
                // 攻撃モーション送信
                L1Attack attack_mortion = new L1Attack(attacker, this, skillId);
                attack_mortion.action();
                return;
            }

            if ((CurrentHp > 0) && !Dead)
            {
                attacker.delInvis();

                bool isCounterBarrier = false;
                L1Attack attack = new L1Attack(attacker, this, skillId);
                if (attack.calcHit())
                {
                    if (hasSkillEffect(L1SkillId.COUNTER_BARRIER))
                    {
                        L1Magic magic = new L1Magic(this, attacker);
                        bool isProbability = magic.calcProbabilityMagic(L1SkillId.COUNTER_BARRIER);
                        bool isShortDistance = attack.ShortDistance;
                        if (isProbability && isShortDistance)
                        {
                            isCounterBarrier = true;
                        }
                    }
                    if (!isCounterBarrier)
                    {
                        attacker.PetTarget = this;

                        attack.calcDamage();
                        attack.calcStaffOfMana();
                        attack.addPcPoisonAttack(attacker, this);
                        attack.addChaserAttack();
                    }
                }
                if (isCounterBarrier)
                {
                    attack.actionCounterBarrier();
                    attack.commitCounterBarrier();
                }
                else
                {
                    attack.action();
                    attack.commit();
                }
            }
        }

        public virtual bool checkNonPvP(L1PcInstance pc, L1Character target)
        {
            L1PcInstance targetpc = null;
            if (target is L1PcInstance)
            {
                targetpc = (L1PcInstance)target;
            }
            else if (target is L1PetInstance)
            {
                targetpc = (L1PcInstance)((L1PetInstance)target).Master;
            }
            else if (target is L1SummonInstance)
            {
                targetpc = (L1PcInstance)((L1SummonInstance)target).Master;
            }
            if (targetpc == null)
            {
                return false; // 相手がPC、サモン、ペット以外
            }
            if (!Config.ALT_NONPVP)
            { // Non-PvP設定
                if (Map.isCombatZone(Location))
                {
                    return false;
                }

                // 全戦争リストを取得
                foreach (L1War war in Container.Instance.Resolve<IGameWorld>().WarList)
                {
                    if ((pc.Clanid != 0) && (targetpc.Clanid != 0))
                    { // 共にクラン所属中
                        bool same_war = war.CheckClanInSameWar(pc.Clanname, targetpc.Clanname);
                        if (same_war == true)
                        { // 同じ戦争に参加中
                            return false;
                        }
                    }
                }
                // Non-PvP設定でも戦争中は布告なしで攻撃可能
                if (target is L1PcInstance)
                {
                    L1PcInstance targetPc = (L1PcInstance)target;
                    if (isInWarAreaAndWarTime(pc, targetPc))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isInWarAreaAndWarTime(L1PcInstance pc, L1PcInstance target)
        {
            // pcとtargetが戦争中に戦争エリアに居るか
            int castleId = L1CastleLocation.getCastleIdByArea(pc);
            int targetCastleId = L1CastleLocation.getCastleIdByArea(target);
            if ((castleId != 0) && (targetCastleId != 0) && (castleId == targetCastleId))
            {
                if (Container.Instance.Resolve<IWarController>().isNowWar(castleId))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual L1Character PetTarget
        {
            set
            {
                object[] petList = PetList.Values.ToArray();
                foreach (object pet in petList)
                {
                    if (pet is L1PetInstance)
                    {
                        L1PetInstance pets = (L1PetInstance)pet;
                        pets.MasterTarget = value;
                    }
                    else if (pet is L1SummonInstance)
                    {
                        L1SummonInstance summon = (L1SummonInstance)pet;
                        summon.MasterTarget = value;
                    }
                }
            }
        }

        public virtual void delInvis()
        {
            // 魔法接続時間内はこちらを利用
            if (hasSkillEffect(L1SkillId.INVISIBILITY))
            { // インビジビリティ
                killSkillEffectTimer(L1SkillId.INVISIBILITY);
                sendPackets(new S_Invis(Id, 0));
                broadcastPacket(new S_OtherCharPacks(this));
            }
            if (hasSkillEffect(L1SkillId.BLIND_HIDING))
            { // ブラインド ハイディング
                killSkillEffectTimer(L1SkillId.BLIND_HIDING);
                sendPackets(new S_Invis(Id, 0));
                broadcastPacket(new S_OtherCharPacks(this));
            }
        }

        public virtual void delBlindHiding()
        {
            // 魔法接続時間終了はこちら
            killSkillEffectTimer(L1SkillId.BLIND_HIDING);
            sendPackets(new S_Invis(Id, 0));
            broadcastPacket(new S_OtherCharPacks(this));
        }

        // 魔法のダメージの場合はここを使用 (ここで魔法ダメージ軽減処理) attr:0.無属性魔法,1.地魔法,2.火魔法,3.水魔法,4.風魔法
        public virtual void receiveDamage(L1Character attacker, int damage, int attr)
        {
            int player_mr = Mr;
            int rnd = RandomHelper.Next(100) + 1;
            if (player_mr >= rnd)
            {
                damage /= 2;
            }
            receiveDamage(attacker, damage, false);
        }

        public virtual void receiveManaDamage(L1Character attacker, int mpDamage)
        { // 攻撃でＭＰを減らすときはここを使用
            if ((mpDamage > 0) && !Dead)
            {
                delInvis();
                if (attacker is L1PcInstance)
                {
                    L1PinkName.onAction(this, attacker);
                }
                if ((attacker is L1PcInstance) && ((L1PcInstance)attacker).PinkName)
                {
                    // ガードが画面内にいれば、攻撃者をガードのターゲットに設定する
                    foreach (GameObject @object in Container.Instance.Resolve<IGameWorld>().getVisibleObjects(attacker))
                    {
                        if (@object is L1GuardInstance)
                        {
                            L1GuardInstance guard = (L1GuardInstance)@object;
                            guard.Target = ((L1PcInstance)attacker);
                        }
                    }
                }

                int newMp = CurrentMp - mpDamage;
                if (newMp > getMaxMp())
                {
                    newMp = getMaxMp();
                }

                if (newMp <= 0)
                {
                    newMp = 0;
                }
                CurrentMp = newMp;
            }
        }

        public DateTime _oldTime; // 連続魔法ダメージの軽減に使用する

        public virtual void receiveDamage(L1Character attacker, double damage, bool isMagicDamage)
        { // 攻撃でＨＰを減らすときはここを使用
            if ((CurrentHp > 0) && !Dead)
            {
                if (attacker != this)
                {
                    if (!(attacker is L1EffectInstance) && !knownsObject(attacker) && (attacker.MapId == MapId))
                    {
                        attacker.onPerceive(this);
                    }
                }

                if (isMagicDamage == true)
                { // 連続魔法ダメージによる軽減
                    DateTime nowTime = DateTime.Now;
                    double interval = (20D - (nowTime - _oldTime).TotalMilliseconds / 100D) % 20D;

                    if (damage > 0)
                    {
                        if (interval > 0)
                        {
                            damage *= (1D - interval / 30D);
                        }

                        if (damage < 1)
                        {
                            damage = 0;
                        }

                        _oldTime = nowTime; // 次回のために時間を保存
                    }
                }
                if (damage > 0)
                {
                    delInvis();
                    if (attacker is L1PcInstance)
                    {
                        L1PinkName.onAction(this, attacker);
                    }
                    if ((attacker is L1PcInstance) && ((L1PcInstance)attacker).PinkName)
                    {
                        // ガードが画面内にいれば、攻撃者をガードのターゲットに設定する
                        foreach (GameObject @object in Container.Instance.Resolve<IGameWorld>().getVisibleObjects(attacker))
                        {
                            if (@object is L1GuardInstance)
                            {
                                L1GuardInstance guard = (L1GuardInstance)@object;
                                guard.Target = ((L1PcInstance)attacker);
                            }
                        }
                    }
                    removeSkillEffect(L1SkillId.FOG_OF_SLEEPING);
                }

                if (hasSkillEffect(L1SkillId.MORTAL_BODY) && (Id != attacker.Id))
                {
                    int rnd = RandomHelper.Next(100) + 1;
                    if ((damage > 0) && (rnd <= 10))
                    {
                        if (attacker is L1PcInstance)
                        {
                            L1PcInstance attackPc = (L1PcInstance)attacker;
                            attackPc.sendPackets(new S_DoActionGFX(attackPc.Id, ActionCodes.ACTION_Damage));
                            attackPc.broadcastPacket(new S_DoActionGFX(attackPc.Id, ActionCodes.ACTION_Damage));
                            attackPc.receiveDamage(this, 30, false);
                        }
                        else if (attacker is L1NpcInstance)
                        {
                            L1NpcInstance attackNpc = (L1NpcInstance)attacker;
                            attackNpc.broadcastPacket(new S_DoActionGFX(attackNpc.Id, ActionCodes.ACTION_Damage));
                            attackNpc.receiveDamage(this, 30);
                        }
                    }
                }
                if (_inventory.checkEquipped(145) || _inventory.checkEquipped(149))
                { // ミノタウルスアックス
                    damage *= 1.5; // 被ダメ1.5倍
                }
                if (hasSkillEffect(L1SkillId.ILLUSION_AVATAR))
                { // 幻術師魔法 (幻覺：化身)
                    damage *= 1.2; // 被ダメ1.2倍
                }
                if (attacker is L1PetInstance)
                {
                    L1PetInstance pet = (L1PetInstance)attacker;
                    // 目標在安區、攻擊者在安區、NOPVP
                    if ((ZoneType == 1) || (pet.ZoneType == 1) || (checkNonPvP(this, pet)))
                    {
                        damage = 0;
                    }
                }
                else if (attacker is L1SummonInstance)
                {
                    L1SummonInstance summon = (L1SummonInstance)attacker;
                    // 目標在安區、攻擊者在安區、NOPVP
                    if ((ZoneType == 1) || (summon.ZoneType == 1) || (checkNonPvP(this, summon)))
                    {
                        damage = 0;
                    }
                }
                int newHp = CurrentHp - (int)(damage);
                if (newHp > getMaxHp())
                {
                    newHp = getMaxHp();
                }
                if (newHp <= 0)
                {
                    if (Gm)
                    {
                        CurrentHp = getMaxHp();
                    }
                    else
                    {
                        death(attacker);
                    }
                }
                if (newHp > 0)
                {
                    CurrentHp = newHp;
                }
            }
            else if (!Dead)
            { // 念のため
                System.Console.WriteLine("警告：プレイヤーのＨＰ減少処理が正しく行われていない箇所があります。※もしくは最初からＨＰ０");
                death(attacker);
            }
        }

        public virtual void death(L1Character lastAttacker)
        {
            lock (this)
            {
                if (Dead)
                {
                    return;
                }
                Dead = true;
                Status = ActionCodes.ACTION_Die;
            }

            //死亡, 取消交易
            if (TradeID != 0)
            {
                L1Trade trade = new L1Trade();
                trade.TradeCancel(this);
            }

            Container.Instance.Resolve<ITaskController>().execute(new Death(this, lastAttacker));
        }
        private class Death : IRunnable
        {
            private readonly L1PcInstance outerInstance;

            internal L1Character _lastAttacker;

            internal Death(L1PcInstance outerInstance, L1Character cha)
            {
                this.outerInstance = outerInstance;
                _lastAttacker = cha;
            }

            public void run()
            {
                L1Character lastAttacker = _lastAttacker;
                _lastAttacker = null;
                outerInstance.CurrentHp = 0;
                outerInstance.GresValid = false; // EXPロストするまでG-RES無効

                while (outerInstance.Teleport)
                { // テレポート中なら終わるまで待つ
                    try
                    {
                        Thread.Sleep(300);
                    }
                    catch (Exception)
                    {
                    }
                }

                outerInstance.stopHpRegeneration();
                outerInstance.stopMpRegeneration();

                int targetobjid = outerInstance.Id;
                outerInstance.Map.setPassable(outerInstance.Location, true);

                // エンチャントを解除する
                // 変身状態も解除されるため、キャンセレーションをかけてから変身状態に戻す
                int tempchargfx = 0;
                if (outerInstance.hasSkillEffect(L1SkillId.SHAPE_CHANGE))
                {
                    tempchargfx = outerInstance.TempCharGfx;
                    outerInstance.TempCharGfxAtDead = tempchargfx;
                }
                else
                {
                    outerInstance.TempCharGfxAtDead = outerInstance.ClassId;
                }

                // キャンセレーションをエフェクトなしでかける
                L1SkillUse l1skilluse = new L1SkillUse();
                l1skilluse.handleCommands(outerInstance, L1SkillId.CANCELLATION, outerInstance.Id, outerInstance.X, outerInstance.Y, null, 0, L1SkillUse.TYPE_LOGIN);

                // 戰鬥藥水
                if (outerInstance.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_BATTLE))
                {
                    outerInstance.removeSkillEffect(L1SkillId.EFFECT_POTION_OF_BATTLE);
                }
                // 象牙塔妙藥
                if (outerInstance.hasSkillEffect(L1SkillId.COOKING_WONDER_DRUG))
                {
                    outerInstance.removeSkillEffect(L1SkillId.COOKING_WONDER_DRUG);
                }

                outerInstance.sendPackets(new S_DoActionGFX(targetobjid, ActionCodes.ACTION_Die));
                outerInstance.broadcastPacket(new S_DoActionGFX(targetobjid, ActionCodes.ACTION_Die));

                L1PcInstance player = null;

                if (lastAttacker != outerInstance)
                {
                    // セーフティーゾーン、コンバットゾーンで最後に殺したキャラが
                    // プレイヤーorペットだったら、ペナルティなし
                    if (outerInstance.ZoneType != 0)
                    {
                        if (lastAttacker is L1PcInstance)
                        {
                            player = (L1PcInstance)lastAttacker;
                        }
                        else if (lastAttacker is L1PetInstance)
                        {
                            player = (L1PcInstance)((L1PetInstance)lastAttacker).Master;
                        }
                        else if (lastAttacker is L1SummonInstance)
                        {
                            player = (L1PcInstance)((L1SummonInstance)lastAttacker).Master;
                        }
                        if (player != null)
                        {
                            // 戦争中に戦争エリアに居る場合は例外
                            if (!outerInstance.isInWarAreaAndWarTime(outerInstance, player))
                            {
                                return;
                            }
                        }
                    }

                    bool sim_ret = outerInstance.simWarResult(lastAttacker); // 模擬戦
                    if (sim_ret == true)
                    { // 模擬戦中ならペナルティなし
                        return;
                    }
                }

                if (!outerInstance.Map.EnabledDeathPenalty)
                {
                    return;
                }

                // 決闘中ならペナルティなし
                L1PcInstance fightPc = null;
                if (lastAttacker is L1PcInstance)
                {
                    fightPc = (L1PcInstance)lastAttacker;
                }

                // 判斷是否屬於新手保護階段, 並且是被其他玩家所殺死
                bool isNovice = false;
                if (outerInstance.hasSkillEffect(L1SkillId.STATUS_NOVICE) && (fightPc != null))
                {

                    // 判斷是否在新手等級保護範圍內
                    if (fightPc.Level > (outerInstance.Level + Config.NOVICE_PROTECTION_LEVEL_RANGE))
                    {
                        isNovice = true;
                    }
                }

                if (fightPc != null)
                {
                    if ((outerInstance.FightId == fightPc.Id) && (fightPc.FightId == outerInstance.Id))
                    { // 決闘中
                        outerInstance.FightId = 0;
                        outerInstance.sendPackets(new S_PacketBox(S_PacketBox.MSG_DUEL, 0, 0));
                        fightPc.FightId = 0;
                        fightPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_DUEL, 0, 0));
                        return;
                    }
                }

                /*
				 * deathPenalty(); // EXPロスト
				 * 
				 * setGresValid(true); // EXPロストしたらG-RES有効
				 * 
				 * if (getExpRes() == 0) { setExpRes(1); }
				 */

                // ガードに殺された場合のみ、PKカウントを減らしガードに攻撃されなくなる
                if (lastAttacker is L1GuardInstance)
                {
                    if (outerInstance.get_PKcount() > 0)
                    {
                        outerInstance.set_PKcount(outerInstance.get_PKcount() - 1);
                    }
                    outerInstance.LastPk = default(DateTime);
                }
                if (lastAttacker is L1GuardianInstance)
                {
                    if (outerInstance.PkCountForElf > 0)
                    {
                        outerInstance.PkCountForElf = outerInstance.PkCountForElf - 1;
                    }
                    outerInstance.LastPkForElf = default(DateTime);
                }

                // 增加新手保護階段, 將不會損失道具(不會噴裝)
                if (!isNovice)
                {
                    // 一定の確率でアイテムをDROP
                    // アライメント32000以上で0%、以降-1000毎に0.4%
                    // アライメントが0未満の場合は-1000毎に0.8%
                    // アライメント-32000以下で最高51.2%のDROP率
                    int lostRate = (int)(((outerInstance.Lawful + 32768D) / 1000D - 65D) * 4D);
                    if (lostRate < 0)
                    {
                        lostRate *= -1;
                        if (outerInstance.Lawful < 0)
                        {
                            lostRate *= 2;
                        }
                        int rnd = RandomHelper.Next(1000) + 1;
                        if (rnd <= lostRate)
                        {
                            int count = 1;
                            if (outerInstance.Lawful <= -30000)
                            {
                                count = RandomHelper.Next(4) + 1;
                            }
                            else if (outerInstance.Lawful <= -20000)
                            {
                                count = RandomHelper.Next(3) + 1;
                            }
                            else if (outerInstance.Lawful <= -10000)
                            {
                                count = RandomHelper.Next(2) + 1;
                            }
                            else if (outerInstance.Lawful < 0)
                            {
                                count = RandomHelper.Next(1) + 1;
                            }
                            outerInstance.caoPenaltyResult(count);
                        }
                    }
                }

                bool castle_ret = outerInstance.castleWarResult(); // 攻城戦
                if (castle_ret == true)
                { // 攻城戦中で旗内なら赤ネームペナルティなし
                    return;
                }

                if (!outerInstance.Map.EnabledDeathPenalty)
                {
                    return;
                }

                // 增加新手保護階段, 將不會損失經驗值
                if (!isNovice)
                {
                    outerInstance.deathPenalty(); // EXPロスト
                    outerInstance.GresValid = true; // EXPロストしたらG-RES有効
                }

                if (outerInstance.get_PKcount() > 0)
                {
                    outerInstance.set_PKcount(outerInstance.get_PKcount() - 1);
                }
                outerInstance.LastPk = default(DateTime); ;

                // 最後に殺したキャラがプレイヤーだったら、赤ネームにする
                if (lastAttacker is L1PcInstance)
                {
                    player = (L1PcInstance)lastAttacker;
                }
                if (player != null)
                {
                    if ((outerInstance.Lawful >= 0) && (outerInstance.PinkName == false))
                    {
                        bool isChangePkCount = false;
                        // アライメントが30000未満の場合はPKカウント増加
                        if (player.Lawful < 30000)
                        {
                            player.set_PKcount(player.get_PKcount() + 1);
                            isChangePkCount = true;
                            if (player.Elf && outerInstance.Elf)
                            {
                                player.PkCountForElf = player.PkCountForElf + 1;
                            }
                        }
                        player.setLastPk();
                        /// <summary>
                        /// 正義值滿不會被警衛追殺 </summary>
                        if (player.Lawful == 32767)
                        {
                            player.LastPk = default(DateTime);
                        }
                        if (player.Elf && outerInstance.Elf)
                        {
                            player.setLastPkForElf();
                        }

                        // アライメント処理
                        // 公式の発表および各LVでのPKからつじつまの合うように変更
                        // （PK側のLVに依存し、高LVほどリスクも高い）
                        // 48あたりで-8kほど DKの時点で10k強
                        // 60で約20k強 65で30k弱
                        int lawful;

                        if (player.Level < 50)
                        {
                            lawful = -1 * (int)((Math.Pow(player.Level, 2) * 4));
                        }
                        else
                        {
                            lawful = -1 * (int)((Math.Pow(player.Level, 3) * 0.08));
                        }
                        // もし(元々のアライメント-1000)が計算後より低い場合
                        // 元々のアライメント-1000をアライメント値とする
                        // （連続でPKしたときにほとんど値が変わらなかった記憶より）
                        // これは上の式よりも自信度が低いうろ覚えですので
                        // 明らかにこうならない！という場合は修正お願いします
                        if ((player.Lawful - 1000) < lawful)
                        {
                            lawful = player.Lawful - 1000;
                        }

                        if (lawful <= -32768)
                        {
                            lawful = -32768;
                        }
                        player.Lawful = lawful;

                        S_Lawful s_lawful = new S_Lawful(player.Id, player.Lawful);
                        player.sendPackets(s_lawful);
                        player.broadcastPacket(s_lawful);

                        if (isChangePkCount && (player.get_PKcount() >= 5) && (player.get_PKcount() < 10))
                        {
                            // あなたのPK回数が%0になりました。回数が%1になると地獄行きです。
                            player.sendPackets(new S_RedMessage(551, player.get_PKcount().ToString(), "10"));
                        }
                        else if (isChangePkCount && (player.get_PKcount() >= 10))
                        {
                            player.beginHell(true);
                        }
                    }
                    else
                    {
                        outerInstance.PinkName = false;
                    }
                }
                outerInstance._pcDeleteTimer = new L1PcDeleteTimer(outerInstance);
                outerInstance._pcDeleteTimer.begin();
            }
        }

        public virtual void stopPcDeleteTimer()
        {
            if (_pcDeleteTimer != null)
            {
                _pcDeleteTimer.cancel();
                _pcDeleteTimer = null;
            }
        }

        private void caoPenaltyResult(int count)
        {
            for (int i = 0; i < count; i++)
            {
                L1ItemInstance item = _inventory.CaoPenalty();

                if (item != null)
                {
                    _inventory.tradeItem(item, item.Stackable ? item.Count : 1, Container.Instance.Resolve<IGameWorld>().getInventory(X, Y, MapId));
                    sendPackets(new S_ServerMessage(638, item.LogName)); // %0を失いました。
                }
                else
                {
                }
            }
        }

        public virtual bool castleWarResult()
        {
            if ((Clanid != 0) && Crown)
            { // クラン所属中プリのチェック
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(Clanname);
                // 全戦争リストを取得
                foreach (L1War war in Container.Instance.Resolve<IGameWorld>().WarList)
                {
                    int warType = war.GetWarType();
                    bool isInWar = war.CheckClanInWar(Clanname);
                    bool isAttackClan = war.CheckAttackClan(Clanname);
                    if ((Id == clan.LeaderId) && (warType == 1) && isInWar && isAttackClan)
                    {
                        string enemyClanName = war.GetEnemyClanName(Clanname);
                        if (!string.ReferenceEquals(enemyClanName, null))
                        {
                            war.CeaseWar(Clanname, enemyClanName); // 終結
                        }
                        break;
                    }
                }
            }

            int castleId = 0;
            bool isNowWar = false;
            castleId = L1CastleLocation.getCastleIdByArea(this);
            if (castleId != 0)
            { // 旗内に居る
                isNowWar = Container.Instance.Resolve<IWarController>().isNowWar(castleId);
            }
            return isNowWar;
        }

        public virtual bool simWarResult(L1Character lastAttacker)
        {
            if (Clanid == 0)
            { // クラン所属していない
                return false;
            }
            if (Config.SIM_WAR_PENALTY)
            { // 模擬戦ペナルティありの場合はfalse
                return false;
            }
            L1PcInstance attacker = null;
            string enemyClanName = null;
            bool sameWar = false;

            if (lastAttacker is L1PcInstance)
            {
                attacker = (L1PcInstance)lastAttacker;
            }
            else if (lastAttacker is L1PetInstance)
            {
                attacker = (L1PcInstance)((L1PetInstance)lastAttacker).Master;
            }
            else if (lastAttacker is L1SummonInstance)
            {
                attacker = (L1PcInstance)((L1SummonInstance)lastAttacker).Master;
            }
            else
            {
                return false;
            }

            // 全戦争リストを取得
            foreach (L1War war in Container.Instance.Resolve<IGameWorld>().WarList)
            {
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(Clanname);

                int warType = war.GetWarType();
                bool isInWar = war.CheckClanInWar(Clanname);
                if ((attacker != null) && (attacker.Clanid != 0))
                { // lastAttackerがPC、サモン、ペットでクラン所属中
                    sameWar = war.CheckClanInSameWar(Clanname, attacker.Clanname);
                }

                if ((Id == clan.LeaderId) && (warType == 2) && (isInWar == true))
                {
                    enemyClanName = war.GetEnemyClanName(Clanname);
                    if (!string.ReferenceEquals(enemyClanName, null))
                    {
                        war.CeaseWar(Clanname, enemyClanName); // 終結
                    }
                }

                if ((warType == 2) && sameWar)
                { // 模擬戦で同じ戦争に参加中の場合、ペナルティなし
                    return true;
                }
            }
            return false;
        }

        public virtual void resExp()
        {
            int oldLevel = Level;
            long needExp = ExpTable.getNeedExpNextLevel(oldLevel);
            long exp = 0;
            if (oldLevel < 45)
            {
                exp = (long)(needExp * 0.05);
            }
            else if (oldLevel == 45)
            {
                exp = (long)(needExp * 0.045);
            }
            else if (oldLevel == 46)
            {
                exp = (long)(needExp * 0.04);
            }
            else if (oldLevel == 47)
            {
                exp = (long)(needExp * 0.035);
            }
            else if (oldLevel == 48)
            {
                exp = (long)(needExp * 0.03);
            }
            else if (oldLevel >= 49)
            {
                exp = (long)(needExp * 0.025);
            }

            if (exp == 0)
            {
                return;
            }
            addExp(exp);
        }

        public virtual void deathPenalty()
        {
            int oldLevel = Level;
            long needExp = ExpTable.getNeedExpNextLevel(oldLevel);
            long exp = 0;
            if ((oldLevel >= 1) && (oldLevel < 11))
            {
                exp = 0;
            }
            else if ((oldLevel >= 11) && (oldLevel < 45))
            {
                exp = (long)(needExp * 0.1);
            }
            else if (oldLevel == 45)
            {
                exp = (long)(needExp * 0.09);
            }
            else if (oldLevel == 46)
            {
                exp = (long)(needExp * 0.08);
            }
            else if (oldLevel == 47)
            {
                exp = (long)(needExp * 0.07);
            }
            else if (oldLevel == 48)
            {
                exp = (long)(needExp * 0.06);
            }
            else if (oldLevel >= 49)
            {
                exp = (long)(needExp * 0.05);
            }

            if (exp == 0)
            {
                return;
            }

            if (ExpRes >= 0)
            {
                ExpRes = ExpRes + 1;
            }
            addExp(-exp);
        }

        private int _originalEr = 0; // ● オリジナルDEX ER補正

        public virtual int OriginalEr
        {
            get
            {

                return _originalEr;
            }
        }

        public virtual int Er
        {
            get
            {
                if (hasSkillEffect(L1SkillId.STRIKER_GALE))
                {
                    return 0;
                }

                int er = 0;
                if (Knight)
                {
                    er = Level / 4; // ナイト
                }
                else if (Crown || Elf)
                {
                    er = Level / 8; // 君主・エルフ
                }
                else if (Darkelf)
                {
                    er = Level / 6; // ダークエルフ
                }
                else if (Wizard)
                {
                    er = Level / 10; // ウィザード
                }
                else if (DragonKnight)
                {
                    er = Level / 7; // ドラゴンナイト
                }
                else if (Illusionist)
                {
                    er = Level / 9; // イリュージョニスト
                }

                er += (getDex() - 8) / 2;

                er += OriginalEr;

                if (hasSkillEffect(L1SkillId.DRESS_EVASION))
                {
                    er += 12;
                }
                if (hasSkillEffect(L1SkillId.SOLID_CARRIAGE))
                {
                    er += 15;
                }
                return er;
            }
        }

        public virtual L1BookMark getBookMark(string name)
        {
            for (int i = 0; i < _bookmarks.Count; i++)
            {
                L1BookMark element = _bookmarks[i];
                if (element.Name == name)
                {
                    return element;
                }

            }
            return null;
        }

        public virtual L1BookMark getBookMark(int id)
        {
            for (int i = 0; i < _bookmarks.Count; i++)
            {
                L1BookMark element = _bookmarks[i];
                if (element.Id == id)
                {
                    return element;
                }

            }
            return null;
        }

        public virtual int BookMarkSize
        {
            get
            {
                return _bookmarks.Count;
            }
        }

        public virtual void addBookMark(L1BookMark book)
        {
            _bookmarks.Add(book);
        }

        public virtual void removeBookMark(L1BookMark book)
        {
            _bookmarks.Remove(book);
        }

        public virtual L1ItemInstance Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = value;
            }
        }


        public virtual L1Quest Quest
        {
            get
            {
                return _quest;
            }
        }

        public virtual bool Crown
        {
            get
            {
                return ((ClassId == CLASSID_PRINCE) || (ClassId == CLASSID_PRINCESS));
            }
        }

        public virtual bool Knight
        {
            get
            {
                return ((ClassId == CLASSID_KNIGHT_MALE) || (ClassId == CLASSID_KNIGHT_FEMALE));
            }
        }

        public virtual bool Elf
        {
            get
            {
                return ((ClassId == CLASSID_ELF_MALE) || (ClassId == CLASSID_ELF_FEMALE));
            }
        }

        public virtual bool Wizard
        {
            get
            {
                return ((ClassId == CLASSID_WIZARD_MALE) || (ClassId == CLASSID_WIZARD_FEMALE));
            }
        }

        public virtual bool Darkelf
        {
            get
            {
                return ((ClassId == CLASSID_DARK_ELF_MALE) || (ClassId == CLASSID_DARK_ELF_FEMALE));
            }
        }

        public virtual bool DragonKnight
        {
            get
            {
                return ((ClassId == CLASSID_DRAGON_KNIGHT_MALE) || (ClassId == CLASSID_DRAGON_KNIGHT_FEMALE));
            }
        }

        public virtual bool Illusionist
        {
            get
            {
                return ((ClassId == CLASSID_ILLUSIONIST_MALE) || (ClassId == CLASSID_ILLUSIONIST_FEMALE));
            }
        }

        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private ClientThread _netConnection;
        private int _classId;
        private int _type;
        private long _exp;
        private readonly L1Karma _karma = new L1Karma();
        private bool _gm;
        private bool _monitor;
        private bool _gmInvis;
        private short _accessLevel;
        private int _currentWeapon;
        private readonly L1PcInventory _inventory;
        private readonly L1DwarfInventory _dwarf;
        private readonly L1DwarfForElfInventory _dwarfForElf;
        private readonly L1Inventory _tradewindow;
        private L1ItemInstance _weapon;
        private L1Party _party;
        private L1ChatParty _chatParty;
        private int _partyID;
        private int _tradeID;
        private bool _tradeOk;
        private int _tempID;
        private bool _isTeleport = false;
        private bool _isDrink = false;
        private bool _isGres = false;
        private bool _isPinkName = false;
        private readonly IList<L1BookMark> _bookmarks;
        private L1Quest _quest;
        private MpRegeneration _mpRegen;
        private MpRegenerationByDoll _mpRegenByDoll;
        private MpReductionByAwake _mpReductionByAwake;
        private HpRegeneration _hpRegen;
        private HpRegenerationByDoll _hpRegenByDoll;
        private ItemMakeByDoll _itemMakeByDoll;
        private RunnableExecuter _regenTimer = new RunnableExecuter();
        private bool _mpRegenActive;
        private bool _mpRegenActiveByDoll;
        private bool _mpReductionActiveByAwake;
        private bool _hpRegenActive;
        private bool _hpRegenActiveByDoll;
        private bool _ItemMakeActiveByDoll;
        private L1EquipmentSlot _equipSlot;
        private L1PcDeleteTimer _pcDeleteTimer;
        private string _accountName; // ● アカウントネーム

        public virtual string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }


        private short _baseMaxHp = 0; // ● ＭＡＸＨＰベース（1～32767）

        public virtual short BaseMaxHp
        {
            get
            {
                return _baseMaxHp;
            }
        }

        public virtual void addBaseMaxHp(short i)
        {
            i += _baseMaxHp;
            if (i >= 32767)
            {
                i = 32767;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addMaxHp(i - _baseMaxHp);
            _baseMaxHp = i;
        }

        private short _baseMaxMp = 0; // ● ＭＡＸＭＰベース（0～32767）

        public virtual short BaseMaxMp
        {
            get
            {
                return _baseMaxMp;
            }
        }

        public virtual void addBaseMaxMp(short i)
        {
            i += _baseMaxMp;
            if (i >= 32767)
            {
                i = 32767;
            }
            else if (i < 0)
            {
                i = 0;
            }
            addMaxMp(i - _baseMaxMp);
            _baseMaxMp = i;
        }

        private int _baseAc = 0; // ● ＡＣベース（-128～127）

        public virtual int BaseAc
        {
            get
            {
                return _baseAc;
            }
        }

        private int _originalAc = 0; // ● オリジナルDEX ＡＣ補正

        public virtual int OriginalAc
        {
            get
            {

                return _originalAc;
            }
        }

        private byte _baseStr = 0; // ● ＳＴＲベース（1～127）

        public virtual byte BaseStr
        {
            get
            {
                return _baseStr;
            }
        }

        public virtual void addBaseStr(byte i)
        {
            i += _baseStr;
            if (i >= 127)
            {
                i = 127;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addStr((byte)(i - _baseStr));
            _baseStr = i;
        }

        private byte _baseCon = 0; // ● ＣＯＮベース（1～127）

        public virtual byte BaseCon
        {
            get
            {
                return _baseCon;
            }
        }

        public virtual void addBaseCon(byte i)
        {
            i += _baseCon;
            if (i >= 127)
            {
                i = 127;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addCon((byte)(i - _baseCon));
            _baseCon = i;
        }

        private byte _baseDex = 0; // ● ＤＥＸベース（1～127）

        public virtual byte BaseDex
        {
            get
            {
                return _baseDex;
            }
        }

        public virtual void addBaseDex(byte i)
        {
            i += _baseDex;
            if (i >= 127)
            {
                i = 127;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addDex((byte)(i - _baseDex));
            _baseDex = i;
        }

        private byte _baseCha = 0; // ● ＣＨＡベース（1～127）

        public virtual byte BaseCha
        {
            get
            {
                return _baseCha;
            }
        }

        public virtual void addBaseCha(byte i)
        {
            i += _baseCha;
            if (i >= 127)
            {
                i = 127;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addCha((byte)(i - _baseCha));
            _baseCha = i;
        }

        private byte _baseInt = 0; // ● ＩＮＴベース（1～127）

        public virtual byte BaseInt
        {
            get
            {
                return _baseInt;
            }
        }

        public virtual void addBaseInt(byte i)
        {
            i += _baseInt;
            if (i >= 127)
            {
                i = 127;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addInt((byte)(i - _baseInt));
            _baseInt = i;
        }

        private byte _baseWis = 0; // ● ＷＩＳベース（1～127）

        public virtual byte BaseWis
        {
            get
            {
                return _baseWis;
            }
        }

        public virtual void addBaseWis(byte i)
        {
            i += _baseWis;
            if (i >= 127)
            {
                i = 127;
            }
            else if (i < 1)
            {
                i = 1;
            }
            addWis((byte)(i - _baseWis));
            _baseWis = i;
        }

        private int _originalStr = 0; // ● オリジナル STR

        public virtual int OriginalStr
        {
            get
            {
                return _originalStr;
            }
            set
            {
                _originalStr = value;
            }
        }


        private int _originalCon = 0; // ● オリジナル CON

        public virtual int OriginalCon
        {
            get
            {
                return _originalCon;
            }
            set
            {
                _originalCon = value;
            }
        }


        private int _originalDex = 0; // ● オリジナル DEX

        public virtual int OriginalDex
        {
            get
            {
                return _originalDex;
            }
            set
            {
                _originalDex = value;
            }
        }


        private int _originalCha = 0; // ● オリジナル CHA

        public virtual int OriginalCha
        {
            get
            {
                return _originalCha;
            }
            set
            {
                _originalCha = value;
            }
        }


        private int _originalInt = 0; // ● オリジナル INT

        public virtual int OriginalInt
        {
            get
            {
                return _originalInt;
            }
            set
            {
                _originalInt = value;
            }
        }


        private int _originalWis = 0; // ● オリジナル WIS

        public virtual int OriginalWis
        {
            get
            {
                return _originalWis;
            }
            set
            {
                _originalWis = value;
            }
        }


        private int _originalDmgup = 0; // ● オリジナルSTR ダメージ補正

        public virtual int OriginalDmgup
        {
            get
            {

                return _originalDmgup;
            }
        }

        private int _originalBowDmgup = 0; // ● オリジナルDEX 弓ダメージ補正

        public virtual int OriginalBowDmgup
        {
            get
            {

                return _originalBowDmgup;
            }
        }

        private int _originalHitup = 0; // ● オリジナルSTR 命中補正

        public virtual int OriginalHitup
        {
            get
            {

                return _originalHitup;
            }
        }

        private int _originalBowHitup = 0; // ● オリジナルDEX 命中補正

        public virtual int OriginalBowHitup
        {
            get
            {

                return _originalBowHitup;
            }
        }

        private int _originalMr = 0; // ● オリジナルWIS 魔法防御

        public virtual int OriginalMr
        {
            get
            {

                return _originalMr;
            }
        }

        private int _originalMagicHit = 0; // ● オリジナルINT 魔法命中

        public virtual int OriginalMagicHit
        {
            get
            {

                return _originalMagicHit;
            }
        }

        private int _originalMagicCritical = 0; // ● オリジナルINT 魔法クリティカル

        public virtual int OriginalMagicCritical
        {
            get
            {

                return _originalMagicCritical;
            }
        }

        private int _originalMagicConsumeReduction = 0; // ● オリジナルINT 消費MP軽減

        public virtual int OriginalMagicConsumeReduction
        {
            get
            {

                return _originalMagicConsumeReduction;
            }
        }

        private int _originalMagicDamage = 0; // ● オリジナルINT 魔法ダメージ

        public virtual int OriginalMagicDamage
        {
            get
            {

                return _originalMagicDamage;
            }
        }

        private int _originalHpup = 0; // ● オリジナルCON HP上昇値補正

        public virtual int OriginalHpup
        {
            get
            {

                return _originalHpup;
            }
        }

        private int _originalMpup = 0; // ● オリジナルWIS MP上昇値補正

        public virtual int OriginalMpup
        {
            get
            {

                return _originalMpup;
            }
        }

        private int _baseDmgup = 0; // ● ダメージ補正ベース（-128～127）

        public virtual int BaseDmgup
        {
            get
            {
                return _baseDmgup;
            }
        }

        private int _baseBowDmgup = 0; // ● 弓ダメージ補正ベース（-128～127）

        public virtual int BaseBowDmgup
        {
            get
            {
                return _baseBowDmgup;
            }
        }

        private int _baseHitup = 0; // ● 命中補正ベース（-128～127）

        public virtual int BaseHitup
        {
            get
            {
                return _baseHitup;
            }
        }

        private int _baseBowHitup = 0; // ● 弓命中補正ベース（-128～127）

        public virtual int BaseBowHitup
        {
            get
            {
                return _baseBowHitup;
            }
        }

        private int _baseMr = 0; // ● 魔法防御ベース（0～）

        public virtual int BaseMr
        {
            get
            {
                return _baseMr;
            }
        }

        private int _advenHp; // ● // アドバンスド スピリッツで増加しているＨＰ

        public virtual int AdvenHp
        {
            get
            {
                return _advenHp;
            }
            set
            {
                _advenHp = value;
            }
        }


        private int _advenMp; // ● // アドバンスド スピリッツで増加しているＭＰ

        public virtual int AdvenMp
        {
            get
            {
                return _advenMp;
            }
            set
            {
                _advenMp = value;
            }
        }


        private int _highLevel; // ● 過去最高レベル

        public virtual int HighLevel
        {
            get
            {
                return _highLevel;
            }
            set
            {
                _highLevel = value;
            }
        }


        private int _bonusStats; // ● 割り振ったボーナスステータス

        public virtual int BonusStats
        {
            get
            {
                return _bonusStats;
            }
            set
            {
                _bonusStats = value;
            }
        }


        private int _elixirStats; // ● エリクサーで上がったステータス

        public virtual int ElixirStats
        {
            get
            {
                return _elixirStats;
            }
            set
            {
                _elixirStats = value;
            }
        }


        private int _elfAttr; // ● エルフの属性

        public virtual int ElfAttr
        {
            get
            {
                return _elfAttr;
            }
            set
            {
                _elfAttr = value;
            }
        }


        private int _expRes; // ● EXP復旧

        public virtual int ExpRes
        {
            get
            {
                return _expRes;
            }
            set
            {
                _expRes = value;
            }
        }


        private int _partnerId; // ● 結婚相手

        public virtual int PartnerId
        {
            get
            {
                return _partnerId;
            }
            set
            {
                _partnerId = value;
            }
        }


        private int _onlineStatus; // ● オンライン状態

        public virtual int OnlineStatus
        {
            get
            {
                return _onlineStatus;
            }
            set
            {
                _onlineStatus = value;
            }
        }


        private int _homeTownId; // ● ホームタウン

        public virtual int HomeTownId
        {
            get
            {
                return _homeTownId;
            }
            set
            {
                _homeTownId = value;
            }
        }


        private int _contribution; // ● 貢献度

        public virtual int Contribution
        {
            get
            {
                return _contribution;
            }
            set
            {
                _contribution = value;
            }
        }


        private int _pay; // 村莊福利金 此欄位由 HomeTownTimeController 處理 update

        public virtual int Pay
        {
            get
            {
                return _pay;
            }
            set
            {
                _pay = value;
            }
        }


        // 地獄に滞在する時間（秒）
        private int _hellTime;

        public virtual int HellTime
        {
            get
            {
                return _hellTime;
            }
            set
            {
                _hellTime = value;
            }
        }


        private bool _banned; // ● 凍結

        public virtual bool Banned
        {
            get
            {
                return _banned;
            }
            set
            {
                _banned = value;
            }
        }


        public virtual L1EquipmentSlot EquipSlot
        {
            get
            {
                return _equipSlot;
            }
        }

        public override void set_food(int i)
        {
            base.set_food(i);
            setCryOfSurvivalTime();
        }

        // 生存吶喊 飽食度 100% 充電時間
        private long _cryofsurvivaltime;

        public virtual long CryOfSurvivalTime
        {
            get
            {
                return _cryofsurvivaltime;
            }
        }

        public virtual void setCryOfSurvivalTime()
        {
            if (get_food() >= 225)
            {
                _cryofsurvivaltime = DateTime.Now.Ticks / 1000;
            }
        }

        // 殺怪數紀錄
        private int _monskill = 0;

        public virtual int MonsKill
        {
            get
            {
                return _monskill;
            }
            set
            {
                _monskill = value;
                sendPackets(new S_OwnCharStatus(this));
            }
        }


        public virtual void addMonsKill()
        {
            _monskill += 1;
            sendPackets(new S_OwnCharStatus(this));
        }

        public static L1PcInstance load(string charName)
        {
            L1PcInstance result = null;
            try
            {
                result = Container.Instance.Resolve<ICharacterController>().loadCharacter(charName);
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return result;
        }

        /// <summary>
        ///  儲存玩家資料
        /// </summary>
        /// <returns>true = success</returns>
        public virtual bool Save()
        {
            if (Ghost)
            {
                return false;
            }
            if (InCharReset)
            {
                return false;
            }

            try
            {
                Container.Instance.Resolve<ICharacterController>().storeCharacter(this);
            }
            catch (Exception e)
            {
                _log.Error(e);
                return false;
            }

            return true;
        }
        // TODO 殷海薩的祝福
        private DateTime _lastActive;

        public virtual DateTime LastActive
        {
            get
            {
                return _lastActive;
            }
            set
            {
                _lastActive = value;
            }
        }


        public virtual void setLastActive()
        {
            _lastActive = DateTime.Now;
        }

        private int _ainZone = 1;

        public virtual int AinZone
        {
            set
            {
                _ainZone = value;
            }
            get
            {
                return _ainZone;
            }
        }


        private int _ainPoint = 0;

        public virtual int AinPoint
        {
            set
            {
                _ainPoint = value;
            }
            get
            {
                return _ainPoint;
            }
        }

        /// <summary>
        /// このプレイヤーのインベントリアイテムの状態をストレージへ書き込む。
        /// </summary>
        public virtual void saveInventory()
        {
            foreach (L1ItemInstance item in _inventory.Items)
            {
                _inventory.saveItem(item, item.RecordingColumns);
                _inventory.saveEnchantAccessory(item, item.RecordingColumnsEnchantAccessory);
            }
        }

        public const int REGENSTATE_NONE = 4;

        public const int REGENSTATE_MOVE = 2;

        public const int REGENSTATE_ATTACK = 1;

        public virtual int RegenState
        {
            set
            {
                _mpRegen.State = value;
                _hpRegen.State = value;
            }
        }

        public virtual double MaxWeight
        {
            get
            {
                int str = getStr();
                int con = getCon();
                double maxWeight = 150 * (Math.Floor(0.6 * str + 0.4 * con + 1));

                double weightReductionByArmor = WeightReduction; // 防具による重量軽減
                weightReductionByArmor /= 100;

                double weightReductionByDoll = 0; // マジックドールによる重量軽減
                weightReductionByDoll += L1MagicDoll.getWeightReductionByDoll(this);
                weightReductionByDoll /= 100;

                int weightReductionByMagic = 0;
                if (hasSkillEffect(L1SkillId.DECREASE_WEIGHT))
                { // ディクリースウェイト
                    weightReductionByMagic = 180;
                }

                double originalWeightReduction = 0; // オリジナルステータスによる重量軽減
                originalWeightReduction += 0.04 * (OriginalStrWeightReduction + OriginalConWeightReduction);

                double weightReduction = 1 + weightReductionByArmor + weightReductionByDoll + originalWeightReduction;

                maxWeight *= weightReduction;

                maxWeight += weightReductionByMagic;

                maxWeight *= Config.RATE_WEIGHT_LIMIT; // ウェイトレートを掛ける

                return maxWeight;
            }
        }

        public virtual bool Ribrave
        {
            get
            { // 生命之樹果實 移速 * 1.15
                return hasSkillEffect(L1SkillId.STATUS_RIBRAVE);
            }
        }

        public virtual bool ThirdSpeed
        {
            get
            { // 三段加速 * 1.15
                return hasSkillEffect(L1SkillId.STATUS_THIRD_SPEED);
            }
        }

        public virtual bool WindShackle
        {
            get
            { // 風之枷鎖  攻速 / 2
                return hasSkillEffect(L1SkillId.WIND_SHACKLE);
            }
        }

        private int invisDelayCounter = 0;

        public virtual bool InvisDelay
        {
            get
            {
                return (invisDelayCounter > 0);
            }
        }

        private object _invisTimerMonitor = new object();

        public virtual void addInvisDelayCounter(int counter)
        {
            lock (_invisTimerMonitor)
            {
                invisDelayCounter += counter;
            }
        }

        public virtual void beginInvisTimer()
        {
            addInvisDelayCounter(1);
            Container.Instance.Resolve<ITaskController>().execute((IRunnable)new L1PcInvisDelay(base.Id), 3000);
        }

        public virtual void addExp(long exp)
        {
            lock (this)
            {
                _exp += exp;
                if (_exp > ExpTable.MAX_EXP)
                {
                    _exp = ExpTable.MAX_EXP;
                }
            }
        }

        public virtual void addContribution(int contribution)
        {
            lock (this)
            {
                _contribution += contribution;
            }
        }

        public virtual void beginExpMonitor()
        {
            _expMonitorFuture = new L1PcExpMonitor(Id);
            Container.Instance.Resolve<ITaskController>().execute(_expMonitorFuture, 0, INTERVAL_EXP_MONITOR);
        }

        private void levelUp(int gap)
        {
            resetLevel();

            // 復活のポーション
            if (Level == 110 && Config.ALT_REVIVAL_POTION && !_inventory.checkItem(43000, 1))
            {
                try
                {
                    L1Item l1item = ItemTable.Instance.getTemplate(43000);
                    if (l1item != null)
                    {
                        _inventory.storeItem(43000, 1);
                        sendPackets(new S_ServerMessage(403, l1item.Name));
                    }
                    else
                    {
                        sendPackets(new S_SystemMessage("返生藥水取得失敗。"));
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    sendPackets(new S_SystemMessage("返生藥水取得失敗。"));
                }
            }

            for (int i = 0; i < gap; i++)
            {
                short randomHp = CalcStat.calcStatHp(Type, BaseMaxHp, BaseCon, OriginalHpup);
                short randomMp = CalcStat.calcStatMp(Type, BaseMaxMp, BaseWis, OriginalMpup);
                addBaseMaxHp(randomHp);
                addBaseMaxMp(randomMp);
                //TODO 升級後血魔補滿
                if (Config.MaxHPMP)
                {
                    CurrentHp = getMaxHp();
                    CurrentMp = getMaxMp();
                }
                //end
            }
            resetBaseHitup();
            resetBaseDmgup();
            resetBaseAc();
            resetBaseMr();
            if (Level > HighLevel)
            {
                HighLevel = Level;
            }

            try
            {
                // DBにキャラクター情報を書き込む
                Save();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            // ボーナスステータス
            if ((Level >= 51) && (Level - 50 > BonusStats))
            {
                if ((BaseStr + BaseDex + BaseCon + BaseInt + BaseWis + BaseCha) < Config.BONUS_STATS1 * 6)
                {
                    sendPackets(new S_bonusstats(Id, 1));
                }
            }
            sendPackets(new S_OwnCharStatus(this));
            LineageServer.william.Level.getItem(this); //TODO　升級獎勵道具  by 阿傑

            // 根據等級判斷地圖限制
            if ((MapId == 2005 || MapId == 86))
            { // 新手村
                if (Level >= 13)
                { // 等級大於13
                    if (Quest.get_step(L1Quest.QUEST_TUTOR) != 255)
                    {
                        Quest.set_step(L1Quest.QUEST_TUTOR, 255);
                    }
                    L1Teleport.teleport(this, 33084, 33391, (short)4, 5, true); // 銀騎士村
                }
            }
            else if (Level >= 52)
            { // 指定レベル
                if (MapId == 777)
                { // 見捨てられた者たちの地(影の神殿)
                    L1Teleport.teleport(this, 34043, 32184, (short)4, 5, true); // 象牙の塔前
                }
                else if ((MapId == 778) || (MapId == 779))
                { // 見捨てられた者たちの地(欲望の洞窟)
                    L1Teleport.teleport(this, 32608, 33178, (short)4, 5, true); // WB
                }
            }

            // 處理新手保護系統(遭遇的守護)狀態資料的變動
            checkNoviceType();
        }

        private void levelDown(int gap)
        {
            resetLevel();

            for (int i = 0; i > gap; i--)
            {
                // レベルダウン時はランダム値をそのままマイナスする為に、base値に0を設定
                short randomHp = CalcStat.calcStatHp(Type, 0, BaseCon, OriginalHpup);
                short randomMp = CalcStat.calcStatMp(Type, 0, BaseWis, OriginalMpup);
                addBaseMaxHp((short)-randomHp);
                addBaseMaxMp((short)-randomMp);
            }
            resetBaseHitup();
            resetBaseDmgup();
            resetBaseAc();
            resetBaseMr();
            if (Config.LEVEL_DOWN_RANGE != 0)
            {
                if (HighLevel - Level >= Config.LEVEL_DOWN_RANGE)
                {
                    sendPackets(new S_ServerMessage(64)); // ワールドとの接続が切断されました。
                    sendPackets(new S_Disconnect());
                    _log.Info(string.Format("レベルダウンの許容範囲を超えたため{0}を強制切断しました。", Name));
                }
            }

            try
            {
                // DBにキャラクター情報を書き込む
                Save();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            sendPackets(new S_OwnCharStatus(this));
            // 處理新手保護系統(遭遇的守護)狀態資料的變動
            checkNoviceType();
        }

        public virtual void beginGameTimeCarrier()
        {
            (new L1GameTimeCarrier(this)).start();
        }

        private bool _ghost = false; // ゴースト

        public virtual bool Ghost
        {
            get
            {
                return _ghost;
            }
            set
            {
                _ghost = value;
            }
        }


        private bool _ghostCanTalk = true; // NPCに話しかけられるか

        public virtual bool GhostCanTalk
        {
            get
            {
                return _ghostCanTalk;
            }
            set
            {
                _ghostCanTalk = value;
            }
        }


        private bool _isReserveGhost = false; // ゴースト解除準備

        public virtual bool ReserveGhost
        {
            get
            {
                return _isReserveGhost;
            }
            set
            {
                _isReserveGhost = value;
            }
        }


        public virtual void beginGhost(int locx, int locy, short mapid, bool canTalk)
        {
            beginGhost(locx, locy, mapid, canTalk, 0);
        }

        public virtual void beginGhost(int locx, int locy, short mapid, bool canTalk, int sec)
        {
            if (Ghost)
            {
                return;
            }
            Ghost = true;
            _ghostSaveLocX = X;
            _ghostSaveLocY = Y;
            _ghostSaveMapId = MapId;
            _ghostSaveHeading = Heading;
            GhostCanTalk = canTalk;
            L1Teleport.teleport(this, locx, locy, mapid, 5, true);
            if (sec > 0)
            {
                _ghostFuture = new L1PcGhostMonitor(Id);
                Container.Instance.Resolve<ITaskController>().execute((IRunnable)this._ghostFuture, sec * 1000);
            }
        }

        public virtual void makeReadyEndGhost()
        {
            ReserveGhost = true;
            L1Teleport.teleport(this, _ghostSaveLocX, _ghostSaveLocY, _ghostSaveMapId, _ghostSaveHeading, true);
        }

        public virtual void endGhost()
        {
            Ghost = false;
            GhostCanTalk = true;
            ReserveGhost = false;
        }

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: private java.util.concurrent.ScheduledFuture<?> _ghostFuture;
        private ITimerTask _ghostFuture;

        private int _ghostSaveLocX = 0;

        private int _ghostSaveLocY = 0;

        private short _ghostSaveMapId = 0;

        private int _ghostSaveHeading = 0;

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: private java.util.concurrent.ScheduledFuture<?> _hellFuture;
        private ITimerTask _hellFuture;

        public virtual void beginHell(bool isFirst)
        {
            // 地獄以外に居るときは地獄へ強制移動
            if (MapId != 666)
            {
                int locx = 32701;
                int locy = 32777;
                short mapid = 666;
                L1Teleport.teleport(this, locx, locy, mapid, 5, false);
            }

            if (isFirst)
            {
                if (get_PKcount() <= 10)
                {
                    HellTime = 300;
                }
                else
                {
                    HellTime = 300 * (get_PKcount() - 10) + 300;
                }
                // あなたのPK回数が%0になり、地獄に落とされました。あなたはここで%1分間反省しなければなりません。
                sendPackets(new S_RedMessage(552, get_PKcount().ToString(), (HellTime / 60).ToString()));
            }
            else
            {
                // あなたは%0秒間ここにとどまらなければなりません。
                sendPackets(new S_RedMessage(637, HellTime.ToString()));
            }
            if (_hellFuture == null)
            {
                _hellFuture = new L1PcHellMonitor(Id);
                Container.Instance.Resolve<ITaskController>().execute(_hellFuture, 0, 1000);
            }
        }

        public virtual void endHell()
        {
            if (_hellFuture != null)
            {
                _hellFuture.cancel();
                _hellFuture = null;
            }
            // 地獄から脱出したら火田村へ帰還させる。
            int[] loc = L1TownLocation.getGetBackLoc(L1TownLocation.TOWNID_ORCISH_FOREST);
            L1Teleport.teleport(this, loc[0], loc[1], (short)loc[2], 5, true);
            try
            {
                Save();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public override int PoisonEffect
        {
            set
            {
                sendPackets(new S_Poison(Id, value));

                if (!GmInvis && !Ghost && !Invisble)
                {
                    broadcastPacket(new S_Poison(Id, value));
                }
                if (GmInvis || Ghost)
                {
                }
                else if (Invisble)
                {
                    broadcastPacketForFindInvis(new S_Poison(Id, value), true);
                }
                else
                {
                    broadcastPacket(new S_Poison(Id, value));
                }
            }
        }

        public override void healHp(int pt)
        {
            base.healHp(pt);

            sendPackets(new S_HPUpdate(this));
        }

        public override int Karma
        {
            get
            {
                return _karma.get();
            }
            set
            {
                _karma.set(value);
            }
        }


        public virtual void addKarma(int i)
        {
            lock (_karma)
            {
                _karma.add(i);
            }
        }

        public virtual int KarmaLevel
        {
            get
            {
                return _karma.Level;
            }
        }

        public virtual int KarmaPercent
        {
            get
            {
                return _karma.Percent;
            }
        }

        private DateTime _lastPk;

        /// <summary>
        /// プレイヤーの最終PK時間を返す。
        /// </summary>
        /// <returns> _lastPk
        ///  </returns>
        public virtual DateTime LastPk
        {
            get
            {
                return _lastPk;
            }
            set
            {
                _lastPk = value;
            }
        }


        /// <summary>
        /// プレイヤーの最終PK時間を現在の時刻に設定する。
        /// </summary>
        public virtual void setLastPk()
        {
            _lastPk = DateTime.Now;
        }

        /// <summary>
        /// プレイヤーが手配中であるかを返す。
        /// </summary>
        /// <returns> 手配中であれば、true </returns>
        public virtual bool Wanted
        {
            get
            {
                if (_lastPk == default(DateTime))
                {
                    return false;
                }
                else if ((DateTime.Now - _lastPk).TotalMilliseconds > 24 * 3600 * 1000)
                {
                    _lastPk = default(DateTime);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private DateTime _lastPkForElf;

        public virtual DateTime LastPkForElf
        {
            get
            {
                return _lastPkForElf;
            }
            set
            {
                _lastPkForElf = value;
            }
        }


        public virtual void setLastPkForElf()
        {
            _lastPkForElf = DateTime.Now;
        }

        public virtual bool WantedForElf
        {
            get
            {
                if (_lastPkForElf == default(DateTime))
                {
                    return false;
                }
                else if ((DateTime.Now - _lastPkForElf).TotalMilliseconds > 24 * 3600 * 1000)
                {
                    _lastPkForElf = default(DateTime);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private DateTime _deleteTime; // キャラクター削除までの時間

        public virtual DateTime DeleteTime
        {
            get
            {
                return _deleteTime;
            }
            set
            {
                _deleteTime = value;
            }
        }


        public override int MagicLevel
        {
            get
            {
                return ClassFeature.GetMagicLevel(Level);
            }
        }

        private int _weightReduction = 0;

        public virtual int WeightReduction
        {
            get
            {
                return _weightReduction;
            }
        }

        public virtual void addWeightReduction(int i)
        {
            _weightReduction += i;
        }

        private int _originalStrWeightReduction = 0; // ● オリジナルSTR 重量軽減

        public virtual int OriginalStrWeightReduction
        {
            get
            {

                return _originalStrWeightReduction;
            }
        }

        private int _originalConWeightReduction = 0; // ● オリジナルCON 重量軽減

        public virtual int OriginalConWeightReduction
        {
            get
            {

                return _originalConWeightReduction;
            }
        }

        private int _hasteItemEquipped = 0;

        public virtual int HasteItemEquipped
        {
            get
            {
                return _hasteItemEquipped;
            }
        }

        public virtual void addHasteItemEquipped(int i)
        {
            _hasteItemEquipped += i;
        }

        public virtual void removeHasteSkillEffect()
        {
            if (hasSkillEffect(L1SkillId.SLOW))
            {
                removeSkillEffect(L1SkillId.SLOW);
            }
            if (hasSkillEffect(L1SkillId.MASS_SLOW))
            {
                removeSkillEffect(L1SkillId.MASS_SLOW);
            }
            if (hasSkillEffect(L1SkillId.ENTANGLE))
            {
                removeSkillEffect(L1SkillId.ENTANGLE);
            }
            if (hasSkillEffect(L1SkillId.HASTE))
            {
                removeSkillEffect(L1SkillId.HASTE);
            }
            if (hasSkillEffect(L1SkillId.GREATER_HASTE))
            {
                removeSkillEffect(L1SkillId.GREATER_HASTE);
            }
            if (hasSkillEffect(L1SkillId.STATUS_HASTE))
            {
                removeSkillEffect(L1SkillId.STATUS_HASTE);
            }
        }

        private int _damageReductionByArmor = 0; // 防具によるダメージ軽減

        public virtual int DamageReductionByArmor
        {
            get
            {
                return _damageReductionByArmor;
            }
        }

        public virtual void addDamageReductionByArmor(int i)
        {
            _damageReductionByArmor += i;
        }

        private int _hitModifierByArmor = 0; // 防具による命中率補正

        public virtual int HitModifierByArmor
        {
            get
            {
                return _hitModifierByArmor;
            }
        }

        public virtual void addHitModifierByArmor(int i)
        {
            _hitModifierByArmor += i;
        }

        private int _dmgModifierByArmor = 0; // 防具によるダメージ補正

        public virtual int DmgModifierByArmor
        {
            get
            {
                return _dmgModifierByArmor;
            }
        }

        public virtual void addDmgModifierByArmor(int i)
        {
            _dmgModifierByArmor += i;
        }

        private int _bowHitModifierByArmor = 0; // 防具による弓の命中率補正

        public virtual int BowHitModifierByArmor
        {
            get
            {
                return _bowHitModifierByArmor;
            }
        }

        public virtual void addBowHitModifierByArmor(int i)
        {
            _bowHitModifierByArmor += i;
        }

        private int _bowDmgModifierByArmor = 0; // 防具による弓のダメージ補正

        public virtual int BowDmgModifierByArmor
        {
            get
            {
                return _bowDmgModifierByArmor;
            }
        }

        public virtual void addBowDmgModifierByArmor(int i)
        {
            _bowDmgModifierByArmor += i;
        }

        private bool _gresValid; // G-RESが有効か

        public bool GresValid
        {
            set
            {
                _gresValid = value;
            }
            get
            {
                return _gresValid;
            }
        }


        private long _fishingTime = 0;

        public virtual long FishingTime
        {
            get
            {
                return _fishingTime;
            }
            set
            {
                _fishingTime = value;
            }
        }


        private bool _isFishing = false;

        public virtual bool Fishing
        {
            get
            {
                return _isFishing;
            }
            set
            {
                _isFishing = value;
            }
        }


        private bool _isFishingReady = false;

        public virtual bool FishingReady
        {
            get
            {
                return _isFishingReady;
            }
            set
            {
                _isFishingReady = value;
            }
        }


        private int _cookingId = 0;

        public virtual int CookingId
        {
            get
            {
                return _cookingId;
            }
            set
            {
                _cookingId = value;
            }
        }


        private int _dessertId = 0;

        public virtual int DessertId
        {
            get
            {
                return _dessertId;
            }
            set
            {
                _dessertId = value;
            }
        }


        /// <summary>
        /// LVによる命中ボーナスを設定する LVが変動した場合などに呼び出せば再計算される
        /// 
        /// @return
        /// </summary>
        public virtual void resetBaseDmgup()
        {
            int newBaseDmgup = 0;
            int newBaseBowDmgup = 0;
            if (Knight || Darkelf || DragonKnight)
            { // ナイト、ダークエルフ、ドラゴンナイト
                newBaseDmgup = Level / 10;
                newBaseBowDmgup = 0;
            }
            else if (Elf)
            { // エルフ
                newBaseDmgup = 0;
                newBaseBowDmgup = Level / 10;
            }
            addDmgup(newBaseDmgup - _baseDmgup);
            addBowDmgup(newBaseBowDmgup - _baseBowDmgup);
            _baseDmgup = newBaseDmgup;
            _baseBowDmgup = newBaseBowDmgup;
        }

        /// <summary>
        /// LVによる命中ボーナスを設定する LVが変動した場合などに呼び出せば再計算される
        /// 
        /// @return
        /// </summary>
        public virtual void resetBaseHitup()
        {
            int newBaseHitup = 0;
            int newBaseBowHitup = 0;
            if (Crown)
            { // プリ
                newBaseHitup = Level / 5;
                newBaseBowHitup = Level / 5;
            }
            else if (Knight)
            { // ナイト
                newBaseHitup = Level / 3;
                newBaseBowHitup = Level / 3;
            }
            else if (Elf)
            { // エルフ
                newBaseHitup = Level / 5;
                newBaseBowHitup = Level / 5;
            }
            else if (Darkelf)
            { // ダークエルフ
                newBaseHitup = Level / 3;
                newBaseBowHitup = Level / 3;
            }
            else if (DragonKnight)
            { // ドラゴンナイト
                newBaseHitup = Level / 3;
                newBaseBowHitup = Level / 3;
            }
            else if (Illusionist)
            { // イリュージョニスト
                newBaseHitup = Level / 5;
                newBaseBowHitup = Level / 5;
            }
            addHitup(newBaseHitup - _baseHitup);
            addBowHitup(newBaseBowHitup - _baseBowHitup);
            _baseHitup = newBaseHitup;
            _baseBowHitup = newBaseBowHitup;
        }

        /// <summary>
        /// キャラクターステータスからACを再計算して設定する 初期設定時、LVUP,LVDown時などに呼び出す
        /// </summary>
        public virtual void resetBaseAc()
        {
            int newAc = CalcStat.calcAc(Level, BaseDex);
            addAc(newAc - _baseAc);
            _baseAc = newAc;
        }

        /// <summary>
        /// キャラクターステータスから素のMRを再計算して設定する 初期設定時、スキル使用時やLVUP,LVDown時に呼び出す
        /// </summary>
        public virtual void resetBaseMr()
        {
            int newMr = 0;
            if (Crown)
            { // プリ
                newMr = 10;
            }
            else if (Elf)
            { // エルフ
                newMr = 25;
            }
            else if (Wizard)
            { // ウィザード
                newMr = 15;
            }
            else if (Darkelf)
            { // ダークエルフ
                newMr = 10;
            }
            else if (DragonKnight)
            { // ドラゴンナイト
                newMr = 18;
            }
            else if (Illusionist)
            { // イリュージョニスト
                newMr = 20;
            }
            newMr += CalcStat.calcStatMr(getWis()); // WIS分のMRボーナス
            newMr += Level / 2; // LVの半分だけ追加
            addMr(newMr - _baseMr);
            _baseMr = newMr;
        }

        /// <summary>
        /// EXPから現在のLvを再計算して設定する 初期設定時、死亡時やLVUP時に呼び出す
        /// </summary>
        public virtual void resetLevel()
        {
            Level = ExpTable.getLevelByExp(_exp);

            if (_hpRegen != null)
            {
                _hpRegen.updateLevel();
            }
        }

        /// <summary>
        /// 初期ステータスから現在のボーナスを再計算して設定する 初期設定時、再配分時に呼び出す
        /// </summary>
        public virtual void resetOriginalHpup()
        {
            int originalCon = OriginalCon;
            if (Crown)
            {
                if ((originalCon == 12) || (originalCon == 13))
                {
                    _originalHpup = 1;
                }
                else if ((originalCon == 14) || (originalCon == 15))
                {
                    _originalHpup = 2;
                }
                else if (originalCon >= 16)
                {
                    _originalHpup = 3;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
            else if (Knight)
            {
                if ((originalCon == 15) || (originalCon == 16))
                {
                    _originalHpup = 1;
                }
                else if (originalCon >= 17)
                {
                    _originalHpup = 3;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
            else if (Elf)
            {
                if ((originalCon >= 13) && (originalCon <= 17))
                {
                    _originalHpup = 1;
                }
                else if (originalCon == 18)
                {
                    _originalHpup = 2;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalCon == 10) || (originalCon == 11))
                {
                    _originalHpup = 1;
                }
                else if (originalCon >= 12)
                {
                    _originalHpup = 2;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalCon == 14) || (originalCon == 15))
                {
                    _originalHpup = 1;
                }
                else if (originalCon >= 16)
                {
                    _originalHpup = 2;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalCon == 15) || (originalCon == 16))
                {
                    _originalHpup = 1;
                }
                else if (originalCon >= 17)
                {
                    _originalHpup = 3;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalCon == 13) || (originalCon == 14))
                {
                    _originalHpup = 1;
                }
                else if (originalCon >= 15)
                {
                    _originalHpup = 2;
                }
                else
                {
                    _originalHpup = 0;
                }
            }
        }

        public virtual void resetOriginalMpup()
        {
            int originalWis = OriginalWis;
            {
                if (Crown)
                {
                    if (originalWis >= 16)
                    {
                        _originalMpup = 1;
                    }
                    else
                    {
                        _originalMpup = 0;
                    }
                }
                else if (Knight)
                {
                    _originalMpup = 0;
                }
                else if (Elf)
                {
                    if ((originalWis >= 14) && (originalWis <= 16))
                    {
                        _originalMpup = 1;
                    }
                    else if (originalWis >= 17)
                    {
                        _originalMpup = 2;
                    }
                    else
                    {
                        _originalMpup = 0;
                    }
                }
                else if (Darkelf)
                {
                    if (originalWis >= 12)
                    {
                        _originalMpup = 1;
                    }
                    else
                    {
                        _originalMpup = 0;
                    }
                }
                else if (Wizard)
                {
                    if ((originalWis >= 13) && (originalWis <= 16))
                    {
                        _originalMpup = 1;
                    }
                    else if (originalWis >= 17)
                    {
                        _originalMpup = 2;
                    }
                    else
                    {
                        _originalMpup = 0;
                    }
                }
                else if (DragonKnight)
                {
                    if ((originalWis >= 13) && (originalWis <= 15))
                    {
                        _originalMpup = 1;
                    }
                    else if (originalWis >= 16)
                    {
                        _originalMpup = 2;
                    }
                    else
                    {
                        _originalMpup = 0;
                    }
                }
                else if (Illusionist)
                {
                    if ((originalWis >= 13) && (originalWis <= 15))
                    {
                        _originalMpup = 1;
                    }
                    else if (originalWis >= 16)
                    {
                        _originalMpup = 2;
                    }
                    else
                    {
                        _originalMpup = 0;
                    }
                }
            }
        }

        public virtual void resetOriginalStrWeightReduction()
        {
            int originalStr = OriginalStr;
            if (Crown)
            {
                if ((originalStr >= 14) && (originalStr <= 16))
                {
                    _originalStrWeightReduction = 1;
                }
                else if ((originalStr >= 17) && (originalStr <= 19))
                {
                    _originalStrWeightReduction = 2;
                }
                else if (originalStr == 20)
                {
                    _originalStrWeightReduction = 3;
                }
                else
                {
                    _originalStrWeightReduction = 0;
                }
            }
            else if (Knight)
            {
                _originalStrWeightReduction = 0;
            }
            else if (Elf)
            {
                if (originalStr >= 16)
                {
                    _originalStrWeightReduction = 2;
                }
                else
                {
                    _originalStrWeightReduction = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalStr >= 13) && (originalStr <= 15))
                {
                    _originalStrWeightReduction = 2;
                }
                else if (originalStr >= 16)
                {
                    _originalStrWeightReduction = 3;
                }
                else
                {
                    _originalStrWeightReduction = 0;
                }
            }
            else if (Wizard)
            {
                if (originalStr >= 9)
                {
                    _originalStrWeightReduction = 1;
                }
                else
                {
                    _originalStrWeightReduction = 0;
                }
            }
            else if (DragonKnight)
            {
                if (originalStr >= 16)
                {
                    _originalStrWeightReduction = 1;
                }
                else
                {
                    _originalStrWeightReduction = 0;
                }
            }
            else if (Illusionist)
            {
                if (originalStr == 18)
                {
                    _originalStrWeightReduction = 1;
                }
                else
                {
                    _originalStrWeightReduction = 0;
                }
            }
        }

        public virtual void resetOriginalDmgup()
        {
            int originalStr = OriginalStr;
            if (Crown)
            {
                if ((originalStr >= 15) && (originalStr <= 17))
                {
                    _originalDmgup = 1;
                }
                else if (originalStr >= 18)
                {
                    _originalDmgup = 2;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
            else if (Knight)
            {
                if ((originalStr == 18) || (originalStr == 19))
                {
                    _originalDmgup = 2;
                }
                else if (originalStr == 20)
                {
                    _originalDmgup = 4;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
            else if (Elf)
            {
                if ((originalStr == 12) || (originalStr == 13))
                {
                    _originalDmgup = 1;
                }
                else if (originalStr >= 14)
                {
                    _originalDmgup = 2;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalStr >= 14) && (originalStr <= 17))
                {
                    _originalDmgup = 1;
                }
                else if (originalStr == 18)
                {
                    _originalDmgup = 2;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalStr == 10) || (originalStr == 11))
                {
                    _originalDmgup = 1;
                }
                else if (originalStr >= 12)
                {
                    _originalDmgup = 2;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalStr >= 15) && (originalStr <= 17))
                {
                    _originalDmgup = 1;
                }
                else if (originalStr >= 18)
                {
                    _originalDmgup = 3;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalStr == 13) || (originalStr == 14))
                {
                    _originalDmgup = 1;
                }
                else if (originalStr >= 15)
                {
                    _originalDmgup = 2;
                }
                else
                {
                    _originalDmgup = 0;
                }
            }
        }

        public virtual void resetOriginalConWeightReduction()
        {
            int originalCon = OriginalCon;
            if (Crown)
            {
                if (originalCon >= 11)
                {
                    _originalConWeightReduction = 1;
                }
                else
                {
                    _originalConWeightReduction = 0;
                }
            }
            else if (Knight)
            {
                if (originalCon >= 15)
                {
                    _originalConWeightReduction = 1;
                }
                else
                {
                    _originalConWeightReduction = 0;
                }
            }
            else if (Elf)
            {
                if (originalCon >= 15)
                {
                    _originalConWeightReduction = 2;
                }
                else
                {
                    _originalConWeightReduction = 0;
                }
            }
            else if (Darkelf)
            {
                if (originalCon >= 9)
                {
                    _originalConWeightReduction = 1;
                }
                else
                {
                    _originalConWeightReduction = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalCon == 13) || (originalCon == 14))
                {
                    _originalConWeightReduction = 1;
                }
                else if (originalCon >= 15)
                {
                    _originalConWeightReduction = 2;
                }
                else
                {
                    _originalConWeightReduction = 0;
                }
            }
            else if (DragonKnight)
            {
                _originalConWeightReduction = 0;
            }
            else if (Illusionist)
            {
                if (originalCon == 17)
                {
                    _originalConWeightReduction = 1;
                }
                else if (originalCon == 18)
                {
                    _originalConWeightReduction = 2;
                }
                else
                {
                    _originalConWeightReduction = 0;
                }
            }
        }

        public virtual void resetOriginalBowDmgup()
        {
            int originalDex = OriginalDex;
            if (Crown)
            {
                if (originalDex >= 13)
                {
                    _originalBowDmgup = 1;
                }
                else
                {
                    _originalBowDmgup = 0;
                }
            }
            else if (Knight)
            {
                _originalBowDmgup = 0;
            }
            else if (Elf)
            {
                if ((originalDex >= 14) && (originalDex <= 16))
                {
                    _originalBowDmgup = 2;
                }
                else if (originalDex >= 17)
                {
                    _originalBowDmgup = 3;
                }
                else
                {
                    _originalBowDmgup = 0;
                }
            }
            else if (Darkelf)
            {
                if (originalDex == 18)
                {
                    _originalBowDmgup = 2;
                }
                else
                {
                    _originalBowDmgup = 0;
                }
            }
            else if (Wizard)
            {
                _originalBowDmgup = 0;
            }
            else if (DragonKnight)
            {
                _originalBowDmgup = 0;
            }
            else if (Illusionist)
            {
                _originalBowDmgup = 0;
            }
        }

        public virtual void resetOriginalHitup()
        {
            int originalStr = OriginalStr;
            if (Crown)
            {
                if ((originalStr >= 16) && (originalStr <= 18))
                {
                    _originalHitup = 1;
                }
                else if (originalStr >= 19)
                {
                    _originalHitup = 2;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
            else if (Knight)
            {
                if ((originalStr == 17) || (originalStr == 18))
                {
                    _originalHitup = 2;
                }
                else if (originalStr >= 19)
                {
                    _originalHitup = 4;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
            else if (Elf)
            {
                if ((originalStr == 13) || (originalStr == 14))
                {
                    _originalHitup = 1;
                }
                else if (originalStr >= 15)
                {
                    _originalHitup = 2;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalStr >= 15) && (originalStr <= 17))
                {
                    _originalHitup = 1;
                }
                else if (originalStr == 18)
                {
                    _originalHitup = 2;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalStr == 11) || (originalStr == 12))
                {
                    _originalHitup = 1;
                }
                else if (originalStr >= 13)
                {
                    _originalHitup = 2;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalStr >= 14) && (originalStr <= 16))
                {
                    _originalHitup = 1;
                }
                else if (originalStr >= 17)
                {
                    _originalHitup = 3;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalStr == 12) || (originalStr == 13))
                {
                    _originalHitup = 1;
                }
                else if ((originalStr == 14) || (originalStr == 15))
                {
                    _originalHitup = 2;
                }
                else if (originalStr == 16)
                {
                    _originalHitup = 3;
                }
                else if (originalStr >= 17)
                {
                    _originalHitup = 4;
                }
                else
                {
                    _originalHitup = 0;
                }
            }
        }

        public virtual void resetOriginalBowHitup()
        {
            int originalDex = OriginalDex;
            if (Crown)
            {
                _originalBowHitup = 0;
            }
            else if (Knight)
            {
                _originalBowHitup = 0;
            }
            else if (Elf)
            {
                if ((originalDex >= 13) && (originalDex <= 15))
                {
                    _originalBowHitup = 2;
                }
                else if (originalDex >= 16)
                {
                    _originalBowHitup = 3;
                }
                else
                {
                    _originalBowHitup = 0;
                }
            }
            else if (Darkelf)
            {
                if (originalDex == 17)
                {
                    _originalBowHitup = 1;
                }
                else if (originalDex == 18)
                {
                    _originalBowHitup = 2;
                }
                else
                {
                    _originalBowHitup = 0;
                }
            }
            else if (Wizard)
            {
                _originalBowHitup = 0;
            }
            else if (DragonKnight)
            {
                _originalBowHitup = 0;
            }
            else if (Illusionist)
            {
                _originalBowHitup = 0;
            }
        }

        public virtual void resetOriginalMr()
        {
            int originalWis = OriginalWis;
            if (Crown)
            {
                if ((originalWis == 12) || (originalWis == 13))
                {
                    _originalMr = 1;
                }
                else if (originalWis >= 14)
                {
                    _originalMr = 2;
                }
                else
                {
                    _originalMr = 0;
                }
            }
            else if (Knight)
            {
                if ((originalWis == 10) || (originalWis == 11))
                {
                    _originalMr = 1;
                }
                else if (originalWis >= 12)
                {
                    _originalMr = 2;
                }
                else
                {
                    _originalMr = 0;
                }
            }
            else if (Elf)
            {
                if ((originalWis >= 13) && (originalWis <= 15))
                {
                    _originalMr = 1;
                }
                else if (originalWis >= 16)
                {
                    _originalMr = 2;
                }
                else
                {
                    _originalMr = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalWis >= 11) && (originalWis <= 13))
                {
                    _originalMr = 1;
                }
                else if (originalWis == 14)
                {
                    _originalMr = 2;
                }
                else if (originalWis == 15)
                {
                    _originalMr = 3;
                }
                else if (originalWis >= 16)
                {
                    _originalMr = 4;
                }
                else
                {
                    _originalMr = 0;
                }
            }
            else if (Wizard)
            {
                if (originalWis >= 15)
                {
                    _originalMr = 1;
                }
                else
                {
                    _originalMr = 0;
                }
            }
            else if (DragonKnight)
            {
                if (originalWis >= 14)
                {
                    _originalMr = 2;
                }
                else
                {
                    _originalMr = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalWis >= 15) && (originalWis <= 17))
                {
                    _originalMr = 2;
                }
                else if (originalWis == 18)
                {
                    _originalMr = 4;
                }
                else
                {
                    _originalMr = 0;
                }
            }

            addMr(_originalMr);
        }

        public virtual void resetOriginalMagicHit()
        {
            int originalInt = OriginalInt;
            if (Crown)
            {
                if ((originalInt == 12) || (originalInt == 13))
                {
                    _originalMagicHit = 1;
                }
                else if (originalInt >= 14)
                {
                    _originalMagicHit = 2;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
            else if (Knight)
            {
                if ((originalInt == 10) || (originalInt == 11))
                {
                    _originalMagicHit = 1;
                }
                else if (originalInt == 12)
                {
                    _originalMagicHit = 2;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
            else if (Elf)
            {
                if ((originalInt == 13) || (originalInt == 14))
                {
                    _originalMagicHit = 1;
                }
                else if (originalInt >= 15)
                {
                    _originalMagicHit = 2;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalInt == 12) || (originalInt == 13))
                {
                    _originalMagicHit = 1;
                }
                else if (originalInt >= 14)
                {
                    _originalMagicHit = 2;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
            else if (Wizard)
            {
                if (originalInt >= 14)
                {
                    _originalMagicHit = 1;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalInt == 12) || (originalInt == 13))
                {
                    _originalMagicHit = 2;
                }
                else if ((originalInt == 14) || (originalInt == 15))
                {
                    _originalMagicHit = 3;
                }
                else if (originalInt >= 16)
                {
                    _originalMagicHit = 4;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
            else if (Illusionist)
            {
                if (originalInt >= 13)
                {
                    _originalMagicHit = 1;
                }
                else
                {
                    _originalMagicHit = 0;
                }
            }
        }

        public virtual void resetOriginalMagicCritical()
        {
            int originalInt = OriginalInt;
            if (Crown)
            {
                _originalMagicCritical = 0;
            }
            else if (Knight)
            {
                _originalMagicCritical = 0;
            }
            else if (Elf)
            {
                if ((originalInt == 14) || (originalInt == 15))
                {
                    _originalMagicCritical = 2;
                }
                else if (originalInt >= 16)
                {
                    _originalMagicCritical = 4;
                }
                else
                {
                    _originalMagicCritical = 0;
                }
            }
            else if (Darkelf)
            {
                _originalMagicCritical = 0;
            }
            else if (Wizard)
            {
                if (originalInt == 15)
                {
                    _originalMagicCritical = 2;
                }
                else if (originalInt == 16)
                {
                    _originalMagicCritical = 4;
                }
                else if (originalInt == 17)
                {
                    _originalMagicCritical = 6;
                }
                else if (originalInt == 18)
                {
                    _originalMagicCritical = 8;
                }
                else
                {
                    _originalMagicCritical = 0;
                }
            }
            else if (DragonKnight)
            {
                _originalMagicCritical = 0;
            }
            else if (Illusionist)
            {
                _originalMagicCritical = 0;
            }
        }

        public virtual void resetOriginalMagicConsumeReduction()
        {
            int originalInt = OriginalInt;
            if (Crown)
            {
                if ((originalInt == 11) || (originalInt == 12))
                {
                    _originalMagicConsumeReduction = 1;
                }
                else if (originalInt >= 13)
                {
                    _originalMagicConsumeReduction = 2;
                }
                else
                {
                    _originalMagicConsumeReduction = 0;
                }
            }
            else if (Knight)
            {
                if ((originalInt == 9) || (originalInt == 10))
                {
                    _originalMagicConsumeReduction = 1;
                }
                else if (originalInt >= 11)
                {
                    _originalMagicConsumeReduction = 2;
                }
                else
                {
                    _originalMagicConsumeReduction = 0;
                }
            }
            else if (Elf)
            {
                _originalMagicConsumeReduction = 0;
            }
            else if (Darkelf)
            {
                if ((originalInt == 13) || (originalInt == 14))
                {
                    _originalMagicConsumeReduction = 1;
                }
                else if (originalInt >= 15)
                {
                    _originalMagicConsumeReduction = 2;
                }
                else
                {
                    _originalMagicConsumeReduction = 0;
                }
            }
            else if (Wizard)
            {
                _originalMagicConsumeReduction = 0;
            }
            else if (DragonKnight)
            {
                _originalMagicConsumeReduction = 0;
            }
            else if (Illusionist)
            {
                if (originalInt == 14)
                {
                    _originalMagicConsumeReduction = 1;
                }
                else if (originalInt >= 15)
                {
                    _originalMagicConsumeReduction = 2;
                }
                else
                {
                    _originalMagicConsumeReduction = 0;
                }
            }
        }

        public virtual void resetOriginalMagicDamage()
        {
            int originalInt = OriginalInt;
            if (Crown)
            {
                _originalMagicDamage = 0;
            }
            else if (Knight)
            {
                _originalMagicDamage = 0;
            }
            else if (Elf)
            {
                _originalMagicDamage = 0;
            }
            else if (Darkelf)
            {
                _originalMagicDamage = 0;
            }
            else if (Wizard)
            {
                if (originalInt >= 13)
                {
                    _originalMagicDamage = 1;
                }
                else
                {
                    _originalMagicDamage = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalInt == 13) || (originalInt == 14))
                {
                    _originalMagicDamage = 1;
                }
                else if ((originalInt == 15) || (originalInt == 16))
                {
                    _originalMagicDamage = 2;
                }
                else if (originalInt == 17)
                {
                    _originalMagicDamage = 3;
                }
                else
                {
                    _originalMagicDamage = 0;
                }
            }
            else if (Illusionist)
            {
                if (originalInt == 16)
                {
                    _originalMagicDamage = 1;
                }
                else if (originalInt == 17)
                {
                    _originalMagicDamage = 2;
                }
                else
                {
                    _originalMagicDamage = 0;
                }
            }
        }

        public virtual void resetOriginalAc()
        {
            int originalDex = OriginalDex;
            if (Crown)
            {
                if ((originalDex >= 12) && (originalDex <= 14))
                {
                    _originalAc = 1;
                }
                else if ((originalDex == 15) || (originalDex == 16))
                {
                    _originalAc = 2;
                }
                else if (originalDex >= 17)
                {
                    _originalAc = 3;
                }
                else
                {
                    _originalAc = 0;
                }
            }
            else if (Knight)
            {
                if ((originalDex == 13) || (originalDex == 14))
                {
                    _originalAc = 1;
                }
                else if (originalDex >= 15)
                {
                    _originalAc = 3;
                }
                else
                {
                    _originalAc = 0;
                }
            }
            else if (Elf)
            {
                if ((originalDex >= 15) && (originalDex <= 17))
                {
                    _originalAc = 1;
                }
                else if (originalDex == 18)
                {
                    _originalAc = 2;
                }
                else
                {
                    _originalAc = 0;
                }
            }
            else if (Darkelf)
            {
                if (originalDex >= 17)
                {
                    _originalAc = 1;
                }
                else
                {
                    _originalAc = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalDex == 8) || (originalDex == 9))
                {
                    _originalAc = 1;
                }
                else if (originalDex >= 10)
                {
                    _originalAc = 2;
                }
                else
                {
                    _originalAc = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalDex == 12) || (originalDex == 13))
                {
                    _originalAc = 1;
                }
                else if (originalDex >= 14)
                {
                    _originalAc = 2;
                }
                else
                {
                    _originalAc = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalDex == 11) || (originalDex == 12))
                {
                    _originalAc = 1;
                }
                else if (originalDex >= 13)
                {
                    _originalAc = 2;
                }
                else
                {
                    _originalAc = 0;
                }
            }

            addAc(0 - _originalAc);
        }

        public virtual void resetOriginalEr()
        {
            int originalDex = OriginalDex;
            if (Crown)
            {
                if ((originalDex == 14) || (originalDex == 15))
                {
                    _originalEr = 1;
                }
                else if ((originalDex == 16) || (originalDex == 17))
                {
                    _originalEr = 2;
                }
                else if (originalDex == 18)
                {
                    _originalEr = 3;
                }
                else
                {
                    _originalEr = 0;
                }
            }
            else if (Knight)
            {
                if ((originalDex == 14) || (originalDex == 15))
                {
                    _originalEr = 1;
                }
                else if (originalDex == 16)
                {
                    _originalEr = 3;
                }
                else
                {
                    _originalEr = 0;
                }
            }
            else if (Elf)
            {
                _originalEr = 0;
            }
            else if (Darkelf)
            {
                if (originalDex >= 16)
                {
                    _originalEr = 2;
                }
                else
                {
                    _originalEr = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalDex == 9) || (originalDex == 10))
                {
                    _originalEr = 1;
                }
                else if (originalDex == 11)
                {
                    _originalEr = 2;
                }
                else
                {
                    _originalEr = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalDex == 13) || (originalDex == 14))
                {
                    _originalEr = 1;
                }
                else if (originalDex >= 15)
                {
                    _originalEr = 2;
                }
                else
                {
                    _originalEr = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalDex == 12) || (originalDex == 13))
                {
                    _originalEr = 1;
                }
                else if (originalDex >= 14)
                {
                    _originalEr = 2;
                }
                else
                {
                    _originalEr = 0;
                }
            }
        }

        public virtual void resetOriginalHpr()
        {
            int originalCon = OriginalCon;
            if (Crown)
            {
                if ((originalCon == 13) || (originalCon == 14))
                {
                    _originalHpr = 1;
                }
                else if ((originalCon == 15) || (originalCon == 16))
                {
                    _originalHpr = 2;
                }
                else if (originalCon == 17)
                {
                    _originalHpr = 3;
                }
                else if (originalCon == 18)
                {
                    _originalHpr = 4;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
            else if (Knight)
            {
                if ((originalCon == 16) || (originalCon == 17))
                {
                    _originalHpr = 2;
                }
                else if (originalCon == 18)
                {
                    _originalHpr = 4;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
            else if (Elf)
            {
                if ((originalCon == 14) || (originalCon == 15))
                {
                    _originalHpr = 1;
                }
                else if (originalCon == 16)
                {
                    _originalHpr = 2;
                }
                else if (originalCon >= 17)
                {
                    _originalHpr = 3;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
            else if (Darkelf)
            {
                if ((originalCon == 11) || (originalCon == 12))
                {
                    _originalHpr = 1;
                }
                else if (originalCon >= 13)
                {
                    _originalHpr = 2;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
            else if (Wizard)
            {
                if (originalCon == 17)
                {
                    _originalHpr = 1;
                }
                else if (originalCon == 18)
                {
                    _originalHpr = 2;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalCon == 16) || (originalCon == 17))
                {
                    _originalHpr = 1;
                }
                else if (originalCon == 18)
                {
                    _originalHpr = 3;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalCon == 14) || (originalCon == 15))
                {
                    _originalHpr = 1;
                }
                else if (originalCon >= 16)
                {
                    _originalHpr = 2;
                }
                else
                {
                    _originalHpr = 0;
                }
            }
        }

        public virtual void resetOriginalMpr()
        {
            int originalWis = OriginalWis;
            if (Crown)
            {
                if ((originalWis == 13) || (originalWis == 14))
                {
                    _originalMpr = 1;
                }
                else if (originalWis >= 15)
                {
                    _originalMpr = 2;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
            else if (Knight)
            {
                if ((originalWis == 11) || (originalWis == 12))
                {
                    _originalMpr = 1;
                }
                else if (originalWis == 13)
                {
                    _originalMpr = 2;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
            else if (Elf)
            {
                if ((originalWis >= 15) && (originalWis <= 17))
                {
                    _originalMpr = 1;
                }
                else if (originalWis == 18)
                {
                    _originalMpr = 2;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
            else if (Darkelf)
            {
                if (originalWis >= 13)
                {
                    _originalMpr = 1;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
            else if (Wizard)
            {
                if ((originalWis == 14) || (originalWis == 15))
                {
                    _originalMpr = 1;
                }
                else if ((originalWis == 16) || (originalWis == 17))
                {
                    _originalMpr = 2;
                }
                else if (originalWis == 18)
                {
                    _originalMpr = 3;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
            else if (DragonKnight)
            {
                if ((originalWis == 15) || (originalWis == 16))
                {
                    _originalMpr = 1;
                }
                else if (originalWis >= 17)
                {
                    _originalMpr = 2;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
            else if (Illusionist)
            {
                if ((originalWis >= 14) && (originalWis <= 16))
                {
                    _originalMpr = 1;
                }
                else if (originalWis >= 17)
                {
                    _originalMpr = 2;
                }
                else
                {
                    _originalMpr = 0;
                }
            }
        }

        public virtual void refresh()
        {
            resetLevel();
            resetBaseHitup();
            resetBaseDmgup();
            resetBaseMr();
            resetBaseAc();
            resetOriginalHpup();
            resetOriginalMpup();
            resetOriginalDmgup();
            resetOriginalBowDmgup();
            resetOriginalHitup();
            resetOriginalBowHitup();
            resetOriginalMr();
            resetOriginalMagicHit();
            resetOriginalMagicCritical();
            resetOriginalMagicConsumeReduction();
            resetOriginalMagicDamage();
            resetOriginalAc();
            resetOriginalEr();
            resetOriginalHpr();
            resetOriginalMpr();
            resetOriginalStrWeightReduction();
            resetOriginalConWeightReduction();
        }

        public virtual void startRefreshParty()
        { // 組隊更新 3.3C

            const int INTERVAL = 25000;

            if (!_rpActive)
            {

                _rp = new L1PartyRefresh(this);

                _regenTimer.execute(_rp, INTERVAL, INTERVAL);

                _rpActive = true;

            }

        }

        public virtual void stopRefreshParty()
        { // 組隊暫停更新 3.3C

            if (_rpActive)
            {
                _rp.cancel();
                _rp = null;
                _rpActive = false;

            }
        }

        private readonly L1ExcludingList _excludingList = new L1ExcludingList();

        public virtual L1ExcludingList ExcludingList
        {
            get
            {
                return _excludingList;
            }
        }

        // -- 加速器検知機能 --
        private AcceleratorChecker _acceleratorChecker;

        public virtual AcceleratorChecker AcceleratorChecker
        {
            get
            {
                return _acceleratorChecker;
            }
        }

        // 使用屠宰者判斷
        private bool _FoeSlayer = false;

        public virtual bool FoeSlayer
        {
            set
            {
                _FoeSlayer = value;
            }
            get
            {
                return _FoeSlayer;
            }
        }


        /// <summary>
        /// テレポート先の座標
        /// </summary>
        private int _teleportX = 0;

        public virtual int TeleportX
        {
            get
            {
                return _teleportX;
            }
            set
            {
                _teleportX = value;
            }
        }


        private int _teleportY = 0;

        public virtual int TeleportY
        {
            get
            {
                return _teleportY;
            }
            set
            {
                _teleportY = value;
            }
        }


        private short _teleportMapId = 0;

        public virtual short TeleportMapId
        {
            get
            {
                return _teleportMapId;
            }
            set
            {
                _teleportMapId = value;
            }
        }


        private int _teleportHeading = 0;

        public virtual int TeleportHeading
        {
            get
            {
                return _teleportHeading;
            }
            set
            {
                _teleportHeading = value;
            }
        }


        private int _tempCharGfxAtDead;

        public virtual int TempCharGfxAtDead
        {
            get
            {
                return _tempCharGfxAtDead;
            }
            set
            {
                _tempCharGfxAtDead = value;
            }
        }


        private bool _isCanWhisper = true;

        public virtual bool CanWhisper
        {
            get
            {
                return _isCanWhisper;
            }
            set
            {
                _isCanWhisper = value;
            }
        }


        private bool _isShowTradeChat = true;

        public virtual bool ShowTradeChat
        {
            get
            {
                return _isShowTradeChat;
            }
            set
            {
                _isShowTradeChat = value;
            }
        }


        // 血盟
        private bool _isShowClanChat = true;

        public virtual bool ShowClanChat
        {
            get
            {
                return _isShowClanChat;
            }
            set
            {
                _isShowClanChat = value;
            }
        }


        // 組隊
        private bool _isShowPartyChat = true;

        public virtual bool ShowPartyChat
        {
            get
            {
                return _isShowPartyChat;
            }
            set
            {
                _isShowPartyChat = value;
            }
        }


        private bool _isShowWorldChat = true;

        public virtual bool ShowWorldChat
        {
            get
            {
                return _isShowWorldChat;
            }
            set
            {
                _isShowWorldChat = value;
            }
        }


        private int _fightId;

        public virtual int FightId
        {
            get
            {
                return _fightId;
            }
            set
            {
                _fightId = value;
            }
        }


        // 釣魚點
        private int _fishX;

        public virtual int FishX
        {
            get
            {
                return _fishX;
            }
            set
            {
                _fishX = value;
            }
        }

        private int _fishY;

        public virtual int FishY
        {
            get
            {
                return _fishY;
            }
            set
            {
                _fishY = value;
            }
        }


        private byte _chatCount = 0;

        private long _oldChatTimeInMillis = 0L;

        public virtual void checkChatInterval()
        {
            long nowChatTimeInMillis = DateTime.Now.Ticks;
            if (_chatCount == 0)
            {
                _chatCount++;
                _oldChatTimeInMillis = nowChatTimeInMillis;
                return;
            }

            long chatInterval = nowChatTimeInMillis - _oldChatTimeInMillis;
            if (chatInterval > 2000)
            {
                _chatCount = 0;
                _oldChatTimeInMillis = 0;
            }
            else
            {
                if (_chatCount >= 3)
                {
                    setSkillEffect(L1SkillId.STATUS_CHAT_PROHIBITED, 120 * 1000);
                    sendPackets(new S_SkillIconGFX(36, 120));
                    sendPackets(new S_ServerMessage(153)); // \f3迷惑なチャット流しをしたので、今後2分間チャットを行うことはできません。
                    _chatCount = 0;
                    _oldChatTimeInMillis = 0;
                }
                _chatCount++;
            }
        }

        private int _callClanId;

        public virtual int CallClanId
        {
            get
            {
                return _callClanId;
            }
            set
            {
                _callClanId = value;
            }
        }


        private int _callClanHeading;

        public virtual int CallClanHeading
        {
            get
            {
                return _callClanHeading;
            }
            set
            {
                _callClanHeading = value;
            }
        }


        private bool _isInCharReset = false;

        public virtual bool InCharReset
        {
            get
            {
                return _isInCharReset;
            }
            set
            {
                _isInCharReset = value;
            }
        }


        private int _tempLevel = 1;

        public virtual int TempLevel
        {
            get
            {
                return _tempLevel;
            }
            set
            {
                _tempLevel = value;
            }
        }


        private int _tempMaxLevel = 1;

        public virtual int TempMaxLevel
        {
            get
            {
                return _tempMaxLevel;
            }
            set
            {
                _tempMaxLevel = value;
            }
        }


        private int _awakeSkillId = 0;

        public virtual int AwakeSkillId
        {
            get
            {
                return _awakeSkillId;
            }
            set
            {
                _awakeSkillId = value;
            }
        }


        private bool _isSummonMonster = false;

        public virtual bool SummonMonster
        {
            set
            {
                _isSummonMonster = value;
            }
            get
            {
                return _isSummonMonster;
            }
        }


        private int _SummonId = 0;

        public virtual int SummonId
        {
            set
            {
                _SummonId = value;
            }
            get
            {
                return _SummonId;
            }
        }


        private bool _isShapeChange = false;

        public virtual bool ShapeChange
        {
            set
            {
                _isShapeChange = value;
            }
            get
            {
                return _isShapeChange;
            }
        }


        public virtual int PartyType
        {
            set
            {
                _partyType = value;
            }
            get
            {
                return _partyType;
            }
        }

        public virtual void setEquipped(L1PcInstance pc, bool isEq)
        {
            foreach (L1ItemInstance item in pc._inventory.Items)
            {
                //3.63　新增裝備欄
                if ((item.Item.Type2 == 2) && (item.Equipped))
                { // 判斷是否可用裝備
                    int items = 0;
                    if ((item.Item.Type == 1))
                    {
                        items = 1;
                    }
                    else if ((item.Item.Type == 2))
                    {
                        items = 2;
                    }
                    else if ((item.Item.Type == 3))
                    {
                        items = 3;
                    }
                    else if ((item.Item.Type == 4))
                    {
                        items = 4;
                    }
                    else if ((item.Item.Type == 5))
                    {
                        items = 6;
                    }
                    else if ((item.Item.Type == 6))
                    {
                        items = 5;
                    }
                    else if ((item.Item.Type == 7))
                    {
                        items = 7;
                    }
                    else if ((item.Item.Type == 8))
                    {
                        items = 10;
                    }
                    else if ((item.Item.Type == 9) && item.RingID == 18)
                    {
                        items = 18;
                    }
                    else if ((item.Item.Type == 9) && item.RingID == 19)
                    {
                        items = 19;
                    }
                    else if ((item.Item.Type == 9) && item.RingID == 20)
                    {
                        items = 20;
                    }
                    else if ((item.Item.Type == 9) && item.RingID == 21)
                    {
                        items = 21;
                    }
                    else if ((item.Item.Type == 10))
                    {
                        items = 11;
                    }
                    else if ((item.Item.Type == 12))
                    {
                        items = 12;
                    }
                    else if ((item.Item.Type == 13))
                    {
                        items = 7;
                    }
                    else if ((item.Item.Type == 14))
                    {
                        items = 22;
                    }
                    else if ((item.Item.Type == 15))
                    {
                        items = 23;
                    }
                    else if ((item.Item.Type == 16))
                    {
                        items = 24;
                    }
                    else if ((item.Item.Type == 17))
                    {
                        items = 25;
                    }
                    else if ((item.Item.Type == 18))
                    {
                        items = 26;
                    }
                    pc.sendPackets(new S_EquipmentWindow(pc, item.Id, items, isEq));
                }

                if ((item.Item.Type2 == 1) && (item.Equipped))
                { // 判斷是否可用裝備
                    int items = 8;
                    pc.sendPackets(new S_EquipmentWindow(pc, item.Id, items, isEq));
                }
                //3.63　新增裝備欄
            }
        }
        /// <summary>
        ///**************************** 戰鬥特化系統 ***************************** </summary>
        // 改變戰鬥特化狀態
        public virtual void changeFightType(int oldType, int newType)
        {
            // 消除既有的戰鬥特化狀態
            switch (oldType)
            {
                case 1:
                    addAc(2);
                    addMr(-3);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_JUSTICE_LEVEL1, S_Fight.FLAG_OFF));
                    break;

                case 2:
                    addAc(4);
                    addMr(-6);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_JUSTICE_LEVEL2, S_Fight.FLAG_OFF));
                    break;

                case 3:
                    addAc(6);
                    addMr(-9);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_JUSTICE_LEVEL3, S_Fight.FLAG_OFF));
                    break;

                case -1:
                    addDmgup(-1);
                    addBowDmgup(-1);
                    addSp(-1);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_EVIL_LEVEL1, S_Fight.FLAG_OFF));
                    break;

                case -2:
                    addDmgup(-3);
                    addBowDmgup(-3);
                    addSp(-2);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_EVIL_LEVEL2, S_Fight.FLAG_OFF));
                    break;

                case -3:
                    addDmgup(-5);
                    addBowDmgup(-5);
                    addSp(-3);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_EVIL_LEVEL3, S_Fight.FLAG_OFF));
                    break;
            }

            // 增加新的戰鬥特化狀態
            switch (newType)
            {
                case 1:
                    addAc(-2);
                    addMr(3);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_JUSTICE_LEVEL1, S_Fight.FLAG_ON));
                    break;

                case 2:
                    addAc(-4);
                    addMr(6);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_JUSTICE_LEVEL2, S_Fight.FLAG_ON));
                    break;

                case 3:
                    addAc(-6);
                    addMr(9);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_JUSTICE_LEVEL3, S_Fight.FLAG_ON));
                    break;

                case -1:
                    addDmgup(1);
                    addBowDmgup(1);
                    addSp(1);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_EVIL_LEVEL1, S_Fight.FLAG_ON));
                    break;

                case -2:
                    addDmgup(3);
                    addBowDmgup(3);
                    addSp(2);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_EVIL_LEVEL2, S_Fight.FLAG_ON));
                    break;

                case -3:
                    addDmgup(5);
                    addBowDmgup(5);
                    addSp(3);
                    sendPackets(new S_OwnCharStatus2(this, 0)); // 更新六项能力值
                    sendPackets(new S_OwnCharAttrDef(this)); // 更新物理防显示
                    sendPackets(new S_SPMR(this)); // 更新魔防及魔攻显示
                    sendPackets(new S_Fight(S_Fight.TYPE_EVIL_LEVEL3, S_Fight.FLAG_ON));
                    break;
            }
        }

        // 確認是否屬於新手, 並設定相關狀態
        public virtual void checkNoviceType()
        {
            // 判斷是否啟動新手保護系統(遭遇的守護)
            if (!Config.NOVICE_PROTECTION_IS_ACTIVE)
            {
                return;
            }

            // 判斷目前等級是否已超過新手階段
            if (Level > Config.NOVICE_MAX_LEVEL)
            {
                // 判斷之前是否具有新手保護狀態
                if (hasSkillEffect(L1SkillId.STATUS_NOVICE))
                {
                    // 移除新手保護狀態
                    removeSkillEffect(L1SkillId.STATUS_NOVICE);

                    // 關閉遭遇的守護圖示
                    sendPackets(new S_Fight(S_Fight.TYPE_ENCOUNTER, S_Fight.FLAG_OFF));
                }
            }
            else
            {
                // 判斷是否未具有新手保護狀態
                if (!hasSkillEffect(L1SkillId.STATUS_NOVICE))
                {
                    // 增加新手保護狀態
                    setSkillEffect(L1SkillId.STATUS_NOVICE, 0);

                    // 開啟遭遇的守護圖示
                    sendPackets(new S_Fight(S_Fight.TYPE_ENCOUNTER, S_Fight.FLAG_ON));
                }
            }
        }
    }

}