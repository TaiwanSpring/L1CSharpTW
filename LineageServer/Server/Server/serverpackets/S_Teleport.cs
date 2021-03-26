using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_Teleport : ServerBasePacket
    {
        private const string S_TELEPORT = "[S] S_Teleport";
        public S_Teleport(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_TELEPORT);
            WriteC(0x00);
            WriteC(0x40);
            WriteD(pc.Id);
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