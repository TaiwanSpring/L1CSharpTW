private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Pets);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Pets.Column_item_obj_id, obj.ItemObjId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Pets.Column_item_obj_id, obj.ItemObjId)
.Set(Pets.Column_objid, obj.Objid)
.Set(Pets.Column_npcid, obj.Npcid)
.Set(Pets.Column_name, obj.Name)
.Set(Pets.Column_lvl, obj.Lvl)
.Set(Pets.Column_hp, obj.Hp)
.Set(Pets.Column_mp, obj.Mp)
.Set(Pets.Column_exp, obj.Exp)
.Set(Pets.Column_lawful, obj.Lawful)
.Set(Pets.Column_food, obj.Food)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Pets.Column_item_obj_id, obj.ItemObjId)
.Set(Pets.Column_objid, obj.Objid)
.Set(Pets.Column_npcid, obj.Npcid)
.Set(Pets.Column_name, obj.Name)
.Set(Pets.Column_lvl, obj.Lvl)
.Set(Pets.Column_hp, obj.Hp)
.Set(Pets.Column_mp, obj.Mp)
.Set(Pets.Column_exp, obj.Exp)
.Set(Pets.Column_lawful, obj.Lawful)
.Set(Pets.Column_food, obj.Food)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Pets.Column_item_obj_id, obj.ItemObjId)
.Execute();


obj.ItemObjId = dataSourceRow.getString(Pets.Column_item_obj_id);
obj.Objid = dataSourceRow.getString(Pets.Column_objid);
obj.Npcid = dataSourceRow.getString(Pets.Column_npcid);
obj.Name = dataSourceRow.getString(Pets.Column_name);
obj.Lvl = dataSourceRow.getString(Pets.Column_lvl);
obj.Hp = dataSourceRow.getString(Pets.Column_hp);
obj.Mp = dataSourceRow.getString(Pets.Column_mp);
obj.Exp = dataSourceRow.getString(Pets.Column_exp);
obj.Lawful = dataSourceRow.getString(Pets.Column_lawful);
obj.Food = dataSourceRow.getString(Pets.Column_food);

