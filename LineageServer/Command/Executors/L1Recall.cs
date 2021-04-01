using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Command.Executors
{
	class L1Recall : ILineageCommand
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				ICollection<L1PcInstance> targets = null;
				if (arg == "all")
				{
					targets = Container.Instance.Resolve<IGameWorld>().AllPlayers;
				}
				else
				{
					targets = ListFactory.NewList<L1PcInstance>();
					L1PcInstance tg = Container.Instance.Resolve<IGameWorld>().getPlayer(arg);
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