using LineageServer.Models;
using LineageServer.Server.Server.Model.Instance;
using System.Threading;
namespace LineageServer.Server.Server.Model.monitor
{
	class L1PcGhostMonitor : L1PcMonitor
	{

		public L1PcGhostMonitor(int oId) : base(oId)
		{
		}

		public override void execTask(L1PcInstance pc)
		{
			// endGhostの実行時間が影響ないように
			RunnableExecuter.Instance.execute(new L1PcMonitorAnonymousInnerClass(this, pc.Id));
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