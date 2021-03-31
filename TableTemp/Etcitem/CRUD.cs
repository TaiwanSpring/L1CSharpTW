private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Etcitem);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Etcitem.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Etcitem.Column_item_id, obj.ItemId)
.Set(Etcitem.Column_name, obj.Name)
.Set(Etcitem.Column_unidentified_name_id, obj.UnidentifiedNameId)
.Set(Etcitem.Column_identified_name_id, obj.IdentifiedNameId)
.Set(Etcitem.Column_item_type, obj.ItemType)
.Set(Etcitem.Column_use_type, obj.UseType)
.Set(Etcitem.Column_material, obj.Material)
.Set(Etcitem.Column_weight, obj.Weight)
.Set(Etcitem.Column_invgfx, obj.Invgfx)
.Set(Etcitem.Column_grdgfx, obj.Grdgfx)
.Set(Etcitem.Column_itemdesc_id, obj.ItemdescId)
.Set(Etcitem.Column_stackable, obj.Stackable)
.Set(Etcitem.Column_max_charge_count, obj.MaxChargeCount)
.Set(Etcitem.Column_dmg_small, obj.DmgSmall)
.Set(Etcitem.Column_dmg_large, obj.DmgLarge)
.Set(Etcitem.Column_min_lvl, obj.MinLvl)
.Set(Etcitem.Column_max_lvl, obj.MaxLvl)
.Set(Etcitem.Column_locx, obj.Locx)
.Set(Etcitem.Column_locy, obj.Locy)
.Set(Etcitem.Column_mapid, obj.Mapid)
.Set(Etcitem.Column_bless, obj.Bless)
.Set(Etcitem.Column_trade, obj.Trade)
.Set(Etcitem.Column_cant_delete, obj.CantDelete)
.Set(Etcitem.Column_can_seal, obj.CanSeal)
.Set(Etcitem.Column_delay_id, obj.DelayId)
.Set(Etcitem.Column_delay_time, obj.DelayTime)
.Set(Etcitem.Column_delay_effect, obj.DelayEffect)
.Set(Etcitem.Column_food_volume, obj.FoodVolume)
.Set(Etcitem.Column_save_at_once, obj.SaveAtOnce)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Etcitem.Column_item_id, obj.ItemId)
.Set(Etcitem.Column_name, obj.Name)
.Set(Etcitem.Column_unidentified_name_id, obj.UnidentifiedNameId)
.Set(Etcitem.Column_identified_name_id, obj.IdentifiedNameId)
.Set(Etcitem.Column_item_type, obj.ItemType)
.Set(Etcitem.Column_use_type, obj.UseType)
.Set(Etcitem.Column_material, obj.Material)
.Set(Etcitem.Column_weight, obj.Weight)
.Set(Etcitem.Column_invgfx, obj.Invgfx)
.Set(Etcitem.Column_grdgfx, obj.Grdgfx)
.Set(Etcitem.Column_itemdesc_id, obj.ItemdescId)
.Set(Etcitem.Column_stackable, obj.Stackable)
.Set(Etcitem.Column_max_charge_count, obj.MaxChargeCount)
.Set(Etcitem.Column_dmg_small, obj.DmgSmall)
.Set(Etcitem.Column_dmg_large, obj.DmgLarge)
.Set(Etcitem.Column_min_lvl, obj.MinLvl)
.Set(Etcitem.Column_max_lvl, obj.MaxLvl)
.Set(Etcitem.Column_locx, obj.Locx)
.Set(Etcitem.Column_locy, obj.Locy)
.Set(Etcitem.Column_mapid, obj.Mapid)
.Set(Etcitem.Column_bless, obj.Bless)
.Set(Etcitem.Column_trade, obj.Trade)
.Set(Etcitem.Column_cant_delete, obj.CantDelete)
.Set(Etcitem.Column_can_seal, obj.CanSeal)
.Set(Etcitem.Column_delay_id, obj.DelayId)
.Set(Etcitem.Column_delay_time, obj.DelayTime)
.Set(Etcitem.Column_delay_effect, obj.DelayEffect)
.Set(Etcitem.Column_food_volume, obj.FoodVolume)
.Set(Etcitem.Column_save_at_once, obj.SaveAtOnce)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Etcitem.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(Etcitem.Column_item_id);
obj.Name = dataSourceRow.getString(Etcitem.Column_name);
obj.UnidentifiedNameId = dataSourceRow.getString(Etcitem.Column_unidentified_name_id);
obj.IdentifiedNameId = dataSourceRow.getString(Etcitem.Column_identified_name_id);
obj.ItemType = dataSourceRow.getString(Etcitem.Column_item_type);
obj.UseType = dataSourceRow.getString(Etcitem.Column_use_type);
obj.Material = dataSourceRow.getString(Etcitem.Column_material);
obj.Weight = dataSourceRow.getString(Etcitem.Column_weight);
obj.Invgfx = dataSourceRow.getString(Etcitem.Column_invgfx);
obj.Grdgfx = dataSourceRow.getString(Etcitem.Column_grdgfx);
obj.ItemdescId = dataSourceRow.getString(Etcitem.Column_itemdesc_id);
obj.Stackable = dataSourceRow.getString(Etcitem.Column_stackable);
obj.MaxChargeCount = dataSourceRow.getString(Etcitem.Column_max_charge_count);
obj.DmgSmall = dataSourceRow.getString(Etcitem.Column_dmg_small);
obj.DmgLarge = dataSourceRow.getString(Etcitem.Column_dmg_large);
obj.MinLvl = dataSourceRow.getString(Etcitem.Column_min_lvl);
obj.MaxLvl = dataSourceRow.getString(Etcitem.Column_max_lvl);
obj.Locx = dataSourceRow.getString(Etcitem.Column_locx);
obj.Locy = dataSourceRow.getString(Etcitem.Column_locy);
obj.Mapid = dataSourceRow.getString(Etcitem.Column_mapid);
obj.Bless = dataSourceRow.getString(Etcitem.Column_bless);
obj.Trade = dataSourceRow.getString(Etcitem.Column_trade);
obj.CantDelete = dataSourceRow.getString(Etcitem.Column_cant_delete);
obj.CanSeal = dataSourceRow.getString(Etcitem.Column_can_seal);
obj.DelayId = dataSourceRow.getString(Etcitem.Column_delay_id);
obj.DelayTime = dataSourceRow.getString(Etcitem.Column_delay_time);
obj.DelayEffect = dataSourceRow.getString(Etcitem.Column_delay_effect);
obj.FoodVolume = dataSourceRow.getString(Etcitem.Column_food_volume);
obj.SaveAtOnce = dataSourceRow.getString(Etcitem.Column_save_at_once);

