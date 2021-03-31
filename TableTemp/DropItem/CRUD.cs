private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.DropItem);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(DropItem.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(DropItem.Column_item_id, obj.ItemId)
.Set(DropItem.Column_drop_rate, obj.DropRate)
.Set(DropItem.Column_drop_amount, obj.DropAmount)
.Set(DropItem.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(DropItem.Column_item_id, obj.ItemId)
.Set(DropItem.Column_drop_rate, obj.DropRate)
.Set(DropItem.Column_drop_amount, obj.DropAmount)
.Set(DropItem.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(DropItem.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(DropItem.Column_item_id);
obj.DropRate = dataSourceRow.getString(DropItem.Column_drop_rate);
obj.DropAmount = dataSourceRow.getString(DropItem.Column_drop_amount);
obj.Note = dataSourceRow.getString(DropItem.Column_note);

