using LineageServer.Model;
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Level : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                int level = int.Parse(tok.nextToken());
                if (level == pc.Level)
                {
                    return;
                }
                if (!level.Includes(1, 110))
                {
                    pc.sendPackets(new S_SystemMessage("請在1-110範圍內指定"));
                    return;
                }
                pc.Exp = ExpTable.getExpByLevel(level);
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " lv "));
            }
        }
    }

}