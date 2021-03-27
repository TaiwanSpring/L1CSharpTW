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
namespace LineageServer.Server.Model
{

	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	public class L1EquipmentTimer : TimerTask
	{
		public L1EquipmentTimer(L1PcInstance pc, L1ItemInstance item)
		{
			_pc = pc;
			_item = item;
		}

		public override void run()
		{
			if ((_item.RemainingTime - 1) > 0)
			{
				_item.RemainingTime = _item.RemainingTime - 1;
				_pc.Inventory.updateItem(_item, L1PcInventory.COL_REMAINING_TIME);
			}
			else
			{
				_pc.Inventory.removeItem(_item, 1);
				cancel();
			}
		}

		private readonly L1PcInstance _pc;

		private readonly L1ItemInstance _item;
	}

}