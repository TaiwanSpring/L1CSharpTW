private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Resolvent);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Resolvent.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Resolvent.Column_item_id, obj.ItemId)
.Set(Resolvent.Column_note, obj.Note)
.Set(Resolvent.Column_crystal_count, obj.CrystalCount)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Resolvent.Column_item_id, obj.ItemId)
.Set(Resolvent.Column_note, obj.Note)
.Set(Resolvent.Column_crystal_count, obj.CrystalCount)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Resolvent.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(Resolvent.Column_item_id);
obj.Note = dataSourceRow.getString(Resolvent.Column_note);
obj.CrystalCount = dataSourceRow.getString(Resolvent.Column_crystal_count);

