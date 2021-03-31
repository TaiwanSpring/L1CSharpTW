namespace LineageServer.Server.Model
{
	class L1ObjectAmount<T>
	{
		private readonly T _obj;
		private readonly int _amount;

		public L1ObjectAmount(T obj, int amount)
		{
			_obj = obj;
			_amount = amount;
		}

		public virtual T Object
		{
			get
			{
				return _obj;
			}
		}

		public virtual int Amount
		{
			get
			{
				return _amount;
			}
		}
	}

}