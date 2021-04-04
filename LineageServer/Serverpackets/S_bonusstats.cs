using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_bonusstats : ServerBasePacket
    {

        private byte[] _byte = null;

        public S_bonusstats(int i, int j)
        {
            buildPacket(i, j);
        }

        private void buildPacket(int i, int j)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(i);
            WriteS("RaiseAttr");
        }

        public override string Type
        {
            get
            {
                return "[S] S_bonusstats";
            }
        }
    }

}