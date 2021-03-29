using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using System;
using System.Threading;
namespace LineageServer.Server
{
    public class UbTimeController : IRunnable
    {
        private static ILogger _log = Logger.GetLogger(nameof(UbTimeController));

        private static UbTimeController _instance;

        public static UbTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UbTimeController();
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
                    checkUbTime(); // 開始檢查無限大戰的時間
                    Thread.Sleep(15000);
                }
            }
            catch (Exception e1)
            {
                _log.Warning(e1.Message);
            }
        }

        private void checkUbTime()
        {
            foreach (L1UltimateBattle ub in UBTable.Instance.AllUb)
            {
                if (ub.checkUbTime() && !ub.Active)
                {
                    ub.start(); // 無限大戰開始
                }
            }
        }
    }
}