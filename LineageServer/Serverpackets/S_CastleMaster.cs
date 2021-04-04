using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CastleMaster : ServerBasePacket
    {

        private const string _S__08_CASTLEMASTER = "[S] S_CastleMaster";

        private byte[] _byte = null;

        public S_CastleMaster(int type, int objecId)
        {
            buildPacket(type, objecId);
        }

        private void buildPacket(int type, int objecId)
        {
            WriteC(Opcodes.S_OPCODE_CASTLEMASTER);
            WriteC(type);
            WriteD(objecId);
        }
        public override string Type
        {
            get
            {
                return _S__08_CASTLEMASTER;
            }
        }

    }

}