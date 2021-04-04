using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Fishing : ServerBasePacket
    {

        private const string S_FISHING = "[S] S_Fishing";

        private byte[] _byte = null;

        public S_Fishing()
        {
            buildPacket();
        }

        public S_Fishing(int objectId, int motionNum, int x, int y)
        {
            buildPacket(objectId, motionNum, x, y);
        }

        private void buildPacket()
        {
            WriteC(Opcodes.S_OPCODE_DOACTIONGFX);
            WriteC(0x37); // ?
            WriteD(0x76002822); // ?
            WriteH(0x8AC3); // ?
        }

        private void buildPacket(int objectId, int motionNum, int x, int y)
        {
            WriteC(Opcodes.S_OPCODE_DOACTIONGFX);
            WriteD(objectId);
            WriteC(motionNum);
            WriteH(x);
            WriteH(y);
            WriteD(0);
            WriteH(0);
        }
        public override string Type
        {
            get
            {
                return S_FISHING;
            }
        }
    }

}