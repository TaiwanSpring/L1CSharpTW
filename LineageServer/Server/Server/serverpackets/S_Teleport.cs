using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_Teleport : ServerBasePacket
    {
        private const string S_TELEPORT = "[S] S_Teleport";
        public S_Teleport(L1PcInstance pc)
        {
            writeC(Opcodes.S_OPCODE_TELEPORT);
            writeC(0x00);
            writeC(0x40);
            writeD(pc.Id);
        }
        public override string Type
        {
            get
            {
                return S_TELEPORT;
            }
        }
    }

}