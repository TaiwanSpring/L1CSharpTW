using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Command.Executors
{
    class L1ToPC : ILineageCommand
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