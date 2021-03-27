using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1HpBar : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            if (arg.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                pc.setSkillEffect(L1SkillId.GMSTATUS_HPBAR, 0);
            }
            else if (arg.Equals("off", StringComparison.OrdinalIgnoreCase))
            {
                pc.removeSkillEffect(L1SkillId.GMSTATUS_HPBAR);

                foreach (GameObject obj in pc.KnownObjects)
                {
                    if (isHpBarTarget(obj))
                    {
                        pc.sendPackets(new S_HPMeter(obj.Id, 0xFF));
                    }
                }
            }
            else
            {
                pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " on|off 。"));
            }
        }

        public static bool isHpBarTarget(GameObject obj)
        {
            if (obj is L1MonsterInstance)
            {
                return true;
            }
            if (obj is L1PcInstance)
            {
                return true;
            }
            if (obj is L1SummonInstance)
            {
                return true;
            }
            if (obj is L1PetInstance)
            {
                return true;
            }
            return false;
        }
    }

}