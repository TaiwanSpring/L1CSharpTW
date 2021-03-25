using LineageServer.Models;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Model.Gametime
{
	class L1GameTimeCarrier : TimerTask
	{
		private L1PcInstance _pc;

		public L1GameTimeCarrier(L1PcInstance pc)
		{
			_pc = pc;
		}

		public override void run()
		{
			try
			{
				if (_pc.NetConnection == null)
				{
					cancel();
					return;
				}

				int serverTime = L1GameTimeClock.Instance.currentTime().Seconds;
				if (serverTime % 300 == 0)
				{
					_pc.sendPackets(new S_GameTime(serverTime));
				}
			}
			catch (Exception)
			{
				// ignore
			}
		}

		public virtual void start()
		{
			RunnableExecuter.Instance.scheduleAtFixedRate(this, 0, 500);
		}

		public virtual void stop()
		{
			cancel();
		}
	}

}