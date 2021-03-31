private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.BoardAuction);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(BoardAuction.Column_house_id, obj.HouseId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(BoardAuction.Column_house_id, obj.HouseId)
.Set(BoardAuction.Column_house_name, obj.HouseName)
.Set(BoardAuction.Column_house_area, obj.HouseArea)
.Set(BoardAuction.Column_deadline, obj.Deadline)
.Set(BoardAuction.Column_price, obj.Price)
.Set(BoardAuction.Column_location, obj.Location)
.Set(BoardAuction.Column_old_owner, obj.OldOwner)
.Set(BoardAuction.Column_old_owner_id, obj.OldOwnerId)
.Set(BoardAuction.Column_bidder, obj.Bidder)
.Set(BoardAuction.Column_bidder_id, obj.BidderId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(BoardAuction.Column_house_id, obj.HouseId)
.Set(BoardAuction.Column_house_name, obj.HouseName)
.Set(BoardAuction.Column_house_area, obj.HouseArea)
.Set(BoardAuction.Column_deadline, obj.Deadline)
.Set(BoardAuction.Column_price, obj.Price)
.Set(BoardAuction.Column_location, obj.Location)
.Set(BoardAuction.Column_old_owner, obj.OldOwner)
.Set(BoardAuction.Column_old_owner_id, obj.OldOwnerId)
.Set(BoardAuction.Column_bidder, obj.Bidder)
.Set(BoardAuction.Column_bidder_id, obj.BidderId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(BoardAuction.Column_house_id, obj.HouseId)
.Execute();


obj.HouseId = dataSourceRow.getString(BoardAuction.Column_house_id);
obj.HouseName = dataSourceRow.getString(BoardAuction.Column_house_name);
obj.HouseArea = dataSourceRow.getString(BoardAuction.Column_house_area);
obj.Deadline = dataSourceRow.getString(BoardAuction.Column_deadline);
obj.Price = dataSourceRow.getString(BoardAuction.Column_price);
obj.Location = dataSourceRow.getString(BoardAuction.Column_location);
obj.OldOwner = dataSourceRow.getString(BoardAuction.Column_old_owner);
obj.OldOwnerId = dataSourceRow.getString(BoardAuction.Column_old_owner_id);
obj.Bidder = dataSourceRow.getString(BoardAuction.Column_bidder);
obj.BidderId = dataSourceRow.getString(BoardAuction.Column_bidder_id);

