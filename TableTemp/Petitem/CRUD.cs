private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Petitem);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Petitem.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Petitem.Column_item_id, obj.ItemId)
.Set(Petitem.Column_note, obj.Note)
.Set(Petitem.Column_use_type, obj.UseType)
.Set(Petitem.Column_hitmodifier, obj.Hitmodifier)
.Set(Petitem.Column_dmgmodifier, obj.Dmgmodifier)
.Set(Petitem.Column_ac, obj.Ac)
.Set(Petitem.Column_add_str, obj.AddStr)
.Set(Petitem.Column_add_con, obj.AddCon)
.Set(Petitem.Column_add_dex, obj.AddDex)
.Set(Petitem.Column_add_int, obj.AddInt)
.Set(Petitem.Column_add_wis, obj.AddWis)
.Set(Petitem.Column_add_hp, obj.AddHp)
.Set(Petitem.Column_add_mp, obj.AddMp)
.Set(Petitem.Column_add_sp, obj.AddSp)
.Set(Petitem.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Petitem.Column_item_id, obj.ItemId)
.Set(Petitem.Column_note, obj.Note)
.Set(Petitem.Column_use_type, obj.UseType)
.Set(Petitem.Column_hitmodifier, obj.Hitmodifier)
.Set(Petitem.Column_dmgmodifier, obj.Dmgmodifier)
.Set(Petitem.Column_ac, obj.Ac)
.Set(Petitem.Column_add_str, obj.AddStr)
.Set(Petitem.Column_add_con, obj.AddCon)
.Set(Petitem.Column_add_dex, obj.AddDex)
.Set(Petitem.Column_add_int, obj.AddInt)
.Set(Petitem.Column_add_wis, obj.AddWis)
.Set(Petitem.Column_add_hp, obj.AddHp)
.Set(Petitem.Column_add_mp, obj.AddMp)
.Set(Petitem.Column_add_sp, obj.AddSp)
.Set(Petitem.Column_m_def, obj.MDef)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Petitem.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(Petitem.Column_item_id);
obj.Note = dataSourceRow.getString(Petitem.Column_note);
obj.UseType = dataSourceRow.getString(Petitem.Column_use_type);
obj.Hitmodifier = dataSourceRow.getString(Petitem.Column_hitmodifier);
obj.Dmgmodifier = dataSourceRow.getString(Petitem.Column_dmgmodifier);
obj.Ac = dataSourceRow.getString(Petitem.Column_ac);
obj.AddStr = dataSourceRow.getString(Petitem.Column_add_str);
obj.AddCon = dataSourceRow.getString(Petitem.Column_add_con);
obj.AddDex = dataSourceRow.getString(Petitem.Column_add_dex);
obj.AddInt = dataSourceRow.getString(Petitem.Column_add_int);
obj.AddWis = dataSourceRow.getString(Petitem.Column_add_wis);
obj.AddHp = dataSourceRow.getString(Petitem.Column_add_hp);
obj.AddMp = dataSourceRow.getString(Petitem.Column_add_mp);
obj.AddSp = dataSourceRow.getString(Petitem.Column_add_sp);
obj.MDef = dataSourceRow.getString(Petitem.Column_m_def);

