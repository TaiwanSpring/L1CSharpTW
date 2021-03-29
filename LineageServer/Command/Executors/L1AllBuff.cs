using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：給對象所有魔法
    /// </summary>
    class L1AllBuff : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);

                string name = st.nextToken();
                L1PcInstance target = null;
                if (name.ToLower() == "me")
                {
                    target = L1World.Instance.getPlayer(name);
                }
                else
                {
                    target = L1World.Instance.getPlayer(name);
                }

                if (target == null)
                {
                    if (name.ToLower() == "all")
                    {
                        foreach (var item in L1World.Instance.AllPlayers)
                        {
                            L1BuffUtil.haste(target, 3600 * 1000);
                            L1BuffUtil.brave(target, 3600 * 1000);
                            L1PolyMorph.doPoly(target, 5641, 7200, L1PolyMorph.MORPH_BY_GM);
                            foreach (int element in L1SkillId.GetAllBuffers())
                            {
                                L1Skills skill = SkillsTable.Instance.getTemplate(element);
                                new L1SkillUse().handleCommands(item, element, target.Id, target.X, target.Y, null, skill.BuffDuration * 1000, L1SkillUse.TYPE_GMBUFF);
                            }
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(73, name)); // \f1%0はゲームをしていません。
                        return;
                    }
                }
                else
                {
                    L1BuffUtil.haste(target, 3600 * 1000);
                    L1BuffUtil.brave(target, 3600 * 1000);
                    L1PolyMorph.doPoly(target, 5641, 7200, L1PolyMorph.MORPH_BY_GM);
                    foreach (int element in L1SkillId.GetAllBuffers())
                    {
                        L1Skills skill = SkillsTable.Instance.getTemplate(element);
                        new L1SkillUse().handleCommands(target, element, target.Id, target.X, target.Y, null, skill.BuffDuration * 1000, L1SkillUse.TYPE_GMBUFF);
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 .allBuff 玩家名稱。"));
            }
        }
    }
}