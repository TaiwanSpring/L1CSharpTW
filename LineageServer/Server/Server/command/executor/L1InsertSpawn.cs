using System;
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

	using NpcSpawnTable = LineageServer.Server.Server.datatables.NpcSpawnTable;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using SpawnTable = LineageServer.Server.Server.datatables.SpawnTable;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using L1SpawnUtil = LineageServer.Server.Server.utils.L1SpawnUtil;

	public class L1InsertSpawn : L1CommandExecutor
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1InsertSpawn).FullName);

		private L1InsertSpawn()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1InsertSpawn();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			string msg = null;

			try
			{
				StringTokenizer tok = new StringTokenizer(arg);
				string type = tok.nextToken();
				int npcId = int.Parse(tok.nextToken().Trim());
				L1Npc template = NpcTable.Instance.getTemplate(npcId);

				if (template == null)
				{
					msg = "找不到符合條件的NPC。";
					return;
				}
				if (type.Equals("mob", StringComparison.OrdinalIgnoreCase))
				{
					if (!template.Impl.Equals("L1Monster"))
					{
						msg = "指定的NPC不是L1Monster類型。";
						return;
					}
					SpawnTable.storeSpawn(pc, template);
				}
				else if (type.Equals("npc", StringComparison.OrdinalIgnoreCase))
				{
					NpcSpawnTable.Instance.storeSpawn(pc, template);
				}
				L1SpawnUtil.spawn(pc, npcId, 0, 0);
				msg = (new StringBuilder()).Append(template.get_name()).Append(" (" + npcId + ") ").Append("新增到資料庫中。").ToString();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, "", e);
				msg = "請輸入 : " + cmdName + " mob|npc NPCID 。";
			}
			finally
			{
				if (!string.ReferenceEquals(msg, null))
				{
					pc.sendPackets(new S_SystemMessage(msg));
				}
			}
		}
	}

}