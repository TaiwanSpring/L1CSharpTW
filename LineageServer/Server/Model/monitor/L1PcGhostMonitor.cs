using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
namespace LineageServer.Server.Model.monitor
{
    class L1PcGhostMonitor : L1PcMonitor
    {

        public L1PcGhostMonitor(int oId) : base(oId)
        {
        }

        public override void execTask(L1PcInstance pc)
        {
            // endGhostの実行時間が影響ないように
            Container.Instance.Resolve<ITaskController>().execute(new L1PcMonitorAnonymousInnerClass(this, pc.Id));
        }

        private class L1PcMonitorAnonymousInnerClass : L1PcMonitor
        {
            private readonly L1PcGhostMonitor outerInstance;

            public L1PcMonitorAnonymousInnerClass(L1PcGhostMonitor outerInstance, int getId) : base(getId)
            {
                this.outerInstance = outerInstance;
            }

            public override void execTask(L1PcInstance pc)
            {
                pc.endGhost();
            }
        }
    }

}