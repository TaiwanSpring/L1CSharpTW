private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ClanWarehouseHistory);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ClanWarehouseHistory.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ClanWarehouseHistory.Column_id, obj.Id)
.Set(ClanWarehouseHistory.Column_clan_id, obj.ClanId)
.Set(ClanWarehouseHistory.Column_char_name, obj.CharName)
.Set(ClanWarehouseHistory.Column_type, obj.Type)
.Set(ClanWarehouseHistory.Column_item_name, obj.ItemName)
.Set(ClanWarehouseHistory.Column_item_count, obj.ItemCount)
.Set(ClanWarehouseHistory.Column_record_time, obj.RecordTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(ClanWarehouseHistory.Column_id, obj.Id)
.Set(ClanWarehouseHistory.Column_clan_id, obj.ClanId)
.Set(ClanWarehouseHistory.Column_char_name, obj.CharName)
.Set(ClanWarehouseHistory.Column_type, obj.Type)
.Set(ClanWarehouseHistory.Column_item_name, obj.ItemName)
.Set(ClanWarehouseHistory.Column_item_count, obj.ItemCount)
.Set(ClanWarehouseHistory.Column_record_time, obj.RecordTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ClanWarehouseHistory.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(ClanWarehouseHistory.Column_id);
obj.ClanId = dataSourceRow.getString(ClanWarehouseHistory.Column_clan_id);
obj.CharName = dataSourceRow.getString(ClanWarehouseHistory.Column_char_name);
obj.Type = dataSourceRow.getString(ClanWarehouseHistory.Column_type);
obj.ItemName = dataSourceRow.getString(ClanWarehouseHistory.Column_item_name);
obj.ItemCount = dataSourceRow.getString(ClanWarehouseHistory.Column_item_count);
obj.RecordTime = dataSourceRow.getString(ClanWarehouseHistory.Column_record_time);

