private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.UbTimes);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(UbTimes.Column_ub_id, obj.UbId)
.Set(UbTimes.Column_ub_time, obj.UbTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(UbTimes.Column_ub_id, obj.UbId)
.Set(UbTimes.Column_ub_time, obj.UbTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Execute();


obj.UbId = dataSourceRow.getString(UbTimes.Column_ub_id);
obj.UbTime = dataSourceRow.getString(UbTimes.Column_ub_time);

