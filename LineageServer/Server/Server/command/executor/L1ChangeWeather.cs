using LineageServer.Model;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：改變天氣
    /// </summary>
    class L1ChangeWeather : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer tok = new StringTokenizer(arg);
                int weather = int.Parse(tok.nextToken());
                L1World.Instance.Weather = weather;
                L1World.Instance.broadcastPacketToAll(new S_Weather(weather));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " 0～3、16～19。"));
            }
        }
    }

}