private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.House);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(House.Column_house_id, obj.HouseId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(House.Column_house_id, obj.HouseId)
.Set(House.Column_house_name, obj.HouseName)
.Set(House.Column_house_area, obj.HouseArea)
.Set(House.Column_location, obj.Location)
.Set(House.Column_keeper_id, obj.KeeperId)
.Set(House.Column_is_on_sale, obj.IsOnSale)
.Set(House.Column_is_purchase_basement, obj.IsPurchaseBasement)
.Set(House.Column_tax_deadline, obj.TaxDeadline)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(House.Column_house_id, obj.HouseId)
.Set(House.Column_house_name, obj.HouseName)
.Set(House.Column_house_area, obj.HouseArea)
.Set(House.Column_location, obj.Location)
.Set(House.Column_keeper_id, obj.KeeperId)
.Set(House.Column_is_on_sale, obj.IsOnSale)
.Set(House.Column_is_purchase_basement, obj.IsPurchaseBasement)
.Set(House.Column_tax_deadline, obj.TaxDeadline)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(House.Column_house_id, obj.HouseId)
.Execute();


obj.HouseId = dataSourceRow.getString(House.Column_house_id);
obj.HouseName = dataSourceRow.getString(House.Column_house_name);
obj.HouseArea = dataSourceRow.getString(House.Column_house_area);
obj.Location = dataSourceRow.getString(House.Column_location);
obj.KeeperId = dataSourceRow.getString(House.Column_keeper_id);
obj.IsOnSale = dataSourceRow.getString(House.Column_is_on_sale);
obj.IsPurchaseBasement = dataSourceRow.getString(House.Column_is_purchase_basement);
obj.TaxDeadline = dataSourceRow.getString(House.Column_tax_deadline);

