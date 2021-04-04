using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_ItemStatus : ServerBasePacket
    {

        private const string S_ITEM_STATUS = "[S] S_ItemStatus";

        /// <summary>
        /// アイテムの名前、状態、特性、重量などの表示を変更する
        /// </summary>
        public S_ItemStatus(L1ItemInstance item)
        {
            WriteC(Opcodes.S_OPCODE_ITEMSTATUS);
            WriteD(item.Id);
            WriteS(item.ViewName);
            WriteD(item.Count);
            if (!item.Identified)
            {
                // 未鑑定の場合ステータスを送る必要はない
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
        }

        public override string Type
        {
            get
            {
                return S_ITEM_STATUS;
            }
        }
    }

}