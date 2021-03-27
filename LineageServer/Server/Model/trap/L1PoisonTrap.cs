/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Model.trap
{
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1DamagePoison = LineageServer.Server.Model.poison.L1DamagePoison;
	using L1ParalysisPoison = LineageServer.Server.Model.poison.L1ParalysisPoison;
	using L1SilencePoison = LineageServer.Server.Model.poison.L1SilencePoison;
	using TrapStorage = LineageServer.Server.storage.TrapStorage;

	public class L1PoisonTrap : L1Trap
	{
		private readonly string _type;
		private readonly int _delay;
		private readonly int _time;
		private readonly int _damage;

		public L1PoisonTrap(TrapStorage storage) : base(storage)
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