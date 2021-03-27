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
namespace LineageServer.Server.Model
{
	using IdFactory = LineageServer.Server.IdFactory;
	using NpcTable = LineageServer.Server.DataSources.NpcTable;
	using UBTable = LineageServer.Server.DataSources.UBTable;
	using L1MonsterInstance = LineageServer.Server.Model.Instance.L1MonsterInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_NPCPack = LineageServer.Serverpackets.S_NPCPack;

	public class L1UbSpawn : IComparable<L1UbSpawn>
	{
		private int _id;

		private int _ubId;

		private int _pattern;

		private int _group;

		private int _npcTemplateId;

		private int _amount;

		private int _spawnDelay;

		private int _sealCount;

		private string _name;

		// --------------------start getter/setter--------------------
		public virtual int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}


		public virtual int UbId
		{
			get
			{
				return _ubId;
			}
			set
			{
				_ubId = value;
			}
		}


		public virtual int Pattern
		{
			get
			{
				return _pattern;
			}
			set
			{
				_pattern = value;
			}
		}


		public virtual int Group
		{
			get
			{
				return _group;
			}
			set
			{
				_group = value;
			}
		}


		public virtual int NpcTemplateId
		{
			get
			{
				return _npcTemplateId;
			}
			set
			{
				_npcTemplateId = value;
			}
		}


		public virtual int Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				_amount = value;
			}
		}


		public virtual int SpawnDelay
		{
			get
			{
				return _spawnDelay;
			}
			set
			{
				_spawnDelay = value;
			}
		}


		public virtual int SealCount
		{
			get
			{
				return _sealCount;
			}
			set
			{
				_sealCount = value;
			}
		}


		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}


		// --------------------end getter/setter--------------------

		public virtual void spawnOne()
		{
			L1UltimateBattle ub = UBTable.Instance.getUb(_ubId);
			L1Location loc = ub.Location.randomLocation((ub.LocX2 - ub.LocX1) / 2, false);
			L1MonsterInstance mob = new L1MonsterInstance(NpcTable.Instance.getTemplate(NpcTemplateId));

			mob.Id = IdFactory.Instance.nextId();
			mob.Heading = 5;
			mob.X = loc.X;
			mob.HomeX = loc.X;
			mob.Y = loc.Y;
			mob.HomeY = loc.Y;
			mob.Map = (short) loc.MapId;
			mob.set_storeDroped(!(3 < Group));
			mob.UbSealCount = SealCount;
			mob.UbId = UbId;

			L1World.Instance.storeObject(mob);
			L1World.Instance.addVisibleObject(mob);

			S_NPCPack s_npcPack = new S_NPCPack(mob);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(mob))
			{
				pc.addKnownObject(mob);
				mob.addKnownObject(pc);
				pc.sendPackets(s_npcPack);
			}
			// モンスターのＡＩを開始
			mob.onNpcAI();
			mob.turnOnOffLight();
			// mob.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
		}

		public virtual void spawnAll()
		{
			for (int i = 0; i < Amount; i++)
			{
				spawnOne();
			}
		}

		public virtual int CompareTo(L1UbSpawn rhs)
		{
			// XXX - 本当はもっと厳密な順序付けがあるはずだが、必要なさそうなので後回し
			if (Id < rhs.Id)
			{
				return -1;
			}
			if (Id > rhs.Id)
			{
				return 1;
			}
			return 0;
		}
	}

}