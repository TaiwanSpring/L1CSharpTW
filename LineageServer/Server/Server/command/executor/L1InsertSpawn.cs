using LineageServer.Models;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.utils;
using System;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：新增NPC重生
    /// </summary>
    class L1InsertSpawn : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            string msg = string.Empty;
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                string type = tok.nextToken();
                int npcId = int.Parse(tok.nextToken().Trim());
                L1Npc template = NpcTable.Instance.getTemplate(npcId);

                if (template == null)
                {
                    msg = "找不到符合條件的NPC。";
                    return;
                }
                if (type.Equals("mob", StringComparison.OrdinalIgnoreCase))
                {
                    if (!template.Impl.Equals("L1Monster"))
                    {
                        msg = "指定的NPC不是L1Monster類型。";
                        return;
                    }
                    SpawnTable.storeSpawn(pc, template);
                }
                else if (type.Equals("npc", StringComparison.OrdinalIgnoreCase))
                {
                    NpcSpawnTable.Instance.storeSpawn(pc, template);
                }
                L1SpawnUtil.spawn(pc, npcId, 0, 0);
                msg = (new StringBuilder()).Append(template.get_name()).Append(" (" + npcId + ") ").Append("新增到資料庫中。").ToString();
            }
            catch (Exception e)
            {
                msg = "請輸入 : " + cmdName + " mob|npc NPCID 。";
            }
            finally
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    pc.sendPackets(new S_SystemMessage(msg));
                }
            }
        }
    }

}