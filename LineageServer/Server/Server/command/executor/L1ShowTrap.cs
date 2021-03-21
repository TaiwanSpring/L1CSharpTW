using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1ShowTrap : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            if (arg == "on")
            {
                pc.setSkillEffect(L1SkillId.GMSTATUS_SHOWTRAPS, 0);
            }
            else if (arg == "off")
            {
                pc.removeSkillEffect(L1SkillId.GMSTATUS_SHOWTRAPS);

                foreach (L1Object obj in pc.KnownObjects)
                {
                    if (obj is L1TrapInstance)
                    {
                        pc.removeKnownObject(obj);
                        pc.sendPackets(new S_RemoveObject(obj));
                    }
                }
            }
            else
            {
                pc.sendPackets(new S_SystemMessage("請輸入: " + cmdName + " on|off 。"));
            }
        }
    }

}