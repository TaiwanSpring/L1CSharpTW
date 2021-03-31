private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.WilliamLevel);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(WilliamLevel.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(WilliamLevel.Column_id, obj.Id)
.Set(WilliamLevel.Column_註解, obj.註解)
.Set(WilliamLevel.Column_level, obj.Level)
.Set(WilliamLevel.Column_give_royal, obj.GiveRoyal)
.Set(WilliamLevel.Column_give_knight, obj.GiveKnight)
.Set(WilliamLevel.Column_give_mage, obj.GiveMage)
.Set(WilliamLevel.Column_give_elf, obj.GiveElf)
.Set(WilliamLevel.Column_give_darkelf, obj.GiveDarkelf)
.Set(WilliamLevel.Column_give_dragonknight, obj.GiveDragonknight)
.Set(WilliamLevel.Column_give_illusionist, obj.GiveIllusionist)
.Set(WilliamLevel.Column_getItem, obj.GetItem)
.Set(WilliamLevel.Column_count, obj.Count)
.Set(WilliamLevel.Column_enchantlvl, obj.Enchantlvl)
.Set(WilliamLevel.Column_quest_id, obj.QuestId)
.Set(WilliamLevel.Column_quest_step, obj.QuestStep)
.Set(WilliamLevel.Column_message, obj.Message)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(WilliamLevel.Column_id, obj.Id)
.Set(WilliamLevel.Column_註解, obj.註解)
.Set(WilliamLevel.Column_level, obj.Level)
.Set(WilliamLevel.Column_give_royal, obj.GiveRoyal)
.Set(WilliamLevel.Column_give_knight, obj.GiveKnight)
.Set(WilliamLevel.Column_give_mage, obj.GiveMage)
.Set(WilliamLevel.Column_give_elf, obj.GiveElf)
.Set(WilliamLevel.Column_give_darkelf, obj.GiveDarkelf)
.Set(WilliamLevel.Column_give_dragonknight, obj.GiveDragonknight)
.Set(WilliamLevel.Column_give_illusionist, obj.GiveIllusionist)
.Set(WilliamLevel.Column_getItem, obj.GetItem)
.Set(WilliamLevel.Column_count, obj.Count)
.Set(WilliamLevel.Column_enchantlvl, obj.Enchantlvl)
.Set(WilliamLevel.Column_quest_id, obj.QuestId)
.Set(WilliamLevel.Column_quest_step, obj.QuestStep)
.Set(WilliamLevel.Column_message, obj.Message)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(WilliamLevel.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(WilliamLevel.Column_id);
obj.註解 = dataSourceRow.getString(WilliamLevel.Column_註解);
obj.Level = dataSourceRow.getString(WilliamLevel.Column_level);
obj.GiveRoyal = dataSourceRow.getString(WilliamLevel.Column_give_royal);
obj.GiveKnight = dataSourceRow.getString(WilliamLevel.Column_give_knight);
obj.GiveMage = dataSourceRow.getString(WilliamLevel.Column_give_mage);
obj.GiveElf = dataSourceRow.getString(WilliamLevel.Column_give_elf);
obj.GiveDarkelf = dataSourceRow.getString(WilliamLevel.Column_give_darkelf);
obj.GiveDragonknight = dataSourceRow.getString(WilliamLevel.Column_give_dragonknight);
obj.GiveIllusionist = dataSourceRow.getString(WilliamLevel.Column_give_illusionist);
obj.GetItem = dataSourceRow.getString(WilliamLevel.Column_getItem);
obj.Count = dataSourceRow.getString(WilliamLevel.Column_count);
obj.Enchantlvl = dataSourceRow.getString(WilliamLevel.Column_enchantlvl);
obj.QuestId = dataSourceRow.getString(WilliamLevel.Column_quest_id);
obj.QuestStep = dataSourceRow.getString(WilliamLevel.Column_quest_step);
obj.Message = dataSourceRow.getString(WilliamLevel.Column_message);

