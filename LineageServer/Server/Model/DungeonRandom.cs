using System.Collections.Generic;
using System.Text;

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
namespace LineageServer.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ABSOLUTE_BARRIER;


	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using Random = LineageServer.Utils.Random;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	// Referenced classes of package l1j.server.server.model:
	// L1Teleport, L1PcInstance

	public class DungeonRandom
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(DungeonRandom).FullName);

		private static DungeonRandom _instance = null;

		private static IDictionary<string, NewDungeonRandom> _dungeonMap = MapFactory.NewMap();

		public static DungeonRandom Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DungeonRandom();
				}
				return _instance;
			}
		}

		private DungeonRandom()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;

				pstm = con.prepareStatement("SELECT * FROM dungeon_random");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int srcMapId = dataSourceRow.getInt("src_mapid");
					int srcX = dataSourceRow.getInt("src_x");
					int srcY = dataSourceRow.getInt("src_y");
					string key = (new StringBuilder()).Append(srcMapId).Append(srcX).Append(srcY).ToString();
					int[] newX = new int[5];
					int[] newY = new int[5];
					short[] NewMapId = new short[5];
					newX[0] = dataSourceRow.getInt("new_x1");
					newY[0] = dataSourceRow.getInt("new_y1");
					NewMapId[0] = dataSourceRow.getShort("new_mapid1");
					newX[1] = dataSourceRow.getInt("new_x2");
					newY[1] = dataSourceRow.getInt("new_y2");
					NewMapId[1] = dataSourceRow.getShort("new_mapid2");
					newX[2] = dataSourceRow.getInt("new_x3");
					newY[2] = dataSourceRow.getInt("new_y3");
					NewMapId[2] = dataSourceRow.getShort("new_mapid3");
					newX[3] = dataSourceRow.getInt("new_x4");
					newY[3] = dataSourceRow.getInt("new_y4");
					NewMapId[3] = dataSourceRow.getShort("new_mapid4");
					newX[4] = dataSourceRow.getInt("new_x5");
					newY[4] = dataSourceRow.getInt("new_y5");
					NewMapId[4] = dataSourceRow.getShort("new_mapid5");
					int heading = dataSourceRow.getInt("new_heading");
					NewDungeonRandom newDungeonRandom = new NewDungeonRandom(newX, newY, NewMapId, heading);
					if (_dungeonMap.ContainsKey(key))
					{
						_log.log(Level.WARNING, "同じキーのdungeonデータがあります。key=" + key);
					}
					_dungeonMap[key] = newDungeonRandom;
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
		}

		private class NewDungeonRandom
		{
			internal int[] _newX = new int[5];

			internal int[] _newY = new int[5];

			internal short[] _NewMapId = new short[5];

			internal int _heading;

			internal NewDungeonRandom(int[] newX, int[] newY, short[] NewMapId, int heading)
			{
				for (int i = 0; i < 5; i++)
				{
					_newX[i] = newX[i];
					_newY[i] = newY[i];
					_NewMapId[i] = NewMapId[i];
				}
				_heading = heading;
			}
		}

		public virtual bool dg(int locX, int locY, int mapId, L1PcInstance pc)
		{
			string key = (new StringBuilder()).Append(mapId).Append(locX).Append(locY).ToString();
			if (_dungeonMap.ContainsKey(key))
			{
				int rnd = RandomHelper.Next(5);
				NewDungeonRandom newDungeonRandom = _dungeonMap[key];
				short NewMap = newDungeonRandom._NewMapId[rnd];
				int newX = newDungeonRandom._newX[rnd];
				int newY = newDungeonRandom._newY[rnd];
				int heading = newDungeonRandom._heading;

				// 2秒無敵狀態。
				pc.setSkillEffect(L1SkillId.ABSOLUTE_BARRIER, 2000);
				pc.stopHpRegeneration();
				pc.stopMpRegeneration();
				pc.stopHpRegenerationByDoll();
				pc.stopMpRegenerationByDoll();
				L1Teleport.teleport(pc, newX, newY, NewMap, heading, true);
				return true;
			}
			return false;
		}
	}

}