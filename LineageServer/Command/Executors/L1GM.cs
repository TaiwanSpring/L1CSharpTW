using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Command.Executors
{
    class L1GM : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            pc.Gm = !pc.Gm;
            pc.sendPackets(new S_SystemMessage("setGm = " + pc.Gm));
        }
    }
}