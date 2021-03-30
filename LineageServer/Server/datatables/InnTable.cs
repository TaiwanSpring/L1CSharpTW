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

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Inn = LineageServer.Server.Templates.L1Inn;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	public class InnTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(InnTable).FullName);

		private class Inn
		{
			internal readonly IDictionary<int, L1Inn> _inn = MapFactory.NewMap();
		}

		private static readonly IDictionary<int, Inn> _dataMap = MapFactory.NewMap();

		private static InnTable _instance;

		public static InnTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new InnTable();
				}
				return _instance;
			}
		}

		private InnTable()
		{
			load();
		}

		private void load()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			Inn inn = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM inn");

				rs = pstm.executeQuery();
				L1Inn l1inn;
				int roomNumber;
				while (rs.next())
				{
					int key = dataSourceRow.getInt("npcid");
					if (!_dataMap.ContainsKey(key))
					{
						inn = new Inn();
						_dataMap[key] = inn;
					}
					else
					{
						inn = _dataMap[key];
					}

					l1inn = new L1Inn();
					l1inn.InnNpcId = dataSourceRow.getInt("npcid");
					roomNumber = dataSourceRow.getInt("room_number");
					l1inn.RoomNumber = roomNumber;
					l1inn.KeyId = dataSourceRow.getInt("key_id");
					l1inn.LodgerId = dataSourceRow.getInt("lodger_id");
					l1inn.Hall = dataSourceRow.getBoolean("hall");
					l1inn.DueTime = dataSourceRow.getTimestamp("due_time");

					inn._inn[Convert.ToInt32(roomNumber)] = l1inn;
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

		public virtual void updateInn(L1Inn inn)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE inn SET key_id=?,lodger_id=?,hall=?,due_time=? WHERE npcid=? and room_number=?");

				pstm.setInt(1, inn.KeyId);
				pstm.setInt(2, inn.LodgerId);
				pstm.setBoolean(3, inn.Hall);
				pstm.setTimestamp(4, inn.DueTime);
				pstm.setInt(5, inn.InnNpcId);
				pstm.setInt(6, inn.RoomNumber);
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

		public virtual L1Inn getTemplate(int npcid, int roomNumber)
		{
			if (_dataMap.ContainsKey(npcid))
			{
				return _dataMap[npcid]._inn[roomNumber];
			}
			return null;
		}
	}

}