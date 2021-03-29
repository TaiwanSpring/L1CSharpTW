using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.Server
{

	using AinTimeController = LineageServer.Server.AinTimeController;
	using Config = LineageServer.Server.Config;
	using L1Message = LineageServer.Server.L1Message;
	using ConsoleProcess = LineageServer.Server.Console.ConsoleProcess;
	using CastleTable = LineageServer.Server.DataTables.CastleTable;
	using CharacterTable = LineageServer.Server.DataTables.CharacterTable;
	using ChatLogTable = LineageServer.Server.DataTables.ChatLogTable;
	using ClanTable = LineageServer.Server.DataTables.ClanTable;
	using DoorTable = LineageServer.Server.DataTables.DoorTable;
	using DropTable = LineageServer.Server.DataTables.DropTable;
	using DropItemTable = LineageServer.Server.DataTables.DropItemTable;
	using FurnitureItemTable = LineageServer.Server.DataTables.FurnitureItemTable;
	using FurnitureSpawnTable = LineageServer.Server.DataTables.FurnitureSpawnTable;
	using GetBackRestartTable = LineageServer.Server.DataTables.GetBackRestartTable;
	using InnTable = LineageServer.Server.DataTables.InnTable;
	using IpTable = LineageServer.Server.DataTables.IpTable;
	using ItemTable = LineageServer.Server.DataTables.ItemTable;
	using MagicDollTable = LineageServer.Server.DataTables.MagicDollTable;
	using MailTable = LineageServer.Server.DataTables.MailTable;
	using MapsTable = LineageServer.Server.DataTables.MapsTable;
	using MobGroupTable = LineageServer.Server.DataTables.MobGroupTable;
	using NpcActionTable = LineageServer.Server.DataTables.NpcActionTable;
	using NpcChatTable = LineageServer.Server.DataTables.NpcChatTable;
	using NpcSpawnTable = LineageServer.Server.DataTables.NpcSpawnTable;
	using NpcTable = LineageServer.Server.DataTables.NpcTable;
	using NPCTalkDataTable = LineageServer.Server.DataTables.NPCTalkDataTable;
	using PetTable = LineageServer.Server.DataTables.PetTable;
	using PetTypeTable = LineageServer.Server.DataTables.PetTypeTable;
	using PolyTable = LineageServer.Server.DataTables.PolyTable;
	using RaceTicketTable = LineageServer.Server.DataTables.RaceTicketTable;
	using ResolventTable = LineageServer.Server.DataTables.ResolventTable;
	using ShopTable = LineageServer.Server.DataTables.ShopTable;
	using SkillsTable = LineageServer.Server.DataTables.SkillsTable;
	using SpawnTable = LineageServer.Server.DataTables.SpawnTable;
	using SprTable = LineageServer.Server.DataTables.SprTable;
	using UBSpawnTable = LineageServer.Server.DataTables.UBSpawnTable;
	using WeaponSkillTable = LineageServer.Server.DataTables.WeaponSkillTable;
	using Dungeon = LineageServer.Server.Model.Dungeon;
	using ElementalStoneGenerator = LineageServer.Server.Model.ElementalStoneGenerator;
	using Getback = LineageServer.Server.Model.Getback;
	using L1BossCycle = LineageServer.Server.Model.L1BossCycle;
	using L1CastleLocation = LineageServer.Server.Model.L1CastleLocation;
	using L1DeleteItemOnGround = LineageServer.Server.Model.L1DeleteItemOnGround;
	using L1NpcRegenerationTimer = LineageServer.Server.Model.L1NpcRegenerationTimer;
	using L1World = LineageServer.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1BugBearRace = LineageServer.Server.Model.Game.L1BugBearRace;
	using L1GameTimeClock = LineageServer.Server.Model.Gametime.L1GameTimeClock;
	using L1TreasureBox = LineageServer.Server.Model.item.L1TreasureBox;
	using L1WorldMap = LineageServer.Server.Model.Map.L1WorldMap;
	using L1NpcDefaultAction = LineageServer.Server.Model.Npc.Action.L1NpcDefaultAction;
	using L1WorldTraps = LineageServer.Server.Model.trap.L1WorldTraps;
	using MysqlAutoBackup = LineageServer.Server.Storage.mysql.MysqlAutoBackup;
	using MysqlAutoBackupTimer = LineageServer.Utils.MysqlAutoBackupTimer;
	using SystemUtil = LineageServer.Utils.SystemUtil;
	using L1GameReStart = LineageServer.william.L1GameReStart;

	public class GameServerOri : IRunnable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(GameServerOri).FullName);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private static int YesNoCount_Conflict = 0;
		public readonly int startTime = (int)(DateTimeHelper.CurrentUnixTimeMillis() / 1000);
		private ServerSocket _serverSocket;
		private int _port;
		private LoginController _loginController;
		private int chatlvl;

		public override void run()
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

		private static GameServerOri _instance;

		private GameServerOri() : base("GameServer")
		{
		}

		public static GameServerOri Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameServerOri();
				}
				return _instance;
			}
		}

		public virtual void initialize()
		{
			string s = Config.GAME_SERVER_HOST_NAME;
			double rateXp = Config.RATE_XP;
			double LA = Config.RATE_LA;
			double rateKarma = Config.RATE_KARMA;
			double rateDropItems = Config.RATE_DROP_ITEMS;
			double rateDropAdena = Config.RATE_DROP_ADENA;

			// Locale 多國語系
			L1Message.Instance;

			chatlvl = Config.GLOBAL_CHAT_LEVEL;
			_port = Config.GAME_SERVER_PORT;
			if (!"*".Equals(s))
			{
				InetAddress inetaddress = InetAddress.getByName(s);
				inetaddress.HostAddress;
				_serverSocket = new ServerSocket(_port, 50, inetaddress);
                System.Console.WriteLine(L1Message.setporton + this._port);
			}
			else
			{
				_serverSocket = new ServerSocket(_port);
                System.Console.WriteLine(L1Message.setporton + this._port);
			}

            System.Console.WriteLine("┌───────────────────────────────┐");
            System.Console.WriteLine("│     " + L1Message.ver + "\t" + "\t" + "│");
            System.Console.WriteLine("└───────────────────────────────┘" + "\n");

            System.Console.WriteLine(L1Message.settingslist + "\n");
            System.Console.WriteLine("┌" + L1Message.exp + ": " + (rateXp) + L1Message.x + "\n\r├" + L1Message.justice + ": " + (LA) + L1Message.x + "\n\r├" + L1Message.karma + ": " + (rateKarma) + L1Message.x + "\n\r├" + L1Message.dropitems + ": " + (rateDropItems) + L1Message.x + "\n\r├" + L1Message.dropadena + ": " + (rateDropAdena) + L1Message.x + "\n\r├" + L1Message.enchantweapon + ": " + (Config.ENCHANT_CHANCE_WEAPON) + "%" + "\n\r├" + L1Message.enchantarmor + ": " + (Config.ENCHANT_CHANCE_ARMOR) + "%");
            System.Console.WriteLine("├" + L1Message.chatlevel + ": " + (this.chatlvl) + L1Message.level);

			if (Config.ALT_NONPVP)
			{ // Non-PvP設定
                System.Console.WriteLine("└" + L1Message.nonpvpNo + "\n");
			}
			else
			{
                System.Console.WriteLine("└" + L1Message.nonpvpYes + "\n");
			}

			int maxOnlineUsers = Config.MAX_ONLINE_USERS;
            System.Console.WriteLine(L1Message.maxplayer + (maxOnlineUsers) + L1Message.player);

            System.Console.WriteLine("┌───────────────────────────────┐");
            System.Console.WriteLine("│     " + L1Message.ver + "\t" + "\t" + "│");
            System.Console.WriteLine("└───────────────────────────────┘" + "\n");

			IdFactory.Instance;
			L1WorldMap.Instance;
			_loginController = LoginController.Instance;
			_loginController.MaxAllowedOnlinePlayers = maxOnlineUsers;

			// 讀取所有角色名稱
			CharacterTable.Instance.loadAllCharName();

			// 初始化角色的上線狀態
			CharacterTable.ClearOnlineStatus();

			// 初始化遊戲時間
			L1GameTimeClock.Init();
			// 伺服器自動重啟 
			if (Config.REST_TIME != 0)
			{
				L1GameReStart.init();
			}
			// 伺服器自動重啟  end

			// UB
			// 初始化無限大戰
			UbTimeController ubTimeContoroller = UbTimeController.Instance;
			RunnableExecuter.Instance.execute(ubTimeContoroller);

			// 初始化攻城
			WarTimeController warTimeController = WarTimeController.Instance;
			RunnableExecuter.Instance.execute(warTimeController);

			// 設定精靈石的產生
			if (Config.ELEMENTAL_STONE_AMOUNT > 0)
			{
				ElementalStoneGenerator elementalStoneGenerator = ElementalStoneGenerator.Instance;
				RunnableExecuter.Instance.execute(elementalStoneGenerator);
			}

			// 初始化 HomeTown 時間
			HomeTownTimeController.Instance;

			// 初始化盟屋拍賣
			AuctionTimeController auctionTimeController = AuctionTimeController.Instance;
			RunnableExecuter.Instance.execute(auctionTimeController);

			// 初始化盟屋的稅金
			HouseTaxTimeController houseTaxTimeController = HouseTaxTimeController.Instance;
			RunnableExecuter.Instance.execute(houseTaxTimeController);

			// 初始化釣魚
			FishingTimeController fishingTimeController = FishingTimeController.Instance;
			RunnableExecuter.Instance.execute(fishingTimeController);

			// 初始化 NPC 聊天
			NpcChatTimeController npcChatTimeController = NpcChatTimeController.Instance;
			RunnableExecuter.Instance.execute(npcChatTimeController);

			// 初始化 Light
			LightTimeController lightTimeController = LightTimeController.Instance;
			RunnableExecuter.Instance.execute(lightTimeController);
			// TODO 殷海薩的祝福
			AinTimeController ainTimeController = AinTimeController.Instance;
			RunnableExecuter.Instance.execute(ainTimeController);
			// 初始化遊戲公告
			Announcements.Instance;

			// 初始化MySQL自動備份程序
			MysqlAutoBackup.Instance;

			// 開始 MySQL自動備份程序 計時器
			MysqlAutoBackupTimer.TimerStart();

			// 初始化帳號使用狀態
			Account.InitialOnlineStatus();

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
				Announcecycle.Instance;
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
			Runtime.Runtime.addShutdownHook(Shutdown.Instance);

			// cmd互動指令
			Thread cp = new ConsoleProcess();
			cp.Start();

			this.Start();
		}

		/// <summary>
		/// 踢掉世界地圖中所有的玩家與儲存資料。
		/// </summary>
		public virtual void disconnectAllCharacters()
		{
			ICollection<L1PcInstance> players = L1World.Instance.AllPlayers;
			foreach (L1PcInstance pc in players)
			{
				pc.NetConnection.ActiveChar = null;
				pc.NetConnection.kick();
			}
			// 踢除所有在線上的玩家
			foreach (L1PcInstance pc in players)
			{
				ClientThread.quitGame(pc);
				L1World.Instance.removeObject(pc);
				Account account = Account.Load(pc.AccountName);
				Account.SetOnline(account, false);
			}
		}

		private class ServerShutdownThread : IRunnable
		{
			private readonly GameServerOri outerInstance;

			internal readonly int _secondsCount;

			public ServerShutdownThread(GameServerOri outerInstance, int secondsCount)
			{
				this.outerInstance = outerInstance;
				_secondsCount = secondsCount;
			}

			public override void run()
			{
				L1World world = L1World.Instance;
				try
				{
					int secondsCount = _secondsCount;
					world.broadcastServerMessage("伺服器即將關閉。");
					world.broadcastServerMessage("請玩家移動到安全區域先行登出");
					while (0 < secondsCount)
					{
						if (secondsCount <= 30)
						{
							world.broadcastServerMessage("伺服器將在" + secondsCount + "秒後關閉，請玩家移動到安全區域先行登出。");
						}
						else
						{
							if (secondsCount % 60 == 0)
							{
								world.broadcastServerMessage("伺服器將在" + secondsCount / 60 + "分鐘後關閉。");
							}
						}
						Thread.Sleep(1000);
						secondsCount--;
					}
					outerInstance.shutdown();
				}
				catch (InterruptedException)
				{
					world.broadcastServerMessage("已取消伺服器關機。伺服器將會正常運作。");
					return;
				}
			}
		}

		private ServerShutdownThread _shutdownThread = null;

		public virtual void shutdownWithCountdown(int secondsCount)
		{
			lock (this)
			{
				if (_shutdownThread != null)
				{
					// 如果正在關閉
					// TODO 可能要有錯誤通知之類的
					return;
				}
				_shutdownThread = new ServerShutdownThread(this, secondsCount);
				RunnableExecuter.Instance.execute(_shutdownThread);
			}
		}

		public virtual void shutdown()
		{
			disconnectAllCharacters();
			Environment.Exit(0);
		}

		public virtual void abortShutdown()
		{
			lock (this)
			{
				if (_shutdownThread == null)
				{
					// 如果正在關閉
					// TODO 可能要有錯誤通知之類的
					return;
				}
        
				_shutdownThread.Interrupt();
				_shutdownThread = null;
			}
		}

		/// <summary>
		/// 取得世界中發送YesNo總次數 </summary>
		/// <returns> YesNo總次數 </returns>
		public static int YesNoCount
		{
			get
			{
				YesNoCount_Conflict += 1;
				return YesNoCount_Conflict;
			}
		}
	}

}