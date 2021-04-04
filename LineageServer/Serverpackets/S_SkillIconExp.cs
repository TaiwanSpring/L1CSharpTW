using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconExp : ServerBasePacket
    {

        public S_SkillIconExp(int i)
        { //TODO 殷海薩的祝福
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(0x52);
            WriteC(i);
        }
    }

}