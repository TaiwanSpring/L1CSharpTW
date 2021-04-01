namespace LineageServer.Server.Templates
{
	class L1CharName
	{
		public virtual int Id { get; }

		public virtual string Name { get; }

		public L1CharName(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}