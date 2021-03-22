using System;
using System.Threading;

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
namespace LineageServer.Server.Server
{

	using Config = LineageServer.Server.Config;
	using AuctionBoardTable = LineageServer.Server.Server.DataSources.AuctionBoardTable;
	using ClanTable = LineageServer.Server.Server.DataSources.ClanTable;
	using HouseTable = LineageServer.Server.Server.DataSources.HouseTable;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1AuctionBoard = LineageServer.Server.Server.Templates.L1AuctionBoard;
	using L1House = LineageServer.Server.Server.Templates.L1House;

	public class HouseTaxTimeController : IRunnableStart
	{
		private static HouseTaxTimeController _instance;

		public static HouseTaxTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new HouseTaxTimeController();
				}
				return _instance;
			}
		}

		public override void run()
		{
			try
			{
				while (true)
				{
					checkTaxDeadline();
					Thread.Sleep(600000);
				}
			}
			catch (Exception)
			{
			}
		}

		public virtual DateTime RealTime
		{
			get
			{
				TimeZone tz = TimeZone.getTimeZone(Config.TIME_ZONE);
				DateTime cal = DateTime.getInstance(tz);
				return cal;
			}
		}

		private void checkTaxDeadline()
		{
			foreach (L1House house in HouseTable.Instance.HouseTableList)
			{
				if (!house.OnSale)
				{ // 不檢查再拍賣的血盟小屋
					if (house.TaxDeadline < RealTime)
					{
						sellHouse(house);
					}
				}
			}
		}

		private void sellHouse(L1House house)
		{
			AuctionBoardTable boardTable = new AuctionBoardTable();
			L1AuctionBoard board = new L1AuctionBoard();
			if (board != null)
			{
				// 在拍賣板張貼新公告
				int houseId = house.HouseId;
				board.HouseId = houseId;
				board.HouseName = house.HouseName;
				board.HouseArea = house.HouseArea;
				TimeZone tz = TimeZone.getTimeZone(Config.TIME_ZONE);
				DateTime cal = DateTime.getInstance(tz);
				cal.AddDays(5); // 5天以後
				cal.set(DateTime.MINUTE, 0);
				cal.set(DateTime.SECOND, 0);
				board.Deadline = cal;
				board.Price = 100000;
				board.Location = house.Location;
				board.OldOwner = "";
				board.OldOwnerId = 0;
				board.Bidder = "";
				board.BidderId = 0;
				boardTable.insertAuctionBoard(board);
				house.OnSale = true; // 設定為拍賣中
				house.PurchaseBasement = true; // TODO: 翻譯 地下アジト未購入に設定
				cal.AddDays(Config.HOUSE_TAX_INTERVAL);
				house.TaxDeadline = cal;
				HouseTable.Instance.updateHouse(house); // 儲存到資料庫中
				// 取消之前的擁有者
				foreach (L1Clan clan in L1World.Instance.AllClans)
				{
					if (clan.HouseId == houseId)
					{
						clan.HouseId = 0;
						ClanTable.Instance.updateClan(clan);
					}
				}
			}
		}

	}

}