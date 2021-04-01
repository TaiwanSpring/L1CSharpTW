using LineageServer.Server.Model.Instance;
using LineageServer.Server.Storage;
using LineageServer.Utils;

namespace LineageServer.Server.Model.trap
{
	class L1HealingTrap : L1Trap
	{
		private readonly Dice _dice;
		private readonly int _base;
		private readonly int _diceCount;

		public L1HealingTrap(ITrapStorage storage) : base(storage)
		{

			_dice = new Dice(storage.getInt("dice"));
			_base = storage.getInt("base");
			_diceCount = storage.getInt("diceCount");
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
			sendEffect(trapObj);

			int pt = _dice.roll(_diceCount) + _base;

			trodFrom.healHp(pt);
		}
	}

}