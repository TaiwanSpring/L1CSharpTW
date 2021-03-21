using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1AuctionBoard = LineageServer.Server.Server.Templates.L1AuctionBoard;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class AuctionBoardTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(AuctionBoardTable).FullName);

		private readonly IDictionary<int, L1AuctionBoard> _boards = Maps.newConcurrentMap();

		private DateTime timestampToCalendar(Timestamp ts)
		{
			DateTime cal = new DateTime();
			cal.TimeInMillis = ts.Time;
			return cal;
		}

		public AuctionBoardTable()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM board_auction ORDER BY house_id");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1AuctionBoard board = new L1AuctionBoard();
					board.HouseId = rs.getInt(1);
					board.HouseName = rs.getString(2);
					board.HouseArea = rs.getInt(3);
					board.Deadline = timestampToCalendar((Timestamp) rs.getObject(4));
					board.Price = rs.getInt(5);
					board.Location = rs.getString(6);
					board.OldOwner = rs.getString(7);
					board.OldOwnerId = rs.getInt(8);
					board.Bidder = rs.getString(9);
					board.BidderId = rs.getInt(10);
					_boards[board.HouseId] = board;
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO board_auction SET house_id=?, house_name=?, house_area=?, deadline=?, price=?, location=?, old_owner=?, old_owner_id=?, bidder=?, bidder_id=?");
				pstm.setInt(1, board.HouseId);
				pstm.setString(2, board.HouseName);
				pstm.setInt(3, board.HouseArea);
				SimpleDateFormat sdf = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
				string fm = sdf.format(board.Deadline);
				pstm.setString(4, fm);
				pstm.setInt(5, board.Price);
				pstm.setString(6, board.Location);
				pstm.setString(7, board.OldOwner);
				pstm.setInt(8, board.OldOwnerId);
				pstm.setString(9, board.Bidder);
				pstm.setInt(10, board.BidderId);
				pstm.execute();

				_boards[board.HouseId] = board;
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void updateAuctionBoard(L1AuctionBoard board)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE board_auction SET house_name=?, house_area=?, deadline=?, price=?, location=?, old_owner=?, old_owner_id=?, bidder=?, bidder_id=? WHERE house_id=?");
				pstm.setString(1, board.HouseName);
				pstm.setInt(2, board.HouseArea);
				SimpleDateFormat sdf = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
				string fm = sdf.format(board.Deadline);
				pstm.setString(3, fm);
				pstm.setInt(4, board.Price);
				pstm.setString(5, board.Location);
				pstm.setString(6, board.OldOwner);
				pstm.setInt(7, board.OldOwnerId);
				pstm.setString(8, board.Bidder);
				pstm.setInt(9, board.BidderId);
				pstm.setInt(10, board.HouseId);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void deleteAuctionBoard(int houseId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM board_auction WHERE house_id=?");
				pstm.setInt(1, houseId);
				pstm.execute();

				_boards.Remove(houseId);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

	}

}