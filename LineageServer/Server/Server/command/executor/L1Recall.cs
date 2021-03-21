using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.utils.collections;
using System;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
	class L1Recall : IL1CommandExecutor
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				ICollection<L1PcInstance> targets = null;
				if (arg == "all")
				{
					targets = L1World.Instance.AllPlayers;
				}
				else
				{
					targets = Lists.newList<L1PcInstance>();
					L1PcInstance tg = L1World.Instance.getPlayer(arg);
					if (tg == null)
					{
						pc.sendPackets(new S_SystemMessage("ID不存在。"));
						return;
					}
					targets.Add(tg);
				}

				foreach (L1PcInstance target in targets)
				{
					if (target.Gm)
					{
						continue;
					}
					L1Teleport.teleportToTargetFront(target, pc, 2);
					pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(target.Name).Append("成功被您召喚回來。").ToString()));
					target.sendPackets(new S_SystemMessage("您被召喚到GM身邊。"));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入: " + cmdName + " all|玩家名稱。"));
			}
		}
	}

}