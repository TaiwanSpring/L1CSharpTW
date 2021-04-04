using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Fight : ServerBasePacket
    {
        private const string S_FIGHT = "[S] S_Fight";

        private byte[] _byte = null;

        // 開啟
        public const int FLAG_ON = 1;

        // 關閉
        public const int FLAG_OFF = 0;

        // 正義第一階段(10000 ~ 19999)
        public const int TYPE_JUSTICE_LEVEL1 = 0;

        // 正義第二階段(20000 ~ 29999)
        public const int TYPE_JUSTICE_LEVEL2 = 1;

        // 正義第三階段(30000 ~ 32767)
        public const int TYPE_JUSTICE_LEVEL3 = 2;

        // 邪惡第一階段(-10000 ~ -19999)
        public const int TYPE_EVIL_LEVEL1 = 3;

        // 邪惡第二階段(-20000 ~ -29999)
        public const int TYPE_EVIL_LEVEL2 = 4;

        // 邪惡第三階段(-30000 ~ -32768)
        public const int TYPE_EVIL_LEVEL3 = 5;

        // 遭遇的守護(新手保護)
        public const int TYPE_ENCOUNTER = 6;

        public S_Fight(int type, int flag)
        {
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(0x72);
            WriteD(type);
            WriteD((flag == FLAG_OFF) ? FLAG_OFF : FLAG_ON);
        }
        public override string Type
        {
            get
            {
                return S_FIGHT;
            }
        }
    }

}