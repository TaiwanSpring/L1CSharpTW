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
namespace LineageServer.Server.Model
{
	using ClientThread = LineageServer.Server.ClientThread;
	using RunnableExecuter = LineageServer.Server.RunnableExecuter;
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_Paralysis = LineageServer.Serverpackets.S_Paralysis;
	using L1EtcItem = LineageServer.Server.Templates.L1EtcItem;

	// Referenced classes of package l1j.server.server.model:
	// L1ItemDelay

	public class L1ItemDelay
	{

		private L1ItemDelay()
		{
		}

		internal class ItemDelayTimer : IRunnableStart
		{
			internal int _delayId;

			internal L1Character _cha;

			public ItemDelayTimer(L1Character cha, int id)
			{
				_cha = cha;
				_delayId = id;
			}

			public override void run()
			{
				stopDelayTimer(_delayId);
			}

			public virtual void stopDelayTimer(int delayId)
			{
				_cha.removeItemDelay(delayId);
			}
		}

		internal class TeleportUnlockTimer : IRunnableStart
		{
			internal L1PcInstance _pc;

			public TeleportUnlockTimer(L1PcInstance pc)
			{
				_pc = pc;
			}

			public override void run()
			{
				_pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
			}
		}

		public static void onItemUse(ClientThread client, L1ItemInstance item)
		{
			int delayId = 0;
			int delayTime = 0;

			L1PcInstance pc = client.ActiveChar;

			if (item.Item.Type2 == 0)
			{
				// 種別：一般道具
				delayId = ((L1EtcItem) item.Item).get_delayid();
				delayTime = ((L1EtcItem) item.Item).get_delaytime();
			}
			else if (item.Item.Type2 == 1)
			{
				// 種別：武器
				return;
			}
			else if (item.Item.Type2 == 2)
			{
				// 種別：防具

				if ((item.Item.ItemId == 20077) || (item.Item.ItemId == 20062) || (item.Item.ItemId == 120077))
				{
					// 隱身防具
					if (item.Equipped && !pc.Invisble)
					{
						pc.beginInvisTimer();
					}
				}
				else
				{
					return;
				}
			}

			ItemDelayTimer timer = new ItemDelayTimer(pc, delayId);
			pc.addItemDelay(delayId, timer);
			RunnableExecuter.Instance.schedule(timer, delayTime);

		}

		public static void teleportUnlock(L1PcInstance pc, L1ItemInstance item)
		{
			int delayTime = ((L1EtcItem) item.Item).get_delaytime();
			TeleportUnlockTimer timer = new TeleportUnlockTimer(pc);
			RunnableExecuter.Instance.schedule(timer, delayTime);
		}

	}

}