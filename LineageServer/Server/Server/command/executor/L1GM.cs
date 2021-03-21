using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1GM : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            pc.Gm = !pc.Gm;
            pc.sendPackets(new S_SystemMessage("setGm = " + pc.Gm));
        }
    }
}