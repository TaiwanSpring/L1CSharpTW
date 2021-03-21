﻿using System;
using System.Collections.Generic;
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
namespace LineageServer.Server.Server.Model
{

	using Config = LineageServer.Server.Config;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using L1DoorInstance = LineageServer.Server.Server.Model.Instance.L1DoorInstance;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1GameTime = LineageServer.Server.Server.Model.gametime.L1GameTime;
	using L1GameTimeAdapter = LineageServer.Server.Server.Model.gametime.L1GameTimeAdapter;
	using L1GameTimeClock = LineageServer.Server.Server.Model.gametime.L1GameTimeClock;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using L1SpawnTime = LineageServer.Server.Server.Templates.L1SpawnTime;
	using Point = LineageServer.Server.Server.Types.Point;
	using Random = LineageServer.Server.Server.utils.Random;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class L1Spawn : L1GameTimeAdapter
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1Spawn).FullName);

		private readonly L1Npc _template;

		private int _id; // just to find this in the spawn table

		private string _location;

		private int _maximumCount;

		private int _npcid;

		private int _groupId;

		private int _locx;

		private int _locy;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private int Randomx_Conflict;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private int Randomy_Conflict;

		private int _locx1;

		private int _locy1;

		private int _locx2;

		private int _locy2;

		private int _heading;

		private int _minRespawnDelay;

		private int _maxRespawnDelay;

		private short _mapid;

		private bool _respaenScreen;

		private int _movementDistance;

		private bool _rest;

		private int _spawnType;

		private int _delayInterval;

		private L1SpawnTime _time;

		private IDictionary<int, Point> _homePoint = null; // initでspawnした個々のオブジェクトのホームポイント

		private IList<L1NpcInstance> _mobs = Lists.newList();

		private string _name;

		private class SpawnTask : IRunnableStart
		{
			private readonly L1Spawn outerInstance;

			internal int _spawnNumber;

			internal int _objectId;

			internal SpawnTask(L1Spawn outerInstance, int spawnNumber, int objectId)
			{
				this.outerInstance = outerInstance;
				_spawnNumber = spawnNumber;
				_objectId = objectId;
			}

			public override void run()
			{
				outerInstance.doSpawn(_spawnNumber, _objectId);
			}
		}

		public L1Spawn(L1Npc mobTemplate)
		{
			_template = mobTemplate;
		}

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


		public virtual short MapId
		{
			get
			{
				return _mapid;
			}
			set
			{
				this._mapid = value;
			}
		}


		public virtual bool RespawnScreen
		{
			get
			{
				return _respaenScreen;
			}
			set
			{
				_respaenScreen = value;
			}
		}


		public virtual int MovementDistance
		{
			get
			{
				return _movementDistance;
			}
			set
			{
				_movementDistance = value;
			}
		}


		public virtual int Amount
		{
			get
			{
				return _maximumCount;
			}
			set
			{
				_maximumCount = value;
			}
		}

		public virtual int GroupId
		{
			get
			{
				return _groupId;
			}
			set
			{
				_groupId = value;
			}
		}

		public virtual int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public virtual string Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		public virtual int LocX
		{
			get
			{
				return _locx;
			}
			set
			{
				_locx = value;
			}
		}

		public virtual int LocY
		{
			get
			{
				return _locy;
			}
			set
			{
				_locy = value;
			}
		}

		public virtual int NpcId
		{
			get
			{
				return _npcid;
			}
		}

		public virtual int Heading
		{
			get
			{
				return _heading;
			}
			set
			{
				_heading = value;
			}
		}

		public virtual int Randomx
		{
			get
			{
				return Randomx_Conflict;
			}
			set
			{
				Randomx_Conflict = value;
			}
		}

		public virtual int Randomy
		{
			get
			{
				return Randomy_Conflict;
			}
			set
			{
				Randomy_Conflict = value;
			}
		}

		public virtual int LocX1
		{
			get
			{
				return _locx1;
			}
			set
			{
				_locx1 = value;
			}
		}

		public virtual int LocY1
		{
			get
			{
				return _locy1;
			}
			set
			{
				_locy1 = value;
			}
		}

		public virtual int LocX2
		{
			get
			{
				return _locx2;
			}
			set
			{
				_locx2 = value;
			}
		}

		public virtual int LocY2
		{
			get
			{
				return _locy2;
			}
			set
			{
				_locy2 = value;
			}
		}

		public virtual int MinRespawnDelay
		{
			get
			{
				return _minRespawnDelay;
			}
			set
			{
				_minRespawnDelay = value;
			}
		}

		public virtual int MaxRespawnDelay
		{
			get
			{
				return _maxRespawnDelay;
			}
			set
			{
				_maxRespawnDelay = value;
			}
		}







		public virtual int Npcid
		{
			set
			{
				_npcid = value;
			}
		}










		private int calcRespawnDelay()
		{
			int respawnDelay = _minRespawnDelay * 1000;
			if (_delayInterval > 0)
			{
				respawnDelay += RandomHelper.Next(_delayInterval) * 1000;
			}
			L1GameTime currentTime = L1GameTimeClock.Instance.currentTime();
			if ((_time != null) && !_time.TimePeriod.includes(currentTime))
			{ // 指定時間外なら指定時間までの時間を足す
				long diff = (_time.TimeStart.Time - currentTime.toTime().Time);
				if (diff < 0)
				{
					diff += 24 * 1000L * 3600L;
				}
				diff /= 6; // real time to game time
				respawnDelay = (int) diff;
			}
			return respawnDelay;
		}

		/// <summary>
		/// SpawnTaskを起動する。
		/// </summary>
		/// <param name="spawnNumber">
		///            L1Spawnで管理されている番号。ホームポイントが無ければ何を指定しても良い。 </param>
		public void ExecuteSpawnTask(int spawnNumber, int objectId)
		{
			SpawnTask task = new SpawnTask(this, spawnNumber, objectId);
			GeneralThreadPool.Instance.schedule(task, calcRespawnDelay());
		}

		private bool _initSpawn = false;

		private bool _spawnHomePoint;

		public virtual void init()
		{
			if ((_time != null) && _time.DeleteAtEndTime)
			{
				// 時間外削除が指定されているなら、時間経過の通知を受ける。
				L1GameTimeClock.Instance.addListener(this);
			}
			_delayInterval = _maxRespawnDelay - _minRespawnDelay;
			_initSpawn = true;
			// ホームポイントを持たせるか
			if (Config.SPAWN_HOME_POINT && (Config.SPAWN_HOME_POINT_COUNT <= Amount) && (Config.SPAWN_HOME_POINT_DELAY >= MinRespawnDelay) && AreaSpawn)
			{
				_spawnHomePoint = true;
				_homePoint = Maps.newMap();
			}

			int spawnNum = 0;
			while (spawnNum < _maximumCount)
			{
				// spawnNumは1～maxmumCountまで
				doSpawn(++spawnNum);
			}
			_initSpawn = false;
		}

		/// <summary>
		/// ホームポイントがある場合は、spawnNumberを基にspawnする。 それ以外の場合は、spawnNumberは未使用。
		/// </summary>
		protected internal virtual void doSpawn(int spawnNumber)
		{ // 初期配置
			// 指定時間外であれば、次spawnを予約して終わる。
			if ((_time != null) && !_time.TimePeriod.includes(L1GameTimeClock.Instance.currentTime()))
			{
				executeSpawnTask(spawnNumber, 0);
				return;
			}
			doSpawn(spawnNumber, 0);
		}

		protected internal virtual void doSpawn(int spawnNumber, int objectId)
		{ // 再出現
			L1NpcInstance mob = null;
			try
			{
				int newlocx = LocX;
				int newlocy = LocY;
				int tryCount = 0;

				mob = NpcTable.Instance.newNpcInstance(_template);
				lock (_mobs)
				{
					_mobs.Add(mob);
				}
				if (objectId == 0)
				{
					mob.Id = IdFactory.Instance.nextId();
				}
				else
				{
					mob.Id = objectId; // オブジェクトID再利用
				}

				if ((0 <= Heading) && (Heading <= 7))
				{
					mob.Heading = Heading;
					//TODO 隨機面向byby9001183ex
				}
				else if (Heading == 9)
				{
					mob.Heading = RandomHelper.Next(8);
					//TODO 隨機面向by9001183ex
				}
				else
				{
					// heading値が正しくない
					mob.Heading = 5;
				}

				int npcId = mob.NpcTemplate.get_npcId();
				if ((npcId == 45488) && (MapId == 9))
				{ // 卡士伯
					mob.Map = (short)(MapId + RandomHelper.Next(2));
				}
				else if ((npcId == 45601) && (MapId == 11))
				{ // 死亡騎士
					mob.Map = (short)(MapId + RandomHelper.Next(3));
				}
				else if ((npcId == 81322) && (MapId == 25))
				{ // 黑騎士副隊長
					mob.Map = (short)(MapId + RandomHelper.Next(2));
				}
				else
				{
					mob.Map = MapId;
				}
				mob.MovementDistance = MovementDistance;
				mob.Rest = Rest;
				while (tryCount <= 50)
				{
					switch (SpawnType)
					{
						case SPAWN_TYPE_PC_AROUND: // PC周辺に湧くタイプ
							if (!_initSpawn)
							{ // 初期配置では無条件に通常spawn
								IList<L1PcInstance> players = Lists.newList();
								foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
								{
									if (MapId == pc.MapId)
									{
										players.Add(pc);
									}
								}
								if (players.Count > 0)
								{
									L1PcInstance pc = players[RandomHelper.Next(players.Count)];
									L1Location loc = pc.Location.randomLocation(PC_AROUND_DISTANCE, false);
									newlocx = loc.X;
									newlocy = loc.Y;
									break;
								}
							}
							// フロアにPCがいなければ通常の出現方法
							goto default;
						default:
							if (AreaSpawn)
							{ // 座標が範囲指定されている場合
								Point pt = null;
								if (_spawnHomePoint && (null != (pt = _homePoint[spawnNumber])))
								{ // ホームポイントを元に再出現させる場合
									L1Location loc = (new L1Location(pt, MapId)).randomLocation(Config.SPAWN_HOME_POINT_RANGE, false);
									newlocx = loc.X;
									newlocy = loc.Y;
								}
								else
								{
									int rangeX = LocX2 - LocX1;
									int rangeY = LocY2 - LocY1;
									newlocx = RandomHelper.Next(rangeX) + LocX1;
									newlocy = RandomHelper.Next(rangeY) + LocY1;
								}
								if (tryCount > 49)
								{ // 出現位置が決まらない時はlocx,locyの値
									newlocx = LocX;
									newlocy = LocY;
								}
							}
							else if (RandomSpawn)
							{ // 座標のランダム値が指定されている場合
								newlocx = (LocX + (RandomHelper.Next(Randomx) - RandomHelper.Next(Randomx)));
								newlocy = (LocY + (RandomHelper.Next(Randomy) - RandomHelper.Next(Randomy)));
							}
							else
							{ // どちらも指定されていない場合
								newlocx = LocX;
								newlocy = LocY;
							}
						break;
					}
					mob.X = newlocx;
					mob.HomeX = newlocx;
					mob.Y = newlocy;
					mob.HomeY = newlocy;

					if (mob.Map.isInMap(mob.Location) && mob.Map.isPassable(mob.Location))
					{
						if (mob is L1MonsterInstance)
						{
							if (RespawnScreen)
							{
								break;
							}
							L1MonsterInstance mobtemp = (L1MonsterInstance) mob;
							if (L1World.Instance.getVisiblePlayer(mobtemp).Count == 0)
							{
								break;
							}
							// 画面内にPCが居て出現できない場合は、3秒後にスケジューリングしてやり直し
							SpawnTask task = new SpawnTask(this, spawnNumber, mob.Id);
							GeneralThreadPool.Instance.schedule(task, 3000L);
							return;
						}
					}
					tryCount++;
				}
				if (mob is L1MonsterInstance)
				{
					((L1MonsterInstance) mob).initHide();
				}

				mob.Spawn = this;
				mob.setreSpawn(true);
				mob.SpawnNumber = spawnNumber; // L1Spawnでの管理番号(ホームポイントに使用)
				if (_initSpawn && _spawnHomePoint)
				{ // 初期配置でホームポイントを設定
					Point pt = new Point(mob.X, mob.Y);
					_homePoint[spawnNumber] = pt; // ここで保存したpointを再出現時に使う
				}

				if (mob is L1MonsterInstance)
				{
					if (mob.MapId == 666)
					{
						((L1MonsterInstance) mob).set_storeDroped(true);
					}
				}
				if ((npcId == 45573) && (mob.MapId == 2))
				{ // バフォメット
					foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
					{
						if (pc.MapId == 2)
						{
							L1Teleport.teleport(pc, 32664, 32797, (short) 2, 0, true);
						}
					}
				}

				if (((npcId == 46142) && (mob.MapId == 73)) || ((npcId == 46141) && (mob.MapId == 74)))
				{
					foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
					{
						if ((pc.MapId >= 72) && (pc.MapId <= 74))
						{
							L1Teleport.teleport(pc, 32840, 32833, (short) 72, pc.Heading, true);
						}
					}
				}
				if ((npcId == 81341) && ((mob.MapId == 2000) || (mob.MapId == 2001) || (mob.MapId == 2002) || (mob.MapId == 2003)))
				{ // 再生之祭壇
					foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
					{
						if ((pc.MapId >= 2000) && (pc.MapId <= 2003))
						{
							L1Teleport.teleport(pc, 32933, 32988, (short) 410, 5, true);
						}
					}
				}

				doCrystalCave(npcId);

				L1World.Instance.storeObject(mob);
				L1World.Instance.addVisibleObject(mob);

				if (mob is L1MonsterInstance)
				{
					L1MonsterInstance mobtemp = (L1MonsterInstance) mob;
					if (!_initSpawn && (mobtemp.HiddenStatus == 0))
					{
						mobtemp.onNpcAI(); // モンスターのＡＩを開始
					}
				}
				if (GroupId != 0)
				{
					L1MobGroupSpawn.Instance.doSpawn(mob, GroupId, RespawnScreen, _initSpawn);
				}
				mob.turnOnOffLight();
				mob.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}

		public virtual bool Rest
		{
			set
			{
				_rest = value;
			}
			get
			{
				return _rest;
			}
		}


		private const int SPAWN_TYPE_PC_AROUND = 1;

		private const int PC_AROUND_DISTANCE = 30;

		private int SpawnType
		{
			get
			{
				return _spawnType;
			}
			set
			{
				_spawnType = value;
			}
		}


		private bool AreaSpawn
		{
			get
			{
				return (LocX1 != 0) && (LocY1 != 0) && (LocX2 != 0) && (LocY2 != 0);
			}
		}

		private bool RandomSpawn
		{
			get
			{
				return (Randomx != 0) || (Randomy != 0);
			}
		}

		public virtual L1SpawnTime Time
		{
			get
			{
				return _time;
			}
			set
			{
				_time = value;
			}
		}


		public override void onMinuteChanged(L1GameTime time)
		{
			if (_time.TimePeriod.includes(time))
			{
				return;
			}
			lock (_mobs)
			{
				if (_mobs.Count == 0)
				{
					return;
				}
				// 指定時間外になっていれば削除
				foreach (L1NpcInstance mob in _mobs)
				{
					mob.CurrentHpDirect = 0;
					mob.Dead = true;
					mob.Status = ActionCodes.ACTION_Die;
					mob.deleteMe();
				}
				_mobs.Clear();
			}
		}

		public static void doCrystalCave(int npcId)
		{
			int[] npcId2 = new int[] {46143, 46144, 46145, 46146, 46147, 46148, 46149, 46150, 46151, 46152};
			int[] doorId = new int[] {5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009, 5010};

			for (int i = 0; i < npcId2.Length; i++)
			{
				if (npcId == npcId2[i])
				{
					closeDoorInCrystalCave(doorId[i]);
				}
			}
		}

		private static void closeDoorInCrystalCave(int doorId)
		{
			foreach (L1Object @object in L1World.Instance.Object)
			{
				if (@object is L1DoorInstance)
				{
					L1DoorInstance door = (L1DoorInstance) @object;
					if (door.DoorId == doorId)
					{
						door.close();
					}
				}
			}
		}
	}

}