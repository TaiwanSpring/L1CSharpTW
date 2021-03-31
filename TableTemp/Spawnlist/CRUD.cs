private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Spawnlist);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Spawnlist.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Spawnlist.Column_id, obj.Id)
.Set(Spawnlist.Column_location, obj.Location)
.Set(Spawnlist.Column_count, obj.Count)
.Set(Spawnlist.Column_npc_templateid, obj.NpcTemplateid)
.Set(Spawnlist.Column_group_id, obj.GroupId)
.Set(Spawnlist.Column_locx, obj.Locx)
.Set(Spawnlist.Column_locy, obj.Locy)
.Set(Spawnlist.Column_randomx, obj.Randomx)
.Set(Spawnlist.Column_randomy, obj.Randomy)
.Set(Spawnlist.Column_locx1, obj.Locx1)
.Set(Spawnlist.Column_locy1, obj.Locy1)
.Set(Spawnlist.Column_locx2, obj.Locx2)
.Set(Spawnlist.Column_locy2, obj.Locy2)
.Set(Spawnlist.Column_heading, obj.Heading)
.Set(Spawnlist.Column_min_respawn_delay, obj.MinRespawnDelay)
.Set(Spawnlist.Column_max_respawn_delay, obj.MaxRespawnDelay)
.Set(Spawnlist.Column_mapid, obj.Mapid)
.Set(Spawnlist.Column_respawn_screen, obj.RespawnScreen)
.Set(Spawnlist.Column_movement_distance, obj.MovementDistance)
.Set(Spawnlist.Column_rest, obj.Rest)
.Set(Spawnlist.Column_near_spawn, obj.NearSpawn)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Spawnlist.Column_id, obj.Id)
.Set(Spawnlist.Column_location, obj.Location)
.Set(Spawnlist.Column_count, obj.Count)
.Set(Spawnlist.Column_npc_templateid, obj.NpcTemplateid)
.Set(Spawnlist.Column_group_id, obj.GroupId)
.Set(Spawnlist.Column_locx, obj.Locx)
.Set(Spawnlist.Column_locy, obj.Locy)
.Set(Spawnlist.Column_randomx, obj.Randomx)
.Set(Spawnlist.Column_randomy, obj.Randomy)
.Set(Spawnlist.Column_locx1, obj.Locx1)
.Set(Spawnlist.Column_locy1, obj.Locy1)
.Set(Spawnlist.Column_locx2, obj.Locx2)
.Set(Spawnlist.Column_locy2, obj.Locy2)
.Set(Spawnlist.Column_heading, obj.Heading)
.Set(Spawnlist.Column_min_respawn_delay, obj.MinRespawnDelay)
.Set(Spawnlist.Column_max_respawn_delay, obj.MaxRespawnDelay)
.Set(Spawnlist.Column_mapid, obj.Mapid)
.Set(Spawnlist.Column_respawn_screen, obj.RespawnScreen)
.Set(Spawnlist.Column_movement_distance, obj.MovementDistance)
.Set(Spawnlist.Column_rest, obj.Rest)
.Set(Spawnlist.Column_near_spawn, obj.NearSpawn)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Spawnlist.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Spawnlist.Column_id);
obj.Location = dataSourceRow.getString(Spawnlist.Column_location);
obj.Count = dataSourceRow.getString(Spawnlist.Column_count);
obj.NpcTemplateid = dataSourceRow.getString(Spawnlist.Column_npc_templateid);
obj.GroupId = dataSourceRow.getString(Spawnlist.Column_group_id);
obj.Locx = dataSourceRow.getString(Spawnlist.Column_locx);
obj.Locy = dataSourceRow.getString(Spawnlist.Column_locy);
obj.Randomx = dataSourceRow.getString(Spawnlist.Column_randomx);
obj.Randomy = dataSourceRow.getString(Spawnlist.Column_randomy);
obj.Locx1 = dataSourceRow.getString(Spawnlist.Column_locx1);
obj.Locy1 = dataSourceRow.getString(Spawnlist.Column_locy1);
obj.Locx2 = dataSourceRow.getString(Spawnlist.Column_locx2);
obj.Locy2 = dataSourceRow.getString(Spawnlist.Column_locy2);
obj.Heading = dataSourceRow.getString(Spawnlist.Column_heading);
obj.MinRespawnDelay = dataSourceRow.getString(Spawnlist.Column_min_respawn_delay);
obj.MaxRespawnDelay = dataSourceRow.getString(Spawnlist.Column_max_respawn_delay);
obj.Mapid = dataSourceRow.getString(Spawnlist.Column_mapid);
obj.RespawnScreen = dataSourceRow.getString(Spawnlist.Column_respawn_screen);
obj.MovementDistance = dataSourceRow.getString(Spawnlist.Column_movement_distance);
obj.Rest = dataSourceRow.getString(Spawnlist.Column_rest);
obj.NearSpawn = dataSourceRow.getString(Spawnlist.Column_near_spawn);

