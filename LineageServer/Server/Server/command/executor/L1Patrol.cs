using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1Patrol : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            pc.sendPackets(new S_PacketBox(S_PacketBox.CALL_SOMETHING));
        }
    }
}