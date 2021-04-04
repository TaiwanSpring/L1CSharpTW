using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillBrave : ServerBasePacket
    {

        public S_SkillBrave(int i, int j, int k)
        {
            WriteC(Opcodes.S_OPCODE_SKILLBRAVE);
            WriteD(i);
            WriteC(j);
            WriteH(k);
        }
    }

}