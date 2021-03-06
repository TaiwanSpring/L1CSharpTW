using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 循環公告 </summary>
/// <returns> bymca </returns>

namespace LineageServer.Server
{
    public class Announcecycle : IGameComponent
    {
        private IList<string> _Announcecycle = new List<string>(); //TODO 加入循環公告元件型字串陣列組

        private int _Announcecyclesize = 0;
        /// <summary>
        /// 讀取循環公告
        /// </summary>
        public virtual void loadAnnouncecycle()
        {
            _Announcecycle.Clear();
            FileInfo file = new FileInfo("data/Announcecycle.txt");
            if (file.Exists)
            {
                readFromDiskmulti(file);
                doAnnouncecycle(); // 若有載入檔案即開始運作循環公告執行緒
            }
        }
        /// <summary>
        /// 循環公告多型法讀取應用
        /// </summary>
        /// <param name="file"></param>
        private void readFromDiskmulti(FileInfo file)
        {
            _Announcecycle = File.ReadAllLines(file.FullName);
        }

        public virtual void doAnnouncecycle()
        {
            AnnouncTask rs = new AnnouncTask(this); // 建立執行緒
            Container.Instance.Resolve<ITaskController>()
                .execute(rs, 3 * 60 * 1000, 60 * 1000 * Config.Show_Announcecycle_Time);
            // 10分鐘公告一次(60秒*1分*1000毫秒)
        }
        private void ShowAnnounceToAll(string msg)
        {
            ICollection<L1PcInstance> allpc = Container.Instance.Resolve<IGameWorld>().AllPlayers;
            foreach (L1PcInstance pc in allpc)
            {
                pc.sendPackets(new S_SystemMessage(msg));
            }
        }

        public void Initialize()
        {
            if (Config.Use_Show_Announcecycle)
            {
                loadAnnouncecycle();
            }
        }


        /// <summary>
        /// The task launching the function doAnnouncCycle() 
        /// </summary>
        internal class AnnouncTask : TimerTask
        {
            private readonly Announcecycle outerInstance;

            public AnnouncTask(Announcecycle outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void run()
            {
                outerInstance.ShowAnnounceToAll(outerInstance._Announcecycle[outerInstance._Announcecyclesize]); // 輪迴式公告發布
                outerInstance._Announcecyclesize++;
                if (outerInstance._Announcecyclesize >= outerInstance._Announcecycle.Count)
                {
                    outerInstance._Announcecyclesize = 0;
                }
            }
        }
    }
}