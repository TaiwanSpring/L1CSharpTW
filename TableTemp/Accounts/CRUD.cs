private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Accounts);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Accounts.Column_login, obj.Login)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Accounts.Column_login, obj.Login)
.Set(Accounts.Column_password, obj.Password)
.Set(Accounts.Column_lastactive, obj.Lastactive)
.Set(Accounts.Column_access_level, obj.AccessLevel)
.Set(Accounts.Column_ip, obj.Ip)
.Set(Accounts.Column_host, obj.Host)
.Set(Accounts.Column_online, obj.Online)
.Set(Accounts.Column_banned, obj.Banned)
.Set(Accounts.Column_character_slot, obj.CharacterSlot)
.Set(Accounts.Column_warepassword, obj.Warepassword)
.Set(Accounts.Column_OnlineStatus, obj.OnlineStatus)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Accounts.Column_login, obj.Login)
.Set(Accounts.Column_password, obj.Password)
.Set(Accounts.Column_lastactive, obj.Lastactive)
.Set(Accounts.Column_access_level, obj.AccessLevel)
.Set(Accounts.Column_ip, obj.Ip)
.Set(Accounts.Column_host, obj.Host)
.Set(Accounts.Column_online, obj.Online)
.Set(Accounts.Column_banned, obj.Banned)
.Set(Accounts.Column_character_slot, obj.CharacterSlot)
.Set(Accounts.Column_warepassword, obj.Warepassword)
.Set(Accounts.Column_OnlineStatus, obj.OnlineStatus)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Accounts.Column_login, obj.Login)
.Execute();


obj.Login = dataSourceRow.getString(Accounts.Column_login);
obj.Password = dataSourceRow.getString(Accounts.Column_password);
obj.Lastactive = dataSourceRow.getString(Accounts.Column_lastactive);
obj.AccessLevel = dataSourceRow.getString(Accounts.Column_access_level);
obj.Ip = dataSourceRow.getString(Accounts.Column_ip);
obj.Host = dataSourceRow.getString(Accounts.Column_host);
obj.Online = dataSourceRow.getString(Accounts.Column_online);
obj.Banned = dataSourceRow.getString(Accounts.Column_banned);
obj.CharacterSlot = dataSourceRow.getString(Accounts.Column_character_slot);
obj.Warepassword = dataSourceRow.getString(Accounts.Column_warepassword);
obj.OnlineStatus = dataSourceRow.getString(Accounts.Column_OnlineStatus);

