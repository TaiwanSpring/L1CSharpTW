private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Mapids);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Mapids.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Mapids.Column_mapid, obj.Mapid)
.Set(Mapids.Column_locationname, obj.Locationname)
.Set(Mapids.Column_startX, obj.StartX)
.Set(Mapids.Column_endX, obj.EndX)
.Set(Mapids.Column_startY, obj.StartY)
.Set(Mapids.Column_endY, obj.EndY)
.Set(Mapids.Column_monster_amount, obj.MonsterAmount)
.Set(Mapids.Column_drop_rate, obj.DropRate)
.Set(Mapids.Column_underwater, obj.Underwater)
.Set(Mapids.Column_markable, obj.Markable)
.Set(Mapids.Column_teleportable, obj.Teleportable)
.Set(Mapids.Column_escapable, obj.Escapable)
.Set(Mapids.Column_resurrection, obj.Resurrection)
.Set(Mapids.Column_painwand, obj.Painwand)
.Set(Mapids.Column_penalty, obj.Penalty)
.Set(Mapids.Column_take_pets, obj.TakePets)
.Set(Mapids.Column_recall_pets, obj.RecallPets)
.Set(Mapids.Column_usable_item, obj.UsableItem)
.Set(Mapids.Column_usable_skill, obj.UsableSkill)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Mapids.Column_mapid, obj.Mapid)
.Set(Mapids.Column_locationname, obj.Locationname)
.Set(Mapids.Column_startX, obj.StartX)
.Set(Mapids.Column_endX, obj.EndX)
.Set(Mapids.Column_startY, obj.StartY)
.Set(Mapids.Column_endY, obj.EndY)
.Set(Mapids.Column_monster_amount, obj.MonsterAmount)
.Set(Mapids.Column_drop_rate, obj.DropRate)
.Set(Mapids.Column_underwater, obj.Underwater)
.Set(Mapids.Column_markable, obj.Markable)
.Set(Mapids.Column_teleportable, obj.Teleportable)
.Set(Mapids.Column_escapable, obj.Escapable)
.Set(Mapids.Column_resurrection, obj.Resurrection)
.Set(Mapids.Column_painwand, obj.Painwand)
.Set(Mapids.Column_penalty, obj.Penalty)
.Set(Mapids.Column_take_pets, obj.TakePets)
.Set(Mapids.Column_recall_pets, obj.RecallPets)
.Set(Mapids.Column_usable_item, obj.UsableItem)
.Set(Mapids.Column_usable_skill, obj.UsableSkill)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Mapids.Column_mapid, obj.Mapid)
.Execute();


obj.Mapid = dataSourceRow.getString(Mapids.Column_mapid);
obj.Locationname = dataSourceRow.getString(Mapids.Column_locationname);
obj.StartX = dataSourceRow.getString(Mapids.Column_startX);
obj.EndX = dataSourceRow.getString(Mapids.Column_endX);
obj.StartY = dataSourceRow.getString(Mapids.Column_startY);
obj.EndY = dataSourceRow.getString(Mapids.Column_endY);
obj.MonsterAmount = dataSourceRow.getString(Mapids.Column_monster_amount);
obj.DropRate = dataSourceRow.getString(Mapids.Column_drop_rate);
obj.Underwater = dataSourceRow.getString(Mapids.Column_underwater);
obj.Markable = dataSourceRow.getString(Mapids.Column_markable);
obj.Teleportable = dataSourceRow.getString(Mapids.Column_teleportable);
obj.Escapable = dataSourceRow.getString(Mapids.Column_escapable);
obj.Resurrection = dataSourceRow.getString(Mapids.Column_resurrection);
obj.Painwand = dataSourceRow.getString(Mapids.Column_painwand);
obj.Penalty = dataSourceRow.getString(Mapids.Column_penalty);
obj.TakePets = dataSourceRow.getString(Mapids.Column_take_pets);
obj.RecallPets = dataSourceRow.getString(Mapids.Column_recall_pets);
obj.UsableItem = dataSourceRow.getString(Mapids.Column_usable_item);
obj.UsableSkill = dataSourceRow.getString(Mapids.Column_usable_skill);

