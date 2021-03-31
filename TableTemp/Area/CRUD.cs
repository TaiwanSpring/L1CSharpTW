private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Area);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Area.Column_areaid, obj.Areaid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Area.Column_areaid, obj.Areaid)
.Set(Area.Column_mapid, obj.Mapid)
.Set(Area.Column_areaname, obj.Areaname)
.Set(Area.Column_x1, obj.X1)
.Set(Area.Column_y1, obj.Y1)
.Set(Area.Column_x2, obj.X2)
.Set(Area.Column_y2, obj.Y2)
.Set(Area.Column_flag, obj.Flag)
.Set(Area.Column_restart, obj.Restart)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Area.Column_areaid, obj.Areaid)
.Set(Area.Column_mapid, obj.Mapid)
.Set(Area.Column_areaname, obj.Areaname)
.Set(Area.Column_x1, obj.X1)
.Set(Area.Column_y1, obj.Y1)
.Set(Area.Column_x2, obj.X2)
.Set(Area.Column_y2, obj.Y2)
.Set(Area.Column_flag, obj.Flag)
.Set(Area.Column_restart, obj.Restart)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Area.Column_areaid, obj.Areaid)
.Execute();


obj.Areaid = dataSourceRow.getString(Area.Column_areaid);
obj.Mapid = dataSourceRow.getString(Area.Column_mapid);
obj.Areaname = dataSourceRow.getString(Area.Column_areaname);
obj.X1 = dataSourceRow.getString(Area.Column_x1);
obj.Y1 = dataSourceRow.getString(Area.Column_y1);
obj.X2 = dataSourceRow.getString(Area.Column_x2);
obj.Y2 = dataSourceRow.getString(Area.Column_y2);
obj.Flag = dataSourceRow.getString(Area.Column_flag);
obj.Restart = dataSourceRow.getString(Area.Column_restart);

