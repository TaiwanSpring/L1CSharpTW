using LineageServer.Server.Server;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Interfaces;
using System;
namespace LineageServer.Server.Console
{
    /// <summary>
    /// cmd 互動命令 處理程序
    /// </summary>
    class ConsoleProcess : IRunnable
    {
        /// <summary>
        /// 開機後是否開啟此功能 
        /// </summary>
        private bool onStarup = true;
        /// <summary>
        /// 程序是否繼續 </summary>
        private bool stillrun = true;

        /// <summary>
        /// 指令執行(有引數)
        /// </summary>
        /// <param name="cmd">
        ///            指令名稱 </param>
        /// <param name="line">
        ///            指令引數 </param>
        private void execute(string cmd, string line)
        {
            if (string.ReferenceEquals(cmd, null) || string.ReferenceEquals(line, null))
            {
                System.Console.WriteLine("error, please input cmd words or args.");
                return;
            }
            if (cmd.Equals("chat", StringComparison.OrdinalIgnoreCase))
            { // cmd與遊戲內對話功能
                L1World.Instance.broadcastPacketToAll(new S_SystemMessage("\\f3" + "[系統管理員]" + line));
                System.Console.WriteLine("[系統管理員]" + line);
            }
            else if (cmd.Equals("shutdown", StringComparison.OrdinalIgnoreCase))
            {
                int sec = int.Parse(line);
                if (sec > 0)
                {
                    GameServer.Instance.shutdownWithCountdown(sec);
                }
                if (sec <= 0)
                {
                    GameServer.Instance.shutdown();
                }
            }
            else
            {
                System.Console.WriteLine("error, doesn't have the command.");
                return;
            }

        }

        /// <summary>
        /// 指令執行
        /// </summary>
        /// <param name="cmd">
        ///            指令名稱 </param>
        private void execute(string cmd)
        {
            if (string.ReferenceEquals(cmd, null))
            {
                System.Console.WriteLine("error, please input cmd words.");
                return;
            }
            if (cmd.Equals("lookup", StringComparison.OrdinalIgnoreCase))
            { // cmd查看遊戲內對話功能
              // TODO 開啟另一個視窗並顯示遊戲內對話
            }
            else
            {
                System.Console.WriteLine("error, doesn't have the command.");
                return;
            }
        }

        public void run()
        {
            while (onStarup && stillrun)
            {
                string action = System.Console.ReadLine();
                if (action.Contains(' '))
                {
                    string[] word = action.Split(' ');
                    if (word.Length == 1)
                    {
                        execute(word[0]);
                    }
                    if (word.Length == 2)
                    {
                        execute(word[0], word[1]);
                    }
                }
                else
                {
                    execute(action);
                }
                System.Console.WriteLine("→提示: 互動指令聽取中..." + "\n" + ">");
            }
        }
    }

}