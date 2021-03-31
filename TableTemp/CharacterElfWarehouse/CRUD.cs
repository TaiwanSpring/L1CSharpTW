private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterElfWarehouse);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterElfWarehouse.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterElfWarehouse.Column_id, obj.Id)
.Set(CharacterElfWarehouse.Column_account_name, obj.AccountName)
.Set(CharacterElfWarehouse.Column_item_id, obj.ItemId)
.Set(CharacterElfWarehouse.Column_item_name, obj.ItemName)
.Set(CharacterElfWarehouse.Column_count, obj.Count)
.Set(CharacterElfWarehouse.Column_is_equipped, obj.IsEquipped)
.Set(CharacterElfWarehouse.Column_enchantlvl, obj.Enchantlvl)
.Set(CharacterElfWarehouse.Column_is_id, obj.IsId)
.Set(CharacterElfWarehouse.Column_durability, obj.Durability)
.Set(CharacterElfWarehouse.Column_charge_count, obj.ChargeCount)
.Set(CharacterElfWarehouse.Column_remaining_time, obj.RemainingTime)
.Set(CharacterElfWarehouse.Column_last_used, obj.LastUsed)
.Set(CharacterElfWarehouse.Column_bless, obj.Bless)
.Set(CharacterElfWarehouse.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(CharacterElfWarehouse.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(CharacterElfWarehouse.Column_firemr, obj.Firemr)
.Set(CharacterElfWarehouse.Column_watermr, obj.Watermr)
.Set(CharacterElfWarehouse.Column_earthmr, obj.Earthmr)
.Set(CharacterElfWarehouse.Column_windmr, obj.Windmr)
.Set(CharacterElfWarehouse.Column_addsp, obj.Addsp)
.Set(CharacterElfWarehouse.Column_addhp, obj.Addhp)
.Set(CharacterElfWarehouse.Column_addmp, obj.Addmp)
.Set(CharacterElfWarehouse.Column_hpr, obj.Hpr)
.Set(CharacterElfWarehouse.Column_mpr, obj.Mpr)
.Set(CharacterElfWarehouse.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterElfWarehouse.Column_id, obj.Id)
.Set(CharacterElfWarehouse.Column_account_name, obj.AccountName)
.Set(CharacterElfWarehouse.Column_item_id, obj.ItemId)
.Set(CharacterElfWarehouse.Column_item_name, obj.ItemName)
.Set(CharacterElfWarehouse.Column_count, obj.Count)
.Set(CharacterElfWarehouse.Column_is_equipped, obj.IsEquipped)
.Set(CharacterElfWarehouse.Column_enchantlvl, obj.Enchantlvl)
.Set(CharacterElfWarehouse.Column_is_id, obj.IsId)
.Set(CharacterElfWarehouse.Column_durability, obj.Durability)
.Set(CharacterElfWarehouse.Column_charge_count, obj.ChargeCount)
.Set(CharacterElfWarehouse.Column_remaining_time, obj.RemainingTime)
.Set(CharacterElfWarehouse.Column_last_used, obj.LastUsed)
.Set(CharacterElfWarehouse.Column_bless, obj.Bless)
.Set(CharacterElfWarehouse.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(CharacterElfWarehouse.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(CharacterElfWarehouse.Column_firemr, obj.Firemr)
.Set(CharacterElfWarehouse.Column_watermr, obj.Watermr)
.Set(CharacterElfWarehouse.Column_earthmr, obj.Earthmr)
.Set(CharacterElfWarehouse.Column_windmr, obj.Windmr)
.Set(CharacterElfWarehouse.Column_addsp, obj.Addsp)
.Set(CharacterElfWarehouse.Column_addhp, obj.Addhp)
.Set(CharacterElfWarehouse.Column_addmp, obj.Addmp)
.Set(CharacterElfWarehouse.Column_hpr, obj.Hpr)
.Set(CharacterElfWarehouse.Column_mpr, obj.Mpr)
.Set(CharacterElfWarehouse.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterElfWarehouse.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(CharacterElfWarehouse.Column_id);
obj.AccountName = dataSourceRow.getString(CharacterElfWarehouse.Column_account_name);
obj.ItemId = dataSourceRow.getString(CharacterElfWarehouse.Column_item_id);
obj.ItemName = dataSourceRow.getString(CharacterElfWarehouse.Column_item_name);
obj.Count = dataSourceRow.getString(CharacterElfWarehouse.Column_count);
obj.IsEquipped = dataSourceRow.getString(CharacterElfWarehouse.Column_is_equipped);
obj.Enchantlvl = dataSourceRow.getString(CharacterElfWarehouse.Column_enchantlvl);
obj.IsId = dataSourceRow.getString(CharacterElfWarehouse.Column_is_id);
obj.Durability = dataSourceRow.getString(CharacterElfWarehouse.Column_durability);
obj.ChargeCount = dataSourceRow.getString(CharacterElfWarehouse.Column_charge_count);
obj.RemainingTime = dataSourceRow.getString(CharacterElfWarehouse.Column_remaining_time);
obj.LastUsed = dataSourceRow.getString(CharacterElfWarehouse.Column_last_used);
obj.Bless = dataSourceRow.getString(CharacterElfWarehouse.Column_bless);
obj.AttrEnchantKind = dataSourceRow.getString(CharacterElfWarehouse.Column_attr_enchant_kind);
obj.AttrEnchantLevel = dataSourceRow.getString(CharacterElfWarehouse.Column_attr_enchant_level);
obj.Firemr = dataSourceRow.getString(CharacterElfWarehouse.Column_firemr);
obj.Watermr = dataSourceRow.getString(CharacterElfWarehouse.Column_watermr);
obj.Earthmr = dataSourceRow.getString(CharacterElfWarehouse.Column_earthmr);
obj.Windmr = dataSourceRow.getString(CharacterElfWarehouse.Column_windmr);
obj.Addsp = dataSourceRow.getString(CharacterElfWarehouse.Column_addsp);
obj.Addhp = dataSourceRow.getString(CharacterElfWarehouse.Column_addhp);
obj.Addmp = dataSourceRow.getString(CharacterElfWarehouse.Column_addmp);
obj.Hpr = dataSourceRow.getString(CharacterElfWarehouse.Column_hpr);
obj.Mpr = dataSourceRow.getString(CharacterElfWarehouse.Column_mpr);
obj.MDef = dataSourceRow.getString(CharacterElfWarehouse.Column_m_def);

