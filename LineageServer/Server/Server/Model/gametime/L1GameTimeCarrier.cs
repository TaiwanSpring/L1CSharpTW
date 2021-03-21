using System;

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
namespace LineageServer.Server.Server.Model.gametime
{

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_GameTime = LineageServer.Server.Server.serverpackets.S_GameTime;

	public class L1GameTimeCarrier : TimerTask
	{
		private static readonly Timer _timer = new Timer();
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
			_timer.scheduleAtFixedRate(this, 0, 500);
		}

		public virtual void stop()
		{
			cancel();
		}
	}

}