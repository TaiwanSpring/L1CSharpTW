using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Model.monitor
{
	class L1PcAutoUpdate : L1PcMonitor
	{

		public L1PcAutoUpdate(int oId) : base(oId)
		{
		}

		public override void execTask(L1PcInstance pc)
		{
			pc.updateObject();
		}
	}

}