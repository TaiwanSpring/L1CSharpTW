private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterWarehouse);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterWarehouse.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterWarehouse.Column_id, obj.Id)
.Set(CharacterWarehouse.Column_account_name, obj.AccountName)
.Set(CharacterWarehouse.Column_item_id, obj.ItemId)
.Set(CharacterWarehouse.Column_item_name, obj.ItemName)
.Set(CharacterWarehouse.Column_count, obj.Count)
.Set(CharacterWarehouse.Column_is_equipped, obj.IsEquipped)
.Set(CharacterWarehouse.Column_enchantlvl, obj.Enchantlvl)
.Set(CharacterWarehouse.Column_is_id, obj.IsId)
.Set(CharacterWarehouse.Column_durability, obj.Durability)
.Set(CharacterWarehouse.Column_charge_count, obj.ChargeCount)
.Set(CharacterWarehouse.Column_remaining_time, obj.RemainingTime)
.Set(CharacterWarehouse.Column_last_used, obj.LastUsed)
.Set(CharacterWarehouse.Column_bless, obj.Bless)
.Set(CharacterWarehouse.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(CharacterWarehouse.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(CharacterWarehouse.Column_firemr, obj.Firemr)
.Set(CharacterWarehouse.Column_watermr, obj.Watermr)
.Set(CharacterWarehouse.Column_earthmr, obj.Earthmr)
.Set(CharacterWarehouse.Column_windmr, obj.Windmr)
.Set(CharacterWarehouse.Column_addsp, obj.Addsp)
.Set(CharacterWarehouse.Column_addhp, obj.Addhp)
.Set(CharacterWarehouse.Column_addmp, obj.Addmp)
.Set(CharacterWarehouse.Column_hpr, obj.Hpr)
.Set(CharacterWarehouse.Column_mpr, obj.Mpr)
.Set(CharacterWarehouse.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterWarehouse.Column_id, obj.Id)
.Set(CharacterWarehouse.Column_account_name, obj.AccountName)
.Set(CharacterWarehouse.Column_item_id, obj.ItemId)
.Set(CharacterWarehouse.Column_item_name, obj.ItemName)
.Set(CharacterWarehouse.Column_count, obj.Count)
.Set(CharacterWarehouse.Column_is_equipped, obj.IsEquipped)
.Set(CharacterWarehouse.Column_enchantlvl, obj.Enchantlvl)
.Set(CharacterWarehouse.Column_is_id, obj.IsId)
.Set(CharacterWarehouse.Column_durability, obj.Durability)
.Set(CharacterWarehouse.Column_charge_count, obj.ChargeCount)
.Set(CharacterWarehouse.Column_remaining_time, obj.RemainingTime)
.Set(CharacterWarehouse.Column_last_used, obj.LastUsed)
.Set(CharacterWarehouse.Column_bless, obj.Bless)
.Set(CharacterWarehouse.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(CharacterWarehouse.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(CharacterWarehouse.Column_firemr, obj.Firemr)
.Set(CharacterWarehouse.Column_watermr, obj.Watermr)
.Set(CharacterWarehouse.Column_earthmr, obj.Earthmr)
.Set(CharacterWarehouse.Column_windmr, obj.Windmr)
.Set(CharacterWarehouse.Column_addsp, obj.Addsp)
.Set(CharacterWarehouse.Column_addhp, obj.Addhp)
.Set(CharacterWarehouse.Column_addmp, obj.Addmp)
.Set(CharacterWarehouse.Column_hpr, obj.Hpr)
.Set(CharacterWarehouse.Column_mpr, obj.Mpr)
.Set(CharacterWarehouse.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterWarehouse.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(CharacterWarehouse.Column_id);
obj.AccountName = dataSourceRow.getString(CharacterWarehouse.Column_account_name);
obj.ItemId = dataSourceRow.getString(CharacterWarehouse.Column_item_id);
obj.ItemName = dataSourceRow.getString(CharacterWarehouse.Column_item_name);
obj.Count = dataSourceRow.getString(CharacterWarehouse.Column_count);
obj.IsEquipped = dataSourceRow.getString(CharacterWarehouse.Column_is_equipped);
obj.Enchantlvl = dataSourceRow.getString(CharacterWarehouse.Column_enchantlvl);
obj.IsId = dataSourceRow.getString(CharacterWarehouse.Column_is_id);
obj.Durability = dataSourceRow.getString(CharacterWarehouse.Column_durability);
obj.ChargeCount = dataSourceRow.getString(CharacterWarehouse.Column_charge_count);
obj.RemainingTime = dataSourceRow.getString(CharacterWarehouse.Column_remaining_time);
obj.LastUsed = dataSourceRow.getString(CharacterWarehouse.Column_last_used);
obj.Bless = dataSourceRow.getString(CharacterWarehouse.Column_bless);
obj.AttrEnchantKind = dataSourceRow.getString(CharacterWarehouse.Column_attr_enchant_kind);
obj.AttrEnchantLevel = dataSourceRow.getString(CharacterWarehouse.Column_attr_enchant_level);
obj.Firemr = dataSourceRow.getString(CharacterWarehouse.Column_firemr);
obj.Watermr = dataSourceRow.getString(CharacterWarehouse.Column_watermr);
obj.Earthmr = dataSourceRow.getString(CharacterWarehouse.Column_earthmr);
obj.Windmr = dataSourceRow.getString(CharacterWarehouse.Column_windmr);
obj.Addsp = dataSourceRow.getString(CharacterWarehouse.Column_addsp);
obj.Addhp = dataSourceRow.getString(CharacterWarehouse.Column_addhp);
obj.Addmp = dataSourceRow.getString(CharacterWarehouse.Column_addmp);
obj.Hpr = dataSourceRow.getString(CharacterWarehouse.Column_hpr);
obj.Mpr = dataSourceRow.getString(CharacterWarehouse.Column_mpr);
obj.MDef = dataSourceRow.getString(CharacterWarehouse.Column_m_def);

