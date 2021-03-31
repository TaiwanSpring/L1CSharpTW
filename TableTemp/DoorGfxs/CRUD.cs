private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.DoorGfxs);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(DoorGfxs.Column_gfxid, obj.Gfxid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(DoorGfxs.Column_gfxid, obj.Gfxid)
.Set(DoorGfxs.Column_note, obj.Note)
.Set(DoorGfxs.Column_direction, obj.Direction)
.Set(DoorGfxs.Column_left_edge_offset, obj.LeftEdgeOffset)
.Set(DoorGfxs.Column_right_edge_offset, obj.RightEdgeOffset)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(DoorGfxs.Column_gfxid, obj.Gfxid)
.Set(DoorGfxs.Column_note, obj.Note)
.Set(DoorGfxs.Column_direction, obj.Direction)
.Set(DoorGfxs.Column_left_edge_offset, obj.LeftEdgeOffset)
.Set(DoorGfxs.Column_right_edge_offset, obj.RightEdgeOffset)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(DoorGfxs.Column_gfxid, obj.Gfxid)
.Execute();


obj.Gfxid = dataSourceRow.getString(DoorGfxs.Column_gfxid);
obj.Note = dataSourceRow.getString(DoorGfxs.Column_note);
obj.Direction = dataSourceRow.getString(DoorGfxs.Column_direction);
obj.LeftEdgeOffset = dataSourceRow.getString(DoorGfxs.Column_left_edge_offset);
obj.RightEdgeOffset = dataSourceRow.getString(DoorGfxs.Column_right_edge_offset);

