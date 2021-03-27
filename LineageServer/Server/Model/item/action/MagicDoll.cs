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
namespace LineageServer.Server.Model.item.action
{
	using Config = LineageServer.Server.Config;
	using MagicDollTable = LineageServer.Server.DataSources.MagicDollTable;
	using NpcTable = LineageServer.Server.DataSources.NpcTable;
	using L1DollInstance = LineageServer.Server.Model.Instance.L1DollInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_OwnCharStatus = LineageServer.Serverpackets.S_OwnCharStatus;
	using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
	using S_SkillIconGFX = LineageServer.Serverpackets.S_SkillIconGFX;
	using S_SkillSound = LineageServer.Serverpackets.S_SkillSound;
	using L1MagicDoll = LineageServer.Server.Templates.L1MagicDoll;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	public class MagicDoll
	{

		public static void useMagicDoll(L1PcInstance pc, int itemId, int itemObjectId)
		{
			L1MagicDoll magic_doll = MagicDollTable.Instance.getTemplate((itemId));
			if (magic_doll != null)
			{
				bool isAppear = true;
				L1DollInstance doll = null;

				foreach (L1DollInstance curdoll in pc.DollList.Values)
				{
					doll = curdoll;
					if (doll.ItemObjId == itemObjectId)
					{
						isAppear = false;
						break;
					}
				}

				if (isAppear)
				{
					if (!pc.Inventory.checkItem(41246, 50))
					{
						pc.sendPackets(new S_ServerMessage(337, "$5240")); // 魔法結晶體不足
						return;
					}
					if (pc.DollList.Count >= Config.MAX_DOLL_COUNT)
					{
						pc.sendPackets(new S_ServerMessage(79)); // 沒有任何事情發生
						return;
					}
					int npcId = magic_doll.DollId;

					L1Npc template = NpcTable.Instance.getTemplate(npcId);
					doll = new L1DollInstance(template, pc, itemId, itemObjectId);
					pc.sendPackets(new S_SkillSound(doll.Id, 5935));
					pc.broadcastPacket(new S_SkillSound(doll.Id, 5935));
					pc.sendPackets(new S_SkillIconGFX(56, 1800));
					pc.sendPackets(new S_OwnCharStatus(pc));
					pc.Inventory.consumeItem(41246, 50);
				}
				else
				{
					pc.sendPackets(new S_SkillSound(doll.Id, 5936));
					pc.broadcastPacket(new S_SkillSound(doll.Id, 5936));
					doll.deleteDoll();
					pc.sendPackets(new S_SkillIconGFX(56, 0));
					pc.sendPackets(new S_OwnCharStatus(pc));
				}
			}
		}

	}

}