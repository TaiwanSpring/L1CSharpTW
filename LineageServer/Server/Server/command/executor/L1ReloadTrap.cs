
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.trap;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1ReloadTrap : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            L1WorldTraps.reloadTraps();
            pc.sendPackets(new S_SystemMessage("已重新讀取陷阱資料"));
        }
    }

}