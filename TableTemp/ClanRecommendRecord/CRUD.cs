private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ClanRecommendRecord);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ClanRecommendRecord.Column_clan_id, obj.ClanId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ClanRecommendRecord.Column_clan_id, obj.ClanId)
.Set(ClanRecommendRecord.Column_clan_name, obj.ClanName)
.Set(ClanRecommendRecord.Column_crown_name, obj.CrownName)
.Set(ClanRecommendRecord.Column_clan_type, obj.ClanType)
.Set(ClanRecommendRecord.Column_type_message, obj.TypeMessage)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(ClanRecommendRecord.Column_clan_id, obj.ClanId)
.Set(ClanRecommendRecord.Column_clan_name, obj.ClanName)
.Set(ClanRecommendRecord.Column_crown_name, obj.CrownName)
.Set(ClanRecommendRecord.Column_clan_type, obj.ClanType)
.Set(ClanRecommendRecord.Column_type_message, obj.TypeMessage)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ClanRecommendRecord.Column_clan_id, obj.ClanId)
.Execute();


obj.ClanId = dataSourceRow.getString(ClanRecommendRecord.Column_clan_id);
obj.ClanName = dataSourceRow.getString(ClanRecommendRecord.Column_clan_name);
obj.CrownName = dataSourceRow.getString(ClanRecommendRecord.Column_crown_name);
obj.ClanType = dataSourceRow.getString(ClanRecommendRecord.Column_clan_type);
obj.TypeMessage = dataSourceRow.getString(ClanRecommendRecord.Column_type_message);

