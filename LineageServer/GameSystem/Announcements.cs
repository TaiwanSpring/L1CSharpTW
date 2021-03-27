using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Collections.Generic;
using System.IO;
namespace LineageServer.Server
{
    /// <summary>
    /// 好像是歡迎公告
    /// </summary>
    class Announcements
    {
        private static Announcements _instance;

        private IList<string> _announcements;

        private Announcements()
        {
            loadAnnouncements();
        }

        public static Announcements Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Announcements();
                }

                return _instance;
            }
        }

        private void loadAnnouncements()
        {
            FileInfo fileInfo = new FileInfo("data/announcements.txt");
            if (File.Exists(fileInfo.FullName))
            {
                _announcements = File.ReadAllLines(fileInfo.FullName);
            }
        }

        public virtual void showAnnouncements(L1PcInstance showTo)
        {
            foreach (string msg in _announcements)
            {
                showTo.sendPackets(new S_SystemMessage(msg));
            }
        }

        public virtual void announceToAll(string msg)
        {
            L1World.Instance.broadcastServerMessage(msg);
        }
    }
}