private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Trap);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Trap.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Trap.Column_id, obj.Id)
.Set(Trap.Column_note, obj.Note)
.Set(Trap.Column_type, obj.Type)
.Set(Trap.Column_gfxId, obj.GfxId)
.Set(Trap.Column_isDetectionable, obj.IsDetectionable)
.Set(Trap.Column_base, obj.Base)
.Set(Trap.Column_dice, obj.Dice)
.Set(Trap.Column_diceCount, obj.DiceCount)
.Set(Trap.Column_poisonType, obj.PoisonType)
.Set(Trap.Column_poisonDelay, obj.PoisonDelay)
.Set(Trap.Column_poisonTime, obj.PoisonTime)
.Set(Trap.Column_poisonDamage, obj.PoisonDamage)
.Set(Trap.Column_monsterNpcId, obj.MonsterNpcId)
.Set(Trap.Column_monsterCount, obj.MonsterCount)
.Set(Trap.Column_teleportX, obj.TeleportX)
.Set(Trap.Column_teleportY, obj.TeleportY)
.Set(Trap.Column_teleportMapId, obj.TeleportMapId)
.Set(Trap.Column_skillId, obj.SkillId)
.Set(Trap.Column_skillTimeSeconds, obj.SkillTimeSeconds)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Trap.Column_id, obj.Id)
.Set(Trap.Column_note, obj.Note)
.Set(Trap.Column_type, obj.Type)
.Set(Trap.Column_gfxId, obj.GfxId)
.Set(Trap.Column_isDetectionable, obj.IsDetectionable)
.Set(Trap.Column_base, obj.Base)
.Set(Trap.Column_dice, obj.Dice)
.Set(Trap.Column_diceCount, obj.DiceCount)
.Set(Trap.Column_poisonType, obj.PoisonType)
.Set(Trap.Column_poisonDelay, obj.PoisonDelay)
.Set(Trap.Column_poisonTime, obj.PoisonTime)
.Set(Trap.Column_poisonDamage, obj.PoisonDamage)
.Set(Trap.Column_monsterNpcId, obj.MonsterNpcId)
.Set(Trap.Column_monsterCount, obj.MonsterCount)
.Set(Trap.Column_teleportX, obj.TeleportX)
.Set(Trap.Column_teleportY, obj.TeleportY)
.Set(Trap.Column_teleportMapId, obj.TeleportMapId)
.Set(Trap.Column_skillId, obj.SkillId)
.Set(Trap.Column_skillTimeSeconds, obj.SkillTimeSeconds)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Trap.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Trap.Column_id);
obj.Note = dataSourceRow.getString(Trap.Column_note);
obj.Type = dataSourceRow.getString(Trap.Column_type);
obj.GfxId = dataSourceRow.getString(Trap.Column_gfxId);
obj.IsDetectionable = dataSourceRow.getString(Trap.Column_isDetectionable);
obj.Base = dataSourceRow.getString(Trap.Column_base);
obj.Dice = dataSourceRow.getString(Trap.Column_dice);
obj.DiceCount = dataSourceRow.getString(Trap.Column_diceCount);
obj.PoisonType = dataSourceRow.getString(Trap.Column_poisonType);
obj.PoisonDelay = dataSourceRow.getString(Trap.Column_poisonDelay);
obj.PoisonTime = dataSourceRow.getString(Trap.Column_poisonTime);
obj.PoisonDamage = dataSourceRow.getString(Trap.Column_poisonDamage);
obj.MonsterNpcId = dataSourceRow.getString(Trap.Column_monsterNpcId);
obj.MonsterCount = dataSourceRow.getString(Trap.Column_monsterCount);
obj.TeleportX = dataSourceRow.getString(Trap.Column_teleportX);
obj.TeleportY = dataSourceRow.getString(Trap.Column_teleportY);
obj.TeleportMapId = dataSourceRow.getString(Trap.Column_teleportMapId);
obj.SkillId = dataSourceRow.getString(Trap.Column_skillId);
obj.SkillTimeSeconds = dataSourceRow.getString(Trap.Column_skillTimeSeconds);

