using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.trap;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1ResetTrap : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            L1WorldTraps.Instance.resetAllTraps();
            pc.sendPackets(new S_SystemMessage("陷阱已被重新分配"));
        }
    }

}