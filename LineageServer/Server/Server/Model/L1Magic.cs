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
	using Config = LineageServer.Server.Config;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using WarTimeController = LineageServer.Server.Server.WarTimeController;
	using SkillsTable = LineageServer.Server.Server.DataSources.SkillsTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using L1MagicDoll = LineageServer.Server.Server.Templates.L1MagicDoll;
	using L1Skills = LineageServer.Server.Server.Templates.L1Skills;
	using Random = LineageServer.Server.Server.utils.Random;

	using static LineageServer.Server.Server.Model.skill.L1SkillId;

	public class L1Magic
	{
		private int _calcType;

		private readonly int PC_PC = 1;

		private readonly int PC_NPC = 2;

		private readonly int NPC_PC = 3;

		private readonly int NPC_NPC = 4;

		private L1Character _target = null;

		private L1PcInstance _pc = null;

		private L1PcInstance _targetPc = null;

		private L1NpcInstance _npc = null;

		private L1NpcInstance _targetNpc = null;

		private int _leverage = 10; // 1/10倍で表現する。

		public virtual int Leverage
		{
			set
			{
				_leverage = value;
			}
			get
			{
				return _leverage;
			}
		}


		public L1Magic(L1Character attacker, L1Character target)
		{
			_target = target;

			if (attacker is L1PcInstance)
			{
				if (target is L1PcInstance)
				{
					_calcType = PC_PC;
					_pc = (L1PcInstance) attacker;
					_targetPc = (L1PcInstance) target;
				}
				else
				{
					_calcType = PC_NPC;
					_pc = (L1PcInstance) attacker;
					_targetNpc = (L1NpcInstance) target;
				}
			}
			else
			{
				if (target is L1PcInstance)
				{
					_calcType = NPC_PC;
					_npc = (L1NpcInstance) attacker;
					_targetPc = (L1PcInstance) target;
				}
				else
				{
					_calcType = NPC_NPC;
					_npc = (L1NpcInstance) attacker;
					_targetNpc = (L1NpcInstance) target;
				}
			}
		}

		private int MagicLevel
		{
			get
			{
				int magicLevel = 0;
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					magicLevel = _pc.MagicLevel;
				}
				else if ((_calcType == NPC_PC) || (_calcType == NPC_NPC))
				{
					magicLevel = _npc.MagicLevel;
				}
				return magicLevel;
			}
		}

		private int MagicBonus
		{
			get
			{
				int magicBonus = 0;
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					magicBonus = _pc.MagicBonus;
				}
				else if ((_calcType == NPC_PC) || (_calcType == NPC_NPC))
				{
					magicBonus = _npc.MagicBonus;
				}
				return magicBonus;
			}
		}

		private int Lawful
		{
			get
			{
				int lawful = 0;
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					lawful = _pc.Lawful;
				}
				else if ((_calcType == NPC_PC) || (_calcType == NPC_NPC))
				{
					lawful = _npc.Lawful;
				}
				return lawful;
			}
		}

		private int TargetMr
		{
			get
			{
				int mr = 0;
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					mr = _targetPc.Mr;
				}
				else
				{
					mr = _targetNpc.Mr;
				}
				return mr;
			}
		}

		/* ■■■■■■■■■■■■■■ 成功判定 ■■■■■■■■■■■■■ */
		// ●●●● 確率系魔法の成功判定 ●●●●
		// 計算方法
		// 攻撃側ポイント：LV + ((MagicBonus * 3) * 魔法固有係数)
		// 防御側ポイント：((LV / 2) + (MR * 3)) / 2
		// 攻撃成功率：攻撃側ポイント - 防御側ポイント
		public virtual bool calcProbabilityMagic(int skillId)
		{
			int probability = 0;
			bool isSuccess = false;

			// 攻撃者がGM権限の場合100%成功
			if ((_pc != null) && _pc.Gm)
			{
				return true;
			}

			// 判斷特定狀態下才可攻擊 NPC
			if ((_calcType == PC_NPC) && (_targetNpc != null))
			{
				if (_pc.isAttackMiss(_pc, _targetNpc.NpcTemplate.get_npcId()))
				{
					return false;
				}
			}

			if (!checkZone(skillId))
			{
				return false;
			}
			if (skillId == CANCELLATION)
			{
				if ((_calcType == PC_PC) && (_pc != null) && (_targetPc != null))
				{
					// 自分自身の場合は100%成功
					if (_pc.Id == _targetPc.Id)
					{
						return true;
					}
					// 同じクランの場合は100%成功
					if ((_pc.Clanid > 0) && (_pc.Clanid == _targetPc.Clanid))
					{
						return true;
					}
					// 同じパーティの場合は100%成功
					if (_pc.InParty)
					{
						if (_pc.Party.isMember(_targetPc))
						{
							return true;
						}
					}
					// それ以外の場合、セーフティゾーン内では無効
					if ((_pc.ZoneType == 1) || (_targetPc.ZoneType == 1))
					{
						return false;
					}
				}
				// 対象がNPC、使用者がNPCの場合は100%成功
				if ((_calcType == PC_NPC) || (_calcType == NPC_PC) || (_calcType == NPC_NPC))
				{
					return true;
				}
			}

			// アースバインド中はWB、キャンセレーション以外無効
			if ((_calcType == PC_PC) || (_calcType == NPC_PC))
			{
				if (_targetpc.hasSkillEffect(L1SkillId.EARTH_BIND))
				{
					if ((skillId != WEAPON_BREAK) && (skillId != CANCELLATION))
					{
						return false;
					}
				}
			}
			else
			{
				if (_targetNpc.hasSkillEffect(L1SkillId.EARTH_BIND))
				{
					if ((skillId != WEAPON_BREAK) && (skillId != CANCELLATION))
					{
						return false;
					}
				}
			}

			probability = calcProbability(skillId);

			int rnd = RandomHelper.Next(100) + 1;
			if (probability > 90)
			{
				probability = 90; // 最高成功率を90%とする。
			}

			if (probability >= rnd)
			{
				isSuccess = true;
			}
			else
			{
				isSuccess = false;
			}

			// 確率系魔法メッセージ
			if (!Config.ALT_ATKMSG)
			{
				return isSuccess;
			}
			if (Config.ALT_ATKMSG)
			{
				if (((_calcType == PC_PC) || (_calcType == PC_NPC)) && !_pc.Gm)
				{
					return isSuccess;
				}
				if (((_calcType == PC_PC) || (_calcType == NPC_PC)) && !_targetPc.Gm)
				{
					return isSuccess;
				}
			}

			string msg0 = "";
			string msg1 = " 施放魔法 ";
			string msg2 = "";
			string msg3 = "";
			string msg4 = "";

			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{ // アタッカーがＰＣの場合
				msg0 = _pc.Name + " 對";
			}
			else if (_calcType == NPC_PC)
			{ // アタッカーがＮＰＣの場合
				msg0 = _npc.Name;
			}

			msg2 = "，機率：" + probability + "%";
			if ((_calcType == NPC_PC) || (_calcType == PC_PC))
			{ // ターゲットがＰＣの場合
				msg4 = _targetPc.Name;
			}
			else if (_calcType == PC_NPC)
			{ // ターゲットがＮＰＣの場合
				msg4 = _targetNpc.Name;
			}
			if (isSuccess == true)
			{
				msg3 = "成功";
			}
			else
			{
				msg3 = "失敗";
			}

			// 0 4 1 3 2 攻擊者 對 目標 施放魔法 成功/失敗，機率：X%。
			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{
				_pc.sendPackets(new S_ServerMessage(166, msg0, msg1, msg2, msg3, msg4));
			}
			// 攻擊者 施放魔法 成功/失敗，機率：X%。
			else if ((_calcType == NPC_PC))
			{
				_targetPc.sendPackets(new S_ServerMessage(166, msg0, msg1, msg2, msg3, null));
			}

			return isSuccess;
		}

		private bool checkZone(int skillId)
		{
			if ((_pc != null) && (_targetPc != null))
			{
				if ((_pc.ZoneType == 1) || (_targetPc.ZoneType == 1))
				{ // セーフティーゾーン
					if ((skillId == WEAPON_BREAK) || (skillId == SLOW) || (skillId == CURSE_PARALYZE) || (skillId == MANA_DRAIN) || (skillId == DARKNESS) || (skillId == WEAKNESS) || (skillId == DISEASE) || (skillId == DECAY_POTION) || (skillId == MASS_SLOW) || (skillId == ENTANGLE) || (skillId == ERASE_MAGIC) || (skillId == EARTH_BIND) || (skillId == AREA_OF_SILENCE) || (skillId == WIND_SHACKLE) || (skillId == STRIKER_GALE) || (skillId == SHOCK_STUN) || (skillId == FOG_OF_SLEEPING) || (skillId == ICE_LANCE) || (skillId == FREEZING_BLIZZARD) || (skillId == FREEZING_BREATH) || (skillId == POLLUTE_WATER) || (skillId == ELEMENTAL_FALL_DOWN) || (skillId == RETURN_TO_NATURE) || (skillId == ICE_LANCE_COCKATRICE) || (skillId == ICE_LANCE_BASILISK))
					{
						return false;
					}
				}
			}
			return true;
		}

		private int calcProbability(int skillId)
		{
			L1Skills l1skills = SkillsTable.Instance.getTemplate(skillId);
			int attackLevel = 0;
			int defenseLevel = 0;
			int probability = 0;

			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{
				attackLevel = _pc.Level;
			}
			else
			{
				attackLevel = _npc.Level;
			}

			if ((_calcType == PC_PC) || (_calcType == NPC_PC))
			{
				defenseLevel = _targetPc.Level;
			}
			else
			{
				defenseLevel = _targetNpc.Level;
				if (skillId == RETURN_TO_NATURE)
				{
					if (_targetNpc is L1SummonInstance)
					{
						L1SummonInstance summon = (L1SummonInstance) _targetNpc;
						defenseLevel = summon.Master.Level;
					}
				}
			}

			if ((skillId == ELEMENTAL_FALL_DOWN) || (skillId == RETURN_TO_NATURE) || (skillId == ENTANGLE) || (skillId == ERASE_MAGIC) || (skillId == AREA_OF_SILENCE) || (skillId == WIND_SHACKLE) || (skillId == STRIKER_GALE) || (skillId == POLLUTE_WATER) || (skillId == EARTH_BIND))
			{
				// 成功確率は 魔法固有係数 × LV差 + 基本確率
				probability = (int)(((l1skills.ProbabilityDice) / 10D) * (attackLevel - defenseLevel)) + l1skills.ProbabilityValue;

				// オリジナルINTによる魔法命中
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					probability += 2 * _pc.OriginalMagicHit;
				}
			}
			else if (skillId == SHOCK_STUN)
			{
				// 成功確率は 基本確率 + LV差1毎に+-2%
				probability = l1skills.ProbabilityValue + (attackLevel - defenseLevel) * 2;

				// オリジナルINTによる魔法命中
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					probability += 2 * _pc.OriginalMagicHit;
				}
			}
			else if (skillId == COUNTER_BARRIER)
			{
				// 成功確率は 基本確率 + LV差1毎に+-1%
				probability = l1skills.ProbabilityValue + attackLevel - defenseLevel;

				// オリジナルINTによる魔法命中
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					probability += 2 * _pc.OriginalMagicHit;
				}
			}
			else if ((skillId == GUARD_BRAKE) || (skillId == RESIST_FEAR) || (skillId == HORROR_OF_DEATH))
			{
				int dice = l1skills.ProbabilityDice;
				int value = l1skills.ProbabilityValue;
				int diceCount = 0;
				diceCount = MagicBonus + MagicLevel;

				if (diceCount < 1)
				{
					diceCount = 1;
				}

				for (int i = 0; i < diceCount; i++)
				{
					probability += (RandomHelper.Next(dice) + 1 + value);
				}

				probability = probability * Leverage / 10;

				// オリジナルINTによる魔法命中
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					probability += 2 * _pc.OriginalMagicHit;
				}

				if (probability >= TargetMr)
				{
					probability = 100;
				}
				else
				{
					probability = 0;
				}
			}
			else
			{
				int dice = l1skills.ProbabilityDice;
				int diceCount = 0;
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					if (_pc.Wizard)
					{
						diceCount = MagicBonus + MagicLevel + 1;
					}
					else if (_pc.Elf)
					{
						diceCount = MagicBonus + MagicLevel - 1;
					}
					else
					{
						diceCount = MagicBonus + MagicLevel - 1;
					}
				}
				else
				{
					diceCount = MagicBonus + MagicLevel;
				}
				if (diceCount < 1)
				{
					diceCount = 1;
				}

				for (int i = 0; i < diceCount; i++)
				{
					probability += (RandomHelper.Next(dice) + 1);
				}
				probability = probability * Leverage / 10;

				// オリジナルINTによる魔法命中
				if ((_calcType == PC_PC) || (_calcType == PC_NPC))
				{
					probability += 2 * _pc.OriginalMagicHit;
				}

				probability -= TargetMr;

				if (skillId == TAMING_MONSTER)
				{
					double probabilityRevision = 1;
					if ((_targetNpc.MaxHp * 1 / 4) > _targetNpc.CurrentHp)
					{
						probabilityRevision = 1.3;
					}
					else if ((_targetNpc.MaxHp * 2 / 4) > _targetNpc.CurrentHp)
					{
						probabilityRevision = 1.2;
					}
					else if ((_targetNpc.MaxHp * 3 / 4) > _targetNpc.CurrentHp)
					{
						probabilityRevision = 1.1;
					}
					probability *= (int)probabilityRevision;
				}
			}

			// 状態異常に対する耐性
			if (skillId == EARTH_BIND)
			{
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					probability -= _targetPc.RegistSustain;
				}
			}
			else if (skillId == SHOCK_STUN)
			{
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					probability -= 2 * _targetPc.RegistStun;
				}
			}
			else if (skillId == CURSE_PARALYZE)
			{
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					probability -= _targetPc.RegistStone;
				}
			}
			else if (skillId == FOG_OF_SLEEPING)
			{
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					probability -= _targetPc.RegistSleep;
				}
			}
			else if ((skillId == ICE_LANCE) || (skillId == FREEZING_BLIZZARD) || (skillId == FREEZING_BREATH) || (skillId == ICE_LANCE_COCKATRICE) || (skillId == ICE_LANCE_BASILISK))
			{
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					probability -= _targetPc.RegistFreeze;
					// 檢查無敵狀態
					foreach (int skillid in INVINCIBLE)
					{
						if (_targetpc.hasSkillEffect(L1SkillId.skillid))
						{
							probability = 0;
							break;
						}
					}
				}
			}
			else if ((skillId == CURSE_BLIND) || (skillId == DARKNESS) || (skillId == DARK_BLIND))
			{
				if ((_calcType == PC_PC) || (_calcType == NPC_PC))
				{
					probability -= _targetPc.RegistBlind;
				}
			}

			return probability;
		}

		// 擁有這些狀態的, 不會受到傷害(無敵)
		private static readonly int[] INVINCIBLE = new int[] {ABSOLUTE_BARRIER, ICE_LANCE, FREEZING_BLIZZARD, FREEZING_BREATH, EARTH_BIND, ICE_LANCE_COCKATRICE, ICE_LANCE_BASILISK};

		/* ■■■■■■■■■■■■■■ 魔法ダメージ算出 ■■■■■■■■■■■■■■ */

		public virtual int calcMagicDamage(int skillId)
		{
			int damage = 0;

			// 檢查無敵狀態
			foreach (int skillid in INVINCIBLE)
			{
				if (_target.hasSkillEffect(skillid))
				{
					return damage;
				}
			}

			if ((_calcType == PC_PC) || (_calcType == NPC_PC))
			{
				damage = calcPcMagicDamage(skillId);
			}
			else if ((_calcType == PC_NPC) || (_calcType == NPC_NPC))
			{
				damage = calcNpcMagicDamage(skillId);
			}

			if (skillId != JOY_OF_PAIN)
			{ // 疼痛的歡愉無視魔免
				damage = calcMrDefense(damage);
			}

			return damage;
		}

		// ●●●● プレイヤー へのファイアーウォールの魔法ダメージ算出 ●●●●
		public virtual int calcPcFireWallDamage()
		{
			int dmg = 0;
			double attrDeffence = calcAttrResistance(L1Skills.ATTR_FIRE);
			L1Skills l1skills = SkillsTable.Instance.getTemplate(FIRE_WALL);
			dmg = (int)((1.0 - attrDeffence) * l1skills.DamageValue);

			if (_targetpc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
			{
				dmg = 0;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.ICE_LANCE))
			{
				dmg = 0;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD))
			{
				dmg = 0;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.FREEZING_BREATH))
			{
				dmg = 0;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.EARTH_BIND))
			{
				dmg = 0;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.ICE_LANCE_COCKATRICE))
			{
				dmg = 0;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.ICE_LANCE_BASILISK))
			{
				dmg = 0;
			}

			if (dmg < 0)
			{
				dmg = 0;
			}

			return dmg;
		}

		// ●●●● ＮＰＣ へのファイアーウォールの魔法ダメージ算出 ●●●●
		public virtual int calcNpcFireWallDamage()
		{
			int dmg = 0;
			double attrDeffence = calcAttrResistance(L1Skills.ATTR_FIRE);
			L1Skills l1skills = SkillsTable.Instance.getTemplate(FIRE_WALL);
			dmg = (int)((1.0 - attrDeffence) * l1skills.DamageValue);

			if (_targetNpc.hasSkillEffect(L1SkillId.ICE_LANCE))
			{
				dmg = 0;
			}
			if (_targetNpc.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD))
			{
				dmg = 0;
			}
			if (_targetNpc.hasSkillEffect(L1SkillId.FREEZING_BREATH))
			{
				dmg = 0;
			}
			if (_targetNpc.hasSkillEffect(L1SkillId.EARTH_BIND))
			{
				dmg = 0;
			}
			if (_targetNpc.hasSkillEffect(L1SkillId.ICE_LANCE_COCKATRICE))
			{
				dmg = 0;
			}
			if (_targetNpc.hasSkillEffect(L1SkillId.ICE_LANCE_BASILISK))
			{
				dmg = 0;
			}

			if (dmg < 0)
			{
				dmg = 0;
			}

			return dmg;
		}

		// ●●●● プレイヤー・ＮＰＣ から プレイヤー への魔法ダメージ算出 ●●●●
		private int calcPcMagicDamage(int skillId)
		{
			int dmg = 0;
			if (skillId == FINAL_BURN)
			{
				if (_calcType == PC_PC)
				{
					dmg = _pc.CurrentMp;
				}
				else
				{
					dmg = _npc.CurrentMp;
				}
			}
			else
			{
				dmg = calcMagicDiceDamage(skillId);
				dmg = (dmg * Leverage) / 10;
			}

			// 心靈破壞消耗目標5點MP造成5倍精神傷害
			if (skillId == MIND_BREAK)
			{
				if (_targetPc.CurrentMp >= 5)
				{
					_targetPc.CurrentMp = _targetPc.CurrentMp - 5;
					if (_calcType == PC_PC)
					{
						dmg += _pc.Wis * 5;
					}
					else if (_calcType == NPC_PC)
					{
						dmg += _npc.Wis * 5;
					}
				}
			}

			dmg -= _targetPc.DamageReductionByArmor; // 防具によるダメージ軽減

			// 魔法娃娃效果 - 傷害減免
			dmg -= L1MagicDoll.getDamageReductionByDoll(_targetPc);

			if (_targetpc.hasSkillEffect(L1SkillId.COOKING_1_0_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_1_1_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_1_2_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_1_3_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_1_4_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_1_5_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_1_6_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_0_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_1_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_2_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_3_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_4_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_5_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_6_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_0_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_1_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_2_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_3_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_4_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_5_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_6_S))
			{
				dmg -= 5;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.COOKING_1_7_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_2_7_S) || _targetpc.hasSkillEffect(L1SkillId.COOKING_3_7_S))
			{
				dmg -= 5;
			}

			if (_targetpc.hasSkillEffect(L1SkillId.REDUCTION_ARMOR))
			{
				int targetPcLvl = _targetPc.Level;
				if (targetPcLvl < 50)
				{
					targetPcLvl = 50;
				}
				dmg -= (targetPcLvl - 50) / 5 + 1;
			}
			if (_targetpc.hasSkillEffect(L1SkillId.DRAGON_SKIN))
			{
				dmg -= 2;
			}

			if (_targetpc.hasSkillEffect(L1SkillId.PATIENCE))
			{
				dmg -= 2;
			}

			if (_calcType == NPC_PC)
			{ // ペット、サモンからプレイヤーに攻撃
				bool isNowWar = false;
				int castleId = L1CastleLocation.getCastleIdByArea(_targetPc);
				if (castleId > 0)
				{
					isNowWar = WarTimeController.Instance.isNowWar(castleId);
				}
				if (!isNowWar)
				{
					if (_npc is L1PetInstance)
					{
						dmg /= 8;
					}
					if (_npc is L1SummonInstance)
					{
						L1SummonInstance summon = (L1SummonInstance) _npc;
						if (summon.ExsistMaster)
						{
							dmg /= 8;
						}
					}
				}
			}

			if (_targetpc.hasSkillEffect(L1SkillId.IMMUNE_TO_HARM))
			{
				dmg /= 2;
			}
			// 疼痛的歡愉傷害：(最大血量 - 目前血量 /5)
			if (skillId == JOY_OF_PAIN)
			{
				int nowDamage = 0;
				if (_calcType == PC_PC)
				{
					nowDamage = _pc.MaxHp - _pc.CurrentHp;
					if (nowDamage > 0)
					{
						dmg = nowDamage / 5;
					}
				}
				else if (_calcType == NPC_PC)
				{
					nowDamage = _npc.MaxHp - _npc.CurrentHp;
					if (nowDamage > 0)
					{
						dmg = nowDamage / 5;
					}
				}
			}

			if (_targetpc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
			{
				dmg = 0;
			}
			else if (_targetpc.hasSkillEffect(L1SkillId.ICE_LANCE))
			{
				dmg = 0;
			}
			else if (_targetpc.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD))
			{
				dmg = 0;
			}
			else if (_targetpc.hasSkillEffect(L1SkillId.FREEZING_BREATH))
			{
				dmg = 0;
			}
			else if (_targetpc.hasSkillEffect(L1SkillId.EARTH_BIND))
			{
				dmg = 0;
			}

			if (_calcType == NPC_PC)
			{
				if ((_npc is L1PetInstance) || (_npc is L1SummonInstance))
				{
					// 目標在安區、攻擊者在安區、NOPVP
					if ((_targetPc.ZoneType == 1) || (_npc.ZoneType == 1) || (_targetPc.checkNonPvP(_targetPc, _npc)))
					{
						dmg = 0;
					}
				}
			}

			if (_targetpc.hasSkillEffect(L1SkillId.COUNTER_MIRROR))
			{
				if (_calcType == PC_PC)
				{
					if (_targetPc.Wis >= RandomHelper.Next(100))
					{
						_pc.sendPackets(new S_DoActionGFX(_pc.Id, ActionCodes.ACTION_Damage));
						_pc.broadcastPacket(new S_DoActionGFX(_pc.Id, ActionCodes.ACTION_Damage));
						_targetPc.sendPackets(new S_SkillSound(_targetPc.Id, 4395));
						_targetPc.broadcastPacket(new S_SkillSound(_targetPc.Id, 4395));
						_pc.receiveDamage(_targetPc, dmg, false);
						dmg = 0;
						_targetPc.killSkillEffectTimer(COUNTER_MIRROR);
					}
				}
				else if (_calcType == NPC_PC)
				{
					int npcId = _npc.NpcTemplate.get_npcId();
					if ((npcId == 45681) || (npcId == 45682) || (npcId == 45683) || (npcId == 45684))
					{
					}
					else if (!_npc.NpcTemplate.get_IsErase())
					{
					}
					else
					{
						if (_targetPc.Wis >= RandomHelper.Next(100))
						{
							_npc.broadcastPacket(new S_DoActionGFX(_npc.Id, ActionCodes.ACTION_Damage));
							_targetPc.sendPackets(new S_SkillSound(_targetPc.Id, 4395));
							_targetPc.broadcastPacket(new S_SkillSound(_targetPc.Id, 4395));
							_npc.receiveDamage(_targetPc, dmg);
							dmg = 0;
							_targetPc.killSkillEffectTimer(COUNTER_MIRROR);
						}
					}
				}
			}

			if (dmg < 0)
			{
				dmg = 0;
			}

			return dmg;
		}

		// ●●●● プレイヤー・ＮＰＣ から ＮＰＣ へのダメージ算出 ●●●●
		private int calcNpcMagicDamage(int skillId)
		{
			int dmg = 0;
			if (skillId == FINAL_BURN)
			{
				if (_calcType == PC_NPC)
				{
					dmg = _pc.CurrentMp;
				}
				else
				{
					dmg = _npc.CurrentMp;
				}
			}
			else
			{
				dmg = calcMagicDiceDamage(skillId);
				dmg = (dmg * Leverage) / 10;
			}

			// 心靈破壞消耗目標5點MP造成5倍精神傷害
			if (skillId == MIND_BREAK)
			{
				if (_targetNpc.CurrentMp >= 5)
				{
					_targetNpc.CurrentMp = _targetNpc.CurrentMp - 5;
					if (_calcType == PC_NPC)
					{
						dmg += _pc.Wis * 5;
					}
					else if (_calcType == NPC_NPC)
					{
						dmg += _npc.Wis * 5;
					}
				}
			}

			// 疼痛的歡愉傷害：(最大血量 - 目前血量 /5)
			if (skillId == JOY_OF_PAIN)
			{
				int nowDamage = 0;
				if (_calcType == PC_NPC)
				{
					nowDamage = _pc.MaxHp - _pc.CurrentHp;
					if (nowDamage > 0)
					{
						dmg = nowDamage / 5;
					}
				}
				else if (_calcType == NPC_NPC)
				{
					nowDamage = _npc.MaxHp - _npc.CurrentHp;
					if (nowDamage > 0)
					{
						dmg = nowDamage / 5;
					}
				}
			}

			if (_calcType == PC_NPC)
			{ // プレイヤーからペット、サモンに攻撃
				bool isNowWar = false;
				int castleId = L1CastleLocation.getCastleIdByArea(_targetNpc);
				if (castleId > 0)
				{
					isNowWar = WarTimeController.Instance.isNowWar(castleId);
				}
				if (!isNowWar)
				{
					if (_targetNpc is L1PetInstance)
					{
						dmg /= 8;
					}
					if (_targetNpc is L1SummonInstance)
					{
						L1SummonInstance summon = (L1SummonInstance) _targetNpc;
						if (summon.ExsistMaster)
						{
							dmg /= 8;
						}
					}
				}
			}

			if (_targetNpc.hasSkillEffect(L1SkillId.ICE_LANCE))
			{
				dmg = 0;
			}
			else if (_targetNpc.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD))
			{
				dmg = 0;
			}
			else if (_targetNpc.hasSkillEffect(L1SkillId.FREEZING_BREATH))
			{
				dmg = 0;
			}
			else if (_targetNpc.hasSkillEffect(L1SkillId.EARTH_BIND))
			{
				dmg = 0;
			}

			// 判斷特定狀態下才可攻擊 NPC
			if ((_calcType == PC_NPC) && (_targetNpc != null))
			{
				if (_pc.isAttackMiss(_pc, _targetNpc.NpcTemplate.get_npcId()))
				{
					dmg = 0;
				}
			}
			if (_calcType == NPC_NPC)
			{
				if (((_npc is L1PetInstance) || (_npc is L1SummonInstance)) && ((_targetNpc is L1PetInstance) || (_targetNpc is L1SummonInstance)))
				{
					// 目標在安區、攻擊者在安區
					if ((_targetNpc.ZoneType == 1) || (_npc.ZoneType == 1))
					{
						dmg = 0;
					}
				}
			}

			return dmg;
		}

		// ●●●● damage_dice、damage_dice_count、damage_value、SPから魔法ダメージを算出 ●●●●
		private int calcMagicDiceDamage(int skillId)
		{
			L1Skills l1skills = SkillsTable.Instance.getTemplate(skillId);
			int dice = l1skills.DamageDice;
			int diceCount = l1skills.DamageDiceCount;
			int value = l1skills.DamageValue;
			int magicDamage = 0;
			int charaIntelligence = 0;

			for (int i = 0; i < diceCount; i++)
			{
				magicDamage += (RandomHelper.Next(dice) + 1);
			}
			magicDamage += value;

			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{
				int weaponAddDmg = 0; // 武器による追加ダメージ
				L1ItemInstance weapon = _pc.Weapon;
				if (weapon != null)
				{
					weaponAddDmg = weapon.Item.MagicDmgModifier;
				}
				magicDamage += weaponAddDmg;
			}

			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{
				int spByItem = _pc.Sp - _pc.TrueSp; // アイテムによるSP変動
				charaIntelligence = _pc.Int + spByItem - 12;
			}
			else if ((_calcType == NPC_PC) || (_calcType == NPC_NPC))
			{
				int spByItem = _npc.Sp - _npc.TrueSp; // アイテムによるSP変動
				charaIntelligence = _npc.Int + spByItem - 12;
			}
			if (charaIntelligence < 1)
			{
				charaIntelligence = 1;
			}

			double attrDeffence = calcAttrResistance(l1skills.Attr);

			double coefficient = (1.0 - attrDeffence + charaIntelligence * 3.0 / 32.0);
			if (coefficient < 0)
			{
				coefficient = 0;
			}

			magicDamage *= (int)coefficient;

			double criticalCoefficient = 1.5; // 魔法クリティカル
			int rnd = RandomHelper.Next(100) + 1;
			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{
				if (l1skills.SkillLevel <= 6)
				{
					if (rnd <= (10 + _pc.OriginalMagicCritical))
					{
						magicDamage *= (int)criticalCoefficient;
					}
				}
			}

			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{ // オリジナルINTによる魔法ダメージ
				magicDamage += _pc.OriginalMagicDamage;
			}
			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{ // アバターによる追加ダメージ
				if (_pc.hasSkillEffect(L1SkillId.ILLUSION_AVATAR))
				{
					magicDamage += 10;
				}
			}

			return magicDamage;
		}

		// ●●●● ヒール回復量（対アンデッドにはダメージ）を算出 ●●●●
		public virtual int calcHealing(int skillId)
		{
			L1Skills l1skills = SkillsTable.Instance.getTemplate(skillId);
			int dice = l1skills.DamageDice;
			int value = l1skills.DamageValue;
			int magicDamage = 0;

			int magicBonus = MagicBonus;
			if (magicBonus > 10)
			{
				magicBonus = 10;
			}

			int diceCount = value + magicBonus;
			for (int i = 0; i < diceCount; i++)
			{
				magicDamage += (RandomHelper.Next(dice) + 1);
			}

			double alignmentRevision = 1.0;
			if (Lawful > 0)
			{
				alignmentRevision += (Lawful / 32768.0);
			}

			magicDamage *= (int)alignmentRevision;

			magicDamage = (magicDamage * Leverage) / 10;

			return magicDamage;
		}

		// ●●●● ＭＲによるダメージ軽減 ●●●●
		private int calcMrDefense(int dmg)
		{
			int mr = TargetMr;

			double mrFloor = 0;
			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{
				if (mr <= 100)
				{
					mrFloor = Math.Floor((mr - _pc.OriginalMagicHit) / 2);
				}
				else if (mr >= 100)
				{
					mrFloor = Math.Floor((mr - _pc.OriginalMagicHit) / 10);
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
				dmg *= (int)mrCoefficient;
			}
			else if ((_calcType == NPC_PC) || (_calcType == NPC_NPC))
			{
				int rnd = RandomHelper.Next(100) + 1;
				if (mr >= rnd)
				{
					dmg /= 2;
				}
			}

			return dmg;
		}

		// ●●●● 属性によるダメージ軽減 ●●●●
		// attr:0.無属性魔法,1.地魔法,2.火魔法,4.水魔法,8.風魔法(,16.光魔法)
		private double calcAttrResistance(int attr)
		{
			int resist = 0;
			if ((_calcType == PC_PC) || (_calcType == NPC_PC))
			{
				if (attr == L1Skills.ATTR_EARTH)
				{
					resist = _targetPc.Earth;
				}
				else if (attr == L1Skills.ATTR_FIRE)
				{
					resist = _targetPc.Fire;
				}
				else if (attr == L1Skills.ATTR_WATER)
				{
					resist = _targetPc.Water;
				}
				else if (attr == L1Skills.ATTR_WIND)
				{
					resist = _targetPc.Wind;
				}
			}
			else if ((_calcType == PC_NPC) || (_calcType == NPC_NPC))
			{
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

			return attrDeffence;
		}

		/* ■■■■■■■■■■■■■■■ 計算結果反映 ■■■■■■■■■■■■■■■ */

		public virtual void commit(int damage, int drainMana)
		{
			if ((_calcType == PC_PC) || (_calcType == NPC_PC))
			{
				commitPc(damage, drainMana);
			}
			else if ((_calcType == PC_NPC) || (_calcType == NPC_NPC))
			{
				commitNpc(damage, drainMana);
			}

			// ダメージ値及び命中率確認用メッセージ
			if (!Config.ALT_ATKMSG)
			{
				return;
			}
			if (Config.ALT_ATKMSG)
			{
				if (((_calcType == PC_PC) || (_calcType == PC_NPC)) && !_pc.Gm)
				{
					return;
				}
				if ((_calcType == NPC_PC) && !_targetPc.Gm)
				{
					return;
				}
			}

			string msg0 = "";
			string msg1 = " 造成 ";
			string msg2 = "";
			string msg3 = "";
			string msg4 = "";

			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{ // アタッカーがＰＣの場合
				msg0 = "魔攻 對";
			}
			else if (_calcType == NPC_PC)
			{ // アタッカーがＮＰＣの場合
				msg0 = _npc.Name + "(魔攻)：";
			}

			if ((_calcType == NPC_PC) || (_calcType == PC_PC))
			{ // ターゲットがＰＣの場合
				msg4 = _targetPc.Name;
				msg2 = "，剩餘 " + _targetPc.CurrentHp;
			}
			else if (_calcType == PC_NPC)
			{ // ターゲットがＮＰＣの場合
				msg4 = _targetNpc.Name;
				msg2 = "，剩餘 " + _targetNpc.CurrentHp;
			}

			msg3 = damage + " 傷害";

			// 魔攻 對 目標 造成 X 傷害，剩餘 Y。
			if ((_calcType == PC_PC) || (_calcType == PC_NPC))
			{ // アタッカーがＰＣの場合
				_pc.sendPackets(new S_ServerMessage(166, msg0, msg1, msg2, msg3, msg4)); // \f1%0が%4%1%3
																							// %2
			}
			// 攻擊者(魔攻)： X傷害，剩餘 Y。
			else if ((_calcType == NPC_PC))
			{ // ターゲットがＰＣの場合
				_targetPc.sendPackets(new S_ServerMessage(166, msg0, null, msg2, msg3, null)); // \f1%0が%4%1%3
																								// %2
			}
		}

		// ●●●● プレイヤーに計算結果を反映 ●●●●
		private void commitPc(int damage, int drainMana)
		{
			if (_calcType == PC_PC)
			{
				if ((drainMana > 0) && (_targetPc.CurrentMp > 0))
				{
					if (drainMana > _targetPc.CurrentMp)
					{
						drainMana = _targetPc.CurrentMp;
					}
					int newMp = _pc.CurrentMp + drainMana;
					_pc.CurrentMp = newMp;
				}
				_targetPc.receiveManaDamage(_pc, drainMana);
				_targetPc.receiveDamage(_pc, damage, true);
			}
			else if (_calcType == NPC_PC)
			{
				_targetPc.receiveDamage(_npc, damage, true);
			}
		}

		// ●●●● ＮＰＣに計算結果を反映 ●●●●
		private void commitNpc(int damage, int drainMana)
		{
			if (_calcType == PC_NPC)
			{
				if (drainMana > 0)
				{
					int drainValue = _targetNpc.drainMana(drainMana);
					int newMp = _pc.CurrentMp + drainValue;
					_pc.CurrentMp = newMp;
				}
				_targetNpc.ReceiveManaDamage(_pc, drainMana);
				_targetNpc.receiveDamage(_pc, damage);
			}
			else if (_calcType == NPC_NPC)
			{
				_targetNpc.receiveDamage(_npc, damage);
			}
		}
	}

}