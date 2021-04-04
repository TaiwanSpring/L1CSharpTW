using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillIconBloodstain : ServerBasePacket
    {
        private const string S_SKILL_ICON_BLOODSTAIN = "[S] S_SkillIconBloodstain";

        public S_SkillIconBloodstain(int i, int j)
        {
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(0x64);
            WriteC(i); // 82:安塔瑞斯的血痕。 85:法利昂的血痕。 88:???的血痕。 91:???的血痕。
            WriteH(j); // 分
        }

        public override string Type
        {
            get
            {
                return S_SKILL_ICON_BLOODSTAIN;
            }
        }
    }

}