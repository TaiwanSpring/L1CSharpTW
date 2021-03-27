using LineageServer.Server.Templates;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model.Instance
{
    class L1FurnitureInstance : L1NpcInstance
    {

        private int _itemObjId;

        public L1FurnitureInstance(L1Npc template) : base(template)
        {
        }

        public override void onAction(L1PcInstance player)
        {
        }

        public override void deleteMe()
        {
            _destroyed = true;
            if (Inventory != null)
            {
                Inventory.clearItems();
            }
            L1World.Instance.removeVisibleObject(this);
            L1World.Instance.removeObject(this);
            foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
            {
                pc.removeKnownObject(this);
                pc.sendPackets(new S_RemoveObject(this));
            }
            removeAllKnownObjects();
        }

        public virtual int ItemObjId
        {
            get
            {
                return _itemObjId;
            }
            set
            {
                _itemObjId = value;
            }
        }
    }

}