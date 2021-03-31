private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Weapon);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Weapon.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Weapon.Column_item_id, obj.ItemId)
.Set(Weapon.Column_name, obj.Name)
.Set(Weapon.Column_unidentified_name_id, obj.UnidentifiedNameId)
.Set(Weapon.Column_identified_name_id, obj.IdentifiedNameId)
.Set(Weapon.Column_type, obj.Type)
.Set(Weapon.Column_material, obj.Material)
.Set(Weapon.Column_weight, obj.Weight)
.Set(Weapon.Column_invgfx, obj.Invgfx)
.Set(Weapon.Column_grdgfx, obj.Grdgfx)
.Set(Weapon.Column_itemdesc_id, obj.ItemdescId)
.Set(Weapon.Column_dmg_small, obj.DmgSmall)
.Set(Weapon.Column_dmg_large, obj.DmgLarge)
.Set(Weapon.Column_range, obj.Range)
.Set(Weapon.Column_safenchant, obj.Safenchant)
.Set(Weapon.Column_use_royal, obj.UseRoyal)
.Set(Weapon.Column_use_knight, obj.UseKnight)
.Set(Weapon.Column_use_mage, obj.UseMage)
.Set(Weapon.Column_use_elf, obj.UseElf)
.Set(Weapon.Column_use_darkelf, obj.UseDarkelf)
.Set(Weapon.Column_use_dragonknight, obj.UseDragonknight)
.Set(Weapon.Column_use_illusionist, obj.UseIllusionist)
.Set(Weapon.Column_hitmodifier, obj.Hitmodifier)
.Set(Weapon.Column_dmgmodifier, obj.Dmgmodifier)
.Set(Weapon.Column_add_str, obj.AddStr)
.Set(Weapon.Column_add_con, obj.AddCon)
.Set(Weapon.Column_add_dex, obj.AddDex)
.Set(Weapon.Column_add_int, obj.AddInt)
.Set(Weapon.Column_add_wis, obj.AddWis)
.Set(Weapon.Column_add_cha, obj.AddCha)
.Set(Weapon.Column_add_hp, obj.AddHp)
.Set(Weapon.Column_add_mp, obj.AddMp)
.Set(Weapon.Column_add_hpr, obj.AddHpr)
.Set(Weapon.Column_add_mpr, obj.AddMpr)
.Set(Weapon.Column_add_sp, obj.AddSp)
.Set(Weapon.Column_m_def, obj.MDef)
.Set(Weapon.Column_haste_item, obj.HasteItem)
.Set(Weapon.Column_double_dmg_chance, obj.DoubleDmgChance)
.Set(Weapon.Column_magicdmgmodifier, obj.Magicdmgmodifier)
.Set(Weapon.Column_canbedmg, obj.Canbedmg)
.Set(Weapon.Column_min_lvl, obj.MinLvl)
.Set(Weapon.Column_max_lvl, obj.MaxLvl)
.Set(Weapon.Column_bless, obj.Bless)
.Set(Weapon.Column_trade, obj.Trade)
.Set(Weapon.Column_cant_delete, obj.CantDelete)
.Set(Weapon.Column_max_use_time, obj.MaxUseTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Weapon.Column_item_id, obj.ItemId)
.Set(Weapon.Column_name, obj.Name)
.Set(Weapon.Column_unidentified_name_id, obj.UnidentifiedNameId)
.Set(Weapon.Column_identified_name_id, obj.IdentifiedNameId)
.Set(Weapon.Column_type, obj.Type)
.Set(Weapon.Column_material, obj.Material)
.Set(Weapon.Column_weight, obj.Weight)
.Set(Weapon.Column_invgfx, obj.Invgfx)
.Set(Weapon.Column_grdgfx, obj.Grdgfx)
.Set(Weapon.Column_itemdesc_id, obj.ItemdescId)
.Set(Weapon.Column_dmg_small, obj.DmgSmall)
.Set(Weapon.Column_dmg_large, obj.DmgLarge)
.Set(Weapon.Column_range, obj.Range)
.Set(Weapon.Column_safenchant, obj.Safenchant)
.Set(Weapon.Column_use_royal, obj.UseRoyal)
.Set(Weapon.Column_use_knight, obj.UseKnight)
.Set(Weapon.Column_use_mage, obj.UseMage)
.Set(Weapon.Column_use_elf, obj.UseElf)
.Set(Weapon.Column_use_darkelf, obj.UseDarkelf)
.Set(Weapon.Column_use_dragonknight, obj.UseDragonknight)
.Set(Weapon.Column_use_illusionist, obj.UseIllusionist)
.Set(Weapon.Column_hitmodifier, obj.Hitmodifier)
.Set(Weapon.Column_dmgmodifier, obj.Dmgmodifier)
.Set(Weapon.Column_add_str, obj.AddStr)
.Set(Weapon.Column_add_con, obj.AddCon)
.Set(Weapon.Column_add_dex, obj.AddDex)
.Set(Weapon.Column_add_int, obj.AddInt)
.Set(Weapon.Column_add_wis, obj.AddWis)
.Set(Weapon.Column_add_cha, obj.AddCha)
.Set(Weapon.Column_add_hp, obj.AddHp)
.Set(Weapon.Column_add_mp, obj.AddMp)
.Set(Weapon.Column_add_hpr, obj.AddHpr)
.Set(Weapon.Column_add_mpr, obj.AddMpr)
.Set(Weapon.Column_add_sp, obj.AddSp)
.Set(Weapon.Column_m_def, obj.MDef)
.Set(Weapon.Column_haste_item, obj.HasteItem)
.Set(Weapon.Column_double_dmg_chance, obj.DoubleDmgChance)
.Set(Weapon.Column_magicdmgmodifier, obj.Magicdmgmodifier)
.Set(Weapon.Column_canbedmg, obj.Canbedmg)
.Set(Weapon.Column_min_lvl, obj.MinLvl)
.Set(Weapon.Column_max_lvl, obj.MaxLvl)
.Set(Weapon.Column_bless, obj.Bless)
.Set(Weapon.Column_trade, obj.Trade)
.Set(Weapon.Column_cant_delete, obj.CantDelete)
.Set(Weapon.Column_max_use_time, obj.MaxUseTime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Weapon.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(Weapon.Column_item_id);
obj.Name = dataSourceRow.getString(Weapon.Column_name);
obj.UnidentifiedNameId = dataSourceRow.getString(Weapon.Column_unidentified_name_id);
obj.IdentifiedNameId = dataSourceRow.getString(Weapon.Column_identified_name_id);
obj.Type = dataSourceRow.getString(Weapon.Column_type);
obj.Material = dataSourceRow.getString(Weapon.Column_material);
obj.Weight = dataSourceRow.getString(Weapon.Column_weight);
obj.Invgfx = dataSourceRow.getString(Weapon.Column_invgfx);
obj.Grdgfx = dataSourceRow.getString(Weapon.Column_grdgfx);
obj.ItemdescId = dataSourceRow.getString(Weapon.Column_itemdesc_id);
obj.DmgSmall = dataSourceRow.getString(Weapon.Column_dmg_small);
obj.DmgLarge = dataSourceRow.getString(Weapon.Column_dmg_large);
obj.Range = dataSourceRow.getString(Weapon.Column_range);
obj.Safenchant = dataSourceRow.getString(Weapon.Column_safenchant);
obj.UseRoyal = dataSourceRow.getString(Weapon.Column_use_royal);
obj.UseKnight = dataSourceRow.getString(Weapon.Column_use_knight);
obj.UseMage = dataSourceRow.getString(Weapon.Column_use_mage);
obj.UseElf = dataSourceRow.getString(Weapon.Column_use_elf);
obj.UseDarkelf = dataSourceRow.getString(Weapon.Column_use_darkelf);
obj.UseDragonknight = dataSourceRow.getString(Weapon.Column_use_dragonknight);
obj.UseIllusionist = dataSourceRow.getString(Weapon.Column_use_illusionist);
obj.Hitmodifier = dataSourceRow.getString(Weapon.Column_hitmodifier);
obj.Dmgmodifier = dataSourceRow.getString(Weapon.Column_dmgmodifier);
obj.AddStr = dataSourceRow.getString(Weapon.Column_add_str);
obj.AddCon = dataSourceRow.getString(Weapon.Column_add_con);
obj.AddDex = dataSourceRow.getString(Weapon.Column_add_dex);
obj.AddInt = dataSourceRow.getString(Weapon.Column_add_int);
obj.AddWis = dataSourceRow.getString(Weapon.Column_add_wis);
obj.AddCha = dataSourceRow.getString(Weapon.Column_add_cha);
obj.AddHp = dataSourceRow.getString(Weapon.Column_add_hp);
obj.AddMp = dataSourceRow.getString(Weapon.Column_add_mp);
obj.AddHpr = dataSourceRow.getString(Weapon.Column_add_hpr);
obj.AddMpr = dataSourceRow.getString(Weapon.Column_add_mpr);
obj.AddSp = dataSourceRow.getString(Weapon.Column_add_sp);
obj.MDef = dataSourceRow.getString(Weapon.Column_m_def);
obj.HasteItem = dataSourceRow.getString(Weapon.Column_haste_item);
obj.DoubleDmgChance = dataSourceRow.getString(Weapon.Column_double_dmg_chance);
obj.Magicdmgmodifier = dataSourceRow.getString(Weapon.Column_magicdmgmodifier);
obj.Canbedmg = dataSourceRow.getString(Weapon.Column_canbedmg);
obj.MinLvl = dataSourceRow.getString(Weapon.Column_min_lvl);
obj.MaxLvl = dataSourceRow.getString(Weapon.Column_max_lvl);
obj.Bless = dataSourceRow.getString(Weapon.Column_bless);
obj.Trade = dataSourceRow.getString(Weapon.Column_trade);
obj.CantDelete = dataSourceRow.getString(Weapon.Column_cant_delete);
obj.MaxUseTime = dataSourceRow.getString(Weapon.Column_max_use_time);

