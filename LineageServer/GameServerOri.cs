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
		private int chatlvl;

		public void run()
		{
			System.Console.WriteLine(L1Message.memoryUse + SystemUtil.UsedMemoryMB + L1Message.memory);
			System.Console.WriteLine(L1Message.waitingforuser);
			while (true)
			{
				try
				{
					Socket socket = _serverSocket.accept();
					System.Console.WriteLine(L1Message.from + socket.InetAddress + L1Message.attempt);
					string host = socket.InetAddress.HostAddress;
					if (IpTable.Instance.isBannedIp(host))
					{
						_log.Info("banned IP(" + host + ")");
					}
					else
					{
						ClientThread client = new ClientThread(socket);
						RunnableExecuter.Instance.execute(client);
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
			RunnableExecuter taskController = new RunnableExecuter();
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

			// Locale 多國語系

			chatlvl = Config.GLOBAL_CHAT_LEVEL;
			_port = Config.GAME_SERVER_PORT;

			if ("*" == s)
			{
				_serverSocket = new ServerSocket(_port);
				System.Console.WriteLine(L1Message.setporton + this._port);
			}
			else
			{
				InetAddress inetaddress = InetAddress.getByName(s);
				inetaddress.HostAddress;
				_serverSocket = new ServerSocket(_port, 50, inetaddress);
				System.Console.WriteLine(L1Message.setporton + this._port);
			}

			System.Console.WriteLine("┌───────────────────────────────┐");
			System.Console.WriteLine($"│     {L1Message.ver}\t\t│");
			System.Console.WriteLine("└───────────────────────────────┘");

			System.Console.WriteLine(L1Message.settingslist);
			System.Console.WriteLine($"┌{L1Message.exp}: { rateXp }{L1Message.x}{Environment.NewLine}├{L1Message.justice}: { LA }{L1Message.x}{Environment.NewLine}├{L1Message.karma}: { rateKarma }{L1Message.x}{Environment.NewLine}├{L1Message.dropitems}: { rateDropItems }{L1Message.x}{Environment.NewLine}├{L1Message.dropadena}: { rateDropAdena }{L1Message.x}\n\r├{L1Message.enchantweapon}: { Config.ENCHANT_CHANCE_WEAPON }%{Environment.NewLine}├{L1Message.enchantarmor}: { Config.ENCHANT_CHANCE_ARMOR }%");
			System.Console.WriteLine($"├{L1Message.chatlevel}: { this.chatlvl }{L1Message.level}");

			if (Config.ALT_NONPVP)
			{ // Non-PvP設定
				System.Console.WriteLine($"└{L1Message.nonpvpNo}");
			}
			else
			{
				System.Console.WriteLine($"└{L1Message.nonpvpYes}");
			}

			int maxOnlineUsers = Config.MAX_ONLINE_USERS;
			System.Console.WriteLine($"{L1Message.maxplayer}{ maxOnlineUsers }{L1Message.player}");

			System.Console.WriteLine("┌───────────────────────────────┐");
			System.Console.WriteLine($"│     {L1Message.ver}\t\t│");
			System.Console.WriteLine("└───────────────────────────────┘");

			//唯一ID產生器
			IdFactory idFactory = new IdFactory();
			idFactory.Initialize();
			containerAdapter.RegisterInstance<IIdFactory>(idFactory);

			//Table


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


			NpcTable.Instance;
			L1DeleteItemOnGround deleteitem = new L1DeleteItemOnGround();
			deleteitem.initialize();

			if (!NpcTable.Instance.Initialized)
			{
				throw new Exception("Could not initialize the npc table");
			}
			// 循環公告 by阿傑
			if (Config.Use_Show_Announcecycle)
			{
				this.gameComponentSet.Add(new Announcecycle().Initialize());
			}
			L1NpcDefaultAction.Instance;
			DoorTable.initialize();
			SpawnTable.Instance;
			MobGroupTable.Instance;
			SkillsTable.Instance;
			PolyTable.Instance;
			ItemTable.Instance;
			DropTable.Instance;
			DropItemTable.Instance;
			ShopTable.Instance;
			NPCTalkDataTable.Instance;
			L1World.Instance;

			//傳送字串訊息給Client
			containerAdapter.RegisterInstance<ISendMessageTo>(new SendMessageTo());

			L1WorldTraps.Instance;
			Dungeon.Instance;
			NpcSpawnTable.Instance;
			IpTable.Instance;
			MapsTable.Instance;
			UBSpawnTable.Instance;
			PetTable.Instance;
			ClanTable.Instance;
			CastleTable.Instance;

			Thread.Sleep(Config.Gamesleep * 1000); //模擬器重開延遲  秒

			L1CastleLocation.setCastleTaxRate(); // 必須在 CastleTable 初始化之後
			GetBackRestartTable.Instance;
			RunnableExecuter.Instance;
			L1NpcRegenerationTimer.Instance;
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
			FurnitureSpawnTable.Instance;
			NpcChatTable.Instance;
			MailTable.Instance;
			RaceTicketTable.Instance;
			L1BugBearRace.Instance;
			InnTable.Instance;
			MagicDollTable.Instance;
			FurnitureItemTable.Instance;

			System.Console.WriteLine(L1Message.initialfinished);

			taskController.Start();
		}
	}

}