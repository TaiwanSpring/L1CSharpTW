
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_TradeAddItem : ServerBasePacket
    {
        private const string S_TRADE_ADD_ITEM = "[S] S_TradeAddItem";

        public S_TradeAddItem(L1ItemInstance item, int count, int type)
        {
            writeC(Opcodes.S_OPCODE_TRADEADDITEM);
            writeC(type); // 0:トレードウィンドウ上段 1:トレードウィンドウ下段
            writeH(item.Item.GfxId);
            writeS(item.getNumberedViewName(count));
            // 0:祝福 1:通常 2:呪い 3:未鑑定
            // 128:祝福&封印 129:&封印 130:呪い&封印 131:未鑑定&封印
            if (!item.Identified)
            {
                writeC(3);
                writeC(0);
                writeC(7);
                writeC(0);
            }
            else
            {
                writeC(item.Bless);
                writeC(item.StatusBytes.Length);
                foreach (byte b in item.StatusBytes)
                {
                    writeC(b);
                }
                writeH(0);
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