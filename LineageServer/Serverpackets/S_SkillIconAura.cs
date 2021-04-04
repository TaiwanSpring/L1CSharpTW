using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconAura : ServerBasePacket
    {

        public S_SkillIconAura(int i, int j)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0x16);
            WriteC(i);
            WriteH(j);
        }

        public S_SkillIconAura(int i, int j, int k)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0x16);
            WriteC(i);
            WriteH(j);
            WriteC(k);
        }
    }

}