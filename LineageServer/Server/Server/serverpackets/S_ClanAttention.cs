
namespace LineageServer.Server.Server.serverpackets
{
    class S_ClanAttention : ServerBasePacket
    {
        private const string S_ClanAttention_Conflict = "[S] S_ClanAttention";
        public S_ClanAttention()
        {
            WriteC(Opcodes.S_OPCODE_CLANATTENTION);
            WriteD(2);
        }

        public override byte[] Content
        {
            get
            {
                return Bytes;
            }
        }

        public override string Type
        {
            get
            {
                return S_ClanAttention_Conflict;
            }
        }
    }

}