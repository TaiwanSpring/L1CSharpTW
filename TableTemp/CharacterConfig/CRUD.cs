private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterConfig);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterConfig.Column_object_id, obj.ObjectId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterConfig.Column_object_id, obj.ObjectId)
.Set(CharacterConfig.Column_length, obj.Length)
.Set(CharacterConfig.Column_data, obj.Data)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterConfig.Column_object_id, obj.ObjectId)
.Set(CharacterConfig.Column_length, obj.Length)
.Set(CharacterConfig.Column_data, obj.Data)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterConfig.Column_object_id, obj.ObjectId)
.Execute();


obj.ObjectId = dataSourceRow.getString(CharacterConfig.Column_object_id);
obj.Length = dataSourceRow.getString(CharacterConfig.Column_length);
obj.Data = dataSourceRow.getString(CharacterConfig.Column_data);

