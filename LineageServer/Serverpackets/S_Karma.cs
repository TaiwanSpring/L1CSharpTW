using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_Karma : ServerBasePacket
    {

        private const string S_KARMA = "[S] S_Karma";

        public S_Karma(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(0x57);
            WriteD(pc.Karma);
        }

        public override string Type
        {
            get
            {
                return S_KARMA;
            }
        }
    }

}