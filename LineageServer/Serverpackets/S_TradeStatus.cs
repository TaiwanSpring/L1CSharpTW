using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_TradeStatus : ServerBasePacket
    {
        public S_TradeStatus(int type)
        {
            WriteC(Opcodes.S_OPCODE_TRADESTATUS);
            WriteC(type); // 0:取引完了 1:取引キャンセル
        }
        public override string Type
        {
            get
            {
                return "[S] S_TradeStatus";
            }
        }
    }

}