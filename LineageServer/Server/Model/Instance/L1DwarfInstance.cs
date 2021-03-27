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
	using L1Attack = LineageServer.Server.Model.L1Attack;
	using L1NpcTalkData = LineageServer.Server.Model.L1NpcTalkData;
	using S_NPCTalkReturn = LineageServer.Serverpackets.S_NPCTalkReturn;
	using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	[Serializable]
	public class L1DwarfInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1DwarfInstance).FullName);

		/// <param name="template"> </param>
		public L1DwarfInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			L1Attack attack = new L1Attack(pc, this, skillId);
			attack.calcHit();
			attack.action();
			attack.calcDamage();
			attack.addChaserAttack();
			attack.calcStaffOfMana();
			attack.addPcPoisonAttack(pc, this);
			attack.commit();
		}

		public override void onTalkAction(L1PcInstance pc)
		{
			int objid = Id;
			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
			int npcId = NpcTemplate.get_npcId();
			string htmlid = null;

			if (talking != null)
			{
				if (npcId == 60028)
				{ // エル
					if (!pc.Elf)
					{
						htmlid = "elCE1";
					}
				}

				if (!string.ReferenceEquals(htmlid, null))
				{ // htmlidが指定されている場合
					pc.sendPackets(new S_NPCTalkReturn(objid, htmlid));
				}
				else
				{
					if (pc.Level < 5)
					{
						pc.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
					}
					else
					{
						pc.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
					}
				}
			}
		}

		public override void onFinalAction(L1PcInstance pc, string Action)
		{
			if (Action.Equals("retrieve", StringComparison.OrdinalIgnoreCase))
			{
				_log.finest("Retrive items in storage");
			}
			else if (Action.Equals("retrieve-pledge", StringComparison.OrdinalIgnoreCase))
			{
				_log.finest("Retrive items in pledge storage");

				if (pc.Clanname.Equals(" ", StringComparison.OrdinalIgnoreCase))
				{
					_log.finest("pc isnt in a pledge");
					S_ServerMessage talk = new S_ServerMessage((S_ServerMessage.NO_PLEDGE), Action);
					pc.sendPackets(talk);
				}
				else
				{
					_log.finest("pc is in a pledge");
				}
			}
		}
	}

}