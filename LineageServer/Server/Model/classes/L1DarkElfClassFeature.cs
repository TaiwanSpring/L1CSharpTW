using System;
namespace LineageServer.Server.Model.Classes
{
	internal class L1DarkElfClassFeature : L1ClassFeature
	{
		public override int GetAcDefenseMax(int ac)
		{
			return ac / 4;
		}

		public override int GetMagicLevel(int playerLevel)
		{
			return Math.Min(2, playerLevel / 12);
		}
	}

}