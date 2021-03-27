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
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public sealed class ResolventTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(ResolventTable).FullName);

		private static ResolventTable _instance;

		private readonly IDictionary<int, int> _resolvent = MapFactory.newMap();

		public static ResolventTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ResolventTable();
				}
				return _instance;
			}
		}

		private ResolventTable()
		{
			loadMapsFromDatabase();
		}

		private void loadMapsFromDatabase()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM resolvent");

				for (rs = pstm.executeQuery(); rs.next();)
				{
					int itemId = dataSourceRow.getInt("item_id");
					int crystalCount = dataSourceRow.getInt("crystal_count");

					_resolvent[itemId] = crystalCount;
				}

				_log.config("resolvent " + _resolvent.Count);
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

		public int getCrystalCount(int itemId)
		{
			int crystalCount = 0;
			if (_resolvent.ContainsKey(itemId))
			{
				crystalCount = _resolvent[itemId];
			}
			return crystalCount;
		}

	}

}