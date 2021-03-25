namespace LineageServer.Server.Server.Model.classes
{
	internal class L1KnightClassFeature : L1ClassFeature
	{
		public override int getAcDefenseMax(int ac)
		{
			return ac / 2;
		}

		public override int getMagicLevel(int playerLevel)
		{
			return playerLevel / 50;
		}
	}
}