using System;
namespace LineageServer.Server.Model.Classes
{
	internal class L1WizardClassFeature : L1ClassFeature
	{
		public override int GetAcDefenseMax(int ac)
		{
			return ac / 5;
		}

		public override int GetMagicLevel(int playerLevel)
		{
			return Math.Min(10, playerLevel / 4);
		}
	}
}