using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_TrueTarget : ServerBasePacket
    {

        private const string S_TRUETARGET = "[S] S_TrueTarget";

        private byte[] _byte = null;

        public S_TrueTarget(int targetId, int objectId, string message)
        {
            buildPacket(targetId, objectId, message);
        }

        private void buildPacket(int targetId, int objectId, string message)
        {
            WriteC(Opcodes.S_OPCODE_TRUETARGET);
            WriteD(targetId);
            WriteD(objectId);
            WriteS(message);
        }
        public override string Type
        {
            get
            {
                return S_TRUETARGET;
            }
        }
    }

}