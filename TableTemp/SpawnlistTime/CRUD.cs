private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SpawnlistTime);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SpawnlistTime.Column_spawn_id, obj.SpawnId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SpawnlistTime.Column_spawn_id, obj.SpawnId)
.Set(SpawnlistTime.Column_time_start, obj.TimeStart)
.Set(SpawnlistTime.Column_time_end, obj.TimeEnd)
.Set(SpawnlistTime.Column_delete_at_endtime, obj.DeleteAtEndtime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SpawnlistTime.Column_spawn_id, obj.SpawnId)
.Set(SpawnlistTime.Column_time_start, obj.TimeStart)
.Set(SpawnlistTime.Column_time_end, obj.TimeEnd)
.Set(SpawnlistTime.Column_delete_at_endtime, obj.DeleteAtEndtime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SpawnlistTime.Column_spawn_id, obj.SpawnId)
.Execute();


obj.SpawnId = dataSourceRow.getString(SpawnlistTime.Column_spawn_id);
obj.TimeStart = dataSourceRow.getString(SpawnlistTime.Column_time_start);
obj.TimeEnd = dataSourceRow.getString(SpawnlistTime.Column_time_end);
obj.DeleteAtEndtime = dataSourceRow.getString(SpawnlistTime.Column_delete_at_endtime);

