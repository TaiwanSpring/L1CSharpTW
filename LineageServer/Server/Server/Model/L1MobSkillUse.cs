using System;
using System.Collections.Generic;
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
namespace LineageServer.Server.Server.Model
{

	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using MobSkillTable = LineageServer.Server.Server.DataSources.MobSkillTable;
	using NpcTable = LineageServer.Server.Server.DataSources.NpcTable;
	using SkillsTable = LineageServer.Server.Server.DataSources.SkillsTable;
	using SprTable = LineageServer.Server.Server.DataSources.SprTable;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using L1SkillUse = LineageServer.Server.Server.Model.skill.L1SkillUse;
	using S_CharVisualUpdate = LineageServer.Server.Server.serverpackets.S_CharVisualUpdate;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_NPCPack = LineageServer.Server.Server.serverpackets.S_NPCPack;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using L1MobSkill = LineageServer.Server.Server.Templates.L1MobSkill;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using L1Skills = LineageServer.Server.Server.Templates.L1Skills;
	using Random = LineageServer.Server.Server.Utils.Random;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	public class L1MobSkillUse
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1MobSkillUse).FullName);

		private L1MobSkill _mobSkillTemplate = null;

		private L1NpcInstance _attacker = null;

		private L1Character _target = null;

		private int _sleepTime = 0;

		private int[] _skillUseCount;

		public L1MobSkillUse(L1NpcInstance npc)
		{
			_sleepTime = 0;

			_mobSkillTemplate = MobSkillTable.Instance.getTemplate(npc.NpcTemplate.get_npcId());
			if (_mobSkillTemplate == null)
			{
				return;
			}
			_attacker = npc;
			_skillUseCount = new int[MobSkillTemplate.SkillSize];
		}

		private int getSkillUseCount(int idx)
		{
			return _skillUseCount[idx];
		}

		private void skillUseCountUp(int idx)
		{
			_skillUseCount[idx]++;
		}

		public virtual void resetAllSkillUseCount()
		{
			if (MobSkillTemplate == null)
			{
				return;
			}

			for (int i = 0; i < MobSkillTemplate.SkillSize; i++)
			{
				_skillUseCount[i] = 0;
			}
		}

		public virtual int SleepTime
		{
			get
			{
				return _sleepTime;
			}
			set
			{
				_sleepTime = value;
			}
		}


		public virtual L1MobSkill MobSkillTemplate
		{
			get
			{
				return _mobSkillTemplate;
			}
		}

		/*
		 * トリガーの条件のみチェック。
		 */
		public virtual bool isSkillTrigger(L1Character tg)
		{
			if (_mobSkillTemplate == null)
			{
				return false;
			}
			_target = tg;

			int type;
			type = MobSkillTemplate.getType(0);

			if (type == L1MobSkill.TYPE_NONE)
			{
				return false;
			}

			for (int i = 0; (i < MobSkillTemplate.SkillSize) && (MobSkillTemplate.getType(i) != L1MobSkill.TYPE_NONE); i++)
			{
				if (isSkillUseble(i, false))
				{
					return true;
				}
			}
			return false;
		}

		/*
		 * スキル攻撃 スキル攻撃可能ならばtrueを返す。 攻撃できなければfalseを返す。
		 */
		public virtual bool skillUse(L1Character tg, bool isTriRnd)
		{
			if (_mobSkillTemplate == null)
			{
				return false;
			}
			_target = tg;

			int type;
			type = MobSkillTemplate.getType(0);

			if (type == L1MobSkill.TYPE_NONE)
			{
				return false;
			}

			int[] skills = null;
			int skillSizeCounter = 0;
			int skillSize = MobSkillTemplate.SkillSize;
			if (skillSize >= 0)
			{
				skills = new int[skillSize];
			}

			for (int i = 0; (i < MobSkillTemplate.SkillSize) && (MobSkillTemplate.getType(i) != L1MobSkill.TYPE_NONE); i++)
			{
				if (isSkillUseble(i, isTriRnd) == false)
				{
					continue;
				}
				else
				{ // 条件にあうスキルが存在する
					skills[skillSizeCounter] = i;
					skillSizeCounter++;
				}
			}

			if (skillSizeCounter != 0)
			{
				int num = RandomHelper.Next(skillSizeCounter);
				if (useSkill(skills[num]))
				{ // スキル使用
					return true;
				}
			}

			return false;
		}

		private bool useSkill(int i)
		{
			// 對自身施法判斷
			int changeType = MobSkillTemplate.getChangeTarget(i);
			if (changeType == 2)
			{
				_target = changeTarget(changeType, i);
			}

			bool isUseSkill = false;
			int type = MobSkillTemplate.getType(i);
			if (type == L1MobSkill.TYPE_PHYSICAL_ATTACK)
			{ // 物理攻撃
				if (physicalAttack(i) == true)
				{
					skillUseCountUp(i);
					isUseSkill = true;
				}
			}
			else if (type == L1MobSkill.TYPE_MAGIC_ATTACK)
			{ // 魔法攻撃
				if (magicAttack(i) == true)
				{
					skillUseCountUp(i);
					isUseSkill = true;
				}
			}
			else if (type == L1MobSkill.TYPE_SUMMON)
			{ // サモンする
				if (summon(i) == true)
				{
					skillUseCountUp(i);
					isUseSkill = true;
				}
			}
			else if (type == L1MobSkill.TYPE_POLY)
			{ // 強制変身させる
				if (poly(i) == true)
				{
					skillUseCountUp(i);
					isUseSkill = true;
				}
			}
			return isUseSkill;
		}

		private bool summon(int idx)
		{
			int summonId = MobSkillTemplate.getSummon(idx);
			int min = MobSkillTemplate.getSummonMin(idx);
			int max = MobSkillTemplate.getSummonMax(idx);
			int count = 0;
			int actId = MobSkillTemplate.getActid(idx);
			int gfxId = MobSkillTemplate.getGfxid(idx);

			if (summonId == 0)
			{
				return false;
			}

			// 施法動作
			if (actId > 0)
			{
				S_DoActionGFX gfx = new S_DoActionGFX(_attacker.Id, actId);
				_attacker.broadcastPacket(gfx);
				_sleepTime = SprTable.Instance.getSprSpeed(_attacker.TempCharGfx, actId);
			}
			// 魔方陣
			if (gfxId > 0)
			{
				_attacker.broadcastPacket(new S_SkillSound(_attacker.Id, gfxId));
			}
			count = RandomHelper.Next(max) + min;
			mobspawn(summonId, count);
			return true;
		}

		/*
		 * 15セル以内で射線が通るPCを指定したモンスターに強制変身させる。 対PCしか使えない。
		 */
		private bool poly(int idx)
		{
			int polyId = MobSkillTemplate.getPolyId(idx);
			int actId = MobSkillTemplate.getActid(idx);
			bool usePoly = false;

			if (polyId == 0)
			{
				return false;
			}
			// 施法動作
			if (actId > 0)
			{
				S_DoActionGFX gfx = new S_DoActionGFX(_attacker.Id, actId);
				_attacker.broadcastPacket(gfx);
				_sleepTime = SprTable.Instance.getSprSpeed(_attacker.TempCharGfx, actId);
			}

			foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(_attacker))
			{
				if (pc.Dead)
				{ // 死亡
					continue;
				}
				if (pc.Ghost)
				{
					continue;
				}
				if (pc.GmInvis)
				{
					continue;
				}
				if (_attacker.glanceCheck(pc.X, pc.Y) == false)
				{
					continue; // 射線が通らない
				}

				switch (_attacker.NpcTemplate.get_npcId())
				{
					case 81082: // 火焰之影
						pc.Inventory.takeoffEquip(945); // 將目標裝備卸下。
						break;
					default:
						break;
				}
				_attacker.broadcastPacket(new S_SkillSound(pc.Id, 230));
				L1PolyMorph.doPoly(pc, polyId, 1800, L1PolyMorph.MORPH_BY_NPC);
				usePoly = true;
			}
			return usePoly;
		}

		private bool magicAttack(int idx)
		{
			L1SkillUse skillUse = new L1SkillUse();
			int skillid = MobSkillTemplate.getSkillId(idx);
			int actId = MobSkillTemplate.getActid(idx);
			int gfxId = MobSkillTemplate.getGfxid(idx);
			int mpConsume = MobSkillTemplate.getMpConsume(idx);
			bool canUseSkill = false;

			if (skillid > 0)
			{
				skillUse.SkillRanged = MobSkillTemplate.getRange(idx); // 變更技能施放距離
				skillUse.SkillRanged = MobSkillTemplate.getSkillArea(idx); // 變更技能施放範圍
				canUseSkill = skillUse.checkUseSkill(null, skillid, _target.Id, _target.X, _target.Y, null, 0, L1SkillUse.TYPE_NORMAL, _attacker, actId, gfxId, mpConsume);
			}

			L1Skills skill = SkillsTable.Instance.getTemplate(skillid);
			if (skill.Target.Equals("buff") && _target.hasSkillEffect(skillid))
			{
				return false;
			}

			if (canUseSkill == true)
			{
				if (MobSkillTemplate.getLeverage(idx) > 0)
				{
					skillUse.Leverage = MobSkillTemplate.getLeverage(idx);
				}
				skillUse.handleCommands(null, skillid, _target.Id, _target.X, _target.X, null, 0, L1SkillUse.TYPE_NORMAL, _attacker);

				// 延遲時間判斷
				if (actId == 0)
				{
					actId = skill.ActionId;
				}
				_sleepTime = SprTable.Instance.getSprSpeed(_attacker.TempCharGfx, actId);

				return true;
			}
			return false;
		}

		/*
		 * 物理攻撃
		 */
		private bool physicalAttack(int idx)
		{
			IDictionary<int, int> targetList = Maps.newConcurrentMap();
			int areaWidth = MobSkillTemplate.getAreaWidth(idx);
			int areaHeight = MobSkillTemplate.getAreaHeight(idx);
			int range = MobSkillTemplate.getRange(idx);
			int actId = MobSkillTemplate.getActid(idx);
			int gfxId = MobSkillTemplate.getGfxid(idx);

			// レンジ外
			if (_attacker.Location.getTileLineDistance(_target.Location) > range)
			{
				return false;
			}

			// 障害物がある場合攻撃不可能
			if (!_attacker.glanceCheck(_target.X, _target.Y))
			{
				return false;
			}

			_attacker.Heading = _attacker.targetDirection(_target.X, _target.Y); // 向きのセット

			if (areaHeight > 0)
			{
				// 範囲攻撃
				IList<L1Object> objs = L1World.Instance.getVisibleBoxObjects(_attacker, _attacker.Heading, areaWidth, areaHeight);

				foreach (L1Object obj in objs)
				{
					if (!(obj is L1Character))
					{ // ターゲットがキャラクター以外の場合何もしない。
						continue;
					}

					L1Character cha = (L1Character) obj;
					if (cha.Dead)
					{ // 死んでるキャラクターは対象外
						continue;
					}

					// ゴースト状態は対象外
					if (cha is L1PcInstance)
					{
						if (((L1PcInstance) cha).Ghost)
						{
							continue;
						}
					}

					// 障害物がある場合は対象外
					if (!_attacker.glanceCheck(cha.X, cha.Y))
					{
						continue;
					}

					if ((_target is L1PcInstance) || (_target is L1SummonInstance) || (_target is L1PetInstance))
					{
						// 対PC
						if (((obj is L1PcInstance) && !((L1PcInstance) obj).Ghost && !((L1PcInstance) obj).GmInvis) || (obj is L1SummonInstance) || (obj is L1PetInstance))
						{
							targetList[obj.Id] = 0;
						}
					}
					else
					{
						// 対NPC
						if (obj is L1MonsterInstance)
						{
							targetList[obj.Id] = 0;
						}
					}
				}
			}
			else
			{
				// 単体攻撃
				targetList[_target.Id] = 0; // ターゲットのみ追加
			}

			if (targetList.Count == 0)
			{
				return false;
			}

			IEnumerator<int> ite = targetList.Keys.GetEnumerator();
			while (ite.MoveNext())
			{
				int targetId = ite.Current;
				L1Attack attack = new L1Attack(_attacker, (L1Character) L1World.Instance.findObject(targetId));
				if (attack.calcHit())
				{
					if (MobSkillTemplate.getLeverage(idx) > 0)
					{
						attack.Leverage = MobSkillTemplate.getLeverage(idx);
					}
					attack.calcDamage();
				}
				if (actId > 0)
				{
					attack.ActId = actId;
				}
				// 攻撃モーションは実際のターゲットに対してのみ行う
				if (targetId == _target.Id)
				{
					if (gfxId > 0)
					{
						_attacker.broadcastPacket(new S_SkillSound(_attacker.Id, gfxId));
					}
					attack.action();
				}
				attack.commit();
			}

			if (actId > 0)
			{
				_sleepTime = SprTable.Instance.getSprSpeed(_attacker.TempCharGfx, actId);
			}
			else
			{
				_sleepTime = _attacker.Atkspeed;
			}
			return true;
		}

		/*
		 * トリガーの条件のみチェック
		 */
		private bool isSkillUseble(int skillIdx, bool isTriRnd)
		{
			bool useble = false;
			int type = MobSkillTemplate.getType(skillIdx);
			int chance = RandomHelper.Next(100) + 1;

			if (chance > MobSkillTemplate.getTriggerRandom(skillIdx))
			{
				return false;
			}

			if (isTriRnd || (type == L1MobSkill.TYPE_SUMMON) || (type == L1MobSkill.TYPE_POLY))
			{
				/*if (getMobSkillTemplate().getTriggerRandom(skillIdx) > 0) {
					int chance = RandomHelper.NextInt(100) + 1;
					if (chance < getMobSkillTemplate().getTriggerRandom(skillIdx)) {
						useble = true;
					}
					else {
						return false;
					}
				}*/
				// 確定此修改後的模式是仿正的，再移除此註解掉的程式碼
				useble = true;
			}

			if (MobSkillTemplate.getTriggerHp(skillIdx) > 0)
			{
				int hpRatio = (_attacker.CurrentHp * 100) / _attacker.MaxHp;
				if (hpRatio <= MobSkillTemplate.getTriggerHp(skillIdx))
				{
					useble = true;
				}
				else
				{
					return false;
				}
			}

			if (MobSkillTemplate.getTriggerCompanionHp(skillIdx) > 0)
			{
				L1NpcInstance companionNpc = searchMinCompanionHp();
				if (companionNpc == null)
				{
					return false;
				}

				int hpRatio = (companionNpc.CurrentHp * 100) / companionNpc.MaxHp;
				if (hpRatio <= MobSkillTemplate.getTriggerCompanionHp(skillIdx))
				{
					useble = true;
					_target = companionNpc; // ターゲットの入れ替え
				}
				else
				{
					return false;
				}
			}

			if (MobSkillTemplate.getTriggerRange(skillIdx) != 0)
			{
				int distance = _attacker.Location.getTileLineDistance(_target.Location);

				if (MobSkillTemplate.isTriggerDistance(skillIdx, distance))
				{
					useble = true;
				}
				else
				{
					return false;
				}
			}

			if (MobSkillTemplate.getTriggerCount(skillIdx) > 0)
			{
				if (getSkillUseCount(skillIdx) < MobSkillTemplate.getTriggerCount(skillIdx))
				{
					useble = true;
				}
				else
				{
					return false;
				}
			}
			return useble;
		}

		private L1NpcInstance searchMinCompanionHp()
		{
			L1NpcInstance npc;
			L1NpcInstance minHpNpc = null;
			int hpRatio = 100;
			int companionHpRatio;
			int family = _attacker.NpcTemplate.get_family();

			foreach (L1Object @object in L1World.Instance.getVisibleObjects(_attacker))
			{
				if (@object is L1NpcInstance)
				{
					npc = (L1NpcInstance) @object;
					if (npc.NpcTemplate.get_family() == family)
					{
						companionHpRatio = (npc.CurrentHp * 100) / npc.MaxHp;
						if (companionHpRatio < hpRatio)
						{
							hpRatio = companionHpRatio;
							minHpNpc = npc;
						}
					}
				}
			}
			return minHpNpc;
		}

		private void mobspawn(int summonId, int count)
		{
			int i;

			for (i = 0; i < count; i++)
			{
				mobspawn(summonId);
			}
		}

		private void mobspawn(int summonId)
		{
			try
			{
				L1Npc spawnmonster = NpcTable.Instance.getTemplate(summonId);
				if (spawnmonster != null)
				{
					L1NpcInstance mob = null;
					try
					{
						string implementationName = spawnmonster.Impl;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> _constructor = Class.forName((new StringBuilder()).append("l1j.server.server.model.Instance.").append(implementationName).append("Instance").toString()).getConstructors()[0];
						System.Reflection.ConstructorInfo<object> _constructor = Type.GetType((new StringBuilder()).Append("l1j.server.server.model.Instance.").Append(implementationName).Append("Instance").ToString()).GetConstructors()[0];
						mob = (L1NpcInstance) _constructor.Invoke(new object[] {spawnmonster});
						mob.Id = IdFactory.Instance.nextId();
						L1Location loc = _attacker.Location.randomLocation(8, false);
						int heading = RandomHelper.Next(8);
						mob.X = loc.X;
						mob.Y = loc.Y;
						mob.HomeX = loc.X;
						mob.HomeY = loc.Y;
						short mapid = _attacker.MapId;
						mob.Map = mapid;
						mob.Heading = heading;
						L1World.Instance.storeObject(mob);
						L1World.Instance.addVisibleObject(mob);
						L1Object @object = L1World.Instance.findObject(mob.Id);
						L1MonsterInstance newnpc = (L1MonsterInstance) @object;
						newnpc.set_storeDroped(true); // 召喚怪不會掉落道具
						if (newnpc.TempCharGfx == 145)
						{ // 史巴托
							newnpc.Status = 11;
							newnpc.broadcastPacket(new S_NPCPack(newnpc));
							newnpc.broadcastPacket(new S_DoActionGFX(newnpc.Id, ActionCodes.ACTION_Appear));
							newnpc.Status = 0;
							newnpc.broadcastPacket(new S_CharVisualUpdate(newnpc, newnpc.Status));
						}
						else if (newnpc.TempCharGfx == 7591)
						{ // 泥龍(地)
							newnpc.broadcastPacket(new S_NPCPack(newnpc));
							newnpc.broadcastPacket(new S_DoActionGFX(newnpc.Id, ActionCodes.ACTION_AxeWalk));
						}
						newnpc.onNpcAI();
						newnpc.turnOnOffLight();
						newnpc.startChat(L1NpcInstance.CHAT_TIMING_APPEARANCE); // チャット開始
					}
					catch (Exception e)
					{
						_log.log(Enum.Level.Server, e.Message, e);
					}
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}

		// 現在ChangeTargetで有効な値は2,3のみ
		private L1Character changeTarget(int type, int idx)
		{
			L1Character target;

			switch (type)
			{
				case L1MobSkill.CHANGE_TARGET_ME:
					target = _attacker;
					break;
				case L1MobSkill.CHANGE_TARGET_RANDOM:
					// ターゲット候補の選定
					IList<L1Character> targetList = Lists.newList();
					foreach (L1Object obj in L1World.Instance.getVisibleObjects(_attacker))
					{
						if ((obj is L1PcInstance) || (obj is L1PetInstance) || (obj is L1SummonInstance))
						{
							L1Character cha = (L1Character) obj;

							int distance = _attacker.Location.getTileLineDistance(cha.Location);

							// 発動範囲外のキャラクターは対象外
							if (!MobSkillTemplate.isTriggerDistance(idx, distance))
							{
								continue;
							}

							// 障害物がある場合は対象外
							if (!_attacker.glanceCheck(cha.X, cha.Y))
							{
								continue;
							}

							if (!_attacker.HateList.containsKey(cha))
							{ // ヘイトがない場合対象外
								continue;
							}

							if (cha.Dead)
							{ // 死んでるキャラクターは対象外
								continue;
							}

							// ゴースト状態は対象外
							if (cha is L1PcInstance)
							{
								if (((L1PcInstance) cha).Ghost)
								{
									continue;
								}
							}
							targetList.Add((L1Character) obj);
						}
					}

					if (targetList.Count == 0)
					{
						target = _target;
					}
					else
					{
						int randomSize = targetList.Count * 100;
						int targetIndex = RandomHelper.Next(randomSize) / 100;
						target = targetList[targetIndex];
					}
					break;

				default:
					target = _target;
					break;
			}
			return target;
		}
	}

}