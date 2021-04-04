using LineageServer.Server;

namespace LineageServer.Serverpackets
{
	class S_AddSkill : ServerBasePacket
	{
		private const string S_ADD_SKILL = "[S] S_AddSkill";

		private byte[] _byte = null;

		public S_AddSkill(int level, int id)
		{
			int[] ids = new int[28];
			for (int i = 0; i < ids.Length; i++)
			{
				ids[i] = 0;
			}
			ids[level] = id;

			bool hasLevel5to8 = 0 < (ids[4] + ids[5] + ids[6] + ids[7]);
			bool hasLevel9to10 = 0 < (ids[8] + ids[9]);

			WriteC(Opcodes.S_OPCODE_ADDSKILL);
			if (hasLevel5to8 && !hasLevel9to10)
			{
				WriteC(50);
			}
			else if (hasLevel9to10)
			{
				WriteC(100);
			}
			else
			{
				WriteC(32);
			}
			foreach (int i in ids)
			{
				WriteC(i);
			}
			WriteD(0);
			WriteD(0);
		}

		public S_AddSkill(int level1, int level2, int level3, int level4, int level5, int level6, int level7, int level8, int level9, int level10, int knight, int l2, int de1, int de2, int royal, int l3, int elf1, int elf2, int elf3, int elf4, int elf5, int elf6, int k5, int l5, int m5, int n5, int o5, int p5)
		{
			int i6 = level5 + level6 + level7 + level8;
			int j6 = level9 + level10;
			WriteC(Opcodes.S_OPCODE_ADDSKILL);
			if ((i6 > 0) && (j6 == 0))
			{
				WriteC(50);
			}
			else if (j6 > 0)
			{
				WriteC(100);
			}
			else
			{
				WriteC(32);
			}
			WriteC(level1);
			WriteC(level2);
			WriteC(level3);
			WriteC(level4);
			WriteC(level5);
			WriteC(level6);
			WriteC(level7);
			WriteC(level8);
			WriteC(level9);
			WriteC(level10);
			WriteC(knight);
			WriteC(l2);
			WriteC(de1);
			WriteC(de2);
			WriteC(royal);
			WriteC(l3);
			WriteC(elf1);
			WriteC(elf2);
			WriteC(elf3);
			WriteC(elf4);
			WriteC(elf5);
			WriteC(elf6);
			WriteC(k5);
			WriteC(l5);
			WriteC(m5);
			WriteC(n5);
			WriteC(o5);
			WriteC(p5);
			WriteD(0);
			WriteD(0);
		}

		public override string Type
		{
			get
			{
				return S_ADD_SKILL;
			}
		}

	}

}