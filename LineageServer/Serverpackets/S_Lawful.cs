using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Lawful : ServerBasePacket
    {

        private const string S_LAWFUL = "[S] S_Lawful";

        private byte[] _byte = null;

        public S_Lawful(int objid, int lawful)
        {
            buildPacket(objid, lawful);
        }

        private void buildPacket(int objid, int lawful)
        {
            WriteC(Opcodes.S_OPCODE_LAWFUL);
            WriteD(objid);
            WriteH(lawful);
            WriteD(0);
        }

        public override string Type
        {
            get
            {
                return S_LAWFUL;
            }
        }

    }
}