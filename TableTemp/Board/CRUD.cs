private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Board);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Board.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Board.Column_id, obj.Id)
.Set(Board.Column_name, obj.Name)
.Set(Board.Column_date, obj.Date)
.Set(Board.Column_title, obj.Title)
.Set(Board.Column_content, obj.Content)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Board.Column_id, obj.Id)
.Set(Board.Column_name, obj.Name)
.Set(Board.Column_date, obj.Date)
.Set(Board.Column_title, obj.Title)
.Set(Board.Column_content, obj.Content)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Board.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Board.Column_id);
obj.Name = dataSourceRow.getString(Board.Column_name);
obj.Date = dataSourceRow.getString(Board.Column_date);
obj.Title = dataSourceRow.getString(Board.Column_title);
obj.Content = dataSourceRow.getString(Board.Column_content);

