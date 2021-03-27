using LineageServer.Interfaces;
using System;
namespace LineageServer.Serverpackets
{
    class S_ApplyAuction : ServerBasePacket
    {
        private static ILogger _log = Logger.GetLogger(nameof(S_ApplyAuction));
        private const string S_APPLYAUCTION = "[S] S_ApplyAuction";
        private byte[] _byte = null;

        public S_ApplyAuction(int objectId, string houseNumber)
        {
            buildPacket(objectId, houseNumber);
        }

        private void buildPacket(int objectId, string houseNumber)
        {
            con = L1DatabaseFactory.Instance.Connection;
            pstm = con.prepareStatement("SELECT * FROM board_auction WHERE house_id=?");
            int number = Convert.ToInt32(houseNumber);
            pstm.setInt(1, number);
            rs = pstm.executeQuery();
            while (rs.next())
            {
                int nowPrice = dataSourceRow.getInt(5);
                int bidderId = dataSourceRow.getInt(10);
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

        public override sbyte[] Content
        {
            get
            {
                if (_byte == null)
                {
                    _byte = Bytes;
                }
                return _byte;
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