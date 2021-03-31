private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterBuff);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterBuff.Column_char_obj_id, obj.CharObjId)
.Where(CharacterBuff.Column_skill_id, obj.SkillId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterBuff.Column_char_obj_id, obj.CharObjId)
.Set(CharacterBuff.Column_skill_id, obj.SkillId)
.Set(CharacterBuff.Column_remaining_time, obj.RemainingTime)
.Set(CharacterBuff.Column_poly_id, obj.PolyId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(CharacterBuff.Column_char_obj_id, obj.CharObjId)
.Where(CharacterBuff.Column_skill_id, obj.SkillId)
.Set(CharacterBuff.Column_remaining_time, obj.RemainingTime)
.Set(CharacterBuff.Column_poly_id, obj.PolyId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterBuff.Column_char_obj_id, obj.CharObjId)
.Where(CharacterBuff.Column_skill_id, obj.SkillId)
.Execute();


obj.CharObjId = dataSourceRow.getString(CharacterBuff.Column_char_obj_id);
obj.SkillId = dataSourceRow.getString(CharacterBuff.Column_skill_id);
obj.RemainingTime = dataSourceRow.getString(CharacterBuff.Column_remaining_time);
obj.PolyId = dataSourceRow.getString(CharacterBuff.Column_poly_id);

