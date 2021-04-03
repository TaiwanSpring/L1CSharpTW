using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System.Threading;
namespace LineageServer.Server.Model
{
    class L1ItemDelay
    {

        private L1ItemDelay()
        {
        }

        internal class ItemDelayTimer : IRunnable
        {
            internal int _delayId;

            internal L1Character _cha;

            public ItemDelayTimer(L1Character cha, int id)
            {
                _cha = cha;
                _delayId = id;
            }

            public void run()
            {
                stopDelayTimer(_delayId);
            }

            public virtual void stopDelayTimer(int delayId)
            {
                _cha.removeItemDelay(delayId);
            }
        }

        internal class TeleportUnlockTimer : IRunnable
        {
            internal L1PcInstance _pc;

            public TeleportUnlockTimer(L1PcInstance pc)
            {
                _pc = pc;
            }

            public void run()
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
                delayId = ((L1EtcItem)item.Item).get_delayid();
                delayTime = ((L1EtcItem)item.Item).get_delaytime();
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
            Container.Instance.Resolve<ITaskController>().execute(timer, delayTime);

        }

        public static void teleportUnlock(L1PcInstance pc, L1ItemInstance item)
        {
            int delayTime = ((L1EtcItem)item.Item).get_delaytime();
            TeleportUnlockTimer timer = new TeleportUnlockTimer(pc);
            Container.Instance.Resolve<ITaskController>().execute(timer, delayTime);
        }

    }

}