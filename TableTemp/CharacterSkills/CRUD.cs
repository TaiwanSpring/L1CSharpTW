private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterSkills);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(CharacterSkills.Column_char_obj_id, obj.CharObjId)
.Where(CharacterSkills.Column_skill_id, obj.SkillId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(CharacterSkills.Column_id, obj.Id)
.Set(CharacterSkills.Column_char_obj_id, obj.CharObjId)
.Set(CharacterSkills.Column_skill_id, obj.SkillId)
.Set(CharacterSkills.Column_skill_name, obj.SkillName)
.Set(CharacterSkills.Column_is_active, obj.IsActive)
.Set(CharacterSkills.Column_activetimeleft, obj.Activetimeleft)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(CharacterSkills.Column_id, obj.Id)
.Where(CharacterSkills.Column_char_obj_id, obj.CharObjId)
.Where(CharacterSkills.Column_skill_id, obj.SkillId)
.Set(CharacterSkills.Column_skill_name, obj.SkillName)
.Set(CharacterSkills.Column_is_active, obj.IsActive)
.Set(CharacterSkills.Column_activetimeleft, obj.Activetimeleft)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(CharacterSkills.Column_char_obj_id, obj.CharObjId)
.Where(CharacterSkills.Column_skill_id, obj.SkillId)
.Execute();


obj.Id = dataSourceRow.getString(CharacterSkills.Column_id);
obj.CharObjId = dataSourceRow.getString(CharacterSkills.Column_char_obj_id);
obj.SkillId = dataSourceRow.getString(CharacterSkills.Column_skill_id);
obj.SkillName = dataSourceRow.getString(CharacterSkills.Column_skill_name);
obj.IsActive = dataSourceRow.getString(CharacterSkills.Column_is_active);
obj.Activetimeleft = dataSourceRow.getString(CharacterSkills.Column_activetimeleft);

