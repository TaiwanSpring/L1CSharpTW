private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.WeaponSkill);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(WeaponSkill.Column_weapon_id, obj.WeaponId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(WeaponSkill.Column_weapon_id, obj.WeaponId)
.Set(WeaponSkill.Column_note, obj.Note)
.Set(WeaponSkill.Column_probability, obj.Probability)
.Set(WeaponSkill.Column_fix_damage, obj.FixDamage)
.Set(WeaponSkill.Column_random_damage, obj.RandomDamage)
.Set(WeaponSkill.Column_area, obj.Area)
.Set(WeaponSkill.Column_skill_id, obj.SkillId)
.Set(WeaponSkill.Column_skill_time, obj.SkillTime)
.Set(WeaponSkill.Column_effect_id, obj.EffectId)
.Set(WeaponSkill.Column_effect_target, obj.EffectTarget)
.Set(WeaponSkill.Column_arrow_type, obj.ArrowType)
.Set(WeaponSkill.Column_attr, obj.Attr)
.Set(WeaponSkill.Column_gfx_id, obj.GfxId)
.Set(WeaponSkill.Column_gfx_id_target, obj.GfxIdTarget)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(WeaponSkill.Column_weapon_id, obj.WeaponId)
.Set(WeaponSkill.Column_note, obj.Note)
.Set(WeaponSkill.Column_probability, obj.Probability)
.Set(WeaponSkill.Column_fix_damage, obj.FixDamage)
.Set(WeaponSkill.Column_random_damage, obj.RandomDamage)
.Set(WeaponSkill.Column_area, obj.Area)
.Set(WeaponSkill.Column_skill_id, obj.SkillId)
.Set(WeaponSkill.Column_skill_time, obj.SkillTime)
.Set(WeaponSkill.Column_effect_id, obj.EffectId)
.Set(WeaponSkill.Column_effect_target, obj.EffectTarget)
.Set(WeaponSkill.Column_arrow_type, obj.ArrowType)
.Set(WeaponSkill.Column_attr, obj.Attr)
.Set(WeaponSkill.Column_gfx_id, obj.GfxId)
.Set(WeaponSkill.Column_gfx_id_target, obj.GfxIdTarget)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(WeaponSkill.Column_weapon_id, obj.WeaponId)
.Execute();


obj.WeaponId = dataSourceRow.getString(WeaponSkill.Column_weapon_id);
obj.Note = dataSourceRow.getString(WeaponSkill.Column_note);
obj.Probability = dataSourceRow.getString(WeaponSkill.Column_probability);
obj.FixDamage = dataSourceRow.getString(WeaponSkill.Column_fix_damage);
obj.RandomDamage = dataSourceRow.getString(WeaponSkill.Column_random_damage);
obj.Area = dataSourceRow.getString(WeaponSkill.Column_area);
obj.SkillId = dataSourceRow.getString(WeaponSkill.Column_skill_id);
obj.SkillTime = dataSourceRow.getString(WeaponSkill.Column_skill_time);
obj.EffectId = dataSourceRow.getString(WeaponSkill.Column_effect_id);
obj.EffectTarget = dataSourceRow.getString(WeaponSkill.Column_effect_target);
obj.ArrowType = dataSourceRow.getString(WeaponSkill.Column_arrow_type);
obj.Attr = dataSourceRow.getString(WeaponSkill.Column_attr);
obj.GfxId = dataSourceRow.getString(WeaponSkill.Column_gfx_id);
obj.GfxIdTarget = dataSourceRow.getString(WeaponSkill.Column_gfx_id_target);

