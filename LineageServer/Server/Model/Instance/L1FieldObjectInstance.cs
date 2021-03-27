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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.CANCELLATION;
	using ItemTable = LineageServer.Server.DataSources.ItemTable;
	using L1HauntedHouse = LineageServer.Server.Model.L1HauntedHouse;
	using L1Inventory = LineageServer.Server.Model.L1Inventory;
	using L1Teleport = LineageServer.Server.Model.L1Teleport;
	using L1World = LineageServer.Server.Model.L1World;
	using L1SkillUse = LineageServer.Server.Model.skill.L1SkillUse;
	using S_RemoveObject = LineageServer.Serverpackets.S_RemoveObject;
	using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	[Serializable]
	public class L1FieldObjectInstance : L1NpcInstance
	{

		private const long serialVersionUID = 1L;

		public L1FieldObjectInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance pc)
		{
			if (NpcTemplate.get_npcId() == 81171)
			{ // おばけ屋敷のゴールの炎
				if (L1HauntedHouse.Instance.HauntedHouseStatus == L1HauntedHouse.STATUS_PLAYING)
				{
					int winnersCount = L1HauntedHouse.Instance.WinnersCount;
					int goalCount = L1HauntedHouse.Instance.GoalCount;
					if (winnersCount == goalCount + 1)
					{
						L1ItemInstance item = ItemTable.Instance.createItem(49280); // 勇者のパンプキン袋(銅)
						int count = 1;
						if (item != null)
						{
							if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
							{
								item.Count = count;
								pc.Inventory.storeItem(item);
								pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
							}
						}
						L1HauntedHouse.Instance.endHauntedHouse();
					}
					else if (winnersCount > goalCount + 1)
					{
						L1HauntedHouse.Instance.GoalCount = goalCount + 1;
						L1HauntedHouse.Instance.removeMember(pc);
						L1ItemInstance item = null;
						if (winnersCount == 3)
						{
							if (goalCount == 1)
							{
								item = ItemTable.Instance.createItem(49278); // 勇者のパンプキン袋(金)
							}
							else if (goalCount == 2)
							{
								item = ItemTable.Instance.createItem(49279); // 勇者のパンプキン袋(銀)
							}
						}
						else if (winnersCount == 2)
						{
							item = ItemTable.Instance.createItem(49279); // 勇者のパンプキン袋(銀)
						}
						int count = 1;
						if (item != null)
						{
							if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
							{
								item.Count = count;
								pc.Inventory.storeItem(item);
								pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
							}
						}
						L1SkillUse l1skilluse = new L1SkillUse();
						l1skilluse.handleCommands(pc, CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
						L1Teleport.teleport(pc, 32624, 32813, (short) 4, 5, true);
					}
				}
			}
		}

		public override void deleteMe()
		{
			_destroyed = true;
			if (Inventory != null)
			{
				Inventory.clearItems();
			}
			L1World.Instance.removeVisibleObject(this);
			L1World.Instance.removeObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				pc.removeKnownObject(this);
				pc.sendPackets(new S_RemoveObject(this));
			}
			removeAllKnownObjects();
		}
	}

}