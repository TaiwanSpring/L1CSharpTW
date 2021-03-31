private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistBoss);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistBoss.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistBoss.Column_id, obj.Id)
.Set(SpawnlistBoss.Column_location, obj.Location)
.Set(SpawnlistBoss.Column_cycle_type, obj.CycleType)
.Set(SpawnlistBoss.Column_count, obj.Count)
.Set(SpawnlistBoss.Column_npc_id, obj.NpcId)
.Set(SpawnlistBoss.Column_group_id, obj.GroupId)
.Set(SpawnlistBoss.Column_locx, obj.Locx)
.Set(SpawnlistBoss.Column_locy, obj.Locy)
.Set(SpawnlistBoss.Column_randomx, obj.Randomx)
.Set(SpawnlistBoss.Column_randomy, obj.Randomy)
.Set(SpawnlistBoss.Column_locx1, obj.Locx1)
.Set(SpawnlistBoss.Column_locy1, obj.Locy1)
.Set(SpawnlistBoss.Column_locx2, obj.Locx2)
.Set(SpawnlistBoss.Column_locy2, obj.Locy2)
.Set(SpawnlistBoss.Column_heading, obj.Heading)
.Set(SpawnlistBoss.Column_mapid, obj.Mapid)
.Set(SpawnlistBoss.Column_respawn_screen, obj.RespawnScreen)
.Set(SpawnlistBoss.Column_movement_distance, obj.MovementDistance)
.Set(SpawnlistBoss.Column_rest, obj.Rest)
.Set(SpawnlistBoss.Column_spawn_type, obj.SpawnType)
.Set(SpawnlistBoss.Column_percentage, obj.Percentage)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistBoss.Column_id, obj.Id)
.Set(SpawnlistBoss.Column_location, obj.Location)
.Set(SpawnlistBoss.Column_cycle_type, obj.CycleType)
.Set(SpawnlistBoss.Column_count, obj.Count)
.Set(SpawnlistBoss.Column_npc_id, obj.NpcId)
.Set(SpawnlistBoss.Column_group_id, obj.GroupId)
.Set(SpawnlistBoss.Column_locx, obj.Locx)
.Set(SpawnlistBoss.Column_locy, obj.Locy)
.Set(SpawnlistBoss.Column_randomx, obj.Randomx)
.Set(SpawnlistBoss.Column_randomy, obj.Randomy)
.Set(SpawnlistBoss.Column_locx1, obj.Locx1)
.Set(SpawnlistBoss.Column_locy1, obj.Locy1)
.Set(SpawnlistBoss.Column_locx2, obj.Locx2)
.Set(SpawnlistBoss.Column_locy2, obj.Locy2)
.Set(SpawnlistBoss.Column_heading, obj.Heading)
.Set(SpawnlistBoss.Column_mapid, obj.Mapid)
.Set(SpawnlistBoss.Column_respawn_screen, obj.RespawnScreen)
.Set(SpawnlistBoss.Column_movement_distance, obj.MovementDistance)
.Set(SpawnlistBoss.Column_rest, obj.Rest)
.Set(SpawnlistBoss.Column_spawn_type, obj.SpawnType)
.Set(SpawnlistBoss.Column_percentage, obj.Percentage)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistBoss.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(SpawnlistBoss.Column_id);
obj.Location = dataSourceRow.getString(SpawnlistBoss.Column_location);
obj.CycleType = dataSourceRow.getString(SpawnlistBoss.Column_cycle_type);
obj.Count = dataSourceRow.getString(SpawnlistBoss.Column_count);
obj.NpcId = dataSourceRow.getString(SpawnlistBoss.Column_npc_id);
obj.GroupId = dataSourceRow.getString(SpawnlistBoss.Column_group_id);
obj.Locx = dataSourceRow.getString(SpawnlistBoss.Column_locx);
obj.Locy = dataSourceRow.getString(SpawnlistBoss.Column_locy);
obj.Randomx = dataSourceRow.getString(SpawnlistBoss.Column_randomx);
obj.Randomy = dataSourceRow.getString(SpawnlistBoss.Column_randomy);
obj.Locx1 = dataSourceRow.getString(SpawnlistBoss.Column_locx1);
obj.Locy1 = dataSourceRow.getString(SpawnlistBoss.Column_locy1);
obj.Locx2 = dataSourceRow.getString(SpawnlistBoss.Column_locx2);
obj.Locy2 = dataSourceRow.getString(SpawnlistBoss.Column_locy2);
obj.Heading = dataSourceRow.getString(SpawnlistBoss.Column_heading);
obj.Mapid = dataSourceRow.getString(SpawnlistBoss.Column_mapid);
obj.RespawnScreen = dataSourceRow.getString(SpawnlistBoss.Column_respawn_screen);
obj.MovementDistance = dataSourceRow.getString(SpawnlistBoss.Column_movement_distance);
obj.Rest = dataSourceRow.getString(SpawnlistBoss.Column_rest);
obj.SpawnType = dataSourceRow.getString(SpawnlistBoss.Column_spawn_type);
obj.Percentage = dataSourceRow.getString(SpawnlistBoss.Column_percentage);

