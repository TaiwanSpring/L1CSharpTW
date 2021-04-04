using LineageServer.Server;
using LineageServer.Server.Templates;

namespace LineageServer.Serverpackets
{
    class S_SkillList : ServerBasePacket
    {
        private const string S_SKILL_LIST = "[S] S_SkillList";

        /*
		 * [Length:40] S -> C 0000 4C 20 FF FF 37 00 00 00 00 00 00 00 00 00 00 00 L
		 * ..7........... 0010 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
		 * ................ 0020 00 00 00 2E EF 67 33 87 .....g3.
		 */
        public S_SkillList(bool Insert, params L1Skills[] skills)
        {
            if (Insert)
            {
                WriteC(Opcodes.S_OPCODE_ADDSKILL);
            }
            else
            {
                WriteC(Opcodes.S_OPCODE_DELSKILL);
            }

            int[] SkillList = new int[0x20];

            WriteC(SkillList.Length);

            foreach (L1Skills skill in skills)
            {
                int level = skill.SkillLevel - 1;

                SkillList[level] |= skill.Id;
            }

            foreach (int i in SkillList)
            {
                WriteC(i);
            }

            WriteC(0x00); // 區分用的數值
        }
        public override string Type
        {
            get
            {
                return S_SKILL_LIST;
            }
        }
    }

}