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
namespace LineageServer.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1MobGroup = LineageServer.Server.Templates.L1MobGroup;
	using L1NpcCount = LineageServer.Server.Templates.L1NpcCount;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using ListFactory = LineageServer.Utils.ListFactory;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class MobGroupTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(MobGroupTable).FullName);

		private static MobGroupTable _instance;

		private readonly IDictionary<int, L1MobGroup> _mobGroupIndex = MapFactory.newMap();

		public static MobGroupTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MobGroupTable();
				}
				return _instance;
			}
		}

		private MobGroupTable()
		{
			loadMobGroup();
		}

		private void loadMobGroup()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM mobgroup");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int mobGroupId = dataSourceRow.getInt("id");
					bool isRemoveGroup = (dataSourceRow.getBoolean("remove_group_if_leader_die"));
					int leaderId = dataSourceRow.getInt("leader_id");
					IList<L1NpcCount> minions = ListFactory.newList();
					for (int i = 1; i <= 7; i++)
					{
						int id = dataSourceRow.getInt("minion" + i + "_id");
						int count = dataSourceRow.getInt("minion" + i + "_count");
						minions.Add(new L1NpcCount(id, count));
					}
					L1MobGroup mobGroup = new L1MobGroup(mobGroupId, leaderId, minions, isRemoveGroup);
					_mobGroupIndex[mobGroupId] = mobGroup;
				}
				_log.config("MOBグループリスト " + _mobGroupIndex.Count + "件ロード");
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating mobgroup table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual L1MobGroup getTemplate(int mobGroupId)
		{
			return _mobGroupIndex[mobGroupId];
		}

	}

}