using LineageServer.Models;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Model.Gametime
{
	class L1GameTimeCarrier : TimerTask
	{
		private L1PcInstance pc;

		public L1GameTimeCarrier(L1PcInstance pc)
		{
			this.pc = pc;
		}

		public override void run()
		{
			try
			{
				if (this.pc.NetConnection == null)
				{
					cancel();
					return;
				}

				pc.sendPackets(new S_GameTime(L1GameTimeClock.Instance.CurrentTime().Seconds));
			}
			catch (Exception)
			{
				// ignore
			}
		}

		public virtual void start()
		{
			RunnableExecuter.Instance.scheduleAtFixedRate(this, 0, 300 * 1000); // 300秒發一次
		}

		public virtual void stop()
		{
			cancel();
		}
	}

}