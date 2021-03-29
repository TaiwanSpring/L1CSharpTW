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
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class InnKeyTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(InnKeyTable).FullName);

		private InnKeyTable()
		{
		}

		public static void StoreKey(L1ItemInstance item)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO inn_key SET item_obj_id=?, key_id=?, npc_id=?, hall=?, due_time=?");

				pstm.setInt(1, item.Id);
				pstm.setInt(2, item.KeyId);
				pstm.setInt(3, item.InnNpcId);
				pstm.setBoolean(4, item.checkRoomOrHall());
				pstm.setTimestamp(5, item.DueTime);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public static void DeleteKey(L1ItemInstance item)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM inn_key WHERE item_obj_id=?");
				pstm.setInt(1, item.Id);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
		}

		public static bool checkey(L1ItemInstance item)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM inn_key WHERE item_obj_id=?");

				pstm.setInt(1, item.Id);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int itemObj = dataSourceRow.getInt("item_obj_id");
					if (item.Id == itemObj)
					{
						item.KeyId = dataSourceRow.getInt("key_id");
						item.InnNpcId = dataSourceRow.getInt("npc_id");
						item.Hall = dataSourceRow.getBoolean("hall");
						item.DueTime = dataSourceRow.getTimestamp("due_time");
						return true;
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
			return false;
		}

	}

}