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
namespace LineageServer.Server.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1SpawnTime = LineageServer.Server.Server.Templates.L1SpawnTime;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class SpawnTimeTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(SpawnTimeTable).FullName);

		private static SpawnTimeTable _instance;

		private readonly IDictionary<int, L1SpawnTime> _times = Maps.newMap();

		public static SpawnTimeTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SpawnTimeTable();
				}
				return _instance;
			}
		}

		private SpawnTimeTable()
		{
			load();
		}

		public virtual L1SpawnTime get(int id)
		{
			return _times[id];
		}

		private void load()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_time");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int id = rs.getInt("spawn_id");
					L1SpawnTime.L1SpawnTimeBuilder builder = new L1SpawnTime.L1SpawnTimeBuilder(id);
					builder.TimeStart = rs.getTime("time_start");
					builder.TimeEnd = rs.getTime("time_end");
					// builder.setPeriodStart(rs.getTimestamp("period_start"));
					// builder.setPeriodEnd(rs.getTimestamp("period_end"));
					builder.DeleteAtEndTime = rs.getBoolean("delete_at_endtime");

					_times[id] = builder.build();
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
	}

}