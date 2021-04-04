using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_NoSell : ServerBasePacket
    {
        private const string _S__25_NoSell = "[S] _S__25_NoSell";

        private byte[] _byte = null;

        public S_NoSell(L1NpcInstance npc)
        {
            buildPacket(npc);
        }

        private void buildPacket(L1NpcInstance npc)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(npc.Id);
            WriteS("nosell");
            WriteC(0x00);
            WriteH(0x00);
        }
        public override string Type
        {
            get
            {
                return _S__25_NoSell;
            }
        }
    }

}