using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Trade : ServerBasePacket
    {
        public S_Trade(string name)
        {
            WriteC(Opcodes.S_OPCODE_TRADE);
            WriteS(name);
        }

        public override string Type
        {
            get
            {
                return "[S] S_Trade";
            }
        }
    }

}