using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Command.Executors
{
    class L1Patrol : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            pc.sendPackets(new S_PacketBox(S_PacketBox.CALL_SOMETHING));
        }
    }
}