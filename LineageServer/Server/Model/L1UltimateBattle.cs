using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace LineageServer.Server.Model
{
    /// <summary>
    /// 無限大戰
    /// </summary>
    class L1UltimateBattle
    {
        private int _locX;

        private int _locY;

        private L1Location _location; // 中心点

        private short _mapId;

        private int _locX1;

        private int _locY1;

        private int _locX2;

        private int _locY2;

        private int _ubId;

        private int _pattern;

        private bool _isNowUb;

        private bool _active; // UB入場可能～競技終了までtrue

        private int _minLevel;

        private int _maxLevel;

        private int _maxPlayer;

        private bool _enterRoyal;

        private bool _enterKnight;

        private bool _enterMage;

        private bool _enterElf;

        private bool _enterDarkelf;

        private bool _enterDragonKnight;

        private bool _enterIllusionist;

        private bool _enterMale;

        private bool _enterFemale;

        private bool _usePot;

        private int _hpr;

        private int _mpr;

        private static int BEFORE_MINUTE = 5; // 5分前から入場開始

        private ISet<int> _managers = new HashSet<int>();

        private SortedSet<int> _ubTimes = new SortedSet<int>();

        private static readonly ILogger _log = Logger.GetLogger(nameof(L1UltimateBattle));

        private readonly IList<L1PcInstance> _members = ListFactory.NewList<L1PcInstance>();

        /// <summary>
        /// ラウンド開始時のメッセージを送信する。
        /// </summary>
        /// <param name="curRound">
        ///            開始するラウンド </param>
        private void sendRoundMessage(int curRound)
        {
            // XXX - このIDは間違っている
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int MSGID_ROUND_TABLE[] = { 893, 894, 895, 896 };
            int[] MSGID_ROUND_TABLE = new int[] { 893, 894, 895, 896 };

            sendMessage(MSGID_ROUND_TABLE[curRound - 1], "");
        }

        /// <summary>
        /// ポーション等の補給アイテムを出現させる。
        /// </summary>
        /// <param name="curRound">
        ///            現在のラウンド </param>
        private void spawnSupplies(int curRound)
        {
            if (curRound == 1)
            {
                spawnGroundItem(L1ItemId.ADENA, 1000, 60);
                spawnGroundItem(L1ItemId.POTION_OF_CURE_POISON, 3, 20);
                spawnGroundItem(L1ItemId.POTION_OF_EXTRA_HEALING, 5, 20);
                spawnGroundItem(L1ItemId.POTION_OF_GREATER_HEALING, 3, 20);
                spawnGroundItem(40317, 1, 5); // 砥石
                spawnGroundItem(40079, 1, 20); // 帰還スク
            }
            else if (curRound == 2)
            {
                spawnGroundItem(L1ItemId.ADENA, 5000, 50);
                spawnGroundItem(L1ItemId.POTION_OF_CURE_POISON, 5, 20);
                spawnGroundItem(L1ItemId.POTION_OF_EXTRA_HEALING, 10, 20);
                spawnGroundItem(L1ItemId.POTION_OF_GREATER_HEALING, 5, 20);
                spawnGroundItem(40317, 1, 7); // 砥石
                spawnGroundItem(40093, 1, 10); // ブランクスク(Lv4)
                spawnGroundItem(40079, 1, 5); // 帰還スク
            }
            else if (curRound == 3)
            {
                spawnGroundItem(L1ItemId.ADENA, 10000, 30);
                spawnGroundItem(L1ItemId.POTION_OF_CURE_POISON, 7, 20);
                spawnGroundItem(L1ItemId.POTION_OF_EXTRA_HEALING, 20, 20);
                spawnGroundItem(L1ItemId.POTION_OF_GREATER_HEALING, 10, 20);
                spawnGroundItem(40317, 1, 10); // 砥石
                spawnGroundItem(40094, 1, 10); // ブランクスク(Lv5)
            }
        }

        /// <summary>
        /// コロシアムから出たメンバーをメンバーリストから削除する。
        /// </summary>
        private void removeRetiredMembers()
        {
            L1PcInstance[] temp = MembersArray;
            foreach (L1PcInstance element in temp)
            {
                if (element.MapId != _mapId)
                {
                    removeMember(element);
                }
            }
        }

        /// <summary>
        /// UBに参加しているプレイヤーへメッセージ(S_ServerMessage)を送信する。
        /// </summary>
        /// <param name="type">
        ///            メッセージタイプ </param>
        /// <param name="msg">
        ///            送信するメッセージ </param>
        private void sendMessage(int type, string msg)
        {
            foreach (L1PcInstance pc in MembersArray)
            {
                pc.sendPackets(new S_ServerMessage(type, msg));
            }
        }

        /// <summary>
        /// コロシアム上へアイテムを出現させる。
        /// </summary>
        /// <param name="itemId">
        ///            出現させるアイテムのアイテムID </param>
        /// <param name="stackCount">
        ///            アイテムのスタック数 </param>
        /// <param name="count">
        ///            出現させる数 </param>
        private void spawnGroundItem(int itemId, int stackCount, int count)
        {
            L1Item temp = ItemTable.Instance.getTemplate(itemId);
            if (temp == null)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                L1Location loc = _location.randomLocation((LocX2 - LocX1) / 2, false);
                if (temp.Stackable)
                {
                    L1ItemInstance item = ItemTable.Instance.createItem(itemId);
                    item.EnchantLevel = 0;
                    item.Count = stackCount;
                    L1GroundInventory ground = Container.Instance.Resolve<IGameWorld>().getInventory(loc.X, loc.Y, _mapId);
                    if (ground.checkAddItem(item, stackCount) == L1Inventory.OK)
                    {
                        ground.storeItem(item);
                    }
                }
                else
                {
                    L1ItemInstance item = null;
                    for (int createCount = 0; createCount < stackCount; createCount++)
                    {
                        item = ItemTable.Instance.createItem(itemId);
                        item.EnchantLevel = 0;
                        L1GroundInventory ground = Container.Instance.Resolve<IGameWorld>().getInventory(loc.X, loc.Y, _mapId);
                        if (ground.checkAddItem(item, stackCount) == L1Inventory.OK)
                        {
                            ground.storeItem(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// コロシアム上のアイテムとモンスターを全て削除する。
        /// </summary>
        private void clearColosseum()
        {
            foreach (object obj in Container.Instance.Resolve<IGameWorld>().getVisibleObjects(_mapId).Values)
            {
                if (obj is L1MonsterInstance) // モンスター削除
                {
                    L1MonsterInstance mob = (L1MonsterInstance)obj;
                    if (!mob.Dead)
                    {
                        mob.Dead = true;
                        mob.Status = ActionCodes.ACTION_Die;
                        mob.CurrentHpDirect = 0;
                        mob.deleteMe();

                    }
                }
                else if (obj is L1Inventory) // アイテム削除
                {
                    L1Inventory inventory = (L1Inventory)obj;
                    inventory.clearItems();
                }
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public L1UltimateBattle()
        {
        }

        internal class UbThread : IRunnable
        {
            private readonly L1UltimateBattle outerInstance;

            public UbThread(L1UltimateBattle outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            /// <summary>
            /// 競技開始までをカウントダウンする。
            /// </summary>
            /// <exception cref="InterruptedException"> </exception>
            //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
            //ORIGINAL LINE: private void countDown() throws InterruptedException
            internal virtual void countDown()
            {
                // XXX - このIDは間違っている
                const int MSGID_COUNT = 637;
                const int MSGID_START = 632;

                for (int loop = 0; loop < BEFORE_MINUTE * 60 - 10; loop++)
                { // 開始10秒前まで待つ
                    Thread.Sleep(1000);
                    // removeRetiredMembers();
                }
                outerInstance.removeRetiredMembers();

                outerInstance.sendMessage(MSGID_COUNT, "10"); // 10秒前

                Thread.Sleep(5000);
                outerInstance.sendMessage(MSGID_COUNT, "5"); // 5秒前

                Thread.Sleep(1000);
                outerInstance.sendMessage(MSGID_COUNT, "4"); // 4秒前

                Thread.Sleep(1000);
                outerInstance.sendMessage(MSGID_COUNT, "3"); // 3秒前

                Thread.Sleep(1000);
                outerInstance.sendMessage(MSGID_COUNT, "2"); // 2秒前

                Thread.Sleep(1000);
                outerInstance.sendMessage(MSGID_COUNT, "1"); // 1秒前

                Thread.Sleep(1000);
                outerInstance.sendMessage(MSGID_START, "アルティメット バトル"); // スタート
                outerInstance.removeRetiredMembers();
            }

            /// <summary>
            /// 全てのモンスターが出現した後、次のラウンドが始まるまでの時間を待機する。
            /// </summary>
            /// <param name="curRound">
            ///            現在のラウンド </param>
            /// <exception cref="InterruptedException"> </exception>
            //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
            //ORIGINAL LINE: private void waitForNextRound(int curRound) throws InterruptedException
            internal virtual void waitForNextRound(int curRound)
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final int WAIT_TIME_TABLE[] = { 6, 6, 2, 18 };
                int[] WAIT_TIME_TABLE = new int[] { 6, 6, 2, 18 };

                int wait = WAIT_TIME_TABLE[curRound - 1];
                for (int i = 0; i < wait; i++)
                {
                    Thread.Sleep(10000);
                    // removeRetiredMembers();
                }
                outerInstance.removeRetiredMembers();
            }

            /// <summary>
            /// スレッドプロシージャ。
            /// </summary>
            public void run()
            {
                try
                {
                    outerInstance.Active = true;
                    countDown();
                    outerInstance.NowUb = true;
                    for (int round = 1; round <= 4; round++)
                    {
                        outerInstance.sendRoundMessage(round);

                        L1UbPattern pattern = UBSpawnTable.Instance.getPattern(outerInstance._ubId, outerInstance._pattern);

                        IList<L1UbSpawn> spawnList = pattern.getSpawnList(round);

                        foreach (L1UbSpawn spawn in spawnList)
                        {
                            if (outerInstance.MembersCount > 0)
                            {
                                spawn.spawnAll();
                            }

                            Thread.Sleep(spawn.SpawnDelay * 1000);
                            // removeRetiredMembers();
                        }

                        if (outerInstance.MembersCount > 0)
                        {
                            outerInstance.spawnSupplies(round);
                        }

                        waitForNextRound(round);
                    }

                    foreach (L1PcInstance pc in outerInstance.MembersArray) // コロシアム内に居るPCを外へ出す
                    {
                        int rndx = RandomHelper.Next(4);
                        int rndy = RandomHelper.Next(4);
                        int locx = 33503 + rndx;
                        int locy = 32764 + rndy;
                        short mapid = 4;
                        L1Teleport.teleport(pc, locx, locy, mapid, 5, true);
                        outerInstance.removeMember(pc);
                    }
                    outerInstance.clearColosseum();
                    outerInstance.Active = false;
                    outerInstance.NowUb = false;
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        /// <summary>
        /// アルティメットバトルを開始する。
        /// </summary>
        /// <param name="ubId">
        ///            開始するアルティメットバトルのID </param>
        public virtual void start()
        {
            int patternsMax = UBSpawnTable.Instance.getMaxPattern(_ubId);
            _pattern = RandomHelper.Next(patternsMax) + 1; // 出現パターンを決める

            UbThread ub = new UbThread(this);
            Container.Instance.Resolve<ITaskController>().execute(ub);
        }

        /// <summary>
        /// プレイヤーを参加メンバーリストへ追加する。
        /// </summary>
        /// <param name="pc">
        ///            新たに参加するプレイヤー </param>
        public virtual void addMember(L1PcInstance pc)
        {
            if (!_members.Contains(pc))
            {
                _members.Add(pc);
            }
        }

        /// <summary>
        /// プレイヤーを参加メンバーリストから削除する。
        /// </summary>
        /// <param name="pc">
        ///            削除するプレイヤー </param>
        public virtual void removeMember(L1PcInstance pc)
        {
            _members.Remove(pc);
        }

        /// <summary>
        /// 参加メンバーリストをクリアする。
        /// </summary>
        public virtual void clearMembers()
        {
            _members.Clear();
        }

        /// <summary>
        /// プレイヤーが、参加メンバーかを返す。
        /// </summary>
        /// <param name="pc">
        ///            調べるプレイヤー </param>
        /// <returns> 参加メンバーであればtrue、そうでなければfalse。 </returns>
        public virtual bool isMember(L1PcInstance pc)
        {
            return _members.Contains(pc);
        }

        /// <summary>
        /// 参加メンバーの配列を作成し、返す。
        /// </summary>
        /// <returns> 参加メンバーの配列 </returns>
        public virtual L1PcInstance[] MembersArray
        {
            get
            {
                return ((List<L1PcInstance>)_members).ToArray();
            }
        }

        /// <summary>
        /// 参加メンバー数を返す。
        /// </summary>
        /// <returns> 参加メンバー数 </returns>
        public virtual int MembersCount
        {
            get
            {
                return _members.Count;
            }
        }

        /// <summary>
        /// UB中かを設定する。
        /// </summary>
        /// <param name="i">
        ///            true/false </param>
        public bool NowUb
        {
            set
            {
                _isNowUb = value;
            }
            get
            {
                return _isNowUb;
            }
        }


        public virtual int UbId
        {
            get
            {
                return _ubId;
            }
            set
            {
                _ubId = value;
            }
        }


        public virtual short MapId
        {
            get
            {
                return _mapId;
            }
            set
            {
                _mapId = value;
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


        public virtual int MaxPlayer
        {
            get
            {
                return _maxPlayer;
            }
            set
            {
                _maxPlayer = value;
            }
        }


        public virtual bool EnterRoyal
        {
            set
            {
                _enterRoyal = value;
            }
        }

        public virtual bool EnterKnight
        {
            set
            {
                _enterKnight = value;
            }
        }

        public virtual bool EnterMage
        {
            set
            {
                _enterMage = value;
            }
        }

        public virtual bool EnterElf
        {
            set
            {
                _enterElf = value;
            }
        }

        public virtual bool EnterDarkelf
        {
            set
            {
                _enterDarkelf = value;
            }
        }

        public virtual bool EnterDragonKnight
        {
            set
            {
                _enterDragonKnight = value;
            }
        }

        public virtual bool EnterIllusionist
        {
            set
            {
                _enterIllusionist = value;
            }
        }

        public virtual bool EnterMale
        {
            set
            {
                _enterMale = value;
            }
        }

        public virtual bool EnterFemale
        {
            set
            {
                _enterFemale = value;
            }
        }

        public virtual bool canUsePot()
        {
            return _usePot;
        }

        public virtual bool UsePot
        {
            set
            {
                _usePot = value;
            }
        }

        public virtual int Hpr
        {
            get
            {
                return _hpr;
            }
            set
            {
                _hpr = value;
            }
        }


        public virtual int Mpr
        {
            get
            {
                return _mpr;
            }
            set
            {
                _mpr = value;
            }
        }


        public virtual int LocX1
        {
            get
            {
                return _locX1;
            }
            set
            {
                _locX1 = value;
            }
        }


        public virtual int LocY1
        {
            get
            {
                return _locY1;
            }
            set
            {
                _locY1 = value;
            }
        }


        public virtual int LocX2
        {
            get
            {
                return _locX2;
            }
            set
            {
                _locX2 = value;
            }
        }


        public virtual int LocY2
        {
            get
            {
                return _locY2;
            }
            set
            {
                _locY2 = value;
            }
        }


        // setされたlocx1～locy2から中心点を求める。
        public virtual void resetLoc()
        {
            _locX = (_locX2 + _locX1) / 2;
            _locY = (_locY2 + _locY1) / 2;
            _location = new L1Location(_locX, _locY, _mapId);
        }

        public virtual L1Location Location
        {
            get
            {
                return _location;
            }
        }

        public virtual void addManager(int npcId)
        {
            _managers.Add(npcId);
        }

        public virtual bool containsManager(int npcId)
        {
            return _managers.Contains(npcId);
        }

        public virtual void addUbTime(int time)
        {
            _ubTimes.Add(time);
        }

        public virtual string NextUbTime
        {
            get
            {
                return intToTimeFormat(nextUbTime());
            }
        }

        private int nextUbTime()
        {
            int key = GetTimeKey(DateTime.Now);

            foreach (var item in _ubTimes)
            {
                if (item >= key)
                {
                    return item;
                }
            }
            return _ubTimes.Min;
        }

        private int GetTimeKey(DateTime dateTime)
        {
            return dateTime.Hour * 100 + dateTime.Minute;
        }

        private static string intToTimeFormat(int n)
        {
            return n / 100 + ":" + n % 100 / 10 + "" + n % 10;
        }

        public virtual bool checkUbTime()
        {
            return _ubTimes.Contains(GetTimeKey(DateTime.Now.AddMinutes(BEFORE_MINUTE)));
        }

        public bool Active
        {
            set
            {
                _active = value;
            }
            get
            {
                return _active;
            }
        }


        /// <summary>
        /// UBに参加可能か、レベル、クラスをチェックする。
        /// </summary>
        /// <param name="pc">
        ///            UBに参加できるかチェックするPC </param>
        /// <returns> 参加出来る場合はtrue,出来ない場合はfalse </returns>
        public virtual bool canPcEnter(L1PcInstance pc)
        {
            // 参加可能なレベルか
            if (!IntRange.includes(pc.Level, _minLevel, _maxLevel))
            {
                return false;
            }

            // 参加可能なクラスか
            if (!((pc.Crown && _enterRoyal) || (pc.Knight && _enterKnight) || (pc.Wizard && _enterMage) || (pc.Elf && _enterElf) || (pc.Darkelf && _enterDarkelf) || (pc.DragonKnight && _enterDragonKnight) || (pc.Illusionist && _enterIllusionist)))
            {
                return false;
            }

            return true;
        }

        private string[] _ubInfo;

        public virtual string[] makeUbInfoStrings()
        {
            if (_ubInfo != null)
            {
                return _ubInfo;
            }
            string nextUbTime = NextUbTime;
            // クラス
            StringBuilder classesBuff = new StringBuilder();
            if (_enterDarkelf)
            {
                classesBuff.Append("ダーク エルフ ");
            }
            if (_enterMage)
            {
                classesBuff.Append("ウィザード ");
            }
            if (_enterElf)
            {
                classesBuff.Append("エルフ ");
            }
            if (_enterKnight)
            {
                classesBuff.Append("ナイト ");
            }
            if (_enterRoyal)
            {
                classesBuff.Append("プリンス ");
            }
            if (_enterDragonKnight)
            {
                classesBuff.Append("ドラゴンナイト ");
            }
            if (_enterIllusionist)
            {
                classesBuff.Append("イリュージョニスト ");
            }
            string classes = classesBuff.ToString().Trim();
            // 性別
            StringBuilder sexBuff = new StringBuilder();
            if (_enterMale)
            {
                sexBuff.Append("男 ");
            }
            if (_enterFemale)
            {
                sexBuff.Append("女 ");
            }
            string sex = sexBuff.ToString().Trim();
            string loLevel = _minLevel.ToString();
            string hiLevel = _maxLevel.ToString();
            string teleport = _location.getMap().Escapable ? "可能" : "不可能";
            string res = _location.getMap().UseResurrection ? "可能" : "不可能";
            string pot = "可能";
            string hpr = _hpr.ToString();
            string mpr = _mpr.ToString();
            string summon = _location.getMap().TakePets ? "可能" : "不可能";
            string summon2 = _location.getMap().RecallPets ? "可能" : "不可能";
            _ubInfo = new string[] { nextUbTime, classes, sex, loLevel, hiLevel, teleport, res, pot, hpr, mpr, summon, summon2 };
            return _ubInfo;
        }
    }

}