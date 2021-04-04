using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_LoginResult : ServerBasePacket
    {
        public const string S_LOGIN_RESULT = "[S] S_LoginResult";

        public const int REASON_LOGIN_OK = 0x00;

        public const int REASON_ACCOUNT_IN_USE = 0x16;

        public const int REASON_ACCOUNT_ALREADY_EXISTS = 0x07;

        public const int REASON_ACCESS_FAILED = 0x08;

        public const int REASON_USER_OR_PASS_WRONG = 0x08;

        public const int REASON_PASS_WRONG = 0x08;

        public const int REASON_OUT_OF_GASH = 0x1c;

        // public static int REASON_SYSTEM_ERROR = 0x01;

        private byte[] _byte = null;

        public S_LoginResult(int reason)
        {
            buildPacket(reason);
        }

        private void buildPacket(int reason)
        {
            WriteC(Opcodes.S_OPCODE_LOGINRESULT);
            WriteC(reason);
            WriteD(0x00000000);
            WriteD(0x00000000);
            WriteD(0x00000000);
            WriteD(0x00000000);
            WriteH(0x8c);
        }
        public override string Type
        {
            get
            {
                return S_LOGIN_RESULT;
            }
        }
    }

}