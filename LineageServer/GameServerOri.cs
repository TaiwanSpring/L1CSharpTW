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
using System;
using System.Data;
using System.Threading;
namespace LineageServer.Server
{
    public class GameServerOri : IRunnable
    {
        private static ILogger _log = Logger.GetLogger(nameof(GameServerOri));

        public readonly int startTime = DateTime.Now.Second;

        private ServerSocket _serverSocket;

        private int _port;
        private LoginController loginController;
        private ITaskController taskController;

        public void run()
        {
            //模擬器重開延遲  秒
            Thread.Sleep(TimeSpan.FromSeconds(Config.Gamesleep));

            Console.WriteLine(L1Message.memoryUse + SystemUtil.UsedMemoryMB + L1Message.memory);
            Console.WriteLine(L1Message.waitingforuser);
            while (true)
            {
                try
                {
                    Socket socket = _serverSocket.accept();
                    Console.WriteLine(L1Message.from + socket.InetAddress + L1Message.attempt);
                    string host = socket.InetAddress.HostAddress;
                    if (IpTable.Instance.isBannedIp(host))
                    {
                        _log.Info("banned IP(" + host + ")");
                    }
                    else
                    {
                        ClientThread client = new ClientThread(socket);
                        this.taskController.execute(client);
                    }
                }
                catch (IOException)
                {
                }
            }
        }

        public virtual void initialize()
        {
            IContainerAdapter containerAdapter = Container.Instance;
            this.taskController = new RunnableExecuter();
            //工作排程，執行緒管理
            containerAdapter.RegisterInstance<ITaskController>(taskController);
            //SQL連線
            containerAdapter.RegisterInstance<IDbConnection>(null);
            //SQL操作
            containerAdapter.RegisterInstance<IDataSourceFactory>(new DataSourceFactory());

            string s = Config.GAME_SERVER_HOST_NAME;
            double rateXp = Config.RATE_XP;
            double LA = Config.RATE_LA;
            double rateKarma = Config.RATE_KARMA;
            double rateDropItems = Config.RATE_DROP_ITEMS;
            double rateDropAdena = Config.RATE_DROP_ADENA;
            int gobalChatLevel = Config.GLOBAL_CHAT_LEVEL;
            // Locale 多國語系
            _port = Config.GAME_SERVER_PORT;

            if ("*" == s)
            {
                _serverSocket = new ServerSocket(_port);
                Console.WriteLine(L1Message.setporton + this._port);
            }
            else
            {
                InetAddress inetaddress = InetAddress.getByName(s);
                inetaddress.HostAddress;
                _serverSocket = new ServerSocket(_port, 50, inetaddress);
                Console.WriteLine(L1Message.setporton + this._port);
            }

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

            //Table
            //門控制
            DoorTable doorTable = new DoorTable();
            doorTable.Initialize();
            containerAdapter.RegisterInstance<IDoorController>(doorTable);
            //怪物重生控制
            SpawnTable spawnTable = new SpawnTable();
            spawnTable.Initialize();
            containerAdapter.RegisterInstance<ISpawnController>(spawnTable);
            //怪物群組控制
            L1MobGroupSpawn mobGroupSpawn = new L1MobGroupSpawn();
            mobGroupSpawn.Initialize();
            containerAdapter.RegisterInstance<IMobGroupController>(mobGroupSpawn);


            SkillsTable.Instance;
            PolyTable.Instance;
            ItemTable.Instance;
            DropTable.Instance;
            DropItemTable.Instance;
            ShopTable.Instance;
            NPCTalkDataTable.Instance;
            L1WorldTraps.Instance;
            Dungeon.Instance;
            NpcContainer.Instance.Resolve<ISpawnController>();
            IpTable.Instance;
            MapsTable.Instance;
            UBContainer.Instance.Resolve<ISpawnController>();
            PetTable.Instance;
            ClanTable.Instance;
            CastleTable.Instance;
            GetBackRestartTable.Instance;

            //L1NpcRegenerationTimer.Instance;
            ChatLogTable.Instance;
            WeaponSkillTable.Instance;



            NpcActionTable.load();
            GMCommandsConfig.Load();
            Getback.loadGetBack();
            PetTypeTable.load();
            L1BossCycle.load();
            L1TreasureBox.load();
            SprTable.Instance;
            ResolventTable.Instance;
            FurnitureContainer.Instance.Resolve<ISpawnController>();
            NpcChatTable.Instance;
            MailTable.Instance;
            RaceTicketTable.Instance;
            L1BugBearRace.Instance;
            InnTable.Instance;
            MagicDollTable.Instance;
            FurnitureItemTable.Instance;
            //無明顯順序的
            //動作控制
            L1NpcDefaultAction npcDefaultAction = new L1NpcDefaultAction();
            npcDefaultAction.Initialize();
            containerAdapter.RegisterInstance<IGameActionProvider>(npcDefaultAction);

            //世界地圖
            L1WorldMap l1WorldMap = new L1WorldMap();
            l1WorldMap.Initialize();
            containerAdapter.RegisterInstance<IWorldMap>(l1WorldMap);

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

            //Npc控制
            NpcTable npcController = new NpcTable();
            npcController.Initialize();
            containerAdapter.RegisterInstance<INpcController>(npcController);

            //遊戲時間
            L1GameTimeClock l1GameTimeClock = new L1GameTimeClock();
            l1GameTimeClock.Initialize();
            containerAdapter.RegisterInstance<IGameTimeClock>(l1GameTimeClock);

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
            //MysqlAutoBackup.Instance;

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