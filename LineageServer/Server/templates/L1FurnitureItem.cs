namespace LineageServer.Server.Templates
{
	class L1FurnitureItem
	{
		private int _furniture_itemId;
		private int _furniture_npcId;

		public virtual int FurnitureItemId
		{
			get
			{
				return _furniture_itemId;
			}
			set
			{
				_furniture_itemId = value;
			}
		}

		public virtual int FurnitureNpcId
		{
			get
			{
				return _furniture_npcId;
			}
			set
			{
				_furniture_npcId = value;
			}
		}
	}
}