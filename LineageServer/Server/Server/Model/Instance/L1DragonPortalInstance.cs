﻿using System;

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
namespace LineageServer.Server.Server.Model.Instance
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.EFFECT_BLOODSTAIN_OF_ANTHARAS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.EFFECT_BLOODSTAIN_OF_FAFURION;

	using NPCTalkDataTable = LineageServer.Server.Server.datatables.NPCTalkDataTable;
	using L1DragonSlayer = LineageServer.Server.Server.Model.L1DragonSlayer;
	using L1NpcTalkData = LineageServer.Server.Server.Model.L1NpcTalkData;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using S_NPCTalkReturn = LineageServer.Server.Server.serverpackets.S_NPCTalkReturn;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	[Serializable]
	public class L1DragonPortalInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;
		/// <param name="template"> </param>
		public L1DragonPortalInstance(L1Npc template) : base(template)
		{
		}

		public override void onTalkAction(L1PcInstance player)
		{
			int npcid = NpcTemplate.get_npcId();
			int portalNumber = PortalNumber; // 龍門編號
			int X = 32599;
			int Y = 32742;
			short mapId = 1005;
			int objid = Id;
			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(npcid);
			string htmlid = null;
			string[] htmldata = null;
			if ((npcid >= 81273 && npcid <= 81276))
			{ // 龍之門扉
				if (portalNumber == -1)
				{
					return;
				}
				mapId = (short)(1005 + portalNumber); // 地圖判斷
				if (L1DragonSlayer.Instance.getPlayersCount(portalNumber) >= 32)
				{
					player.sendPackets(new S_ServerMessage(1536)); // 參與人員已額滿，目前無法再入場。
				}
				else if (L1DragonSlayer.Instance.DragonSlayerStatus[portalNumber] >= 5)
				{
					player.sendPackets(new S_ServerMessage(1537)); // 攻略已經開始，目前無法入場。
				}
				else
				{
					if (portalNumber >= 0 && portalNumber <= 5)
					{ // 安塔瑞斯副本
						if (player.hasSkillEffect(EFFECT_BLOODSTAIN_OF_ANTHARAS))
						{
							player.sendPackets(new S_ServerMessage(1626)); // 龍之血痕已穿透全身，在血痕的氣味消失之前，無法再進入龍之門扉。
							return;
						}
					}
					else if (portalNumber >= 6 && portalNumber <= 11)
					{ // 法利昂副本
						if (player.hasSkillEffect(EFFECT_BLOODSTAIN_OF_FAFURION))
						{
							player.sendPackets(new S_ServerMessage(1626)); // 龍之血痕已穿透全身，在血痕的氣味消失之前，無法再進入龍之門扉。
							return;
						}
						X = 32927;
						Y = 32741;
					}
					player.PortalNumber = portalNumber;
					L1DragonSlayer.Instance.addPlayerList(player, portalNumber);
					L1Teleport.teleport(player, X, Y, mapId, 2, true);
				}
			}
			else if (npcid == 81301)
			{ // 傳送進入安塔瑞斯棲息地
				L1DragonSlayer.Instance.startDragonSlayer(player.PortalNumber);
				L1Teleport.teleport(player, 32795, 32665, player.MapId, 4, true);
			}
			else if (npcid == 81302)
			{ // 傳送出去安塔瑞斯棲息地
				L1Teleport.teleport(player, 32700, 32671, player.MapId, 6, true);
			}
			else if (npcid == 81303)
			{ // 傳送進入法利昂棲息地
				L1DragonSlayer.Instance.startDragonSlayer(player.PortalNumber);
				L1Teleport.teleport(player, 32988, 32843, player.MapId, 6, true);
			}
			else if (npcid == 81304)
			{ // 傳送出去法利昂棲息地
				L1Teleport.teleport(player, 32937, 32672, player.MapId, 6, true);
			}
			else if (npcid == 81305)
			{ // 傳送進入安塔瑞斯洞穴
			}
			else if (npcid == 81306)
			{ // 傳送到安塔瑞斯 洞穴入口(階段型)
				L1Teleport.teleport(player, 32677, 32746, player.MapId, 6, true);
			}
			else if (npcid == 81277)
			{ // 隱匿的巨龍谷入口
				int playerLv = player.Level; //角色等級
				if (playerLv >= 30 && playerLv <= 51)
				{
					htmlid = "dsecret1";
				}
				else if (playerLv >= 52)
				{
					htmlid = "dsecret2";
				}
				else
				{
					htmlid = "dsecret3";
				}
			}

			if (!string.ReferenceEquals(htmlid, null))
			{
				player.sendPackets(new S_NPCTalkReturn(objid, htmlid, htmldata));
			}
			else
			{
				player.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
			}
		}
	}

}