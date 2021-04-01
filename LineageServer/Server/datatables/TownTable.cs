using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
	class TownTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Town);

		private static TownTable _instance;

		private readonly IDictionary<int, L1Town> _towns = MapFactory.NewConcurrentMap<int, L1Town>();

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
			_towns.Clear();

			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			int townid;

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1Town town = new L1Town();
				townid = dataSourceRow.getInt(Town.Column_town_id);
				town.set_townid(townid);
				town.set_name(dataSourceRow.getString(Town.Column_name));
				town.set_leader_id(dataSourceRow.getInt(Town.Column_leader_id));
				town.set_leader_name(dataSourceRow.getString(Town.Column_leader_name));
				town.set_tax_rate(dataSourceRow.getInt(Town.Column_tax_rate));
				town.set_tax_rate_reserved(dataSourceRow.getInt(Town.Column_tax_rate_reserved));
				town.set_sales_money(dataSourceRow.getInt(Town.Column_sales_money));
				town.set_sales_money_yesterday(dataSourceRow.getInt(Town.Column_sales_money_yesterday));
				town.set_town_tax(dataSourceRow.getInt(Town.Column_town_tax));
				town.set_town_fix_tax(dataSourceRow.getInt(Town.Column_town_fix_tax));
				_towns[townid] = town;
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
			return ( town.get_leader_id() == pc.Id );
		}

		public virtual void addSalesMoney(int town_id, int salesMoney)
		{
			lock (this)
			{
				L1Town town = TownTable.Instance.getTownTable(town_id);
				int townTaxRate = town.get_tax_rate();

				int townTax = salesMoney / 100 * townTaxRate;
				int townFixTax = salesMoney / 100 * 2;

				if (( townTax <= 0 ) && ( townTaxRate > 0 ))
				{
					townTax = 1;
				}
				if (( townFixTax <= 0 ) && ( townTaxRate > 0 ))
				{
					townFixTax = 1;
				}

				IDataSourceRow dataSourceRow = dataSource.NewRow();
				dataSourceRow.Update()
				.Where(Town.Column_town_id, town_id)
				.Set(Town.Column_sales_money, town.get_sales_money() + salesMoney)
				.Set(Town.Column_town_tax, town.get_town_tax() + townTax)
				.Set(Town.Column_town_fix_tax, town.get_town_fix_tax() + townFixTax)
				.Execute();

				town.set_sales_money(town.get_sales_money() + salesMoney);
				town.set_town_tax(town.get_town_tax() + townTax);
				town.set_town_fix_tax(town.get_town_fix_tax() + townFixTax);
			}
		}

		public virtual void updateTaxRate()
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
			.SetToColumn(Town.Column_tax_rate, Town.Column_tax_rate_reserved)
			.Execute();
		}

		public virtual void updateSalesMoneyYesterday()
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
			.SetToColumn(Town.Column_sales_money_yesterday, Town.Column_sales_money)
			.Set(Town.Column_sales_money, 0)
			.Execute();
		}
	}

}