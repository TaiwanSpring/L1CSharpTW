using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server;
using System;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_AuctionBoardRead : ServerBasePacket
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.BoardAuction);
        private const string S_AUCTIONBOARDREAD = "[S] S_AuctionBoardRead";
        private byte[] _byte = null;

        public S_AuctionBoardRead(int objectId, string house_number)
        {
            buildPacket(objectId, house_number);
        }

        private void buildPacket(int objectId, string house_number)
        {
            int number = Convert.ToInt32(house_number);
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Where(BoardAuction.Column_house_id, number).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                WriteC(Opcodes.S_OPCODE_SHOWHTML);
                WriteD(objectId);
                WriteS("agsel");
                WriteS(house_number); // アジトの番号
                WriteH(9); // 以下の文字列の個数
                WriteS(dataSourceRow.getString(BoardAuction.Column_house_name)); // アジトの名前
                WriteS(dataSourceRow.getString(BoardAuction.Column_location)); // アジトの位置
                WriteS(dataSourceRow.getInt(BoardAuction.Column_house_area).ToString()); // アジトの広さ
                WriteS(dataSourceRow.getString(BoardAuction.Column_old_owner)); // 以前の所有者
                WriteS(dataSourceRow.getString(BoardAuction.Column_bidder)); // 現在の入札者
                WriteS(dataSourceRow.getInt(BoardAuction.Column_price).ToString()); // 現在の入札価格
                DateTime cal = dataSourceRow.getTimestamp(BoardAuction.Column_deadline);
                int month = cal.Month + 1;
                int day = cal.Day;
                int hour = cal.Hour;
                WriteS(month.ToString()); // 締切月
                WriteS(day.ToString()); // 締切日
                WriteS(hour.ToString()); // 締切時
            }
        }

        public override string Type
        {
            get
            {
                return S_AUCTIONBOARDREAD;
            }
        }
    }

}