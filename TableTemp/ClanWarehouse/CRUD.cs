private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ClanWarehouse);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ClanWarehouse.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ClanWarehouse.Column_id, obj.Id)
.Set(ClanWarehouse.Column_clan_name, obj.ClanName)
.Set(ClanWarehouse.Column_item_id, obj.ItemId)
.Set(ClanWarehouse.Column_item_name, obj.ItemName)
.Set(ClanWarehouse.Column_count, obj.Count)
.Set(ClanWarehouse.Column_is_equipped, obj.IsEquipped)
.Set(ClanWarehouse.Column_enchantlvl, obj.Enchantlvl)
.Set(ClanWarehouse.Column_is_id, obj.IsId)
.Set(ClanWarehouse.Column_durability, obj.Durability)
.Set(ClanWarehouse.Column_charge_count, obj.ChargeCount)
.Set(ClanWarehouse.Column_remaining_time, obj.RemainingTime)
.Set(ClanWarehouse.Column_last_used, obj.LastUsed)
.Set(ClanWarehouse.Column_bless, obj.Bless)
.Set(ClanWarehouse.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(ClanWarehouse.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(ClanWarehouse.Column_firemr, obj.Firemr)
.Set(ClanWarehouse.Column_watermr, obj.Watermr)
.Set(ClanWarehouse.Column_earthmr, obj.Earthmr)
.Set(ClanWarehouse.Column_windmr, obj.Windmr)
.Set(ClanWarehouse.Column_addsp, obj.Addsp)
.Set(ClanWarehouse.Column_addhp, obj.Addhp)
.Set(ClanWarehouse.Column_addmp, obj.Addmp)
.Set(ClanWarehouse.Column_hpr, obj.Hpr)
.Set(ClanWarehouse.Column_mpr, obj.Mpr)
.Set(ClanWarehouse.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(ClanWarehouse.Column_id, obj.Id)
.Set(ClanWarehouse.Column_clan_name, obj.ClanName)
.Set(ClanWarehouse.Column_item_id, obj.ItemId)
.Set(ClanWarehouse.Column_item_name, obj.ItemName)
.Set(ClanWarehouse.Column_count, obj.Count)
.Set(ClanWarehouse.Column_is_equipped, obj.IsEquipped)
.Set(ClanWarehouse.Column_enchantlvl, obj.Enchantlvl)
.Set(ClanWarehouse.Column_is_id, obj.IsId)
.Set(ClanWarehouse.Column_durability, obj.Durability)
.Set(ClanWarehouse.Column_charge_count, obj.ChargeCount)
.Set(ClanWarehouse.Column_remaining_time, obj.RemainingTime)
.Set(ClanWarehouse.Column_last_used, obj.LastUsed)
.Set(ClanWarehouse.Column_bless, obj.Bless)
.Set(ClanWarehouse.Column_attr_enchant_kind, obj.AttrEnchantKind)
.Set(ClanWarehouse.Column_attr_enchant_level, obj.AttrEnchantLevel)
.Set(ClanWarehouse.Column_firemr, obj.Firemr)
.Set(ClanWarehouse.Column_watermr, obj.Watermr)
.Set(ClanWarehouse.Column_earthmr, obj.Earthmr)
.Set(ClanWarehouse.Column_windmr, obj.Windmr)
.Set(ClanWarehouse.Column_addsp, obj.Addsp)
.Set(ClanWarehouse.Column_addhp, obj.Addhp)
.Set(ClanWarehouse.Column_addmp, obj.Addmp)
.Set(ClanWarehouse.Column_hpr, obj.Hpr)
.Set(ClanWarehouse.Column_mpr, obj.Mpr)
.Set(ClanWarehouse.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ClanWarehouse.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(ClanWarehouse.Column_id);
obj.ClanName = dataSourceRow.getString(ClanWarehouse.Column_clan_name);
obj.ItemId = dataSourceRow.getString(ClanWarehouse.Column_item_id);
obj.ItemName = dataSourceRow.getString(ClanWarehouse.Column_item_name);
obj.Count = dataSourceRow.getString(ClanWarehouse.Column_count);
obj.IsEquipped = dataSourceRow.getString(ClanWarehouse.Column_is_equipped);
obj.Enchantlvl = dataSourceRow.getString(ClanWarehouse.Column_enchantlvl);
obj.IsId = dataSourceRow.getString(ClanWarehouse.Column_is_id);
obj.Durability = dataSourceRow.getString(ClanWarehouse.Column_durability);
obj.ChargeCount = dataSourceRow.getString(ClanWarehouse.Column_charge_count);
obj.RemainingTime = dataSourceRow.getString(ClanWarehouse.Column_remaining_time);
obj.LastUsed = dataSourceRow.getString(ClanWarehouse.Column_last_used);
obj.Bless = dataSourceRow.getString(ClanWarehouse.Column_bless);
obj.AttrEnchantKind = dataSourceRow.getString(ClanWarehouse.Column_attr_enchant_kind);
obj.AttrEnchantLevel = dataSourceRow.getString(ClanWarehouse.Column_attr_enchant_level);
obj.Firemr = dataSourceRow.getString(ClanWarehouse.Column_firemr);
obj.Watermr = dataSourceRow.getString(ClanWarehouse.Column_watermr);
obj.Earthmr = dataSourceRow.getString(ClanWarehouse.Column_earthmr);
obj.Windmr = dataSourceRow.getString(ClanWarehouse.Column_windmr);
obj.Addsp = dataSourceRow.getString(ClanWarehouse.Column_addsp);
obj.Addhp = dataSourceRow.getString(ClanWarehouse.Column_addhp);
obj.Addmp = dataSourceRow.getString(ClanWarehouse.Column_addmp);
obj.Hpr = dataSourceRow.getString(ClanWarehouse.Column_hpr);
obj.Mpr = dataSourceRow.getString(ClanWarehouse.Column_mpr);
obj.MDef = dataSourceRow.getString(ClanWarehouse.Column_m_def);

