private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ClanData);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ClanData.Column_clan_id, obj.ClanId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ClanData.Column_clan_id, obj.ClanId)
.Set(ClanData.Column_clan_name, obj.ClanName)
.Set(ClanData.Column_leader_id, obj.LeaderId)
.Set(ClanData.Column_leader_name, obj.LeaderName)
.Set(ClanData.Column_hascastle, obj.Hascastle)
.Set(ClanData.Column_hashouse, obj.Hashouse)
.Set(ClanData.Column_found_date, obj.FoundDate)
.Set(ClanData.Column_announcement, obj.Announcement)
.Set(ClanData.Column_emblem_id, obj.EmblemId)
.Set(ClanData.Column_emblem_status, obj.EmblemStatus)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(ClanData.Column_clan_id, obj.ClanId)
.Set(ClanData.Column_clan_name, obj.ClanName)
.Set(ClanData.Column_leader_id, obj.LeaderId)
.Set(ClanData.Column_leader_name, obj.LeaderName)
.Set(ClanData.Column_hascastle, obj.Hascastle)
.Set(ClanData.Column_hashouse, obj.Hashouse)
.Set(ClanData.Column_found_date, obj.FoundDate)
.Set(ClanData.Column_announcement, obj.Announcement)
.Set(ClanData.Column_emblem_id, obj.EmblemId)
.Set(ClanData.Column_emblem_status, obj.EmblemStatus)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ClanData.Column_clan_id, obj.ClanId)
.Execute();


obj.ClanId = dataSourceRow.getString(ClanData.Column_clan_id);
obj.ClanName = dataSourceRow.getString(ClanData.Column_clan_name);
obj.LeaderId = dataSourceRow.getString(ClanData.Column_leader_id);
obj.LeaderName = dataSourceRow.getString(ClanData.Column_leader_name);
obj.Hascastle = dataSourceRow.getString(ClanData.Column_hascastle);
obj.Hashouse = dataSourceRow.getString(ClanData.Column_hashouse);
obj.FoundDate = dataSourceRow.getString(ClanData.Column_found_date);
obj.Announcement = dataSourceRow.getString(ClanData.Column_announcement);
obj.EmblemId = dataSourceRow.getString(ClanData.Column_emblem_id);
obj.EmblemStatus = dataSourceRow.getString(ClanData.Column_emblem_status);

