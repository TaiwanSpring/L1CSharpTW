using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1Invisible : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                pc.GmInvis = true;
                pc.sendPackets(new S_Invis(pc.Id, 1));
                pc.broadcastPacket(new S_RemoveObject(pc));
                pc.sendPackets(new S_SystemMessage("現在是隱身狀態。"));

            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 指令錯誤"));
            }
        }
    }
}