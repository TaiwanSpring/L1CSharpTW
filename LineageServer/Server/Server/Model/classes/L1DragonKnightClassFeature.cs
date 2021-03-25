using System;
namespace LineageServer.Server.Server.Model.classes
{
	internal class L1DragonKnightClassFeature : L1ClassFeature
	{
		public override int getAcDefenseMax(int ac)
		{
			return ac / 3;
		}

		public override int getMagicLevel(int playerLevel)
		{
			return Math.Min(4, playerLevel / 9);
		}
	}
}