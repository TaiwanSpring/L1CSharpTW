private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Getback);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Getback.Column_area_x1, obj.AreaX1)
.Where(Getback.Column_area_y1, obj.AreaY1)
.Where(Getback.Column_area_x2, obj.AreaX2)
.Where(Getback.Column_area_y2, obj.AreaY2)
.Where(Getback.Column_area_mapid, obj.AreaMapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Getback.Column_area_x1, obj.AreaX1)
.Set(Getback.Column_area_y1, obj.AreaY1)
.Set(Getback.Column_area_x2, obj.AreaX2)
.Set(Getback.Column_area_y2, obj.AreaY2)
.Set(Getback.Column_area_mapid, obj.AreaMapid)
.Set(Getback.Column_getback_x1, obj.GetbackX1)
.Set(Getback.Column_getback_y1, obj.GetbackY1)
.Set(Getback.Column_getback_x2, obj.GetbackX2)
.Set(Getback.Column_getback_y2, obj.GetbackY2)
.Set(Getback.Column_getback_x3, obj.GetbackX3)
.Set(Getback.Column_getback_y3, obj.GetbackY3)
.Set(Getback.Column_getback_mapid, obj.GetbackMapid)
.Set(Getback.Column_getback_townid, obj.GetbackTownid)
.Set(Getback.Column_getback_townid_elf, obj.GetbackTownidElf)
.Set(Getback.Column_getback_townid_darkelf, obj.GetbackTownidDarkelf)
.Set(Getback.Column_scrollescape, obj.Scrollescape)
.Set(Getback.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Getback.Column_area_x1, obj.AreaX1)
.Where(Getback.Column_area_y1, obj.AreaY1)
.Where(Getback.Column_area_x2, obj.AreaX2)
.Where(Getback.Column_area_y2, obj.AreaY2)
.Where(Getback.Column_area_mapid, obj.AreaMapid)
.Set(Getback.Column_getback_x1, obj.GetbackX1)
.Set(Getback.Column_getback_y1, obj.GetbackY1)
.Set(Getback.Column_getback_x2, obj.GetbackX2)
.Set(Getback.Column_getback_y2, obj.GetbackY2)
.Set(Getback.Column_getback_x3, obj.GetbackX3)
.Set(Getback.Column_getback_y3, obj.GetbackY3)
.Set(Getback.Column_getback_mapid, obj.GetbackMapid)
.Set(Getback.Column_getback_townid, obj.GetbackTownid)
.Set(Getback.Column_getback_townid_elf, obj.GetbackTownidElf)
.Set(Getback.Column_getback_townid_darkelf, obj.GetbackTownidDarkelf)
.Set(Getback.Column_scrollescape, obj.Scrollescape)
.Set(Getback.Column_note, obj.Note)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Getback.Column_area_x1, obj.AreaX1)
.Where(Getback.Column_area_y1, obj.AreaY1)
.Where(Getback.Column_area_x2, obj.AreaX2)
.Where(Getback.Column_area_y2, obj.AreaY2)
.Where(Getback.Column_area_mapid, obj.AreaMapid)
.Execute();


obj.AreaX1 = dataSourceRow.getString(Getback.Column_area_x1);
obj.AreaY1 = dataSourceRow.getString(Getback.Column_area_y1);
obj.AreaX2 = dataSourceRow.getString(Getback.Column_area_x2);
obj.AreaY2 = dataSourceRow.getString(Getback.Column_area_y2);
obj.AreaMapid = dataSourceRow.getString(Getback.Column_area_mapid);
obj.GetbackX1 = dataSourceRow.getString(Getback.Column_getback_x1);
obj.GetbackY1 = dataSourceRow.getString(Getback.Column_getback_y1);
obj.GetbackX2 = dataSourceRow.getString(Getback.Column_getback_x2);
obj.GetbackY2 = dataSourceRow.getString(Getback.Column_getback_y2);
obj.GetbackX3 = dataSourceRow.getString(Getback.Column_getback_x3);
obj.GetbackY3 = dataSourceRow.getString(Getback.Column_getback_y3);
obj.GetbackMapid = dataSourceRow.getString(Getback.Column_getback_mapid);
obj.GetbackTownid = dataSourceRow.getString(Getback.Column_getback_townid);
obj.GetbackTownidElf = dataSourceRow.getString(Getback.Column_getback_townid_elf);
obj.GetbackTownidDarkelf = dataSourceRow.getString(Getback.Column_getback_townid_darkelf);
obj.Scrollescape = dataSourceRow.getString(Getback.Column_scrollescape);
obj.Note = dataSourceRow.getString(Getback.Column_note);

