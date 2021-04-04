using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_ChangeName : ServerBasePacket
    {

        private const string S_CHANGE_NAME = "[S] S_ChangeName";

        private byte[] _byte = null;

        public S_ChangeName(int objectId, string name)
        {
            WriteC(Opcodes.S_OPCODE_CHANGENAME);
            WriteD(objectId);
            WriteS(name);
        }
        public override string Type
        {
            get
            {
                return S_CHANGE_NAME;
            }
        }
    }

}