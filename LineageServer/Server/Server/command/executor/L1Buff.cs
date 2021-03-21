using System;
using System.Collections.Generic;

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

	using SkillsTable = LineageServer.Server.Server.datatables.SkillsTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1SkillUse = LineageServer.Server.Server.Model.skill.L1SkillUse;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Skills = LineageServer.Server.Server.Templates.L1Skills;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	/// <summary>
	/// GM指令：輔助魔法
	/// </summary>
	public class L1Buff : L1CommandExecutor
	{
		private L1Buff()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Buff();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer tok = new StringTokenizer(arg);
				ICollection<L1PcInstance> players = null;
				string s = tok.nextToken();
				if (s.Equals("me", StringComparison.OrdinalIgnoreCase))
				{
					players = Lists.newList();
					players.Add(pc);
					s = tok.nextToken();
				}
				else if (s.Equals("all", StringComparison.OrdinalIgnoreCase))
				{
					players = L1World.Instance.AllPlayers;
					s = tok.nextToken();
				}
				else
				{
					players = L1World.Instance.getVisiblePlayer(pc);
				}

				int skillId = int.Parse(s);
				int time = 0;
				if (tok.hasMoreTokens())
				{
					time = int.Parse(tok.nextToken());
				}

				L1Skills skill = SkillsTable.Instance.getTemplate(skillId);

				if (skill.Target.Equals("buff"))
				{
					foreach (L1PcInstance tg in players)
					{
						(new L1SkillUse()).handleCommands(pc, skillId, tg.Id, tg.X, tg.Y, null, time, L1SkillUse.TYPE_SPELLSC);
					}
				}
				else if (skill.Target.Equals("none"))
				{
					foreach (L1PcInstance tg in players)
					{
						(new L1SkillUse()).handleCommands(tg, skillId, tg.Id, tg.X, tg.Y, null, time, L1SkillUse.TYPE_GMBUFF);
					}
				}
				else
				{
					pc.sendPackets(new S_SystemMessage("非buff類型的魔法。"));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " [all|me] skillId time。"));
			}
		}
	}

}