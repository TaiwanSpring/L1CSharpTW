private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistUb);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistUb.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistUb.Column_id, obj.Id)
.Set(SpawnlistUb.Column_ub_id, obj.UbId)
.Set(SpawnlistUb.Column_pattern, obj.Pattern)
.Set(SpawnlistUb.Column_group_id, obj.GroupId)
.Set(SpawnlistUb.Column_location, obj.Location)
.Set(SpawnlistUb.Column_npc_templateid, obj.NpcTemplateid)
.Set(SpawnlistUb.Column_count, obj.Count)
.Set(SpawnlistUb.Column_spawn_delay, obj.SpawnDelay)
.Set(SpawnlistUb.Column_seal_count, obj.SealCount)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistUb.Column_id, obj.Id)
.Set(SpawnlistUb.Column_ub_id, obj.UbId)
.Set(SpawnlistUb.Column_pattern, obj.Pattern)
.Set(SpawnlistUb.Column_group_id, obj.GroupId)
.Set(SpawnlistUb.Column_location, obj.Location)
.Set(SpawnlistUb.Column_npc_templateid, obj.NpcTemplateid)
.Set(SpawnlistUb.Column_count, obj.Count)
.Set(SpawnlistUb.Column_spawn_delay, obj.SpawnDelay)
.Set(SpawnlistUb.Column_seal_count, obj.SealCount)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistUb.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(SpawnlistUb.Column_id);
obj.UbId = dataSourceRow.getString(SpawnlistUb.Column_ub_id);
obj.Pattern = dataSourceRow.getString(SpawnlistUb.Column_pattern);
obj.GroupId = dataSourceRow.getString(SpawnlistUb.Column_group_id);
obj.Location = dataSourceRow.getString(SpawnlistUb.Column_location);
obj.NpcTemplateid = dataSourceRow.getString(SpawnlistUb.Column_npc_templateid);
obj.Count = dataSourceRow.getString(SpawnlistUb.Column_count);
obj.SpawnDelay = dataSourceRow.getString(SpawnlistUb.Column_spawn_delay);
obj.SealCount = dataSourceRow.getString(SpawnlistUb.Column_seal_count);

