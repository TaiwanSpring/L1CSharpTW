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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1BossSpawn = LineageServer.Server.Server.Model.L1BossSpawn;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class BossSpawnTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(BossSpawnTable).FullName);

		private BossSpawnTable()
		{
		}

		public static void fillSpawnTable()
		{

			int spawnCount = 0;
			java.sql.Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_boss");
				rs = pstm.executeQuery();

				L1BossSpawn spawnDat;
				L1Npc template1;
				while (rs.next())
				{
					int npcTemplateId = rs.getInt("npc_id");
					template1 = NpcTable.Instance.getTemplate(npcTemplateId);

					if (template1 == null)
					{
						_log.warning("mob data for id:" + npcTemplateId + " missing in npc table");
						spawnDat = null;
					}
					else
					{
						spawnDat = new L1BossSpawn(template1);
						spawnDat.Id = rs.getInt("id");
						spawnDat.Npcid = npcTemplateId;
						spawnDat.Location = rs.getString("location");
						spawnDat.CycleType = rs.getString("cycle_type");
						spawnDat.Amount = rs.getInt("count");
						spawnDat.GroupId = rs.getInt("group_id");
						spawnDat.LocX = rs.getInt("locx");
						spawnDat.LocY = rs.getInt("locy");
						spawnDat.Randomx = rs.getInt("randomx");
						spawnDat.Randomy = rs.getInt("randomy");
						spawnDat.LocX1 = rs.getInt("locx1");
						spawnDat.LocY1 = rs.getInt("locy1");
						spawnDat.LocX2 = rs.getInt("locx2");
						spawnDat.LocY2 = rs.getInt("locy2");
						spawnDat.Heading = rs.getInt("heading");
						spawnDat.MapId = rs.getShort("mapid");
						spawnDat.RespawnScreen = rs.getBoolean("respawn_screen");
						spawnDat.MovementDistance = rs.getInt("movement_distance");
						spawnDat.Rest = rs.getBoolean("rest");
						spawnDat.SpawnType = rs.getInt("spawn_type");
						spawnDat.Percentage = rs.getInt("percentage");

						spawnDat.Name = template1.get_name();

						// start the spawning
						spawnDat.init();
						spawnCount += spawnDat.Amount;

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
			_log.log(Level.FINE, "総ボスモンスター数 " + spawnCount + "匹");
		}
	}

}