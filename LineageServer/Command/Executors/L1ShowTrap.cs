using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Command.Executors
{
    class L1ShowTrap : ILineageCommand
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

                foreach (GameObject obj in pc.KnownObjects)
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