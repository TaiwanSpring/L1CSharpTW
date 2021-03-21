using System;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.command.executor
{
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static l1j.server.server.model.skill.L1SkillId.GMSTATUS_FINDINVIS;
    using L1World = LineageServer.Server.Server.Model.L1World;
    using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
    using S_RemoveObject = LineageServer.Server.Server.serverpackets.S_RemoveObject;
    using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

    public class L1FindInvis : L1CommandExecutor
    {
        private L1FindInvis()
        {
        }

        public static L1CommandExecutor Instance
        {
            get
            {
                return new L1FindInvis();
            }
        }

        public virtual void execute(L1PcInstance pc, string cmdName, string arg)
        {
            if (arg.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                pc.setSkillEffect(GMSTATUS_FINDINVIS, 0);
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