using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
namespace LineageServer.Server.Model.monitor
{
	class L1PcHellMonitor : L1PcMonitor
	{
		public L1PcHellMonitor(int oId) : base(oId)
		{
		}

		public override void execTask(L1PcInstance pc)
		{
			if (pc.Dead)
			{ // 死んでいたらカウントダウンしない
				return;
			}
			pc.HellTime = pc.HellTime - 1;
			if (pc.HellTime <= 0)
			{
				// endHellの実行時間が影響ないように
				Container.Instance.Resolve<ITaskController>().execute(new L1PcMonitorAnonymousInnerClass(this, pc.Id));
			}
		}

		private class L1PcMonitorAnonymousInnerClass : L1PcMonitor
		{
			private readonly L1PcHellMonitor outerInstance;

			public L1PcMonitorAnonymousInnerClass(L1PcHellMonitor outerInstance, int getId) : base(getId)
			{
				this.outerInstance = outerInstance;
			}

			public override void execTask(L1PcInstance pc)
			{
				pc.endHell();
			}
		}
	}
}