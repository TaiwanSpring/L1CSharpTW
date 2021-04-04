using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_ChangeHeading : ServerBasePacket
    {
        private byte[] _byte = null;

        public S_ChangeHeading(L1Character cha)
        {
            buildPacket(cha);
        }

        private void buildPacket(L1Character cha)
        {
            WriteC(Opcodes.S_OPCODE_CHANGEHEADING);
            WriteD(cha.Id);
            WriteC(cha.Heading);
        }

        public override string Type
        {
            get
            {
                return "[S] S_ChangeHeading";
            }
        }
    }

}