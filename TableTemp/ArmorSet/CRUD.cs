private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ArmorSet);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ArmorSet.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ArmorSet.Column_id, obj.Id)
.Set(ArmorSet.Column_note, obj.Note)
.Set(ArmorSet.Column_sets, obj.Sets)
.Set(ArmorSet.Column_polyid, obj.Polyid)
.Set(ArmorSet.Column_ac, obj.Ac)
.Set(ArmorSet.Column_hp, obj.Hp)
.Set(ArmorSet.Column_mp, obj.Mp)
.Set(ArmorSet.Column_hpr, obj.Hpr)
.Set(ArmorSet.Column_mpr, obj.Mpr)
.Set(ArmorSet.Column_mr, obj.Mr)
.Set(ArmorSet.Column_str, obj.Str)
.Set(ArmorSet.Column_dex, obj.Dex)
.Set(ArmorSet.Column_con, obj.Con)
.Set(ArmorSet.Column_wis, obj.Wis)
.Set(ArmorSet.Column_cha, obj.Cha)
.Set(ArmorSet.Column_intl, obj.Intl)
.Set(ArmorSet.Column_hit_modifier, obj.HitModifier)
.Set(ArmorSet.Column_dmg_modifier, obj.DmgModifier)
.Set(ArmorSet.Column_bow_hit_modifier, obj.BowHitModifier)
.Set(ArmorSet.Column_bow_dmg_modifier, obj.BowDmgModifier)
.Set(ArmorSet.Column_sp, obj.Sp)
.Set(ArmorSet.Column_defense_water, obj.DefenseWater)
.Set(ArmorSet.Column_defense_wind, obj.DefenseWind)
.Set(ArmorSet.Column_defense_fire, obj.DefenseFire)
.Set(ArmorSet.Column_defense_earth, obj.DefenseEarth)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(ArmorSet.Column_id, obj.Id)
.Set(ArmorSet.Column_note, obj.Note)
.Set(ArmorSet.Column_sets, obj.Sets)
.Set(ArmorSet.Column_polyid, obj.Polyid)
.Set(ArmorSet.Column_ac, obj.Ac)
.Set(ArmorSet.Column_hp, obj.Hp)
.Set(ArmorSet.Column_mp, obj.Mp)
.Set(ArmorSet.Column_hpr, obj.Hpr)
.Set(ArmorSet.Column_mpr, obj.Mpr)
.Set(ArmorSet.Column_mr, obj.Mr)
.Set(ArmorSet.Column_str, obj.Str)
.Set(ArmorSet.Column_dex, obj.Dex)
.Set(ArmorSet.Column_con, obj.Con)
.Set(ArmorSet.Column_wis, obj.Wis)
.Set(ArmorSet.Column_cha, obj.Cha)
.Set(ArmorSet.Column_intl, obj.Intl)
.Set(ArmorSet.Column_hit_modifier, obj.HitModifier)
.Set(ArmorSet.Column_dmg_modifier, obj.DmgModifier)
.Set(ArmorSet.Column_bow_hit_modifier, obj.BowHitModifier)
.Set(ArmorSet.Column_bow_dmg_modifier, obj.BowDmgModifier)
.Set(ArmorSet.Column_sp, obj.Sp)
.Set(ArmorSet.Column_defense_water, obj.DefenseWater)
.Set(ArmorSet.Column_defense_wind, obj.DefenseWind)
.Set(ArmorSet.Column_defense_fire, obj.DefenseFire)
.Set(ArmorSet.Column_defense_earth, obj.DefenseEarth)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ArmorSet.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(ArmorSet.Column_id);
obj.Note = dataSourceRow.getString(ArmorSet.Column_note);
obj.Sets = dataSourceRow.getString(ArmorSet.Column_sets);
obj.Polyid = dataSourceRow.getString(ArmorSet.Column_polyid);
obj.Ac = dataSourceRow.getString(ArmorSet.Column_ac);
obj.Hp = dataSourceRow.getString(ArmorSet.Column_hp);
obj.Mp = dataSourceRow.getString(ArmorSet.Column_mp);
obj.Hpr = dataSourceRow.getString(ArmorSet.Column_hpr);
obj.Mpr = dataSourceRow.getString(ArmorSet.Column_mpr);
obj.Mr = dataSourceRow.getString(ArmorSet.Column_mr);
obj.Str = dataSourceRow.getString(ArmorSet.Column_str);
obj.Dex = dataSourceRow.getString(ArmorSet.Column_dex);
obj.Con = dataSourceRow.getString(ArmorSet.Column_con);
obj.Wis = dataSourceRow.getString(ArmorSet.Column_wis);
obj.Cha = dataSourceRow.getString(ArmorSet.Column_cha);
obj.Intl = dataSourceRow.getString(ArmorSet.Column_intl);
obj.HitModifier = dataSourceRow.getString(ArmorSet.Column_hit_modifier);
obj.DmgModifier = dataSourceRow.getString(ArmorSet.Column_dmg_modifier);
obj.BowHitModifier = dataSourceRow.getString(ArmorSet.Column_bow_hit_modifier);
obj.BowDmgModifier = dataSourceRow.getString(ArmorSet.Column_bow_dmg_modifier);
obj.Sp = dataSourceRow.getString(ArmorSet.Column_sp);
obj.DefenseWater = dataSourceRow.getString(ArmorSet.Column_defense_water);
obj.DefenseWind = dataSourceRow.getString(ArmorSet.Column_defense_wind);
obj.DefenseFire = dataSourceRow.getString(ArmorSet.Column_defense_fire);
obj.DefenseEarth = dataSourceRow.getString(ArmorSet.Column_defense_earth);

