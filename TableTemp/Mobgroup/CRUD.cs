private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Mobgroup);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Mobgroup.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Mobgroup.Column_id, obj.Id)
.Set(Mobgroup.Column_note, obj.Note)
.Set(Mobgroup.Column_remove_group_if_leader_die, obj.RemoveGroupIfLeaderDie)
.Set(Mobgroup.Column_leader_id, obj.LeaderId)
.Set(Mobgroup.Column_minion1_id, obj.Minion1Id)
.Set(Mobgroup.Column_minion1_count, obj.Minion1Count)
.Set(Mobgroup.Column_minion2_id, obj.Minion2Id)
.Set(Mobgroup.Column_minion2_count, obj.Minion2Count)
.Set(Mobgroup.Column_minion3_id, obj.Minion3Id)
.Set(Mobgroup.Column_minion3_count, obj.Minion3Count)
.Set(Mobgroup.Column_minion4_id, obj.Minion4Id)
.Set(Mobgroup.Column_minion4_count, obj.Minion4Count)
.Set(Mobgroup.Column_minion5_id, obj.Minion5Id)
.Set(Mobgroup.Column_minion5_count, obj.Minion5Count)
.Set(Mobgroup.Column_minion6_id, obj.Minion6Id)
.Set(Mobgroup.Column_minion6_count, obj.Minion6Count)
.Set(Mobgroup.Column_minion7_id, obj.Minion7Id)
.Set(Mobgroup.Column_minion7_count, obj.Minion7Count)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Mobgroup.Column_id, obj.Id)
.Set(Mobgroup.Column_note, obj.Note)
.Set(Mobgroup.Column_remove_group_if_leader_die, obj.RemoveGroupIfLeaderDie)
.Set(Mobgroup.Column_leader_id, obj.LeaderId)
.Set(Mobgroup.Column_minion1_id, obj.Minion1Id)
.Set(Mobgroup.Column_minion1_count, obj.Minion1Count)
.Set(Mobgroup.Column_minion2_id, obj.Minion2Id)
.Set(Mobgroup.Column_minion2_count, obj.Minion2Count)
.Set(Mobgroup.Column_minion3_id, obj.Minion3Id)
.Set(Mobgroup.Column_minion3_count, obj.Minion3Count)
.Set(Mobgroup.Column_minion4_id, obj.Minion4Id)
.Set(Mobgroup.Column_minion4_count, obj.Minion4Count)
.Set(Mobgroup.Column_minion5_id, obj.Minion5Id)
.Set(Mobgroup.Column_minion5_count, obj.Minion5Count)
.Set(Mobgroup.Column_minion6_id, obj.Minion6Id)
.Set(Mobgroup.Column_minion6_count, obj.Minion6Count)
.Set(Mobgroup.Column_minion7_id, obj.Minion7Id)
.Set(Mobgroup.Column_minion7_count, obj.Minion7Count)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Mobgroup.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Mobgroup.Column_id);
obj.Note = dataSourceRow.getString(Mobgroup.Column_note);
obj.RemoveGroupIfLeaderDie = dataSourceRow.getString(Mobgroup.Column_remove_group_if_leader_die);
obj.LeaderId = dataSourceRow.getString(Mobgroup.Column_leader_id);
obj.Minion1Id = dataSourceRow.getString(Mobgroup.Column_minion1_id);
obj.Minion1Count = dataSourceRow.getString(Mobgroup.Column_minion1_count);
obj.Minion2Id = dataSourceRow.getString(Mobgroup.Column_minion2_id);
obj.Minion2Count = dataSourceRow.getString(Mobgroup.Column_minion2_count);
obj.Minion3Id = dataSourceRow.getString(Mobgroup.Column_minion3_id);
obj.Minion3Count = dataSourceRow.getString(Mobgroup.Column_minion3_count);
obj.Minion4Id = dataSourceRow.getString(Mobgroup.Column_minion4_id);
obj.Minion4Count = dataSourceRow.getString(Mobgroup.Column_minion4_count);
obj.Minion5Id = dataSourceRow.getString(Mobgroup.Column_minion5_id);
obj.Minion5Count = dataSourceRow.getString(Mobgroup.Column_minion5_count);
obj.Minion6Id = dataSourceRow.getString(Mobgroup.Column_minion6_id);
obj.Minion6Count = dataSourceRow.getString(Mobgroup.Column_minion6_count);
obj.Minion7Id = dataSourceRow.getString(Mobgroup.Column_minion7_id);
obj.Minion7Count = dataSourceRow.getString(Mobgroup.Column_minion7_count);

