private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.InnKey);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(InnKey.Column_item_obj_id, obj.ItemObjId)
.Where(InnKey.Column_key_id, obj.KeyId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(InnKey.Column_item_obj_id, obj.ItemObjId)
.Set(InnKey.Column_key_id, obj.KeyId)
.Set(InnKey.Column_npc_id, obj.NpcId)
.Set(InnKey.Column_hall, obj.Hall)
.Set(InnKey.Column_due_time, obj.DueTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(InnKey.Column_item_obj_id, obj.ItemObjId)
.Where(InnKey.Column_key_id, obj.KeyId)
.Set(InnKey.Column_npc_id, obj.NpcId)
.Set(InnKey.Column_hall, obj.Hall)
.Set(InnKey.Column_due_time, obj.DueTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(InnKey.Column_item_obj_id, obj.ItemObjId)
.Where(InnKey.Column_key_id, obj.KeyId)
.Execute();


obj.ItemObjId = dataSourceRow.getString(InnKey.Column_item_obj_id);
obj.KeyId = dataSourceRow.getString(InnKey.Column_key_id);
obj.NpcId = dataSourceRow.getString(InnKey.Column_npc_id);
obj.Hall = dataSourceRow.getString(InnKey.Column_hall);
obj.DueTime = dataSourceRow.getString(InnKey.Column_due_time);

