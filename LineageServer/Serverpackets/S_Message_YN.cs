using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Message_YN : ServerBasePacket
    {

        private byte[] _byte = null;

        private int yesNoCount = 0;

        public S_Message_YN(int type, string msg1)
        {
            buildPacket(type, msg1, null, null, 1);
        }

        public S_Message_YN(int type, string msg1, string msg2)
        {
            buildPacket(type, msg1, msg2, null, 2);
        }

        public S_Message_YN(int type, string msg1, string msg2, string msg3)
        {
            buildPacket(type, msg1, msg2, msg3, 3);
        }

        private void buildPacket(int type, string msg1, string msg2, string msg3, int check)
        {
            System.Threading.Interlocked.Increment(ref this.yesNoCount);
            WriteC(Opcodes.S_OPCODE_YES_NO);
            WriteH(0x0000); // 3.51未知封包
            WriteD(this.yesNoCount);
            WriteH(type);
            if (check == 1)
            {
                WriteS(msg1);
            }
            else if (check == 2)
            {
                WriteS(msg1);
                WriteS(msg2);
            }
            else if (check == 3)
            {
                WriteS(msg1);
                WriteS(msg2);
                WriteS(msg3);
            }
        }
        public override string Type
        {
            get
            {
                return "[S] S_Message_YN";
            }
        }
    }

}