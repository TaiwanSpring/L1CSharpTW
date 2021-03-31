private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterQuests);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterQuests.Column_char_id, obj.CharId)
.Where(CharacterQuests.Column_quest_id, obj.QuestId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterQuests.Column_char_id, obj.CharId)
.Set(CharacterQuests.Column_quest_id, obj.QuestId)
.Set(CharacterQuests.Column_quest_step, obj.QuestStep)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterQuests.Column_char_id, obj.CharId)
.Where(CharacterQuests.Column_quest_id, obj.QuestId)
.Set(CharacterQuests.Column_quest_step, obj.QuestStep)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterQuests.Column_char_id, obj.CharId)
.Where(CharacterQuests.Column_quest_id, obj.QuestId)
.Execute();


obj.CharId = dataSourceRow.getString(CharacterQuests.Column_char_id);
obj.QuestId = dataSourceRow.getString(CharacterQuests.Column_quest_id);
obj.QuestStep = dataSourceRow.getString(CharacterQuests.Column_quest_step);

