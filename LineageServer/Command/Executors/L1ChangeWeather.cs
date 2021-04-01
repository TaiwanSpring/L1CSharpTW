using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：改變天氣
    /// </summary>
    class L1ChangeWeather : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                int weather = int.Parse(tok.nextToken());
                Container.Instance.Resolve<IGameWorld>().Weather = weather;
                Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_Weather(weather));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " 0～3、16～19。"));
            }
        }
    }

}