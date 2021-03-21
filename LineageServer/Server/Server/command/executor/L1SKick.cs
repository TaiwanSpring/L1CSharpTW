using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1SKick : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                L1PcInstance target = L1World.Instance.getPlayer(arg);
                if (target != null)
                {
                    pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(target.Name).Append("已被您強制踢除遊戲。").ToString()));
                    // SKTへ移動させる
                    target.X = 33080;
                    target.Y = 33392;
                    target.Map = L1WorldMap.Instance.getMap(4);
                    target.sendPackets(new S_Disconnect());
                    ClientThread targetClient = target.NetConnection;
                    targetClient.kick();
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage("指定的ID不存在。"));
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入: " + cmdName + " 玩家名稱。"));
            }
        }
    }
}