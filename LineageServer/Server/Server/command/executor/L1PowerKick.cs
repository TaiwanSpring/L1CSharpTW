using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{	class L1PowerKick : IL1CommandExecutor
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				L1PcInstance target = L1World.Instance.getPlayer(arg);

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