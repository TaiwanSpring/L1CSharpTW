using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconWisdomPotion : ServerBasePacket
    {

        public S_SkillIconWisdomPotion(int time)
        {
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0x39);
            WriteC(0x2c);
            WriteC(time);
        }
    }

}