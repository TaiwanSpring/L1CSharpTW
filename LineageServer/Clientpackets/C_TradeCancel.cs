using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來交易取消的封包
    /// </summary>
    class C_TradeCancel : ClientBasePacket
    {
        private const string C_TRADE_CANCEL = "[C] C_TradeCancel";
        public C_TradeCancel(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance player = clientthread.ActiveChar;
            if (player == null)
            {
                return;
            }
            L1Trade trade = new L1Trade();
            trade.TradeCancel(player);
        }

        public override string Type
        {
            get
            {
                return C_TRADE_CANCEL;
            }
        }
    }

}