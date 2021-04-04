using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Light : ServerBasePacket
    {

        private const string S_LIGHT = "[S] S_Light";
        private byte[] _byte = null;

        public S_Light(int objid, int type)
        {
            buildPacket(objid, type);
        }

        private void buildPacket(int objid, int type)
        {
            WriteC(Opcodes.S_OPCODE_LIGHT);
            WriteD(objid);
            WriteC(type);
        }
        public override string Type
        {
            get
            {
                return S_LIGHT;
            }
        }
    }

}