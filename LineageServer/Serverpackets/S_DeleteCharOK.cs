using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_DeleteCharOK : ServerBasePacket
    {
        private const string S_DELETE_CHAR_OK = "[S] S_DeleteCharOK";

        public const int DELETE_CHAR_NOW = 0x05;
        public const int DELETE_CHAR_AFTER_7DAYS = 0x51;

        public S_DeleteCharOK(int type)
        {
            WriteC(Opcodes.S_OPCODE_DETELECHAROK);
            WriteC(type);
        }

        public override string Type
        {
            get
            {
                return S_DELETE_CHAR_OK;
            }
        }
    }

}