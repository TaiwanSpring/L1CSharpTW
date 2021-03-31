private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Castle);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Castle.Column_castle_id, obj.CastleId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Castle.Column_castle_id, obj.CastleId)
.Set(Castle.Column_name, obj.Name)
.Set(Castle.Column_war_time, obj.WarTime)
.Set(Castle.Column_tax_rate, obj.TaxRate)
.Set(Castle.Column_public_money, obj.PublicMoney)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Castle.Column_castle_id, obj.CastleId)
.Set(Castle.Column_name, obj.Name)
.Set(Castle.Column_war_time, obj.WarTime)
.Set(Castle.Column_tax_rate, obj.TaxRate)
.Set(Castle.Column_public_money, obj.PublicMoney)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Castle.Column_castle_id, obj.CastleId)
.Execute();


obj.CastleId = dataSourceRow.getString(Castle.Column_castle_id);
obj.Name = dataSourceRow.getString(Castle.Column_name);
obj.WarTime = dataSourceRow.getString(Castle.Column_war_time);
obj.TaxRate = dataSourceRow.getString(Castle.Column_tax_rate);
obj.PublicMoney = dataSourceRow.getString(Castle.Column_public_money);

