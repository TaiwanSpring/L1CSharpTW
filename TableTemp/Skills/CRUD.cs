private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Skills);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Skills.Column_skill_id, obj.SkillId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Skills.Column_skill_id, obj.SkillId)
.Set(Skills.Column_name, obj.Name)
.Set(Skills.Column_skill_level, obj.SkillLevel)
.Set(Skills.Column_skill_number, obj.SkillNumber)
.Set(Skills.Column_mpConsume, obj.MpConsume)
.Set(Skills.Column_hpConsume, obj.HpConsume)
.Set(Skills.Column_itemConsumeId, obj.ItemConsumeId)
.Set(Skills.Column_itemConsumeCount, obj.ItemConsumeCount)
.Set(Skills.Column_reuseDelay, obj.ReuseDelay)
.Set(Skills.Column_buffDuration, obj.BuffDuration)
.Set(Skills.Column_target, obj.Target)
.Set(Skills.Column_target_to, obj.TargetTo)
.Set(Skills.Column_damage_value, obj.DamageValue)
.Set(Skills.Column_damage_dice, obj.DamageDice)
.Set(Skills.Column_damage_dice_count, obj.DamageDiceCount)
.Set(Skills.Column_probability_value, obj.ProbabilityValue)
.Set(Skills.Column_probability_dice, obj.ProbabilityDice)
.Set(Skills.Column_attr, obj.Attr)
.Set(Skills.Column_type, obj.Type)
.Set(Skills.Column_lawful, obj.Lawful)
.Set(Skills.Column_ranged, obj.Ranged)
.Set(Skills.Column_area, obj.Area)
.Set(Skills.Column_through, obj.Through)
.Set(Skills.Column_id, obj.Id)
.Set(Skills.Column_nameid, obj.Nameid)
.Set(Skills.Column_action_id, obj.ActionId)
.Set(Skills.Column_castgfx, obj.Castgfx)
.Set(Skills.Column_castgfx2, obj.Castgfx2)
.Set(Skills.Column_sysmsgID_happen, obj.SysmsgIDHappen)
.Set(Skills.Column_sysmsgID_stop, obj.SysmsgIDStop)
.Set(Skills.Column_sysmsgID_fail, obj.SysmsgIDFail)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Skills.Column_skill_id, obj.SkillId)
.Set(Skills.Column_name, obj.Name)
.Set(Skills.Column_skill_level, obj.SkillLevel)
.Set(Skills.Column_skill_number, obj.SkillNumber)
.Set(Skills.Column_mpConsume, obj.MpConsume)
.Set(Skills.Column_hpConsume, obj.HpConsume)
.Set(Skills.Column_itemConsumeId, obj.ItemConsumeId)
.Set(Skills.Column_itemConsumeCount, obj.ItemConsumeCount)
.Set(Skills.Column_reuseDelay, obj.ReuseDelay)
.Set(Skills.Column_buffDuration, obj.BuffDuration)
.Set(Skills.Column_target, obj.Target)
.Set(Skills.Column_target_to, obj.TargetTo)
.Set(Skills.Column_damage_value, obj.DamageValue)
.Set(Skills.Column_damage_dice, obj.DamageDice)
.Set(Skills.Column_damage_dice_count, obj.DamageDiceCount)
.Set(Skills.Column_probability_value, obj.ProbabilityValue)
.Set(Skills.Column_probability_dice, obj.ProbabilityDice)
.Set(Skills.Column_attr, obj.Attr)
.Set(Skills.Column_type, obj.Type)
.Set(Skills.Column_lawful, obj.Lawful)
.Set(Skills.Column_ranged, obj.Ranged)
.Set(Skills.Column_area, obj.Area)
.Set(Skills.Column_through, obj.Through)
.Set(Skills.Column_id, obj.Id)
.Set(Skills.Column_nameid, obj.Nameid)
.Set(Skills.Column_action_id, obj.ActionId)
.Set(Skills.Column_castgfx, obj.Castgfx)
.Set(Skills.Column_castgfx2, obj.Castgfx2)
.Set(Skills.Column_sysmsgID_happen, obj.SysmsgIDHappen)
.Set(Skills.Column_sysmsgID_stop, obj.SysmsgIDStop)
.Set(Skills.Column_sysmsgID_fail, obj.SysmsgIDFail)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Skills.Column_skill_id, obj.SkillId)
.Execute();


obj.SkillId = dataSourceRow.getString(Skills.Column_skill_id);
obj.Name = dataSourceRow.getString(Skills.Column_name);
obj.SkillLevel = dataSourceRow.getString(Skills.Column_skill_level);
obj.SkillNumber = dataSourceRow.getString(Skills.Column_skill_number);
obj.MpConsume = dataSourceRow.getString(Skills.Column_mpConsume);
obj.HpConsume = dataSourceRow.getString(Skills.Column_hpConsume);
obj.ItemConsumeId = dataSourceRow.getString(Skills.Column_itemConsumeId);
obj.ItemConsumeCount = dataSourceRow.getString(Skills.Column_itemConsumeCount);
obj.ReuseDelay = dataSourceRow.getString(Skills.Column_reuseDelay);
obj.BuffDuration = dataSourceRow.getString(Skills.Column_buffDuration);
obj.Target = dataSourceRow.getString(Skills.Column_target);
obj.TargetTo = dataSourceRow.getString(Skills.Column_target_to);
obj.DamageValue = dataSourceRow.getString(Skills.Column_damage_value);
obj.DamageDice = dataSourceRow.getString(Skills.Column_damage_dice);
obj.DamageDiceCount = dataSourceRow.getString(Skills.Column_damage_dice_count);
obj.ProbabilityValue = dataSourceRow.getString(Skills.Column_probability_value);
obj.ProbabilityDice = dataSourceRow.getString(Skills.Column_probability_dice);
obj.Attr = dataSourceRow.getString(Skills.Column_attr);
obj.Type = dataSourceRow.getString(Skills.Column_type);
obj.Lawful = dataSourceRow.getString(Skills.Column_lawful);
obj.Ranged = dataSourceRow.getString(Skills.Column_ranged);
obj.Area = dataSourceRow.getString(Skills.Column_area);
obj.Through = dataSourceRow.getString(Skills.Column_through);
obj.Id = dataSourceRow.getString(Skills.Column_id);
obj.Nameid = dataSourceRow.getString(Skills.Column_nameid);
obj.ActionId = dataSourceRow.getString(Skills.Column_action_id);
obj.Castgfx = dataSourceRow.getString(Skills.Column_castgfx);
obj.Castgfx2 = dataSourceRow.getString(Skills.Column_castgfx2);
obj.SysmsgIDHappen = dataSourceRow.getString(Skills.Column_sysmsgID_happen);
obj.SysmsgIDStop = dataSourceRow.getString(Skills.Column_sysmsgID_stop);
obj.SysmsgIDFail = dataSourceRow.getString(Skills.Column_sysmsgID_fail);

