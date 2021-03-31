private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterBuddys);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterBuddys.Column_char_id, obj.CharId)
.Where(CharacterBuddys.Column_buddy_id, obj.BuddyId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterBuddys.Column_id, obj.Id)
.Set(CharacterBuddys.Column_char_id, obj.CharId)
.Set(CharacterBuddys.Column_buddy_id, obj.BuddyId)
.Set(CharacterBuddys.Column_buddy_name, obj.BuddyName)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(CharacterBuddys.Column_id, obj.Id)
.Where(CharacterBuddys.Column_char_id, obj.CharId)
.Where(CharacterBuddys.Column_buddy_id, obj.BuddyId)
.Set(CharacterBuddys.Column_buddy_name, obj.BuddyName)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterBuddys.Column_char_id, obj.CharId)
.Where(CharacterBuddys.Column_buddy_id, obj.BuddyId)
.Execute();


obj.Id = dataSourceRow.getString(CharacterBuddys.Column_id);
obj.CharId = dataSourceRow.getString(CharacterBuddys.Column_char_id);
obj.BuddyId = dataSourceRow.getString(CharacterBuddys.Column_buddy_id);
obj.BuddyName = dataSourceRow.getString(CharacterBuddys.Column_buddy_name);

