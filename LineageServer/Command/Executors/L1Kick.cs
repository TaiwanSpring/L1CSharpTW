using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Command.Executors
{
	class L1Kick : ILineageCommand
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				L1PcInstance target = Container.Instance.Resolve<IGameWorld>().getPlayer(arg);

				if (target != null)
				{
					pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(target.Name).Append("已被您強制踢除遊戲。").ToString()));
					target.sendPackets(new S_Disconnect());
				}
				else
				{
					pc.sendPackets(new S_SystemMessage("您指定的該玩家名稱不存在。"));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " 玩家名稱。"));
			}
		}
    }

}