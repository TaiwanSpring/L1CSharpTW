using LineageServer.Interfaces;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Templates;
using System;
using System.Threading;
namespace LineageServer.Server
{
    public class HouseTaxTimeController : IRunnable
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

        public void run()
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

        private void checkTaxDeadline()
        {
            foreach (L1House house in HouseTable.Instance.HouseTableList)
            {
                if (!house.OnSale)
                {
                    // 不檢查再拍賣的血盟小屋
                    if (house.TaxDeadline < DateTime.Now)
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
                // 5天以後
                board.Deadline = DateTime.Now.Date.AddDays(5);
                board.Price = 100000;
                board.Location = house.Location;
                board.OldOwner = "";
                board.OldOwnerId = 0;
                board.Bidder = "";
                board.BidderId = 0;
                boardTable.insertAuctionBoard(board);
                house.OnSale = true; // 設定為拍賣中
                house.PurchaseBasement = true; // TODO: 翻譯 地下アジト未購入に設定                
                house.TaxDeadline = board.Deadline.AddDays(Config.HOUSE_TAX_INTERVAL);

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