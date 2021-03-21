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
namespace LineageServer.Server.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ABSOLUTE_BARRIER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.BERSERKERS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.COUNTER_MAGIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.EARTH_BIND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.FREEZING_BLIZZARD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.FREEZING_BREATH;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ICE_LANCE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ILLUSION_AVATAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_FREEZE;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using WarTimeController = LineageServer.Server.Server.WarTimeController;
	using SkillsTable = LineageServer.Server.Server.datatables.SkillsTable;
	using WeaponSkillTable = LineageServer.Server.Server.datatables.WeaponSkillTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using L1SkillUse = LineageServer.Server.Server.Model.skill.L1SkillUse;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_EffectLocation = LineageServer.Server.Server.serverpackets.S_EffectLocation;
	using S_Paralysis = LineageServer.Server.Server.serverpackets.S_Paralysis;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using S_UseAttackSkill = LineageServer.Server.Server.serverpackets.S_UseAttackSkill;
	using L1Skills = LineageServer.Server.Server.Templates.L1Skills;
	using Random = LineageServer.Server.Server.utils.Random;

	// Referenced classes of package l1j.server.server.model:
	// L1PcInstance

	public class L1WeaponSkill
	{

		private int _weaponId;

		private int _probability;

		private int _fixDamage;

		private int _randomDamage;

		private int _area;

		private int _skillId;

		private int _skillTime;

		private int _effectId;

		private int _effectTarget; // エフェクトの対象 0:相手 1:自分

		private bool _isArrowType;

		private int _attr;

		public L1WeaponSkill(int weaponId, int probability, int fixDamage, int randomDamage, int area, int skillId, int skillTime, int effectId, int effectTarget, bool isArrowType, int attr)
		{
			_weaponId = weaponId;
			_probability = probability;
			_fixDamage = fixDamage;
			_randomDamage = randomDamage;
			_area = area;
			_skillId = skillId;
			_skillTime = skillTime;
			_effectId = effectId;
			_effectTarget = effectTarget;
			_isArrowType = isArrowType;
			_attr = attr;
		}

		public virtual int WeaponId
		{
			get
			{
				return _weaponId;
			}
		}

		public virtual int Probability
		{
			get
			{
				return _probability;
			}
		}

		public virtual int FixDamage
		{
			get
			{
				return _fixDamage;
			}
		}

		public virtual int RandomDamage
		{
			get
			{
				return _randomDamage;
			}
		}

		public virtual int Area
		{
			get
			{
				return _area;
			}
		}

		public virtual int SkillId
		{
			get
			{
				return _skillId;
			}
		}

		public virtual int SkillTime
		{
			get
			{
				return _skillTime;
			}
		}

		public virtual int EffectId
		{
			get
			{
				return _effectId;
			}
		}

		public virtual int EffectTarget
		{
			get
			{
				return _effectTarget;
			}
		}

		public virtual bool ArrowType
		{
			get
			{
				return _isArrowType;
			}
		}

		public virtual int Attr
		{
			get
			{
				return _attr;
			}
		}

		public static double getWeaponSkillDamage(L1PcInstance pc, L1Character cha, int weaponId)
		{
			L1WeaponSkill weaponSkill = WeaponSkillTable.Instance.getTemplate(weaponId);
			if ((pc == null) || (cha == null) || (weaponSkill == null))
			{
				return 0;
			}

			int chance = RandomHelper.Next(100) + 1;
			if (weaponSkill.Probability < chance)
			{
				return 0;
			}

			int skillId = weaponSkill.SkillId;
			if (skillId != 0)
			{
				L1Skills skill = SkillsTable.Instance.getTemplate(skillId);
				if ((skill != null) && skill.Target.Equals("buff"))
				{
					if (!isFreeze(cha))
					{ // 凍結状態orカウンターマジック中
						cha.setSkillEffect(skillId, weaponSkill.SkillTime * 1000);
					}
				}
			}

			int effectId = weaponSkill.EffectId;
			if (effectId != 0)
			{
				int chaId = 0;
				if (weaponSkill.EffectTarget == 0)
				{
					chaId = cha.Id;
				}
				else
				{
					chaId = pc.Id;
				}
				bool isArrowType = weaponSkill.ArrowType;
				if (!isArrowType)
				{
					pc.sendPackets(new S_SkillSound(chaId, effectId));
					pc.broadcastPacket(new S_SkillSound(chaId, effectId));
				}
				else
				{
					int[] data = new int[] {ActionCodes.ACTION_Attack, 0, effectId, 6};
					S_UseAttackSkill packet = new S_UseAttackSkill(pc, cha.Id, cha.X, cha.Y, data, false);
					pc.sendPackets(packet);
					pc.broadcastPacket(packet);
				}
			}

			double damage = 0;
			int randomDamage = weaponSkill.RandomDamage;
			if (randomDamage != 0)
			{
				damage = RandomHelper.Next(randomDamage);
			}
			damage += weaponSkill.FixDamage;

			int area = weaponSkill.Area;
			if ((area > 0) || (area == -1))
			{ // 範囲の場合
				foreach (L1Object @object in L1World.Instance.getVisibleObjects(cha, area))
				{
					if (@object == null)
					{
						continue;
					}
					if (!(@object is L1Character))
					{
						continue;
					}
					if (@object.Id == pc.Id)
					{
						continue;
					}
					if (@object.Id == cha.Id)
					{ // 攻撃対象はL1Attackで処理するため除外
						continue;
					}

					// 攻撃対象がMOBの場合は、範囲内のMOBにのみ当たる
					// 攻撃対象がPC,Summon,Petの場合は、範囲内のPC,Summon,Pet,MOBに当たる
					if (cha is L1MonsterInstance)
					{
						if (!(@object is L1MonsterInstance))
						{
							continue;
						}
					}
					if ((cha is L1PcInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
					{
						if (!((@object is L1PcInstance) || (@object is L1SummonInstance) || (@object is L1PetInstance) || (@object is L1MonsterInstance)))
						{
							continue;
						}
					}

					// 判斷是否在攻城戰中
					bool isNowWar = false;
					int castleId = L1CastleLocation.getCastleIdByArea((L1Character)@object);
					if (castleId > 0)
					{
						isNowWar = WarTimeController.Instance.isNowWar(castleId);
					}
					if (!isNowWar)
					{ // 非攻城戰區域
						// 對象不是怪物 且在安全區 不會打到
						if (!(@object is L1MonsterInstance) && ((L1Character)@object).ZoneType == 1)
						{
							continue;
						}
						// 寵物減傷
						if (@object is L1PetInstance)
						{
							damage /= 8;
						}
						else if (@object is L1SummonInstance)
						{
							L1SummonInstance summon = (L1SummonInstance) @object;
							if (summon.ExsistMaster)
							{
								damage /= 8;
							}
						}
					}

					damage = calcDamageReduction(pc, (L1Character) @object, damage, weaponSkill.Attr);
					if (damage <= 0)
					{
						continue;
					}
					if (@object is L1PcInstance)
					{
						L1PcInstance targetPc = (L1PcInstance) @object;
						targetPc.sendPackets(new S_DoActionGFX(targetPc.Id, ActionCodes.ACTION_Damage));
						targetPc.broadcastPacket(new S_DoActionGFX(targetPc.Id, ActionCodes.ACTION_Damage));
						targetPc.receiveDamage(pc, (int) damage, false);
					}
					else if ((@object is L1SummonInstance) || (@object is L1PetInstance) || (@object is L1MonsterInstance))
					{
						L1NpcInstance targetNpc = (L1NpcInstance) @object;
						targetNpc.broadcastPacket(new S_DoActionGFX(targetNpc.Id, ActionCodes.ACTION_Damage));
						targetNpc.receiveDamage(pc, (int) damage);
					}
				}
			}

			return calcDamageReduction(pc, cha, damage, weaponSkill.Attr);
		}

		public static double getBaphometStaffDamage(L1PcInstance pc, L1Character cha)
		{
			double dmg = 0;
			int chance = RandomHelper.Next(100) + 1;
			if (14 >= chance)
			{
				int locx = cha.X;
				int locy = cha.Y;
				int sp = pc.Sp;
				int intel = pc.Int;
				double bsk = 0;
				if (pc.hasSkillEffect(L1SkillId.BERSERKERS))
				{
					bsk = 0.2;
				}
				dmg = (intel + sp) * (1.8 + bsk) + RandomHelper.Next(intel + sp) * 1.8;
				S_EffectLocation packet = new S_EffectLocation(locx, locy, 129);
				pc.sendPackets(packet);
				pc.broadcastPacket(packet);
			}
			return calcDamageReduction(pc, cha, dmg, L1Skills.ATTR_EARTH);
		}

		/// <summary>
		/// 骰子匕首 </summary>
		public static double getDiceDaggerDamage(L1PcInstance pc, L1Character cha, L1ItemInstance weapon)
		{
			double dmg = 0;
			int chance = RandomHelper.Next(100) + 1;
			if (2 >= chance)
			{
				dmg = cha.CurrentHp * 2 / 3;
				if (cha.CurrentHp - dmg < 0)
				{
					dmg = 0;
				}
				string msg = weapon.LogName;
				pc.sendPackets(new S_ServerMessage(158, msg));
				// \f1%0%s 消失。
				pc.Inventory.removeItem(weapon, 1);
			}
			return dmg;
		}

		public static double getKiringkuDamage(L1PcInstance pc, L1Character cha)
		{
			int dmg = 0;
			int dice = 5;
			int diceCount = 2;
			int value = 0;
			int kiringkuDamage = 0;
			int charaIntelligence = 0;
			if (pc.Weapon.Item.ItemId == 270)
			{
				value = 16;
			}
			else
			{
				value = 14;
			}

			for (int i = 0; i < diceCount; i++)
			{
				kiringkuDamage += (RandomHelper.Next(dice) + 1);
			}
			kiringkuDamage += value;

			int spByItem = pc.Sp - pc.TrueSp; // アイテムによるSP変動
			charaIntelligence = pc.Int + spByItem - 12;
			if (charaIntelligence < 1)
			{
				charaIntelligence = 1;
			}
			double kiringkuCoefficientA = (1.0 + charaIntelligence * 3.0 / 32.0);

			kiringkuDamage *= (int)kiringkuCoefficientA;

			double kiringkuFloor = Math.Floor(kiringkuDamage);

			dmg += (int)(kiringkuFloor + pc.Weapon.EnchantLevel + pc.OriginalMagicDamage);

			if (pc.hasSkillEffect(L1SkillId.ILLUSION_AVATAR))
			{
				dmg += 10;
			}

			if (pc.Weapon.Item.ItemId == 270)
			{
				pc.sendPackets(new S_SkillSound(pc.Id, 6983));
				pc.broadcastPacket(new S_SkillSound(pc.Id, 6983));
			}
			else
			{
				pc.sendPackets(new S_SkillSound(pc.Id, 7049));
				pc.broadcastPacket(new S_SkillSound(pc.Id, 7049));
			}

			return calcDamageReduction(pc, cha, dmg, 0);
		}

		public static double getAreaSkillWeaponDamage(L1PcInstance pc, L1Character cha, int weaponId)
		{
			double dmg = 0;
			int probability = 0;
			int attr = 0;
			int chance = RandomHelper.Next(100) + 1;
			if (weaponId == 263 || weaponId == 287)
			{ // フリージングランサー
				probability = 5;
				attr = L1Skills.ATTR_WATER;
			}
			else if (weaponId == 260)
			{ // レイジングウィンド
				probability = 4;
				attr = L1Skills.ATTR_WIND;
			}
			if (probability >= chance)
			{
				int sp = pc.Sp;
				int intel = pc.Int;
				int area = 0;
				int effectTargetId = 0;
				int effectId = 0;
				L1Character areaBase = cha;
				double damageRate = 0;

				if (weaponId == 263 || weaponId == 290)
				{ // フリージングランサー
					area = 3;
					damageRate = 1.4D;
					effectTargetId = cha.Id;
					effectId = 1804;
					areaBase = cha;
				}
				else if (weaponId == 260)
				{ // レイジングウィンド
					area = 4;
					damageRate = 1.5D;
					effectTargetId = pc.Id;
					effectId = 758;
					areaBase = pc;
				}
				double bsk = 0;
				if (pc.hasSkillEffect(L1SkillId.BERSERKERS))
				{
					bsk = 0.2;
				}
				dmg = (intel + sp) * (damageRate + bsk) + RandomHelper.Next(intel + sp) * damageRate;
				pc.sendPackets(new S_SkillSound(effectTargetId, effectId));
				pc.broadcastPacket(new S_SkillSound(effectTargetId, effectId));

				foreach (L1Object @object in L1World.Instance.getVisibleObjects(areaBase, area))
				{
					if (@object == null)
					{
						continue;
					}
					if (!(@object is L1Character))
					{
						continue;
					}
					if (@object.Id == pc.Id)
					{
						continue;
					}
					if (@object.Id == cha.Id)
					{ // 攻撃対象は除外
						continue;
					}

					// 攻撃対象がMOBの場合は、範囲内のMOBにのみ当たる
					// 攻撃対象がPC,Summon,Petの場合は、範囲内のPC,Summon,Pet,MOBに当たる
					if (cha is L1MonsterInstance)
					{
						if (!(@object is L1MonsterInstance))
						{
							continue;
						}
					}
					if ((cha is L1PcInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
					{
						if (!((@object is L1PcInstance) || (@object is L1SummonInstance) || (@object is L1PetInstance) || (@object is L1MonsterInstance)))
						{
							continue;
						}
					}

					dmg = calcDamageReduction(pc, (L1Character) @object, dmg, attr);
					if (dmg <= 0)
					{
						continue;
					}
					if (@object is L1PcInstance)
					{
						L1PcInstance targetPc = (L1PcInstance) @object;
						targetPc.sendPackets(new S_DoActionGFX(targetPc.Id, ActionCodes.ACTION_Damage));
						targetPc.broadcastPacket(new S_DoActionGFX(targetPc.Id, ActionCodes.ACTION_Damage));
						targetPc.receiveDamage(pc, (int) dmg, false);
					}
					else if ((@object is L1SummonInstance) || (@object is L1PetInstance) || (@object is L1MonsterInstance))
					{
						L1NpcInstance targetNpc = (L1NpcInstance) @object;
						targetNpc.broadcastPacket(new S_DoActionGFX(targetNpc.Id, ActionCodes.ACTION_Damage));
						targetNpc.receiveDamage(pc, (int) dmg);
					}
				}
			}
			return calcDamageReduction(pc, cha, dmg, attr);
		}

		public static double getLightningEdgeDamage(L1PcInstance pc, L1Character cha)
		{
			double dmg = 0;
			int chance = RandomHelper.Next(100) + 1;
			if (4 >= chance)
			{
				int sp = pc.Sp;
				int intel = pc.Int;
				double bsk = 0;
				if (pc.hasSkillEffect(L1SkillId.BERSERKERS))
				{
					bsk = 0.2;
				}
				dmg = (intel + sp) * (2 + bsk) + RandomHelper.Next(intel + sp) * 2;

				pc.sendPackets(new S_SkillSound(cha.Id, 10));
				pc.broadcastPacket(new S_SkillSound(cha.Id, 10));
			}
			return calcDamageReduction(pc, cha, dmg, L1Skills.ATTR_WIND);
		}

		public static void giveArkMageDiseaseEffect(L1PcInstance pc, L1Character cha)
		{
			int chance = RandomHelper.Next(1000) + 1;
			int probability = (5 - ((cha.Mr / 10) * 5)) * 10;
			if (probability == 0)
			{
				probability = 10;
			}
			if (probability >= chance)
			{
				L1SkillUse l1skilluse = new L1SkillUse();
				l1skilluse.handleCommands(pc, 56, cha.Id, cha.X, cha.Y, null, 0, L1SkillUse.TYPE_GMBUFF);
			}
		}

		public static void giveFettersEffect(L1PcInstance pc, L1Character cha)
		{
			int fettersTime = 8000;
			if (isFreeze(cha))
			{ // 凍結状態orカウンターマジック中
				return;
			}
			if ((RandomHelper.Next(100) + 1) <= 2)
			{
				L1EffectSpawn.Instance.spawnEffect(81182, fettersTime, cha.X, cha.Y, cha.MapId);
				if (cha is L1PcInstance)
				{
					L1PcInstance targetPc = (L1PcInstance) cha;
					targetPc.setSkillEffect(STATUS_FREEZE, fettersTime);
					targetPc.sendPackets(new S_SkillSound(targetPc.Id, 4184));
					targetPc.broadcastPacket(new S_SkillSound(targetPc.Id, 4184));
					targetPc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, true));
				}
				else if ((cha is L1MonsterInstance) || (cha is L1SummonInstance) || (cha is L1PetInstance))
				{
					L1NpcInstance npc = (L1NpcInstance) cha;
					npc.setSkillEffect(STATUS_FREEZE, fettersTime);
					npc.broadcastPacket(new S_SkillSound(npc.Id, 4184));
					npc.Paralyzed = true;
				}
			}
		}

		public static double calcDamageReduction(L1PcInstance pc, L1Character cha, double dmg, int attr)
		{
			// 凍結状態orカウンターマジック中
			if (isFreeze(cha))
			{
				return 0;
			}

			// MRによるダメージ軽減
			int mr = cha.Mr;
			double mrFloor = 0;
			if (mr <= 100)
			{
				mrFloor = Math.Floor((mr - pc.OriginalMagicHit) / 2);
			}
			else if (mr >= 100)
			{
				mrFloor = Math.Floor((mr - pc.OriginalMagicHit) / 10);
			}
			double mrCoefficient = 0;
			if (mr <= 100)
			{
				mrCoefficient = 1 - 0.01 * mrFloor;
			}
			else if (mr >= 100)
			{
				mrCoefficient = 0.6 - 0.01 * mrFloor;
			}
			dmg *= mrCoefficient;

			// 属性によるダメージ軽減
			int resist = 0;
			if (attr == L1Skills.ATTR_EARTH)
			{
				resist = cha.Earth;
			}
			else if (attr == L1Skills.ATTR_FIRE)
			{
				resist = cha.Fire;
			}
			else if (attr == L1Skills.ATTR_WATER)
			{
				resist = cha.Water;
			}
			else if (attr == L1Skills.ATTR_WIND)
			{
				resist = cha.Wind;
			}
			int resistFloor = (int)(0.32 * Math.Abs(resist));
			if (resist >= 0)
			{
				resistFloor *= 1;
			}
			else
			{
				resistFloor *= -1;
			}
			double attrDeffence = resistFloor / 32.0;
			dmg = (1.0 - attrDeffence) * dmg;

			return dmg;
		}

		private static bool isFreeze(L1Character cha)
		{
			if (cha.hasSkillEffect(STATUS_FREEZE))
			{
				return true;
			}
			if (cha.hasSkillEffect(ABSOLUTE_BARRIER))
			{
				return true;
			}
			if (cha.hasSkillEffect(ICE_LANCE))
			{
				return true;
			}
			if (cha.hasSkillEffect(FREEZING_BLIZZARD))
			{
				return true;
			}
			if (cha.hasSkillEffect(FREEZING_BREATH))
			{
				return true;
			}
			if (cha.hasSkillEffect(EARTH_BIND))
			{
				return true;
			}

			// カウンターマジック判定
			if (cha.hasSkillEffect(COUNTER_MAGIC))
			{
				cha.removeSkillEffect(COUNTER_MAGIC);
				int castgfx = SkillsTable.Instance.getTemplate(COUNTER_MAGIC).CastGfx;
				cha.broadcastPacket(new S_SkillSound(cha.Id, castgfx));
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_SkillSound(pc.Id, castgfx));
				}
				return true;
			}
			return false;
		}

	}

}