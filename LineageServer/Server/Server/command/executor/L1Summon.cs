using System;

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

	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	public class L1Summon : L1CommandExecutor
	{
		private L1Summon()
		{
		}

		public static L1Summon Instance
		{
			get
			{
				return new L1Summon();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer tok = new StringTokenizer(arg);
				string nameid = tok.nextToken();
				int npcid = 0;
				try
				{
					npcid = int.Parse(nameid);
				}
				catch (System.FormatException)
				{
					npcid = NpcTable.Instance.findNpcIdByNameWithoutSpace(nameid);
					if (npcid == 0)
					{
						pc.sendPackets(new S_SystemMessage("找不到符合條件的NPC。"));
						return;
					}
				}
				int count = 1;
				if (tok.hasMoreTokens())
				{
					count = int.Parse(tok.nextToken());
				}
				L1Npc npc = NpcTable.Instance.getTemplate(npcid);
				for (int i = 0; i < count; i++)
				{
					L1SummonInstance summonInst = new L1SummonInstance(npc, pc);
					summonInst.Petcost = 0;
				}
				nameid = NpcTable.Instance.getTemplate(npcid).get_name();
				pc.sendPackets(new S_SystemMessage(nameid + "(ID:" + npcid + ") (" + count + ") 召喚了。"));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入" + cmdName + " npcid|name [數量] 。"));
			}
		}
	}

}