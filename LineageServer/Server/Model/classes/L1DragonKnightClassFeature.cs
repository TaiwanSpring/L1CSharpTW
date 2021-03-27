using System;
namespace LineageServer.Server.Model.Classes
{
	internal class L1DragonKnightClassFeature : L1ClassFeature
	{
		public override int GetAcDefenseMax(int ac)
		{
			return ac / 3;
		}

		public override int GetMagicLevel(int playerLevel)
		{
			return Math.Min(4, playerLevel / 9);
		}
	}
}