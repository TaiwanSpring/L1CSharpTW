using LineageServer.DataBase;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Game;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.item;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Model.npc.action;
using LineageServer.Server.Model.trap;
using LineageServer.Utils;
using LineageServer.william;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace LineageServer.Server
{
    public class GameServerOri : IRunnable, IGameServer
    {
        private static ILogger _log = Logger.GetLogger(nameof(GameServerOri));

        public int startTime { get; } = DateTime.Now.Second;

        private LoginController loginController;
        private ITaskController taskController;

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }
        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding { get; } = GobalParameters.Encoding;
        /// <summary>
		/// 
		/// </summary>
		private TcpListener listener;
        public GameServerOri()
        {
            Config.load();
        }

        public void run()
        {
            //模擬器重開延遲  秒
            //Thread.Sleep(TimeSpan.FromSeconds(Config.Gamesleep));

            this.listener = new TcpListener(Address, Port);

            Console.WriteLine(L1Message.memoryUse + SystemUtil.UsedMemoryMB + L1Message.memory);
            Console.WriteLine(L1Message.waitingforuser);
            this.listener.Start();
            while (true)
            {
                try
                {
                    TcpClient client = this.listener.AcceptTcpClient();

                    if (client.Client.RemoteEndPoint is IPEndPoint endPoint)
                    {
                        Console.WriteLine($"{L1Message.from}{endPoint.Address}{L1Message.attempt}");
                        if (IpTable.Instance.isBannedIp(endPoint.Address.ToString()))
                        {
                            _log.Info($"banned IP({endPoint.Address})");
                        }
                        else
                        {
                            ClientThread clientThread = new ClientThread(client);
                            this.taskController.execute(clientThread);
                        }
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public virtual void initialize()
        {
            IContainerAdapter containerAdapter = Container.Instance;

            ContainerObject.SetContainerAdapter(containerAdapter);

            this.taskController = new RunnableExecuter();
            containerAdapter.RegisterInstance<IEncode>(new LineageServer.Models.Encode());
            containerAdapter.RegisterInstance<IXmlDeserialize>(new XmlDeserialize());
            containerAdapter.RegisterInstance<IGameServer>(this);
            //工作排程，執行緒管理
            containerAdapter.RegisterInstance<ITaskController>(taskController);

            string connectString = $"server=localHost;uid=root;pwd=753951;database=l1csdb;";
            //SQL連線
            containerAdapter.RegisterInstance<IDbConnection>(new MySqlConnection(connectString));
            //SQL操作
            containerAdapter.RegisterInstance<IDataSourceFactory>(new DataSourceFactory());

            string s = Config.GAME_SERVER_HOST_NAME;

            if (s == "*")
            {
                Address = IPAddress.Any;
            }
            else if (IPAddress.TryParse(s, out IPAddress address))
            {
                Address = address;
            }

            Port = Config.GAME_SERVER_PORT;

            double rateXp = Config.RATE_XP;
            double LA = Config.RATE_LA;
            double rateKarma = Config.RATE_KARMA;
            double rateDropItems = Config.RATE_DROP_ITEMS;
            double rateDropAdena = Config.RATE_DROP_ADENA;
            int gobalChatLevel = Config.GLOBAL_CHAT_LEVEL;

            Console.WriteLine("┌───────────────────────────────┐");
            Console.WriteLine($"│     {L1Message.ver}\t\t│");
            Console.WriteLine("└───────────────────────────────┘");

            Console.WriteLine(L1Message.settingslist);
            Console.WriteLine($"┌{L1Message.exp}: { rateXp }{L1Message.x}{Environment.NewLine}├{L1Message.justice}: { LA }{L1Message.x}{Environment.NewLine}├{L1Message.karma}: { rateKarma }{L1Message.x}{Environment.NewLine}├{L1Message.dropitems}: { rateDropItems }{L1Message.x}{Environment.NewLine}├{L1Message.dropadena}: { rateDropAdena }{L1Message.x}\n\r├{L1Message.enchantweapon}: { Config.ENCHANT_CHANCE_WEAPON }%{Environment.NewLine}├{L1Message.enchantarmor}: { Config.ENCHANT_CHANCE_ARMOR }%");
            Console.WriteLine($"├{L1Message.chatlevel}: { gobalChatLevel }{L1Message.level}");

            if (Config.ALT_NONPVP)
            { // Non-PvP設定
                Console.WriteLine($"└{L1Message.nonpvpNo}");
            }
            else
            {
                Console.WriteLine($"└{L1Message.nonpvpYes}");
            }

            int maxOnlineUsers = Config.MAX_ONLINE_USERS;
            Console.WriteLine($"{L1Message.maxplayer}{ maxOnlineUsers }{L1Message.player}");

            Console.WriteLine("┌───────────────────────────────┐");
            Console.WriteLine($"│     {L1Message.ver}\t\t│");
            Console.WriteLine("└───────────────────────────────┘");

            //唯一ID產生器
            IdFactory idFactory = new IdFactory();
            idFactory.Initialize();
            containerAdapter.RegisterInstance<IIdFactory>(idFactory);

            //遊戲世界
            GameWorld gameWorld = new GameWorld();
            gameWorld.Initialize();
            containerAdapter.RegisterInstance<IGameWorld>(gameWorld);

            //遊戲時間
            L1GameTimeClock l1GameTimeClock = new L1GameTimeClock();
            l1GameTimeClock.Initialize();
            containerAdapter.RegisterInstance<IGameTimeClock>(l1GameTimeClock);

            MapsTable.Instance.Initialize();

            //世界地圖
            L1WorldMap l1WorldMap = new L1WorldMap();
            l1WorldMap.Initialize();
            containerAdapter.RegisterInstance<IWorldMap>(l1WorldMap);

            //Npc控制
            NpcTable npcController = new NpcTable();
            npcController.Initialize();
            containerAdapter.RegisterInstance<INpcController>(npcController);

            NpcActionTable.load();
            //動作控制
            L1NpcDefaultAction npcDefaultAction = new L1NpcDefaultAction();
            npcDefaultAction.Initialize();
            containerAdapter.RegisterInstance<IGameActionProvider>(npcDefaultAction);
            //Table
            //門控制
            DoorTable doorTable = new DoorTable();
            doorTable.Initialize();
            containerAdapter.RegisterInstance<IDoorController>(doorTable);
            //怪物群組控制
            L1MobGroupSpawn mobGroupSpawn = new L1MobGroupSpawn();
            mobGroupSpawn.Initialize();
            containerAdapter.RegisterInstance<IMobGroupController>(mobGroupSpawn);

            NpcChatTable.Instance.Initialize();
            //怪物重生控制
            SpawnTable spawnTable = new SpawnTable();
            spawnTable.Initialize();
            containerAdapter.RegisterInstance<ISpawnController>(spawnTable);

            //技能
            SkillsTable.Instance.Initialize();
            //變身
            PolyTable.Instance.Initialize();
            //物品
            ItemTable.Instance.Initialize();
            //掉落
            DropTable.Instance.Initialize();
            //裝備掉落
            DropItemTable.Instance.Initialize();

            ShopTable.Instance.Initialize();

            NPCTalkDataTable.Instance.Initialize();

            L1WorldTraps.Instance.Initialize();

            DungeonController.Instance.Initialize();

            NpcSpawnTable.Instance.Initialize();

            IpTable.Instance.Initialize();

            UBSpawnTable.Instance.Initialize();

            PetTable.Instance.Initialize();

            ClanTable.Instance.Initialize();

            CastleTable.Instance.Initialize();

            GetBackRestartTable.Instance.Initialize();

            //L1NpcRegenerationTimer.Instance.Initialize();
            ChatLogTable.Instance.Initialize();

            WeaponSkillTable.Instance.Initialize();

            GMCommandsConfig.Load();
            GetbackController.loadGetBack();
            PetTypeTable.load();
            //L1BossCycle.load();
            L1TreasureBox.load();

            SprTable.Instance.Initialize();

            ResolventTable.Instance.Initialize();

            FurnitureSpawnTable.Instance.Initialize();

            MailTable.Instance.Initialize();

            RaceTicketTable.Instance.Initialize();

            L1BugBearRace.Instance.Initialize();

            InnTable.Instance.Initialize();

            MagicDollTable.Instance.Initialize();

            FurnitureItemTable.Instance.Initialize();
            //無明顯順序的


            // 初始化帳號使用狀態
            Account.InitialOnlineStatus();

            //登入控制
            this.loginController = new LoginController();
            this.loginController.MaxAllowedOnlinePlayers = maxOnlineUsers;
            containerAdapter.RegisterInstance<ILoginController>(this.loginController);

            //角色控制
            ICharacterController characterController = new CharacterTable();
            //讀取所有角色名稱
            characterController.loadAllCharName();
            //讀取角色的上線狀態
            characterController.ClearOnlineStatus();
            containerAdapter.RegisterInstance<ICharacterController>(characterController);

            // 伺服器自動重啟 
            L1GameReStart restartController = new L1GameReStart();
            restartController.Initialize();
            containerAdapter.RegisterInstance<IRestartController>(restartController);

            // 初始化無限大戰
            UbTimeController ubTimeContoroller = new UbTimeController();
            taskController.execute(ubTimeContoroller);

            // 初始化攻城
            WarTimeController warTimeController = new WarTimeController();
            taskController.execute(warTimeController);
            containerAdapter.RegisterInstance<IWarController>(warTimeController);

            // 設定精靈石的產生
            if (Config.ELEMENTAL_STONE_AMOUNT > 0)
            {
                taskController.execute(new ElementalStoneGenerator());
            }

            // 初始化 HomeTown 時間
            HomeTownTimeController homeTownTimeController = new HomeTownTimeController();
            homeTownTimeController.Initialize();
            containerAdapter.RegisterInstance(homeTownTimeController);

            // 初始化盟屋拍賣
            taskController.execute(new AuctionTimeController());

            // 初始化盟屋的稅金
            taskController.execute(new HouseTaxTimeController());

            // 初始化釣魚
            taskController.execute(new FishingTimeController());

            // 初始化 NPC 聊天
            taskController.execute(new NpcChatTimeController());

            // 初始化 Light
            taskController.execute(new LightTimeController());
            // TODO 殷海薩的祝福
            taskController.execute(new AinTimeController());

            // 初始化MySQL自動備份程序
            //MysqlAutoBackup.Instance.Initialize();

            // 開始 MySQL自動備份程序 計時器
            //MysqlAutoBackupTimer.TimerStart();

            //地上物品刪除功能
            new L1DeleteItemOnGround().Initialize();

            // 循環公告 by阿傑
            new Announcecycle().Initialize();

            //傳送字串訊息給Client
            containerAdapter.RegisterInstance<ISendMessageTo>(new SendMessageTo());

            L1CastleLocation.setCastleTaxRate(); // 必須在 CastleTable 初始化之後

            Console.WriteLine(L1Message.initialfinished);

            this.taskController.Start();
        }
    }
}