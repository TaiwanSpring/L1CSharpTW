using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Loc : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                int locx = pc.X;
                int locy = pc.Y;
                short mapid = pc.MapId;
                int gab = Container.Instance.Resolve<IWorldMap>().getMap(mapid).getOriginalTile(locx, locy);
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