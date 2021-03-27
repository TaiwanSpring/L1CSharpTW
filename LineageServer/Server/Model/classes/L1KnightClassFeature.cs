namespace LineageServer.Server.Model.Classes
{
	internal class L1KnightClassFeature : L1ClassFeature
	{
		public override int GetAcDefenseMax(int ac)
		{
			return ac / 2;
		}

		public override int GetMagicLevel(int playerLevel)
		{
			return playerLevel / 50;
		}
	}
}