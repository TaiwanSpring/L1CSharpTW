using LineageServer.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
	using L1GetBackRestart = LineageServer.Server.Templates.L1GetBackRestart;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class GetBackRestartTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(GetBackRestartTable).FullName);

		private static GetBackRestartTable _instance;

		private readonly IDictionary<int, L1GetBackRestart> _getbackrestart = MapFactory.newMap();

		public static GetBackRestartTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GetBackRestartTable();
				}
				return _instance;
			}
		}

		public GetBackRestartTable()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM getback_restart");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1GetBackRestart gbr = new L1GetBackRestart();
					int area = dataSourceRow.getInt("area");
					gbr.Area = area;
					gbr.LocX = dataSourceRow.getInt("locx");
					gbr.LocY = dataSourceRow.getInt("locy");
					gbr.MapId = dataSourceRow.getShort("mapid");

					_getbackrestart[area] = gbr;
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

		public virtual L1GetBackRestart[] GetBackRestartTableList
		{
			get
			{
				return _getbackrestart.Values.ToArray();
			}
		}

	}

}