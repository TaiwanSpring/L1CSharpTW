using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.poison;
using LineageServer.Server.Storage;

namespace LineageServer.Server.Model.trap
{
	class L1PoisonTrap : L1Trap
	{
		private readonly string _type;
		private readonly int _delay;
		private readonly int _time;
		private readonly int _damage;

		public L1PoisonTrap(ITrapStorage storage) : base(storage)
		{

			_type = storage.getString("poisonType");
			_delay = storage.getInt("poisonDelay");
			_time = storage.getInt("poisonTime");
			_damage = storage.getInt("poisonDamage");
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
			sendEffect(trapObj);

			if (_type.Equals("d"))
			{
				L1DamagePoison.doInfection(trodFrom, trodFrom, _time, _damage);
			}
			else if (_type.Equals("s"))
			{
				L1SilencePoison.doInfection(trodFrom);
			}
			else if (_type.Equals("p"))
			{
				L1ParalysisPoison.doInfection(trodFrom, _delay, _time);
			}
		}
	}

}