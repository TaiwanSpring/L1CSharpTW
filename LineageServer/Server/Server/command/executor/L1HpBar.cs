using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.skill;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1HpBar : IL1CommandExecutor
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

                foreach (L1Object obj in pc.KnownObjects)
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

        public static bool isHpBarTarget(L1Object obj)
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