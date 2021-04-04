using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SelectTarget : ServerBasePacket
    {

        private const string S_SELECT_TARGET = "[S] S_SelectTarget";

        private byte[] _byte = null;

        public S_SelectTarget(int ObjectId)
        {
            WriteC(Opcodes.S_OPCODE_SELECTTARGET);
            WriteD(ObjectId);
            WriteC(0x00);
            WriteC(0x00);
            WriteC(0x00);
        }
        public override string Type
        {
            get
            {
                return S_SELECT_TARGET;
            }
        }
    }

}