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

	using FastTable = javolution.util.FastTable;
	using DoorTable = LineageServer.Server.Server.datatables.DoorTable;
	using ItemTable = LineageServer.Server.Server.datatables.ItemTable;
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1PolyMorph = LineageServer.Server.Server.Model.L1PolyMorph;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1DoorInstance = LineageServer.Server.Server.Model.Instance.L1DoorInstance;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1ItemId = LineageServer.Server.Server.Model.identity.L1ItemId;
	using L1SkillId = LineageServer.Server.Server.Model.skill.L1SkillId;
	using L1SkillUse = LineageServer.Server.Server.Model.skill.L1SkillUse;
	using S_EffectLocation = LineageServer.Server.Server.serverpackets.S_EffectLocation;
	using S_Message_YN = LineageServer.Server.Server.serverpackets.S_Message_YN;
	using S_Race = LineageServer.Server.Server.serverpackets.S_Race;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SkillBrave = LineageServer.Server.Server.serverpackets.S_SkillBrave;
	using S_SkillHaste = LineageServer.Server.Server.serverpackets.S_SkillHaste;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using Random = LineageServer.Server.Server.utils.Random;


	public class L1PolyRace
	{

		/// <summary>
		///*
		/// [變身清單] 資料提供 CTKI 有錯請去幹蹻他 :)
		/// </summary>
		private int[] polyList = new int[] {936, 3134, 1642, 931, 96, 4038, 938, 929, 1540, 3783, 2145, 934, 3918, 3199, 3184, 3132, 3107, 3188, 3211, 3143, 3182, 3156, 3154, 3178, 4133, 5089, 945, 4171, 2541, 2001, 1649, 29};

		private static L1PolyRace instance;

		public static L1PolyRace Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new L1PolyRace();
				}
				return instance;
			}
		}

		public const int STATUS_NONE = 0;
		public const int STATUS_READY = 1;
		public const int STATUS_PLAYING = 2;
		public const int STATUS_END = 3;

		private const int maxLap = 4; //遊戲圈數 最小:1 最大:你高興
		private const int maxPlayer = 10; //最大玩家數 1~20
		private const int minPlayer = 2; //最小玩家數

		private static int readyTime = 60 * 1000; //進場之後等待時間 60秒
		private static int limitTime = 240 * 1000; //遊戲時間 240秒

		private FastTable<L1PcInstance> playerList = new FastTable<L1PcInstance>();

		public virtual void addPlayerList(L1PcInstance pc)
		{
			if (!playerList.contains(pc))
			{
				playerList.add(pc);
			}
		}

		public virtual void removePlayerList(L1PcInstance pc)
		{
			if (playerList.contains(pc))
			{
				playerList.remove(pc);
			}
		}

		public virtual void enterGame(L1PcInstance pc)
		{
			if (pc.Level < 30)
			{
				pc.sendPackets(new S_ServerMessage(1273,"30","99"));
				return;
			}
			if (!pc.Inventory.consumeItem(L1ItemId.ADENA, 1000))
			{
				pc.sendPackets(new S_ServerMessage(189)); //金錢不足
				return;
			}
			if (playerList.size() + orderList.size() >= maxPlayer)
			{
				pc.sendPackets(new S_SystemMessage("遊戲人數已達上限"));
				return;
			}
			if (GameStatus == STATUS_PLAYING || GameStatus == STATUS_END)
			{
				pc.sendPackets(new S_ServerMessage(1182)); //遊戲已經開始了。
				return;
			}
			if (GameStatus == STATUS_NONE)
			{
				addOrderList(pc);
				return;
			}

			addPlayerList(pc);
			L1Teleport.teleport(pc, 32768, 32849, (short) 5143, 6, true);
		}

		private FastTable<L1PcInstance> orderList = new FastTable<L1PcInstance>();

		public virtual void removeOrderList(L1PcInstance pc)
		{
			orderList.remove(pc);
		}

		//預約進場...試做1
		public virtual void addOrderList(L1PcInstance pc)
		{
			if (orderList.contains(pc))
			{
				pc.sendPackets(new S_ServerMessage(1254));
				return;
			}
			orderList.add(pc);
			pc.InOrderList = true;
			pc.sendPackets(new S_ServerMessage(1253, orderList.size().ToString())); //已預約到第%0順位進入比賽場地。

			if (orderList.size() >= minPlayer)
			{
				foreach (L1PcInstance player in orderList)
				{
					player.sendPackets(new S_Message_YN(1256, null)); //要進入到競賽場地嗎？(Y/N)
				}
				GameStatus = STATUS_READY;
				startReadyTimer();
			}
		}

		private bool checkPlayersOK()
		{
			if (GameStatus == STATUS_READY)
			{
				return playerList.size() >= minPlayer;
			}
			return false;
		}

		private void setGameStart()
		{
			GameStatus = STATUS_PLAYING;
			foreach (L1PcInstance pc in playerList)
			{
				speedUp(pc, 0, 0);
				randomPoly(pc, 0, 0);
				pc.sendPackets(new S_ServerMessage(1257)); //稍後比賽即將開始，請做好準備。
				pc.sendPackets(new S_Race(S_Race.GameStart)); //5.4.3.2.1.GO!
				pc.sendPackets(new S_Race(maxLap, pc.Lap)); //圈數
				pc.sendPackets(new S_Race(playerList, pc)); //玩家名單
			}
			startCompareTimer();
			startClockTimer();
		}

		private L1PcInstance GameWinner
		{
			set
			{
				if (Winner == null)
				{
					Winner = value;
					GameEnd = END_STATUS_WINNER;
				}
			}
		}

		private const int END_STATUS_WINNER = 1;
		private const int END_STATUS_NOWINNER = 2;
		private const int END_STATUS_NOPLAYER = 3;

		/// <summary>
		/// 三種情況 1:有勝利者   2:時間到沒人贏   3:人數不足 </summary>
		/// <param name="type"> 情況 </param>
		private int GameEnd
		{
			set
			{
				GameStatus = STATUS_END;
				switch (value)
				{
					case END_STATUS_WINNER:
						stopCompareTimer();
						stopGameTimeLimitTimer();
						sendEndMessage();
					break;
					case END_STATUS_NOWINNER:
						stopCompareTimer();
						sendEndMessage();
					break;
					case END_STATUS_NOPLAYER:
						foreach (L1PcInstance pc in playerList)
						{
							//未達到比賽最低人數(2人)，因此強制關閉比賽並退還1000個金幣。
							pc.sendPackets(new S_ServerMessage(1264));
							pc.Inventory.storeItem(L1ItemId.ADENA, 1000);
						}
					break;
				}
				startEndTimer(); //5秒後傳回村
			}
		}

		private void giftWinner()
		{
			L1PcInstance winner = Winner;
			L1ItemInstance item = ItemTable.Instance.createItem(41308);
			if (winner == null || item == null)
			{
				return;
			}
			if (winner.Inventory.checkAddItem(item, 1) == L1Inventory.OK)
			{
				item.Count = 1;
				winner.Inventory.storeItem(item);
				winner.sendPackets(new S_ServerMessage(403, item.LogName));
			}
		}

		private void sendEndMessage()
		{
			L1PcInstance winner = Winner;
			foreach (L1PcInstance pc in playerList)
			{
				if (winner != null)
				{
					pc.sendPackets(new S_ServerMessage(1259)); //稍後將往村莊移動。
					pc.sendPackets(new S_Race(winner.Name, _time * 2));
					continue;
				}
				pc.sendPackets(new S_Race(S_Race.GameOver));
			}
		}

		//初始化 + 下一場準備
		private void setGameInit()
		{
			foreach (L1PcInstance pc in playerList)
			{
				pc.sendPackets(new S_Race(S_Race.GameEnd));
				pc.Lap = 1;
				pc.LapCheck = 0;
				L1Teleport.teleport(pc, 32616, 32782, (short) 4, 5, true);
				removeSkillEffect(pc);
			}
			DoorClose = true;
			GameStatus = STATUS_NONE;
			Winner = null;
			playerList.clear();
			clearTime();
		}

		//XXX for ClientThread.java
		public virtual void checkLeaveGame(L1PcInstance pc)
		{
			if (pc.MapId == 5143)
			{
				removePlayerList(pc);
				L1PolyMorph.undoPoly(pc);
			}
			if (pc.InOrderList)
			{
				removeOrderList(pc);
			}
		}

		//XXX for C_Attr.java
		public virtual void requsetAttr(L1PcInstance pc, int c)
		{
			if (c == 0)
			{ //NO
				removeOrderList(pc);
				pc.InOrderList = false;
				pc.sendPackets(new S_ServerMessage(1255));
			}
			else
			{ //YES
				addPlayerList(pc);
				L1Teleport.teleport(pc, 32768, 32849, (short) 5143, 6, true);
				removeSkillEffect(pc);
				removeOrderList(pc);
				pc.InOrderList = false;
			}
		}

		private FastTable<L1PcInstance> position = new FastTable<L1PcInstance>();

		//判斷排名
		private void comparePosition()
		{
			FastTable<L1PcInstance> temp = new FastTable<L1PcInstance>();
			int size = playerList.size();
			int count = 0;
			while (size > count)
			{
				int maxLapScore = 0;
				foreach (L1PcInstance pc in playerList)
				{
					if (temp.contains(pc))
					{
						continue;
					}
					if (pc.LapScore >= maxLapScore)
					{
						maxLapScore = pc.LapScore;
					}
				}
				foreach (L1PcInstance player in playerList)
				{
					if (player.LapScore == maxLapScore)
					{
						temp.add(player);
					}
				}
				count++;
			}
			if (!position.Equals(temp))
			{
				position.clear();
				position.addAll(temp);
				foreach (L1PcInstance pc in playerList)
				{
					pc.sendPackets(new S_Race(position, pc)); //info
				}
			}
		}

		private bool DoorClose
		{
			set
			{
				L1DoorInstance[] list = DoorTable.Instance.DoorList;
				foreach (L1DoorInstance door in list)
				{
					if (door.MapId == 5143)
					{
						if (value)
						{
							door.close();
						}
						else
						{
							door.open();
						}
					}
				}
			}
		}

		public virtual void removeSkillEffect(L1PcInstance pc)
		{
			L1SkillUse skill = new L1SkillUse();
			skill.handleCommands(pc, L1SkillId.CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
		}

		//很蠢的陷阱設定 ...
		private void onEffectTrap(L1PcInstance pc)
		{
			int x = pc.X;
			int y = pc.Y;
			if (x == 32748 && (y == 32845 || y == 32846))
			{
				speedUp(pc, 32748, 32845);
			}
			else if (x == 32748 && (y == 32847 || y == 32848))
			{
				speedUp(pc, 32748, 32847);
			}
			else if (x == 32748 && (y == 32849 || y == 32850))
			{
				speedUp(pc, 32748, 32849);
			}
			else if (x == 32748 && y == 32851)
			{
				speedUp(pc, 32748, 32851);
			}
			else if (x == 32762 && (y == 32811 || y == 32812))
			{
				speedUp(pc, 32762, 32811);
			}
			else if ((x == 32799 || x == 32800) && y == 32830)
			{
				speedUp(pc, 32800, 32830);
			}
			else if ((x == 32736 || x == 32737) && y == 32840)
			{
				randomPoly(pc, 32737, 32840);
			}
			else if ((x == 32738 || x == 32739) && y == 32840)
			{
				randomPoly(pc, 32739, 32840);
			}
			else if ((x == 32740 || x == 32741) && y == 32840)
			{
				randomPoly(pc, 32741, 32840);
			}
			else if (x == 32749 && (y == 32818 || y == 32817))
			{
				randomPoly(pc, 32749, 32817);
			}
			else if (x == 32749 && (y == 32816 || y == 32815))
			{
				randomPoly(pc, 32749, 32815);
			}
			else if (x == 32749 && (y == 32814 || y == 32813))
			{
				randomPoly(pc, 32749, 32813);
			}
			else if (x == 32749 && (y == 32812 || y == 32811))
			{
				randomPoly(pc, 32749, 32811);
			}
			else if (x == 32790 && (y == 32812 || y == 32813))
			{
				randomPoly(pc, 32790, 32812);
			}
			else if ((x == 32793 || x == 32794) && y == 32831)
			{
				randomPoly(pc, 32794, 32831);
			}
		}

		private static int POLY_EFFECT = 15566;
		private static int SPEED_EFFECT = 18333;

		//變身效果
		private void randomPoly(L1PcInstance pc, int x, int y)
		{
			if (pc.hasSkillEffect(L1SkillId.POLY_EFFECT))
			{
				return;
			}
			pc.setSkillEffect(POLY_EFFECT, 4 * 1000);

			int i = RandomHelper.Next(polyList.Length);
			L1PolyMorph.doPoly(pc, polyList[i], 3600, L1PolyMorph.MORPH_BY_NPC);

			foreach (L1PcInstance player in playerList)
			{
				player.sendPackets(new S_EffectLocation(x, y, 6675));
			}
		}

		//加速效果
		private void speedUp(L1PcInstance pc, int x, int y)
		{
			if (pc.hasSkillEffect(L1SkillId.SPEED_EFFECT))
			{
				return;
			}
			pc.setSkillEffect(SPEED_EFFECT, 4 * 1000);
			int time = 15;
			int objectId = pc.Id;
			//競速專用 -超級加速
			pc.sendPackets(new S_SkillBrave(objectId, 5, time));
			pc.broadcastPacket(new S_SkillBrave(objectId, 5, time));
			pc.setSkillEffect(L1SkillId.STATUS_BRAVE2, time * 1000);
			pc.BraveSpeed = 5;
			/// <summary>
			/// XXX 注意!加速效果必須給同畫面的人知道 否則會造成錯位!!! pc.broadcastPacket(new
			/// S_SkillBrave(objectId, 5, time))!!!
			/// </summary>
			pc.sendPackets(new S_SkillHaste(objectId, 1, time * 10));
			pc.setSkillEffect(L1SkillId.STATUS_HASTE, time * 10 * 1000);
			pc.MoveSpeed = 1;

			foreach (L1PcInstance player in playerList)
			{
				player.sendPackets(new S_EffectLocation(x, y, 6674));
			}
		}

		//很蠢的判斷圈數...
		public virtual void checkLapFinish(L1PcInstance pc)
		{
			if (pc.MapId != 5143 || GameStatus != STATUS_PLAYING)
			{
				return;
			}

			onEffectTrap(pc);
			int x = pc.X;
			int y = pc.Y;
			int check = pc.LapCheck;

			if (x == 32762 && y >= 32845 && check == 0)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32754 && y >= 32845 && check == 1)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32748 && y >= 32845 && check == 2)
			{
				pc.LapCheck = check + 1;
			}
			else if (x <= 32743 && y == 32844 && check == 3)
			{
				pc.LapCheck = check + 1;
			}
			else if (x <= 32742 && y == 32840 && check == 4)
			{
				pc.LapCheck = check + 1;
			}
			else if (x <= 32742 && y == 32835 && check == 5)
			{
				pc.LapCheck = check + 1;
			}
			else if (x <= 32742 && y == 32830 && check == 6)
			{
				pc.LapCheck = check + 1;
			}
			else if (x <= 32742 && y == 32826 && check == 7)
			{
				pc.LapCheck = check + 1;
			}
			else if (x <= 32742 && y == 32822 && check == 8)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32749 && y <= 32818 && check == 9)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32755 && y <= 32818 && check == 10)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32760 && y <= 32818 && check == 11)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32765 && y <= 32818 && check == 12)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32770 && y <= 32818 && check == 13)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32775 && y <= 32818 && check == 14)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32780 && y <= 32818 && check == 15)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32785 && y <= 32818 && check == 16)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32789 && y <= 32818 && check == 17)
			{
				pc.LapCheck = check + 1;
			}
			else if (x >= 32792 && y == 32821 && check == 18)
			{
				pc.LapCheck = check + 1;
			}
			else if (x >= 32793 && y == 32826 && check == 19)
			{
				pc.LapCheck = check + 1;
			}
			else if (x >= 32793 && y == 32831 && check == 20)
			{
				pc.LapCheck = check + 1;
			}
			else if (x >= 32793 && y == 32836 && check == 21)
			{
				pc.LapCheck = check + 1;
			}
			else if (x >= 32793 && y == 32842 && check == 22)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32790 && y >= 32845 && check == 23)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32785 && y >= 32845 && check == 24)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32780 && y >= 32845 && check == 25)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32775 && y >= 32845 && check == 26)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32770 && y >= 32845 && check == 27)
			{
				pc.LapCheck = check + 1;
			}
			else if (x == 32764 && y >= 32845 && check == 28)
			{
				if (pc.Lap == maxLap)
				{
					GameWinner = pc;
					return;
				}
				pc.LapCheck = 0;
				pc.Lap = pc.Lap + 1;
				pc.sendPackets(new S_Race(maxLap, pc.Lap)); //lap

			}
		}

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


		private int _time = 0;

		private void clearTime()
		{
			_time = 0;
		}

		private void addTime()
		{
			_time++;
		}

		private L1PcInstance _winner = null;

		public virtual L1PcInstance Winner
		{
			set
			{
				_winner = value;
			}
			get
			{
				return _winner;
			}
		}


		///////////////////////////////////////////////////////////////	

		private void startReadyTimer()
		{
			(new ReadyTimer(this)).begin();
		}

		private void startCheckTimer()
		{
			(new CheckTimer(this)).begin();
		}

		private void startClockTimer()
		{
			(new ClockTimer(this)).begin();
		}

		private GameTimeLimitTimer limitTimer;

		private void startGameTimeLimitTimer()
		{
			Timer timer = new Timer();
			limitTimer = new GameTimeLimitTimer(this);
			timer.schedule(limitTimer, limitTime);
		}

		private void stopGameTimeLimitTimer()
		{
			limitTimer.stopTimer();
		}

		private void startEndTimer()
		{
			(new EndTimer(this)).begin();
		}

		private CompareTimer compareTimer;

		private void startCompareTimer()
		{
			Timer timer = new Timer();
			compareTimer = new CompareTimer(this);
			timer.schedule(compareTimer, 2000, 2000);
		}

		private void stopCompareTimer()
		{
			compareTimer.stopTimer();
		}

		//////////////////////////////////////////////////////////

		//	進場等待--->確認人數
		private class ReadyTimer : TimerTask
		{
			private readonly L1PolyRace outerInstance;

			public ReadyTimer(L1PolyRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				foreach (L1PcInstance pc in outerInstance.playerList)
				{
					pc.sendPackets(new S_ServerMessage(1258));
				}
				outerInstance.startCheckTimer();
				this.cancel();
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, readyTime);
			}
		}

		//	確認人數OK --->開始
		private class CheckTimer : TimerTask
		{
			private readonly L1PolyRace outerInstance;

			public CheckTimer(L1PolyRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				if (outerInstance.checkPlayersOK())
				{
					outerInstance.setGameStart();
				}
				else
				{
					outerInstance.GameEnd = END_STATUS_NOPLAYER;
				}
				this.cancel();
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 30 * 1000); //60s
			}
		}

		//	倒數5秒--->開始計時
		private class ClockTimer : TimerTask
		{
			private readonly L1PolyRace outerInstance;

			public ClockTimer(L1PolyRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				// 計時封包
				foreach (L1PcInstance pc in outerInstance.playerList)
				{
					pc.sendPackets(new S_Race(S_Race.CountDown));
				}
				outerInstance.DoorClose = false;
				outerInstance.startGameTimeLimitTimer();
				this.cancel();
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 5000); // 5s
			}
		}

		//	開始計時--->遊戲結束
		private class GameTimeLimitTimer : TimerTask
		{
			private readonly L1PolyRace outerInstance;

			public GameTimeLimitTimer(L1PolyRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance.GameEnd = END_STATUS_NOWINNER;
				this.cancel();
			}

			public virtual void stopTimer()
			{
				this.cancel();
			}
		}

		private class EndTimer : TimerTask
		{
			private readonly L1PolyRace outerInstance;

			public EndTimer(L1PolyRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance.giftWinner();
				outerInstance.setGameInit();
				this.cancel();
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 5000); // 10s
			}
		}

		private class CompareTimer : TimerTask
		{
			private readonly L1PolyRace outerInstance;

			public CompareTimer(L1PolyRace outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void run()
			{
				outerInstance.comparePosition();
				outerInstance.addTime();
			}

			public virtual void stopTimer()
			{
				this.cancel();
			}
		}
	}
}