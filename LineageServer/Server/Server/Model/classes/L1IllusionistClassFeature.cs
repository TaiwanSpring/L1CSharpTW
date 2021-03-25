using System;
namespace LineageServer.Server.Server.Model.classes
{
	internal class L1IllusionistClassFeature : L1ClassFeature
	{
		public override int getAcDefenseMax(int ac)
		{
			return ac / 3;
		}

		public override int getMagicLevel(int playerLevel)
		{
			return Math.Min(6, playerLevel / 8);
		}
	}
}