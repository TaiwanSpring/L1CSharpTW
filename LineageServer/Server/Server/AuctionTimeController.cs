using LineageServer.Interfaces;
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.storage;
using LineageServer.Server.Server.Templates;
using System;
using System.Threading;

namespace LineageServer.Server.Server
{
    public class AuctionTimeController : IRunnableStart
    {
        private static ILogger _log = Logger.getLogger(nameof(AuctionTimeController));

        private static AuctionTimeController _instance;

        public static AuctionTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AuctionTimeController();
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
                    checkAuctionDeadline();
                    Thread.Sleep(60000);
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
                return DateTime.Now.Date;
            }
        }

        private void checkAuctionDeadline()
        {
            AuctionBoardTable boardTable = new AuctionBoardTable();
            foreach (L1AuctionBoard board in boardTable.AuctionBoardTableList)
            {
                if (board.Deadline < RealTime)
                {
                    endAuction(board);
                }
            }
        }

        private void endAuction(L1AuctionBoard board)
        {
            int houseId = board.HouseId;
            int price = board.Price;
            int oldOwnerId = board.OldOwnerId;
            string bidder = board.Bidder;
            int bidderId = board.BidderId;

            if (oldOwnerId != 0 && bidderId != 0)
            { // 在前主人與得標者都存在的情況下
                L1PcInstance oldOwnerPc = (L1PcInstance)L1World.Instance.findObject(oldOwnerId);
                int payPrice = (int)(price * 0.9);
                if (oldOwnerPc != null)
                { // 如果有前主人
                    oldOwnerPc.Inventory.storeItem(L1ItemId.ADENA, payPrice);
                    // あなたが所有していた家が最終価格%1アデナで落札されました。%n
                    // 手数料10%%を除いた残りの金額%0アデナを差し上げます。%nありがとうございました。%n%n
                    oldOwnerPc.sendPackets(new S_ServerMessage(527, payPrice.ToString()));
                }
                else
                { // 沒有前主人
                    L1ItemInstance item = ItemTable.Instance.createItem(L1ItemId.ADENA);
                    item.Count = payPrice;
                    try
                    {
                        CharactersItemStorage storage = CharactersItemStorage.create();
                        storage.storeItem(oldOwnerId, item);
                    }
                    catch (Exception e)
                    {
                        _log.log(Enum.Level.Server, e.Message, e);
                    }
                }

                L1PcInstance bidderPc = (L1PcInstance)L1World.Instance.findObject(bidderId);
                if (bidderPc != null)
                { // 如果有得標者
                  // おめでとうございます。%nあなたが参加された競売は最終価格%0アデナの価格で落札されました。%n
                  // 様がご購入された家はすぐにご利用できます。%nありがとうございました。%n%n
                    bidderPc.sendPackets(new S_ServerMessage(524, price.ToString(), bidder));
                }
                deleteHouseInfo(houseId);
                setHouseInfo(houseId, bidderId);
                deleteNote(houseId);
            }
            else if (oldOwnerId == 0 && bidderId != 0)
            { // 在先前的擁有者沒有中標
                L1PcInstance bidderPc = (L1PcInstance)L1World.Instance.findObject(bidderId);
                if (bidderPc != null)
                { // 落札者がオンライン中
                  // おめでとうございます。%nあなたが参加された競売は最終価格%0アデナの価格で落札されました。%n
                  // 様がご購入された家はすぐにご利用できます。%nありがとうございました。%n%n
                    bidderPc.sendPackets(new S_ServerMessage(524, price.ToString(), bidder));
                }

                setHouseInfo(houseId, bidderId);
                deleteNote(houseId);
            }
            else if (oldOwnerId != 0 && bidderId == 0)
            { // 以前沒有人成功競投無
                L1PcInstance oldOwnerPc = (L1PcInstance)L1World.Instance.findObject(oldOwnerId);
                if (oldOwnerPc != null)
                { // 以前の所有者がオンライン中
                  // あなたが申請なさった競売は、競売期間内に提示した金額以上での支払いを表明した方が現れなかったため、結局取り消されました。%n
                  // 従って、所有権があなたに戻されたことをお知らせします。%nありがとうございました。%n%n
                    oldOwnerPc.sendPackets(new S_ServerMessage(528));
                }
                deleteNote(houseId);
            }
            else if (oldOwnerId == 0 && bidderId == 0)
            {
                // 在先前的擁有者沒有中標
                // 設定五天之後再次競標
                // 5天後
                board.Deadline = RealTime.AddDays(5);
                AuctionBoardTable boardTable = new AuctionBoardTable();
                boardTable.updateAuctionBoard(board);
            }
        }

        /// <summary>
        /// 取消擁有者的血盟小屋
        /// </summary>
        /// <param name="houseId">
        ///            血盟小屋的編號
        /// @return </param>
        private void deleteHouseInfo(int houseId)
        {
            foreach (L1Clan clan in L1World.Instance.AllClans)
            {
                if (clan.HouseId == houseId)
                {
                    clan.HouseId = 0;
                    ClanTable.Instance.updateClan(clan);
                }
            }
        }

        /// <summary>
        /// 設定得標者血盟小屋的編號
        /// </summary>
        /// <param name="houseId">
        ///            血盟小屋的編號 </param>
        /// <param name="bidderId">
        ///            得標者的編號
        /// @return </param>
        private void setHouseInfo(int houseId, int bidderId)
        {
            foreach (L1Clan clan in L1World.Instance.AllClans)
            {
                if (clan.LeaderId == bidderId)
                {
                    clan.HouseId = houseId;
                    ClanTable.Instance.updateClan(clan);
                    break;
                }
            }
        }

        /// <summary>
        /// 將血盟小屋拍賣的告示取消、設定血盟小屋為不拍賣狀態
        /// </summary>
        /// <param name="houseId">
        ///            血盟小屋的編號
        /// @return </param>
        private void deleteNote(int houseId)
        {
            // 將血盟小屋的狀態設定為不拍賣
            L1House house = HouseTable.Instance.getHouseTable(houseId);
            house.OnSale = false;
            house.TaxDeadline = RealTime.AddDays(Config.HOUSE_TAX_INTERVAL);
            HouseTable.Instance.updateHouse(house);

            // 取消拍賣告示
            AuctionBoardTable boardTable = new AuctionBoardTable();
            boardTable.deleteAuctionBoard(houseId);
        }
    }

}