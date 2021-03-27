using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.trap;
using LineageServer.Serverpackets;

namespace LineageServer.Command.Executors
{
    class L1ResetTrap : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            L1WorldTraps.Instance.resetAllTraps();
            pc.sendPackets(new S_SystemMessage("陷阱已被重新分配"));
        }
    }

}