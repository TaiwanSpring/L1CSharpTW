using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Command.Executors
{
    class L1Invisible : ILineageCommand
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