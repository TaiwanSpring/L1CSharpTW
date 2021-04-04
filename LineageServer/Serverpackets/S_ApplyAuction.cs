using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using System;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_ApplyAuction : ServerBasePacket
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.BoardAuction);
        private static ILogger _log = Logger.GetLogger(nameof(S_ApplyAuction));
        private const string S_APPLYAUCTION = "[S] S_ApplyAuction";
        private byte[] _byte = null;

        public S_ApplyAuction(int objectId, string houseNumber)
        {
            buildPacket(objectId, houseNumber);
        }

        private void buildPacket(int objectId, string houseNumber)
        {
            int houseId = Convert.ToInt32(houseNumber);
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
                .Where(BoardAuction.Column_house_id, houseId).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int nowPrice = dataSourceRow.getInt(BoardAuction.Column_price);
                int bidderId = dataSourceRow.getInt(BoardAuction.Column_bidder_id);
                WriteC(Opcodes.S_OPCODE_INPUTAMOUNT);
                WriteD(objectId);
                WriteD(0); // ?
                if (bidderId == 0)
                { // 入札者なし
                    WriteD(nowPrice); // スピンコントロールの初期価格
                    WriteD(nowPrice); // 価格の下限
                }
                else
                { // 入札者あり
                    WriteD(nowPrice + 1); // スピンコントロールの初期価格
                    WriteD(nowPrice + 1); // 価格の下限
                }
                WriteD(2000000000); // 価格の上限
                WriteH(0); // ?
                WriteS("agapply");
                WriteS("agapply " + houseNumber);
            }
        }

        public override string Type
        {
            get
            {
                return S_APPLYAUCTION;
            }
        }
    }

}