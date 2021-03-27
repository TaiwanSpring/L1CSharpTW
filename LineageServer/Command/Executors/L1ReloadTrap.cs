
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.trap;
using LineageServer.Serverpackets;

namespace LineageServer.Command.Executors
{
    class L1ReloadTrap : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            L1WorldTraps.reloadTraps();
            pc.sendPackets(new S_SystemMessage("已重新讀取陷阱資料"));
        }
    }

}