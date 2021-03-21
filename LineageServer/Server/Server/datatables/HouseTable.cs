using System;
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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1House = LineageServer.Server.Server.Templates.L1House;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class HouseTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(HouseTable).FullName);

		private static HouseTable _instance;

		private readonly IDictionary<int, L1House> _house = Maps.newConcurrentMap();

		public static HouseTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new HouseTable();
				}
				return _instance;
			}
		}

		private DateTime timestampToCalendar(Timestamp ts)
		{
			DateTime cal = new DateTime();
			cal.TimeInMillis = ts.Time;
			return cal;
		}

		public HouseTable()
		{
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM house ORDER BY house_id");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1House house = new L1House();
					house.HouseId = rs.getInt(1);
					house.HouseName = rs.getString(2);
					house.HouseArea = rs.getInt(3);
					house.Location = rs.getString(4);
					house.KeeperId = rs.getInt(5);
					house.OnSale = rs.getInt(6) == 1 ? true : false;
					house.PurchaseBasement = rs.getInt(7) == 1 ? true : false;
					house.TaxDeadline = timestampToCalendar((Timestamp) rs.getObject(8));
					_house[house.HouseId] = house;
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

		public virtual L1House[] HouseTableList
		{
			get
			{
				return _house.Values.ToArray();
			}
		}

		public virtual L1House getHouseTable(int houseId)
		{
			return _house[houseId];
		}

		public virtual void updateHouse(L1House house)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE house SET house_name=?, house_area=?, location=?, keeper_id=?, is_on_sale=?, is_purchase_basement=?, tax_deadline=? WHERE house_id=?");
				pstm.setString(1, house.HouseName);
				pstm.setInt(2, house.HouseArea);
				pstm.setString(3, house.Location);
				pstm.setInt(4, house.KeeperId);
				pstm.setInt(5, house.OnSale == true ? 1 : 0);
				pstm.setInt(6, house.PurchaseBasement == true ? 1 : 0);
				SimpleDateFormat sdf = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
				string fm = sdf.format(house.TaxDeadline);
				pstm.setString(7, fm);
				pstm.setInt(8, house.HouseId);
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

		public static IList<int> HouseIdList
		{
			get
			{
				IList<int> houseIdList = Lists.newList();
    
				Connection con = null;
				PreparedStatement pstm = null;
				ResultSet rs = null;
    
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("SELECT house_id FROM house ORDER BY house_id");
					rs = pstm.executeQuery();
					while (rs.next())
					{
						int houseId = rs.getInt("house_id");
						houseIdList.Add(Convert.ToInt32(houseId));
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
    
				return houseIdList;
			}
		}
	}

}