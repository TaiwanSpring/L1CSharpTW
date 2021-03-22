using System;
using System.Collections.Generic;
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
namespace LineageServer.Server.Server
{

	using Config = LineageServer.Server.Config;
	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_CharVisualUpdate = LineageServer.Server.Server.serverpackets.S_CharVisualUpdate;
	using S_OwnCharStatus = LineageServer.Server.Server.serverpackets.S_OwnCharStatus;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using Random = LineageServer.Server.Server.utils.Random;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class FishingTimeController : IRunnableStart
	{
		private static FishingTimeController _instance;

		private readonly IList<L1PcInstance> _fishingList = Lists.newList();

		public static FishingTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new FishingTimeController();
				}
				return _instance;
			}
		}

		public override void run()
		{
			try
			{
				while (true)
				{
					Thread.Sleep(300);
					fishing();
				}
			}
			catch (Exception)
			{
			}
		}

		public virtual void addMember(L1PcInstance pc)
		{
			if ((pc == null) || _fishingList.Contains(pc))
			{
				return;
			}
			_fishingList.Add(pc);
		}

		public virtual void removeMember(L1PcInstance pc)
		{
			if ((pc == null) || !_fishingList.Contains(pc))
			{
				return;
			}
			_fishingList.Remove(pc);
		}

		private void fishing()
		{
			if (_fishingList.Count > 0)
			{
				long currentTime = DateTimeHelper.CurrentUnixTimeMillis();
				for (int i = 0; i < _fishingList.Count; i++)
				{
					L1PcInstance pc = _fishingList[i];
					if (pc.Fishing)
					{ // 釣魚中
						long time = pc.FishingTime;
						if ((currentTime <= (time + 500)) && (currentTime >= (time - 500)) && !pc.FishingReady)
						{
							pc.FishingReady = true;
							finishFishing(pc);
						}
					}
				}
			}
		}

		// 釣魚完成
		private void finishFishing(L1PcInstance pc)
		{
			int chance = RandomHelper.Next(215) + 1;
			bool finish = false;
			int[] fish = new int[] {41296, 41297, 41298, 41299, 41300, 41301, 41302, 41303, 41304, 41305, 41306, 41307, 21051, 21052, 21053, 21054, 21055, 21056, 21140, 21141, 41252, 46001, 47104};
			int[] random = new int[] {20, 40, 60, 80, 100, 110, 120, 130, 140, 145, 150, 155, 160, 165, 170, 175, 180, 185, 190, 195, 198, 201, 204};
			for (int i = 0; i < fish.Length; i++)
			{
				if (random[i] > chance)
				{
					successFishing(pc, fish[i]);
					finish = true;
					break;
				}
			}
			if (!finish)
			{
				pc.sendPackets(new S_ServerMessage(1517)); // 沒有釣到魚。
				if (pc.FishingReady)
				{
					restartFishing(pc);
				}
			}
		}

		// 釣魚成功
		private void successFishing(L1PcInstance pc, int itemId)
		{
			L1ItemInstance item = ItemTable.Instance.createItem(itemId);
			if (item != null)
			{
				pc.sendPackets(new S_ServerMessage(403, item.Item.Name));
				pc.addExp((int)(2 * Config.RATE_XP));
				pc.sendPackets(new S_OwnCharStatus(pc));
				item.Count = 1;
				if (pc.Inventory.checkAddItem(item, 1) == L1Inventory.OK)
				{
					pc.Inventory.storeItem(item);
				}
				else
				{ // 負重過重，結束釣魚
					stopFishing(pc);
					item.startItemOwnerTimer(pc);
					L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
					return;
				}
			}
			else
			{ // 結束釣魚
				pc.sendPackets(new S_ServerMessage(1517)); // 沒有釣到魚。
				stopFishing(pc);
				return;
			}

			if (pc.FishingReady)
			{
				if (itemId == 47104)
				{
					pc.sendPackets(new S_ServerMessage(1739)); // 釣到了閃爍的鱗片，自動釣魚已停止。
					stopFishing(pc);
					return;
				}
				restartFishing(pc);
			}
		}

		// 重新釣魚
		private void restartFishing(L1PcInstance pc)
		{
			if (pc.Inventory.consumeItem(47103, 1))
			{ // 消耗餌，重新釣魚
				long fishTime = DateTimeHelper.CurrentUnixTimeMillis() + 10000 + RandomHelper.Next(5) * 1000;
				pc.FishingTime = fishTime;
				pc.FishingReady = false;
			}
			else
			{
				pc.sendPackets(new S_ServerMessage(1137)); // 釣魚需要有餌。
				stopFishing(pc);
			}
		}

		// 停止釣魚
		private void stopFishing(L1PcInstance pc)
		{
			pc.FishingTime = 0;
			pc.FishingReady = false;
			pc.Fishing = false;
			pc.sendPackets(new S_CharVisualUpdate(pc));
			pc.broadcastPacket(new S_CharVisualUpdate(pc));
			FishingTimeController.Instance.removeMember(pc);
		}
	}

}