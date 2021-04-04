using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconWaterLife : ServerBasePacket
    {

        public S_SkillIconWaterLife()
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0x3b);
            WriteH(0x0000);
        }
    }
}