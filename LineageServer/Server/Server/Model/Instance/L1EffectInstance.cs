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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_BALANCE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_IGNITION_TO_ALLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_IGNITION_TO_ENEMY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_QUAKE_TO_ALLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_QUAKE_TO_ENEMY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_SHOCK_TO_ALLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CUBE_SHOCK_TO_ENEMY;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using WarTimeController = LineageServer.Server.Server.WarTimeController;
	using SkillsTable = LineageServer.Server.Server.datatables.SkillsTable;
	using L1CastleLocation = LineageServer.Server.Server.Model.L1CastleLocation;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1Cube = LineageServer.Server.Server.Model.L1Cube;
	using L1Magic = LineageServer.Server.Server.Model.L1Magic;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1DamagePoison = LineageServer.Server.Server.Model.poison.L1DamagePoison;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_OwnCharAttrDef = LineageServer.Server.Server.serverpackets.S_OwnCharAttrDef;
	using S_RemoveObject = LineageServer.Server.Server.serverpackets.S_RemoveObject;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	[Serializable]
	public class L1EffectInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;

		private const int FW_DAMAGE_INTERVAL = 1000;

		private const int CUBE_INTERVAL = 500; // キューブ範囲内に居るキャラクターをチェックする間隔

		private const int CUBE_TIME = 8000; // 効果時間8秒?

		private const int POISON_INTERVAL = 1000;

		public L1EffectInstance(L1Npc template) : base(template)
		{

			int npcId = NpcTemplate.get_npcId();
			if (npcId == 81157)
			{ // FW
				GeneralThreadPool.Instance.schedule(new FwDamageTimer(this, this), 0);
			}
			else if ((npcId == 80149) || (npcId == 80150) || (npcId == 80151) || (npcId == 80152))
			{ // キューブ[バランス]
				GeneralThreadPool.Instance.schedule(new CubeTimer(this, this), 0);
			}
			else if (npcId == 93002)
			{ // 毒霧
				GeneralThreadPool.Instance.schedule(new PoisonTimer(this, this), 0);
			}
		}

		public override void onAction(L1PcInstance pc)
		{
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

		internal class FwDamageTimer : IRunnableStart
		{
			private readonly L1EffectInstance outerInstance;

			internal L1EffectInstance _effect;

			public FwDamageTimer(L1EffectInstance outerInstance, L1EffectInstance effect)
			{
				this.outerInstance = outerInstance;
				_effect = effect;
			}

			public override void run()
			{
				while (!outerInstance._destroyed)
				{
					try
					{
						foreach (L1Object objects in L1World.Instance.getVisibleObjects(_effect, 0))
						{
							if (objects is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance) objects;
								if (pc.Dead)
								{
									continue;
								}
								if (pc.ZoneType == 1)
								{
									bool isNowWar = false;
									int castleId = L1CastleLocation.getCastleIdByArea(pc);
									if (castleId > 0)
									{
										isNowWar = WarTimeController.Instance.isNowWar(castleId);
									}
									if (!isNowWar)
									{
										continue;
									}
								}
								L1Magic magic = new L1Magic(_effect, pc);
								int damage = magic.calcPcFireWallDamage();
								if (damage == 0)
								{
									continue;
								}
								pc.sendPackets(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
								pc.broadcastPacket(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Damage));
								pc.receiveDamage(_effect, damage, false);
							}
							else if (objects is L1MonsterInstance)
							{
								L1MonsterInstance mob = (L1MonsterInstance) objects;
								if (mob.Dead)
								{
									continue;
								}
								L1Magic magic = new L1Magic(_effect, mob);
								int damage = magic.calcNpcFireWallDamage();
								if (damage == 0)
								{
									continue;
								}
								mob.broadcastPacket(new S_DoActionGFX(mob.Id, ActionCodes.ACTION_Damage));
								mob.receiveDamage(_effect, damage);
							}
						}
						Thread.Sleep(FW_DAMAGE_INTERVAL);
					}
					catch (InterruptedException)
					{
						// ignore
					}
				}
			}
		}

		internal class CubeTimer : IRunnableStart
		{
			private readonly L1EffectInstance outerInstance;

			internal L1EffectInstance _effect;

			public CubeTimer(L1EffectInstance outerInstance, L1EffectInstance effect)
			{
				this.outerInstance = outerInstance;
				_effect = effect;
			}

			public override void run()
			{
				while (!outerInstance._destroyed)
				{
					try
					{
						foreach (L1Object objects in L1World.Instance.getVisibleObjects(_effect, 3))
						{
							if (objects is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance) objects;
								if (pc.Dead)
								{
									continue;
								}
								L1PcInstance user = outerInstance.User; // Cube使用者
								if (pc.Id == user.Id)
								{
									outerInstance.cubeToAlly(pc, _effect);
									continue;
								}
								if ((pc.Clanid != 0) && (user.Clanid == pc.Clanid))
								{
									outerInstance.cubeToAlly(pc, _effect);
									continue;
								}
								if (pc.InParty && pc.Party.isMember(user))
								{
									outerInstance.cubeToAlly(pc, _effect);
									continue;
								}
								if (pc.ZoneType == 1)
								{ // セーフティーゾーンでは戦争中を除き敵には無効
									bool isNowWar = false;
									int castleId = L1CastleLocation.getCastleIdByArea(pc);
									if (castleId > 0)
									{
										isNowWar = WarTimeController.Instance.isNowWar(castleId);
									}
									if (!isNowWar)
									{
										continue;
									}
									outerInstance.cubeToEnemy(pc, _effect);
								}
								else
								{
									outerInstance.cubeToEnemy(pc, _effect);
								}
							}
							else if (objects is L1MonsterInstance)
							{
								L1MonsterInstance mob = (L1MonsterInstance) objects;
								if (mob.Dead)
								{
									continue;
								}
								outerInstance.cubeToEnemy(mob, _effect);
							}
						}
						Thread.Sleep(CUBE_INTERVAL);
					}
					catch (InterruptedException)
					{
						// ignore
					}
				}
			}
		}

		internal class PoisonTimer : IRunnableStart
		{
			private readonly L1EffectInstance outerInstance;

			internal L1EffectInstance _effect;

			public PoisonTimer(L1EffectInstance outerInstance, L1EffectInstance effect)
			{
				this.outerInstance = outerInstance;
				_effect = effect;
			}

			public override void run()
			{
				while (!outerInstance._destroyed)
				{
					try
					{
						foreach (L1Object objects in L1World.Instance.getVisibleObjects(_effect, 0))
						{
							if (!(objects is L1MonsterInstance))
							{
								L1Character cha = (L1Character) objects;
								L1DamagePoison.doInfection(_effect, cha, 3000, 20);
							}
						}
						Thread.Sleep(POISON_INTERVAL);
					}
					catch (InterruptedException)
					{
						// ignore
					}
				}
			}
		}

		private void cubeToAlly(L1Character cha, L1Character effect)
		{
			int npcId = NpcTemplate.get_npcId();
			int castGfx = SkillsTable.Instance.getTemplate(SkillId).CastGfx;
			L1PcInstance pc = null;

			if (npcId == 80149)
			{ // キューブ[イグニション]
				if (!cha.hasSkillEffect(STATUS_CUBE_IGNITION_TO_ALLY))
				{
					cha.addFire(30);
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_OwnCharAttrDef(pc));
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
					cha.setSkillEffect(STATUS_CUBE_IGNITION_TO_ALLY, CUBE_TIME);
				}
			}
			else if (npcId == 80150)
			{ // キューブ[クエイク]
				if (!cha.hasSkillEffect(STATUS_CUBE_QUAKE_TO_ALLY))
				{
					cha.addEarth(30);
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_OwnCharAttrDef(pc));
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
					cha.setSkillEffect(STATUS_CUBE_QUAKE_TO_ALLY, CUBE_TIME);
				}
			}
			else if (npcId == 80151)
			{ // キューブ[ショック]
				if (!cha.hasSkillEffect(STATUS_CUBE_SHOCK_TO_ALLY))
				{
					cha.addWind(30);
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_OwnCharAttrDef(pc));
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
					cha.setSkillEffect(STATUS_CUBE_SHOCK_TO_ALLY, CUBE_TIME);
				}
			}
			else if (npcId == 80152)
			{ // キューブ[バランス]
				if (!cha.hasSkillEffect(STATUS_CUBE_BALANCE))
				{
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx));
					cha.setSkillEffect(STATUS_CUBE_BALANCE, CUBE_TIME);
					L1Cube cube = new L1Cube(effect, cha, STATUS_CUBE_BALANCE);
					cube.begin();
				}
			}
		}

		private void cubeToEnemy(L1Character cha, L1Character effect)
		{
			int npcId = NpcTemplate.get_npcId();
			int castGfx2 = SkillsTable.Instance.getTemplate(SkillId).CastGfx2;
			L1PcInstance pc = null;
			if (npcId == 80149)
			{ // キューブ[イグニション]
				if (!cha.hasSkillEffect(STATUS_CUBE_IGNITION_TO_ENEMY))
				{
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
					cha.setSkillEffect(STATUS_CUBE_IGNITION_TO_ENEMY, CUBE_TIME);
					L1Cube cube = new L1Cube(effect, cha, STATUS_CUBE_IGNITION_TO_ENEMY);
					cube.begin();
				}
			}
			else if (npcId == 80150)
			{ // キューブ[クエイク]
				if (!cha.hasSkillEffect(STATUS_CUBE_QUAKE_TO_ENEMY))
				{
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
					cha.setSkillEffect(STATUS_CUBE_QUAKE_TO_ENEMY, CUBE_TIME);
					L1Cube cube = new L1Cube(effect, cha, STATUS_CUBE_QUAKE_TO_ENEMY);
					cube.begin();
				}
			}
			else if (npcId == 80151)
			{ // キューブ[ショック]
				if (!cha.hasSkillEffect(STATUS_CUBE_SHOCK_TO_ENEMY))
				{
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
					cha.setSkillEffect(STATUS_CUBE_SHOCK_TO_ENEMY, CUBE_TIME);
					L1Cube cube = new L1Cube(effect, cha, STATUS_CUBE_SHOCK_TO_ENEMY);
					cube.begin();
				}
			}
			else if (npcId == 80152)
			{ // キューブ[バランス]
				if (!cha.hasSkillEffect(STATUS_CUBE_BALANCE))
				{
					if (cha is L1PcInstance)
					{
						pc = (L1PcInstance) cha;
						pc.sendPackets(new S_SkillSound(pc.Id, castGfx2));
					}
					cha.broadcastPacket(new S_SkillSound(cha.Id, castGfx2));
					cha.setSkillEffect(STATUS_CUBE_BALANCE, CUBE_TIME);
					L1Cube cube = new L1Cube(effect, cha, STATUS_CUBE_BALANCE);
					cube.begin();
				}
			}
		}

		private L1PcInstance _pc;

		public virtual L1PcInstance User
		{
			set
			{
				_pc = value;
			}
			get
			{
				return _pc;
			}
		}


		private int _skillId;

		public virtual int SkillId
		{
			set
			{
				_skillId = value;
			}
			get
			{
				return _skillId;
			}
		}


	}

}