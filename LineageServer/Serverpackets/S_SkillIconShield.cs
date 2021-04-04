using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconShield : ServerBasePacket
    {

        public S_SkillIconShield(int type, int time)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONSHIELD);
            WriteH(time);
            WriteC(type);
            WriteD(0);
        }
    }

}