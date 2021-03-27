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
namespace LineageServer.Server.Model.Instance
{

	using IdFactory = LineageServer.Server.IdFactory;
	using ItemTable = LineageServer.Server.DataSources.ItemTable;
	using NpcTable = LineageServer.Server.DataSources.NpcTable;
	using L1Attack = LineageServer.Server.Model.L1Attack;
	using L1Character = LineageServer.Server.Model.L1Character;
	using L1Inventory = LineageServer.Server.Model.L1Inventory;
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1Quest = LineageServer.Server.Model.L1Quest;
	using L1World = LineageServer.Server.Model.L1World;
	using S_FollowerPack = LineageServer.Serverpackets.S_FollowerPack;
	using S_NPCTalkReturn = LineageServer.Serverpackets.S_NPCTalkReturn;
	using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	[Serializable]
	public class L1FollowerInstance : L1NpcInstance
	{
		private const long serialVersionUID = 1L;

		public override bool noTarget()
		{
			foreach (GameObject @object in L1World.Instance.getVisibleObjects(this))
			{
				if (@object is L1NpcInstance)
				{
					L1NpcInstance npc = (L1NpcInstance) @object;
					if ((npc.NpcTemplate.get_npcId() == 70740) && (NpcTemplate.get_npcId() == 71093))
					{ // 調査員
						Paralyzed = true;
						L1PcInstance pc = (L1PcInstance) _master;
						if (!pc.Inventory.checkItem(40593))
						{
							createNewItem(pc, 40593, 1);
						}
						deleteMe();
						return true;
					}
					else if ((npc.NpcTemplate.get_npcId() == 70811) && (NpcTemplate.get_npcId() == 71094))
					{ // 安迪亞
						Paralyzed = true;
						L1PcInstance pc = (L1PcInstance) _master;
						if (!pc.Inventory.checkItem(40582) && !pc.Inventory.checkItem(40583))
						{ // 身上無安迪亞之袋、安迪亞之信
							createNewItem(pc, 40582, 1);
						}
						deleteMe();
						return true;
					}
					else if ((npc.NpcTemplate.get_npcId() == 71061) && (NpcTemplate.get_npcId() == 71062))
					{ // カミット
						if (Location.getTileLineDistance(_master.Location) < 3)
						{
							L1PcInstance pc = (L1PcInstance) _master;
							if (((pc.X >= 32448) && (pc.X <= 32452)) && ((pc.Y >= 33048) && (pc.Y <= 33052)) && (pc.MapId == 440))
							{
								Paralyzed = true;
								if (!pc.Inventory.checkItem(40711))
								{
									createNewItem(pc, 40711, 1);
									pc.Quest.set_step(L1Quest.QUEST_CADMUS, 3);
								}
								deleteMe();
								return true;
							}
						}
					}
					else if ((npc.NpcTemplate.get_npcId() == 71074) && (NpcTemplate.get_npcId() == 71075))
					{
						// 疲れ果てたリザードマンファイター
						if (Location.getTileLineDistance(_master.Location) < 3)
						{
							L1PcInstance pc = (L1PcInstance) _master;
							if (((pc.X >= 32731) && (pc.X <= 32735)) && ((pc.Y >= 32854) && (pc.Y <= 32858)) && (pc.MapId == 480))
							{
								Paralyzed = true;
								if (!pc.Inventory.checkItem(40633))
								{
									createNewItem(pc, 40633, 1);
									pc.Quest.set_step(L1Quest.QUEST_LIZARD, 2);
								}
								deleteMe();
								return true;
							}
						}
					}
					else if ((npc.NpcTemplate.get_npcId() == 70964) && (NpcTemplate.get_npcId() == 70957))
					{ // ロイ
						if (Location.getTileLineDistance(_master.Location) < 3)
						{
							L1PcInstance pc = (L1PcInstance) _master;
							if (((pc.X >= 32917) && (pc.X <= 32921)) && ((pc.Y >= 32974) && (pc.Y <= 32978)) && (pc.MapId == 410))
							{
								Paralyzed = true;
								createNewItem(pc, 41003, 1);
								pc.Quest.set_step(L1Quest.QUEST_ROI, 0);
								deleteMe();
								return true;
							}
						}
					}
					else if ((npc.NpcTemplate.get_npcId() == 71114) && (NpcTemplate.get_npcId() == 81350))
					{ // 迪嘉勒廷的女間諜
						if (Location.getTileLineDistance(_master.Location) < 15)
						{
							L1PcInstance pc = (L1PcInstance) _master;
							if (((pc.X >= 32542) && (pc.X <= 32585)) && ((pc.Y >= 32656) && (pc.Y <= 32698)) && (pc.MapId == 400))
							{
								Paralyzed = true;
								createNewItem(pc, 49163, 1);
								pc.Quest.set_step(4, 4);
								deleteMe();
								return true;
							}
						}
					}
				}
			}

			if (_master.Dead || (Location.getTileLineDistance(_master.Location) > 10))
			{
				Paralyzed = true;
				spawn(NpcTemplate.get_npcId(), X, Y, Heading, MapId);
				deleteMe();
				return true;
			}
			else if ((_master != null) && (_master.MapId == MapId))
			{
				if (Location.getTileLineDistance(_master.Location) > 2)
				{
					DirectionMove = moveDirection(_master.X, _master.Y);
					SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
				}
			}
			return false;
		}

