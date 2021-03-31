private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Pettypes);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Pettypes.Column_BaseNpcId, obj.BaseNpcId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Pettypes.Column_BaseNpcId, obj.BaseNpcId)
.Set(Pettypes.Column_Name, obj.Name)
.Set(Pettypes.Column_ItemIdForTaming, obj.ItemIdForTaming)
.Set(Pettypes.Column_HpUpMin, obj.HpUpMin)
.Set(Pettypes.Column_HpUpMax, obj.HpUpMax)
.Set(Pettypes.Column_MpUpMin, obj.MpUpMin)
.Set(Pettypes.Column_MpUpMax, obj.MpUpMax)
.Set(Pettypes.Column_EvolvItemId, obj.EvolvItemId)
.Set(Pettypes.Column_NpcIdForEvolving, obj.NpcIdForEvolving)
.Set(Pettypes.Column_MessageId1, obj.MessageId1)
.Set(Pettypes.Column_MessageId2, obj.MessageId2)
.Set(Pettypes.Column_MessageId3, obj.MessageId3)
.Set(Pettypes.Column_MessageId4, obj.MessageId4)
.Set(Pettypes.Column_MessageId5, obj.MessageId5)
.Set(Pettypes.Column_DefyMessageId, obj.DefyMessageId)
.Set(Pettypes.Column_canUseEquipment, obj.CanUseEquipment)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Pettypes.Column_BaseNpcId, obj.BaseNpcId)
.Set(Pettypes.Column_Name, obj.Name)
.Set(Pettypes.Column_ItemIdForTaming, obj.ItemIdForTaming)
.Set(Pettypes.Column_HpUpMin, obj.HpUpMin)
.Set(Pettypes.Column_HpUpMax, obj.HpUpMax)
.Set(Pettypes.Column_MpUpMin, obj.MpUpMin)
.Set(Pettypes.Column_MpUpMax, obj.MpUpMax)
.Set(Pettypes.Column_EvolvItemId, obj.EvolvItemId)
.Set(Pettypes.Column_NpcIdForEvolving, obj.NpcIdForEvolving)
.Set(Pettypes.Column_MessageId1, obj.MessageId1)
.Set(Pettypes.Column_MessageId2, obj.MessageId2)
.Set(Pettypes.Column_MessageId3, obj.MessageId3)
.Set(Pettypes.Column_MessageId4, obj.MessageId4)
.Set(Pettypes.Column_MessageId5, obj.MessageId5)
.Set(Pettypes.Column_DefyMessageId, obj.DefyMessageId)
.Set(Pettypes.Column_canUseEquipment, obj.CanUseEquipment)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Pettypes.Column_BaseNpcId, obj.BaseNpcId)
.Execute();


obj.BaseNpcId = dataSourceRow.getString(Pettypes.Column_BaseNpcId);
obj.Name = dataSourceRow.getString(Pettypes.Column_Name);
obj.ItemIdForTaming = dataSourceRow.getString(Pettypes.Column_ItemIdForTaming);
obj.HpUpMin = dataSourceRow.getString(Pettypes.Column_HpUpMin);
obj.HpUpMax = dataSourceRow.getString(Pettypes.Column_HpUpMax);
obj.MpUpMin = dataSourceRow.getString(Pettypes.Column_MpUpMin);
obj.MpUpMax = dataSourceRow.getString(Pettypes.Column_MpUpMax);
obj.EvolvItemId = dataSourceRow.getString(Pettypes.Column_EvolvItemId);
obj.NpcIdForEvolving = dataSourceRow.getString(Pettypes.Column_NpcIdForEvolving);
obj.MessageId1 = dataSourceRow.getString(Pettypes.Column_MessageId1);
obj.MessageId2 = dataSourceRow.getString(Pettypes.Column_MessageId2);
obj.MessageId3 = dataSourceRow.getString(Pettypes.Column_MessageId3);
obj.MessageId4 = dataSourceRow.getString(Pettypes.Column_MessageId4);
obj.MessageId5 = dataSourceRow.getString(Pettypes.Column_MessageId5);
obj.DefyMessageId = dataSourceRow.getString(Pettypes.Column_DefyMessageId);
obj.CanUseEquipment = dataSourceRow.getString(Pettypes.Column_canUseEquipment);

