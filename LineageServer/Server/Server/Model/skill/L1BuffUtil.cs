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
namespace LineageServer.Server.Server.Model.skill
{
	using Config = LineageServer.Server.Config;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using NpcTable = LineageServer.Server.Server.DataSources.NpcTable;
	using L1Awake = LineageServer.Server.Server.Model.L1Awake;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1CurseParalysis = LineageServer.Server.Server.Model.L1CurseParalysis;
	using L1PinkName = LineageServer.Server.Server.Model.L1PinkName;
	using L1PolyMorph = LineageServer.Server.Server.Model.L1PolyMorph;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using L1TowerInstance = LineageServer.Server.Server.Model.Instance.L1TowerInstance;
	using L1DamagePoison = LineageServer.Server.Server.Model.poison.L1DamagePoison;
	using S_ChangeName = LineageServer.Server.Server.serverpackets.S_ChangeName;
	using S_CharVisualUpdate = LineageServer.Server.Server.serverpackets.S_CharVisualUpdate;
	using S_ChatPacket = LineageServer.Server.Server.serverpackets.S_ChatPacket;
	using S_CurseBlind = LineageServer.Server.Server.serverpackets.S_CurseBlind;
	using S_Dexup = LineageServer.Server.Server.serverpackets.S_Dexup;
	using S_DoActionShop = LineageServer.Server.Server.serverpackets.S_DoActionShop;
	using S_EffectLocation = LineageServer.Server.Server.serverpackets.S_EffectLocation;
	using S_HPUpdate = LineageServer.Server.Server.serverpackets.S_HPUpdate;
	using S_Invis = LineageServer.Server.Server.serverpackets.S_Invis;
	using S_Liquor = LineageServer.Server.Server.serverpackets.S_Liquor;
	using S_MPUpdate = LineageServer.Server.Server.serverpackets.S_MPUpdate;
	using S_Message_YN = LineageServer.Server.Server.serverpackets.S_Message_YN;
	using S_NpcChangeShape = LineageServer.Server.Server.serverpackets.S_NpcChangeShape;
	using S_NpcChatPacket = LineageServer.Server.Server.serverpackets.S_NpcChatPacket;
	using S_OwnCharAttrDef = LineageServer.Server.Server.serverpackets.S_OwnCharAttrDef;
	using S_OwnCharStatus2 = LineageServer.Server.Server.serverpackets.S_OwnCharStatus2;
	using S_PacketBox = LineageServer.Server.Server.serverpackets.S_PacketBox;
	using S_Paralysis = LineageServer.Server.Server.serverpackets.S_Paralysis;
	using S_RemoveObject = LineageServer.Server.Server.serverpackets.S_RemoveObject;
	using S_SPMR = LineageServer.Server.Server.serverpackets.S_SPMR;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_ShowPolyList = LineageServer.Server.Server.serverpackets.S_ShowPolyList;
	using S_SkillBrave = LineageServer.Server.Server.serverpackets.S_SkillBrave;
	using S_SkillHaste = LineageServer.Server.Server.serverpackets.S_SkillHaste;
	using S_SkillIconAura = LineageServer.Server.Server.serverpackets.S_SkillIconAura;
	using S_SkillIconBloodstain = LineageServer.Server.Server.serverpackets.S_SkillIconBloodstain;
	using S_SkillIconGFX = LineageServer.Server.Server.serverpackets.S_SkillIconGFX;
	using S_SkillIconShield = LineageServer.Server.Server.serverpackets.S_SkillIconShield;
	using S_SkillIconWindShackle = LineageServer.Server.Server.serverpackets.S_SkillIconWindShackle;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using S_Strup = LineageServer.Server.Server.serverpackets.S_Strup;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using Random = LineageServer.Server.Server.utils.Random;
	using static LineageServer.Server.Server.Model.skill.L1SkillId;

	public class L1BuffUtil
	{
		public static void haste(L1PcInstance pc, int timeMillis)
		{

			int objId = pc.Id;

			/* 已存在加速狀態消除 */
			if (pc.hasSkillEffect(L1SkillId.HASTE) || pc.hasSkillEffect(L1SkillId.GREATER_HASTE) || pc.hasSkillEffect(L1SkillId.STATUS_HASTE))
			{
				if (pc.hasSkillEffect(L1SkillId.HASTE))
				{ // 加速術
					pc.killSkillEffectTimer(HASTE);
				}
				else if (pc.hasSkillEffect(L1SkillId.GREATER_HASTE))
				{ // 強力加速術
					pc.killSkillEffectTimer(GREATER_HASTE);
				}
				else if (pc.hasSkillEffect(L1SkillId.STATUS_HASTE))
				{ // 自我加速藥水
					pc.killSkillEffectTimer(STATUS_HASTE);
				}
			}
			/* 抵消緩速魔法效果 緩速術 集體緩速術 地面障礙 */
			if (pc.hasSkillEffect(L1SkillId.SLOW) || pc.hasSkillEffect(L1SkillId.MASS_SLOW) || pc.hasSkillEffect(L1SkillId.ENTANGLE))
			{
				if (pc.hasSkillEffect(L1SkillId.SLOW))
				{ // 緩速術
					pc.killSkillEffectTimer(SLOW);
				}
				else if (pc.hasSkillEffect(L1SkillId.MASS_SLOW))
				{ // 集體緩速術
					pc.killSkillEffectTimer(MASS_SLOW);
				}
				else if (pc.hasSkillEffect(L1SkillId.ENTANGLE))
				{ // 地面障礙
					pc.killSkillEffectTimer(ENTANGLE);
				}
				pc.sendPackets(new S_SkillHaste(objId, 0, 0));
				pc.broadcastPacket(new S_SkillHaste(objId, 0, 0));
			}

			pc.setSkillEffect(STATUS_HASTE, timeMillis);

			pc.sendPackets(new S_SkillSound(objId, 191));
			pc.broadcastPacket(new S_SkillSound(objId, 191));
			pc.sendPackets(new S_SkillHaste(objId, 1, timeMillis / 1000));
			pc.broadcastPacket(new S_SkillHaste(objId, 1, 0));
			pc.sendPackets(new S_ServerMessage(184)); // \f1你的動作突然變快。 */
			pc.MoveSpeed = 1;
		}

