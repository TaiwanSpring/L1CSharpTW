using System;
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
namespace LineageServer.Server.Server.Model
{
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using WarTimeController = LineageServer.Server.Server.WarTimeController;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_PinkName = LineageServer.Server.Server.serverpackets.S_PinkName;

	// Referenced classes of package l1j.server.server.model:
	// L1PinkName

	public class L1PinkName
	{
		private L1PinkName()
		{
		}

		internal class PinkNameTimer : IRunnableStart
		{
			internal L1PcInstance _attacker = null;

			public PinkNameTimer(L1PcInstance attacker)
			{
				_attacker = attacker;
			}

			public override void run()
			{
				for (int i = 0; i < 180; i++)
				{
					try
					{
						Thread.Sleep(1000);
					}
					catch (Exception)
					{
						break;
					}
					// 死亡、または、相手を倒して赤ネームになったら終了
					if (_attacker.Dead)
					{
						// setPinkName(false);はL1PcInstance#death()で行う
						break;
					}
					if (_attacker.Lawful < 0)
					{
						_attacker.PinkName = false;
						break;
					}
				}
				stopPinkName(_attacker);
			}

			internal virtual void stopPinkName(L1PcInstance attacker)
			{
				attacker.sendPackets(new S_PinkName(attacker.Id, 0));
				attacker.broadcastPacket(new S_PinkName(attacker.Id, 0));
				attacker.PinkName = false;
			}
		}

		public static void onAction(L1PcInstance pc, L1Character cha)
		{
			if ((pc == null) || (cha == null))
			{
				return;
			}

			if (!(cha is L1PcInstance))
			{
				return;
			}
			L1PcInstance attacker = (L1PcInstance) cha;
			if (pc.Id == attacker.Id)
			{
				return;
			}
			if (attacker.FightId == pc.Id)
			{
				return;
			}

			bool isNowWar = false;
			int castleId = L1CastleLocation.getCastleIdByArea(pc);
			if (castleId != 0)
			{ // 旗内に居る
				isNowWar = WarTimeController.Instance.isNowWar(castleId);
			}

			if ((pc.Lawful >= 0) && !pc.PinkName && (attacker.Lawful >= 0) && !attacker.PinkName)
			{
				if ((pc.ZoneType == 0) && (attacker.ZoneType == 0) && (isNowWar == false))
				{
					attacker.PinkName = true;
					attacker.sendPackets(new S_PinkName(attacker.Id, 180));
					if (!attacker.GmInvis)
					{
						attacker.broadcastPacket(new S_PinkName(attacker.Id, 180));
					}
					PinkNameTimer pink = new PinkNameTimer(attacker);
					GeneralThreadPool.Instance.execute(pink);
				}
			}
		}
	}

}