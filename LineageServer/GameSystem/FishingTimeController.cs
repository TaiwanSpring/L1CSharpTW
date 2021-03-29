using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace LineageServer.Server
{
    /// <summary>
    /// 釣魚
    /// </summary>
    class FishingTimeController : IRunnable
    {
        private static FishingTimeController _instance;

        private readonly HashSet<L1PcInstance> _fishingList = new HashSet<L1PcInstance>();

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

        public void run()
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
                long currentTime = DateTime.Now.Ticks;

                foreach (var item in _fishingList.ToArray())
                {
                    L1PcInstance pc = item;
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
            int[] fish = new int[] { 41296, 41297, 41298, 41299, 41300, 41301, 41302, 41303, 41304, 41305, 41306, 41307, 21051, 21052, 21053, 21054, 21055, 21056, 21140, 21141, 41252, 46001, 47104 };
            int[] random = new int[] { 20, 40, 60, 80, 100, 110, 120, 130, 140, 145, 150, 155, 160, 165, 170, 175, 180, 185, 190, 195, 198, 201, 204 };
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
                { 
                    // 負重過重，結束釣魚
                    stopFishing(pc);
                    item.startItemOwnerTimer(pc);
                    L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
                    return;
                }
            }
            else
            {
                // 結束釣魚
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
                long fishTime = DateTime.Now.Ticks + 10000 + RandomHelper.Next(5) * 1000;
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