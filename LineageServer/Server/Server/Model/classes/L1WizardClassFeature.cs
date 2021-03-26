using System;
namespace LineageServer.Server.Server.Model.Classes
{
	internal class L1WizardClassFeature : L1ClassFeature
	{
		public override int getAcDefenseMax(int ac)
		{
			return ac / 5;
		}

		public override int getMagicLevel(int playerLevel)
		{
			return Math.Min(10, playerLevel / 4);
		}
	}
}