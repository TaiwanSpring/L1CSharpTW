using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Model.Classes
{
	public abstract class L1ClassFeature
	{
		public static L1ClassFeature newClassFeature(int classId)
		{
			if (classId == L1PcInstance.CLASSID_PRINCE || classId == L1PcInstance.CLASSID_PRINCESS)
			{
				return new L1RoyalClassFeature();
			}
			if (classId == L1PcInstance.CLASSID_ELF_MALE || classId == L1PcInstance.CLASSID_ELF_FEMALE)
			{
				return new L1ElfClassFeature();
			}
			if (classId == L1PcInstance.CLASSID_KNIGHT_MALE || classId == L1PcInstance.CLASSID_KNIGHT_FEMALE)
			{
				return new L1KnightClassFeature();
			}
			if (classId == L1PcInstance.CLASSID_WIZARD_MALE || classId == L1PcInstance.CLASSID_WIZARD_FEMALE)
			{
				return new L1WizardClassFeature();
			}
			if (classId == L1PcInstance.CLASSID_DARK_ELF_MALE || classId == L1PcInstance.CLASSID_DARK_ELF_FEMALE)
			{
				return new L1DarkElfClassFeature();
			}
			if (classId == L1PcInstance.CLASSID_DRAGON_KNIGHT_MALE || classId == L1PcInstance.CLASSID_DRAGON_KNIGHT_FEMALE)
			{
				return new L1DragonKnightClassFeature();
			}
			if (classId == L1PcInstance.CLASSID_ILLUSIONIST_MALE || classId == L1PcInstance.CLASSID_ILLUSIONIST_FEMALE)
			{
				return new L1IllusionistClassFeature();
			}
			throw new System.ArgumentException();
		}

		public abstract int getAcDefenseMax(int ac);

		public abstract int getMagicLevel(int playerLevel);
	}
}