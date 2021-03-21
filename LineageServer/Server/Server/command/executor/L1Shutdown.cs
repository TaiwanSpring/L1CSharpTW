using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Shutdown : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                if (arg == "now")
                {
                    GameServer.Instance.shutdown();
                }
                else if (arg == "abort")
                {
                    GameServer.Instance.abortShutdown();
                }
                else
                {
                    int sec = Math.Max(5, int.Parse(arg));
                    GameServer.Instance.shutdownWithCountdown(sec);
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入: .shutdown sec|now|abort 。"));
            }
        }
    }
}