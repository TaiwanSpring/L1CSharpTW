using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Extensions;

namespace LineageServer.Command.Executors
{
    class L1Level : ILineageCommand
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