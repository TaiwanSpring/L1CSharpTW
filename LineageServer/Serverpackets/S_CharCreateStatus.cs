using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CharCreateStatus : ServerBasePacket
    {
        private const string S_CHAR_CREATE_STATUS = "[S] S_CharCreateStatus";

        public const int REASON_OK = 0x02;

        public const int REASON_ALREADY_EXSISTS = 0x06;

        public const int REASON_INVALID_NAME = 0x09;

        public const int REASON_WRONG_AMOUNT = 0x15;

        public S_CharCreateStatus(int reason)
        {
            WriteC(Opcodes.S_OPCODE_NEWCHARWRONG);
            WriteC(reason);
            WriteD(0x00000000);
            WriteH(0x0000);
        }

        public override string Type
        {
            get
            {
                return S_CHAR_CREATE_STATUS;
            }
        }
    }

}