using System;
namespace LineageServer.Server.Server.Model.classes
{
	internal class L1DarkElfClassFeature : L1ClassFeature
	{
		public override int getAcDefenseMax(int ac)
		{
			return ac / 4;
		}

		public override int getMagicLevel(int playerLevel)
		{
			return Math.Min(2, playerLevel / 12);
		}
	}

}