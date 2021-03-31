private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Armor);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Armor.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Armor.Column_item_id, obj.ItemId)
.Set(Armor.Column_name, obj.Name)
.Set(Armor.Column_unidentified_name_id, obj.UnidentifiedNameId)
.Set(Armor.Column_identified_name_id, obj.IdentifiedNameId)
.Set(Armor.Column_type, obj.Type)
.Set(Armor.Column_material, obj.Material)
.Set(Armor.Column_weight, obj.Weight)
.Set(Armor.Column_invgfx, obj.Invgfx)
.Set(Armor.Column_grdgfx, obj.Grdgfx)
.Set(Armor.Column_itemdesc_id, obj.ItemdescId)
.Set(Armor.Column_ac, obj.Ac)
.Set(Armor.Column_safenchant, obj.Safenchant)
.Set(Armor.Column_use_royal, obj.UseRoyal)
.Set(Armor.Column_use_knight, obj.UseKnight)
.Set(Armor.Column_use_mage, obj.UseMage)
.Set(Armor.Column_use_elf, obj.UseElf)
.Set(Armor.Column_use_darkelf, obj.UseDarkelf)
.Set(Armor.Column_use_dragonknight, obj.UseDragonknight)
.Set(Armor.Column_use_illusionist, obj.UseIllusionist)
.Set(Armor.Column_add_str, obj.AddStr)
.Set(Armor.Column_add_con, obj.AddCon)
.Set(Armor.Column_add_dex, obj.AddDex)
.Set(Armor.Column_add_int, obj.AddInt)
.Set(Armor.Column_add_wis, obj.AddWis)
.Set(Armor.Column_add_cha, obj.AddCha)
.Set(Armor.Column_add_hp, obj.AddHp)
.Set(Armor.Column_add_mp, obj.AddMp)
.Set(Armor.Column_add_hpr, obj.AddHpr)
.Set(Armor.Column_add_mpr, obj.AddMpr)
.Set(Armor.Column_add_sp, obj.AddSp)
.Set(Armor.Column_min_lvl, obj.MinLvl)
.Set(Armor.Column_max_lvl, obj.MaxLvl)
.Set(Armor.Column_m_def, obj.MDef)
.Set(Armor.Column_haste_item, obj.HasteItem)
.Set(Armor.Column_damage_reduction, obj.DamageReduction)
.Set(Armor.Column_weight_reduction, obj.WeightReduction)
.Set(Armor.Column_hit_modifier, obj.HitModifier)
.Set(Armor.Column_dmg_modifier, obj.DmgModifier)
.Set(Armor.Column_bow_hit_modifier, obj.BowHitModifier)
.Set(Armor.Column_bow_dmg_modifier, obj.BowDmgModifier)
.Set(Armor.Column_bless, obj.Bless)
.Set(Armor.Column_trade, obj.Trade)
.Set(Armor.Column_cant_delete, obj.CantDelete)
.Set(Armor.Column_max_use_time, obj.MaxUseTime)
.Set(Armor.Column_defense_water, obj.DefenseWater)
.Set(Armor.Column_defense_wind, obj.DefenseWind)
.Set(Armor.Column_defense_fire, obj.DefenseFire)
.Set(Armor.Column_defense_earth, obj.DefenseEarth)
.Set(Armor.Column_regist_stun, obj.RegistStun)
.Set(Armor.Column_regist_stone, obj.RegistStone)
.Set(Armor.Column_regist_sleep, obj.RegistSleep)
.Set(Armor.Column_regist_freeze, obj.RegistFreeze)
.Set(Armor.Column_regist_sustain, obj.RegistSustain)
.Set(Armor.Column_regist_blind, obj.RegistBlind)
.Set(Armor.Column_grade, obj.Grade)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Armor.Column_item_id, obj.ItemId)
.Set(Armor.Column_name, obj.Name)
.Set(Armor.Column_unidentified_name_id, obj.UnidentifiedNameId)
.Set(Armor.Column_identified_name_id, obj.IdentifiedNameId)
.Set(Armor.Column_type, obj.Type)
.Set(Armor.Column_material, obj.Material)
.Set(Armor.Column_weight, obj.Weight)
.Set(Armor.Column_invgfx, obj.Invgfx)
.Set(Armor.Column_grdgfx, obj.Grdgfx)
.Set(Armor.Column_itemdesc_id, obj.ItemdescId)
.Set(Armor.Column_ac, obj.Ac)
.Set(Armor.Column_safenchant, obj.Safenchant)
.Set(Armor.Column_use_royal, obj.UseRoyal)
.Set(Armor.Column_use_knight, obj.UseKnight)
.Set(Armor.Column_use_mage, obj.UseMage)
.Set(Armor.Column_use_elf, obj.UseElf)
.Set(Armor.Column_use_darkelf, obj.UseDarkelf)
.Set(Armor.Column_use_dragonknight, obj.UseDragonknight)
.Set(Armor.Column_use_illusionist, obj.UseIllusionist)
.Set(Armor.Column_add_str, obj.AddStr)
.Set(Armor.Column_add_con, obj.AddCon)
.Set(Armor.Column_add_dex, obj.AddDex)
.Set(Armor.Column_add_int, obj.AddInt)
.Set(Armor.Column_add_wis, obj.AddWis)
.Set(Armor.Column_add_cha, obj.AddCha)
.Set(Armor.Column_add_hp, obj.AddHp)
.Set(Armor.Column_add_mp, obj.AddMp)
.Set(Armor.Column_add_hpr, obj.AddHpr)
.Set(Armor.Column_add_mpr, obj.AddMpr)
.Set(Armor.Column_add_sp, obj.AddSp)
.Set(Armor.Column_min_lvl, obj.MinLvl)
.Set(Armor.Column_max_lvl, obj.MaxLvl)
.Set(Armor.Column_m_def, obj.MDef)
.Set(Armor.Column_haste_item, obj.HasteItem)
.Set(Armor.Column_damage_reduction, obj.DamageReduction)
.Set(Armor.Column_weight_reduction, obj.WeightReduction)
.Set(Armor.Column_hit_modifier, obj.HitModifier)
.Set(Armor.Column_dmg_modifier, obj.DmgModifier)
.Set(Armor.Column_bow_hit_modifier, obj.BowHitModifier)
.Set(Armor.Column_bow_dmg_modifier, obj.BowDmgModifier)
.Set(Armor.Column_bless, obj.Bless)
.Set(Armor.Column_trade, obj.Trade)
.Set(Armor.Column_cant_delete, obj.CantDelete)
.Set(Armor.Column_max_use_time, obj.MaxUseTime)
.Set(Armor.Column_defense_water, obj.DefenseWater)
.Set(Armor.Column_defense_wind, obj.DefenseWind)
.Set(Armor.Column_defense_fire, obj.DefenseFire)
.Set(Armor.Column_defense_earth, obj.DefenseEarth)
.Set(Armor.Column_regist_stun, obj.RegistStun)
.Set(Armor.Column_regist_stone, obj.RegistStone)
.Set(Armor.Column_regist_sleep, obj.RegistSleep)
.Set(Armor.Column_regist_freeze, obj.RegistFreeze)
.Set(Armor.Column_regist_sustain, obj.RegistSustain)
.Set(Armor.Column_regist_blind, obj.RegistBlind)
.Set(Armor.Column_grade, obj.Grade)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Armor.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(Armor.Column_item_id);
obj.Name = dataSourceRow.getString(Armor.Column_name);
obj.UnidentifiedNameId = dataSourceRow.getString(Armor.Column_unidentified_name_id);
obj.IdentifiedNameId = dataSourceRow.getString(Armor.Column_identified_name_id);
obj.Type = dataSourceRow.getString(Armor.Column_type);
obj.Material = dataSourceRow.getString(Armor.Column_material);
obj.Weight = dataSourceRow.getString(Armor.Column_weight);
obj.Invgfx = dataSourceRow.getString(Armor.Column_invgfx);
obj.Grdgfx = dataSourceRow.getString(Armor.Column_grdgfx);
obj.ItemdescId = dataSourceRow.getString(Armor.Column_itemdesc_id);
obj.Ac = dataSourceRow.getString(Armor.Column_ac);
obj.Safenchant = dataSourceRow.getString(Armor.Column_safenchant);
obj.UseRoyal = dataSourceRow.getString(Armor.Column_use_royal);
obj.UseKnight = dataSourceRow.getString(Armor.Column_use_knight);
obj.UseMage = dataSourceRow.getString(Armor.Column_use_mage);
obj.UseElf = dataSourceRow.getString(Armor.Column_use_elf);
obj.UseDarkelf = dataSourceRow.getString(Armor.Column_use_darkelf);
obj.UseDragonknight = dataSourceRow.getString(Armor.Column_use_dragonknight);
obj.UseIllusionist = dataSourceRow.getString(Armor.Column_use_illusionist);
obj.AddStr = dataSourceRow.getString(Armor.Column_add_str);
obj.AddCon = dataSourceRow.getString(Armor.Column_add_con);
obj.AddDex = dataSourceRow.getString(Armor.Column_add_dex);
obj.AddInt = dataSourceRow.getString(Armor.Column_add_int);
obj.AddWis = dataSourceRow.getString(Armor.Column_add_wis);
obj.AddCha = dataSourceRow.getString(Armor.Column_add_cha);
obj.AddHp = dataSourceRow.getString(Armor.Column_add_hp);
obj.AddMp = dataSourceRow.getString(Armor.Column_add_mp);
obj.AddHpr = dataSourceRow.getString(Armor.Column_add_hpr);
obj.AddMpr = dataSourceRow.getString(Armor.Column_add_mpr);
obj.AddSp = dataSourceRow.getString(Armor.Column_add_sp);
obj.MinLvl = dataSourceRow.getString(Armor.Column_min_lvl);
obj.MaxLvl = dataSourceRow.getString(Armor.Column_max_lvl);
obj.MDef = dataSourceRow.getString(Armor.Column_m_def);
obj.HasteItem = dataSourceRow.getString(Armor.Column_haste_item);
obj.DamageReduction = dataSourceRow.getString(Armor.Column_damage_reduction);
obj.WeightReduction = dataSourceRow.getString(Armor.Column_weight_reduction);
obj.HitModifier = dataSourceRow.getString(Armor.Column_hit_modifier);
obj.DmgModifier = dataSourceRow.getString(Armor.Column_dmg_modifier);
obj.BowHitModifier = dataSourceRow.getString(Armor.Column_bow_hit_modifier);
obj.BowDmgModifier = dataSourceRow.getString(Armor.Column_bow_dmg_modifier);
obj.Bless = dataSourceRow.getString(Armor.Column_bless);
obj.Trade = dataSourceRow.getString(Armor.Column_trade);
obj.CantDelete = dataSourceRow.getString(Armor.Column_cant_delete);
obj.MaxUseTime = dataSourceRow.getString(Armor.Column_max_use_time);
obj.DefenseWater = dataSourceRow.getString(Armor.Column_defense_water);
obj.DefenseWind = dataSourceRow.getString(Armor.Column_defense_wind);
obj.DefenseFire = dataSourceRow.getString(Armor.Column_defense_fire);
obj.DefenseEarth = dataSourceRow.getString(Armor.Column_defense_earth);
obj.RegistStun = dataSourceRow.getString(Armor.Column_regist_stun);
obj.RegistStone = dataSourceRow.getString(Armor.Column_regist_stone);
obj.RegistSleep = dataSourceRow.getString(Armor.Column_regist_sleep);
obj.RegistFreeze = dataSourceRow.getString(Armor.Column_regist_freeze);
obj.RegistSustain = dataSourceRow.getString(Armor.Column_regist_sustain);
obj.RegistBlind = dataSourceRow.getString(Armor.Column_regist_blind);
obj.Grade = dataSourceRow.getString(Armor.Column_grade);

