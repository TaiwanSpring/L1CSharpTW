using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconThirdSpeed : ServerBasePacket
    {

        public S_SkillIconThirdSpeed(int j)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0x3c);
            WriteC(j); // time / 4
            WriteC(0x8);
        }
        public override string Type
        {
            get
            {
                return "[S] S_SkillIconThirdSpeed";
            }
        }
    }

}