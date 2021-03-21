using System;
using System.Threading;

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

	using ItemTable = LineageServer.Server.Server.datatables.ItemTable;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using PetTable = LineageServer.Server.Server.datatables.PetTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using L1Pet = LineageServer.Server.Server.Templates.L1Pet;

	public class L1PetMatch
	{
		public const int STATUS_NONE = 0;

		public const int STATUS_READY1 = 1;

		public const int STATUS_READY2 = 2;

		public const int STATUS_PLAYING = 3;

		public const int MAX_PET_MATCH = 1;

		private static readonly short[] PET_MATCH_MAPID = new short[] {5125, 5131, 5132, 5133, 5134};

		private string[] _pc1Name = new string[MAX_PET_MATCH];

		private string[] _pc2Name = new string[MAX_PET_MATCH];

		private L1PetInstance[] _pet1 = new L1PetInstance[MAX_PET_MATCH];

		private L1PetInstance[] _pet2 = new L1PetInstance[MAX_PET_MATCH];

		private static L1PetMatch _instance;

		public static L1PetMatch Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1PetMatch();
				}
				return _instance;
			}
		}

		public virtual int setPetMatchPc(int petMatchNo, L1PcInstance pc, L1PetInstance pet)
		{
			int status = getPetMatchStatus(petMatchNo);
			if (status == STATUS_NONE)
			{
				_pc1Name[petMatchNo] = pc.Name;
				_pet1[petMatchNo] = pet;
				return STATUS_READY1;
			}
			else if (status == STATUS_READY1)
			{
				_pc2Name[petMatchNo] = pc.Name;
				_pet2[petMatchNo] = pet;
				return STATUS_PLAYING;
			}
			else if (status == STATUS_READY2)
			{
				_pc1Name[petMatchNo] = pc.Name;
				_pet1[petMatchNo] = pet;
				return STATUS_PLAYING;
			}
			return STATUS_NONE;
		}

		private int getPetMatchStatus(int petMatchNo)
		{
			lock (this)
			{
				L1PcInstance pc1 = null;
				if (!string.ReferenceEquals(_pc1Name[petMatchNo], null))
				{
					pc1 = L1World.Instance.getPlayer(_pc1Name[petMatchNo]);
				}
				L1PcInstance pc2 = null;
				if (!string.ReferenceEquals(_pc2Name[petMatchNo], null))
				{
					pc2 = L1World.Instance.getPlayer(_pc2Name[petMatchNo]);
				}
        
				if ((pc1 == null) && (pc2 == null))
				{
					return STATUS_NONE;
				}
				if ((pc1 == null) && (pc2 != null))
				{
					if (pc2.MapId == PET_MATCH_MAPID[petMatchNo])
					{
						return STATUS_READY2;
					}
					else
					{
						_pc2Name[petMatchNo] = null;
						_pet2[petMatchNo] = null;
						return STATUS_NONE;
					}
				}
				if ((pc1 != null) && (pc2 == null))
				{
					if (pc1.MapId == PET_MATCH_MAPID[petMatchNo])
					{
						return STATUS_READY1;
					}
					else
					{
						_pc1Name[petMatchNo] = null;
						_pet1[petMatchNo] = null;
						return STATUS_NONE;
					}
				}
        
				// PCが試合場に2人いる場合
				if ((pc1.MapId == PET_MATCH_MAPID[petMatchNo]) && (pc2.MapId == PET_MATCH_MAPID[petMatchNo]))
				{
					return STATUS_PLAYING;
				}
        
				// PCが試合場に1人いる場合
				if (pc1.MapId == PET_MATCH_MAPID[petMatchNo])
				{
					_pc2Name[petMatchNo] = null;
					_pet2[petMatchNo] = null;
					return STATUS_READY1;
				}
				if (pc2.MapId == PET_MATCH_MAPID[petMatchNo])
				{
					_pc1Name[petMatchNo] = null;
					_pet1[petMatchNo] = null;
					return STATUS_READY2;
				}
				return STATUS_NONE;
			}
		}

		private int decidePetMatchNo()
		{
			// 相手が待機中の試合を探す
			for (int i = 0; i < MAX_PET_MATCH; i++)
			{
				int status = getPetMatchStatus(i);
				if ((status == STATUS_READY1) || (status == STATUS_READY2))
				{
					return i;
				}
			}
			// 待機中の試合がなければ空いている試合を探す
			for (int i = 0; i < MAX_PET_MATCH; i++)
			{
				int status = getPetMatchStatus(i);
				if (status == STATUS_NONE)
				{
					return i;
				}
			}
			return -1;
		}

		public virtual bool enterPetMatch(L1PcInstance pc, int amuletId)
		{
			lock (this)
			{
				int petMatchNo = decidePetMatchNo();
				if (petMatchNo == -1)
				{
					return false;
				}
        
				L1PetInstance pet = withdrawPet(pc, amuletId);
				L1Teleport.teleport(pc, 32799, 32868, PET_MATCH_MAPID[petMatchNo], 0, true);
        
				L1PetMatchReadyTimer timer = new L1PetMatchReadyTimer(this, petMatchNo, pc, pet);
				timer.begin();
				return true;
			}
		}

		private L1PetInstance withdrawPet(L1PcInstance pc, int amuletId)
		{
			L1Pet l1pet = PetTable.Instance.getTemplate(amuletId);
			if (l1pet == null)
			{
				return null;
			}
			L1Npc npcTemp = NpcTable.Instance.getTemplate(l1pet.get_npcid());
			L1PetInstance pet = new L1PetInstance(npcTemp, pc, l1pet);
			pet.Petcost = 6;
			return pet;
		}

		public virtual void startPetMatch(int petMatchNo)
		{
			_pet1[petMatchNo].CurrentPetStatus = 1;
			_pet1[petMatchNo].Target = _pet2[petMatchNo];

			_pet2[petMatchNo].CurrentPetStatus = 1;
			_pet2[petMatchNo].Target = _pet1[petMatchNo];

			L1PetMatchTimer timer = new L1PetMatchTimer(this, _pet1[petMatchNo], _pet2[petMatchNo], petMatchNo);
			timer.begin();
		}

		public virtual void endPetMatch(int petMatchNo, int winNo)
		{
			L1PcInstance pc1 = L1World.Instance.getPlayer(_pc1Name[petMatchNo]);
			L1PcInstance pc2 = L1World.Instance.getPlayer(_pc2Name[petMatchNo]);
			if (winNo == 1)
			{
				giveMedal(pc1, petMatchNo, true);
				giveMedal(pc2, petMatchNo, false);
			}
			else if (winNo == 2)
			{
				giveMedal(pc1, petMatchNo, false);
				giveMedal(pc2, petMatchNo, true);
			}
			else if (winNo == 3)
			{ // 引き分け
				giveMedal(pc1, petMatchNo, false);
				giveMedal(pc2, petMatchNo, false);
			}
			qiutPetMatch(petMatchNo);
		}

		private void giveMedal(L1PcInstance pc, int petMatchNo, bool isWin)
		{
			if (pc == null)
			{
				return;
			}
			if (pc.MapId != PET_MATCH_MAPID[petMatchNo])
			{
				return;
			}
			if (isWin)
			{
				pc.sendPackets(new S_ServerMessage(1166, pc.Name)); // %0%sペットマッチで勝利を収めました。
				L1ItemInstance item = ItemTable.Instance.createItem(41309);
				int count = 3;
				if (item != null)
				{
					if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
					{
						item.Count = count;
						pc.Inventory.storeItem(item);
						pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
					}
				}
			}
			else
			{
				L1ItemInstance item = ItemTable.Instance.createItem(41309);
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
			}
		}

		private void qiutPetMatch(int petMatchNo)
		{
			L1PcInstance pc1 = L1World.Instance.getPlayer(_pc1Name[petMatchNo]);
			if ((pc1 != null) && (pc1.MapId == PET_MATCH_MAPID[petMatchNo]))
			{
				foreach (object @object in pc1.PetList.Values.ToArray())
				{
					if (@object is L1PetInstance)
					{
						L1PetInstance pet = (L1PetInstance) @object;
						pet.dropItem();
						pc1.PetList.Remove(pet.Id);
						pet.deleteMe();
					}
				}
				L1Teleport.teleport(pc1, 32630, 32744, (short) 4, 4, true);
			}
			_pc1Name[petMatchNo] = null;
			_pet1[petMatchNo] = null;

			L1PcInstance pc2 = L1World.Instance.getPlayer(_pc2Name[petMatchNo]);
			if ((pc2 != null) && (pc2.MapId == PET_MATCH_MAPID[petMatchNo]))
			{
				foreach (object @object in pc2.PetList.Values.ToArray())
				{
					if (@object is L1PetInstance)
					{
						L1PetInstance pet = (L1PetInstance) @object;
						pet.dropItem();
						pc2.PetList.Remove(pet.Id);
						pet.deleteMe();
					}
				}
				L1Teleport.teleport(pc2, 32630, 32744, (short) 4, 4, true);
			}
			_pc2Name[petMatchNo] = null;
			_pet2[petMatchNo] = null;
		}

		public class L1PetMatchReadyTimer : TimerTask
		{
			private readonly L1PetMatch outerInstance;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			internal Logger _log = Logger.getLogger(typeof(L1PetMatchReadyTimer).FullName);

			internal readonly int _petMatchNo;

			internal readonly L1PcInstance _pc;

			internal readonly L1PetInstance _pet;

			public L1PetMatchReadyTimer(L1PetMatch outerInstance, int petMatchNo, L1PcInstance pc, L1PetInstance pet)
			{
				this.outerInstance = outerInstance;
				_petMatchNo = petMatchNo;
				_pc = pc;
				_pet = pet;
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 3000);
			}

			public override void run()
			{
				try
				{
					for (;;)
					{
						Thread.Sleep(1000);
						if ((_pc == null) || (_pet == null))
						{
							cancel();
							return;
						}

						if (_pc.Teleport)
						{
							continue;
						}
						if (L1PetMatch.Instance.setPetMatchPc(_petMatchNo, _pc, _pet) == L1PetMatch.STATUS_PLAYING)
						{
							L1PetMatch.Instance.startPetMatch(_petMatchNo);
						}
						cancel();
						return;
					}
				}
				catch (Exception e)
				{
					_log.log(Level.WARNING, e.Message, e);
				}
			}

		}

		public class L1PetMatchTimer : TimerTask
		{
			private readonly L1PetMatch outerInstance;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			internal Logger _log = Logger.getLogger(typeof(L1PetMatchTimer).FullName);

			internal readonly L1PetInstance _pet1;

			internal readonly L1PetInstance _pet2;

			internal readonly int _petMatchNo;

			internal int _counter = 0;

			public L1PetMatchTimer(L1PetMatch outerInstance, L1PetInstance pet1, L1PetInstance pet2, int petMatchNo)
			{
				this.outerInstance = outerInstance;
				_pet1 = pet1;
				_pet2 = pet2;
				_petMatchNo = petMatchNo;
			}

			public virtual void begin()
			{
				Timer timer = new Timer();
				timer.schedule(this, 0);
			}

			public override void run()
			{
				try
				{
					for (;;)
					{
						Thread.Sleep(3000);
						_counter++;
						if ((_pet1 == null) || (_pet2 == null))
						{
							cancel();
							return;
						}

						if (_pet1.Dead || _pet2.Dead)
						{
							int winner = 0;
							if (!_pet1.Dead && _pet2.Dead)
							{
								winner = 1;
							}
							else if (_pet1.Dead && !_pet2.Dead)
							{
								winner = 2;
							}
							else
							{
								winner = 3;
							}
							L1PetMatch.Instance.endPetMatch(_petMatchNo, winner);
							cancel();
							return;
						}

						if (_counter == 100)
						{ // 5分経っても終わらない場合は引き分け
							L1PetMatch.Instance.endPetMatch(_petMatchNo, 3);
							cancel();
							return;
						}
					}
				}
				catch (Exception e)
				{
					_log.log(Level.WARNING, e.Message, e);
				}
			}

		}

	}

}