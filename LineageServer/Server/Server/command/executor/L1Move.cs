using LineageServer.Model;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Move : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                int locx = int.Parse(st.nextToken());
                int locy = int.Parse(st.nextToken());
                short mapid;
                if (st.hasMoreTokens())
                {
                    mapid = short.Parse(st.nextToken());
                }
                else
                {
                    mapid = pc.MapId;
                }
                L1Teleport.teleport(pc, locx, locy, mapid, 5, false);
                pc.sendPackets(new S_SystemMessage("座標 " + locx + ", " + locy + ", " + mapid + "已經到達。"));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + "請輸入 X座標 Y座標 [地圖編號]。"));
            }
        }
    }

}