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
	using L1NpcTalkData = LineageServer.Server.Model.L1NpcTalkData;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class NPCTalkDataTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(NPCTalkDataTable).FullName);

		private static NPCTalkDataTable _instance;

		private IDictionary<int, L1NpcTalkData> _datatable = MapFactory.NewMap();

		public static NPCTalkDataTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NPCTalkDataTable();
				}
				return _instance;
			}
		}

		private NPCTalkDataTable()
		{
			parseList();
		}

		private void parseList()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM npcaction");

				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1NpcTalkData l1npctalkdata = new L1NpcTalkData();
					l1npctalkdata.NpcID = dataSourceRow.getInt(1);
					l1npctalkdata.NormalAction = dataSourceRow.getString(2);
					l1npctalkdata.CaoticAction = dataSourceRow.getString(3);
					l1npctalkdata.TeleportURL = dataSourceRow.getString(4);
					l1npctalkdata.TeleportURLA = dataSourceRow.getString(5);
					_datatable[l1npctalkdata.NpcID] = l1npctalkdata;
				}
				_log.config("NPCアクションリスト " + _datatable.Count + "件ロード");
			}
			catch (SQLException e)
			{
				_log.warning("error while creating npc action table " + e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual L1NpcTalkData getTemplate(int i)
		{
			return _datatable[i];
		}

	}

}