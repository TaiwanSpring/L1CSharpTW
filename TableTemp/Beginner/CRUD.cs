private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Beginner);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Beginner.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Beginner.Column_id, obj.Id)
.Set(Beginner.Column_item_id, obj.ItemId)
.Set(Beginner.Column_count, obj.Count)
.Set(Beginner.Column_charge_count, obj.ChargeCount)
.Set(Beginner.Column_enchantlvl, obj.Enchantlvl)
.Set(Beginner.Column_item_name, obj.ItemName)
.Set(Beginner.Column_activate, obj.Activate)
.Set(Beginner.Column_bless, obj.Bless)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Beginner.Column_id, obj.Id)
.Set(Beginner.Column_item_id, obj.ItemId)
.Set(Beginner.Column_count, obj.Count)
.Set(Beginner.Column_charge_count, obj.ChargeCount)
.Set(Beginner.Column_enchantlvl, obj.Enchantlvl)
.Set(Beginner.Column_item_name, obj.ItemName)
.Set(Beginner.Column_activate, obj.Activate)
.Set(Beginner.Column_bless, obj.Bless)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Beginner.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Beginner.Column_id);
obj.ItemId = dataSourceRow.getString(Beginner.Column_item_id);
obj.Count = dataSourceRow.getString(Beginner.Column_count);
obj.ChargeCount = dataSourceRow.getString(Beginner.Column_charge_count);
obj.Enchantlvl = dataSourceRow.getString(Beginner.Column_enchantlvl);
obj.ItemName = dataSourceRow.getString(Beginner.Column_item_name);
obj.Activate = dataSourceRow.getString(Beginner.Column_activate);
obj.Bless = dataSourceRow.getString(Beginner.Column_bless);

