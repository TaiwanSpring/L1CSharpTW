using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Loc : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                int locx = pc.X;
                int locy = pc.Y;
                short mapid = pc.MapId;
                int gab = L1WorldMap.Instance.getMap(mapid).getOriginalTile(locx, locy);
                string msg = string.Format("座標 ({0:D}, {1:D}, {2:D}) {3:D}", locx, locy, mapid, gab);
                pc.sendPackets(new S_SystemMessage(msg));
            }
            catch (Exception e)
            {
                pc.sendPackets(new S_SystemMessage(e.Message));
            }
        }
    }

}