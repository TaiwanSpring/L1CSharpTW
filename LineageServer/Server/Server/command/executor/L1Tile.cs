using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Tile : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                int locX = pc.X;
                int locY = pc.Y;
                short mapId = pc.MapId;
                int tile0 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX, locY - 1);
                int tile1 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX + 1, locY - 1);
                int tile2 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX + 1, locY);
                int tile3 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX + 1, locY + 1);
                int tile4 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX, locY + 1);
                int tile5 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX - 1, locY + 1);
                int tile6 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX - 1, locY);
                int tile7 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX - 1, locY - 1);
                string msg = string.Format("0:{0:D} 1:{1:D} 2:{2:D} 3:{3:D} 4:{4:D} 5:{5:D} 6:{6:D} 7:{7:D}", tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7);
                pc.sendPackets(new S_SystemMessage(msg));
            }
            catch (Exception e)
            {
                pc.sendPackets(new S_SystemMessage(e.Message));
            }
        }
    }

}