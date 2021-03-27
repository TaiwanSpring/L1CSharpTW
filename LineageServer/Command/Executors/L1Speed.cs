using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Speed : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                L1BuffUtil.haste(pc, 3600 * 1000);
                L1BuffUtil.brave(pc, 3600 * 1000);
                L1BuffUtil.thirdSpeed(pc);
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(".speed 指令錯誤"));
            }
        }
    }

}