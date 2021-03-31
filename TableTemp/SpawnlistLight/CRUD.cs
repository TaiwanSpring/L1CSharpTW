private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistLight);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistLight.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistLight.Column_id, obj.Id)
.Set(SpawnlistLight.Column_npcid, obj.Npcid)
.Set(SpawnlistLight.Column_locx, obj.Locx)
.Set(SpawnlistLight.Column_locy, obj.Locy)
.Set(SpawnlistLight.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistLight.Column_id, obj.Id)
.Set(SpawnlistLight.Column_npcid, obj.Npcid)
.Set(SpawnlistLight.Column_locx, obj.Locx)
.Set(SpawnlistLight.Column_locy, obj.Locy)
.Set(SpawnlistLight.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistLight.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(SpawnlistLight.Column_id);
obj.Npcid = dataSourceRow.getString(SpawnlistLight.Column_npcid);
obj.Locx = dataSourceRow.getString(SpawnlistLight.Column_locx);
obj.Locy = dataSourceRow.getString(SpawnlistLight.Column_locy);
obj.Mapid = dataSourceRow.getString(SpawnlistLight.Column_mapid);