		public L1FollowerInstance(L1Npc template, L1NpcInstance target, L1Character master) : base(template)
		{

			_master = master;
			Id = IdFactory.Instance.nextId();

			Master = master;
			X = target.X;
			Y = target.Y;
			Map = target.MapId;
			Heading = target.Heading;
			LightSize = target.LightSize;

			target.Paralyzed = true;
			target.Dead = true;
			target.deleteMe();

			L1World.Instance.storeObject(this);
			L1World.Instance.addVisibleObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				onPerceive(pc);
			}

			startAI();
			master.addFollower(this);
		}

		public override void deleteMe()
		{
			lock (this)
			{
				_master.FollowerList.Remove(Id);
				System.Collections.IDictionary.setPassable(Location, true);
				base.deleteMe();
			}
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			L1Attack attack = new L1Attack(pc, this, skillId);
			if (attack.calcHit())
			{
				attack.calcDamage();
				attack.calcStaffOfMana();
				attack.addPcPoisonAttack(pc, this);
				attack.addChaserAttack();
			}
			attack.action();
			attack.commit();
		}

		public override void onTalkAction(L1PcInstance player)
		{
			if (Dead)
			{
				return;
			}
			if (NpcTemplate.get_npcId() == 71093)
			{
				if (_master.Equals(player))
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "searcherk2"));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "searcherk4"));
				}
			}
			else if (NpcTemplate.get_npcId() == 71094)
			{
				if (_master.Equals(player))
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "endiaq2"));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "endiaq4"));
				}
			}
			else if (NpcTemplate.get_npcId() == 71062)
			{
				if (_master.Equals(player))
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "kamit2"));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "kamit1"));
				}
			}
			else if (NpcTemplate.get_npcId() == 71075)
			{
				if (_master.Equals(player))
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "llizard2"));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "llizard1a"));
				}
			}
			else if (NpcTemplate.get_npcId() == 70957)
			{
				if (_master.Equals(player))
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "roi2"));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "roi2"));
				}
			}
			else if (NpcTemplate.get_npcId() == 81350)
			{
				if (_master.Equals(player))
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "dspy3"));
				}
				else
				{
					player.sendPackets(new S_NPCTalkReturn(Id, "dspy3"));
				}
			}

		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_FollowerPack(this, perceivedFrom));
		}

		private void createNewItem(L1PcInstance pc, int item_id, int count)
		{
			L1ItemInstance item = ItemTable.Instance.createItem(item_id);
			item.Count = count;
			if (item != null)
			{
				if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
				{
					pc.Inventory.storeItem(item);
				}
				else
				{
					L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
				}
				pc.sendPackets(new S_ServerMessage(403, item.LogName));
			}
		}

		public virtual void spawn(int npcId, int X, int Y, int H, short Map)
		{
			L1Npc l1npc = NpcTable.Instance.getTemplate(npcId);
			if (l1npc != null)
			{
				L1NpcInstance mob = null;
				try
				{
					string implementationName = l1npc.Impl;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> _constructor = Class.forName((new StringBuilder()).append("l1j.server.server.model.Instance.").append(implementationName).append("Instance").toString()).getConstructors()[0];
					System.Reflection.ConstructorInfo<object> _constructor = Type.GetType((new StringBuilder()).Append("l1j.server.server.model.Instance.").Append(implementationName).Append("Instance").ToString()).GetConstructors()[0];
					mob = (L1NpcInstance) _constructor.Invoke(new object[] {l1npc});
					mob.Id = IdFactory.Instance.nextId();
					mob.X = X;
					mob.Y = Y;
					mob.HomeX = X;
					mob.HomeY = Y;
					mob.Map = Map;
					mob.Heading = H;
					L1World.Instance.storeObject(mob);
					L1World.Instance.addVisibleObject(mob);
					GameObject @object = L1World.Instance.findObject(mob.Id);
					L1QuestInstance newnpc = (L1QuestInstance) @object;
					newnpc.onNpcAI();
					newnpc.turnOnOffLight();
					newnpc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
				}
				catch (Exception e)
				{
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
				}
			}
		}

	}

}