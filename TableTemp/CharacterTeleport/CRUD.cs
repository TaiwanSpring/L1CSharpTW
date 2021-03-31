private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterTeleport);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterTeleport.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterTeleport.Column_id, obj.Id)
.Set(CharacterTeleport.Column_char_id, obj.CharId)
.Set(CharacterTeleport.Column_name, obj.Name)
.Set(CharacterTeleport.Column_locx, obj.Locx)
.Set(CharacterTeleport.Column_locy, obj.Locy)
.Set(CharacterTeleport.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterTeleport.Column_id, obj.Id)
.Set(CharacterTeleport.Column_char_id, obj.CharId)
.Set(CharacterTeleport.Column_name, obj.Name)
.Set(CharacterTeleport.Column_locx, obj.Locx)
.Set(CharacterTeleport.Column_locy, obj.Locy)
.Set(CharacterTeleport.Column_mapid, obj.Mapid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterTeleport.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(CharacterTeleport.Column_id);
obj.CharId = dataSourceRow.getString(CharacterTeleport.Column_char_id);
obj.Name = dataSourceRow.getString(CharacterTeleport.Column_name);
obj.Locx = dataSourceRow.getString(CharacterTeleport.Column_locx);
obj.Locy = dataSourceRow.getString(CharacterTeleport.Column_locy);
obj.Mapid = dataSourceRow.getString(CharacterTeleport.Column_mapid);

