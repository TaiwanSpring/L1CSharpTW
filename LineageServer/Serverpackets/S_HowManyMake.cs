using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_HowManyMake : ServerBasePacket
    {
        public S_HowManyMake(int objId, int max, string htmlId)
        {
            WriteC(Opcodes.S_OPCODE_INPUTAMOUNT);
            WriteD(objId);
            WriteD(0); // ?
            WriteD(0); // スピンコントロールの初期価格
            WriteD(0); // 価格の下限
            WriteD(max); // 価格の上限
            WriteH(0); // ?
            WriteS("request");
            WriteS(htmlId);
        }
    }
}