private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistTrap);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistTrap.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistTrap.Column_id, obj.Id)
.Set(SpawnlistTrap.Column_note, obj.Note)
.Set(SpawnlistTrap.Column_trapId, obj.TrapId)
.Set(SpawnlistTrap.Column_mapId, obj.MapId)
.Set(SpawnlistTrap.Column_locX, obj.LocX)
.Set(SpawnlistTrap.Column_locY, obj.LocY)
.Set(SpawnlistTrap.Column_locRndX, obj.LocRndX)
.Set(SpawnlistTrap.Column_locRndY, obj.LocRndY)
.Set(SpawnlistTrap.Column_count, obj.Count)
.Set(SpawnlistTrap.Column_span, obj.Span)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistTrap.Column_id, obj.Id)
.Set(SpawnlistTrap.Column_note, obj.Note)
.Set(SpawnlistTrap.Column_trapId, obj.TrapId)
.Set(SpawnlistTrap.Column_mapId, obj.MapId)
.Set(SpawnlistTrap.Column_locX, obj.LocX)
.Set(SpawnlistTrap.Column_locY, obj.LocY)
.Set(SpawnlistTrap.Column_locRndX, obj.LocRndX)
.Set(SpawnlistTrap.Column_locRndY, obj.LocRndY)
.Set(SpawnlistTrap.Column_count, obj.Count)
.Set(SpawnlistTrap.Column_span, obj.Span)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistTrap.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(SpawnlistTrap.Column_id);
obj.Note = dataSourceRow.getString(SpawnlistTrap.Column_note);
obj.TrapId = dataSourceRow.getString(SpawnlistTrap.Column_trapId);
obj.MapId = dataSourceRow.getString(SpawnlistTrap.Column_mapId);
obj.LocX = dataSourceRow.getString(SpawnlistTrap.Column_locX);
obj.LocY = dataSourceRow.getString(SpawnlistTrap.Column_locY);
obj.LocRndX = dataSourceRow.getString(SpawnlistTrap.Column_locRndX);
obj.LocRndY = dataSourceRow.getString(SpawnlistTrap.Column_locRndY);
obj.Count = dataSourceRow.getString(SpawnlistTrap.Column_count);
obj.Span = dataSourceRow.getString(SpawnlistTrap.Column_span);

