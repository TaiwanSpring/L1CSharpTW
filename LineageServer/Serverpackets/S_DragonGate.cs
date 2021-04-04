using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_DragonGate : ServerBasePacket
    {
        private const string S_DRAGON_GATE = "[S] S_DragonGate";

        private byte[] _byte = null;

        public S_DragonGate(L1PcInstance pc, bool[] i)
        {
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(0x66); // = 102
            WriteD(pc.Id);
            // true 可點選，false 不能點選
            WriteC(i[0] ? 1 : 0); // 安塔瑞斯
            WriteC(i[1] ? 1 : 0); // 法利昂
            WriteC(i[2] ? 1 : 0); // 林德拜爾
            WriteC(i[3] ? 1 : 0); // 巴拉卡斯
        }
        public override string Type
        {
            get
            {
                return S_DRAGON_GATE;
            }
        }
    }

}