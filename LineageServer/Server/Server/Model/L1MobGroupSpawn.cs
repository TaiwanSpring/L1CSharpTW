using System;

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
	using Random = LineageServer.Server.Server.utils.Random;

	using IdFactory = LineageServer.Server.Server.IdFactory;
	using MobGroupTable = LineageServer.Server.Server.datatables.MobGroupTable;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1MobGroup = LineageServer.Server.Server.Templates.L1MobGroup;
	using L1NpcCount = LineageServer.Server.Server.Templates.L1NpcCount;

	// Referenced classes of package l1j.server.server.model:
	// L1MobGroupSpawn

	public class L1MobGroupSpawn
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static readonly Logger _log = Logger.getLogger(typeof(L1MobGroupSpawn).FullName);

		private static L1MobGroupSpawn _instance;

		private bool _isRespawnScreen;

		private bool _isInitSpawn;

		private L1MobGroupSpawn()
		{
		}

		public static L1MobGroupSpawn Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1MobGroupSpawn();
				}
				return _instance;
			}
		}

		public virtual void doSpawn(L1NpcInstance leader, int groupId, bool isRespawnScreen, bool isInitSpawn)
		{

			L1MobGroup mobGroup = MobGroupTable.Instance.getTemplate(groupId);
			if (mobGroup == null)
			{
				return;
			}

			L1NpcInstance mob;
			_isRespawnScreen = isRespawnScreen;
			_isInitSpawn = isInitSpawn;

			L1MobGroupInfo mobGroupInfo = new L1MobGroupInfo();

			mobGroupInfo.RemoveGroup = mobGroup.RemoveGroupIfLeaderDie;
			mobGroupInfo.addMember(leader);

			foreach (L1NpcCount minion in mobGroup.Minions)
			{
				if (minion.Zero)
				{
					continue;
				}
				for (int i = 0; i < minion.Count; i++)
				{
					mob = spawn(leader, minion.Id);
					if (mob != null)
					{
						mobGroupInfo.addMember(mob);
					}
				}
			}
		}

		private L1NpcInstance spawn(L1NpcInstance leader, int npcId)
		{
			L1NpcInstance mob = null;
			try
			{
				mob = NpcTable.Instance.newNpcInstance(npcId);

				mob.Id = IdFactory.Instance.nextId();

				mob.Heading = leader.Heading;
				mob.Map = leader.MapId;
				mob.MovementDistance = leader.MovementDistance;
				mob.Rest = leader.Rest;

				mob.X = leader.X + RandomHelper.Next(5) - 2;
				mob.Y = leader.Y + RandomHelper.Next(5) - 2;
				// マップ外、障害物上、画面内沸き不可で画面内にPCがいる場合、リーダーと同じ座標
				if (!canSpawn(mob))
				{
					mob.X = leader.X;
					mob.Y = leader.Y;
				}
				mob.HomeX = mob.X;
				mob.HomeY = mob.Y;

				if (mob is L1MonsterInstance)
				{
					((L1MonsterInstance) mob).initHideForMinion(leader);
				}

				mob.Spawn = leader.Spawn;
				mob.setreSpawn(leader.ReSpawn);
				mob.SpawnNumber = leader.SpawnNumber;

				if (mob is L1MonsterInstance)
				{
					if (mob.MapId == 666)
					{
						((L1MonsterInstance) mob).set_storeDroped(true);
					}
				}

				L1World.Instance.storeObject(mob);
				L1World.Instance.addVisibleObject(mob);

				if (mob is L1MonsterInstance)
				{
					if (!_isInitSpawn && mob.HiddenStatus == 0)
					{
						mob.onNpcAI(); // モンスターのＡＩを開始
					}
				}
				mob.turnOnOffLight();
				mob.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			return mob;
		}

		private bool canSpawn(L1NpcInstance mob)
		{
			if (mob.Map.isInMap(mob.Location) && mob.Map.isPassable(mob.Location))
			{
				if (_isRespawnScreen)
				{
					return true;
				}
				if (L1World.Instance.getVisiblePlayer(mob).Count == 0)
				{
					return true;
				}
			}
			return false;
		}

	}

}