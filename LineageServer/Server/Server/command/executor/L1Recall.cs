using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.command.executor
{

	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class L1Recall : L1CommandExecutor
	{
		private L1Recall()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Recall();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				ICollection<L1PcInstance> targets = null;
				if (arg.Equals("all", StringComparison.OrdinalIgnoreCase))
				{
					targets = L1World.Instance.AllPlayers;
				}
				else
				{
					targets = Lists.newList();
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