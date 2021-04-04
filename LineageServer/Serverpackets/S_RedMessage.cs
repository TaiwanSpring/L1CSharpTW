using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_RedMessage : ServerBasePacket
    {

        private const string _S__18_REDMESSAGE = "[S] S_RedMessage";

        private byte[] _byte = null;

        public S_RedMessage(int type, string msg1)
        {
            buildPacket(type, msg1, null, null, 1);
        }

        public S_RedMessage(int type, string msg1, string msg2)
        {
            buildPacket(type, msg1, msg2, null, 2);
        }

        public S_RedMessage(int type, string msg1, string msg2, string msg3)
        {
            buildPacket(type, msg1, msg2, msg3, 3);
        }

        private void buildPacket(int type, string msg1, string msg2, string msg3, int check)
        {
            WriteC(Opcodes.S_OPCODE_REDMESSAGE);
            WriteH(type);
            if (check == 1)
            {
                if (msg1.Length <= 0)
                {
                    WriteC(0);
                }
                else
                {
                    WriteC(1);
                    WriteS(msg1);
                }
            }
            else if (check == 2)
            {
                WriteC(2);
                WriteS(msg1);
                WriteS(msg2);
            }
            else if (check == 3)
            {
                WriteC(3);
                WriteS(msg1);
                WriteS(msg2);
                WriteS(msg3);
            }
        }
        public override string Type
        {
            get
            {
                return _S__18_REDMESSAGE;
            }
        }
    }

}