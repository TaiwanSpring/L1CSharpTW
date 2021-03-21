using System;
using System.Threading;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.Model.game
{

	using IdFactory = LineageServer.Server.Server.IdFactory;
	using DoorTable = LineageServer.Server.Server.datatables.DoorTable;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using RaceTicketTable = LineageServer.Server.Server.datatables.RaceTicketTable;
	using ShopTable = LineageServer.Server.Server.datatables.ShopTable;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1DoorInstance = LineageServer.Server.Server.Model.Instance.L1DoorInstance;
	using L1MerchantInstance = LineageServer.Server.Server.Model.Instance.L1MerchantInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Shop = LineageServer.Server.Server.Model.shop.L1Shop;
	using S_NPCPack = LineageServer.Server.Server.serverpackets.S_NPCPack;
	using S_NpcChatPacket = LineageServer.Server.Server.serverpackets.S_NpcChatPacket;
	using L1RaceTicket = LineageServer.Server.Server.Templates.L1RaceTicket;
	using L1ShopItem = LineageServer.Server.Server.Templates.L1ShopItem;

	public class L1BugBearRace
	{
		internal L1MerchantInstance pory;
		internal L1MerchantInstance cecile;
		internal L1MerchantInstance parkin;
		private const int FIRST_ID = 0x0000000;
		private const int STATUS_NONE = 0;
		private const int STATUS_READY = 1;
		private const int STATUS_PLAYING = 2;
		private const int STATUS_END = 3;
		private const int WAIT_TIME = 60;
		private const int READY_TIME = 9 * 60 - 10; // test 60;//
		private const int FIRST_NPCID = 91350; // ~20
		private L1NpcInstance[] _runner;
		private int[] _runnerStatus = new int[5];
		private double[] _winning_average = new double[5];
		private double[] _allotment_percentage = new double[5];
		private int[] _condition = new int[5];
		private int _allBet;
		private int[] _betCount = new int[5];

		private int _round;

		public virtual int Round
		{
			get
			{
				return _round;
			}
			set
			{
				this._round = value;
			}
		}


		private static Random _random = new Random();

		private static L1BugBearRace instance;

		public static L1BugBearRace Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new L1BugBearRace();
				}
				return instance;
			}
		}

		internal L1BugBearRace()
		{
			Round = RaceTicketTable.Instance.RoundNumOfMax;
			_runner = new L1NpcInstance[5];
			foreach (L1Object obj in L1World.Instance.Object)
			{
				if (obj is L1MerchantInstance)
				{
					if (((L1MerchantInstance) obj).NpcId == 70041)
					{
						parkin = (L1MerchantInstance) obj;
					}
				}
			}
			foreach (L1Object obj in L1World.Instance.Object)
			{
				if (obj is L1MerchantInstance)
				{
					if (((L1MerchantInstance) obj).NpcId == 70035)
					{
						cecile = (L1MerchantInstance) obj;
					}
				}
			}
			foreach (L1Object obj in L1World.Instance.Object)
			{
				if (obj is L1MerchantInstance)
				{
					if (((L1MerchantInstance) obj).NpcId == 70042)
					{
						pory = (L1MerchantInstance) obj;
					}
				}
			}
			(new RaceTimer(this, 0)).begin();
		}

		private void setRandomRunner()
		{
			for (int i = 0; i < 5; i++)
			{
				int npcid = FIRST_NPCID + _RandomHelper.Next(20);
				while (checkDuplicate(npcid, i))
				{
					npcid = FIRST_NPCID + _RandomHelper.Next(20);
				}
				L1Location loc = new L1Location(33522 - (i * 2), 32861 + (i * 2), 4);
				_runner[i] = spawnOne(loc, npcid, 6);

			}
		}

		private bool checkDuplicate(int npcid, int curi)
		{
			for (int i = 0; i < curi; i++)
			{
				if (_runner[i] != null)
				{
					if (_runner[i].NpcId == npcid)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void clearRunner()
		{
			for (int i = 0; i < 5; i++)
			{
				if (_runner[i] != null)
				{
					_runner[i].deleteMe();
					if (_runner[i].Map.isInMap(_runner[i].X, _runner[i].Y))
					{
						_runner[i].Map.setPassable(_runner[i].X, _runner[i].Y, true);
					}
				}
				_runner[i] = null;
				_runnerStatus[i] = 0;
				_condition[i] = 0;
				_winning_average[i] = 0;
				_allotment_percentage[i] = 0.0;
				setBetCount(i, 0);
			}
			AllBet = 0;
			foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
			{
				if (door.DoorId <= 812 && door.DoorId >= 808)
				{
					door.close();
				}
			}
		}

		public virtual bool checkPosition(int runnerNumber)
		{ // 現在のポジションを確認
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] defaultHead = { 6, 7, 0, 1, 2, 2 };
			int[] defaultHead = new int[] {6, 7, 0, 1, 2, 2};
			if (GameStatus != STATUS_PLAYING)
			{
				return false;
			}
			bool flag = false; // ゴールするまではfalseを返す
			L1NpcInstance npc = _runner[runnerNumber];
			int x = npc.X;
			int y = npc.Y;
			if (_runnerStatus[runnerNumber] == 0)
			{ // スタート　直線
				if ((x >= 33476 && x <= 33476 + 8) && (y >= 32861 && y <= 32861 + 8))
				{
					_runnerStatus[runnerNumber] = _runnerStatus[runnerNumber] + 1;
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを変更
				}
				else
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを復元
				}
			}
			else if (_runnerStatus[runnerNumber] == 1)
			{
				if ((x <= 33473 && x >= 33473 - 9) && y == 32858)
				{
					_runnerStatus[runnerNumber] = _runnerStatus[runnerNumber] + 1;
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを変更
				}
				else
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを復元
				}
			}
			else if (_runnerStatus[runnerNumber] == 2)
			{
				if ((x <= 33473 && x >= 33473 - 9) && y == 32852)
				{
					_runnerStatus[runnerNumber] = _runnerStatus[runnerNumber] + 1;
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを変更
				}
				else
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを復元
				}
			}
			else if (_runnerStatus[runnerNumber] == 3)
			{
				if ((x == 33478 && (y <= 32847 && y >= 32847 - 9)))
				{
					_runnerStatus[runnerNumber] = _runnerStatus[runnerNumber] + 1;
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを変更
				}
				else
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを復元
				}
			}
			else if (_runnerStatus[runnerNumber] == 4)
			{
				if (x == 33523 && (y >= 32847 - 9 && y <= 32847))
				{
					_runnerStatus[runnerNumber] = _runnerStatus[runnerNumber] + 1;
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを変更
					// goal
					goal(runnerNumber);
				}
				else
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを復元
				}
			}
			else if (_runnerStatus[runnerNumber] == 5)
			{
				if (x == 33527 && (y >= 32847 - 8 && y <= 32847))
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを変更
					finish();
					flag = true;
				}
				else
				{
					npc.Heading = defaultHead[_runnerStatus[runnerNumber]]; // ヘッジを復元
				}
			}
			return flag;
		}

		private void finish()
		{
			int cnt = 0;
			for (int i = 0; i < _runnerStatus.Length; i++)
			{
				if (_runnerStatus[i] == 5)
				{
					cnt++;
				}
			}
			if (cnt == 5)
			{
				GameStatus = STATUS_END;
				(new RaceTimer(this, 30)).begin();
				/* SHOP格納処理 */

			}
		}

		private void goal(int runnberNumber)
		{
			int cnt = 0;
			for (int i = 0; i < _runnerStatus.Length; i++)
			{
				if (_runnerStatus[i] == 5)
				{
					cnt++;
				}
			}
			if (cnt == 1)
			{
				cecile.wideBroadcastPacket(new S_NpcChatPacket(cecile, "第 " + Round + " $366 " + NpcTable.Instance.getTemplate(_runner[runnberNumber].NpcId).get_nameid() + " $367", 2)); // 5>3バイト
				/* DB格納処理 */
				RaceTicketTable.Instance.updateTicket(Round, _runner[runnberNumber].NpcId - FIRST_NPCID + 1, _allotment_percentage[runnberNumber]);
			}
		}

		// TODO 周回数判定処理　end

		private int _status = 0;

		public virtual int GameStatus
		{
			set
			{
				_status = value;
			}
			get
			{
				return _status;
			}
		}


		private void sendMessage(string id)
		{
			parkin.wideBroadcastPacket(new S_NpcChatPacket(parkin, id, 2));
			// cecile.broadcastPacket(new S_NpcChatPacket(cecile,id, 2));
			pory.wideBroadcastPacket(new S_NpcChatPacket(pory, id, 2));
		}

		private class RaceTimer : TimerTask
		{
			private readonly L1BugBearRace outerInstance;

			internal int _startTime;

			internal RaceTimer(L1BugBearRace outerInstance, int startTime)
			{
				this.outerInstance = outerInstance;
				_startTime = startTime;
			}

			public override void run()
			{

				try
				{
					// ゲームのステータスをNONEに（10分前）
					outerInstance.GameStatus = STATUS_NONE;
					outerInstance.sendMessage("$376 10 $377");
					for (int loop = 0; loop < WAIT_TIME; loop++)
					{
						Thread.Sleep(1000);
					}
					outerInstance.clearRunner();
					outerInstance.Round = outerInstance.Round + 1;
					/* レース回数-記録処理 */
					L1RaceTicket ticket = new L1RaceTicket();
					ticket.set_itemobjid(FIRST_ID); // 重複可能
					ticket.set_allotment_percentage(0);
					ticket.set_round(outerInstance.Round);
					ticket.set_runner_num(0);
					ticket.set_victory(0);
					RaceTicketTable.Instance.storeNewTiket(ticket); // 記録用
					RaceTicketTable.Instance.oldTicketDelete(outerInstance.Round); // 古い記録を削除
					outerInstance.setRandomRunner(); // ランナー準備
					outerInstance.setRandomCondition();
					/* SHOP BuyList格納処理 */
					L1Shop shop1 = ShopTable.Instance.get(70035);
					L1Shop shop2 = ShopTable.Instance.get(70041);
					L1Shop shop3 = ShopTable.Instance.get(70042);
					for (int i = 0; i < 5; i++)
					{
						L1ShopItem shopItem1 = new L1ShopItem(40309, 500, 1);
						shopItem1.Name = i;
						L1ShopItem shopItem2 = new L1ShopItem(40309, 500, 1);
						shopItem2.Name = i;
						L1ShopItem shopItem3 = new L1ShopItem(40309, 500, 1);
						shopItem3.Name = i;
						shop1.SellingItems.Add(shopItem1);
						shop2.SellingItems.Add(shopItem2);
						shop3.SellingItems.Add(shopItem3);
					}
					outerInstance.setWinnigAverage();
					outerInstance.GameStatus = STATUS_READY;
					for (int loop = 0; loop < READY_TIME - 1; loop++)
					{
						if (loop % 60 == 0)
						{
							outerInstance.sendMessage("$376 " + (1 + (READY_TIME - loop) / 60) + " $377");
						}
						Thread.Sleep(1000);
					}
					outerInstance.sendMessage("$363"); // 363 レディー！
					Thread.Sleep(1000);
					for (int loop = 10; loop > 0; loop--)
					{
						outerInstance.sendMessage("" + loop);
						Thread.Sleep(1000);
					}
					outerInstance.sendMessage("$364"); // 364 ゴー！
					outerInstance.GameStatus = STATUS_PLAYING;
					/* SHOP BuyListから削除 */
					shop1.SellingItems.Clear();
					shop2.SellingItems.Clear();
					shop3.SellingItems.Clear();
					foreach (L1DoorInstance door in DoorTable.Instance.DoorList)
					{
						if (door.DoorId <= 812 && door.DoorId >= 808)
						{
							door.open();
						}
					}
					for (int i = 0; i < outerInstance._runner.Length; i++)
					{
						(new BugBearRunning(outerInstance, i)).begin(0);
					}

					(new StartBuffTimer(outerInstance)).begin();

					for (int i = 0; i < outerInstance._runner.Length; i++)
					{
						if (outerInstance.getBetCount(i) > 0)
						{
							outerInstance._allotment_percentage[i] = (double)(outerInstance.AllBet / (outerInstance.getBetCount(i)) / 500D);
						}
						else
						{
							outerInstance._allotment_percentage[i] = 0.0;
						}
					}
					for (int i = 0; i < outerInstance._runner.Length; i++)
					{
						Thread.Sleep(1000);
						outerInstance.sendMessage(NpcTable.Instance.getTemplate(outerInstance._runner[i].NpcId).get_nameid() + " $402 " + outerInstance._allotment_percentage[i].ToString()); // 402
																			// の配当率は
					}
					this.cancel();
				}
				catch (InterruptedException e)
				{
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
				}

			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, _startTime * 1000);
			}
		}

		private class BugBearRunning : TimerTask
		{
			private readonly L1BugBearRace outerInstance;

			internal L1NpcInstance _bugBear;
			internal int _runnerNumber;

			internal BugBearRunning(L1BugBearRace outerInstance, int runnerNumber)
			{
				this.outerInstance = outerInstance;
				_bugBear = outerInstance._runner[runnerNumber];
				_runnerNumber = runnerNumber;
			}

			public override void run()
			{
				int sleepTime = 0;
				while (outerInstance.GameStatus == STATUS_PLAYING)
				{
					try
					{
						Thread.Sleep(sleepTime);
					}
					catch (InterruptedException e)
					{
                        System.Console.WriteLine(e.ToString());
                        System.Console.Write(e.StackTrace);
					}
					while (!_bugBear.Map.isPassable(_bugBear.X, _bugBear.Y, _bugBear.Heading))
					{
						if (_bugBear.Map.isPassable(_bugBear.X, _bugBear.Y, _bugBear.Heading + 1))
						{
							_bugBear.Heading = outerInstance.rePressHeading(_bugBear.Heading + 1);
							break;
						}
						else
						{
							_bugBear.Heading = outerInstance.rePressHeading(_bugBear.Heading - 1);
							if (_bugBear.Map.isPassable(_bugBear.X, _bugBear.Y, _bugBear.Heading))
							{
								break;
							}
						}
					}
					_bugBear.DirectionMove = _bugBear.Heading; // ヘッジ方向
					if (outerInstance.checkPosition(_runnerNumber))
					{
						_bugBear = null;
						return;
					}
					else
					{
						// new BugBearRunning(_runnerNumber).
						// インスタンスを生成しないでください　メモリリークが発生します
						sleepTime = outerInstance.calcSleepTime(_bugBear.Passispeed, _runnerNumber);
					}
				}
			}

			public virtual void begin(int startTime)
			{
				Timer _timer = new Timer();
				_timer.schedule(this, startTime);
			}
		}

		private int rePressHeading(int heading)
		{
			if (0 > heading)
			{ // 0未満ならば7
				heading = 7;
			}
			if (7 < heading)
			{ // 7より大きいなら0
				heading = 0;
			}
			return heading;
		}

		/// <summary>
		/// 指定されたロケーションに任意のNpcを一匹生成する。
		/// </summary>
		/// <param name="loc">
		///            出現位置 </param>
		/// <param name="npcid">
		///            任意のNpcId </param>
		/// <param name="heading">
		///            向き </param>
		/// <returns> L1NpcInstance 戻り値 : 成功=生成したインスタンス 失敗=null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") private l1j.server.server.model.Instance.L1NpcInstance spawnOne(l1j.server.server.model.L1Location loc, int npcid, int heading)
		private L1NpcInstance spawnOne(L1Location loc, int npcid, int heading)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.model.Instance.L1NpcInstance mob = new l1j.server.server.model.Instance.L1NpcInstance(l1j.server.server.datatables.NpcTable.getInstance().getTemplate(npcid));
			L1NpcInstance mob = new L1NpcInstance(NpcTable.Instance.getTemplate(npcid));
			if (mob == null)
			{
				return mob;
			}

			mob.NameId = "#" + (mob.NpcId - FIRST_NPCID + 1) + " " + mob.NameId;
			mob.Id = IdFactory.Instance.nextId();
			mob.Heading = heading;
			mob.X = loc.X;
			mob.HomeX = loc.X;
			mob.Y = loc.Y;
			mob.HomeY = loc.Y;
			mob.Map = (short) loc.MapId;
			mob.Passispeed = mob.Passispeed * 2;
			L1World.Instance.storeObject(mob);
			L1World.Instance.addVisibleObject(mob);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final l1j.server.server.serverpackets.S_NPCPack s_npcPack = new l1j.server.server.serverpackets.S_NPCPack(mob);
			S_NPCPack s_npcPack = new S_NPCPack(mob);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(mob))
			{
				pc.addKnownObject(mob);
				mob.addKnownObject(pc);
				pc.sendPackets(s_npcPack);
			}
			// モンスターのＡＩを開始
			mob.onNpcAI();
			mob.turnOnOffLight();
			mob.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
			return mob;
		}

		public virtual int AllBet
		{
			set
			{ // allbetは3桁の整数
				this._allBet = (int)(value * 0.9); // 1割は管理人が取得
			}
			get
			{
				return _allBet;
			}
		}


		public virtual int getBetCount(int i)
		{
			return _betCount[i];
		}

		public virtual void setBetCount(int i, int count)
		{
			_betCount[i] = count;
		}

		private int calcSleepTime(int sleepTime, int runnerNumber)
		{
			L1NpcInstance npc = _runner[runnerNumber];
			if (npc.BraveSpeed == 1)
			{
				sleepTime -= (int)(sleepTime * 0.25);
			}

			return sleepTime;
		}

		private class StartBuffTimer : TimerTask
		{
			private readonly L1BugBearRace outerInstance;

			internal StartBuffTimer(L1BugBearRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				if (outerInstance.GameStatus == STATUS_PLAYING)
				{
					for (int i = 0; i < outerInstance._runner.Length; i++)
					{
						if (outerInstance.RandomProbability <= outerInstance._winning_average[i] * (1 + (0.2 * outerInstance.getCondition(i))))
						{
							outerInstance._runner[i].BraveSpeed = 1;
						}
						else
						{
							outerInstance._runner[i].BraveSpeed = 0;
						}
					}
				}
				else
				{
					this.cancel();
				}
			}

			public virtual void begin()
			{
				Timer _timer = new Timer();
				_timer.scheduleAtFixedRate(this, 1000, 1000);
			}
		}

		private double RandomProbability
		{
			get
			{
				return (_RandomHelper.Next(10000) + 1) / 100D;
			}
		}
		private void setWinnigAverage()
		{
			for (int i = 0; i < _winning_average.Length ; i++)
			{
				double winningAverage = RandomProbability;

				while (checkDuplicateAverage(winningAverage, i))
				{
					winningAverage = RandomProbability;
				}
				_winning_average[i] = winningAverage;
			}
		}

		private bool checkDuplicateAverage(double winning_average, int curi)
		{
			// 勝率跟狀態都一樣算重覆
			for (int i = 0; i < curi; i++)
			{
				if (_winning_average[i] == winning_average && _condition[i] == _condition[curi])
				{
					return true;
				}
			}
			return false;
		}

		/*
		 * private void setCondition(int num, int condition) { this._condition[num]
		 * = condition; }
		 */

		public virtual int getCondition(int num)
		{
			return _condition[num];
		}

		private void setRandomCondition()
		{
			for (int i = 0; i < _condition.Length; i++)
			{
				_condition[i] = -1 + _RandomHelper.Next(3);
			}
		}

		public virtual L1NpcInstance getRunner(int num)
		{
			return _runner[num];
		}

		public virtual double getWinningAverage(int num)
		{
			return _winning_average[num];
		}
	}

}