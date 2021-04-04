
using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_TradeAddItem : ServerBasePacket
    {
        private const string S_TRADE_ADD_ITEM = "[S] S_TradeAddItem";

        public S_TradeAddItem(L1ItemInstance item, int count, int type)
        {
            WriteC(Opcodes.S_OPCODE_TRADEADDITEM);
            WriteC(type); // 0:トレードウィンドウ上段 1:トレードウィンドウ下段
            WriteH(item.Item.GfxId);
            WriteS(item.getNumberedViewName(count));
            // 0:祝福 1:通常 2:呪い 3:未鑑定
            // 128:祝福&封印 129:&封印 130:呪い&封印 131:未鑑定&封印
            if (!item.Identified)
            {
                WriteC(3);
                WriteC(0);
                WriteC(7);
                WriteC(0);
            }
            else
            {
                WriteC(item.Bless);
                WriteC(item.StatusBytes.Length);
                foreach (byte b in item.StatusBytes)
                {
                    WriteC(b);
                }
                WriteH(0);
            }
        }

        public override string Type
        {
            get
            {
                return S_TRADE_ADD_ITEM;
            }
        }
    }

}