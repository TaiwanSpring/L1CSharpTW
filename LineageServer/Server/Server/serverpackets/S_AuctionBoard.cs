using LineageServer.Interfaces;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Utils.collections;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Server.serverpackets
{
    class S_AuctionBoard : ServerBasePacket
    {
        private const string S_AUCTIONBOARD = "[S] S_AuctionBoard";

        public S_AuctionBoard(L1NpcInstance board)
        {
            buildPacket(board);
        }

        private void buildPacket(L1NpcInstance board)
        {
            IList<int> houseList = Lists.newList<int>();
            int houseId = 0;
            int count = 0;
            int[] id = null;
            string[] name = null;
            int[] area = null;
            int[] month = null;
            int[] day = null;
            int[] price = null;
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;

            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM board_auction");
                rs = pstm.executeQuery();
                while (rs.next())
                {
                    houseId = dataSourceRow.getInt(1);
                    if ((board.X == 33421) && (board.Y == 32823))
                    { // 競売掲示板(ギラン)
                        if ((houseId >= 262145) && (houseId <= 262189))
                        {
                            houseList.Add(houseId);
                            count++;
                        }
                    }
                    else if ((board.X == 33585) && (board.Y == 33235))
                    { // 競売掲示板(ハイネ)
                        if ((houseId >= 327681) && (houseId <= 327691))
                        {
                            houseList.Add(houseId);
                            count++;
                        }
                    }
                    else if ((board.X == 33959) && (board.Y == 33253))
                    { // 競売掲示板(アデン)
                        if ((houseId >= 458753) && (houseId <= 458819))
                        {
                            houseList.Add(houseId);
                            count++;
                        }
                    }
                    else if ((board.X == 32611) && (board.Y == 32775))
                    { // 競売掲示板(グルーディン)
                        if ((houseId >= 524289) && (houseId <= 524294))
                        {
                            houseList.Add(houseId);
                            count++;
                        }
                    }
                }
                id = new int[count];
                name = new string[count];
                area = new int[count];
                month = new int[count];
                day = new int[count];
                price = new int[count];

                for (int i = 0; i < count; ++i)
                {
                    pstm = con.prepareStatement("SELECT * FROM board_auction WHERE house_id=?");
                    houseId = houseList[i];
                    pstm.setInt(1, houseId);
                    rs = pstm.executeQuery();
                    while (rs.next())
                    {
                        id[i] = dataSourceRow.getInt(1);
                        name[i] = dataSourceRow.getString(2);
                        area[i] = dataSourceRow.getInt(3);
                        DateTime cal = timestampToCalendar((Timestamp)dataSourceRow.getObject(4));
                        month[i] = cal.Month + 1;
                        day[i] = cal.Day;
                        price[i] = dataSourceRow.getInt(5);
                    }
                }
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }

            WriteC(Opcodes.S_OPCODE_HOUSELIST);
            WriteD(board.Id);
            WriteH(count); // レコード数
            for (int i = 0; i < count; ++i)
            {
                WriteD(id[i]); // アジトの番号
                WriteS(name[i]); // アジトの名前
                WriteH(area[i]); // アジトの広さ
                WriteC(month[i]); // 締切月
                WriteC(day[i]); // 締切日
                WriteD(price[i]); // 現在の入札価格
            }
        }

        private DateTime timestampToCalendar(Timestamp ts)
        {
            DateTime cal = new DateTime();
            cal.TimeInMillis = ts.Time;
            return cal;
        }

        public override string Type
        {
            get
            {
                return S_AUCTIONBOARD;
            }
        }
    }

}