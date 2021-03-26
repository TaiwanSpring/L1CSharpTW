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
namespace LineageServer.Server.Server
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.Utils.SQLUtil;

	public class IdFactory
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(IdFactory).FullName);

		private int _curId;

		private object _monitor = new object();

		private const int FIRST_ID = 0x10000000;

		private static IdFactory _instance = new IdFactory();

		private IdFactory()
		{
			loadState();
		}

		public static IdFactory Instance
		{
			get
			{
				return _instance;
			}
		}

		public virtual int nextId()
		{
			lock (_monitor)
			{
				return _curId++;
			}
		}

		private void loadState()
		{
			// 取得資料庫中最大的ID+1
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("select max(id)+1 as nextid from (select id from character_items union all select id from character_teleport union all select id from character_warehouse union all select id from character_elf_warehouse union all select objid as id from characters union all select clan_id as id from clan_data union all select index_id as id from clan_members union all select id from clan_warehouse union all select objid as id from pets ) t");
				rs = pstm.executeQuery();

				int id = 0;
				if (rs.next())
				{
					id = dataSourceRow.getInt("nextid");
				}
				if (id < FIRST_ID)
				{
					id = FIRST_ID;
				}
				_curId = id;
				_log.info("目前的物件ID: " + _curId);
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