private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Droplist);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Droplist.Column_mobId, obj.MobId)
.Where(Droplist.Column_itemId, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Droplist.Column_mobId, obj.MobId)
.Set(Droplist.Column_itemId, obj.ItemId)
.Set(Droplist.Column_min, obj.Min)
.Set(Droplist.Column_max, obj.Max)
.Set(Droplist.Column_chance, obj.Chance)
.Set(Droplist.Column_enchantlvl, obj.Enchantlvl)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Droplist.Column_mobId, obj.MobId)
.Where(Droplist.Column_itemId, obj.ItemId)
.Set(Droplist.Column_min, obj.Min)
.Set(Droplist.Column_max, obj.Max)
.Set(Droplist.Column_chance, obj.Chance)
.Set(Droplist.Column_enchantlvl, obj.Enchantlvl)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Droplist.Column_mobId, obj.MobId)
.Where(Droplist.Column_itemId, obj.ItemId)
.Execute();


obj.MobId = dataSourceRow.getString(Droplist.Column_mobId);
obj.ItemId = dataSourceRow.getString(Droplist.Column_itemId);
obj.Min = dataSourceRow.getString(Droplist.Column_min);
obj.Max = dataSourceRow.getString(Droplist.Column_max);
obj.Chance = dataSourceRow.getString(Droplist.Column_chance);
obj.Enchantlvl = dataSourceRow.getString(Droplist.Column_enchantlvl);

