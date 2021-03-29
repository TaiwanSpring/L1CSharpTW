using LineageServer.Models;
using LineageServer.Server.Storage.mysql;

namespace LineageServer.Utils
{
    //public class MysqlAutoBackupTimer
    //{
    //    /// <summary>
    //    /// Mysql自動備份程序計時器
    //    /// </summary>
    //    public static void TimerStart()
    //    {
    //        lock (typeof(MysqlAutoBackupTimer))
    //        {
    //            int minutes = Config.MysqlAutoBackup;
    //            if (minutes == 0)
    //            {
    //                return;
    //            }
    //            RunnableExecuter.Instance.scheduleAtFixedRate(new MysqlAutoBackup(), 60000, minutes * 60000); // 開機1分鐘後,每隔設定之時間備份一次
    //        }
    //    }
    //}

}