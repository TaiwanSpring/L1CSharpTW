private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.DungeonRandom);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(DungeonRandom.Column_src_x, obj.SrcX)
.Where(DungeonRandom.Column_src_y, obj.SrcY)
.Where(DungeonRandom.Column_src_mapid, obj.SrcMapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(DungeonRandom.Column_src_x, obj.SrcX)
.Set(DungeonRandom.Column_src_y, obj.SrcY)
.Set(DungeonRandom.Column_src_mapid, obj.SrcMapid)
.Set(DungeonRandom.Column_new_x1, obj.NewX1)
.Set(DungeonRandom.Column_new_y1, obj.NewY1)
.Set(DungeonRandom.Column_new_mapid1, obj.NewMapid1)
.Set(DungeonRandom.Column_new_x2, obj.NewX2)
.Set(DungeonRandom.Column_new_y2, obj.NewY2)
.Set(DungeonRandom.Column_new_mapid2, obj.NewMapid2)
.Set(DungeonRandom.Column_new_x3, obj.NewX3)
.Set(DungeonRandom.Column_new_y3, obj.NewY3)
.Set(DungeonRandom.Column_new_mapid3, obj.NewMapid3)
.Set(DungeonRandom.Column_new_x4, obj.NewX4)
.Set(DungeonRandom.Column_new_y4, obj.NewY4)
.Set(DungeonRandom.Column_new_mapid4, obj.NewMapid4)
.Set(DungeonRandom.Column_new_x5, obj.NewX5)
.Set(DungeonRandom.Column_new_y5, obj.NewY5)
.Set(DungeonRandom.Column_new_mapid5, obj.NewMapid5)
.Set(DungeonRandom.Column_new_heading, obj.NewHeading)
.Set(DungeonRandom.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(DungeonRandom.Column_src_x, obj.SrcX)
.Where(DungeonRandom.Column_src_y, obj.SrcY)
.Where(DungeonRandom.Column_src_mapid, obj.SrcMapid)
.Set(DungeonRandom.Column_new_x1, obj.NewX1)
.Set(DungeonRandom.Column_new_y1, obj.NewY1)
.Set(DungeonRandom.Column_new_mapid1, obj.NewMapid1)
.Set(DungeonRandom.Column_new_x2, obj.NewX2)
.Set(DungeonRandom.Column_new_y2, obj.NewY2)
.Set(DungeonRandom.Column_new_mapid2, obj.NewMapid2)
.Set(DungeonRandom.Column_new_x3, obj.NewX3)
.Set(DungeonRandom.Column_new_y3, obj.NewY3)
.Set(DungeonRandom.Column_new_mapid3, obj.NewMapid3)
.Set(DungeonRandom.Column_new_x4, obj.NewX4)
.Set(DungeonRandom.Column_new_y4, obj.NewY4)
.Set(DungeonRandom.Column_new_mapid4, obj.NewMapid4)
.Set(DungeonRandom.Column_new_x5, obj.NewX5)
.Set(DungeonRandom.Column_new_y5, obj.NewY5)
.Set(DungeonRandom.Column_new_mapid5, obj.NewMapid5)
.Set(DungeonRandom.Column_new_heading, obj.NewHeading)
.Set(DungeonRandom.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(DungeonRandom.Column_src_x, obj.SrcX)
.Where(DungeonRandom.Column_src_y, obj.SrcY)
.Where(DungeonRandom.Column_src_mapid, obj.SrcMapid)
.Execute();


obj.SrcX = dataSourceRow.getString(DungeonRandom.Column_src_x);
obj.SrcY = dataSourceRow.getString(DungeonRandom.Column_src_y);
obj.SrcMapid = dataSourceRow.getString(DungeonRandom.Column_src_mapid);
obj.NewX1 = dataSourceRow.getString(DungeonRandom.Column_new_x1);
obj.NewY1 = dataSourceRow.getString(DungeonRandom.Column_new_y1);
obj.NewMapid1 = dataSourceRow.getString(DungeonRandom.Column_new_mapid1);
obj.NewX2 = dataSourceRow.getString(DungeonRandom.Column_new_x2);
obj.NewY2 = dataSourceRow.getString(DungeonRandom.Column_new_y2);
obj.NewMapid2 = dataSourceRow.getString(DungeonRandom.Column_new_mapid2);
obj.NewX3 = dataSourceRow.getString(DungeonRandom.Column_new_x3);
obj.NewY3 = dataSourceRow.getString(DungeonRandom.Column_new_y3);
obj.NewMapid3 = dataSourceRow.getString(DungeonRandom.Column_new_mapid3);
obj.NewX4 = dataSourceRow.getString(DungeonRandom.Column_new_x4);
obj.NewY4 = dataSourceRow.getString(DungeonRandom.Column_new_y4);
obj.NewMapid4 = dataSourceRow.getString(DungeonRandom.Column_new_mapid4);
obj.NewX5 = dataSourceRow.getString(DungeonRandom.Column_new_x5);
obj.NewY5 = dataSourceRow.getString(DungeonRandom.Column_new_y5);
obj.NewMapid5 = dataSourceRow.getString(DungeonRandom.Column_new_mapid5);
obj.NewHeading = dataSourceRow.getString(DungeonRandom.Column_new_heading);
obj.Note = dataSourceRow.getString(DungeonRandom.Column_note);

