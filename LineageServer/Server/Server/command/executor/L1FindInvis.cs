using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1FindInvis : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            if (arg.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                pc.setSkillEffect(L1SkillId.GMSTATUS_FINDINVIS, 0);
                pc.removeAllKnownObjects();
                pc.updateObject();
            }
            else if (arg.Equals("off", StringComparison.OrdinalIgnoreCase))
            {
                pc.removeSkillEffect(L1SkillId.GMSTATUS_FINDINVIS);
                foreach (L1PcInstance visible in L1World.Instance.getVisiblePlayer(pc))
                {
                    if (visible.Invisble)
                    {
                        pc.sendPackets(new S_RemoveObject(visible));
                    }
                }
            }
            else
            {
                pc.sendPackets(new S_SystemMessage(cmdName + "請輸入  on|off 。"));
            }
        }

    }

}