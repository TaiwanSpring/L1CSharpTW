using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_ItemAmount : ServerBasePacket
    {

        private const string S_ITEM_AMOUNT = "[S] S_ItemAmount";

        public S_ItemAmount(L1ItemInstance item)
        {
            if (item == null)
            {
                return;
            }

            buildPacket(item);
        }

        private void buildPacket(L1ItemInstance item)
        {
            // WriteC(Opcodes.S_OPCODE_ITEMAMOUNT);
            // WriteD(item.getId());
            // WriteD(item.getCount());
            // WriteC(0);
            // 3.0
            WriteC(Opcodes.S_OPCODE_ITEMAMOUNT);
            WriteD(item.Id);
            WriteS(item.ViewName);
            WriteD(item.Count);
            if (!item.Identified)
            { // 未鑑定の場合ステータスを送る必要はない
                WriteC(0);
            }
            else
            {
                byte[] status = item.StatusBytes;
                WriteC(status.Length);
                foreach (byte b in status)
                {
                    WriteC(b);
                }
            }
            // 3.0 end
        }

        public override string Type
        {
            get
            {
                return S_ITEM_AMOUNT;
            }
        }

    }

}