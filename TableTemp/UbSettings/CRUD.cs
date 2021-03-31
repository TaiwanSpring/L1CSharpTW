private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.UbSettings);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(UbSettings.Column_ub_id, obj.UbId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(UbSettings.Column_ub_id, obj.UbId)
.Set(UbSettings.Column_ub_name, obj.UbName)
.Set(UbSettings.Column_ub_mapid, obj.UbMapid)
.Set(UbSettings.Column_ub_area_x1, obj.UbAreaX1)
.Set(UbSettings.Column_ub_area_y1, obj.UbAreaY1)
.Set(UbSettings.Column_ub_area_x2, obj.UbAreaX2)
.Set(UbSettings.Column_ub_area_y2, obj.UbAreaY2)
.Set(UbSettings.Column_min_lvl, obj.MinLvl)
.Set(UbSettings.Column_max_lvl, obj.MaxLvl)
.Set(UbSettings.Column_max_player, obj.MaxPlayer)
.Set(UbSettings.Column_enter_royal, obj.EnterRoyal)
.Set(UbSettings.Column_enter_knight, obj.EnterKnight)
.Set(UbSettings.Column_enter_mage, obj.EnterMage)
.Set(UbSettings.Column_enter_elf, obj.EnterElf)
.Set(UbSettings.Column_enter_darkelf, obj.EnterDarkelf)
.Set(UbSettings.Column_enter_dragonknight, obj.EnterDragonknight)
.Set(UbSettings.Column_enter_illusionist, obj.EnterIllusionist)
.Set(UbSettings.Column_enter_male, obj.EnterMale)
.Set(UbSettings.Column_enter_female, obj.EnterFemale)
.Set(UbSettings.Column_use_pot, obj.UsePot)
.Set(UbSettings.Column_hpr_bonus, obj.HprBonus)
.Set(UbSettings.Column_mpr_bonus, obj.MprBonus)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(UbSettings.Column_ub_id, obj.UbId)
.Set(UbSettings.Column_ub_name, obj.UbName)
.Set(UbSettings.Column_ub_mapid, obj.UbMapid)
.Set(UbSettings.Column_ub_area_x1, obj.UbAreaX1)
.Set(UbSettings.Column_ub_area_y1, obj.UbAreaY1)
.Set(UbSettings.Column_ub_area_x2, obj.UbAreaX2)
.Set(UbSettings.Column_ub_area_y2, obj.UbAreaY2)
.Set(UbSettings.Column_min_lvl, obj.MinLvl)
.Set(UbSettings.Column_max_lvl, obj.MaxLvl)
.Set(UbSettings.Column_max_player, obj.MaxPlayer)
.Set(UbSettings.Column_enter_royal, obj.EnterRoyal)
.Set(UbSettings.Column_enter_knight, obj.EnterKnight)
.Set(UbSettings.Column_enter_mage, obj.EnterMage)
.Set(UbSettings.Column_enter_elf, obj.EnterElf)
.Set(UbSettings.Column_enter_darkelf, obj.EnterDarkelf)
.Set(UbSettings.Column_enter_dragonknight, obj.EnterDragonknight)
.Set(UbSettings.Column_enter_illusionist, obj.EnterIllusionist)
.Set(UbSettings.Column_enter_male, obj.EnterMale)
.Set(UbSettings.Column_enter_female, obj.EnterFemale)
.Set(UbSettings.Column_use_pot, obj.UsePot)
.Set(UbSettings.Column_hpr_bonus, obj.HprBonus)
.Set(UbSettings.Column_mpr_bonus, obj.MprBonus)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(UbSettings.Column_ub_id, obj.UbId)
.Execute();


obj.UbId = dataSourceRow.getString(UbSettings.Column_ub_id);
obj.UbName = dataSourceRow.getString(UbSettings.Column_ub_name);
obj.UbMapid = dataSourceRow.getString(UbSettings.Column_ub_mapid);
obj.UbAreaX1 = dataSourceRow.getString(UbSettings.Column_ub_area_x1);
obj.UbAreaY1 = dataSourceRow.getString(UbSettings.Column_ub_area_y1);
obj.UbAreaX2 = dataSourceRow.getString(UbSettings.Column_ub_area_x2);
obj.UbAreaY2 = dataSourceRow.getString(UbSettings.Column_ub_area_y2);
obj.MinLvl = dataSourceRow.getString(UbSettings.Column_min_lvl);
obj.MaxLvl = dataSourceRow.getString(UbSettings.Column_max_lvl);
obj.MaxPlayer = dataSourceRow.getString(UbSettings.Column_max_player);
obj.EnterRoyal = dataSourceRow.getString(UbSettings.Column_enter_royal);
obj.EnterKnight = dataSourceRow.getString(UbSettings.Column_enter_knight);
obj.EnterMage = dataSourceRow.getString(UbSettings.Column_enter_mage);
obj.EnterElf = dataSourceRow.getString(UbSettings.Column_enter_elf);
obj.EnterDarkelf = dataSourceRow.getString(UbSettings.Column_enter_darkelf);
obj.EnterDragonknight = dataSourceRow.getString(UbSettings.Column_enter_dragonknight);
obj.EnterIllusionist = dataSourceRow.getString(UbSettings.Column_enter_illusionist);
obj.EnterMale = dataSourceRow.getString(UbSettings.Column_enter_male);
obj.EnterFemale = dataSourceRow.getString(UbSettings.Column_enter_female);
obj.UsePot = dataSourceRow.getString(UbSettings.Column_use_pot);
obj.HprBonus = dataSourceRow.getString(UbSettings.Column_hpr_bonus);
obj.MprBonus = dataSourceRow.getString(UbSettings.Column_mpr_bonus);

