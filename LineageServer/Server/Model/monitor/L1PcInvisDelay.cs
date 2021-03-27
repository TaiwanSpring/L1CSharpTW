using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model.monitor
{
	class L1PcInvisDelay : L1PcMonitor
	{

		public L1PcInvisDelay(int oId) : base(oId)
		{
		}

		public override void execTask(L1PcInstance pc)
		{
			pc.addInvisDelayCounter(-1);
		}
	}
}