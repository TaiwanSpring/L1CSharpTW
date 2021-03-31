private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistNpc);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistNpc.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistNpc.Column_id, obj.Id)
.Set(SpawnlistNpc.Column_location, obj.Location)
.Set(SpawnlistNpc.Column_count, obj.Count)
.Set(SpawnlistNpc.Column_npc_templateid, obj.NpcTemplateid)
.Set(SpawnlistNpc.Column_locx, obj.Locx)
.Set(SpawnlistNpc.Column_locy, obj.Locy)
.Set(SpawnlistNpc.Column_randomx, obj.Randomx)
.Set(SpawnlistNpc.Column_randomy, obj.Randomy)
.Set(SpawnlistNpc.Column_heading, obj.Heading)
.Set(SpawnlistNpc.Column_respawn_delay, obj.RespawnDelay)
.Set(SpawnlistNpc.Column_mapid, obj.Mapid)
.Set(SpawnlistNpc.Column_movement_distance, obj.MovementDistance)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistNpc.Column_id, obj.Id)
.Set(SpawnlistNpc.Column_location, obj.Location)
.Set(SpawnlistNpc.Column_count, obj.Count)
.Set(SpawnlistNpc.Column_npc_templateid, obj.NpcTemplateid)
.Set(SpawnlistNpc.Column_locx, obj.Locx)
.Set(SpawnlistNpc.Column_locy, obj.Locy)
.Set(SpawnlistNpc.Column_randomx, obj.Randomx)
.Set(SpawnlistNpc.Column_randomy, obj.Randomy)
.Set(SpawnlistNpc.Column_heading, obj.Heading)
.Set(SpawnlistNpc.Column_respawn_delay, obj.RespawnDelay)
.Set(SpawnlistNpc.Column_mapid, obj.Mapid)
.Set(SpawnlistNpc.Column_movement_distance, obj.MovementDistance)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistNpc.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(SpawnlistNpc.Column_id);
obj.Location = dataSourceRow.getString(SpawnlistNpc.Column_location);
obj.Count = dataSourceRow.getString(SpawnlistNpc.Column_count);
obj.NpcTemplateid = dataSourceRow.getString(SpawnlistNpc.Column_npc_templateid);
obj.Locx = dataSourceRow.getString(SpawnlistNpc.Column_locx);
obj.Locy = dataSourceRow.getString(SpawnlistNpc.Column_locy);
obj.Randomx = dataSourceRow.getString(SpawnlistNpc.Column_randomx);
obj.Randomy = dataSourceRow.getString(SpawnlistNpc.Column_randomy);
obj.Heading = dataSourceRow.getString(SpawnlistNpc.Column_heading);
obj.RespawnDelay = dataSourceRow.getString(SpawnlistNpc.Column_respawn_delay);
obj.Mapid = dataSourceRow.getString(SpawnlistNpc.Column_mapid);
obj.MovementDistance = dataSourceRow.getString(SpawnlistNpc.Column_movement_distance);

