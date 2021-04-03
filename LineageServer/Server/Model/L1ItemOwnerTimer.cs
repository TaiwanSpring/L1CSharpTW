using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model
{
    class L1ItemOwnerTimer : TimerTask
    {
        public L1ItemOwnerTimer(L1ItemInstance item, int timeMillis)
        {
            _item = item;
            _timeMillis = timeMillis;
        }

        public override void run()
        {
            _item.ItemOwnerId = 0;
            cancel();
        }

        public virtual void begin()
        {
            Container.Instance.Resolve<ITaskController>().execute(this, _timeMillis);
        }

        private readonly L1ItemInstance _item;

        private readonly int _timeMillis;
    }

}