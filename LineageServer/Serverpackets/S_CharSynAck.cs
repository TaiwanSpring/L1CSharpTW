using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CharSynAck : ServerBasePacket
    {

        private const string S_CHARSYNACK = "[S] S_CharSynAck";

        private byte[] _byte = null;
        public const int SYN = 0x0a;
        public const int ACK = 0x40;

        public S_CharSynAck(int type)
        {
            buildPacket(type);
        }

        private void buildPacket(int type)
        {
            WriteC(Opcodes.S_OPCODE_CHARSYNACK);
            WriteC(type); // SYN: 0x0a  ACK: 0x40
            if (type == 0x0a)
            {
                WriteC(0x02);
                WriteC(0x00);
                WriteC(0x00);
                WriteC(0x00);
                WriteC(0x08);
                WriteC(0x00);
            }
            else
            {
                WriteD(0x00000000);
                WriteH(0x0000);
            }

        }

        public override string Type
        {
            get
            {
                return S_CHARSYNACK;
            }
        }
    }

}