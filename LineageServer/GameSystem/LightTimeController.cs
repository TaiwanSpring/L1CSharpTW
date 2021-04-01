using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.Instance;
using System;
using System.Threading;
namespace LineageServer.Server
{
    public class LightTimeController : IRunnable
    {
        private static LightTimeController _instance;

        private bool isSpawn = false;

        public static LightTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LightTimeController();
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
                    checkLightTime();
                    Thread.Sleep(60000);
                }
            }
            catch (Exception)
            {
            }
        }

        private void checkLightTime()
        {
            int serverTime = L1GameTimeClock.Instance.CurrentTime().Seconds;
            int nowTime = serverTime % 86400;
            if ((nowTime >= ((5 * 3600) + 3300)) && (nowTime < ((17 * 3600) + 3300)))
            {
                // 5:55~17:55
                if (isSpawn)
                {
                    isSpawn = false;
                    foreach (GameObject item in Container.Instance.Resolve<IGameWorld>().Object)
                    {
                        if (item is L1FieldObjectInstance npc)
                        {
                            if (((npc.NpcTemplate.get_npcId() == 81177) || (npc.NpcTemplate.get_npcId() == 81178) || (npc.NpcTemplate.get_npcId() == 81179) || (npc.NpcTemplate.get_npcId() == 81180) || (npc.NpcTemplate.get_npcId() == 81181)) && ((npc.MapId == 0) || (npc.MapId == 4)))
                            {
                                npc.deleteMe();
                            }
                        }
                    }
                }
            }
            else if (((nowTime >= ((17 * 3600) + 3300)) && (nowTime <= 24 * 3600)) || ((nowTime >= 0 * 3600) && (nowTime < ((5 * 3600) + 3300))))
            {
                // 17:55~24:00,0:00~5:55
                if (!isSpawn)
                {
                    isSpawn = true;
                }
            }
        }

    }

}