		public static void brave(L1PcInstance pc, int timeMillis)
		{
			// 消除重複狀態
			if (pc.hasSkillEffect(L1SkillId.STATUS_BRAVE))
			{ // 勇敢藥水 1.33倍
				pc.killSkillEffectTimer(STATUS_BRAVE);
			}
			if (pc.hasSkillEffect(L1SkillId.STATUS_ELFBRAVE))
			{ // 精靈餅乾 1.15倍
				pc.killSkillEffectTimer(STATUS_ELFBRAVE);
			}
			if (pc.hasSkillEffect(L1SkillId.HOLY_WALK))
			{ // 神聖疾走 移速1.33倍
				pc.killSkillEffectTimer(HOLY_WALK);
			}
			if (pc.hasSkillEffect(L1SkillId.MOVING_ACCELERATION))
			{ // 行走加速 移速1.33倍
				pc.killSkillEffectTimer(MOVING_ACCELERATION);
			}
			if (pc.hasSkillEffect(L1SkillId.WIND_WALK))
			{ // 風之疾走 移速1.33倍
				pc.killSkillEffectTimer(WIND_WALK);
			}
			if (pc.hasSkillEffect(L1SkillId.BLOODLUST))
			{ // 血之渴望 攻速1.33倍
				pc.killSkillEffectTimer(BLOODLUST);
			}
			if (pc.hasSkillEffect(L1SkillId.STATUS_BRAVE2))
			{ // 超級加速 2.66倍
				pc.killSkillEffectTimer(STATUS_BRAVE2);
			}

			pc.setSkillEffect(STATUS_BRAVE, timeMillis);

			int objId = pc.Id;
			pc.sendPackets(new S_SkillSound(objId, 751));
			pc.broadcastPacket(new S_SkillSound(objId, 751));
			pc.sendPackets(new S_SkillBrave(objId, 1, timeMillis / 1000));
			pc.broadcastPacket(new S_SkillBrave(objId, 1, 0));
			pc.BraveSpeed = 1;
		}

		public static void thirdSpeed(L1PcInstance pc)
		{
			if (pc.hasSkillEffect(L1SkillId.STATUS_THIRD_SPEED))
			{
				pc.killSkillEffectTimer(STATUS_THIRD_SPEED);
			}

			pc.setSkillEffect(STATUS_THIRD_SPEED, 600 * 1000);

			pc.sendPackets(new S_Liquor(pc.Id, 8)); // 人物 * 1.15
			pc.broadcastPacket(new S_Liquor(pc.Id, 8)); // 人物 * 1.15
			pc.sendPackets(new S_SkillSound(pc.Id, 7976));
			pc.broadcastPacket(new S_SkillSound(pc.Id, 7976));
			pc.sendPackets(new S_ServerMessage(1065)); // 將發生神秘的奇蹟力量。
		}

		public static void bloodstain(L1PcInstance pc, sbyte type, int time, bool showGfx)
		{
			if (showGfx)
			{
				pc.sendPackets(new S_SkillSound(pc.Id, 7783));
				pc.broadcastPacket(new S_SkillSound(pc.Id, 7783));
			}

			int skillId = EFFECT_BLOODSTAIN_OF_ANTHARAS;
			int iconType = 0;
			if (type == 0)
			{ // 安塔瑞斯
				if (!pc.hasSkillEffect(L1SkillId.skillId))
				{
					pc.addAc(-2); // 防禦 -2
					pc.addWater(50); // 水屬性 +50
				}
				iconType = 82;
				// 安塔瑞斯的血痕
			}
			else if (type == 1)
			{ // 法利昂
				skillId = EFFECT_BLOODSTAIN_OF_FAFURION;
				if (!pc.hasSkillEffect(L1SkillId.skillId))
				{
					pc.addWind(50); // 風屬性 +50
				}
				iconType = 85;
			}
			pc.sendPackets(new S_OwnCharAttrDef(pc));
			pc.sendPackets(new S_SkillIconBloodstain(iconType, time));
			pc.setSkillEffect(skillId, (time * 60 * 1000));
		}

		public static void effectBlessOfDragonSlayer(L1PcInstance pc, int skillId, int time, int showGfx)
		{
			if (showGfx != 0)
			{
				pc.sendPackets(new S_SkillSound(pc.Id, showGfx));
				pc.broadcastPacket(new S_SkillSound(pc.Id, showGfx));
			}

			if (!pc.hasSkillEffect(L1SkillId.skillId))
			{
				switch (skillId)
				{
				case EFFECT_BLESS_OF_CRAY: // 卡瑞的祝福
					if (pc.hasSkillEffect(L1SkillId.EFFECT_BLESS_OF_SAELL))
					{
						pc.removeSkillEffect(L1SkillId.EFFECT_BLESS_OF_SAELL);
					}
					pc.addMaxHp(100);
					pc.addMaxMp(50);
					pc.addHpr(3);
					pc.addMpr(3);
					pc.addEarth(30);
					pc.addDmgup(1);
					pc.addHitup(5);
					pc.addWeightReduction(40);
					break;
				case EFFECT_BLESS_OF_SAELL: // 莎爾的祝福
					if (pc.hasSkillEffect(L1SkillId.EFFECT_BLESS_OF_CRAY))
					{
						pc.removeSkillEffect(L1SkillId.EFFECT_BLESS_OF_CRAY);
					}
					pc.addMaxHp(80);
					pc.addMaxMp(10);
					pc.addWater(30);
					pc.addAc(-8);
					break;
				}
				pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
				if (pc.InParty)
				{
					pc.Party.updateMiniHP(pc);
				}
				pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
				pc.sendPackets(new S_OwnCharStatus2(pc, 0));
				pc.sendPackets(new S_OwnCharAttrDef(pc));
			}
			pc.setSkillEffect(skillId, (time * 1000));
		}

