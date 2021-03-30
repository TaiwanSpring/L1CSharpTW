using System;
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

	using Config = LineageServer.Server.Config;
	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Spawn = LineageServer.Server.Model.L1Spawn;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1Npc = LineageServer.Server.Templates.L1Npc;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	// Referenced classes of package l1j.server.server:
	// MobTable, IdFactory

	public class NpcSpawnTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(NpcSpawnTable).FullName);

		private static NpcSpawnTable _instance;

		private IDictionary<int, L1Spawn> _spawntable = MapFactory.NewMap();

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

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_npc");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					if (Config.ALT_GMSHOP == false)
					{
						int npcid = dataSourceRow.getInt(1);
						if ((npcid >= Config.ALT_GMSHOP_MIN_ID) && (npcid <= Config.ALT_GMSHOP_MAX_ID))
						{
							continue;
						}
					}
					if (Config.ALT_HALLOWEENIVENT == false)
					{
						int npcid = dataSourceRow.getInt("id");
						if (((npcid >= 130852) && (npcid <= 130862)) || ((npcid >= 26656) && (npcid <= 26734)) || ((npcid >= 89634) && (npcid <= 89644)))
						{
							continue;
						}
					}
					if (Config.ALT_JPPRIVILEGED == false)
					{
						int npcid = dataSourceRow.getInt("id");
						if ((npcid >= 1310368) && (npcid <= 1310379))
						{
							continue;
						}
					}
					if (Config.ALT_TALKINGSCROLLQUEST == false)
					{
						int npcid = dataSourceRow.getInt("id");
						if (((npcid >= 87537) && (npcid <= 87551)) || ((npcid >= 1310387) && (npcid <= 1310389)))
						{
							continue;
						}
					}
					if (Config.ALT_TALKINGSCROLLQUEST == true)
					{
						int npcid = dataSourceRow.getInt("id");
						if ((npcid >= 90066) && (npcid <= 90069))
						{
							continue;
						}
					}
					int npcTemplateid = dataSourceRow.getInt("npc_templateid");
					L1Npc l1npc = NpcTable.Instance.getTemplate(npcTemplateid);
					L1Spawn l1spawn;
					if (l1npc == null)
					{
						_log.Warning("mob data for id:" + npcTemplateid + " missing in npc table");
						l1spawn = null;
					}
					else
					{
						if (dataSourceRow.getInt("count") == 0)
						{
							continue;
						}
						l1spawn = new L1Spawn(l1npc);
						l1spawn.Id = dataSourceRow.getInt("id");
						l1spawn.Amount = dataSourceRow.getInt("count");
						l1spawn.LocX = dataSourceRow.getInt("locx");
						l1spawn.LocY = dataSourceRow.getInt("locy");
						l1spawn.Randomx = dataSourceRow.getInt("randomx");
						l1spawn.Randomy = dataSourceRow.getInt("randomy");
						l1spawn.LocX1 = 0;
						l1spawn.LocY1 = 0;
						l1spawn.LocX2 = 0;
						l1spawn.LocY2 = 0;
						l1spawn.Heading = dataSourceRow.getInt("heading");
						l1spawn.MinRespawnDelay = dataSourceRow.getInt("respawn_delay");
						l1spawn.MapId = dataSourceRow.getShort("mapid");
						l1spawn.MovementDistance = dataSourceRow.getInt("movement_distance");
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

			_log.config("NPC配置リスト " + _spawntable.Count + "件ロード");
			_log.fine("総NPC数 " + spawnCount + "件");
		}

		public virtual void storeSpawn(L1PcInstance pc, L1Npc npc)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{
				int count = 1;
				string note = npc.get_name();

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO spawnlist_npc SET location=?,count=?,npc_templateid=?,locx=?,locy=?,heading=?,mapid=?");
				pstm.setString(1, note);
				pstm.setInt(2, count);
				pstm.setInt(3, npc.get_npcId());
				pstm.setInt(4, pc.X);
				pstm.setInt(5, pc.Y);
				pstm.setInt(6, pc.Heading);
				pstm.setInt(7, pc.MapId);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.Error(e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
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