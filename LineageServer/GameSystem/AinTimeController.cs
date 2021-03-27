using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Threading;

namespace LineageServer.Server
{
    /// <summary>
    /// 殷海薩的祝福 時間控制
    /// </summary>
    public class AinTimeController : IRunnable
    {
        private static ILogger _log = Logger.GetLogger(nameof(AinTimeController));

        private static AinTimeController _instance;

        public static AinTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AinTimeController();
                }
                return _instance;
            }
        }

        public object RealTime { get; private set; }

        public void run()
        {
            try
            {
                while (true)
                {
                    checkAinTime();
                    Thread.Sleep(60 * 1000);
                }
            }
            catch (Exception e1)
            {
                _log.Warning(e1.Message);
            }
        }

        private void checkAinTime()
        {
            DateTime now = DateTime.Now;
            int nowTime = (now.Hour * 100) + now.Minute;

            int ainTime = Config.RATE_AIN_TIME; // 時間比例
            int ainMaxPercent = Config.RATE_MAX_CHARGE_PERCENT; // 殷海薩的祝福 百分比
            if (nowTime % ainTime == 0)
            {
                foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
                {
                    if (pc.Level >= 49)
                    {
                        // 等級限制
                        if (pc.AinPoint < ainMaxPercent && pc.Map.isSafetyZone(pc.Location))
                        {
                            // 限制最高點數
                            pc.AinPoint = pc.AinPoint + 1; // 安全區域點數 +1
                            pc.AinZone = 1;
                            pc.sendPackets(new S_SkillIconExp(pc.AinPoint));
                        }
                        else
                        {
                            pc.AinZone = 0;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

}