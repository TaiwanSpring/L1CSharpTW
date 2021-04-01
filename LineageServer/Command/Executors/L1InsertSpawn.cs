using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Text;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：新增NPC重生
    /// </summary>
    class L1InsertSpawn : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            string msg = string.Empty;
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                string type = tok.nextToken();
                int npcId = int.Parse(tok.nextToken().Trim());
                L1Npc template = Container.Instance.Resolve<INpcController>().getTemplate(npcId);

                if (template == null)
                {
                    msg = "找不到符合條件的NPC。";
                    return;
                }
                if (type == "mob")
                {
                    if (!template.Impl.Equals("L1Monster"))
                    {
                        msg = "指定的NPC不是L1Monster類型。";
                        return;
                    }
                    SpawnTable.storeSpawn(pc, template);
                }
                else if (type == "npc")
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