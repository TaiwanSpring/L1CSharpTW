using LineageServer.Server.Model.Instance;
using LineageServer.Server.Storage;
using LineageServer.Utils;

namespace LineageServer.Server.Model.trap
{
	class L1DamageTrap : L1Trap
	{
		private readonly Dice _dice;
		private readonly int _base;
		private readonly int _diceCount;

		public L1DamageTrap(ITrapStorage storage) : base(storage)
		{

			_dice = new Dice(storage.getInt("dice"));
			_base = storage.getInt("base");
			_diceCount = storage.getInt("diceCount");
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
			sendEffect(trapObj);

			int dmg = _dice.roll(_diceCount) + _base;

			trodFrom.receiveDamage(trodFrom, dmg, false);
		}
	}

}