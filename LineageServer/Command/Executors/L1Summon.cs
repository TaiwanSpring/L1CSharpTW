using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
namespace LineageServer.Command.Executors
{
    class L1Summon : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                string nameid = tok.nextToken();
                int npcid = nameid.ParseInt();
                if (npcid == 0)
                {
                    npcid = NpcTable.Instance.findNpcIdByNameWithoutSpace(nameid);
                    if (npcid == 0)
                    {
                        pc.sendPackets(new S_SystemMessage("找不到符合條件的NPC。"));
                        return;
                    }
                }
                int count = 1;
                if (tok.hasMoreTokens())
                {
                    count = int.Parse(tok.nextToken());
                }
                L1Npc npc = NpcTable.Instance.getTemplate(npcid);
                for (int i = 0; i < count; i++)
                {
                    L1SummonInstance summonInst = new L1SummonInstance(npc, pc);
                    summonInst.Petcost = 0;
                }
                nameid = NpcTable.Instance.getTemplate(npcid).get_name();
                pc.sendPackets(new S_SystemMessage(nameid + "(ID:" + npcid + ") (" + count + ") 召喚了。"));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入" + cmdName + " npcid|name [數量] 。"));
            }
        }
    }

}