using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_ItemColor : ServerBasePacket
    {

        private const string S_ITEM_COLOR = "[S] S_ItemColor";

        /// <summary>
        /// アイテムの色を変更する。祝福・呪い、封印状態が変化した時などに送る
        /// </summary>
        public S_ItemColor(L1ItemInstance item)
        {
            if (item == null)
            {
                return;
            }
            buildPacket(item);
        }

        private void buildPacket(L1ItemInstance item)
        {
            WriteC(Opcodes.S_OPCODE_ITEMCOLOR);
            WriteD(item.Id);
            // 0:祝福 1:通常 2:呪い 3:未鑑定
            // 128:祝福&封印 129:&封印 130:呪い&封印 131:未鑑定&封印
            WriteC(item.Bless);
        }
        public override string Type
        {
            get
            {
                return S_ITEM_COLOR;
            }
        }

    }

}