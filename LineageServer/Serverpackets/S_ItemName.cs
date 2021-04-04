using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_ItemName : ServerBasePacket
    {

        private const string S_ITEM_NAME = "[S] S_ItemName";

        /// <summary>
        /// アイテムの名前を変更する。装備や強化状態が変わったときに送る。
        /// </summary>
        public S_ItemName(L1ItemInstance item)
        {
            if (item == null)
            {
                return;
            }
            // jumpを見る限り、このOpcodeはアイテム名を更新させる目的だけに使用される模様（装備後やOE後専用？）
            // 後に何かデータを続けて送っても全て無視されてしまう
            WriteC(Opcodes.S_OPCODE_ITEMNAME);
            WriteD(item.Id);
            WriteS(item.ViewName);
        }
        public override string Type
        {
            get
            {
                return S_ITEM_NAME;
            }
        }
    }

}