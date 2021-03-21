using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Speed : IL1CommandExecutor
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