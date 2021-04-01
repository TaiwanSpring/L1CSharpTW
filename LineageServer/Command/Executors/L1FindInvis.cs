using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1FindInvis : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            if (arg == "on")
            {
                pc.setSkillEffect(L1SkillId.GMSTATUS_FINDINVIS, 0);
                pc.removeAllKnownObjects();
                pc.updateObject();
            }
            else if (arg == "off")
            {
                pc.removeSkillEffect(L1SkillId.GMSTATUS_FINDINVIS);
                foreach (L1PcInstance visible in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(pc))
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