private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Town);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Town.Column_town_id, obj.TownId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Town.Column_town_id, obj.TownId)
.Set(Town.Column_name, obj.Name)
.Set(Town.Column_leader_id, obj.LeaderId)
.Set(Town.Column_leader_name, obj.LeaderName)
.Set(Town.Column_tax_rate, obj.TaxRate)
.Set(Town.Column_tax_rate_reserved, obj.TaxRateReserved)
.Set(Town.Column_sales_money, obj.SalesMoney)
.Set(Town.Column_sales_money_yesterday, obj.SalesMoneyYesterday)
.Set(Town.Column_town_tax, obj.TownTax)
.Set(Town.Column_town_fix_tax, obj.TownFixTax)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Town.Column_town_id, obj.TownId)
.Set(Town.Column_name, obj.Name)
.Set(Town.Column_leader_id, obj.LeaderId)
.Set(Town.Column_leader_name, obj.LeaderName)
.Set(Town.Column_tax_rate, obj.TaxRate)
.Set(Town.Column_tax_rate_reserved, obj.TaxRateReserved)
.Set(Town.Column_sales_money, obj.SalesMoney)
.Set(Town.Column_sales_money_yesterday, obj.SalesMoneyYesterday)
.Set(Town.Column_town_tax, obj.TownTax)
.Set(Town.Column_town_fix_tax, obj.TownFixTax)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Town.Column_town_id, obj.TownId)
.Execute();


obj.TownId = dataSourceRow.getString(Town.Column_town_id);
obj.Name = dataSourceRow.getString(Town.Column_name);
obj.LeaderId = dataSourceRow.getString(Town.Column_leader_id);
obj.LeaderName = dataSourceRow.getString(Town.Column_leader_name);
obj.TaxRate = dataSourceRow.getString(Town.Column_tax_rate);
obj.TaxRateReserved = dataSourceRow.getString(Town.Column_tax_rate_reserved);
obj.SalesMoney = dataSourceRow.getString(Town.Column_sales_money);
obj.SalesMoneyYesterday = dataSourceRow.getString(Town.Column_sales_money_yesterday);
obj.TownTax = dataSourceRow.getString(Town.Column_town_tax);
obj.TownFixTax = dataSourceRow.getString(Town.Column_town_fix_tax);

