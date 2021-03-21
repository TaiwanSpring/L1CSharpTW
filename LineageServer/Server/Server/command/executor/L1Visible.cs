using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Visible : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                pc.GmInvis = false;
                pc.sendPackets(new S_Invis(pc.Id, 0));
                pc.broadcastPacket(new S_OtherCharPacks(pc));
                pc.sendPackets(new S_SystemMessage("隱形狀態解除。"));
            }
            catch (Exception e)
            {
                pc.sendPackets(new S_SystemMessage(e.Message));
            }
        }
    }

}