private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistDoor);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistDoor.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistDoor.Column_id, obj.Id)
.Set(SpawnlistDoor.Column_location, obj.Location)
.Set(SpawnlistDoor.Column_gfxid, obj.Gfxid)
.Set(SpawnlistDoor.Column_locx, obj.Locx)
.Set(SpawnlistDoor.Column_locy, obj.Locy)
.Set(SpawnlistDoor.Column_mapid, obj.Mapid)
.Set(SpawnlistDoor.Column_hp, obj.Hp)
.Set(SpawnlistDoor.Column_keeper, obj.Keeper)
.Set(SpawnlistDoor.Column_isOpening, obj.IsOpening)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistDoor.Column_id, obj.Id)
.Set(SpawnlistDoor.Column_location, obj.Location)
.Set(SpawnlistDoor.Column_gfxid, obj.Gfxid)
.Set(SpawnlistDoor.Column_locx, obj.Locx)
.Set(SpawnlistDoor.Column_locy, obj.Locy)
.Set(SpawnlistDoor.Column_mapid, obj.Mapid)
.Set(SpawnlistDoor.Column_hp, obj.Hp)
.Set(SpawnlistDoor.Column_keeper, obj.Keeper)
.Set(SpawnlistDoor.Column_isOpening, obj.IsOpening)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistDoor.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(SpawnlistDoor.Column_id);
obj.Location = dataSourceRow.getString(SpawnlistDoor.Column_location);
obj.Gfxid = dataSourceRow.getString(SpawnlistDoor.Column_gfxid);
obj.Locx = dataSourceRow.getString(SpawnlistDoor.Column_locx);
obj.Locy = dataSourceRow.getString(SpawnlistDoor.Column_locy);
obj.Mapid = dataSourceRow.getString(SpawnlistDoor.Column_mapid);
obj.Hp = dataSourceRow.getString(SpawnlistDoor.Column_hp);
obj.Keeper = dataSourceRow.getString(SpawnlistDoor.Column_keeper);
obj.IsOpening = dataSourceRow.getString(SpawnlistDoor.Column_isOpening);

