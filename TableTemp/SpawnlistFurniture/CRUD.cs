private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistFurniture);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistFurniture.Column_item_obj_id, obj.ItemObjId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistFurniture.Column_item_obj_id, obj.ItemObjId)
.Set(SpawnlistFurniture.Column_npcid, obj.Npcid)
.Set(SpawnlistFurniture.Column_locx, obj.Locx)
.Set(SpawnlistFurniture.Column_locy, obj.Locy)
.Set(SpawnlistFurniture.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistFurniture.Column_item_obj_id, obj.ItemObjId)
.Set(SpawnlistFurniture.Column_npcid, obj.Npcid)
.Set(SpawnlistFurniture.Column_locx, obj.Locx)
.Set(SpawnlistFurniture.Column_locy, obj.Locy)
.Set(SpawnlistFurniture.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistFurniture.Column_item_obj_id, obj.ItemObjId)
.Execute();


obj.ItemObjId = dataSourceRow.getString(SpawnlistFurniture.Column_item_obj_id);
obj.Npcid = dataSourceRow.getString(SpawnlistFurniture.Column_npcid);
obj.Locx = dataSourceRow.getString(SpawnlistFurniture.Column_locx);
obj.Locy = dataSourceRow.getString(SpawnlistFurniture.Column_locy);
obj.Mapid = dataSourceRow.getString(SpawnlistFurniture.Column_mapid);

