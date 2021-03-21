﻿using LineageServer.Model;
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.utils;
using System;

namespace LineageServer.Server.Server.Command.Executor
{
    class L1SpawnCmd : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                string nameId = tok.nextToken();
                int count = 1;
                if (tok.hasMoreTokens())
                {
                    count = int.Parse(tok.nextToken());
                }
                int randomrange = 0;
                if (tok.hasMoreTokens())
                {
                    randomrange = Convert.ToInt32(tok.nextToken(), 10);
                }
                int npcid = GetNpcId(nameId);

                L1Npc npc = NpcTable.Instance.getTemplate(npcid);
                if (npc == null)
                {
                    pc.sendPackets(new S_SystemMessage("找不到符合條件的NPC。"));
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    L1SpawnUtil.spawn(pc, npcid, randomrange, 0);
                }
                string msg = string.Format("{0}({1:D}) ({2:D}) 召喚了。 (範圍:{3:D})", npc.get_name(), npcid, count, randomrange);
                pc.sendPackets(new S_SystemMessage(msg));
            }
            catch (Exception e)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 内部錯誤。"));
            }
        }
        private int GetNpcId(string nameId)
        {
            if (int.TryParse(nameId, out int id))
            {
                return id;
            }
            else
            {
                return NpcTable.Instance.findNpcIdByNameWithoutSpace(nameId);
            }
        }
    }

}