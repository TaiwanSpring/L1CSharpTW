private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.LogChat);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(LogChat.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(LogChat.Column_id, obj.Id)
.Set(LogChat.Column_account_name, obj.AccountName)
.Set(LogChat.Column_char_id, obj.CharId)
.Set(LogChat.Column_name, obj.Name)
.Set(LogChat.Column_clan_id, obj.ClanId)
.Set(LogChat.Column_clan_name, obj.ClanName)
.Set(LogChat.Column_locx, obj.Locx)
.Set(LogChat.Column_locy, obj.Locy)
.Set(LogChat.Column_mapid, obj.Mapid)
.Set(LogChat.Column_type, obj.Type)
.Set(LogChat.Column_target_account_name, obj.TargetAccountName)
.Set(LogChat.Column_target_id, obj.TargetId)
.Set(LogChat.Column_target_name, obj.TargetName)
.Set(LogChat.Column_target_clan_id, obj.TargetClanId)
.Set(LogChat.Column_target_clan_name, obj.TargetClanName)
.Set(LogChat.Column_target_locx, obj.TargetLocx)
.Set(LogChat.Column_target_locy, obj.TargetLocy)
.Set(LogChat.Column_target_mapid, obj.TargetMapid)
.Set(LogChat.Column_content, obj.Content)
.Set(LogChat.Column_datetime, obj.Datetime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(LogChat.Column_id, obj.Id)
.Set(LogChat.Column_account_name, obj.AccountName)
.Set(LogChat.Column_char_id, obj.CharId)
.Set(LogChat.Column_name, obj.Name)
.Set(LogChat.Column_clan_id, obj.ClanId)
.Set(LogChat.Column_clan_name, obj.ClanName)
.Set(LogChat.Column_locx, obj.Locx)
.Set(LogChat.Column_locy, obj.Locy)
.Set(LogChat.Column_mapid, obj.Mapid)
.Set(LogChat.Column_type, obj.Type)
.Set(LogChat.Column_target_account_name, obj.TargetAccountName)
.Set(LogChat.Column_target_id, obj.TargetId)
.Set(LogChat.Column_target_name, obj.TargetName)
.Set(LogChat.Column_target_clan_id, obj.TargetClanId)
.Set(LogChat.Column_target_clan_name, obj.TargetClanName)
.Set(LogChat.Column_target_locx, obj.TargetLocx)
.Set(LogChat.Column_target_locy, obj.TargetLocy)
.Set(LogChat.Column_target_mapid, obj.TargetMapid)
.Set(LogChat.Column_content, obj.Content)
.Set(LogChat.Column_datetime, obj.Datetime)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(LogChat.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(LogChat.Column_id);
obj.AccountName = dataSourceRow.getString(LogChat.Column_account_name);
obj.CharId = dataSourceRow.getString(LogChat.Column_char_id);
obj.Name = dataSourceRow.getString(LogChat.Column_name);
obj.ClanId = dataSourceRow.getString(LogChat.Column_clan_id);
obj.ClanName = dataSourceRow.getString(LogChat.Column_clan_name);
obj.Locx = dataSourceRow.getString(LogChat.Column_locx);
obj.Locy = dataSourceRow.getString(LogChat.Column_locy);
obj.Mapid = dataSourceRow.getString(LogChat.Column_mapid);
obj.Type = dataSourceRow.getString(LogChat.Column_type);
obj.TargetAccountName = dataSourceRow.getString(LogChat.Column_target_account_name);
obj.TargetId = dataSourceRow.getString(LogChat.Column_target_id);
obj.TargetName = dataSourceRow.getString(LogChat.Column_target_name);
obj.TargetClanId = dataSourceRow.getString(LogChat.Column_target_clan_id);
obj.TargetClanName = dataSourceRow.getString(LogChat.Column_target_clan_name);
obj.TargetLocx = dataSourceRow.getString(LogChat.Column_target_locx);
obj.TargetLocy = dataSourceRow.getString(LogChat.Column_target_locy);
obj.TargetMapid = dataSourceRow.getString(LogChat.Column_target_mapid);
obj.Content = dataSourceRow.getString(LogChat.Column_content);
obj.Datetime = dataSourceRow.getString(LogChat.Column_datetime);

