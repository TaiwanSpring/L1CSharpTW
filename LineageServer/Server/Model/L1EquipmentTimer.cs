using LineageServer.Models;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model
{
    class L1EquipmentTimer : TimerTask
    {
        public L1EquipmentTimer(L1PcInstance pc, L1ItemInstance item)
        {
            _pc = pc;
            _item = item;
        }

        public void run()
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