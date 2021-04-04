using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconWindShackle : ServerBasePacket
    {

        public S_SkillIconWindShackle(int objectId, int time)
        {
            int buffTime = (time / 4); // なぜか4倍されるため4で割っておく
            WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
            WriteC(0x2c);
            WriteD(objectId);
            WriteH(buffTime);
        }
    }

}