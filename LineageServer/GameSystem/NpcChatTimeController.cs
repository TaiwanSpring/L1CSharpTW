using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Threading;
namespace LineageServer.Server
{
    /// <summary>
    /// Npc聊天?
    /// </summary>
    public class NpcChatTimeController : IRunnable
    {
        private static ILogger _log = Logger.GetLogger(nameof(NpcChatTimeController));

        private static NpcChatTimeController _instance;

        public static NpcChatTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NpcChatTimeController();
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
                    checkNpcChatTime(); // 檢查開始聊天時間
                    Thread.Sleep(60000);
                }
            }
            catch (Exception e1)
            {
                _log.Warning(e1.Message);
            }
        }

        private void checkNpcChatTime()
        {
            foreach (L1NpcChat npcChat in NpcChatTable.Instance.AllGameTime)
            {
                if (isChatTime(npcChat.GameTime))
                {
                    int npcId = npcChat.NpcId;
                    foreach (GameObject obj in L1World.Instance.Object)
                    {
                        if (obj is L1NpcInstance npc)
                        {
                            if (npc.NpcTemplate.get_npcId() == npcId)
                            {
                                npc.startChat(L1NpcInstance.CHAT_TIMING_GAME_TIME);
                            }
                        }
                    }
                }
            }
        }

        private bool isChatTime(int chatTime)
        {
            return ((DateTime.Now.Hour * 100) + DateTime.Now.Minute) == chatTime;
        }
    }
}