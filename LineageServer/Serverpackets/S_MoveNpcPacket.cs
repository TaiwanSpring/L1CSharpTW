using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_MoveNpcPacket : ServerBasePacket
    {
        private const string S_MOVENPCPACKET = "[S] S_MoveNpcPacket";

        public S_MoveNpcPacket(L1MonsterInstance npc, int x, int y, int heading)
        {
            // npc.set_moving(true);

            WriteC(Opcodes.S_OPCODE_MOVEOBJECT);
            WriteD(npc.Id);
            WriteH(x);
            WriteH(y);
            WriteC(heading);
            WriteC(0x80); // 3.80C 更動
            WriteD(0x00000000);

            // npc.set_moving(false);
        }

        public override string Type
        {
            get
            {
                return S_MOVENPCPACKET;
            }
        }
    }

}