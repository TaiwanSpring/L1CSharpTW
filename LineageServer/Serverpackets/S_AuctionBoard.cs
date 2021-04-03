using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Serverpackets
{
    class S_AuctionBoard : ServerBasePacket
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.BoardAuction);
        private const string S_AUCTIONBOARD = "[S] S_AuctionBoard";

        public S_AuctionBoard(L1NpcInstance board)
        {
            buildPacket(board);
        }

        private void buildPacket(L1NpcInstance board)
        {
            IList<int> houseList = ListFactory.NewList<int>();
            int houseId = 0;
            int count = 0;
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                houseId = dataSourceRow.getInt(BoardAuction.Column_house_id);
                if ((board.X == 33421) && (board.Y == 32823))
                { // 競売掲示板(ギラン)
                    if ((houseId >= 262145) && (houseId <= 262189))
                    {
                        houseList.Add(houseId);
                    }
                }
                else if ((board.X == 33585) && (board.Y == 33235))
                { // 競売掲示板(ハイネ)
                    if ((houseId >= 327681) && (houseId <= 327691))
                    {
                        houseList.Add(houseId);
                    }
                }
                else if ((board.X == 33959) && (board.Y == 33253))
                { // 競売掲示板(アデン)
                    if ((houseId >= 458753) && (houseId <= 458819))
                    {
                        houseList.Add(houseId);
                    }
                }
                else if ((board.X == 32611) && (board.Y == 32775))
                { // 競売掲示板(グルーディン)
                    if ((houseId >= 524289) && (houseId <= 524294))
                    {
                        houseList.Add(houseId);
                    }
                }
            }
            int[] id = new int[houseList.Count];
            string[] name = new string[houseList.Count];
            int[] area = new int[houseList.Count];
            int[] month = new int[houseList.Count];
            int[] day = new int[houseList.Count];
            int[] price = new int[houseList.Count];
            for (int i = 0; i < houseList.Count; ++i)
            {
                houseId = houseList[i];
                dataSourceRows = dataSource.Select().Where(BoardAuction.Column_house_id, houseId).Query();
                for (int j = 0; j < dataSourceRows.Count; j++)
                {
                    IDataSourceRow dataSourceRow = dataSourceRows[j];
                    id[i] = dataSourceRow.getInt(BoardAuction.Column_house_id);
                    name[i] = dataSourceRow.getString(BoardAuction.Column_house_name);
                    area[i] = dataSourceRow.getInt(BoardAuction.Column_house_area);
                    DateTime cal = dataSourceRow.getTimestamp(BoardAuction.Column_deadline);
                    month[i] = cal.Month + 1;
                    day[i] = cal.Day;
                    price[i] = dataSourceRow.getInt(BoardAuction.Column_price);
                }
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

        public override string Type
        {
            get
            {
                return S_AUCTIONBOARD;
            }
        }
    }

}