using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：執行玩家/NPC動作
    /// </summary>
    class L1Action : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                int actId = Convert.ToInt32(st.nextToken(), 10);
                pc.sendPackets(new S_DoActionGFX(pc.Id, actId));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " actid。"));
            }
        }
    }

}