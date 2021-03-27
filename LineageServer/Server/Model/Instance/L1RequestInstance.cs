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
namespace LineageServer.Server.Model.Instance
{

	using NPCTalkDataTable = LineageServer.Server.DataSources.NPCTalkDataTable;
	using L1NpcTalkData = LineageServer.Server.Model.L1NpcTalkData;
	using S_NPCTalkReturn = LineageServer.Serverpackets.S_NPCTalkReturn;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	[Serializable]
	public class L1RequestInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1RequestInstance).FullName);

		public L1RequestInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance player)
		{
			int objid = Id;

			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());

			if (talking != null)
			{
				if (player.Lawful < -1000)
				{ // プレイヤーがカオティック
					player.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
				}
			}
			else
			{
				_log.finest("No actions for npc id : " + objid);
			}
		}

		public override void onFinalAction(L1PcInstance player, string action)
		{

		}

		public virtual void doFinalAction(L1PcInstance player)
		{

		}
	}

}