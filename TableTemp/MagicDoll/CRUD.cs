private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.MagicDoll);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(MagicDoll.Column_item_id, obj.ItemId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(MagicDoll.Column_item_id, obj.ItemId)
.Set(MagicDoll.Column_note, obj.Note)
.Set(MagicDoll.Column_doll_id, obj.DollId)
.Set(MagicDoll.Column_ac, obj.Ac)
.Set(MagicDoll.Column_hpr, obj.Hpr)
.Set(MagicDoll.Column_hpr_time, obj.HprTime)
.Set(MagicDoll.Column_mpr, obj.Mpr)
.Set(MagicDoll.Column_mpr_time, obj.MprTime)
.Set(MagicDoll.Column_hit, obj.Hit)
.Set(MagicDoll.Column_dmg, obj.Dmg)
.Set(MagicDoll.Column_dmg_chance, obj.DmgChance)
.Set(MagicDoll.Column_bow_hit, obj.BowHit)
.Set(MagicDoll.Column_bow_dmg, obj.BowDmg)
.Set(MagicDoll.Column_dmg_reduction, obj.DmgReduction)
.Set(MagicDoll.Column_dmg_reduction_chance, obj.DmgReductionChance)
.Set(MagicDoll.Column_dmg_evasion_chance, obj.DmgEvasionChance)
.Set(MagicDoll.Column_weight_reduction, obj.WeightReduction)
.Set(MagicDoll.Column_regist_stun, obj.RegistStun)
.Set(MagicDoll.Column_regist_stone, obj.RegistStone)
.Set(MagicDoll.Column_regist_sleep, obj.RegistSleep)
.Set(MagicDoll.Column_regist_freeze, obj.RegistFreeze)
.Set(MagicDoll.Column_regist_sustain, obj.RegistSustain)
.Set(MagicDoll.Column_regist_blind, obj.RegistBlind)
.Set(MagicDoll.Column_make_itemid, obj.MakeItemid)
.Set(MagicDoll.Column_effect, obj.Effect)
.Set(MagicDoll.Column_effect_chance, obj.EffectChance)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(MagicDoll.Column_item_id, obj.ItemId)
.Set(MagicDoll.Column_note, obj.Note)
.Set(MagicDoll.Column_doll_id, obj.DollId)
.Set(MagicDoll.Column_ac, obj.Ac)
.Set(MagicDoll.Column_hpr, obj.Hpr)
.Set(MagicDoll.Column_hpr_time, obj.HprTime)
.Set(MagicDoll.Column_mpr, obj.Mpr)
.Set(MagicDoll.Column_mpr_time, obj.MprTime)
.Set(MagicDoll.Column_hit, obj.Hit)
.Set(MagicDoll.Column_dmg, obj.Dmg)
.Set(MagicDoll.Column_dmg_chance, obj.DmgChance)
.Set(MagicDoll.Column_bow_hit, obj.BowHit)
.Set(MagicDoll.Column_bow_dmg, obj.BowDmg)
.Set(MagicDoll.Column_dmg_reduction, obj.DmgReduction)
.Set(MagicDoll.Column_dmg_reduction_chance, obj.DmgReductionChance)
.Set(MagicDoll.Column_dmg_evasion_chance, obj.DmgEvasionChance)
.Set(MagicDoll.Column_weight_reduction, obj.WeightReduction)
.Set(MagicDoll.Column_regist_stun, obj.RegistStun)
.Set(MagicDoll.Column_regist_stone, obj.RegistStone)
.Set(MagicDoll.Column_regist_sleep, obj.RegistSleep)
.Set(MagicDoll.Column_regist_freeze, obj.RegistFreeze)
.Set(MagicDoll.Column_regist_sustain, obj.RegistSustain)
.Set(MagicDoll.Column_regist_blind, obj.RegistBlind)
.Set(MagicDoll.Column_make_itemid, obj.MakeItemid)
.Set(MagicDoll.Column_effect, obj.Effect)
.Set(MagicDoll.Column_effect_chance, obj.EffectChance)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(MagicDoll.Column_item_id, obj.ItemId)
.Execute();


obj.ItemId = dataSourceRow.getString(MagicDoll.Column_item_id);
obj.Note = dataSourceRow.getString(MagicDoll.Column_note);
obj.DollId = dataSourceRow.getString(MagicDoll.Column_doll_id);
obj.Ac = dataSourceRow.getString(MagicDoll.Column_ac);
obj.Hpr = dataSourceRow.getString(MagicDoll.Column_hpr);
obj.HprTime = dataSourceRow.getString(MagicDoll.Column_hpr_time);
obj.Mpr = dataSourceRow.getString(MagicDoll.Column_mpr);
obj.MprTime = dataSourceRow.getString(MagicDoll.Column_mpr_time);
obj.Hit = dataSourceRow.getString(MagicDoll.Column_hit);
obj.Dmg = dataSourceRow.getString(MagicDoll.Column_dmg);
obj.DmgChance = dataSourceRow.getString(MagicDoll.Column_dmg_chance);
obj.BowHit = dataSourceRow.getString(MagicDoll.Column_bow_hit);
obj.BowDmg = dataSourceRow.getString(MagicDoll.Column_bow_dmg);
obj.DmgReduction = dataSourceRow.getString(MagicDoll.Column_dmg_reduction);
obj.DmgReductionChance = dataSourceRow.getString(MagicDoll.Column_dmg_reduction_chance);
obj.DmgEvasionChance = dataSourceRow.getString(MagicDoll.Column_dmg_evasion_chance);
obj.WeightReduction = dataSourceRow.getString(MagicDoll.Column_weight_reduction);
obj.RegistStun = dataSourceRow.getString(MagicDoll.Column_regist_stun);
obj.RegistStone = dataSourceRow.getString(MagicDoll.Column_regist_stone);
obj.RegistSleep = dataSourceRow.getString(MagicDoll.Column_regist_sleep);
obj.RegistFreeze = dataSourceRow.getString(MagicDoll.Column_regist_freeze);
obj.RegistSustain = dataSourceRow.getString(MagicDoll.Column_regist_sustain);
obj.RegistBlind = dataSourceRow.getString(MagicDoll.Column_regist_blind);
obj.MakeItemid = dataSourceRow.getString(MagicDoll.Column_make_itemid);
obj.Effect = dataSourceRow.getString(MagicDoll.Column_effect);
obj.EffectChance = dataSourceRow.getString(MagicDoll.Column_effect_chance);

