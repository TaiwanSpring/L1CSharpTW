using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillHaste : ServerBasePacket
    {

        public S_SkillHaste(int i, int j, int k)
        {
            WriteC(Opcodes.S_OPCODE_SKILLHASTE);
            WriteD(i);
            WriteC(j);
            WriteH(k);
        }
    }

}