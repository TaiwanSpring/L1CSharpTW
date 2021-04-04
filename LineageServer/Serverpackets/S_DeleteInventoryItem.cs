using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_DeleteInventoryItem : ServerBasePacket
    {

        private const string S_DELETE_INVENTORY_ITEM = "[S] S_DeleteInventoryItem";

        /// <summary>
        /// インベントリからアイテムを削除する。
        /// </summary>
        /// <param name="item">
        ///            - 削除するアイテム </param>
        public S_DeleteInventoryItem(L1ItemInstance item)
        {
            if (item != null)
            {
                WriteC(Opcodes.S_OPCODE_DELETEINVENTORYITEM);
                WriteD(item.Id);
            }
        }
        public override string Type
        {
            get
            {
                return S_DELETE_INVENTORY_ITEM;
            }
        }
    }

}