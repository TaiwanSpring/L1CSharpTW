private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.UbManagers);
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
.Set(UbManagers.Column_ub_id, obj.UbId)
.Set(UbManagers.Column_ub_manager_npc_id, obj.UbManagerNpcId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(UbManagers.Column_ub_id, obj.UbId)
.Set(UbManagers.Column_ub_manager_npc_id, obj.UbManagerNpcId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Execute();


obj.UbId = dataSourceRow.getString(UbManagers.Column_ub_id);
obj.UbManagerNpcId = dataSourceRow.getString(UbManagers.Column_ub_manager_npc_id);

