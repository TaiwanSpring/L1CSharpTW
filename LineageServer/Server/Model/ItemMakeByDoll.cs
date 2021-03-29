using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
namespace LineageServer.Server.Model
{
    class ItemMakeByDoll : PcInstanceRunnableBase
    {
        private readonly L1PcInstance _pc;

        public ItemMakeByDoll(L1PcInstance pc) : base(pc)
        {
            _pc = pc;
        }

        public virtual void itemMake()
        {
            L1ItemInstance temp = ItemTable.Instance.createItem(L1MagicDoll.getMakeItemId(_pc));
            if (temp != null)
            {
                if (_pc.Inventory.checkAddItem(temp, 1) == L1Inventory.OK)
                {
                    L1ItemInstance item = _pc.Inventory.storeItem(temp.ItemId, 1);
                    _pc.sendPackets(new S_ServerMessage(403, item.Item.Name)); // 獲得%0%o 。
                }
            }
        }

        protected override void DoRun()
        {
            itemMake();
        }
    }
}