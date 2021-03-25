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
namespace LineageServer.Server.Server.Model.Instance
{
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using RunnableExecuter = LineageServer.Server.Server.RunnableExecuter;
	using WarTimeController = LineageServer.Server.Server.WarTimeController;
	using L1Attack = LineageServer.Server.Server.Model.L1Attack;
	using L1CastleLocation = LineageServer.Server.Server.Model.L1CastleLocation;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1War = LineageServer.Server.Server.Model.L1War;
	using L1WarSpawn = LineageServer.Server.Server.Model.L1WarSpawn;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_NPCPack = LineageServer.Server.Server.serverpackets.S_NPCPack;
	using S_RemoveObject = LineageServer.Server.Server.serverpackets.S_RemoveObject;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	[Serializable]
	public class L1TowerInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;

		public L1TowerInstance(L1Npc template) : base(template)
		{
		}

		private L1Character _lastattacker;

		private int _castle_id;

		private int _crackStatus;

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_NPCPack(this));
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			if ((CurrentHp > 0) && !Dead)
			{
				L1Attack attack = new L1Attack(pc, this, skillId);
				if (attack.calcHit())
				{
					attack.calcDamage();
					attack.addPcPoisonAttack(pc, this);
					attack.addChaserAttack();
				}
				attack.action();
				attack.commit();
			}
		}

		public override void receiveDamage(L1Character attacker, int damage)
		{ // 攻撃でＨＰを減らすときはここを使用
			if (_castle_id == 0)
			{ // 初期設定で良いがいい場所がない
				if (SubTower)
				{
					_castle_id = L1CastleLocation.ADEN_CASTLE_ID;
				}
				else
				{
					_castle_id = L1CastleLocation.getCastleId(X, Y, MapId);
				}
			}

			if ((_castle_id > 0) && WarTimeController.Instance.isNowWar(_castle_id))
			{ // 戦争時間内

				// アデン城のメインタワーはサブタワーが3つ以上破壊されている場合のみ攻撃可能
				if ((_castle_id == L1CastleLocation.ADEN_CASTLE_ID) && !SubTower)
				{
					int subTowerDeadCount = 0;
					foreach (L1Object l1object in L1World.Instance.Object)
					{
						if (l1object is L1TowerInstance)
						{
							L1TowerInstance tower = (L1TowerInstance) l1object;
							if (tower.SubTower && tower.Dead)
							{
								subTowerDeadCount++;
								if (subTowerDeadCount == 4)
								{
									break;
								}
							}
						}
					}
					if (subTowerDeadCount < 3)
					{
						return;
					}
				}

				L1PcInstance pc = null;
				if (attacker is L1PcInstance)
				{
					pc = (L1PcInstance) attacker;
				}
				else if (attacker is L1PetInstance)
				{
					pc = (L1PcInstance)((L1PetInstance) attacker).Master;
				}
				else if (attacker is L1SummonInstance)
				{
					pc = (L1PcInstance)((L1SummonInstance) attacker).Master;
				}
				if (pc == null)
				{
					return;
				}

				// 布告しているかチェック。但し、城主が居ない場合は布告不要
				bool existDefenseClan = false;
				foreach (L1Clan clan in L1World.Instance.AllClans)
				{
					int clanCastleId = clan.CastleId;
					if (clanCastleId == _castle_id)
					{
						existDefenseClan = true;
						break;
					}
				}
				bool isProclamation = false;
				// 全戦争リストを取得
				foreach (L1War war in L1World.Instance.WarList)
				{
					if (_castle_id == war.GetCastleId())
					{ // 今居る城の戦争
						isProclamation = war.CheckClanInWar(pc.Clanname);
						break;
					}
				}
				if ((existDefenseClan == true) && (isProclamation == false))
				{ // 城主が居て、布告していない場合
					return;
				}

				if ((CurrentHp > 0) && !Dead)
				{
					int newHp = CurrentHp - damage;
					if ((newHp <= 0) && !Dead)
					{
						CurrentHpDirect = 0;
						Dead = true;
						Status = ActionCodes.ACTION_TowerDie;
						_lastattacker = attacker;
						_crackStatus = 0;
						Death death = new Death(this);
						RunnableExecuter.Instance.execute(death);
						// Death(attacker);
					}
					if (newHp > 0)
					{
						CurrentHp = newHp;
						if ((MaxHp * 1 / 4) > CurrentHp)
						{
							if (_crackStatus != 3)
							{
								broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_TowerCrack3));
								Status = ActionCodes.ACTION_TowerCrack3;
								_crackStatus = 3;
							}
						}
						else if ((MaxHp * 2 / 4) > CurrentHp)
						{
							if (_crackStatus != 2)
							{
								broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_TowerCrack2));
								Status = ActionCodes.ACTION_TowerCrack2;
								_crackStatus = 2;
							}
						}
						else if ((MaxHp * 3 / 4) > CurrentHp)
						{
							if (_crackStatus != 1)
							{
								broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_TowerCrack1));
								Status = ActionCodes.ACTION_TowerCrack1;
								_crackStatus = 1;
							}
						}
					}
				}
				else if (!Dead)
				{ // 念のため
					Dead = true;
					Status = ActionCodes.ACTION_TowerDie;
					_lastattacker = attacker;
					Death death = new Death(this);
					RunnableExecuter.Instance.execute(death);
					// Death(attacker);
				}
			}
		}

		public override int CurrentHp
		{
			set
			{
				int currentHp = value;
				if (currentHp >= MaxHp)
				{
					currentHp = MaxHp;
				}
				CurrentHpDirect = currentHp;
			}
		}

		internal class Death : IRunnableStart
		{
			internal bool InstanceFieldsInitialized = false;

			internal virtual void InitializeInstanceFields()
			{
				lastAttacker = outerInstance._lastattacker;
				@object = L1World.Instance.findObject(outerInstance.Id);
				npc = (L1TowerInstance) @object;
			}

			private readonly L1TowerInstance outerInstance;

			public Death(L1TowerInstance outerInstance)
			{
				this.outerInstance = outerInstance;

				if (!InstanceFieldsInitialized)
				{
					InitializeInstanceFields();
					InstanceFieldsInitialized = true;
				}
			}

			internal L1Character lastAttacker;

			internal L1Object @object;

			internal L1TowerInstance npc;

			public override void run()
			{
				outerInstance.CurrentHpDirect = 0;
				outerInstance.Dead = true;
				outerInstance.Status = ActionCodes.ACTION_TowerDie;
				int targetobjid = npc.Id;

				npc.Map.setPassable(npc.Location, true);

				npc.broadcastPacket(new S_DoActionGFX(targetobjid, ActionCodes.ACTION_TowerDie));

				// クラウンをspawnする
				if (!outerInstance.SubTower)
				{
					L1WarSpawn warspawn = new L1WarSpawn();
					warspawn.SpawnCrown(outerInstance._castle_id);
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
			allTargetClear();
			_master = null;
			L1World.Instance.removeVisibleObject(this);
			L1World.Instance.removeObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				pc.removeKnownObject(this);
				pc.sendPackets(new S_RemoveObject(this));
			}
			removeAllKnownObjects();
		}

		public virtual bool SubTower
		{
			get
			{
				return ((NpcTemplate.get_npcId() == 81190) || (NpcTemplate.get_npcId() == 81191) || (NpcTemplate.get_npcId() == 81192) || (NpcTemplate.get_npcId() == 81193));
			}
		}

	}

}