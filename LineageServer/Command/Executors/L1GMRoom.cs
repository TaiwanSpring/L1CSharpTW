using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Command.Executors
{
    class L1GMRoom : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                if (int.TryParse(arg, out int i))
                {
                    if (i == 1)
                    {
                        L1Teleport.teleport(pc, 32737, 32796, (short)99, 5, false);
                    }
                    else if (i == 2)
                    {
                        L1Teleport.teleport(pc, 32734, 32799, (short)17100, 5, false); // 17100!?
                    }
                    else if (i == 3)
                    {
                        L1Teleport.teleport(pc, 32644, 32955, (short)0, 5, false);
                    }
                    else if (i == 4)
                    {
                        L1Teleport.teleport(pc, 33429, 32814, (short)4, 5, false);
                    }
                    else if (i == 5)
                    {
                        L1Teleport.teleport(pc, 32894, 32535, (short)300, 5, false);
                    }
                    else
                    {
                        L1Location loc = GMCommandsConfig.ROOMS[arg.ToLower()];
                        if (loc == null)
                        {
                            pc.sendPackets(new S_SystemMessage(arg + " 未定義的Room～"));
                            return;
                        }
                        L1Teleport.teleport(pc, loc.X, loc.Y, (short)loc.MapId, 5, false);
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 .gmroom1～.gmroom5 or .gmroom name 。"));
            }
        }
    }

}