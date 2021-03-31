private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.RaceTicket);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(RaceTicket.Column_item_obj_id, obj.ItemObjId)
.Where(RaceTicket.Column_round, obj.Round)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(RaceTicket.Column_item_obj_id, obj.ItemObjId)
.Set(RaceTicket.Column_round, obj.Round)
.Set(RaceTicket.Column_allotment_percentage, obj.AllotmentPercentage)
.Set(RaceTicket.Column_victory, obj.Victory)
.Set(RaceTicket.Column_runner_num, obj.RunnerNum)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(RaceTicket.Column_item_obj_id, obj.ItemObjId)
.Where(RaceTicket.Column_round, obj.Round)
.Set(RaceTicket.Column_allotment_percentage, obj.AllotmentPercentage)
.Set(RaceTicket.Column_victory, obj.Victory)
.Set(RaceTicket.Column_runner_num, obj.RunnerNum)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(RaceTicket.Column_item_obj_id, obj.ItemObjId)
.Where(RaceTicket.Column_round, obj.Round)
.Execute();


obj.ItemObjId = dataSourceRow.getString(RaceTicket.Column_item_obj_id);
obj.Round = dataSourceRow.getString(RaceTicket.Column_round);
obj.AllotmentPercentage = dataSourceRow.getString(RaceTicket.Column_allotment_percentage);
obj.Victory = dataSourceRow.getString(RaceTicket.Column_victory);
obj.RunnerNum = dataSourceRow.getString(RaceTicket.Column_runner_num);

