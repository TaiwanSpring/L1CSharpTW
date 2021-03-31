private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Dungeon);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Dungeon.Column_src_x, obj.SrcX)
.Where(Dungeon.Column_src_y, obj.SrcY)
.Where(Dungeon.Column_src_mapid, obj.SrcMapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Dungeon.Column_src_x, obj.SrcX)
.Set(Dungeon.Column_src_y, obj.SrcY)
.Set(Dungeon.Column_src_mapid, obj.SrcMapid)
.Set(Dungeon.Column_new_x, obj.NewX)
.Set(Dungeon.Column_new_y, obj.NewY)
.Set(Dungeon.Column_new_mapid, obj.NewMapid)
.Set(Dungeon.Column_new_heading, obj.NewHeading)
.Set(Dungeon.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Dungeon.Column_src_x, obj.SrcX)
.Where(Dungeon.Column_src_y, obj.SrcY)
.Where(Dungeon.Column_src_mapid, obj.SrcMapid)
.Set(Dungeon.Column_new_x, obj.NewX)
.Set(Dungeon.Column_new_y, obj.NewY)
.Set(Dungeon.Column_new_mapid, obj.NewMapid)
.Set(Dungeon.Column_new_heading, obj.NewHeading)
.Set(Dungeon.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Dungeon.Column_src_x, obj.SrcX)
.Where(Dungeon.Column_src_y, obj.SrcY)
.Where(Dungeon.Column_src_mapid, obj.SrcMapid)
.Execute();


obj.SrcX = dataSourceRow.getString(Dungeon.Column_src_x);
obj.SrcY = dataSourceRow.getString(Dungeon.Column_src_y);
obj.SrcMapid = dataSourceRow.getString(Dungeon.Column_src_mapid);
obj.NewX = dataSourceRow.getString(Dungeon.Column_new_x);
obj.NewY = dataSourceRow.getString(Dungeon.Column_new_y);
obj.NewMapid = dataSourceRow.getString(Dungeon.Column_new_mapid);
obj.NewHeading = dataSourceRow.getString(Dungeon.Column_new_heading);
obj.Note = dataSourceRow.getString(Dungeon.Column_note);