		public static int skillEffect(L1Character _user, L1Character cha, L1Character _target, int skillId, int _getBuffIconDuration, int dmg)
		{
			L1PcInstance _player = null;
			if (_user is L1PcInstance)
			{
				L1PcInstance _pc = (L1PcInstance) _user;
				_player = _pc;
			}

			switch (skillId)
			{
			// PC、NPC 2方皆有效果
			// 解毒術
			case CURE_POISON:
				cha.curePoison();
				break;
			// 聖潔之光
			case REMOVE_CURSE:
				cha.curePoison();
				if (cha.hasSkillEffect(STATUS_CURSE_PARALYZING) || cha.hasSkillEffect(STATUS_CURSE_PARALYZED))
				{
					cha.cureParalaysis();
				}
				break;
			// 返生術、終極返生術
			case RESURRECTION:
			case GREATER_RESURRECTION:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					if (_player.Id != pc.Id)
					{
						if (L1World.Instance.getVisiblePlayer(pc, 0).Count > 0)
						{
							foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(pc, 0))
							{
								if (!visiblePc.Dead)
								{
									// 復活失敗，因為這個位置已被佔據。
									_player.sendPackets(new S_ServerMessage(592));
									return 0;
								}
							}
						}
						if ((pc.CurrentHp == 0) && pc.Dead)
						{
							if (pc.Map.UseResurrection)
							{
								if (skillId == RESURRECTION)
								{
									pc.Gres = false;
								}
								else if (skillId == GREATER_RESURRECTION)
								{
									pc.Gres = true;
								}
								pc.TempID = _player.Id;
								pc.sendPackets(new S_Message_YN(322, "")); // 是否要復活？(Y/N)
							}
						}
					}
				}
				else if (cha is L1NpcInstance)
				{
					if (!(cha is L1TowerInstance))
					{
						L1NpcInstance npc = (L1NpcInstance) cha;
						if (npc.NpcTemplate.CantResurrect && !(npc is L1PetInstance))
						{
							return 0;
						}
						if ((npc is L1PetInstance) && (L1World.Instance.getVisiblePlayer(npc, 0).Count > 0))
						{
							foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(npc, 0))
							{
								if (!visiblePc.Dead)
								{
									// 復活失敗，因為這個位置已被佔據。
									_player.sendPackets(new S_ServerMessage(592));
									return 0;
								}
							}
						}
						if ((npc.CurrentHp == 0) && npc.Dead)
						{
							npc.resurrect(npc.MaxHp / 4);
							npc.Resurrect = true;
							if ((npc is L1PetInstance))
							{
								L1PetInstance pet = (L1PetInstance) npc;
								// 開始飽食度計時
								pet.startFoodTimer(pet);
								// 開始回血回魔
								pet.startHpRegeneration();
								pet.startMpRegeneration();
							}
						}
					}
				}
				break;
			// 生命呼喚
			case CALL_OF_NATURE:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					if (_player.Id != pc.Id)
					{
						if (L1World.Instance.getVisiblePlayer(pc, 0).Count > 0)
						{
							foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(pc, 0))
							{
								if (!visiblePc.Dead)
								{
									// 復活失敗，因為這個位置已被佔據。
									_player.sendPackets(new S_ServerMessage(592));
									return 0;
								}
							}
						}
						if ((pc.CurrentHp == 0) && pc.Dead)
						{
							pc.TempID = _player.Id;
							pc.sendPackets(new S_Message_YN(322, "")); // 是否要復活？
																		// (Y/N)
						}
					}
				}
				else if (cha is L1NpcInstance)
				{
					if (!(cha is L1TowerInstance))
					{
						L1NpcInstance npc = (L1NpcInstance) cha;
						if (npc.NpcTemplate.CantResurrect && !(npc is L1PetInstance))
						{
							return 0;
						}
						if ((npc is L1PetInstance) && (L1World.Instance.getVisiblePlayer(npc, 0).Count > 0))
						{
							foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(npc, 0))
							{
								if (!visiblePc.Dead)
								{
									// 復活失敗，因為這個位置已被佔據。
									_player.sendPackets(new S_ServerMessage(592));
									return 0;
								}
							}
						}
						if ((npc.CurrentHp == 0) && npc.Dead)
						{
							npc.resurrect(cha.getMaxHp());
							npc.resurrect(cha.getMaxMp() / 100);
							npc.Resurrect = true;
							if ((npc is L1PetInstance))
							{
								L1PetInstance pet = (L1PetInstance) npc;
								// 開始飽食度計時
								pet.startFoodTimer(pet);
								// 開始回血回魔
								pet.startHpRegeneration();
								pet.startMpRegeneration();
							}
						}
					}
				}
				break;
			// 無所遁形
			case DETECTION:
				if (cha is L1NpcInstance)
				{
					L1NpcInstance npc = (L1NpcInstance) cha;
					int hiddenStatus = npc.HiddenStatus;
					if (hiddenStatus == L1NpcInstance.HIDDEN_STATUS_SINK)
					{
						npc.appearOnGround(_player);
					}
				}
				break;
			// 弱化屬性
			case ELEMENTAL_FALL_DOWN:
				if (_user is L1PcInstance)
				{
					int playerAttr = _player.ElfAttr;
					int i = -50;
					if (playerAttr != 0)
					{
						_player.sendPackets(new S_SkillSound(cha.Id, 4396));
						_player.broadcastPacket(new S_SkillSound(cha.Id, 4396));
					}
					switch (playerAttr)
					{
					case 0:
						_player.sendPackets(new S_ServerMessage(79));
						break;
					case 1:
						cha.addEarth(i);
						cha.AddAttrKind = 1;
						break;
					case 2:
						cha.addFire(i);
						cha.AddAttrKind = 2;
						break;
					case 4:
						cha.addWater(i);
						cha.AddAttrKind = 4;
						break;
					case 8:
						cha.addWind(i);
						cha.AddAttrKind = 8;
						break;
					default:
						break;
					}
				}
				break;

			// 物理性技能效果
			// 三重矢
			case TRIPLE_ARROW:
				bool gfxcheck = false;
				int[] BowGFX = new int[] {138, 37, 3860, 3126, 3420, 2284, 3105, 3145, 3148, 3151, 3871, 4125, 2323, 3892, 3895, 3898, 3901, 4917, 4918, 4919, 4950, 6087, 6140, 6145, 6150, 6155, 6160, 6269, 6272, 6275, 6278, 6826, 6827, 6836, 6837, 6846, 6847, 6856, 6857, 6866, 6867, 6876, 6877, 6886, 6887, 8719, 8786, 8792, 8798, 8804, 8808, 8900, 8913};
				int playerGFX = _player.TempCharGfx;
				foreach (int gfx in BowGFX)
				{
					if (playerGFX == gfx)
					{
						gfxcheck = true;
						break;
					}
				}
				if (!gfxcheck)
				{
					return 0;
				}

				for (int i = 3; i > 0; i--)
				{
					_target.onAction(_player);
				}
				_player.sendPackets(new S_SkillSound(_player.Id, 4394));
				_player.broadcastPacket(new S_SkillSound(_player.Id, 4394));
				break;
			// 屠宰者
			case FOE_SLAYER:
				_player.FoeSlayer = true;
				for (int i = 3; i > 0; i--)
				{
					_target.onAction(_player);
				}
				_player.FoeSlayer = false;

				_player.sendPackets(new S_EffectLocation(_target.X, _target.Y, 6509));
				_player.broadcastPacket(new S_EffectLocation(_target.X, _target.Y, 6509));
				_player.sendPackets(new S_SkillSound(_player.Id, 7020));
				_player.broadcastPacket(new S_SkillSound(_player.Id, 7020));

				if (_player.hasSkillEffect(SPECIAL_EFFECT_WEAKNESS_LV1))
				{
					_player.killSkillEffectTimer(SPECIAL_EFFECT_WEAKNESS_LV1);
					_player.sendPackets(new S_SkillIconGFX(75, 0));
				}
				else if (_player.hasSkillEffect(SPECIAL_EFFECT_WEAKNESS_LV2))
				{
					_player.killSkillEffectTimer(SPECIAL_EFFECT_WEAKNESS_LV2);
					_player.sendPackets(new S_SkillIconGFX(75, 0));
				}
				else if (_player.hasSkillEffect(SPECIAL_EFFECT_WEAKNESS_LV3))
				{
					_player.killSkillEffectTimer(SPECIAL_EFFECT_WEAKNESS_LV3);
					_player.sendPackets(new S_SkillIconGFX(75, 0));
				}
				break;
			// 暴擊
			case SMASH:
				_target.onAction(_player, SMASH);
				break;
			// 骷髏毀壞
			case BONE_BREAK:
				_target.onAction(_player, BONE_BREAK);
				break;

			// 機率性魔法
			// 混亂
			case CONFUSION:
				// 發動判斷
				if (_user is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) _user;
					if (!cha.hasSkillEffect(CONFUSION))
					{
						int change = RandomHelper.Next(100) + 1;
						if (change < (30 + RandomHelper.Next(11)))
						{ // 30 ~ 40%
							pc.sendPackets(new S_SkillSound(cha.Id, 6525));
							pc.broadcastPacket(new S_SkillSound(cha.Id, 6525));
							cha.setSkillEffect(CONFUSION, 2 * 1000); // 發動後再次發動間隔 2秒
							cha.setSkillEffect(CONFUSION_ING, 8 * 1000);
							if (cha is L1PcInstance)
							{
								L1PcInstance targetPc = (L1PcInstance) cha;
								targetPc.sendPackets(new S_ServerMessage(1339)); // 突然感覺到混亂。
							}
						}
					}
				}
				break;
			// 闇盲咒術
			// 黑闇之影
			case CURSE_BLIND:
			case DARKNESS:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					if (pc.hasSkillEffect(L1SkillId.STATUS_FLOATING_EYE))
					{ // 漂浮之眼肉效果
						pc.sendPackets(new S_CurseBlind(2));
					}
					else
					{
						pc.sendPackets(new S_CurseBlind(1));
					}
				}
				break;
			// 毒咒
			case CURSE_POISON:
				L1DamagePoison.doInfection(_user, cha, 3000, 5);
				break;
			// 木乃伊的咀咒
			case CURSE_PARALYZE:
			case CURSE_PARALYZE2:
				if (!cha.hasSkillEffect(EARTH_BIND) && !cha.hasSkillEffect(ICE_LANCE) && !cha.hasSkillEffect(FREEZING_BLIZZARD) && !cha.hasSkillEffect(FREEZING_BREATH))
				{
					if (cha is L1PcInstance)
					{
						L1CurseParalysis.curse(cha, 8000, 16000);
					}
					else if (cha is L1MonsterInstance)
					{
						L1CurseParalysis.curse(cha, 8000, 16000);
					}
				}
				break;
			// 弱化術
			case WEAKNESS:
				cha.addDmgup(-5);
				cha.addHitup(-1);
				break;
			// 疾病術
			case DISEASE:
				cha.addDmgup(-6);
				cha.addAc(12);
				break;
			// 風之枷鎖
			case WIND_SHACKLE:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_SkillIconWindShackle(pc.Id, _getBuffIconDuration));
					pc.broadcastPacket(new S_SkillIconWindShackle(pc.Id, _getBuffIconDuration));
				}
				break;
			// 魔法相消術
			case CANCELLATION:
				if (cha is L1NpcInstance)
				{
					L1NpcInstance npc = (L1NpcInstance) cha;
					int npcId = npc.NpcTemplate.get_npcId();
					if (npcId == 71092)
					{ // 調査員
						if (npc.GfxId == npc.TempCharGfx)
						{
							npc.TempCharGfx = 1314;
							npc.broadcastPacket(new S_NpcChangeShape(npc.Id,1314, npc.Lawful, npc.Status));
							return 0;
						}
						else
						{
							return 0;
						}
					}
					if (npcId == 45640)
					{ // 獨角獸
						if (npc.GfxId == npc.TempCharGfx)
						{
							npc.CurrentHp = npc.MaxHp;
							npc.TempCharGfx = 2332;
							npc.broadcastPacket(new S_NpcChangeShape(npc.Id, 2332, npc.Lawful, npc.Status));
							npc.Name = "$2103";
							npc.NameId = "$2103";
							npc.broadcastPacket(new S_ChangeName(npc.Id,"$2103"));
						}
						else if (npc.TempCharGfx == 2332)
						{
							npc.CurrentHp = npc.MaxHp;
							npc.TempCharGfx = 2755;
							npc.broadcastPacket(new S_NpcChangeShape(npc.Id, 2755, npc.Lawful, npc.Status));
							npc.Name = "$2488";
							npc.NameId = "$2488";
							npc.broadcastPacket(new S_ChangeName(npc.Id, "$2488"));
						}
					}
					if (npcId == 81209)
					{ // 羅伊
						if (npc.GfxId == npc.TempCharGfx)
						{
							npc.TempCharGfx = 4310;
							npc.broadcastPacket(new S_NpcChangeShape(npc.Id, 4310, npc.Lawful, npc.Status));
							return 0;
						}
						else
						{
							return 0;
						}
					}
					if (npcId == 81352)
					{ // 歐姆民兵
						if (npc.GfxId == npc.TempCharGfx)
						{
							npc.TempCharGfx = 148;
							npc.broadcastPacket(new S_NpcChangeShape(npc.Id, 148, npc.Lawful, npc.Status));
							npc.Name = "$6068";
							npc.NameId = "$6068";
							npc.broadcastPacket(new S_ChangeName(npc.Id, "$6068"));
						}
					}
				}
				if ((_player != null) && _player.Invisble)
				{
					_player.delInvis();
				}
				if (!(cha is L1PcInstance))
				{
					L1NpcInstance npc = (L1NpcInstance) cha;
					npc.MoveSpeed = 0;
					npc.BraveSpeed = 0;
					npc.broadcastPacket(new S_SkillHaste(cha.Id, 0, 0));
					npc.broadcastPacket(new S_SkillBrave(cha.Id, 0, 0));
					npc.WeaponBreaked = false;
					npc.Paralyzed = false;
					npc.ParalysisTime = 0;
				}

				// スキルの解除
				for (int skillNum = SKILLS_BEGIN; skillNum <= SKILLS_END; skillNum++)
				{
					if (isNotCancelable(skillNum) && !cha.Dead)
					{
						continue;
					}
					cha.removeSkillEffect(skillNum);
				}

				// ステータス強化、異常の解除
				cha.curePoison();
				cha.cureParalaysis();
				for (int skillNum = STATUS_BEGIN; skillNum <= STATUS_END; skillNum++)
				{
					if (skillNum == STATUS_CHAT_PROHIBITED)
					{ // 禁言
						continue;
					}
					cha.removeSkillEffect(skillNum);
				}

				if (cha is L1PcInstance)
				{
				}

				// 料理の解除
				for (int skillNum = COOKING_BEGIN; skillNum <= COOKING_END; skillNum++)
				{
					if (isNotCancelable(skillNum))
					{
						continue;
					}
					cha.removeSkillEffect(skillNum);
				}

				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;

					// アイテム装備による変身の解除
					L1PolyMorph.undoPoly(pc);
					pc.sendPackets(new S_CharVisualUpdate(pc));
					pc.broadcastPacket(new S_CharVisualUpdate(pc));

					// ヘイストアイテム装備時はヘイスト関連のスキルが何も掛かっていないはずなのでここで解除
					if (pc.HasteItemEquipped > 0)
					{
						pc.MoveSpeed = 0;
						pc.sendPackets(new S_SkillHaste(pc.Id, 0, 0));
						pc.broadcastPacket(new S_SkillHaste(pc.Id, 0, 0));
					}
				}
				cha.removeSkillEffect(STATUS_FREEZE); // Freeze解除
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_CharVisualUpdate(pc));
					pc.broadcastPacket(new S_CharVisualUpdate(pc));
					if (pc.PrivateShop)
					{
						pc.sendPackets(new S_DoActionShop(pc.Id, ActionCodes.ACTION_Shop, pc.ShopChat));
						pc.broadcastPacket(new S_DoActionShop(pc.Id, ActionCodes.ACTION_Shop, pc.ShopChat));
					}
					if (_user is L1PcInstance)
					{
						L1PinkName.onAction(pc, _user);
					}
				}
				break;
			// 沉睡之霧
			case FOG_OF_SLEEPING:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_SLEEP, true));
				}
				cha.Sleeped = true;
				break;
			// 護衛毀滅
			case GUARD_BRAKE:
				cha.addAc(15);
				break;
			// 驚悚死神
			case HORROR_OF_DEATH:
				cha.addStr(-5);
				cha.addInt(-5);
				break;
			// 恐慌
			case PANIC:
				cha.addStr((sbyte) -1);
				cha.addCon((sbyte) -1);
				cha.addDex((sbyte) -1);
				cha.addWis((sbyte) -1);
				cha.addInt((sbyte) -1);
				break;
			// 恐懼無助
			case RESIST_FEAR:
				cha.addNdodge((sbyte) 5); // 閃避率 - 50%
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					// 更新閃避率顯示
					pc.sendPackets(new S_PacketBox(101, pc.Ndodge));
				}
				break;
			// 釋放元素
			case RETURN_TO_NATURE:
				if (Config.RETURN_TO_NATURE && (cha is L1SummonInstance))
				{
					L1SummonInstance summon = (L1SummonInstance) cha;
					summon.broadcastPacket(new S_SkillSound(summon.Id, 2245));
					summon.returnToNature();
				}
				else
				{
					if (_user is L1PcInstance)
					{
						_player.sendPackets(new S_ServerMessage(79));
					}
				}
				break;
			// 壞物術
			case WEAPON_BREAK:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					L1ItemInstance weapon = pc.Weapon;
					if (weapon != null)
					{
						int weaponDamage = RandomHelper.Next(_user.getInt() / 3) + 1;
						// \f1你的%0%s壞了。
						pc.sendPackets(new S_ServerMessage(268, weapon.LogName));
						pc.Inventory.receiveDamage(weapon, weaponDamage);
					}
				}
				else
				{
					((L1NpcInstance) cha).WeaponBreaked = true;
				}
				break;

			// 輔助性魔法
			// 鏡像、暗影閃避
			case MIRROR_IMAGE:
			case UNCANNY_DODGE:
				if (_user is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) _user;
					pc.addDodge((sbyte) 5); // 閃避率 + 50%
					// 更新閃避率顯示
					pc.sendPackets(new S_PacketBox(88, pc.Dodge));
				}
				break;
			// 激勵士氣
			case GLOWING_AURA:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addHitup(5);
					pc.addBowHitup(5);
					pc.addMr(20);
					pc.sendPackets(new S_SkillIconAura(113, _getBuffIconDuration));
					pc.sendPackets(new S_SPMR(pc));
				}
				break;
			// 鋼鐵士氣
			case SHINING_AURA:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(-8);
					pc.sendPackets(new S_SkillIconAura(114, _getBuffIconDuration));
				}
				break;
			// 衝擊士氣
			case BRAVE_AURA:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addDmgup(5);
					pc.sendPackets(new S_SkillIconAura(116, _getBuffIconDuration));
				}
				break;
			// 防護罩
			case SHIELD:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(-2);
					pc.sendPackets(new S_SkillIconShield(2, _getBuffIconDuration));
				}
				break;
			// 影之防護
			case SHADOW_ARMOR:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(-3);
					pc.sendPackets(new S_SkillIconShield(3, _getBuffIconDuration));
				}
				break;
			// 大地防護
			case EARTH_SKIN:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(-6);
					pc.sendPackets(new S_SkillIconShield(6, _getBuffIconDuration));
				}
				break;
			// 大地的祝福
			case EARTH_BLESS:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(-7);
					pc.sendPackets(new S_SkillIconShield(7, _getBuffIconDuration));
				}
				break;
			// 鋼鐵防護
			case IRON_SKIN:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(-10);
					pc.sendPackets(new S_SkillIconShield(10, _getBuffIconDuration));
				}
				break;
			// 體魄強健術
			case PHYSICAL_ENCHANT_STR:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addStr((sbyte) 5);
					pc.sendPackets(new S_Strup(pc, 5, _getBuffIconDuration));
				}
				break;
			// 通暢氣脈術
			case PHYSICAL_ENCHANT_DEX:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addDex((sbyte) 5);
					pc.sendPackets(new S_Dexup(pc, 5, _getBuffIconDuration));
				}
				break;
			// 力量提升
			case DRESS_MIGHTY:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addStr((sbyte) 2);
					pc.sendPackets(new S_Strup(pc, 2, _getBuffIconDuration));
				}
				break;
			// 敏捷提升
			case DRESS_DEXTERITY:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addDex((sbyte) 2);
					pc.sendPackets(new S_Dexup(pc, 2, _getBuffIconDuration));
				}
				break;
			// 魔法防禦
			case RESIST_MAGIC:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addMr(10);
					pc.sendPackets(new S_SPMR(pc));
				}
				break;
			// 淨化精神
			case CLEAR_MIND:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addWis((sbyte) 3);
					pc.resetBaseMr();
				}
				break;
			// 屬性防禦
			case RESIST_ELEMENTAL:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addWind(10);
					pc.addWater(10);
					pc.addFire(10);
					pc.addEarth(10);
					pc.sendPackets(new S_OwnCharAttrDef(pc));
				}
				break;
			// 單屬性防禦
			case ELEMENTAL_PROTECTION:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					int attr = pc.ElfAttr;
					if (attr == 1)
					{
						pc.addEarth(50);
					}
					else if (attr == 2)
					{
						pc.addFire(50);
					}
					else if (attr == 4)
					{
						pc.addWater(50);
					}
					else if (attr == 8)
					{
						pc.addWind(50);
					}
				}
				break;
			// 心靈轉換
			case BODY_TO_MIND:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.CurrentMp = pc.CurrentMp + 2;
				}
				break;
			// 魂體轉換
			case BLOODY_SOUL:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.CurrentMp = pc.CurrentMp + 12;
				}
				break;
			// 隱身術、暗隱術
			case INVISIBILITY:
			case BLIND_HIDING:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_Invis(pc.Id, 1));
					pc.broadcastPacketForFindInvis(new S_RemoveObject(pc), false);
				}
				break;
			// 火焰武器
			case FIRE_WEAPON:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addDmgup(4);
					pc.sendPackets(new S_SkillIconAura(147, _getBuffIconDuration));
				}
				break;
			// 烈炎氣息
			case FIRE_BLESS:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addDmgup(4);
					pc.sendPackets(new S_SkillIconAura(154, _getBuffIconDuration));
				}
				break;
			// 烈炎武器
			case BURNING_WEAPON:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addDmgup(6);
					pc.addHitup(3);
					pc.sendPackets(new S_SkillIconAura(162, _getBuffIconDuration));
				}
				break;
			// 風之神射
			case WIND_SHOT:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addBowHitup(6);
					pc.sendPackets(new S_SkillIconAura(148, _getBuffIconDuration));
				}
				break;
			// 暴風之眼
			case STORM_EYE:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addBowHitup(2);
					pc.addBowDmgup(3);
					pc.sendPackets(new S_SkillIconAura(155, _getBuffIconDuration));
				}
				break;
			// 暴風神射
			case STORM_SHOT:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addBowDmgup(5);
					pc.addBowHitup(-1);
					pc.sendPackets(new S_SkillIconAura(165, _getBuffIconDuration));
				}
				break;
			// 狂暴術
			case BERSERKERS:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addAc(10);
					pc.addDmgup(5);
					pc.addHitup(2);
				}
				break;
			// 變形術
			case SHAPE_CHANGE:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_ShowPolyList(pc.Id));
					if (!pc.ShapeChange)
					{
						pc.ShapeChange = true;
					}
				}
				break;
			// 靈魂昇華
			case ADVANCE_SPIRIT:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.AdvenHp = pc.BaseMaxHp / 5;
					pc.AdvenMp = pc.BaseMaxMp / 5;
					pc.addMaxHp(pc.AdvenHp);
					pc.addMaxMp(pc.AdvenMp);
					pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
					if (pc.InParty)
					{ // パーティー中
						pc.Party.updateMiniHP(pc);
					}
					pc.sendPackets(new S_MPUpdate(pc.CurrentMp, pc.MaxMp));
				}
				break;
			// 神聖疾走、行走加速、風之疾走
			case HOLY_WALK:
			case MOVING_ACCELERATION:
			case WIND_WALK:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.BraveSpeed = 4;
					pc.sendPackets(new S_SkillBrave(pc.Id, 4,_getBuffIconDuration));
					pc.broadcastPacket(new S_SkillBrave(pc.Id, 4, 0));
				}
				break;
			// 血之渴望
			case BLOODLUST:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.BraveSpeed = 6;
					pc.sendPackets(new S_SkillBrave(pc.Id, 6, _getBuffIconDuration));
					pc.broadcastPacket(new S_SkillBrave(pc.Id, 6, 0));
				}
				break;
			// 覺醒技能
			case AWAKEN_ANTHARAS:
			case AWAKEN_FAFURION:
			case AWAKEN_VALAKAS:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					L1Awake.start(pc, skillId);
				}
				break;
			// 幻覺：歐吉
			case ILLUSION_OGRE:
				cha.addDmgup(4);
				cha.addHitup(4);
				cha.addBowDmgup(4);
				cha.addBowHitup(4);
				break;
			// 幻覺：巫妖
			case ILLUSION_LICH:
				cha.addSp(2);
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_SPMR(pc));
				}
				break;
			// 幻覺：鑽石高侖
			case ILLUSION_DIA_GOLEM:
				cha.addAc(-20);
				break;
			// 幻覺：化身
			case ILLUSION_AVATAR:
				cha.addDmgup(10);
				cha.addBowDmgup(10);
				break;
			// 洞察
			case INSIGHT:
				cha.addStr((sbyte) 1);
				cha.addCon((sbyte) 1);
				cha.addDex((sbyte) 1);
				cha.addWis((sbyte) 1);
				cha.addInt((sbyte) 1);
				break;
			// 絕對屏障
			case ABSOLUTE_BARRIER:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.stopHpRegeneration();
					pc.stopMpRegeneration();
					pc.stopHpRegenerationByDoll();
					pc.stopMpRegenerationByDoll();
				}
				break;
			// 冥想術
			case MEDITATION:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addMpr(5);
				}
				break;
			// 專注
			case CONCENTRATION:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.addMpr(2);
				}
				break;

			// 目標 NPC
			// 能量感測
			case WEAK_ELEMENTAL:
				if (cha is L1MonsterInstance)
				{
					L1Npc npcTemp = ((L1MonsterInstance) cha).NpcTemplate;
					int weakAttr = npcTemp.get_weakAttr();
					if ((weakAttr & 1) == 1)
					{ // 地
						cha.broadcastPacket(new S_SkillSound(cha.Id, 2169));
					}
					else if ((weakAttr & 2) == 2)
					{ // 火
						cha.broadcastPacket(new S_SkillSound(cha.Id, 2166));
					}
					else if ((weakAttr & 4) == 4)
					{ // 水
						cha.broadcastPacket(new S_SkillSound(cha.Id, 2167));
					}
					else if ((weakAttr & 8) == 8)
					{ // 風
						cha.broadcastPacket(new S_SkillSound(cha.Id, 2168));
					}
					else
					{
						if (_user is L1PcInstance)
						{
							_player.sendPackets(new S_ServerMessage(79));
						}
					}
				}
				else
				{
					if (_user is L1PcInstance)
					{
						_player.sendPackets(new S_ServerMessage(79));
					}
				}
				break;

			// 傳送性魔法
			// 世界樹的呼喚
			case TELEPORT_TO_MATHER:
				if (_user is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					if (pc.Map.Escapable || pc.Gm)
					{
						L1Teleport.teleport(pc, 33051, 32337, (short) 4, 5, true);
					}
					else
					{
						pc.sendPackets(new S_ServerMessage(276)); // \f1在此無法使用傳送。
						pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
					}
				}
				break;

			// 召喚、迷魅、造屍
			// 召喚術
			case SUMMON_MONSTER:
				if (_user is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					int level = pc.Level;
					int[] summons;
					if (pc.Map.RecallPets)
					{
						if (pc.Inventory.checkEquipped(20284))
						{ // 召喚戒指
							if (!pc.SummonMonster)
							{
								pc.SummonMonster = true;
							}
							string SummonString = pc.SummonId.ToString();
							summonMonster(pc, SummonString);
						}
						else
						{
							summons = new int[] {81210, 81213, 81216, 81219, 81222, 81225, 81228};
							int summonid = 0;
							int summoncost = 8;
							int levelRange = 32;
							for (int i = 0; i < summons.Length; i++)
							{ // 該当ＬＶ範囲検索
								if ((level < levelRange) || (i == summons.Length - 1))
								{
									summonid = summons[i];
									break;
								}
								levelRange += 4;
							}

							int petcost = 0;
							object[] petlist = pc.PetList.Values.ToArray();
							foreach (object pet in petlist)
							{
								// 現在のペットコスト
								petcost += ((L1NpcInstance) pet).Petcost;
							}
							int pcCha = pc.Cha;
							if (pcCha > 34)
							{ // max count = 5
								pcCha = 34;
							}
							int charisma = pcCha + 6 - petcost;
							// int charisma = pc.getCha() + 6 - petcost;
							int summoncount = charisma / summoncost;
							L1Npc npcTemp = NpcTable.Instance.getTemplate(summonid);
							for (int i = 0; i < summoncount; i++)
							{
								L1SummonInstance summon = new L1SummonInstance(npcTemp, pc);
								summon.Petcost = summoncost;
							}
						}
						pc.SummonMonster = false;
					}
					else
					{
						pc.sendPackets(new S_ServerMessage(79));
					}
				}
				break;
			// 召喚屬性精靈、召喚強力屬性精靈
			case LESSER_ELEMENTAL:
			case GREATER_ELEMENTAL:
				if (_user is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					int attr = pc.ElfAttr;
					if (attr != 0)
					{ // 無属性でなければ実行
						if (pc.Map.RecallPets)
						{
							int petcost = 0;
							foreach (L1NpcInstance petNpc in pc.PetList.Values)
							{
								// 現在のペットコスト
								petcost += petNpc.Petcost;
							}

							if (petcost == 0)
							{ // 1匹も所属NPCがいなければ実行
								int summonid = 0;
								int[] summons;
								if (skillId == LESSER_ELEMENTAL)
								{ // レッサーエレメンタル[地,火,水,風]
									summons = new int[] {45306, 45303, 45304, 45305};
								}
								else
								{
									// グレーターエレメンタル[地,火,水,風]
									summons = new int[] {81053, 81050, 81051, 81052};
								}
								int npcattr = 1;
								for (int i = 0; i < summons.Length; i++)
								{
									if (npcattr == attr)
									{
										summonid = summons[i];
										i = summons.Length;
									}
									npcattr *= 2;
								}
								// 特殊設定の場合ランダムで出現
								if (summonid == 0)
								{

									int k3 = RandomHelper.Next(4);
									summonid = summons[k3];
								}

								L1Npc npcTemp = NpcTable.Instance.getTemplate(summonid);
								L1SummonInstance summon = new L1SummonInstance(npcTemp, pc);
								summon.Petcost = pc.Cha + 7; // 精霊の他にはNPCを所属させられない
							}
						}
						else
						{
							pc.sendPackets(new S_ServerMessage(79));
						}
					}
					else
					{
						pc.sendPackets(new S_ServerMessage(79));
					}
				}
				break;
			// 迷魅術
			case TAMING_MONSTER:
				if (cha is L1MonsterInstance)
				{
					L1MonsterInstance npc = (L1MonsterInstance) cha;
					// 可迷魅的怪物
					if (npc.NpcTemplate.Tamable)
					{
						int petcost = 0;
						object[] petlist = _user.PetList.Values.ToArray();
						foreach (object pet in petlist)
						{
							// 現在のペットコスト
							petcost += ((L1NpcInstance) pet).Petcost;
						}
						int charisma = _user.getCha();
						if (_player.Elf)
						{ // エルフ
							if (charisma > 30)
							{ // max count = 7
								charisma = 30;
							}
							charisma += 12;
						}
						else if (_player.Wizard)
						{ // ウィザード
							if (charisma > 36)
							{ // max count = 7
								charisma = 36;
							}
							charisma += 6;
						}
						charisma -= petcost;
						if (charisma >= 6)
						{ // ペットコストの確認
							L1SummonInstance summon = new L1SummonInstance(npc, _user, false);
							_target = summon; // ターゲット入替え
						}
						else
						{
							_player.sendPackets(new S_ServerMessage(319)); // \f1これ以上のモンスターを操ることはできません。
						}
					}
				}
				break;
			// 造屍術
			case CREATE_ZOMBIE:
				if (cha is L1MonsterInstance)
				{
					L1MonsterInstance npc = (L1MonsterInstance) cha;
					int petcost = 0;
					object[] petlist = _user.PetList.Values.ToArray();
					foreach (object pet in petlist)
					{
						// 現在のペットコスト
						petcost += ((L1NpcInstance) pet).Petcost;
					}
					int charisma = _user.getCha();
					if (_player.Elf)
					{ // エルフ
						if (charisma > 30)
						{ // max count = 7
							charisma = 30;
						}
						charisma += 12;
					}
					else if (_player.Wizard)
					{ // ウィザード
						if (charisma > 36)
						{ // max count = 7
							charisma = 36;
						}
						charisma += 6;
					}
					charisma -= petcost;
					if (charisma >= 6)
					{ // ペットコストの確認
						L1SummonInstance summon = new L1SummonInstance(npc, _user, true);
						_target = summon; // ターゲット入替え
					}
					else
					{
						_player.sendPackets(new S_ServerMessage(319)); // \f1これ以上のモンスターを操ることはできません。
					}
				}
				break;

			// 怪物專屬魔法
			case 10026:
			case 10027:
			case 10028:
			case 10029:
				if (_user is L1NpcInstance)
				{
					L1NpcInstance npc = (L1NpcInstance) _user;
					_user.broadcastPacket(new S_NpcChatPacket(npc, "$3717", 0)); // さあ、おまえに安息を与えよう。
				}
				else
				{
					_player.broadcastPacket(new S_ChatPacket(_player, "$3717", 0, 0)); // さあ、おまえに安息を与えよう。
				}
				break;
			case 10057:
				L1Teleport.teleportToTargetFront(cha, _user, 1);
				break;
			case STATUS_FREEZE:
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) cha;
					pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, true));
				}
				break;
			default:
				break;
			}
			return dmg;
		}

		private static bool isNotCancelable(int skillNum)
		{
			return (skillNum == ENCHANT_WEAPON) || (skillNum == BLESSED_ARMOR) || (skillNum == ABSOLUTE_BARRIER) || (skillNum == ADVANCE_SPIRIT) || (skillNum == SHOCK_STUN) || (skillNum == SHADOW_FANG) || (skillNum == REDUCTION_ARMOR) || (skillNum == SOLID_CARRIAGE) || (skillNum == COUNTER_BARRIER) || (skillNum == AWAKEN_ANTHARAS) || (skillNum == AWAKEN_FAFURION) || (skillNum == AWAKEN_VALAKAS) || (skillNum == COOKING_WONDER_DRUG);
		}

		private static void summonMonster(L1PcInstance pc, string s)
		{
			string[] summonstr_list;
			int[] summonid_list;
			int[] summonlvl_list;
			int[] summoncha_list;
			int summonid = 0;
			int levelrange = 0;
			int summoncost = 0;
			summonstr_list = new string[] {"7", "263", "519", "8", "264", "520", "9", "265", "521", "10", "266", "522", "11", "267", "523", "12", "268", "524", "13", "269", "525", "14", "270", "526", "15", "271", "527", "16", "17", "18", "274"};
			summonid_list = new int[] {81210, 81211, 81212, 81213, 81214, 81215, 81216, 81217, 81218, 81219, 81220, 81221, 81222, 81223, 81224, 81225, 81226, 81227, 81228, 81229, 81230, 81231, 81232, 81233, 81234, 81235, 81236, 81237, 81238, 81239, 81240};
			summonlvl_list = new int[] {28, 28, 28, 32, 32, 32, 36, 36, 36, 40, 40, 40, 44, 44, 44, 48, 48, 48, 52, 52, 52, 56, 56, 56, 60, 60, 60, 64, 68, 72, 72};
			summoncha_list = new int[] {8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 14, 36, 36, 44};
			// サモンの種類、必要Lv、ペットコストを得る
			for (int loop = 0; loop < summonstr_list.Length; loop++)
			{
				if (s.Equals(summonstr_list[loop], StringComparison.OrdinalIgnoreCase))
				{
					summonid = summonid_list[loop];
					levelrange = summonlvl_list[loop];
					summoncost = summoncha_list[loop];
					break;
				}
			}
			// Lv不足
			if (pc.Level < levelrange)
			{
				// レベルが低くて該当のモンスターを召還することができません。
				pc.sendPackets(new S_ServerMessage(743));
				return;
			}

			int petcost = 0;
			foreach (L1NpcInstance petNpc in pc.PetList.Values)
			{
				// 現在のペットコスト
				petcost += petNpc.Petcost;
			}

			int pcCha = pc.Cha;
			int charisma = 0;
			int summoncount = 0;
			if ((levelrange <= 56) || (levelrange == 64))
			{ // max count = 2
				if (pcCha > 34)
				{
					pcCha = 34;
				}
			}
			else if (levelrange == 60)
			{
				if (pcCha > 30)
				{ // max count = 3
					pcCha = 30;
				}
			}
			else if (levelrange > 64)
			{
				if (pcCha > 44)
				{ // max count = 1
					pcCha = 44;
				}
			}
			charisma = pcCha + 6 - petcost;
			summoncount = charisma / summoncost;

			L1Npc npcTemp = NpcTable.Instance.getTemplate(summonid);
			for (int cnt = 0; cnt < summoncount; cnt++)
			{
				L1SummonInstance summon = new L1SummonInstance(npcTemp, pc);
				summon.Petcost = summoncost;
			}
		}
	}

}