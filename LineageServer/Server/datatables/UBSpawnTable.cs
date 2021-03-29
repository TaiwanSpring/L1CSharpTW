using System.Collections.Generic;

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
namespace LineageServer.Server.DataTables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1UbPattern = LineageServer.Server.Model.L1UbPattern;
	using L1UbSpawn = LineageServer.Server.Model.L1UbSpawn;
	using L1Npc = LineageServer.Server.Templates.L1Npc;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class UBSpawnTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(UBSpawnTable).FullName);

		private static UBSpawnTable _instance;

		private IDictionary<int, L1UbSpawn> _spawnTable = MapFactory.newMap();

		public static UBSpawnTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new UBSpawnTable();
				}
				return _instance;
			}
		}

		private UBSpawnTable()
		{
			loadSpawnTable();
		}

		private void loadSpawnTable()
		{

			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_ub");
				rs = pstm.executeQuery();

				while (rs.next())
				{
					L1Npc npcTemp = NpcTable.Instance.getTemplate(dataSourceRow.getInt(6));
					if (npcTemp == null)
					{
						continue;
					}

					L1UbSpawn spawnDat = new L1UbSpawn();
					spawnDat.Id = dataSourceRow.getInt(1);
					spawnDat.UbId = dataSourceRow.getInt(2);
					spawnDat.Pattern = dataSourceRow.getInt(3);
					spawnDat.Group = dataSourceRow.getInt(4);
					spawnDat.Name = npcTemp.get_name();
					spawnDat.NpcTemplateId = dataSourceRow.getInt(6);
					spawnDat.Amount = dataSourceRow.getInt(7);
					spawnDat.SpawnDelay = dataSourceRow.getInt(8);
					spawnDat.SealCount = dataSourceRow.getInt(9);

					_spawnTable[spawnDat.Id] = spawnDat;
				}
			}
			catch (SQLException e)
			{
				// problem with initializing spawn, go to next one
				_log.warning("spawn couldnt be initialized:" + e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			_log.config("UBモンスター配置リスト " + _spawnTable.Count + "件ロード");
		}

		public virtual L1UbSpawn getSpawn(int spawnId)
		{
			return _spawnTable[spawnId];
		}

		/// <summary>
		/// 指定されたUBIDに対するパターンの最大数を返す。
		/// </summary>
		/// <param name="ubId">
		///            調べるUBID。 </param>
		/// <returns> パターンの最大数。 </returns>
		public virtual int getMaxPattern(int ubId)
		{
			int n = 0;
			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT MAX(pattern) FROM spawnlist_ub WHERE ub_id=?");
				pstm.setInt(1, ubId);
				rs = pstm.executeQuery();
				if (rs.next())
				{
					n = dataSourceRow.getInt(1);
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return n;
		}

		public virtual L1UbPattern getPattern(int ubId, int patternNumer)
		{
			L1UbPattern pattern = new L1UbPattern();
			foreach (L1UbSpawn spawn in _spawnTable.Values)
			{
				if ((spawn.UbId == ubId) && (spawn.Pattern == patternNumer))
				{
					pattern.addSpawn(spawn.Group, spawn);
				}
			}
			pattern.freeze();

			return pattern;
		}
	}

}