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
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1Town = LineageServer.Server.Templates.L1Town;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class TownTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(TownTable).FullName);

		private static TownTable _instance;

		private readonly IDictionary<int, L1Town> _towns = MapFactory.newConcurrentMap();

		public static TownTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TownTable();
				}
    
				return _instance;
			}
		}

		private TownTable()
		{
			load();
		}

		public virtual void load()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			_towns.Clear();

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM town");

				int townid;
				rs = pstm.executeQuery();

				while (rs.next())
				{
					L1Town town = new L1Town();
					townid = dataSourceRow.getInt("town_id");
					town.set_townid(townid);
					town.set_name(dataSourceRow.getString("name"));
					town.set_leader_id(dataSourceRow.getInt("leader_id"));
					town.set_leader_name(dataSourceRow.getString("leader_name"));
					town.set_tax_rate(dataSourceRow.getInt("tax_rate"));
					town.set_tax_rate_reserved(dataSourceRow.getInt("tax_rate_reserved"));
					town.set_sales_money(dataSourceRow.getInt("sales_money"));
					town.set_sales_money_yesterday(dataSourceRow.getInt("sales_money_yesterday"));
					town.set_town_tax(dataSourceRow.getInt("town_tax"));
					town.set_town_fix_tax(dataSourceRow.getInt("town_fix_tax"));

					_towns[townid] = town;
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

		public virtual L1Town[] TownTableList
		{
			get
			{
				return _towns.Values.ToArray();
			}
		}

		public virtual L1Town getTownTable(int id)
		{
			return _towns[id];
		}

		public virtual bool isLeader(L1PcInstance pc, int town_id)
		{
			L1Town town = getTownTable(town_id);
			return (town.get_leader_id() == pc.Id);
		}

		public virtual void addSalesMoney(int town_id, int salesMoney)
		{
			lock (this)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
        
				L1Town town = TownTable.Instance.getTownTable(town_id);
				int townTaxRate = town.get_tax_rate();
        
				int townTax = salesMoney / 100 * townTaxRate;
				int townFixTax = salesMoney / 100 * 2;
        
				if ((townTax <= 0) && (townTaxRate > 0))
				{
					townTax = 1;
				}
				if ((townFixTax <= 0) && (townTaxRate > 0))
				{
					townFixTax = 1;
				}
        
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("UPDATE town SET sales_money = sales_money + ?, town_tax = town_tax + ?, town_fix_tax = town_fix_tax + ? WHERE town_id = ?");
					pstm.setInt(1, salesMoney);
					pstm.setInt(2, townTax);
					pstm.setInt(3, townFixTax);
					pstm.setInt(4, town_id);
					pstm.execute();
        
					town.set_sales_money(town.get_sales_money() + salesMoney);
					town.set_town_tax(town.get_town_tax() + townTax);
					town.set_town_fix_tax(town.get_town_fix_tax() + townFixTax);
        
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
		}

		public virtual void updateTaxRate()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE town SET tax_rate = tax_rate_reserved");
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

		public virtual void updateSalesMoneyYesterday()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE town SET sales_money_yesterday = sales_money, sales_money = 0");
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
	}

}