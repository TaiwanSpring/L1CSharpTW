private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Commands);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Commands.Column_name, obj.Name)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Commands.Column_name, obj.Name)
.Set(Commands.Column_access_level, obj.AccessLevel)
.Set(Commands.Column_class_name, obj.ClassName)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Commands.Column_name, obj.Name)
.Set(Commands.Column_access_level, obj.AccessLevel)
.Set(Commands.Column_class_name, obj.ClassName)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Commands.Column_name, obj.Name)
.Execute();


obj.Name = dataSourceRow.getString(Commands.Column_name);
obj.AccessLevel = dataSourceRow.getString(Commands.Column_access_level);
obj.ClassName = dataSourceRow.getString(Commands.Column_class_name);

