private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.LogEnchant);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(LogEnchant.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(LogEnchant.Column_id, obj.Id)
.Set(LogEnchant.Column_char_id, obj.CharId)
.Set(LogEnchant.Column_item_id, obj.ItemId)
.Set(LogEnchant.Column_old_enchantlvl, obj.OldEnchantlvl)
.Set(LogEnchant.Column_new_enchantlvl, obj.NewEnchantlvl)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(LogEnchant.Column_id, obj.Id)
.Set(LogEnchant.Column_char_id, obj.CharId)
.Set(LogEnchant.Column_item_id, obj.ItemId)
.Set(LogEnchant.Column_old_enchantlvl, obj.OldEnchantlvl)
.Set(LogEnchant.Column_new_enchantlvl, obj.NewEnchantlvl)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(LogEnchant.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(LogEnchant.Column_id);
obj.CharId = dataSourceRow.getString(LogEnchant.Column_char_id);
obj.ItemId = dataSourceRow.getString(LogEnchant.Column_item_id);
obj.OldEnchantlvl = dataSourceRow.getString(LogEnchant.Column_old_enchantlvl);
obj.NewEnchantlvl = dataSourceRow.getString(LogEnchant.Column_new_enchantlvl);

