using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LineageServer.Server.DataTables
{
	class SpawnTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Spawnlist);

		private static SpawnTable _instance;

		private IDictionary<int, L1Spawn> _spawntable = MapFactory.NewMap<int, L1Spawn>();

		private int _highestId;

		public static SpawnTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SpawnTable();
				}
				return _instance;
			}
		}

		private SpawnTable()
		{
			Stopwatch timer = Stopwatch.StartNew();
			System.Console.Write("【讀取】 【spawn mob】【設定】");
			fillSpawnTable();
			timer.Stop();
			System.Console.WriteLine($"【完成】【{timer.ElapsedMilliseconds}】【毫秒】。");
		}

		private void fillSpawnTable()
		{

			int spawnCount = 0;

			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];

				if (Config.ALT_HALLOWEENIVENT == false)
				{
					int npcid = dataSourceRow.getInt(Spawnlist.Column_id);

					if (( npcid >= 26656 ) && ( npcid <= 26734 ))
					{
						continue;
					}
				}

				int npcTemplateId = dataSourceRow.getInt(Spawnlist.Column_npc_templateid);

				L1Npc npc = NpcTable.Instance.getTemplate(npcTemplateId);

				if (npc == null)
				{

				}
				else
				{
					if (dataSourceRow.getInt(Spawnlist.Column_count) == 0)
					{
						continue;
					}
					double amount_rate = MapsTable.Instance.getMonsterAmount(dataSourceRow.getShort(Spawnlist.Column_mapid));
					int count = calcCount(npc, dataSourceRow.getInt(Spawnlist.Column_count), amount_rate);
					if (count == 0)
					{
						continue;
					}

					L1Spawn spawnDat = new L1Spawn(npc);
					spawnDat.Id = dataSourceRow.getInt(Spawnlist.Column_id);
					spawnDat.Amount = count;
					spawnDat.GroupId = dataSourceRow.getInt(Spawnlist.Column_group_id);
					spawnDat.LocX = dataSourceRow.getInt(Spawnlist.Column_locx);
					spawnDat.LocY = dataSourceRow.getInt(Spawnlist.Column_locy);
					spawnDat.Randomx = dataSourceRow.getInt(Spawnlist.Column_randomx);
					spawnDat.Randomy = dataSourceRow.getInt(Spawnlist.Column_randomy);
					spawnDat.LocX1 = dataSourceRow.getInt(Spawnlist.Column_locx1);
					spawnDat.LocY1 = dataSourceRow.getInt(Spawnlist.Column_locy1);
					spawnDat.LocX2 = dataSourceRow.getInt(Spawnlist.Column_locx2);
					spawnDat.LocY2 = dataSourceRow.getInt(Spawnlist.Column_locy2);
					spawnDat.Heading = dataSourceRow.getInt(Spawnlist.Column_heading);
					spawnDat.MinRespawnDelay = dataSourceRow.getInt(Spawnlist.Column_min_respawn_delay);
					spawnDat.MaxRespawnDelay = dataSourceRow.getInt(Spawnlist.Column_max_respawn_delay);
					spawnDat.MapId = dataSourceRow.getShort(Spawnlist.Column_mapid);
					spawnDat.RespawnScreen = dataSourceRow.getBoolean(Spawnlist.Column_respawn_screen);
					spawnDat.MovementDistance = dataSourceRow.getInt(Spawnlist.Column_movement_distance);
					spawnDat.Rest = dataSourceRow.getBoolean(Spawnlist.Column_rest);
					spawnDat.SpawnType = dataSourceRow.getInt(Spawnlist.Column_near_spawn);
					spawnDat.Time = SpawnTimeTable.Instance.get(spawnDat.Id);

					spawnDat.Name = npc.get_name();

					if (( count > 1 ) && ( spawnDat.LocX1 == 0 ))
					{
						// 複数かつ固定spawnの場合は、個体数 * 6 の範囲spawnに変える。
						// ただし範囲が30を超えないようにする
						int range = Math.Min(count * 6, 30);
						spawnDat.LocX1 = spawnDat.LocX - range;
						spawnDat.LocY1 = spawnDat.LocY - range;
						spawnDat.LocX2 = spawnDat.LocX + range;
						spawnDat.LocY2 = spawnDat.LocY + range;
					}

					// start the spawning
					spawnDat.init();
					spawnCount += spawnDat.Amount;

					_spawntable[spawnDat.Id] = spawnDat;
					if (spawnDat.Id > _highestId)
					{
						_highestId = spawnDat.Id;
					}
				}
			}
		}

		public virtual L1Spawn getTemplate(int Id)
		{
			return _spawntable[Id];
		}

		public virtual void addNewSpawn(L1Spawn spawn)
		{
			_highestId++;
			spawn.Id = _highestId;
			_spawntable[spawn.Id] = spawn;
		}

		public static void storeSpawn(L1PcInstance pc, L1Npc npc)
		{
			int count = 1;
			int randomXY = 12;
			int minRespawnDelay = 60;
			int maxRespawnDelay = 120;
			string note = npc.get_name();

			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Insert()
			//.Set(Spawnlist.Column_id, obj.Id)
			.Set(Spawnlist.Column_location, note)
			.Set(Spawnlist.Column_count, count)
			.Set(Spawnlist.Column_npc_templateid, npc.get_npcId())
			//	.Set(Spawnlist.Column_group_id, 0)
			.Set(Spawnlist.Column_locx, pc.X)
			.Set(Spawnlist.Column_locy, pc.Y)
			.Set(Spawnlist.Column_randomx, randomXY)
			.Set(Spawnlist.Column_randomy, randomXY)
			.Set(Spawnlist.Column_heading, pc.Heading)
			.Set(Spawnlist.Column_min_respawn_delay, minRespawnDelay)
			.Set(Spawnlist.Column_max_respawn_delay, maxRespawnDelay)
			.Set(Spawnlist.Column_mapid, pc.MapId)
			//.Set(Spawnlist.Column_respawn_screen, obj.RespawnScreen)
			//.Set(Spawnlist.Column_movement_distance, obj.MovementDistance)
			//.Set(Spawnlist.Column_rest, obj.Rest)
			//.Set(Spawnlist.Column_near_spawn, obj.NearSpawn)
			.Execute();
		}

		private static int calcCount(L1Npc npc, int count, double rate)
		{
			if (rate == 0)
			{
				return 0;
			}
			if (( rate == 1 ) || npc.AmountFixed)
			{
				return count;
			}
			else
			{
				return randomRound(( count * rate ));
			}
		}

		/// <summary>
		/// 少数を小数点第二位までの確率で上か下に丸めた整数を返す。
		/// 例えば1.3は30%の確率で切り捨て、70%の確率で切り上げられる。
		/// </summary>
		/// <param name="number"> - もとの少数 </param>
		/// <returns> 丸められた整数 </returns>
		public static int randomRound(double number)
		{
			double percentage = ( number - Math.Floor(number) ) * 100;

			if (percentage == 0)
			{
				return ( (int)number );
			}
			else
			{
				int r = RandomHelper.Next(100);

				if (r < percentage)
				{
					return ( (int)number + 1 );
				}
				else
				{
					return ( (int)number );
				}
			}
		}
	}

}