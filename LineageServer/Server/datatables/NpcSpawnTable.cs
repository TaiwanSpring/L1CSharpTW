using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class NpcSpawnTable
	{
		private readonly static IDataSource dataSource =
			   Container.Instance.Resolve<IDataSourceFactory>()
			   .Factory(Enum.DataSourceTypeEnum.SpawnlistNpc);
		private static NpcSpawnTable _instance;

		private IDictionary<int, L1Spawn> _spawntable = MapFactory.NewMap<int, L1Spawn>();

		private int _highestId;

		public static NpcSpawnTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NpcSpawnTable();
				}
				return _instance;
			}
		}

		private NpcSpawnTable()
		{
			fillNpcSpawnTable();
		}

		private void fillNpcSpawnTable()
		{

			int spawnCount = 0;
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int count = dataSourceRow.getInt(SpawnlistNpc.Column_count);
				if (count == 0)
				{
					continue;
				}

				int npcid = dataSourceRow.getInt(SpawnlistNpc.Column_id);

				if (Config.ALT_GMSHOP == false)
				{
					if (( npcid >= Config.ALT_GMSHOP_MIN_ID ) && ( npcid <= Config.ALT_GMSHOP_MAX_ID ))
					{
						continue;
					}
				}
				if (Config.ALT_HALLOWEENIVENT == false)
				{
					if (( ( npcid >= 130852 ) && ( npcid <= 130862 ) ) || ( ( npcid >= 26656 ) && ( npcid <= 26734 ) ) || ( ( npcid >= 89634 ) && ( npcid <= 89644 ) ))
					{
						continue;
					}
				}
				if (Config.ALT_JPPRIVILEGED == false)
				{
					if (( npcid >= 1310368 ) && ( npcid <= 1310379 ))
					{
						continue;
					}
				}
				if (Config.ALT_TALKINGSCROLLQUEST == false)
				{
					if (( ( npcid >= 87537 ) && ( npcid <= 87551 ) ) || ( ( npcid >= 1310387 ) && ( npcid <= 1310389 ) ))
					{
						continue;
					}
				}
				if (Config.ALT_TALKINGSCROLLQUEST == true)
				{
					if (( npcid >= 90066 ) && ( npcid <= 90069 ))
					{
						continue;
					}
				}
				int npcTemplateid = dataSourceRow.getInt(SpawnlistNpc.Column_npc_templateid);
				L1Npc l1npc = NpcTable.Instance.getTemplate(npcTemplateid);
				if (l1npc == null)
				{
					Logger.GenericLogger.Log("mob data for id:" + npcTemplateid + " missing in npc table");
				}
				else
				{
					L1Spawn l1spawn = new L1Spawn(l1npc);
					l1spawn.Id = npcid;
					l1spawn.Amount = count;
					l1spawn.LocX = dataSourceRow.getInt(SpawnlistNpc.Column_locx);
					l1spawn.LocY = dataSourceRow.getInt(SpawnlistNpc.Column_locy);
					l1spawn.Randomx = dataSourceRow.getInt(SpawnlistNpc.Column_randomx);
					l1spawn.Randomy = dataSourceRow.getInt(SpawnlistNpc.Column_randomy);
					l1spawn.LocX1 = 0;
					l1spawn.LocY1 = 0;
					l1spawn.LocX2 = 0;
					l1spawn.LocY2 = 0;
					l1spawn.Heading = dataSourceRow.getInt(SpawnlistNpc.Column_heading);
					l1spawn.MinRespawnDelay = dataSourceRow.getInt(SpawnlistNpc.Column_respawn_delay);
					l1spawn.MapId = dataSourceRow.getShort(SpawnlistNpc.Column_mapid);
					l1spawn.MovementDistance = dataSourceRow.getInt(SpawnlistNpc.Column_movement_distance);
					l1spawn.Name = l1npc.get_name();
					l1spawn.init();
					spawnCount += l1spawn.Amount;

					_spawntable[l1spawn.Id] = l1spawn;
					if (l1spawn.Id > _highestId)
					{
						_highestId = l1spawn.Id;
					}
				}
			}

			Logger.GenericLogger.Log("NPC配置リスト " + _spawntable.Count + "件ロード");
			Logger.GenericLogger.Log("総NPC数 " + spawnCount + "件");
		}

		public virtual void storeSpawn(L1PcInstance pc, L1Npc npc)
		{
			_highestId++;
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Insert()
			.Set(SpawnlistNpc.Column_id, _highestId)
			.Set(SpawnlistNpc.Column_location, npc.get_name())
			.Set(SpawnlistNpc.Column_count, 1)
			.Set(SpawnlistNpc.Column_npc_templateid, npc.get_npcId())
			.Set(SpawnlistNpc.Column_locx, pc.X)
			.Set(SpawnlistNpc.Column_locy, pc.Y)
			.Set(SpawnlistNpc.Column_heading, pc.Heading)
			.Set(SpawnlistNpc.Column_mapid, pc.MapId)
			.Execute();
		}

		public virtual L1Spawn getTemplate(int i)
		{
			return _spawntable[i];
		}

		public virtual void addNewSpawn(L1Spawn l1spawn)
		{
			_highestId++;
			l1spawn.Id = _highestId;
			_spawntable[l1spawn.Id] = l1spawn;
		}

	}

}