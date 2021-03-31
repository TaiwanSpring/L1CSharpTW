private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Mobskill);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Mobskill.Column_mobid, obj.Mobid)
.Where(Mobskill.Column_actNo, obj.ActNo)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Mobskill.Column_mobid, obj.Mobid)
.Set(Mobskill.Column_actNo, obj.ActNo)
.Set(Mobskill.Column_mobname, obj.Mobname)
.Set(Mobskill.Column_Type, obj.Type)
.Set(Mobskill.Column_mpConsume, obj.MpConsume)
.Set(Mobskill.Column_TriRnd, obj.TriRnd)
.Set(Mobskill.Column_TriHp, obj.TriHp)
.Set(Mobskill.Column_TriCompanionHp, obj.TriCompanionHp)
.Set(Mobskill.Column_TriRange, obj.TriRange)
.Set(Mobskill.Column_TriCount, obj.TriCount)
.Set(Mobskill.Column_ChangeTarget, obj.ChangeTarget)
.Set(Mobskill.Column_Range, obj.Range)
.Set(Mobskill.Column_AreaWidth, obj.AreaWidth)
.Set(Mobskill.Column_AreaHeight, obj.AreaHeight)
.Set(Mobskill.Column_Leverage, obj.Leverage)
.Set(Mobskill.Column_SkillId, obj.SkillId)
.Set(Mobskill.Column_SkillArea, obj.SkillArea)
.Set(Mobskill.Column_Gfxid, obj.Gfxid)
.Set(Mobskill.Column_ActId, obj.ActId)
.Set(Mobskill.Column_SummonId, obj.SummonId)
.Set(Mobskill.Column_SummonMin, obj.SummonMin)
.Set(Mobskill.Column_SummonMax, obj.SummonMax)
.Set(Mobskill.Column_PolyId, obj.PolyId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Mobskill.Column_mobid, obj.Mobid)
.Where(Mobskill.Column_actNo, obj.ActNo)
.Set(Mobskill.Column_mobname, obj.Mobname)
.Set(Mobskill.Column_Type, obj.Type)
.Set(Mobskill.Column_mpConsume, obj.MpConsume)
.Set(Mobskill.Column_TriRnd, obj.TriRnd)
.Set(Mobskill.Column_TriHp, obj.TriHp)
.Set(Mobskill.Column_TriCompanionHp, obj.TriCompanionHp)
.Set(Mobskill.Column_TriRange, obj.TriRange)
.Set(Mobskill.Column_TriCount, obj.TriCount)
.Set(Mobskill.Column_ChangeTarget, obj.ChangeTarget)
.Set(Mobskill.Column_Range, obj.Range)
.Set(Mobskill.Column_AreaWidth, obj.AreaWidth)
.Set(Mobskill.Column_AreaHeight, obj.AreaHeight)
.Set(Mobskill.Column_Leverage, obj.Leverage)
.Set(Mobskill.Column_SkillId, obj.SkillId)
.Set(Mobskill.Column_SkillArea, obj.SkillArea)
.Set(Mobskill.Column_Gfxid, obj.Gfxid)
.Set(Mobskill.Column_ActId, obj.ActId)
.Set(Mobskill.Column_SummonId, obj.SummonId)
.Set(Mobskill.Column_SummonMin, obj.SummonMin)
.Set(Mobskill.Column_SummonMax, obj.SummonMax)
.Set(Mobskill.Column_PolyId, obj.PolyId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Mobskill.Column_mobid, obj.Mobid)
.Where(Mobskill.Column_actNo, obj.ActNo)
.Execute();


obj.Mobid = dataSourceRow.getString(Mobskill.Column_mobid);
obj.ActNo = dataSourceRow.getString(Mobskill.Column_actNo);
obj.Mobname = dataSourceRow.getString(Mobskill.Column_mobname);
obj.Type = dataSourceRow.getString(Mobskill.Column_Type);
obj.MpConsume = dataSourceRow.getString(Mobskill.Column_mpConsume);
obj.TriRnd = dataSourceRow.getString(Mobskill.Column_TriRnd);
obj.TriHp = dataSourceRow.getString(Mobskill.Column_TriHp);
obj.TriCompanionHp = dataSourceRow.getString(Mobskill.Column_TriCompanionHp);
obj.TriRange = dataSourceRow.getString(Mobskill.Column_TriRange);
obj.TriCount = dataSourceRow.getString(Mobskill.Column_TriCount);
obj.ChangeTarget = dataSourceRow.getString(Mobskill.Column_ChangeTarget);
obj.Range = dataSourceRow.getString(Mobskill.Column_Range);
obj.AreaWidth = dataSourceRow.getString(Mobskill.Column_AreaWidth);
obj.AreaHeight = dataSourceRow.getString(Mobskill.Column_AreaHeight);
obj.Leverage = dataSourceRow.getString(Mobskill.Column_Leverage);
obj.SkillId = dataSourceRow.getString(Mobskill.Column_SkillId);
obj.SkillArea = dataSourceRow.getString(Mobskill.Column_SkillArea);
obj.Gfxid = dataSourceRow.getString(Mobskill.Column_Gfxid);
obj.ActId = dataSourceRow.getString(Mobskill.Column_ActId);
obj.SummonId = dataSourceRow.getString(Mobskill.Column_SummonId);
obj.SummonMin = dataSourceRow.getString(Mobskill.Column_SummonMin);
obj.SummonMax = dataSourceRow.getString(Mobskill.Column_SummonMax);
obj.PolyId = dataSourceRow.getString(Mobskill.Column_PolyId);

