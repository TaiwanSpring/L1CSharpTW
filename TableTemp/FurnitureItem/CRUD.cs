private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.FurnitureItem);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(FurnitureItem.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(FurnitureItem.Column_item_id, obj.ItemId)
.Set(FurnitureItem.Column_npc_id, obj.NpcId)
.Set(FurnitureItem.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(FurnitureItem.Column_item_id, obj.ItemId)
.Set(FurnitureItem.Column_npc_id, obj.NpcId)
.Set(FurnitureItem.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(FurnitureItem.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(FurnitureItem.Column_item_id);
obj.NpcId = dataSourceRow.getString(FurnitureItem.Column_npc_id);
obj.Note = dataSourceRow.getString(FurnitureItem.Column_note);

