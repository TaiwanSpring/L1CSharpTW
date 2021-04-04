
using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SellHouse : ServerBasePacket
    {

        private const string S_SELLHOUSE = "[S] S_SellHouse";

        private byte[] _byte = null;

        public S_SellHouse(int objectId, string houseNumber)
        {
            buildPacket(objectId, houseNumber);
        }

        private void buildPacket(int objectId, string houseNumber)
        {
            WriteC(Opcodes.S_OPCODE_INPUTAMOUNT);
            WriteD(objectId);
            WriteD(0); // ?
            WriteD(100000); // スピンコントロールの初期価格
            WriteD(100000); // 価格の下限
            WriteD(2000000000); // 価格の上限
            WriteH(0); // ?
            WriteS("agsell");
            WriteS("agsell " + houseNumber);
        }
        public override string Type
        {
            get
            {
                return S_SELLHOUSE;
            }
        }
    }

}