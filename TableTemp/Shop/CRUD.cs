private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Shop);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Shop.Column_npc_id, obj.NpcId)
.Where(Shop.Column_item_id, obj.ItemId)
.Where(Shop.Column_order_id, obj.OrderId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Shop.Column_npc_id, obj.NpcId)
.Set(Shop.Column_item_id, obj.ItemId)
.Set(Shop.Column_order_id, obj.OrderId)
.Set(Shop.Column_selling_price, obj.SellingPrice)
.Set(Shop.Column_pack_count, obj.PackCount)
.Set(Shop.Column_purchasing_price, obj.PurchasingPrice)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Shop.Column_npc_id, obj.NpcId)
.Where(Shop.Column_item_id, obj.ItemId)
.Where(Shop.Column_order_id, obj.OrderId)
.Set(Shop.Column_selling_price, obj.SellingPrice)
.Set(Shop.Column_pack_count, obj.PackCount)
.Set(Shop.Column_purchasing_price, obj.PurchasingPrice)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Shop.Column_npc_id, obj.NpcId)
.Where(Shop.Column_item_id, obj.ItemId)
.Where(Shop.Column_order_id, obj.OrderId)
.Execute();


obj.NpcId = dataSourceRow.getString(Shop.Column_npc_id);
obj.ItemId = dataSourceRow.getString(Shop.Column_item_id);
obj.OrderId = dataSourceRow.getString(Shop.Column_order_id);
obj.SellingPrice = dataSourceRow.getString(Shop.Column_selling_price);
obj.PackCount = dataSourceRow.getString(Shop.Column_pack_count);
obj.PurchasingPrice = dataSourceRow.getString(Shop.Column_purchasing_price);

