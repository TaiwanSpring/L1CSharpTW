using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconBlessOfEva : ServerBasePacket
    {

        public S_SkillIconBlessOfEva(int objectId, int time)
        {
            WriteC(Opcodes.S_OPCODE_BLESSOFEVA);
            WriteD(objectId);
            WriteH(time);
        }
    }

}