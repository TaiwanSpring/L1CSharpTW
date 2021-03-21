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
namespace LineageServer.Server.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.CANCELLATION;


	using L1DoorInstance = LineageServer.Server.Server.Model.Instance.L1DoorInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1SkillUse = LineageServer.Server.Server.Model.skill.L1SkillUse;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class L1HauntedHouse
	{
		public const int STATUS_NONE = 0;

		public const int STATUS_READY = 1;

		public const int STATUS_PLAYING = 2;

		private readonly IList<L1PcInstance> _members = Lists.newList();

		private int _hauntedHouseStatus = STATUS_NONE;

		private int _winnersCount = 0;

		private int _goalCount = 0;

		private static L1HauntedHouse _instance;

		public static L1HauntedHouse Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1HauntedHouse();
				}
				return _instance;
			}
		}

		private void readyHauntedHouse()
		{
			HauntedHouseStatus = STATUS_READY;
			L1HauntedHouseReadyTimer hhrTimer = new L1HauntedHouseReadyTimer(this);
			hhrTimer.begin();
		}

		private void startHauntedHouse()
		{
			HauntedHouseStatus = STATUS_PLAYING;
			int membersCount = MembersCount;
			if (membersCount <= 4)
			{
				WinnersCount = 1;
			}
			else if ((5 >= membersCount) && (membersCount <= 7))
			{
				WinnersCount = 2;
			}
			else if ((8 >= membersCount) && (membersCount <= 10))
			{
				WinnersCount = 3;
			}
			foreach (L1PcInstance pc in MembersArray)
			{
				L1SkillUse l1skilluse = new L1SkillUse();
				l1skilluse.handleCommands(pc, CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
				L1PolyMorph.doPoly(pc, 6284, 300, L1PolyMorph.MORPH_BY_NPC);
			}

			foreach (L1Object @object in L1World.Instance.Object)
			{
				if (@object is L1DoorInstance)
				{
					L1DoorInstance door = (L1DoorInstance) @object;
					if (door.MapId == 5140)
					{
						door.open();
					}
				}
			}
		}

		public virtual void endHauntedHouse()
		{
			HauntedHouseStatus = STATUS_NONE;
			WinnersCount = 0;
			GoalCount = 0;
			foreach (L1PcInstance pc in MembersArray)
			{
				if (pc.MapId == 5140)
				{
					L1SkillUse l1skilluse = new L1SkillUse();
					l1skilluse.handleCommands(pc, CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
					L1Teleport.teleport(pc, 32624, 32813, (short) 4, 5, true);
				}
			}
			clearMembers();
			foreach (L1Object @object in L1World.Instance.Object)
			{
				if (@object is L1DoorInstance)
				{
					L1DoorInstance door = (L1DoorInstance) @object;
					if (door.MapId == 5140)
					{
						door.close();
					}
				}
			}
		}

		public virtual void removeRetiredMembers()
		{
			L1PcInstance[] temp = MembersArray;
			foreach (L1PcInstance element in temp)
			{
				if (element.MapId != 5140)
				{
					removeMember(element);
				}
			}
		}

		public virtual void sendMessage(int type, string msg)
		{
			foreach (L1PcInstance pc in MembersArray)
			{
				pc.sendPackets(new S_ServerMessage(type, msg));
			}
		}

		public virtual void addMember(L1PcInstance pc)
		{
			if (!_members.Contains(pc))
			{
				_members.Add(pc);
			}
			if ((MembersCount == 1) && (HauntedHouseStatus == STATUS_NONE))
			{
				readyHauntedHouse();
			}
		}

		public virtual void removeMember(L1PcInstance pc)
		{
			_members.Remove(pc);
		}

		public virtual void clearMembers()
		{
			_members.Clear();
		}

		public virtual bool isMember(L1PcInstance pc)
		{
			return _members.Contains(pc);
		}

		public virtual L1PcInstance[] MembersArray
		{
			get
			{
				return ((List<L1PcInstance>)_members).ToArray();
			}
		}

		public virtual int MembersCount
		{
			get
			{
				return _members.Count;
			}
		}

		public int HauntedHouseStatus
		{
			set
			{
				_hauntedHouseStatus = value;
			}
			get
			{
				return _hauntedHouseStatus;
			}
		}


		public int WinnersCount
		{
			set
			{
				_winnersCount = value;
			}
			get
			{
				return _winnersCount;
			}
		}


		public virtual int GoalCount
		{
			set
			{
				_goalCount = value;
			}
			get
			{
				return _goalCount;
			}
		}


		public class L1HauntedHouseReadyTimer : TimerTask
		{
			private readonly L1HauntedHouse outerInstance;


			public L1HauntedHouseReadyTimer(L1HauntedHouse outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance.startHauntedHouse();
				L1HauntedHouseTimer hhTimer = new L1HauntedHouseTimer(outerInstance);
				hhTimer.begin();
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 90000); // 90秒くらい？
			}

		}

		public class L1HauntedHouseTimer : TimerTask
		{
			private readonly L1HauntedHouse outerInstance;


			public L1HauntedHouseTimer(L1HauntedHouse outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance.endHauntedHouse();
				cancel();
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 300000); // 5分
			}

		}

	}

}