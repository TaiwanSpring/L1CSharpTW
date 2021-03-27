using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataSources
{
	public class AuctionBoardTable
	{
		private readonly IDictionary<int, L1AuctionBoard> _boards = MapFactory.NewConcurrentMap<int, L1AuctionBoard>();

		private readonly IDataSourceTable dataSourceTable = new EmptyDataSource();

		private DateTime timestampToCalendar(Timestamp ts)
		{
			DateTime cal = new DateTime();
			cal.TimeInMillis = ts.Time;
			return cal;
		}

		public AuctionBoardTable()
		{

			foreach (IDataSourceRow rs in this.dataSourceTable.Select())
			{
				L1AuctionBoard board = new L1AuctionBoard();
				board.HouseId = dataSourceRow.getInt(1);
				board.HouseName = dataSourceRow.getString(2);
				board.HouseArea = dataSourceRow.getInt(3);
				board.Deadline = timestampToCalendar((Timestamp)dataSourceRow.getObject(4));
				board.Price = dataSourceRow.getInt(5);
				board.Location = dataSourceRow.getString(6);
				board.OldOwner = dataSourceRow.getString(7);
				board.OldOwnerId = dataSourceRow.getInt(8);
				board.Bidder = dataSourceRow.getString(9);
				board.BidderId = dataSourceRow.getInt(10);
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
				IDataSourceRow dataSourceRow = this.dataSourceTable.NewRow();

				bool ok = dataSourceRow.Update()
						.Set("", board.HouseId)
						.Set("house_name", board.HouseName)
						.Set("house_area", board.HouseArea)
						.Set("deadline", board.Deadline)
						.Set("price", board.Price)
						.Set("location", board.Location)
						.Set("old_owner", board.OldOwner)
						.Set("old_owner_id", board.OldOwnerId)
						.Set("bidder", board.Bidder)
						.Set("bidder_id", board.HouseId).Execute();
				if (ok)
				{
					_boards[board.HouseId] = board;
				}
			}
		}

		public virtual void updateAuctionBoard(L1AuctionBoard board)
		{
			if (this._boards.ContainsKey(board.HouseId))
			{
				IDataSourceRow dataSourceRow = this.dataSourceTable.NewRow();

				bool ok = dataSourceRow.Update()
						.Where("house_id", board.HouseId)
						.Set("house_name", board.HouseName)
						.Set("house_area", board.HouseArea)
						.Set("deadline", board.Deadline)
						.Set("price", board.Price)
						.Set("location", board.Location)
						.Set("old_owner", board.OldOwner)
						.Set("old_owner_id", board.OldOwnerId)
						.Set("bidder", board.Bidder)
						.Set("bidder_id", board.BidderId).Execute();
				if (ok)
				{
					_boards[board.HouseId] = board;
				}
			}
		}

		public virtual void deleteAuctionBoard(int houseId)
		{
			if (this._boards.ContainsKey(houseId))
			{
				IDataSourceRow dataSourceRow = this.dataSourceTable.NewRow();

				bool ok = dataSourceRow.Delete()
						.Where("house_id", houseId).Execute();
				if (ok)
				{
					this._boards.Remove(houseId);
				}
			}
		}

	}

}