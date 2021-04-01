using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Command.Executors
{	class L1PowerKick : ILineageCommand
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				L1PcInstance target = Container.Instance.Resolve<IGameWorld>().getPlayer(arg);

				IpTable iptable = IpTable.Instance;
				if (target != null)
				{
					iptable.banIp(target.NetConnection.Ip); // 加入IP至BAN名單
					pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(target.Name).Append("被您強制踢除遊戲並封鎖IP。").ToString()));
					target.sendPackets(new S_Disconnect());
				}
				else
				{
					pc.sendPackets(new S_SystemMessage("您指定的腳色名稱不存在。"));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " 玩家名稱。"));
			}
		}
	}

}