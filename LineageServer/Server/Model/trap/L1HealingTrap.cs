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
	using TrapStorage = LineageServer.Server.Storage.TrapStorage;
	using Dice = LineageServer.Utils.Dice;

	public class L1HealingTrap : L1Trap
	{
		private readonly Dice _dice;
		private readonly int _base;
		private readonly int _diceCount;

		public L1HealingTrap(TrapStorage storage) : base(storage)
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