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
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using L1SpawnUtil = LineageServer.Server.Server.utils.L1SpawnUtil;

	public class L1SpawnCmd : L1CommandExecutor
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1SpawnCmd).FullName);

		private L1SpawnCmd()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1SpawnCmd();
			}
		}

		private void sendErrorMessage(L1PcInstance pc, string cmdName)
		{
			string errorMsg = "請輸入: " + cmdName + " npcid|name [數量] [範圍] 。";
			pc.sendPackets(new S_SystemMessage(errorMsg));
		}

		private int parseNpcId(string nameId)
		{
			int npcid = 0;
			try
			{
				npcid = int.Parse(nameId);
			}
			catch (System.FormatException)
			{
				npcid = NpcTable.Instance.findNpcIdByNameWithoutSpace(nameId);
			}
			return npcid;
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer tok = new StringTokenizer(arg);
				string nameId = tok.nextToken();
				int count = 1;
				if (tok.hasMoreTokens())
				{
					count = int.Parse(tok.nextToken());
				}
				int randomrange = 0;
				if (tok.hasMoreTokens())
				{
					randomrange = Convert.ToInt32(tok.nextToken(), 10);
				}
				int npcid = parseNpcId(nameId);

				L1Npc npc = NpcTable.Instance.getTemplate(npcid);
				if (npc == null)
				{
					pc.sendPackets(new S_SystemMessage("找不到符合條件的NPC。"));
					return;
				}
				for (int i = 0; i < count; i++)
				{
					L1SpawnUtil.spawn(pc, npcid, randomrange, 0);
				}
				string msg = string.Format("{0}({1:D}) ({2:D}) 召喚了。 (範圍:{3:D})", npc.get_name(), npcid, count, randomrange);
				pc.sendPackets(new S_SystemMessage(msg));
			}
			catch (NoSuchElementException)
			{
				sendErrorMessage(pc, cmdName);
			}
			catch (System.FormatException)
			{
				sendErrorMessage(pc, cmdName);
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				pc.sendPackets(new S_SystemMessage(cmdName + " 内部錯誤。"));
			}
		}
	}

}