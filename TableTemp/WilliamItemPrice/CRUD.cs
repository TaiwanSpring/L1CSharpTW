private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.WilliamItemPrice);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(WilliamItemPrice.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(WilliamItemPrice.Column_item_id, obj.ItemId)
.Set(WilliamItemPrice.Column_name, obj.Name)
.Set(WilliamItemPrice.Column_price, obj.Price)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(WilliamItemPrice.Column_item_id, obj.ItemId)
.Set(WilliamItemPrice.Column_name, obj.Name)
.Set(WilliamItemPrice.Column_price, obj.Price)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(WilliamItemPrice.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(WilliamItemPrice.Column_item_id);
obj.Name = dataSourceRow.getString(WilliamItemPrice.Column_name);
obj.Price = dataSourceRow.getString(WilliamItemPrice.Column_price);

