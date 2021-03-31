private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Npcaction);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Npcaction.Column_npcid, obj.Npcid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Npcaction.Column_npcid, obj.Npcid)
.Set(Npcaction.Column_normal_action, obj.NormalAction)
.Set(Npcaction.Column_caotic_action, obj.CaoticAction)
.Set(Npcaction.Column_teleport_url, obj.TeleportUrl)
.Set(Npcaction.Column_teleport_urla, obj.TeleportUrla)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Npcaction.Column_npcid, obj.Npcid)
.Set(Npcaction.Column_normal_action, obj.NormalAction)
.Set(Npcaction.Column_caotic_action, obj.CaoticAction)
.Set(Npcaction.Column_teleport_url, obj.TeleportUrl)
.Set(Npcaction.Column_teleport_urla, obj.TeleportUrla)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Npcaction.Column_npcid, obj.Npcid)
.Execute();


obj.Npcid = dataSourceRow.getString(Npcaction.Column_npcid);
obj.NormalAction = dataSourceRow.getString(Npcaction.Column_normal_action);
obj.CaoticAction = dataSourceRow.getString(Npcaction.Column_caotic_action);
obj.TeleportUrl = dataSourceRow.getString(Npcaction.Column_teleport_url);
obj.TeleportUrla = dataSourceRow.getString(Npcaction.Column_teleport_urla);

