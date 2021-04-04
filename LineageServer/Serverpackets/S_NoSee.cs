using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_NoSee : ServerBasePacket
    {

        private byte[] _byte = null;

        private const string S_NoSee_Conflict = "[S] S_NoSee";

        /// <summary>
        /// 『來源:伺服器』<位址:39>{長度:32}(時間:-1841663603)
        /// 0000:  27 [00 00 00 00] [6e 6f 73 65 65 62 00] 00 01 00 a7    '....noseeb.....
        /// 0010:  c6 bf d5 a9 5f a8 c8 b4 b5 00 8f 00 00 07 af 69    ...._..........i
        /// </summary>
        public S_NoSee(string targetName)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(0x00000000);
            WriteS("noseeb");
            WriteC(0);
            WriteH(1);
            WriteS(targetName);
            WriteC(0x8f);
            WriteH(0);
        }

        public override string Type
        {
            get
            {
                return S_NoSee_Conflict;
            }
        }
    }

}