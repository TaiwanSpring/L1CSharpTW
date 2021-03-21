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
namespace LineageServer.Server.Server.Model.trap
{
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using TrapStorage = LineageServer.Server.Server.storage.TrapStorage;
	using Dice = LineageServer.Server.Server.utils.Dice;

	public class L1DamageTrap : L1Trap
	{
		private readonly Dice _dice;
		private readonly int _base;
		private readonly int _diceCount;

		public L1DamageTrap(TrapStorage storage) : base(storage)
		{

			_dice = new Dice(storage.getInt("dice"));
			_base = storage.getInt("base");
			_diceCount = storage.getInt("diceCount");
		}

		public override void onTrod(L1PcInstance trodFrom, L1Object trapObj)
		{
			sendEffect(trapObj);

			int dmg = _dice.roll(_diceCount) + _base;

			trodFrom.receiveDamage(trodFrom, dmg, false);
		}
	}

}