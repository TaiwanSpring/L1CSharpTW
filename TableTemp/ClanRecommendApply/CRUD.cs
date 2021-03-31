private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.ClanRecommendApply);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(ClanRecommendApply.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(ClanRecommendApply.Column_id, obj.Id)
.Set(ClanRecommendApply.Column_clan_id, obj.ClanId)
.Set(ClanRecommendApply.Column_clan_name, obj.ClanName)
.Set(ClanRecommendApply.Column_char_name, obj.CharName)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(ClanRecommendApply.Column_id, obj.Id)
.Set(ClanRecommendApply.Column_clan_id, obj.ClanId)
.Set(ClanRecommendApply.Column_clan_name, obj.ClanName)
.Set(ClanRecommendApply.Column_char_name, obj.CharName)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(ClanRecommendApply.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(ClanRecommendApply.Column_id);
obj.ClanId = dataSourceRow.getString(ClanRecommendApply.Column_clan_id);
obj.ClanName = dataSourceRow.getString(ClanRecommendApply.Column_clan_name);
obj.CharName = dataSourceRow.getString(ClanRecommendApply.Column_char_name);

