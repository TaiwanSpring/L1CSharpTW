private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ClanMembers);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ClanMembers.Column_index_id, obj.IndexId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ClanMembers.Column_clan_id, obj.ClanId)
.Set(ClanMembers.Column_index_id, obj.IndexId)
.Set(ClanMembers.Column_char_id, obj.CharId)
.Set(ClanMembers.Column_char_name, obj.CharName)
.Set(ClanMembers.Column_notes, obj.Notes)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(ClanMembers.Column_clan_id, obj.ClanId)
.Where(ClanMembers.Column_index_id, obj.IndexId)
.Set(ClanMembers.Column_char_id, obj.CharId)
.Set(ClanMembers.Column_char_name, obj.CharName)
.Set(ClanMembers.Column_notes, obj.Notes)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ClanMembers.Column_index_id, obj.IndexId)
.Execute();


obj.ClanId = dataSourceRow.getString(ClanMembers.Column_clan_id);
obj.IndexId = dataSourceRow.getString(ClanMembers.Column_index_id);
obj.CharId = dataSourceRow.getString(ClanMembers.Column_char_id);
obj.CharName = dataSourceRow.getString(ClanMembers.Column_char_name);
obj.Notes = dataSourceRow.getString(ClanMembers.Column_notes);

