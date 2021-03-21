using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1ToPC : IL1CommandExecutor
    {

        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                L1PcInstance target = L1World.Instance.getPlayer(arg);

                if (target != null)
                {
                    L1Teleport.teleport(pc, target.X, target.Y, target.MapId, 5, false);
                    pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(arg).Append("移動到玩家身邊。").ToString()));
                }
                else
                {
                    pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(arg).Append("不在線上。").ToString()));
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入: " + cmdName + " 玩家名稱 。"));
            }
        }
    }

}