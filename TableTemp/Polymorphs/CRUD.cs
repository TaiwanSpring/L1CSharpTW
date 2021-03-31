private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Polymorphs);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Polymorphs.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Polymorphs.Column_id, obj.Id)
.Set(Polymorphs.Column_name, obj.Name)
.Set(Polymorphs.Column_polyid, obj.Polyid)
.Set(Polymorphs.Column_minlevel, obj.Minlevel)
.Set(Polymorphs.Column_weaponequip, obj.Weaponequip)
.Set(Polymorphs.Column_armorequip, obj.Armorequip)
.Set(Polymorphs.Column_isSkillUse, obj.IsSkillUse)
.Set(Polymorphs.Column_cause, obj.Cause)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Polymorphs.Column_id, obj.Id)
.Set(Polymorphs.Column_name, obj.Name)
.Set(Polymorphs.Column_polyid, obj.Polyid)
.Set(Polymorphs.Column_minlevel, obj.Minlevel)
.Set(Polymorphs.Column_weaponequip, obj.Weaponequip)
.Set(Polymorphs.Column_armorequip, obj.Armorequip)
.Set(Polymorphs.Column_isSkillUse, obj.IsSkillUse)
.Set(Polymorphs.Column_cause, obj.Cause)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Polymorphs.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Polymorphs.Column_id);
obj.Name = dataSourceRow.getString(Polymorphs.Column_name);
obj.Polyid = dataSourceRow.getString(Polymorphs.Column_polyid);
obj.Minlevel = dataSourceRow.getString(Polymorphs.Column_minlevel);
obj.Weaponequip = dataSourceRow.getString(Polymorphs.Column_weaponequip);
obj.Armorequip = dataSourceRow.getString(Polymorphs.Column_armorequip);
obj.IsSkillUse = dataSourceRow.getString(Polymorphs.Column_isSkillUse);
obj.Cause = dataSourceRow.getString(Polymorphs.Column_cause);

