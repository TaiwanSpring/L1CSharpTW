using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Move : ILineageCommand
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