private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.SprAction);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(SprAction.Column_spr_id, obj.SprId)
.Where(SprAction.Column_act_id, obj.ActId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(SprAction.Column_spr_id, obj.SprId)
.Set(SprAction.Column_act_id, obj.ActId)
.Set(SprAction.Column_framecount, obj.Framecount)
.Set(SprAction.Column_framerate, obj.Framerate)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(SprAction.Column_spr_id, obj.SprId)
.Where(SprAction.Column_act_id, obj.ActId)
.Set(SprAction.Column_framecount, obj.Framecount)
.Set(SprAction.Column_framerate, obj.Framerate)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(SprAction.Column_spr_id, obj.SprId)
.Where(SprAction.Column_act_id, obj.ActId)
.Execute();


obj.SprId = dataSourceRow.getString(SprAction.Column_spr_id);
obj.ActId = dataSourceRow.getString(SprAction.Column_act_id);
obj.Framecount = dataSourceRow.getString(SprAction.Column_framecount);
obj.Framerate = dataSourceRow.getString(SprAction.Column_framerate);

