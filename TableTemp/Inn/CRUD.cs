private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Inn);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Inn.Column_npcid, obj.Npcid)
.Where(Inn.Column_room_number, obj.RoomNumber)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Inn.Column_name, obj.Name)
.Set(Inn.Column_npcid, obj.Npcid)
.Set(Inn.Column_room_number, obj.RoomNumber)
.Set(Inn.Column_key_id, obj.KeyId)
.Set(Inn.Column_lodger_id, obj.LodgerId)
.Set(Inn.Column_hall, obj.Hall)
.Set(Inn.Column_due_time, obj.DueTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(Inn.Column_name, obj.Name)
.Where(Inn.Column_npcid, obj.Npcid)
.Where(Inn.Column_room_number, obj.RoomNumber)
.Set(Inn.Column_key_id, obj.KeyId)
.Set(Inn.Column_lodger_id, obj.LodgerId)
.Set(Inn.Column_hall, obj.Hall)
.Set(Inn.Column_due_time, obj.DueTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Inn.Column_npcid, obj.Npcid)
.Where(Inn.Column_room_number, obj.RoomNumber)
.Execute();


obj.Name = dataSourceRow.getString(Inn.Column_name);
obj.Npcid = dataSourceRow.getString(Inn.Column_npcid);
obj.RoomNumber = dataSourceRow.getString(Inn.Column_room_number);
obj.KeyId = dataSourceRow.getString(Inn.Column_key_id);
obj.LodgerId = dataSourceRow.getString(Inn.Column_lodger_id);
obj.Hall = dataSourceRow.getString(Inn.Column_hall);
obj.DueTime = dataSourceRow.getString(Inn.Column_due_time);

