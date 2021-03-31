private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterItems);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterItems.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterItems.Column_id, obj.Id)
.Set(CharacterItems.Column_item_id, obj.ItemId)
.Set(CharacterItems.Column_char_id, obj.CharId)
.Set(CharacterItems.Column_item_name, obj.ItemName)
.Set(CharacterItems.Column_count, obj.Count)
.Set(CharacterItems.Column_is_equipped, obj.IsEquipped)
.Set(CharacterItems.Column_enchantlvl, obj.Enchantlvl)
.Set(CharacterItems.Column_is_id, obj.IsId)
.Set(CharacterItems.Column_durability, obj.Durability)
.Set(CharacterItems.Column_charge_count, obj.ChargeCount)
.Set(CharacterItems.Column_remaining_time, obj.RemainingTime)
.Set(CharacterItems.Column_last_used, obj.LastUsed)
.Set(CharacterItems.Column_bless, obj.Bless)
.Set(CharacterItems.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(CharacterItems.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(CharacterItems.Column_firemr, obj.Firemr)
.Set(CharacterItems.Column_watermr, obj.Watermr)
.Set(CharacterItems.Column_earthmr, obj.Earthmr)
.Set(CharacterItems.Column_windmr, obj.Windmr)
.Set(CharacterItems.Column_addsp, obj.Addsp)
.Set(CharacterItems.Column_addhp, obj.Addhp)
.Set(CharacterItems.Column_addmp, obj.Addmp)
.Set(CharacterItems.Column_hpr, obj.Hpr)
.Set(CharacterItems.Column_mpr, obj.Mpr)
.Set(CharacterItems.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterItems.Column_id, obj.Id)
.Set(CharacterItems.Column_item_id, obj.ItemId)
.Set(CharacterItems.Column_char_id, obj.CharId)
.Set(CharacterItems.Column_item_name, obj.ItemName)
.Set(CharacterItems.Column_count, obj.Count)
.Set(CharacterItems.Column_is_equipped, obj.IsEquipped)
.Set(CharacterItems.Column_enchantlvl, obj.Enchantlvl)
.Set(CharacterItems.Column_is_id, obj.IsId)
.Set(CharacterItems.Column_durability, obj.Durability)
.Set(CharacterItems.Column_charge_count, obj.ChargeCount)
.Set(CharacterItems.Column_remaining_time, obj.RemainingTime)
.Set(CharacterItems.Column_last_used, obj.LastUsed)
.Set(CharacterItems.Column_bless, obj.Bless)
.Set(CharacterItems.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(CharacterItems.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(CharacterItems.Column_firemr, obj.Firemr)
.Set(CharacterItems.Column_watermr, obj.Watermr)
.Set(CharacterItems.Column_earthmr, obj.Earthmr)
.Set(CharacterItems.Column_windmr, obj.Windmr)
.Set(CharacterItems.Column_addsp, obj.Addsp)
.Set(CharacterItems.Column_addhp, obj.Addhp)
.Set(CharacterItems.Column_addmp, obj.Addmp)
.Set(CharacterItems.Column_hpr, obj.Hpr)
.Set(CharacterItems.Column_mpr, obj.Mpr)
.Set(CharacterItems.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterItems.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(CharacterItems.Column_id);
obj.ItemId = dataSourceRow.getString(CharacterItems.Column_item_id);
obj.CharId = dataSourceRow.getString(CharacterItems.Column_char_id);
obj.ItemName = dataSourceRow.getString(CharacterItems.Column_item_name);
obj.Count = dataSourceRow.getString(CharacterItems.Column_count);
obj.IsEquipped = dataSourceRow.getString(CharacterItems.Column_is_equipped);
obj.Enchantlvl = dataSourceRow.getString(CharacterItems.Column_enchantlvl);
obj.IsId = dataSourceRow.getString(CharacterItems.Column_is_id);
obj.Durability = dataSourceRow.getString(CharacterItems.Column_durability);
obj.ChargeCount = dataSourceRow.getString(CharacterItems.Column_charge_count);
obj.RemainingTime = dataSourceRow.getString(CharacterItems.Column_remaining_time);
obj.LastUsed = dataSourceRow.getString(CharacterItems.Column_last_used);
obj.Bless = dataSourceRow.getString(CharacterItems.Column_bless);
obj.AttrEnchantKind = dataSourceRow.getString(CharacterItems.Column_attr_enchant_kind);
obj.AttrEnchantLevel = dataSourceRow.getString(CharacterItems.Column_attr_enchant_level);
obj.Firemr = dataSourceRow.getString(CharacterItems.Column_firemr);
obj.Watermr = dataSourceRow.getString(CharacterItems.Column_watermr);
obj.Earthmr = dataSourceRow.getString(CharacterItems.Column_earthmr);
obj.Windmr = dataSourceRow.getString(CharacterItems.Column_windmr);
obj.Addsp = dataSourceRow.getString(CharacterItems.Column_addsp);
obj.Addhp = dataSourceRow.getString(CharacterItems.Column_addhp);
obj.Addmp = dataSourceRow.getString(CharacterItems.Column_addmp);
obj.Hpr = dataSourceRow.getString(CharacterItems.Column_hpr);
obj.Mpr = dataSourceRow.getString(CharacterItems.Column_mpr);
obj.MDef = dataSourceRow.getString(CharacterItems.Column_m_def);

