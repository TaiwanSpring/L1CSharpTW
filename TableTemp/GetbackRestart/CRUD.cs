private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.GetbackRestart);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(GetbackRestart.Column_area, obj.Area)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(GetbackRestart.Column_area, obj.Area)
.Set(GetbackRestart.Column_note, obj.Note)
.Set(GetbackRestart.Column_locx, obj.Locx)
.Set(GetbackRestart.Column_locy, obj.Locy)
.Set(GetbackRestart.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(GetbackRestart.Column_area, obj.Area)
.Set(GetbackRestart.Column_note, obj.Note)
.Set(GetbackRestart.Column_locx, obj.Locx)
.Set(GetbackRestart.Column_locy, obj.Locy)
.Set(GetbackRestart.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(GetbackRestart.Column_area, obj.Area)
.Execute();


obj.Area = dataSourceRow.getString(GetbackRestart.Column_area);
obj.Note = dataSourceRow.getString(GetbackRestart.Column_note);
obj.Locx = dataSourceRow.getString(GetbackRestart.Column_locx);
obj.Locy = dataSourceRow.getString(GetbackRestart.Column_locy);
obj.Mapid = dataSourceRow.getString(GetbackRestart.Column_mapid);

