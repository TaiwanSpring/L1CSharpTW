using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
    public class AuctionBoardTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.BoardAuction);

        private readonly IDictionary<int, L1AuctionBoard> _boards = MapFactory.NewConcurrentMap<int, L1AuctionBoard>();

        public AuctionBoardTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                L1AuctionBoard board = new L1AuctionBoard();
                board.HouseId = dataSourceRow.getInt(BoardAuction.Column_house_id);
                board.HouseName = dataSourceRow.getString(BoardAuction.Column_house_name);
                board.HouseArea = dataSourceRow.getInt(BoardAuction.Column_house_area);
                board.Deadline = dataSourceRow.getTimestamp(BoardAuction.Column_deadline);
                board.Price = dataSourceRow.getInt(BoardAuction.Column_price);
                board.Location = dataSourceRow.getString(BoardAuction.Column_location);
                board.OldOwner = dataSourceRow.getString(BoardAuction.Column_old_owner);
                board.OldOwnerId = dataSourceRow.getInt(BoardAuction.Column_old_owner_id);
                board.Bidder = dataSourceRow.getString(BoardAuction.Column_bidder);
                board.BidderId = dataSourceRow.getInt(BoardAuction.Column_bidder_id);
                _boards[board.HouseId] = board;
            }
        }

        public virtual L1AuctionBoard[] AuctionBoardTableList
        {
            get
            {
                return _boards.Values.ToArray();
            }
        }

        public virtual L1AuctionBoard getAuctionBoardTable(int houseId)
        {
            return _boards[houseId];
        }

        public virtual void insertAuctionBoard(L1AuctionBoard board)
        {
            if (!this._boards.ContainsKey(board.HouseId))
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();

                dataSourceRow.Insert()
                        .Set(BoardAuction.Column_house_id, board.HouseId)
                        .Set(BoardAuction.Column_house_name, board.HouseName)
                        .Set(BoardAuction.Column_house_area, board.HouseArea)
                        .Set(BoardAuction.Column_deadline, board.Deadline)
                        .Set(BoardAuction.Column_price, board.Price)
                        .Set(BoardAuction.Column_location, board.Location)
                        .Set(BoardAuction.Column_old_owner, board.OldOwner)
                        .Set(BoardAuction.Column_old_owner_id, board.OldOwnerId)
                        .Set(BoardAuction.Column_bidder, board.Bidder)
                        .Set(BoardAuction.Column_bidder_id, board.HouseId).Execute();
                _boards[board.HouseId] = board;
            }
        }

        public virtual void updateAuctionBoard(L1AuctionBoard board)
        {
            if (this._boards.ContainsKey(board.HouseId))
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();

                dataSourceRow.Insert()
                        .Where(BoardAuction.Column_house_id, board.HouseId)
                        .Set(BoardAuction.Column_house_name, board.HouseName)
                        .Set(BoardAuction.Column_house_area, board.HouseArea)
                        .Set(BoardAuction.Column_deadline, board.Deadline)
                        .Set(BoardAuction.Column_price, board.Price)
                        .Set(BoardAuction.Column_location, board.Location)
                        .Set(BoardAuction.Column_old_owner, board.OldOwner)
                        .Set(BoardAuction.Column_old_owner_id, board.OldOwnerId)
                        .Set(BoardAuction.Column_bidder, board.Bidder)
                        .Set(BoardAuction.Column_bidder_id, board.HouseId).Execute();
                _boards[board.HouseId] = board;
            }
        }

        public virtual void deleteAuctionBoard(int houseId)
        {
            if (this._boards.ContainsKey(houseId))
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Delete()
                .Where(BoardAuction.Column_house_id, houseId)
                .Execute();
                this._boards.Remove(houseId);
            }
        }
    }
}