using System.Threading;

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
namespace LineageServer.Server.Server.Model.monitor
{
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class L1PcHellMonitor : L1PcMonitor
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
				ThreadStart r = new L1PcMonitorAnonymousInnerClass(this, pc.Id);
				GeneralThreadPool.Instance.execute(r);
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