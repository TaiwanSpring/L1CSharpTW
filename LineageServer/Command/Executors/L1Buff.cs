using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：輔助魔法
    /// </summary>
    class L1Buff : ILineageCommand
    {

        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                ICollection<L1PcInstance> players = null;
                string s = tok.nextToken();
                if (s == "me")
                {
                    players = ListFactory.NewList<L1PcInstance>();
                    players.Add(pc);
                    s = tok.nextToken();
                }
                else if (s == "all")
                {
                    players = L1World.Instance.AllPlayers;
                    s = tok.nextToken();
                }
                else
                {
                    players = L1World.Instance.getVisiblePlayer(pc);
                }

                int skillId = int.Parse(s);
                int time = 0;
                if (tok.hasMoreTokens())
                {
                    time = int.Parse(tok.nextToken());
                }

                L1Skills skill = SkillsTable.Instance.getTemplate(skillId);

                if (skill.Target.Equals("buff"))
                {
                    foreach (L1PcInstance tg in players)
                    {
                        (new L1SkillUse()).handleCommands(pc, skillId, tg.Id, tg.X, tg.Y, null, time, L1SkillUse.TYPE_SPELLSC);
                    }
                }
                else if (skill.Target.Equals("none"))
                {
                    foreach (L1PcInstance tg in players)
                    {
                        (new L1SkillUse()).handleCommands(tg, skillId, tg.Id, tg.X, tg.Y, null, time, L1SkillUse.TYPE_GMBUFF);
                    }
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage("非buff類型的魔法。"));
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " [all|me] skillId time。"));
            }
        }
    }

}