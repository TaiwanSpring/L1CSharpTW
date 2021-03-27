using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.trap;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Model.skill
{
	class L1SkillUse
	{
		public const int TYPE_NORMAL = 0;

		public const int TYPE_LOGIN = 1;

		public const int TYPE_SPELLSC = 2;

		public const int TYPE_NPCBUFF = 3;

		public const int TYPE_GMBUFF = 4;

		private L1Skills _skill;

		private int _skillId;

		private int _dmg;

		private int _getBuffDuration;

		private int _shockStunDuration;

		private int _getBuffIconDuration;

		private int _targetID;

		private int _mpConsume = 0;

		private int _hpConsume = 0;

		private int _targetX = 0;

		private int _targetY = 0;

		private string _message = null;

		private int _skillTime = 0;

		private int _type = 0;

		private bool _isPK = false;

		private int _bookmarkId = 0;

		private int _itemobjid = 0;

		private bool _checkedUseSkill = false; // 事前チェック済みか

		private int _leverage = 10; // 1/10倍なので10で1倍

		private int _skillRanged = 0;

		private int _skillArea = 0;

		private bool _isFreeze = false;

		private bool _isCounterMagic = true;

		private bool _isGlanceCheckFail = false;

		private L1Character _user = null;

		private L1Character _target = null;

		private L1PcInstance _player = null;

		private L1NpcInstance _npc = null;

		private int _calcType;

		private const int PC_PC = 1;

		private const int PC_NPC = 2;

		private const int NPC_PC = 3;

		private const int NPC_NPC = 4;

		private IList<TargetStatus> _targetList;

		private int _actid = 0;

		private int _gfxid = 0;

		private static ILogger _log = Logger.GetLogger(nameof(L1SkillUse));

		private static readonly int[] CAST_WITH_INVIS = new int[]
		{
			1, 2, 3, 5, 8,
			9, 12, 13, 14, 19,
			21, 26, 31, 32, 35,
			37, 42, 43, 44, 48,
			49, 52, 54, 55, 57,
			60, 61, 63, 67, 68,
			69, 72, 73, 75, 78,
			79, L1SkillId.REDUCTION_ARMOR, L1SkillId.BOUNCE_ATTACK, L1SkillId.SOLID_CARRIAGE, L1SkillId.COUNTER_BARRIER,
			97, 98, 99, 100, 101,
			102, 104, 105, 106, 107,
			109, 110, 111, 113, 114,
			115, 116, 117, 118, 129,
			130, 131, 133, 134, 137,
			138, 146, 147, 148, 149,
			150, 151, 155, 156, 158,
			159, 163, 164, 165, 166,
			168, 169, 170, 171, L1SkillId.SOUL_OF_FLAME,
			L1SkillId.ADDITIONAL_FIRE, L1SkillId.DRAGON_SKIN, L1SkillId.AWAKEN_ANTHARAS, L1SkillId.AWAKEN_FAFURION, L1SkillId.AWAKEN_VALAKAS,
			L1SkillId.MIRROR_IMAGE, L1SkillId.ILLUSION_OGRE, L1SkillId.ILLUSION_LICH, L1SkillId.PATIENCE, L1SkillId.ILLUSION_DIA_GOLEM, L1SkillId.INSIGHT, L1SkillId.ILLUSION_AVATAR };

		// 設定魔法屏障不可抵擋的魔法
		private static readonly int[] EXCEPT_COUNTER_MAGIC = new int[]
		{
			1, 2, 3, 5, 8,
			9, 12, 13, 14, 19,
			21, 26, 31, 32, 35,
			37, 42, 43, 44, 48,
			49, 52, 54, 55, 57,
			60, 61, 63, 67, 68,
			69, 72, 73, 75, 78,
			79, L1SkillId.SHOCK_STUN, L1SkillId.REDUCTION_ARMOR, L1SkillId.BOUNCE_ATTACK, L1SkillId.SOLID_CARRIAGE,
			L1SkillId.COUNTER_BARRIER, 97, 98, 99, 100,
			101, 102, 104, 105, 106,
			107, 109, 110, 111, 113,
			114, 115, 116, 117, 118,
			129, 130, 131, 132, 134,
			137, 138, 146, 147, 148,
			149, 150, 151, 155, 156,
			158, 159, 161, 163, 164,
			165, 166, 168, 169, 170,
			171, L1SkillId.SOUL_OF_FLAME, L1SkillId.ADDITIONAL_FIRE, L1SkillId.DRAGON_SKIN, L1SkillId.AWAKEN_ANTHARAS,
			L1SkillId.AWAKEN_FAFURION, L1SkillId.AWAKEN_VALAKAS, L1SkillId.MIRROR_IMAGE, L1SkillId.ILLUSION_OGRE, L1SkillId.ILLUSION_LICH,
			L1SkillId.PATIENCE, 10026, 10027, L1SkillId.ILLUSION_DIA_GOLEM, L1SkillId.INSIGHT,
			L1SkillId.ILLUSION_AVATAR, 10028, 10029 };

		public L1SkillUse()
		{
		}

		private class TargetStatus
		{
			internal L1Character _target = null;

			internal bool _isCalc = true; // ダメージや確率魔法の計算をする必要があるか？

			public TargetStatus(L1Character _cha)
			{
				_target = _cha;
			}

			public TargetStatus(L1Character _cha, bool _flg)
			{
				_isCalc = _flg;
			}

			public virtual L1Character Target
			{
				get
				{
					return _target;
				}
			}

			public virtual bool Calc
			{
				get
				{
					return _isCalc;
				}
			}
		}

		/*
		 * 攻擊距離變更。
		 */
		public virtual int SkillRanged
		{
			set
			{
				_skillRanged = value;
			}
			get
			{
				if (_skillRanged == 0)
				{
					return _skill.Ranged;
				}
				return _skillRanged;
			}
		}


		/*
		 * 攻擊範圍變更。
		 */
		public virtual int SkillArea
		{
			set
			{
				_skillArea = value;
			}
			get
			{
				if (_skillArea == 0)
				{
					return _skill.Area;
				}
				return _skillArea;
			}
		}


		/*
		 * 1/10倍で表現する。
		 */
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


		private bool CheckedUseSkill
		{
			get
			{
				return _checkedUseSkill;
			}
			set
			{
				_checkedUseSkill = value;
			}
		}


		public virtual bool checkUseSkill(L1PcInstance player, int skillid, int target_id, int x, int y, string message, int time, int type, L1Character attacker)
		{
			return checkUseSkill(player, skillid, target_id, x, y, message, time, type, attacker, 0, 0, 0);
		}

		public virtual bool checkUseSkill(L1PcInstance player, int skillid, int target_id, int x, int y, string message, int time, int type, L1Character attacker, int actid, int gfxid, int mpConsume)
		{
			// 初期設定ここから
			CheckedUseSkill = true;
			_targetList = ListFactory.NewList<TargetStatus>(); // ターゲットリストの初期化

			_skill = SkillsTable.Instance.getTemplate(skillid);
			_skillId = skillid;
			_targetX = x;
			_targetY = y;
			_message = message;
			_skillTime = time;
			_type = type;
			_actid = actid;
			_gfxid = gfxid;
			_mpConsume = mpConsume;
			bool checkedResult = true;

			if (attacker == null)
			{
				// pc
				_player = player;
				_user = _player;
			}
			else
			{
				// npc
				_npc = (L1NpcInstance)attacker;
				_user = _npc;
			}

			if (_skill.Target.Equals("none"))
			{
				_targetID = _user.Id;
				_targetX = _user.X;
				_targetY = _user.Y;
			}
			else
			{
				_targetID = target_id;
			}

			if (type == TYPE_NORMAL)
			{ // 通常の魔法使用時
				checkedResult = NormalSkillUsable;
			}
			else if (type == TYPE_SPELLSC)
			{ // スペルスクロール使用時
				checkedResult = SpellScrollUsable;
			}
			else if (type == TYPE_NPCBUFF)
			{
				checkedResult = true;
			}
			if (!checkedResult)
			{
				return false;
			}

			// ファイアーウォール、ライフストリームは詠唱対象が座標
			// キューブは詠唱者の座標に配置されるため例外
			if (( _skillId == L1SkillId.FIRE_WALL ) ||
				( _skillId == L1SkillId.LIFE_STREAM ) ||
				( _skillId == L1SkillId.TRUE_TARGET ))
			{
				return true;
			}

			GameObject l1object = L1World.Instance.findObject(_targetID);
			if (l1object is L1ItemInstance)
			{
				_log.Info($"skill target item name: {( (L1ItemInstance)l1object ).ViewName}");
				// スキルターゲットが精霊の石になることがある。
				// Linux環境で確認（Windowsでは未確認）
				// 2008.5.4追記：地面のアイテムに魔法を使うとなる。継続してもエラーになるだけなのでreturn
				return false;
			}
			if (_user is L1PcInstance)
			{
				if (l1object is L1PcInstance)
				{
					_calcType = PC_PC;
				}
				else
				{
					_calcType = PC_NPC;
				}
			}
			else if (_user is L1NpcInstance)
			{
				if (l1object is L1PcInstance)
				{
					_calcType = NPC_PC;
				}
				else if (_skill.Target.Equals("none"))
				{
					_calcType = NPC_PC;
				}
				else
				{
					_calcType = NPC_NPC;
				}
			}

			// テレポート、マステレポートは対象がブックマークID
			if (( _skillId == L1SkillId.TELEPORT ) ||
				( _skillId == L1SkillId.MASS_TELEPORT ))
			{
				_bookmarkId = target_id;
			}
			// 対象がアイテムのスキル
			if (( _skillId == L1SkillId.CREATE_MAGICAL_WEAPON ) ||
				( _skillId == L1SkillId.BRING_STONE ) ||
				( _skillId == L1SkillId.BLESSED_ARMOR ) ||
				( _skillId == L1SkillId.ENCHANT_WEAPON ) ||
				( _skillId == L1SkillId.SHADOW_FANG ))
			{
				_itemobjid = target_id;
			}
			_target = (L1Character)l1object;

			if (!( _target is L1MonsterInstance ) && _skill.Target.Equals("attack") && ( _user.Id != target_id ))
			{
				_isPK = true; // ターゲットがモンスター以外で攻撃系スキルで、自分以外の場合PKモードとする。
			}

			// 初期設定ここまで

			// 事前チェック
			if (!( l1object is L1Character ))
			{ // ターゲットがキャラクター以外の場合何もしない。
				checkedResult = false;
			}
			makeTargetList(); // ターゲットの一覧を作成
			if (_targetList.Count == 0 && ( _user is L1NpcInstance ))
			{
				checkedResult = false;
			}
			// 事前チェックここまで
			return checkedResult;
		}

		/// <summary>
		/// 通常のスキル使用時に使用者の状態からスキルが使用可能であるか判断する
		/// </summary>
		/// <returns> false スキルが使用不可能な状態である場合 </returns>
		private bool NormalSkillUsable
		{
			get
			{
				// スキル使用者がPCの場合のチェック
				if (_user is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance)_user;

					if (pc.Teleport)
					{ // 傳送中
						return false;
					}
					if (pc.Paralyzed)
					{ // 麻痺・凍結状態か
						return false;
					}
					if (( pc.Invisble || pc.InvisDelay ) && !InvisUsableSkill)
					{ // 隱身中無法使用技能
						return false;
					}
					if (pc.Inventory.Weight242 >= 197)
					{ // \f1你攜帶太多物品，因此無法使用法術。
						pc.sendPackets(new S_ServerMessage(316));
						return false;
					}
					int polyId = pc.TempCharGfx;
					L1PolyMorph poly = PolyTable.Instance.getTemplate(polyId);
					// 魔法が使えない変身
					if (( poly != null ) && !poly.canUseSkill())
					{
						pc.sendPackets(new S_ServerMessage(285)); // \f1在此狀態下無法使用魔法。
						return false;
					}

					if (!AttrAgrees)
					{ // 精霊魔法で、属性が一致しなければ何もしない。
						return false;
					}

					if (( _skillId == L1SkillId.ELEMENTAL_PROTECTION ) && ( pc.ElfAttr == 0 ))
					{
						pc.sendPackets(new S_ServerMessage(280)); // \f1施咒失敗。
						return false;
					}

					/* 水中無法使用火屬性魔法 */
					if (pc.Map.Underwater && _skill.Attr == 2)
					{
						pc.sendPackets(new S_ServerMessage(280)); // \f1施咒失敗。
						return false;
					}

					// スキルディレイ中使用不可
					if (pc.SkillDelay)
					{
						return false;
					}

					// 魔法封印、封印禁地、卡毒、幻想
					if (pc.hasSkillEffect(L1SkillId.SILENCE) || pc.hasSkillEffect(L1SkillId.AREA_OF_SILENCE) || pc.hasSkillEffect(L1SkillId.STATUS_POISON_SILENCE) || pc.hasSkillEffect(L1SkillId.CONFUSION_ING))
					{
						pc.sendPackets(new S_ServerMessage(285)); // \f1在此狀態下無法使用魔法。
						return false;
					}

					// DIGはロウフルでのみ使用可
					if (( _skillId == L1SkillId.DISINTEGRATE ) && ( pc.Lawful < 500 ))
					{
						// このメッセージであってるか未確認
						pc.sendPackets(new S_ServerMessage(352, "$967")); // 若要使用這個法術，屬性必須成為 (正義)。
						return false;
					}

					// 同じキューブは効果範囲外であれば配置可能
					if (( _skillId == L1SkillId.CUBE_IGNITION ) || ( _skillId == L1SkillId.CUBE_QUAKE ) || ( _skillId == L1SkillId.CUBE_SHOCK ) || ( _skillId == L1SkillId.CUBE_BALANCE ))
					{
						bool isNearSameCube = false;
						int gfxId = 0;
						foreach (GameObject obj in L1World.Instance.getVisibleObjects(pc, 3))
						{
							if (obj is L1EffectInstance)
							{
								L1EffectInstance effect = (L1EffectInstance)obj;
								gfxId = effect.GfxId;
								if (( ( _skillId == L1SkillId.CUBE_IGNITION ) && ( gfxId == 6706 ) ) || ( ( _skillId == L1SkillId.CUBE_QUAKE ) && ( gfxId == 6712 ) ) || ( ( _skillId == L1SkillId.CUBE_SHOCK ) && ( gfxId == 6718 ) ) || ( ( _skillId == L1SkillId.CUBE_BALANCE ) && ( gfxId == 6724 ) ))
								{
									isNearSameCube = true;
									break;
								}
							}
						}
						if (isNearSameCube)
						{
							pc.sendPackets(new S_ServerMessage(1412)); // 已在地板上召喚了魔法立方塊。
							return false;
						}
					}

					// 覺醒狀態 - 非覺醒技能無法使用
					if (( ( pc.AwakeSkillId == L1SkillId.AWAKEN_ANTHARAS ) && ( _skillId != L1SkillId.AWAKEN_ANTHARAS ) ) ||
						( ( pc.AwakeSkillId == L1SkillId.AWAKEN_FAFURION ) && ( _skillId != L1SkillId.AWAKEN_FAFURION ) ) ||
						( ( pc.AwakeSkillId == L1SkillId.AWAKEN_VALAKAS ) && ( _skillId != L1SkillId.AWAKEN_VALAKAS ) ) && ( _skillId != L1SkillId.MAGMA_BREATH ) && ( _skillId != L1SkillId.SHOCK_SKIN ) && ( _skillId != L1SkillId.FREEZING_BREATH ))
					{
						pc.sendPackets(new S_ServerMessage(1385)); // 目前狀態中無法使用覺醒魔法。
						return false;
					}

					if (( ItemConsume == false ) && !_player.Gm)
					{ // 法術消耗道具判斷。
						_player.sendPackets(new S_ServerMessage(299)); // \f1施放魔法所需材料不足。
						return false;
					}
				}
				// スキル使用者がNPCの場合のチェック
				else if (_user is L1NpcInstance)
				{

					// サイレンス状態では使用不可
					if (_user.hasSkillEffect(L1SkillId.SILENCE))
					{
						// NPCにサイレンスが掛かっている場合は1回だけ使用をキャンセルさせる効果。
						_user.removeSkillEffect(L1SkillId.SILENCE);
						return false;
					}
				}

				// PC、NPC共通檢查HP、MP是否足夠
				if (!HPMPConsume)
				{ // 花費的HP、MP計算
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// スペルスクロール使用時に使用者の状態からスキルが使用可能であるか判断する
		/// </summary>
		/// <returns> false スキルが使用不可能な状態である場合 </returns>
		private bool SpellScrollUsable
		{
			get
			{
				// スペルスクロールを使用するのはPCのみ
				L1PcInstance pc = (L1PcInstance)_user;

				if (pc.Teleport)
				{ // 傳送中
					return false;
				}

				if (pc.Paralyzed)
				{ // 麻痺・凍結状態か
					return false;
				}

				// インビジ中に使用不可のスキル
				if (( pc.Invisble || pc.InvisDelay ) && !InvisUsableSkill)
				{
					return false;
				}

				return true;
			}
		}

		// インビジ中に使用可能なスキルかを返す
		private bool InvisUsableSkill
		{
			get
			{
				foreach (int skillId in CAST_WITH_INVIS)
				{
					if (skillId == _skillId)
					{
						return true;
					}
				}
				return false;
			}
		}

		public virtual void handleCommands(L1PcInstance player, int skillId, int targetId, int x, int y, string message, int timeSecs, int type)
		{
			L1Character attacker = null;
			handleCommands(player, skillId, targetId, x, y, message, timeSecs, type, attacker);
		}

		public virtual void handleCommands(L1PcInstance player, int skillId, int targetId, int x, int y, string message, int timeSecs, int type, L1Character attacker)
		{

			try
			{
				// 事前チェックをしているか？
				if (!CheckedUseSkill)
				{
					bool isUseSkill = checkUseSkill(player, skillId, targetId, x, y, message, timeSecs, type, attacker);

					if (!isUseSkill)
					{
						failSkill();
						return;
					}
				}

				if (type == TYPE_NORMAL)
				{ // 魔法詠唱時
					if (!_isGlanceCheckFail || ( SkillArea > 0 ) || _skill.Target.Equals("none"))
					{
						runSkill();
						useConsume();
						sendGrfx(true);
						sendFailMessageHandle();
						setDelay();
					}
				}
				else if (type == TYPE_LOGIN)
				{ // ログイン時（HPMP材料消費なし、グラフィックなし）
					runSkill();
				}
				else if (type == TYPE_SPELLSC)
				{ // スペルスクロール使用時（HPMP材料消費なし）
					runSkill();
					sendGrfx(true);
				}
				else if (type == TYPE_GMBUFF)
				{ // GMBUFF使用時（HPMP材料消費なし、魔法モーションなし）
					runSkill();
					sendGrfx(false);
				}
				else if (type == TYPE_NPCBUFF)
				{ // NPCBUFF使用時（HPMP材料消費なし）
					runSkill();
					sendGrfx(true);
				}
				CheckedUseSkill = false;
			}
			catch (Exception e)
			{
				_log.Error(Enum.Level.Server, "", e);
			}
		}

		/// <summary>
		/// スキルの失敗処理(PCのみ）
		/// </summary>
		private void failSkill()
		{
			// HPが足りなくてスキルが使用できない場合のみ、MPのみ消費したいが未実装（必要ない？）
			// その他の場合は何も消費されない。
			// useConsume(); // HP、MPは減らす
			CheckedUseSkill = false;
			// テレポートスキル
			if (( _skillId == L1SkillId.TELEPORT ) || ( _skillId == L1SkillId.MASS_TELEPORT ) || ( _skillId == L1SkillId.TELEPORT_TO_MATHER ))
			{
				// テレポートできない場合でも、クライアント側は応答を待っている
				// テレポート待ち状態の解除（第2引数に意味はない）
				_player.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, false));
			}
		}

		// ターゲットか？
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private boolean isTarget(l1j.server.server.model.L1Character cha) throws Exception
		private bool isTarget(L1Character cha)
		{
			bool _flg = false;

			if (cha is L1PcInstance)
			{
				L1PcInstance pc = (L1PcInstance)cha;
				if (pc.Ghost || pc.GmInvis)
				{
					return false;
				}
			}
			if (( _calcType == NPC_PC ) && ( ( cha is L1PcInstance ) || ( cha is L1PetInstance ) || ( cha is L1SummonInstance ) ))
			{
				_flg = true;
			}

			// 破壊不可能なドアは対象外
			if (cha is L1DoorInstance)
			{
				if (( cha.getMaxHp() == 0 ) || ( cha.getMaxHp() == 1 ))
				{
					return false;
				}
			}

			// マジックドールは対象外
			if (( cha is L1DollInstance ) && ( _skillId != L1SkillId.HASTE ))
			{
				return false;
			}

			// 元のターゲットがPet、Summon以外のNPCの場合、PC、Pet、Summonは対象外
			if (( _calcType == PC_NPC ) && ( _target is L1NpcInstance ) && !( _target is L1PetInstance ) && !( _target is L1SummonInstance ) && ( ( cha is L1PetInstance ) ||
				( cha is L1SummonInstance ) || ( cha is L1PcInstance ) ))
			{
				return false;
			}

			// 元のターゲットがガード以外のNPCの場合、ガードは対象外
			if (( _calcType == PC_NPC ) && ( _target is L1NpcInstance ) && !( _target is L1GuardInstance ) && ( cha is L1GuardInstance ))
			{
				return false;
			}

			// NPC対PCでターゲットがモンスターの場合ターゲットではない。
			if (( _skill.Target.Equals("attack") ||
				( _skill.Type == L1Skills.TYPE_ATTACK ) ) && ( _calcType == NPC_PC ) && !( cha is L1PetInstance ) && !( cha is L1SummonInstance ) && !( cha is L1PcInstance ))
			{
				return false;
			}

			// NPC対NPCで使用者がMOBで、ターゲットがMOBの場合ターゲットではない。
			if (( _skill.Target.Equals("attack") ||
				( _skill.Type == L1Skills.TYPE_ATTACK ) ) && ( _calcType == NPC_NPC ) && ( _user is L1MonsterInstance ) && ( cha is L1MonsterInstance ))
			{
				return false;
			}

			// 無方向範囲攻撃魔法で攻撃できないNPCは対象外
			if (_skill.Target.Equals("none") && ( _skill.Type == L1Skills.TYPE_ATTACK ) && ( ( cha is L1AuctionBoardInstance ) || ( cha is L1BoardInstance ) || ( cha is L1CrownInstance ) || ( cha is L1DwarfInstance ) || ( cha is L1EffectInstance ) || ( cha is L1FieldObjectInstance ) || ( cha is L1FurnitureInstance ) || ( cha is L1HousekeeperInstance ) || ( cha is L1MerchantInstance ) || ( cha is L1TeleporterInstance ) ))
			{
				return false;
			}

			// 攻擊型魔法無法攻擊自己
			if (( _skill.Type == L1Skills.TYPE_ATTACK ) && ( cha.Id == _user.Id ))
			{
				return false;
			}

			// 體力回復術判斷施法者不補血
			if (( cha.Id == _user.Id ) && ( _skillId == L1SkillId.HEAL_ALL ))
			{
				return false;
			}

			if (( ( ( _skill.TargetTo & L1Skills.TARGET_TO_PC ) == L1Skills.TARGET_TO_PC ) || ( ( _skill.TargetTo & L1Skills.TARGET_TO_CLAN ) == L1Skills.TARGET_TO_CLAN ) || ( ( _skill.TargetTo & L1Skills.TARGET_TO_PARTY ) == L1Skills.TARGET_TO_PARTY ) ) && ( cha.Id == _user.Id ) && ( _skillId != L1SkillId.HEAL_ALL ))
			{
				return true; // ターゲットがパーティーかクラン員のものは自分に効果がある。（ただし、ヒールオールは除外）
			}

			// スキル使用者がPCで、PKモードではない場合、自分のサモン・ペットは対象外
			if (( _user is L1PcInstance ) && ( _skill.Target.Equals("attack") || ( _skill.Type == L1Skills.TYPE_ATTACK ) ) && ( _isPK == false ))
			{
				if (cha is L1SummonInstance)
				{
					L1SummonInstance summon = (L1SummonInstance)cha;
					if (_player.Id == summon.Master.Id)
					{
						return false;
					}
				}
				else if (cha is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance)cha;
					if (_player.Id == pet.Master.Id)
					{
						return false;
					}
				}
			}

			if (( _skill.Target.Equals("attack") || ( _skill.Type == L1Skills.TYPE_ATTACK ) ) && !( cha is L1MonsterInstance ) && ( _isPK == false ) && ( _target is L1PcInstance ))
			{
				L1PcInstance enemy = (L1PcInstance)cha;
				// カウンターディテクション
				if (( _skillId == L1SkillId.COUNTER_DETECTION ) && ( enemy.ZoneType != 1 ) && ( cha.hasSkillEffect(L1SkillId.INVISIBILITY) || cha.hasSkillEffect(L1SkillId.BLIND_HIDING) ))
				{
					return true; // インビジかブラインドハイディング中
				}
				if (( _player.Clanid != 0 ) && ( enemy.Clanid != 0 ))
				{ // クラン所属中
				  // 全戦争リストを取得
					foreach (L1War war in L1World.Instance.WarList)
					{
						if (war.CheckClanInWar(_player.Clanname))
						{ // 自クランが戦争に参加中
							if (war.CheckClanInSameWar(_player.Clanname, enemy.Clanname))
							{
								if (L1CastleLocation.checkInAllWarArea(enemy.X, enemy.Y, enemy.MapId))
								{
									return true;
								}
							}
						}
					}
				}
				return false; // 攻撃スキルでPKモードじゃない場合
			}

			if (( _user.glanceCheck(cha.X, cha.Y) == false ) && ( _skill.Through == false ))
			{
				// エンチャント、復活スキルは障害物の判定をしない
				if (!( ( _skill.Type == L1Skills.TYPE_CHANGE ) || ( _skill.Type == L1Skills.TYPE_RESTORE ) ))
				{
					_isGlanceCheckFail = true;
					return false; // 直線上に障害物がある
				}
			}

			if (cha.hasSkillEffect(L1SkillId.ICE_LANCE) || cha.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD) || cha.hasSkillEffect(L1SkillId.FREEZING_BREATH) || cha.hasSkillEffect(L1SkillId.ICE_LANCE_COCKATRICE) || cha.hasSkillEffect(L1SkillId.ICE_LANCE_BASILISK))
			{
				if (_skillId == L1SkillId.ICE_LANCE || _skillId == L1SkillId.FREEZING_BLIZZARD || _skillId == L1SkillId.FREEZING_BREATH || _skillId == L1SkillId.ICE_LANCE_COCKATRICE || _skillId == L1SkillId.ICE_LANCE_BASILISK)
				{
					return false;
				}
			}
			/*
					if (cha.hasSkillEffect(L1SkillId.ICE_LANCE) && ((_skillId == L1SkillId.ICE_LANCE) || (_skillId == L1SkillId.FREEZING_BLIZZARD) || (_skillId == L1SkillId.FREEZING_BREATH))) {
						return false; // アイスランス中にアイスランス、フリージングブリザード、フリージングブレス
					}

					if (cha.hasSkillEffect(L1SkillId.FREEZING_BLIZZARD) && ((_skillId == L1SkillId.ICE_LANCE) || (_skillId == L1SkillId.FREEZING_BLIZZARD) || (_skillId == L1SkillId.FREEZING_BREATH))) {
						return false; // フリージングブリザード中にアイスランス、フリージングブリザード、フリージングブレス
					}

					if (cha.hasSkillEffect(L1SkillId.FREEZING_BREATH) && ((_skillId == L1SkillId.ICE_LANCE) || (_skillId == L1SkillId.FREEZING_BLIZZARD) || (_skillId == L1SkillId.FREEZING_BREATH))) {
						return false; // フリージングブレス中にアイスランス、フリージングブリザード、フリージングブレス
					}
			*/
			if (cha.hasSkillEffect(L1SkillId.EARTH_BIND) && ( _skillId == L1SkillId.EARTH_BIND ))
			{
				return false; // アース バインド中にアース バインド
			}

			if (!( cha is L1MonsterInstance ) && ( ( _skillId == L1SkillId.TAMING_MONSTER ) || ( _skillId == L1SkillId.CREATE_ZOMBIE ) ))
			{
				return false; // ターゲットがモンスターじゃない（テイミングモンスター）
			}
			if (cha.Dead && ( ( _skillId != L1SkillId.CREATE_ZOMBIE ) && ( _skillId != L1SkillId.RESURRECTION ) && ( _skillId != L1SkillId.GREATER_RESURRECTION ) && ( _skillId != L1SkillId.CALL_OF_NATURE ) ))
			{
				return false; // 目標已死亡 法術非復活類
			}

			if (( cha.Dead == false ) && ( ( _skillId == L1SkillId.CREATE_ZOMBIE ) || ( _skillId == L1SkillId.RESURRECTION ) || ( _skillId == L1SkillId.GREATER_RESURRECTION ) || ( _skillId == L1SkillId.CALL_OF_NATURE ) ))
			{
				return false; // 目標未死亡 法術復活類
			}

			if (( ( cha is L1TowerInstance ) || ( cha is L1DoorInstance ) ) && ( ( _skillId == L1SkillId.CREATE_ZOMBIE ) || ( _skillId == L1SkillId.RESURRECTION ) || ( _skillId == L1SkillId.GREATER_RESURRECTION ) || ( _skillId == L1SkillId.CALL_OF_NATURE ) ))
			{
				return false; // 塔跟門不可放復活法術
			}

			if (cha is L1PcInstance)
			{
				L1PcInstance pc = (L1PcInstance)cha;
				if (pc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
				{ // 絕對屏障狀態中
					if (( _skillId == L1SkillId.CURSE_BLIND ) || ( _skillId == L1SkillId.WEAPON_BREAK ) || ( _skillId == L1SkillId.DARKNESS ) || ( _skillId == L1SkillId.WEAKNESS ) || ( _skillId == L1SkillId.DISEASE ) || ( _skillId == L1SkillId.FOG_OF_SLEEPING ) || ( _skillId == L1SkillId.MASS_SLOW ) || ( _skillId == L1SkillId.SLOW ) || ( _skillId == L1SkillId.CANCELLATION ) || ( _skillId == L1SkillId.SILENCE ) || ( _skillId == L1SkillId.DECAY_POTION ) || ( _skillId == L1SkillId.MASS_TELEPORT ) || ( _skillId == L1SkillId.DETECTION ) || ( _skillId == L1SkillId.COUNTER_DETECTION ) || ( _skillId == L1SkillId.ERASE_MAGIC ) || ( _skillId == L1SkillId.ENTANGLE ) || ( _skillId == L1SkillId.PHYSICAL_ENCHANT_DEX ) || ( _skillId == L1SkillId.PHYSICAL_ENCHANT_STR ) || ( _skillId == L1SkillId.BLESS_WEAPON ) || ( _skillId == L1SkillId.EARTH_SKIN ) || ( _skillId == L1SkillId.IMMUNE_TO_HARM ) || ( _skillId == L1SkillId.REMOVE_CURSE ))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}

			if (cha is L1NpcInstance)
			{
				int hiddenStatus = ( (L1NpcInstance)cha ).HiddenStatus;
				if (hiddenStatus == L1NpcInstance.HIDDEN_STATUS_SINK)
				{
					if (( _skillId == L1SkillId.DETECTION ) || ( _skillId == L1SkillId.COUNTER_DETECTION ))
					{ // ディテク、Cディテク
						return true;
					}
					else
					{
						return false;
					}
				}
				else if (hiddenStatus == L1NpcInstance.HIDDEN_STATUS_FLY)
				{
					return false;
				}
			}

			if (( ( _skill.TargetTo & L1Skills.TARGET_TO_PC ) == L1Skills.TARGET_TO_PC ) && ( cha is L1PcInstance ))
			{
				_flg = true;
			}
			else if (( ( _skill.TargetTo & L1Skills.TARGET_TO_NPC ) == L1Skills.TARGET_TO_NPC ) && ( ( cha is L1MonsterInstance ) || ( cha is L1NpcInstance ) || ( cha is L1SummonInstance ) || ( cha is L1PetInstance ) ))
			{
				_flg = true;
			}
			else if (( ( _skill.TargetTo & L1Skills.TARGET_TO_PET ) == L1Skills.TARGET_TO_PET ) && ( _user is L1PcInstance ))
			{ // ターゲットがSummon,Pet
				if (cha is L1SummonInstance)
				{
					L1SummonInstance summon = (L1SummonInstance)cha;
					if (summon.Master != null)
					{
						if (_player.Id == summon.Master.Id)
						{
							_flg = true;
						}
					}
				}
				else if (cha is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance)cha;
					if (pet.Master != null)
					{
						if (_player.Id == pet.Master.Id)
						{
							_flg = true;
						}
					}
				}
			}

			if (( _calcType == PC_PC ) && ( cha is L1PcInstance ))
			{
				if (( ( _skill.TargetTo & L1Skills.TARGET_TO_CLAN ) == L1Skills.TARGET_TO_CLAN ) && ( ( ( _player.Clanid != 0 ) && ( _player.Clanid == ( (L1PcInstance)cha ).Clanid ) ) || _player.Gm ))
				{
					return true;
				}
				if (( ( _skill.TargetTo & L1Skills.TARGET_TO_PARTY ) == L1Skills.TARGET_TO_PARTY ) && ( _player.Party.isMember((L1PcInstance)cha) || _player.Gm ))
				{
					return true;
				}
			}

			return _flg;
		}

		// ターゲットの一覧を作成
		private void makeTargetList()
		{
			try
			{
				if (_type == TYPE_LOGIN)
				{ // ログイン時(死亡時、お化け屋敷のキャンセレーション含む)は使用者のみ
					_targetList.Add(new TargetStatus(_user));
					return;
				}
				if (( _skill.TargetTo == L1Skills.TARGET_TO_ME ) && ( ( _skill.Type & L1Skills.TYPE_ATTACK ) != L1Skills.TYPE_ATTACK ))
				{
					_targetList.Add(new TargetStatus(_user)); // ターゲットは使用者のみ
					return;
				}

				// 射程距離-1の場合は画面内のオブジェクトが対象
				if (SkillRanged != -1)
				{
					if (_user.Location.getTileLineDistance(_target.Location) > SkillRanged)
					{
						return; // 射程範囲外
					}
				}
				else
				{
					if (!_user.Location.isInScreen(_target.Location))
					{
						return; // 射程範囲外
					}
				}

				if (( isTarget(_target) == false ) && !( _skill.Target.Equals("none") ))
				{
					// 対象が違うのでスキルが発動しない。
					return;
				}

				if (( _skillId == L1SkillId.LIGHTNING ) || ( _skillId == L1SkillId.FREEZING_BREATH ))
				{ // ライトニング、フリージングブレス直線的に範囲を決める
					IList<GameObject> al1object = L1World.Instance.getVisibleLineObjects(_user, _target);

					foreach (GameObject tgobj in al1object)
					{
						if (tgobj == null)
						{
							continue;
						}
						if (!( tgobj is L1Character ))
						{ // ターゲットがキャラクター以外の場合何もしない。
							continue;
						}
						L1Character cha = (L1Character)tgobj;
						if (isTarget(cha) == false)
						{
							continue;
						}
						_targetList.Add(new TargetStatus(cha));
					}
					return;
				}
				if (_player.AccessLevel >= 200 && _skillId == L1SkillId.ENERGY_BOLT)
				{ // ライトニング、フリージングブレス直線的に範囲を決める
					IList<GameObject> al1object = L1World.Instance.getVisibleObjects(_user, 10);

					foreach (GameObject tgobj in al1object)
					{
						if (tgobj == null)
						{
							continue;
						}
						if (!( tgobj is L1Character ))
						{ // ターゲットがキャラクター以外の場合何もしない。
							continue;
						}
						L1Character cha = (L1Character)tgobj;
						if (isTarget(cha) == false)
						{
							continue;
						}
						_targetList.Add(new TargetStatus(cha));
					}
					return;
				}
				if (SkillArea == 0)
				{ // 単体の場合
					if (!_user.glanceCheck(_target.X, _target.Y))
					{ // 直線上に障害物があるか
						if (( ( _skill.Type & L1Skills.TYPE_ATTACK ) == L1Skills.TYPE_ATTACK ) && ( _skillId != 10026 ) && ( _skillId != 10027 ) && ( _skillId != 10028 ) && ( _skillId != 10029 ))
						{ // 安息攻撃以外の攻撃スキル
							_targetList.Add(new TargetStatus(_target, false)); // ダメージも発生しないし、ダメージモーションも発生しないが、スキルは発動
							return;
						}
					}
					_targetList.Add(new TargetStatus(_target));
				}
				else
				{ // 範囲の場合
					if (!_skill.Target.Equals("none"))
					{
						_targetList.Add(new TargetStatus(_target));
					}

					if (( _skillId != L1SkillId.HEAL_ALL ) && !( _skill.Target.Equals("attack") || ( _skill.Type == L1Skills.TYPE_ATTACK ) ))
					{
						// 攻撃系以外のスキルとH-A以外はターゲット自身を含める
						_targetList.Add(new TargetStatus(_user));
					}

					IList<GameObject> objects;
					if (SkillArea == -1)
					{
						objects = L1World.Instance.getVisibleObjects(_user);
					}
					else
					{
						objects = L1World.Instance.getVisibleObjects(_target, SkillArea);
					}
					foreach (GameObject tgobj in objects)
					{
						if (tgobj == null)
						{
							continue;
						}
						if (!( tgobj is L1Character ))
						{ // ターゲットがキャラクター以外の場合何もしない。
							continue;
						}
						L1Character cha = (L1Character)tgobj;
						if (!isTarget(cha))
						{
							continue;
						}

						_targetList.Add(new TargetStatus(cha));
					}
					return;
				}

			}
			catch (Exception e)
			{
				_log.Error(Level.FINEST, "exception in L1Skilluse makeTargetList{0}", e);
			}
		}

		// メッセージの表示（何か起こったとき）
		private void sendHappenMessage(L1PcInstance pc)
		{
			int msgID = _skill.SysmsgIdHappen;
			if (msgID > 0)
			{
				// 效果訊息排除施法者本身。
				if (_skillId == L1SkillId.AREA_OF_SILENCE && _user.Id == pc.Id)
				{ // 封印禁地
					return;
				}
				pc.sendPackets(new S_ServerMessage(msgID));
			}
		}

		// 失敗メッセージ表示のハンドル
		private void sendFailMessageHandle()
		{
			// 攻撃スキル以外で対象を指定するスキルが失敗した場合は失敗したメッセージをクライアントに送信
			// ※攻撃スキルは障害物があっても成功時と同じアクションであるべき。
			if (( _skill.Type != L1Skills.TYPE_ATTACK ) && !_skill.Target.Equals("none") && _targetList.Count == 0)
			{
				sendFailMessage();
			}
		}

		// メッセージの表示（失敗したとき）
		private void sendFailMessage()
		{
			int msgID = _skill.SysmsgIdFail;
			if (( msgID > 0 ) && ( _user is L1PcInstance ))
			{
				_player.sendPackets(new S_ServerMessage(msgID));
			}
		}

		// 精霊魔法の属性と使用者の属性は一致するか？（とりあえずの対処なので、対応できたら消去して下さい)
		private bool AttrAgrees
		{
			get
			{
				int magicattr = _skill.Attr;
				if (_user is L1NpcInstance)
				{ // NPCが使った場合なんでもOK
					return true;
				}

				if (( _skill.SkillLevel >= 17 ) && ( _skill.SkillLevel <= 22 ) && ( magicattr != 0 ) && ( magicattr != _player.ElfAttr ) && !_player.Gm)
				{ // ただしGMは例外
					return false;
				}
				return true;
			}
		}

		// 必要ＨＰ、ＭＰがあるか？
		private bool HPMPConsume
		{
			get
			{
				if (_mpConsume == 0)
				{
					_mpConsume = _skill.MpConsume;
				}
				_hpConsume = _skill.HpConsume;
				int currentMp = 0;
				int currentHp = 0;

				if (_user is L1NpcInstance)
				{
					currentMp = _npc.CurrentMp;
					currentHp = _npc.CurrentHp;
				}
				else
				{
					currentMp = _player.CurrentMp;
					currentHp = _player.CurrentHp;

					// MPのINT軽減
					if (( _player.Int > 12 ) && ( _skillId > L1SkillId.HOLY_WEAPON ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV2以上
						_mpConsume--;
					}
					if (( _player.Int > 13 ) && ( _skillId > L1SkillId.STALAC ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV3以上
						_mpConsume--;
					}
					if (( _player.Int > 14 ) && ( _skillId > L1SkillId.WEAK_ELEMENTAL ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV4以上
						_mpConsume--;
					}
					if (( _player.Int > 15 ) && ( _skillId > L1SkillId.MEDITATION ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV5以上
						_mpConsume--;
					}
					if (( _player.Int > 16 ) && ( _skillId > L1SkillId.DARKNESS ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV6以上
						_mpConsume--;
					}
					if (( _player.Int > 17 ) && ( _skillId > L1SkillId.BLESS_WEAPON ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV7以上
						_mpConsume--;
					}
					if (( _player.Int > 18 ) && ( _skillId > L1SkillId.DISEASE ) && ( _skillId <= L1SkillId.FREEZING_BLIZZARD ))
					{ // LV8以上
						_mpConsume--;
					}

					// 騎士智力減免
					if (( _player.Int > 12 ) && ( _skillId >= L1SkillId.SHOCK_STUN ) && ( _skillId <= L1SkillId.COUNTER_BARRIER ))
					{
						if (_player.Int <= 17)
						{
							_mpConsume -= ( _player.Int - 12 );
						}
						else
						{
							_mpConsume -= 5; // int > 18
							if (_mpConsume > 1)
							{ // 法術還可以減免
								sbyte extraInt = (sbyte)( _player.Int - 17 );
								// 減免公式
								for (int first = 1, range = 2; first <= extraInt; first += range, range++)
								{
									_mpConsume--;
								}
							}
						}

					}

					// 裝備MP減免 一次只需判斷一個 
					if (( _skillId == L1SkillId.PHYSICAL_ENCHANT_DEX ) && _player.Inventory.checkEquipped(20013))
					{ // 敏捷魔法頭盔使用通暢氣脈術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.HASTE ) && _player.Inventory.checkEquipped(20013))
					{ // 敏捷魔法頭盔使用加速術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.HEAL ) && _player.Inventory.checkEquipped(20014))
					{ // 治癒魔法頭盔使用初級治癒術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.EXTRA_HEAL ) && _player.Inventory.checkEquipped(20014))
					{ // 治癒魔法頭盔使用中級治癒術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.ENCHANT_WEAPON ) && _player.Inventory.checkEquipped(20015))
					{ // 力量魔法頭盔使用擬似魔法武器
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.DETECTION ) && _player.Inventory.checkEquipped(20015))
					{ // 力量魔法頭盔使用無所遁形術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.PHYSICAL_ENCHANT_STR ) && _player.Inventory.checkEquipped(20015))
					{ // 力量魔法頭盔使用體魄強健術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.HASTE ) && _player.Inventory.checkEquipped(20008))
					{ // 小型風之頭盔使用加速術
						_mpConsume /= 2;
					}
					else if (( _skillId == L1SkillId.HASTE ) && _player.Inventory.checkEquipped(20023))
					{ // 風之頭盔使用加速術
						_mpConsume = 25;
					}
					else if (( _skillId == L1SkillId.GREATER_HASTE ) && _player.Inventory.checkEquipped(20023))
					{ // 風之頭盔使用強力加速術
						_mpConsume /= 2;
					}

					// 初始能力減免
					if (_player.OriginalMagicConsumeReduction > 0)
					{
						_mpConsume -= _player.OriginalMagicConsumeReduction;
					}

					if (0 < _skill.MpConsume)
					{
						_mpConsume = Math.Max(_mpConsume, 1); // 最小值 1
					}
				}

				if (currentHp < _hpConsume + 1)
				{
					if (_user is L1PcInstance)
					{
						_player.sendPackets(new S_ServerMessage(279)); // \f1因體力不足而無法使用魔法。
					}
					return false;
				}
				else if (currentMp < _mpConsume)
				{
					if (_user is L1PcInstance)
					{
						_player.sendPackets(new S_ServerMessage(278)); // \f1因魔力不足而無法使用魔法。
					}
					return false;
				}

				return true;
			}
		}

		// 必要材料があるか？
		private bool ItemConsume
		{
			get
			{

				int itemConsume = _skill.ItemConsumeId;
				int itemConsumeCount = _skill.ItemConsumeCount;

				if (itemConsume == 0)
				{
					return true; // 材料を必要としない魔法
				}

				if (!_player.Inventory.checkItem(itemConsume, itemConsumeCount))
				{
					return false; // 必要材料が足りなかった。
				}

				return true;
			}
		}

		// 使用材料、HP・MP、Lawfulをマイナスする。
		private void useConsume()
		{
			if (_user is L1NpcInstance)
			{
				// NPCの場合、HP、MPのみマイナス
				int current_hp = _npc.CurrentHp - _hpConsume;
				_npc.CurrentHp = current_hp;

				int current_mp = _npc.CurrentMp - _mpConsume;
				_npc.CurrentMp = current_mp;
				return;
			}

			// HP・MP花費 已經計算使用量
			if (_skillId == L1SkillId.FINAL_BURN)
			{ // 會心一擊
				_player.CurrentHp = 1;
				_player.CurrentMp = 0;
			}
			else
			{
				int current_hp = _player.CurrentHp - _hpConsume;
				_player.CurrentHp = current_hp;

				int current_mp = _player.CurrentMp - _mpConsume;
				_player.CurrentMp = current_mp;
			}

			// Lawfulをマイナス
			int lawful = _player.Lawful + _skill.Lawful;
			if (lawful > 32767)
			{
				lawful = 32767;
			}
			else if (lawful < -32767)
			{
				lawful = -32767;
			}
			_player.Lawful = lawful;

			int itemConsume = _skill.ItemConsumeId;
			int itemConsumeCount = _skill.ItemConsumeCount;

			if (itemConsume == 0)
			{
				return; // 材料を必要としない魔法
			}

			// 使用材料をマイナス
			_player.Inventory.consumeItem(itemConsume, itemConsumeCount);
		}

		// マジックリストに追加する。
		private void addMagicList(L1Character cha, bool repetition)
		{
			if (_skillTime == 0)
			{
				_getBuffDuration = _skill.BuffDuration * 1000; // 効果時間
				if (_skill.BuffDuration == 0)
				{
					if (_skillId == L1SkillId.INVISIBILITY)
					{ // インビジビリティ
						cha.setSkillEffect(L1SkillId.INVISIBILITY, 0);
					}
					return;
				}
			}
			else
			{
				_getBuffDuration = _skillTime * 1000; // パラメータのtimeが0以外なら、効果時間として設定する
			}

			if (_skillId == L1SkillId.SHOCK_STUN)
			{
				_getBuffDuration = _shockStunDuration;
			}

			if (_skillId == L1SkillId.CURSE_POISON)
			{ // 毒咒持續時間移至 L1Poison 處理。
				return;
			}
			if (( _skillId == L1SkillId.CURSE_PARALYZE ) || ( _skillId == L1SkillId.CURSE_PARALYZE2 ))
			{ // 木乃伊的咀咒、石化持續時間移至 L1CurseParalysis 處理。
				return;
			}
			if (_skillId == L1SkillId.SHAPE_CHANGE)
			{ // 變形術持續時間移至 L1PolyMorph 處理。
				return;
			}
			if (( _skillId == L1SkillId.BLESSED_ARMOR ) || ( _skillId == L1SkillId.HOLY_WEAPON ) || ( _skillId == L1SkillId.ENCHANT_WEAPON ) || ( _skillId == L1SkillId.BLESS_WEAPON ) || ( _skillId == L1SkillId.SHADOW_FANG ))
			{
				return;
			}
			if (( ( _skillId == L1SkillId.ICE_LANCE ) || ( _skillId == L1SkillId.FREEZING_BLIZZARD ) || ( _skillId == L1SkillId.FREEZING_BREATH ) || ( _skillId == L1SkillId.ICE_LANCE_COCKATRICE ) || ( _skillId == L1SkillId.ICE_LANCE_BASILISK ) ) && !_isFreeze)
			{ // 凍結失敗
				return;
			}
			if (( _skillId == L1SkillId.AWAKEN_ANTHARAS ) || ( _skillId == L1SkillId.AWAKEN_FAFURION ) || ( _skillId == L1SkillId.AWAKEN_VALAKAS ))
			{ // 覚醒の効果処理はL1Awakeに移譲。
				return;
			}
			// 骷髏毀壞持續時間另外處理
			if (_skillId == L1SkillId.BONE_BREAK || _skillId == L1SkillId.CONFUSION)
			{
				return;
			}
			cha.setSkillEffect(L1SkillId._skillId, _getBuffDuration);

			if (_skillId == L1SkillId.ELEMENTAL_FALL_DOWN && repetition)
			{ // 弱化屬性重複施放
				if (_skillTime == 0)
				{
					_getBuffIconDuration = _skill.BuffDuration; // 効果時間
				}
				else
				{
					_getBuffIconDuration = _skillTime;
				}
				_target.removeSkillEffect(L1SkillId.ELEMENTAL_FALL_DOWN);
				runSkill();
				return;
			}
			if (( cha is L1PcInstance ) && repetition)
			{ // 対象がPCで既にスキルが重複している場合
				L1PcInstance pc = (L1PcInstance)cha;
				sendIcon(pc);
			}
		}

		// アイコンの送信
		private void sendIcon(L1PcInstance pc)
		{
			if (_skillTime == 0)
			{
				_getBuffIconDuration = _skill.BuffDuration; // 効果時間
			}
			else
			{
				_getBuffIconDuration = _skillTime; // パラメータのtimeが0以外なら、効果時間として設定する
			}

			if (_skillId == L1SkillId.SHIELD)
			{ // シールド
				pc.sendPackets(new S_SkillIconShield(2, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.SHADOW_ARMOR)
			{ // シャドウ アーマー
				pc.sendPackets(new S_SkillIconShield(3, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.DRESS_DEXTERITY)
			{ // ドレス デクスタリティー
				pc.sendPackets(new S_Dexup(pc, 2, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.DRESS_MIGHTY)
			{ // ドレス マイティー
				pc.sendPackets(new S_Strup(pc, 2, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.GLOWING_AURA)
			{ // グローウィング オーラ
				pc.sendPackets(new S_SkillIconAura(113, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.SHINING_AURA)
			{ // シャイニング オーラ
				pc.sendPackets(new S_SkillIconAura(114, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.BRAVE_AURA)
			{ // ブレイブ オーラ
				pc.sendPackets(new S_SkillIconAura(116, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.FIRE_WEAPON)
			{ // ファイアー ウェポン
				pc.sendPackets(new S_SkillIconAura(147, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.WIND_SHOT)
			{ // ウィンド ショット
				pc.sendPackets(new S_SkillIconAura(148, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.FIRE_BLESS)
			{ // ファイアー ブレス
				pc.sendPackets(new S_SkillIconAura(154, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.STORM_EYE)
			{ // ストーム アイ
				pc.sendPackets(new S_SkillIconAura(155, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.EARTH_BLESS)
			{ // アース ブレス
				pc.sendPackets(new S_SkillIconShield(7, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.BURNING_WEAPON)
			{ // バーニング ウェポン
				pc.sendPackets(new S_SkillIconAura(162, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.STORM_SHOT)
			{ // ストーム ショット
				pc.sendPackets(new S_SkillIconAura(165, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.IRON_SKIN)
			{ // アイアン スキン
				pc.sendPackets(new S_SkillIconShield(10, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.EARTH_SKIN)
			{ // アース スキン
				pc.sendPackets(new S_SkillIconShield(6, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.PHYSICAL_ENCHANT_STR)
			{ // フィジカル エンチャント：STR
				pc.sendPackets(new S_Strup(pc, 5, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.PHYSICAL_ENCHANT_DEX)
			{ // フィジカル エンチャント：DEX
				pc.sendPackets(new S_Dexup(pc, 5, _getBuffIconDuration));
			}
			else if (( _skillId == L1SkillId.HASTE ) || ( _skillId == L1SkillId.GREATER_HASTE ))
			{ // グレーターヘイスト
				pc.sendPackets(new S_SkillHaste(pc.Id, 1, _getBuffIconDuration));
				pc.broadcastPacket(new S_SkillHaste(pc.Id, 1, 0));
			}
			else if (( _skillId == L1SkillId.HOLY_WALK ) || ( _skillId == L1SkillId.MOVING_ACCELERATION ) || ( _skillId == L1SkillId.WIND_WALK ))
			{ // ホーリーウォーク、ムービングアクセレーション、ウィンドウォーク
				pc.sendPackets(new S_SkillBrave(pc.Id, 4, _getBuffIconDuration));
				pc.broadcastPacket(new S_SkillBrave(pc.Id, 4, 0));
			}
			else if (_skillId == L1SkillId.BLOODLUST)
			{ // ブラッドラスト
				pc.sendPackets(new S_SkillBrave(pc.Id, 6, _getBuffIconDuration));
				pc.broadcastPacket(new S_SkillBrave(pc.Id, 6, 0));
			}
			else if (( _skillId == L1SkillId.SLOW ) || ( _skillId == L1SkillId.MASS_SLOW ) || ( _skillId == L1SkillId.ENTANGLE ))
			{ // スロー、エンタングル、マススロー
				pc.sendPackets(new S_SkillHaste(pc.Id, 2, _getBuffIconDuration));
				pc.broadcastPacket(new S_SkillHaste(pc.Id, 2, 0));
			}
			else if (_skillId == L1SkillId.IMMUNE_TO_HARM)
			{
				pc.sendPackets(new S_SkillIconGFX(40, _getBuffIconDuration));
			}
			else if (_skillId == L1SkillId.WIND_SHACKLE)
			{ // 風之枷鎖
				pc.sendPackets(new S_SkillIconWindShackle(pc.Id, _getBuffIconDuration));
				pc.broadcastPacket(new S_SkillIconWindShackle(pc.Id, _getBuffIconDuration));
			}
			//pc.sendPackets(new S_OwnCharStatus(pc)); TODO
		}

		// グラフィックの送信
		private void sendGrfx(bool isSkillAction)
		{
			if (_actid == 0)
			{
				_actid = _skill.ActionId;
			}
			if (_gfxid == 0)
			{
				_gfxid = _skill.CastGfx;
			}
			if (_gfxid == 0)
			{
				return; // 表示するグラフィックが無い
			}
			int[] data = null;

			if (_user is L1PcInstance)
			{

				int targetid = 0;
				if (_skillId != L1SkillId.FIRE_WALL && _skillId != L1SkillId.LIFE_STREAM)
				{
					targetid = _target.Id;
				}
				L1PcInstance pc = (L1PcInstance)_user;

				switch (_skillId)
				{
					case L1SkillId.FIRE_WALL: // 火牢
					case L1SkillId.LIFE_STREAM: // 治癒能量風暴
					case L1SkillId.ELEMENTAL_FALL_DOWN: // 弱化屬性
						if (_skillId == L1SkillId.FIRE_WALL)
						{
							pc.Heading = pc.targetDirection(_targetX, _targetY);
							pc.sendPackets(new S_ChangeHeading(pc));
							pc.broadcastPacket(new S_ChangeHeading(pc));
						}
						S_DoActionGFX gfx = new S_DoActionGFX(pc.Id, _actid);
						pc.sendPackets(gfx);
						pc.broadcastPacket(gfx);
						return;
					case L1SkillId.SHOCK_STUN: // 衝擊之暈
						if (_targetList.Count == 0)
						{ // 失敗
							return;
						}
						else
						{
							if (_target is L1PcInstance)
							{
								L1PcInstance targetPc = (L1PcInstance)_target;
								targetPc.sendPackets(new S_SkillSound(targetid, 4434));
								targetPc.broadcastPacket(new S_SkillSound(targetid, 4434));
							}
							else if (_target is L1NpcInstance)
							{
								_target.broadcastPacket(new S_SkillSound(targetid, 4434));
							}
							return;
						}
					case L1SkillId.LIGHT: // 日光術
						pc.sendPackets(new S_Sound(145));
						break;
					case L1SkillId.MIND_BREAK: // 心靈破壞
					case L1SkillId.JOY_OF_PAIN: // 疼痛的歡愉
						data = new int[] { _actid, _dmg, 0 }; // data = {actid, dmg, effect}
						pc.sendPackets(new S_AttackPacket(pc, targetid, data));
						pc.broadcastPacket(new S_AttackPacket(pc, targetid, data));
						pc.sendPackets(new S_SkillSound(targetid, _gfxid));
						pc.broadcastPacket(new S_SkillSound(targetid, _gfxid));
						return;
					case L1SkillId.CONFUSION: // 混亂
						data = new int[] { _actid, _dmg, 0 }; // data = {actid, dmg, effect}
						pc.sendPackets(new S_AttackPacket(pc, targetid, data));
						pc.broadcastPacket(new S_AttackPacket(pc, targetid, data));
						return;
					case L1SkillId.SMASH: // 暴擊
						pc.sendPackets(new S_SkillSound(targetid, _gfxid));
						pc.broadcastPacket(new S_SkillSound(targetid, _gfxid));
						return;
					case L1SkillId.TAMING_MONSTER: // 迷魅
						pc.sendPackets(new S_EffectLocation(_targetX, _targetY, _gfxid));
						pc.broadcastPacket(new S_EffectLocation(_targetX, _targetY, _gfxid));
						return;
					default:
						break;
				}

				if (_targetList.Count == 0 && !( _skill.Target.Equals("none") ))
				{
					// ターゲット数が０で対象を指定するスキルの場合、魔法使用エフェクトだけ表示して終了
					int tempchargfx = _player.TempCharGfx;
					if (( tempchargfx == 5727 ) || ( tempchargfx == 5730 ))
					{ // シャドウ系変身のモーション対応
						_actid = ActionCodes.ACTION_SkillBuff;
					}
					else if (( tempchargfx == 5733 ) || ( tempchargfx == 5736 ))
					{
						_actid = ActionCodes.ACTION_Attack;
					}
					if (isSkillAction)
					{
						S_DoActionGFX gfx = new S_DoActionGFX(_player.Id, _actid);
						_player.sendPackets(gfx);
						_player.broadcastPacket(gfx);
					}
					return;
				}

				if (_skill.Target.Equals("attack") && ( _skillId != L1SkillId.TURN_UNDEAD ))
				{
					if (isPcSummonPet(_target))
					{ // 目標玩家、寵物、召喚獸
						if (( _player.ZoneType == 1 ) || ( _target.ZoneType == 1 ) || _player.checkNonPvP(_player, _target))
						{ // Non-PvP設定
							data = new int[] { _actid, 0, _gfxid, 6 };
							_player.sendPackets(new S_UseAttackSkill(_player, _target.Id, _targetX, _targetY, data));
							_player.broadcastPacket(new S_UseAttackSkill(_player, _target.Id, _targetX, _targetY, data));
							return;
						}
					}

					if (SkillArea == 0)
					{ // 單體攻擊魔法
						data = new int[] { _actid, _dmg, _gfxid, 6 };
						_player.sendPackets(new S_UseAttackSkill(_player, targetid, _targetX, _targetY, data));
						_player.broadcastPacket(new S_UseAttackSkill(_player, targetid, _targetX, _targetY, data));
						_target.broadcastPacketExceptTargetSight(new S_DoActionGFX(targetid, ActionCodes.ACTION_Damage), _player);
					}
					else
					{ // 有方向範囲攻撃魔法
						L1Character[] cha = new L1Character[_targetList.Count];
						int i = 0;
						foreach (TargetStatus ts in _targetList)
						{
							cha[i] = ts.Target;
							i++;
						}
						_player.sendPackets(new S_RangeSkill(_player, cha, _gfxid, _actid, S_RangeSkill.TYPE_DIR));
						_player.broadcastPacket(new S_RangeSkill(_player, cha, _gfxid, _actid, S_RangeSkill.TYPE_DIR));
					}
				}
				else if (_skill.Target.Equals("none") && ( _skill.Type == L1Skills.TYPE_ATTACK ))
				{ // 無方向範囲攻撃魔法
					L1Character[] cha = new L1Character[_targetList.Count];
					int i = 0;
					foreach (TargetStatus ts in _targetList)
					{
						cha[i] = ts.Target;
						cha[i].broadcastPacketExceptTargetSight(new S_DoActionGFX(cha[i].Id, ActionCodes.ACTION_Damage), _player);
						i++;
					}
					_player.sendPackets(new S_RangeSkill(_player, cha, _gfxid, _actid, S_RangeSkill.TYPE_NODIR));
					_player.broadcastPacket(new S_RangeSkill(_player, cha, _gfxid, _actid, S_RangeSkill.TYPE_NODIR));
				}
				else
				{ // 補助魔法
				  // 指定傳送、集體傳送術、世界樹的呼喚以外
					if (( _skillId != L1SkillId.TELEPORT ) && ( _skillId != L1SkillId.MASS_TELEPORT ) && ( _skillId != L1SkillId.TELEPORT_TO_MATHER ))
					{
						// 施法動作
						if (isSkillAction)
						{
							S_DoActionGFX gfx = new S_DoActionGFX(_player.Id, _skill.ActionId);
							_player.sendPackets(gfx);
							_player.broadcastPacket(gfx);
						}
						// 魔法屏障、反擊屏障、鏡反射 魔法效果只有自身顯示
						if (( _skillId == L1SkillId.COUNTER_MAGIC ) || ( _skillId == L1SkillId.COUNTER_BARRIER ) || ( _skillId == L1SkillId.COUNTER_MIRROR ))
						{
							_player.sendPackets(new S_SkillSound(targetid, _gfxid));
						}
						else if (( _skillId == L1SkillId.AWAKEN_ANTHARAS ) || ( _skillId == L1SkillId.AWAKEN_FAFURION ) || ( _skillId == L1SkillId.AWAKEN_VALAKAS ))
						{ // 覚醒：ヴァラカス
							if (_skillId == _player.AwakeSkillId)
							{ // 再詠唱なら解除でエフェクトなし
								_player.sendPackets(new S_SkillSound(targetid, _gfxid));
								_player.broadcastPacket(new S_SkillSound(targetid, _gfxid));
							}
							else
							{
								return;
							}
						}
						else
						{
							_player.sendPackets(new S_SkillSound(targetid, _gfxid));
							_player.broadcastPacket(new S_SkillSound(targetid, _gfxid));
						}
					}

					// スキルのエフェクト表示はターゲット全員だが、あまり必要性がないので、ステータスのみ送信
					foreach (TargetStatus ts in _targetList)
					{
						L1Character cha = ts.Target;
						if (cha is L1PcInstance)
						{
							L1PcInstance chaPc = (L1PcInstance)cha;
							chaPc.sendPackets(new S_OwnCharStatus(chaPc));
						}
					}
				}
			}
			else if (_user is L1NpcInstance)
			{ // NPCがスキルを使った場合
				int targetid = _target.Id;

				if (_user is L1MerchantInstance)
				{
					_user.broadcastPacket(new S_SkillSound(targetid, _gfxid));
					return;
				}

				if (_skillId == L1SkillId.CURSE_PARALYZE || _skillId == L1SkillId.WEAKNESS || _skillId == L1SkillId.DISEASE)
				{ // 木乃伊的詛咒、弱化術、疾病術
					_user.Heading = _user.targetDirection(_targetX, _targetY); // 改變面向
					_user.broadcastPacket(new S_ChangeHeading(_user));
				}

				if (_targetList.Count == 0 && !( _skill.Target.Equals("none") ))
				{
					// ターゲット数が０で対象を指定するスキルの場合、魔法使用エフェクトだけ表示して終了
					S_DoActionGFX gfx = new S_DoActionGFX(_user.Id, _actid);
					_user.broadcastPacket(gfx);
					return;
				}

				if (_skill.Target.Equals("attack") && ( _skillId != L1SkillId.TURN_UNDEAD ))
				{
					if (SkillArea == 0)
					{ // 單體攻擊魔法
						data = new int[] { _actid, _dmg, _gfxid, 6 };
						_user.broadcastPacket(new S_UseAttackSkill(_user, targetid, _targetX, _targetY, data));
						_target.broadcastPacketExceptTargetSight(new S_DoActionGFX(targetid, ActionCodes.ACTION_Damage), _user);
					}
					else
					{ // 有方向範囲攻撃魔法
						L1Character[] cha = new L1Character[_targetList.Count];
						int i = 0;
						foreach (TargetStatus ts in _targetList)
						{
							cha[i] = ts.Target;
							cha[i].broadcastPacketExceptTargetSight(new S_DoActionGFX(cha[i].Id, ActionCodes.ACTION_Damage), _user);
							i++;
						}
						_user.broadcastPacket(new S_RangeSkill(_user, cha, _gfxid, _actid, S_RangeSkill.TYPE_DIR));
					}
				}
				else if (_skill.Target.Equals("none") && ( _skill.Type == L1Skills.TYPE_ATTACK ))
				{ // 無方向範囲攻撃魔法
					L1Character[] cha = new L1Character[_targetList.Count];
					int i = 0;
					foreach (TargetStatus ts in _targetList)
					{
						cha[i] = ts.Target;
						i++;
					}
					_user.broadcastPacket(new S_RangeSkill(_user, cha, _gfxid, _actid, S_RangeSkill.TYPE_NODIR));
				}
				else
				{ // 補助魔法
				  // テレポート、マステレ、テレポートトゥマザー以外
					if (( _skillId != L1SkillId.TELEPORT ) && ( _skillId != L1SkillId.MASS_TELEPORT ) && ( _skillId != L1SkillId.TELEPORT_TO_MATHER ))
					{
						// 魔法を使う動作のエフェクトは使用者だけ
						S_DoActionGFX gfx = new S_DoActionGFX(_user.Id, _actid);
						_user.broadcastPacket(gfx);
						_user.broadcastPacket(new S_SkillSound(targetid, _gfxid));
					}
				}
			}
		}

		/// <summary>
		/// 刪除重複的魔法狀態 </summary>
		private void deleteRepeatedSkills(L1Character cha)
		{
			int[][] repeatedSkills = new int[][]
			{
				new int[] { L1SkillId.FIRE_WEAPON, L1SkillId.WIND_SHOT, L1SkillId.FIRE_BLESS, L1SkillId.STORM_EYE, L1SkillId.BURNING_WEAPON, L1SkillId.STORM_SHOT, L1SkillId.EFFECT_BLESS_OF_MAZU },
				new int[] { L1SkillId.SHIELD, L1SkillId.SHADOW_ARMOR, L1SkillId.EARTH_SKIN, L1SkillId.EARTH_BLESS, L1SkillId.IRON_SKIN },
				new int[] { L1SkillId.STATUS_BRAVE, L1SkillId.STATUS_ELFBRAVE, L1SkillId.HOLY_WALK, L1SkillId.MOVING_ACCELERATION, L1SkillId.WIND_WALK, L1SkillId.STATUS_BRAVE2, L1SkillId.BLOODLUST },
				new int[] { L1SkillId.HASTE, L1SkillId.GREATER_HASTE, L1SkillId.STATUS_HASTE },
				new int[] { L1SkillId.SLOW, L1SkillId.MASS_SLOW, L1SkillId.ENTANGLE },
				new int[] { L1SkillId.PHYSICAL_ENCHANT_DEX, L1SkillId.DRESS_DEXTERITY },
				new int[] { L1SkillId.PHYSICAL_ENCHANT_STR, L1SkillId.DRESS_MIGHTY },
				new int[] { L1SkillId.GLOWING_AURA, L1SkillId.SHINING_AURA },
				new int[] { L1SkillId.MIRROR_IMAGE, L1SkillId.UNCANNY_DODGE }
			};


			foreach (int[] skills in repeatedSkills)
			{
				foreach (int id in skills)
				{
					if (id == _skillId)
					{
						stopSkillList(cha, skills);
					}
				}
			}
		}

		// 重複しているスキルを一旦すべて削除
		private void stopSkillList(L1Character cha, int[] repeat_skill)
		{
			foreach (int skillId in repeat_skill)
			{
				if (skillId != _skillId)
				{
					cha.removeSkillEffect(L1SkillId.skillId);
				}
			}
		}

		// ディレイの設定
		private void setDelay()
		{
			if (_skill.ReuseDelay > 0)
			{
				L1SkillDelay.onSkillUse(_user, _skill.ReuseDelay);
			}
		}

		private void runSkill()
		{

			switch (_skillId)
			{
				case L1SkillId.LIFE_STREAM:
					L1EffectSpawn.Instance.spawnEffect(81169, _skill.BuffDuration * 1000, _targetX, _targetY, _user.MapId);
					return;
				case L1SkillId.CUBE_IGNITION:
					L1EffectSpawn.Instance.spawnEffect(80149, _skill.BuffDuration * 1000, _targetX, _targetY, _user.MapId, (L1PcInstance)_user, _skillId);
					return;
				case L1SkillId.CUBE_QUAKE:
					L1EffectSpawn.Instance.spawnEffect(80150, _skill.BuffDuration * 1000, _targetX, _targetY, _user.MapId, (L1PcInstance)_user, _skillId);
					return;
				case L1SkillId.CUBE_SHOCK:
					L1EffectSpawn.Instance.spawnEffect(80151, _skill.BuffDuration * 1000, _targetX, _targetY, _user.MapId, (L1PcInstance)_user, _skillId);
					return;
				case L1SkillId.CUBE_BALANCE:
					L1EffectSpawn.Instance.spawnEffect(80152, _skill.BuffDuration * 1000, _targetX, _targetY, _user.MapId, (L1PcInstance)_user, _skillId);
					return;
				case L1SkillId.FIRE_WALL: // 火牢
					L1EffectSpawn.Instance.doSpawnFireWall(_user, _targetX, _targetY);
					return;
				case L1SkillId.TRUE_TARGET: // 精準目標
					if (_user is L1PcInstance)
					{
						L1PcInstance pri = (L1PcInstance)_user;
						L1EffectInstance effect = L1EffectSpawn.Instance.spawnEffect(80153, 5 * 1000, _targetX + 2, _targetY - 1, _user.MapId);
						if (_targetID != 0)
						{
							pri.sendPackets(new S_TrueTarget(_targetID, pri.Id, _message));
							if (pri.Clanid != 0)
							{
								L1PcInstance[] players = L1World.Instance.getClan(pri.Clanname).OnlineClanMember;
								foreach (L1PcInstance pc in players)
								{
									pc.sendPackets(new S_TrueTarget(_targetID, pc.Id, _message));
								}
							}
						}
						else if (effect != null)
						{
							pri.sendPackets(new S_TrueTarget(effect.Id, pri.Id, _message));
							if (pri.Clanid != 0)
							{
								L1PcInstance[] players = L1World.Instance.getClan(pri.Clanname).OnlineClanMember;
								foreach (L1PcInstance pc in players)
								{
									pc.sendPackets(new S_TrueTarget(effect.Id, pc.Id, _message));
								}
							}
						}
					}
					return;
				default:
					break;
			}

			// 魔法屏障不可抵擋的魔法
			foreach (int skillId in EXCEPT_COUNTER_MAGIC)
			{
				if (_skillId == skillId)
				{
					_isCounterMagic = false;
					break;
				}
			}

			// NPCにショックスタンを使用させるとonActionでNullPointerExceptionが発生するため
			// とりあえずPCが使用した時のみ
			if (( _skillId == L1SkillId.SHOCK_STUN ) && ( _user is L1PcInstance ))
			{
				_target.onAction(_player);
			}

			if (!isTargetCalc(_target))
			{
				return;
			}

			try
			{
				TargetStatus ts = null;
				L1Character cha = null;
				int dmg = 0;
				int drainMana = 0;
				int heal = 0;
				bool isSuccess = false;
				int undeadType = 0;
				TargetStatus[] targetStatuses = _targetList.ToArray();
				for (int currentIndex = 0; currentIndex < targetStatuses.Length; currentIndex++)
				{
					ts = targetStatuses[currentIndex];
					cha = null;
					dmg = 0;
					heal = 0;
					isSuccess = false;
					undeadType = 0;

					cha = ts.Target;

					if (!ts.Calc || !isTargetCalc(cha))
					{
						continue; // 計算する必要がない。
					}

					L1Magic _magic = new L1Magic(_user, cha);
					_magic.Leverage = Leverage;

					if (cha is L1MonsterInstance)
					{ // 不死係判斷
						undeadType = ( (L1MonsterInstance)cha ).NpcTemplate.get_undead();
					}

					// 確率系スキルで失敗が確定している場合
					if (( ( _skill.Type == L1Skills.TYPE_CURSE ) || ( _skill.Type == L1Skills.TYPE_PROBABILITY ) ) && isTargetFailure(cha))
					{
						_targetList.Remove(ts);//先這樣吧
						continue;
					}

					if (cha is L1PcInstance)
					{ // ターゲットがPCの場合のみアイコンは送信する。
						if (_skillTime == 0)
						{
							_getBuffIconDuration = _skill.BuffDuration; // 効果時間
						}
						else
						{
							_getBuffIconDuration = _skillTime; // パラメータのtimeが0以外なら、効果時間として設定する
						}
					}

					deleteRepeatedSkills(cha); // 刪除無法共同存在的魔法狀態

					if (( _skill.Type == L1Skills.TYPE_ATTACK ) && ( _user.Id != cha.Id ))
					{ // 攻撃系スキル＆ターゲットが使用者以外であること。
						if (isUseCounterMagic(cha))
						{
							// カウンターマジックが発動した場合、リストから削除
							_targetList.Remove(ts);//先這樣吧
							continue;
						}
						dmg = _magic.calcMagicDamage(_skillId);
						_dmg = dmg;
						cha.removeSkillEffect(L1SkillId.ERASE_MAGIC); // イレースマジック中なら、攻撃魔法で解除
					}
					else if (( _skill.Type == L1Skills.TYPE_CURSE ) || ( _skill.Type == L1Skills.TYPE_PROBABILITY ))
					{ // 確率系スキル
						isSuccess = _magic.calcProbabilityMagic(_skillId);
						if (_skillId != L1SkillId.ERASE_MAGIC)
						{
							cha.removeSkillEffect(L1SkillId.ERASE_MAGIC); // イレースマジック中なら、確率魔法で解除
						}
						if (_skillId != L1SkillId.FOG_OF_SLEEPING)
						{
							cha.removeSkillEffect(L1SkillId.FOG_OF_SLEEPING); // フォグオブスリーピング中なら、確率魔法で解除
						}
						if (isSuccess)
						{ // 成功したがカウンターマジックが発動した場合、リストから削除
							if (isUseCounterMagic(cha))
							{ // カウンターマジックが発動したか
								_targetList.Remove(ts);//先這樣吧
								continue;
							}
						}
						else
						{ // 失敗した場合、リストから削除
							if (( _skillId == L1SkillId.FOG_OF_SLEEPING ) && ( cha is L1PcInstance ))
							{
								L1PcInstance pc = (L1PcInstance)cha;
								pc.sendPackets(new S_ServerMessage(297)); // 你感覺些微地暈眩。
							}
							_targetList.Remove(ts);//先這樣吧
							continue;
						}
					}
					// 治癒性魔法
					else if (_skill.Type == L1Skills.TYPE_HEAL)
					{
						// 回復量
						dmg = -1 * _magic.calcHealing(_skillId);
						if (cha.hasSkillEffect(L1SkillId.WATER_LIFE))
						{ // 水之元氣-效果 2倍
							dmg *= 2;
							cha.killSkillEffectTimer(L1SkillId.WATER_LIFE); // 效果只有一次
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								pc.sendPackets(new S_SkillIconWaterLife());
							}
						}
						if (cha.hasSkillEffect(L1SkillId.POLLUTE_WATER))
						{ // 汙濁之水-效果減半
							dmg /= 2;
						}
					}
					// 顯示團體魔法效果在隊友或盟友
					else if (( _skillId == L1SkillId.FIRE_BLESS || _skillId == L1SkillId.STORM_EYE || _skillId == L1SkillId.EARTH_BLESS || _skillId == L1SkillId.GLOWING_AURA || _skillId == L1SkillId.SHINING_AURA || _skillId == L1SkillId.BRAVE_AURA ) && _user.Id != cha.Id)
					{
						if (cha is L1PcInstance)
						{
							L1PcInstance _targetPc = (L1PcInstance)cha;
							_targetPc.sendPackets(new S_SkillSound(_targetPc.Id, _skill.CastGfx));
							_targetPc.broadcastPacket(new S_SkillSound(_targetPc.Id, _skill.CastGfx));
						}
					}

					// ■■■■ 個別処理のあるスキルのみ書いてください。 ■■■■

					// 除了衝暈、骷髏毀壞之外魔法效果存在時，只更新效果時間跟圖示。
					if (cha.hasSkillEffect(L1SkillId._skillId) && ( _skillId != L1SkillId.SHOCK_STUN && _skillId != L1SkillId.BONE_BREAK && _skillId != L1SkillId.CONFUSION && _skillId != L1SkillId.THUNDER_GRAB ))
					{
						addMagicList(cha, true); // 魔法效果已存在時
						if (_skillId != L1SkillId.SHAPE_CHANGE)
						{ // 除了變形術之外
							continue;
						}
					}

					switch (_skillId)
					{
						// 加速術
						case L1SkillId.HASTE:
							if (cha.MoveSpeed != 2)
							{ // スロー中以外
								if (cha is L1PcInstance)
								{
									L1PcInstance pc = (L1PcInstance)cha;
									if (pc.HasteItemEquipped > 0)
									{
										continue;
									}
									pc.Drink = false;
									pc.sendPackets(new S_SkillHaste(pc.Id, 1, _getBuffIconDuration));
								}
								cha.broadcastPacket(new S_SkillHaste(cha.Id, 1, 0));
								cha.MoveSpeed = 1;
							}
							else
							{ // スロー中
								int skillNum = 0;
								if (cha.hasSkillEffect(L1SkillId.SLOW))
								{
									skillNum = L1SkillId.SLOW;
								}
								else if (cha.hasSkillEffect(L1SkillId.MASS_SLOW))
								{
									skillNum = L1SkillId.MASS_SLOW;
								}
								else if (cha.hasSkillEffect(L1SkillId.ENTANGLE))
								{
									skillNum = L1SkillId.ENTANGLE;
								}
								if (skillNum != 0)
								{
									cha.removeSkillEffect(L1SkillId.skillNum);
									cha.removeSkillEffect(L1SkillId.HASTE);
									cha.MoveSpeed = 0;
									continue;
								}
							}
							break;
						// 強力加速術
						case L1SkillId.GREATER_HASTE:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								if (pc.HasteItemEquipped > 0)
								{
									continue;
								}
								if (pc.MoveSpeed != 2)
								{ // スロー中以外
									pc.Drink = false;
									pc.MoveSpeed = 1;
									pc.sendPackets(new S_SkillHaste(pc.Id, 1, _getBuffIconDuration));
									pc.broadcastPacket(new S_SkillHaste(pc.Id, 1, 0));
								}
								else
								{ // スロー中
									int skillNum = 0;
									if (pc.hasSkillEffect(L1SkillId.SLOW))
									{
										skillNum = L1SkillId.SLOW;
									}
									else if (pc.hasSkillEffect(L1SkillId.MASS_SLOW))
									{
										skillNum = L1SkillId.MASS_SLOW;
									}
									else if (pc.hasSkillEffect(L1SkillId.ENTANGLE))
									{
										skillNum = L1SkillId.ENTANGLE;
									}
									if (skillNum != 0)
									{
										pc.removeSkillEffect(L1SkillId.skillNum);
										pc.removeSkillEffect(L1SkillId.GREATER_HASTE);
										pc.MoveSpeed = 0;
										continue;
									}
								}
							}
							break;
						// 緩速術、集體緩速術、地面障礙
						case L1SkillId.SLOW:
						case L1SkillId.MASS_SLOW:
						case L1SkillId.ENTANGLE:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								if (pc.HasteItemEquipped > 0)
								{
									continue;
								}
							}
							if (cha.MoveSpeed == 0)
							{
								if (cha is L1PcInstance)
								{
									L1PcInstance pc = (L1PcInstance)cha;
									pc.sendPackets(new S_SkillHaste(pc.Id, 2, _getBuffIconDuration));
								}
								cha.broadcastPacket(new S_SkillHaste(cha.Id, 2, _getBuffIconDuration));
								cha.MoveSpeed = 2;
							}
							else if (cha.MoveSpeed == 1)
							{
								int skillNum = 0;
								if (cha.hasSkillEffect(L1SkillId.HASTE))
								{
									skillNum = L1SkillId.HASTE;
								}
								else if (cha.hasSkillEffect(L1SkillId.GREATER_HASTE))
								{
									skillNum = L1SkillId.GREATER_HASTE;
								}
								else if (cha.hasSkillEffect(L1SkillId.STATUS_HASTE))
								{
									skillNum = L1SkillId.STATUS_HASTE;
								}
								if (skillNum != 0)
								{
									cha.removeSkillEffect(L1SkillId.skillNum);
									cha.removeSkillEffect(L1SkillId._skillId);
									cha.MoveSpeed = 0;
									continue;
								}
							}
							break;
						// 寒冷戰慄、吸血鬼之吻
						case L1SkillId.CHILL_TOUCH:
						case L1SkillId.VAMPIRIC_TOUCH:
							heal = dmg;
							break;
						// 亞力安冰矛圍籬
						case L1SkillId.ICE_LANCE_COCKATRICE:
						// 邪惡蜥蜴冰矛圍籬
						case L1SkillId.ICE_LANCE_BASILISK:
						// 冰毛圍籬、冰雪颶風、寒冰噴吐
						case L1SkillId.ICE_LANCE:
						case L1SkillId.FREEZING_BLIZZARD:
						case L1SkillId.FREEZING_BREATH:
							_isFreeze = _magic.calcProbabilityMagic(_skillId);
							if (_isFreeze)
							{
								int time = _skill.BuffDuration * 1000;
								L1EffectSpawn.Instance.spawnEffect(81168, time, cha.X, cha.Y, cha.MapId);
								if (cha is L1PcInstance)
								{
									L1PcInstance pc = (L1PcInstance)cha;
									pc.sendPackets(new S_Poison(pc.Id, 2));
									pc.broadcastPacket(new S_Poison(pc.Id, 2));
									pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_FREEZE, true));
								}
								else if (( cha is L1MonsterInstance ) || ( cha is L1SummonInstance ) || ( cha is L1PetInstance ))
								{
									L1NpcInstance npc = (L1NpcInstance)cha;
									npc.broadcastPacket(new S_Poison(npc.Id, 2));
									npc.Paralyzed = true;
									npc.ParalysisTime = time;
								}
							}
							break;
						// 大地屏障
						case L1SkillId.EARTH_BIND:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								pc.sendPackets(new S_Poison(pc.Id, 2));
								pc.broadcastPacket(new S_Poison(pc.Id, 2));
								pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_FREEZE, true));
							}
							else if (( cha is L1MonsterInstance ) || ( cha is L1SummonInstance ) || ( cha is L1PetInstance ))
							{
								L1NpcInstance npc = (L1NpcInstance)cha;
								npc.broadcastPacket(new S_Poison(npc.Id, 2));
								npc.Paralyzed = true;
								npc.ParalysisTime = _skill.BuffDuration * 1000;
							}
							break;
						case L1SkillId.AREA_POISON: // 毒霧-前方 3X3
							_user.Heading = _user.targetDirection(_targetX, _targetY); // 改變面向
							int locX = 0;
							int locY = 0;
							for (int i = 0; i < 3; i++)
							{
								for (int j = 0; j < 3; j++)
								{
									switch (_user.Heading)
									{
										case 0:
											locX = ( -1 + j );
											locY = -1 * ( -3 + i );
											break;
										case 1:
											locX = -1 * ( 2 + j - i );
											locY = -1 * ( -4 + j + i );
											break;
										case 2:
											locX = -1 * ( 3 - i );
											locY = ( -1 + j );
											break;
										case 3:
											locX = -1 * ( 4 - j - i );
											locY = -1 * ( 2 + j - i );
											break;
										case 4:
											locX = ( 1 - j );
											locY = -1 * ( 3 - i );
											break;
										case 5:
											locX = -1 * ( -2 - j + i );
											locY = -1 * ( 4 - j - i );
											break;
										case 6:
											locX = -1 * ( -3 + i );
											locY = ( 1 - j );
											break;
										case 7:
											locX = -1 * ( -4 + j + i );
											locY = -1 * ( -2 - j + i );
											break;
									}
									L1EffectSpawn.Instance.spawnEffect(93002, 10000, _user.X - locX, _user.Y - locY, _user.MapId);
								}
							}
							break;
						// 衝擊之暈
						case L1SkillId.SHOCK_STUN:
							int[] stunTimeArray = new int[] { 500, 1000, 1500, 2000, 2500, 3000 };
							int rnd = RandomHelper.Next(stunTimeArray.Length);
							_shockStunDuration = stunTimeArray[rnd];
							if (( cha is L1PcInstance ) && cha.hasSkillEffect(L1SkillId.SHOCK_STUN))
							{
								_shockStunDuration += cha.getSkillEffectTimeSec(L1SkillId.SHOCK_STUN) * 1000;
							}

							L1EffectSpawn.Instance.spawnEffect(81162, _shockStunDuration, cha.X, cha.Y, cha.MapId);
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_STUN, true));
							}
							else if (( cha is L1MonsterInstance ) || ( cha is L1SummonInstance ) || ( cha is L1PetInstance ))
							{
								L1NpcInstance npc = (L1NpcInstance)cha;
								npc.Paralyzed = true;
								npc.ParalysisTime = _shockStunDuration;
							}
							break;
						// 奪命之雷
						case L1SkillId.THUNDER_GRAB:
							isSuccess = _magic.calcProbabilityMagic(_skillId);
							if (isSuccess)
							{
								if (!cha.hasSkillEffect(L1SkillId.THUNDER_GRAB_START) && !cha.hasSkillEffect(L1SkillId.STATUS_FREEZE))
								{
									if (cha is L1PcInstance)
									{
										L1PcInstance pc = (L1PcInstance)cha;
										pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_BIND, true));
										pc.sendPackets(new S_SkillSound(pc.Id, 4184));
										pc.broadcastPacket(new S_SkillSound(pc.Id, 4184));
									}
									else if (cha is L1NpcInstance)
									{
										L1NpcInstance npc = (L1NpcInstance)cha;
										npc.Paralyzed = true;
										npc.broadcastPacket(new S_SkillSound(npc.Id, 4184));
									}
									cha.setSkillEffect(L1SkillId.THUNDER_GRAB_START, 500);
								}
							}
							break;
						// 起死回生術
						case L1SkillId.TURN_UNDEAD:
							if (undeadType == 1 || undeadType == 3)
							{
								dmg = cha.CurrentHp;
							}
							break;
						// 魔力奪取
						case L1SkillId.MANA_DRAIN:
							int chance = RandomHelper.Next(10) + 5;
							drainMana = chance + ( _user.getInt() / 2 );
							if (cha.CurrentMp < drainMana)
							{
								drainMana = cha.CurrentMp;
							}
							break;
						// 指定傳送、集體傳送術
						case L1SkillId.TELEPORT:
						case L1SkillId.MASS_TELEPORT:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1BookMark bookm = pc.getBookMark(_bookmarkId);
								if (bookm != null)
								{ // ブックマークを取得出来たらテレポート
									if (pc.Map.Escapable || pc.Gm)
									{
										int newX = bookm.LocX;
										int newY = bookm.LocY;
										short mapId = bookm.MapId;

										if (_skillId == L1SkillId.MASS_TELEPORT)
										{ // マステレポート
											IList<L1PcInstance> clanMember = L1World.Instance.getVisiblePlayer(pc);
											foreach (L1PcInstance member in clanMember)
											{
												if (( pc.Location.getTileLineDistance(member.Location) <= 3 ) && ( member.Clanid == pc.Clanid ) && ( pc.Clanid != 0 ) && ( member.Id != pc.Id ))
												{
													L1Teleport.teleport(member, newX, newY, mapId, 5, true);
												}
											}
										}
										L1Teleport.teleport(pc, newX, newY, mapId, 5, true);
									}
									else
									{
										pc.sendPackets(new S_ServerMessage(79));
										pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
									}
								}
								else
								{ // ブックマークが取得出来なかった、あるいは「任意の場所」を選択した場合の処理
									if (pc.Map.Teleportable || pc.Gm)
									{
										L1Location newLocation = pc.Location.randomLocation(200, true);
										int newX = newLocation.X;
										int newY = newLocation.Y;
										short mapId = (short)newLocation.MapId;

										if (_skillId == L1SkillId.MASS_TELEPORT)
										{
											IList<L1PcInstance> clanMember = L1World.Instance.getVisiblePlayer(pc);
											foreach (L1PcInstance member in clanMember)
											{
												if (( pc.Location.getTileLineDistance(member.Location) <= 3 ) && ( member.Clanid == pc.Clanid ) && ( pc.Clanid != 0 ) && ( member.Id != pc.Id ))
												{
													L1Teleport.teleport(member, newX, newY, mapId, 5, true);
												}
											}
										}
										L1Teleport.teleport(pc, newX, newY, mapId, 5, true);
									}
									else
									{
										pc.sendPackets(new S_ServerMessage(276)); // \f1在此無法使用傳送。
										pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
									}
								}
							}
							break;
						// 呼喚盟友
						case L1SkillId.CALL_CLAN:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1PcInstance clanPc = (L1PcInstance)L1World.Instance.findObject(_targetID);
								if (clanPc != null)
								{
									clanPc.TempID = pc.Id;
									clanPc.sendPackets(new S_Message_YN(729, "")); // 盟主正在呼喚你，你要接受他的呼喚嗎？(Y/N)
								}
							}
							break;
						// 援護盟友
						case L1SkillId.RUN_CLAN:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1PcInstance clanPc = (L1PcInstance)L1World.Instance.findObject(_targetID);
								if (clanPc != null)
								{
									if (pc.Map.Escapable || pc.Gm)
									{
										bool castle_area = L1CastleLocation.checkInAllWarArea(clanPc.X, clanPc.Y, clanPc.MapId);
										if (( ( clanPc.MapId == 0 ) || ( clanPc.MapId == 4 ) || ( clanPc.MapId == 304 ) ) && ( castle_area == false ))
										{
											L1Teleport.teleport(pc, clanPc.X, clanPc.Y, clanPc.MapId, 5, true);
										}
										else
										{
											pc.sendPackets(new S_ServerMessage(79));
										}
									}
									else
									{
										// 這附近的能量影響到瞬間移動。在此地無法使用瞬間移動。
										pc.sendPackets(new S_ServerMessage(647));
										pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, true));
									}
								}
							}
							break;
						// 強力無所遁形
						case L1SkillId.COUNTER_DETECTION:
							if (cha is L1PcInstance)
							{
								dmg = _magic.calcMagicDamage(_skillId);
							}
							else if (cha is L1NpcInstance)
							{
								L1NpcInstance npc = (L1NpcInstance)cha;
								int hiddenStatus = npc.HiddenStatus;
								if (hiddenStatus == L1NpcInstance.HIDDEN_STATUS_SINK)
								{
									npc.appearOnGround(_player);
								}
								else
								{
									dmg = 0;
								}
							}
							else
							{
								dmg = 0;
							}
							break;
						// 創造魔法武器
						case L1SkillId.CREATE_MAGICAL_WEAPON:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1ItemInstance item = pc.Inventory.getItem(_itemobjid);
								if (( item != null ) && ( item.Item.Type2 == 1 ))
								{
									int item_type = item.Item.Type2;
									int safe_enchant = item.Item.get_safeenchant();
									int enchant_level = item.EnchantLevel;
									string item_name = item.Name;
									if (safe_enchant < 0)
									{ // 強化不可
										pc.sendPackets(new S_ServerMessage(79));
									}
									else if (safe_enchant == 0)
									{ // 安全圏+0
										pc.sendPackets(new S_ServerMessage(79));
									}
									else if (( item_type == 1 ) && ( enchant_level == 0 ))
									{
										if (!item.Identified)
										{ // 未鑑定
											pc.sendPackets(new S_ServerMessage(161, item_name, "$245", "$247"));
										}
										else
										{
											item_name = "+0 " + item_name;
											pc.sendPackets(new S_ServerMessage(161, "+0 " + item_name, "$245", "$247"));
										}
										item.EnchantLevel = 1;
										pc.Inventory.updateItem(item, L1PcInventory.COL_ENCHANTLVL);
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
						// 提煉魔石
						case L1SkillId.BRING_STONE:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;

								L1ItemInstance item = pc.Inventory.getItem(_itemobjid);
								if (item != null)
								{
									int dark = (int)( 10 + ( pc.Level * 0.8 ) + ( pc.Wis - 6 ) * 1.2 );
									int brave = (int)( dark / 2.1 );
									int wise = (int)( brave / 2.0 );
									int kayser = (int)( wise / 1.9 );
									int run = RandomHelper.Next(100) + 1;
									if (item.Item.ItemId == 40320)
									{
										pc.Inventory.removeItem(item, 1);
										if (dark >= run)
										{
											pc.Inventory.storeItem(40321, 1);
											pc.sendPackets(new S_ServerMessage(403, "$2475")); // 獲得%0%o 。
										}
										else
										{
											pc.sendPackets(new S_ServerMessage(280)); // \f1施咒失敗。
										}
									}
									else if (item.Item.ItemId == 40321)
									{
										pc.Inventory.removeItem(item, 1);
										if (brave >= run)
										{
											pc.Inventory.storeItem(40322, 1);
											pc.sendPackets(new S_ServerMessage(403, "$2476")); // 獲得%0%o 。
										}
										else
										{
											pc.sendPackets(new S_ServerMessage(280)); // \f1施咒失敗。
										}
									}
									else if (item.Item.ItemId == 40322)
									{
										pc.Inventory.removeItem(item, 1);
										if (wise >= run)
										{
											pc.Inventory.storeItem(40323, 1);
											pc.sendPackets(new S_ServerMessage(403, "$2477")); // 獲得%0%o 。
										}
										else
										{
											pc.sendPackets(new S_ServerMessage(280)); // \f1施咒失敗。
										}
									}
									else if (item.Item.ItemId == 40323)
									{
										pc.Inventory.removeItem(item, 1);
										if (kayser >= run)
										{
											pc.Inventory.storeItem(40324, 1);
											pc.sendPackets(new S_ServerMessage(403, "$2478")); // 獲得%0%o 。
										}
										else
										{
											pc.sendPackets(new S_ServerMessage(280)); // \f1施咒失敗。
										}
									}
								}
							}
							break;
						// 日光術
						case L1SkillId.LIGHT:
							if (cha is L1PcInstance)
							{
							}
							break;
						// 暗影之牙
						case L1SkillId.SHADOW_FANG:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1ItemInstance item = pc.Inventory.getItem(_itemobjid);
								if (( item != null ) && ( item.Item.Type2 == 1 ))
								{
									item.setSkillWeaponEnchant(pc, _skillId, _skill.BuffDuration * 1000);
								}
								else
								{
									pc.sendPackets(new S_ServerMessage(79));
								}
							}
							break;
						// 擬似魔法武器
						case L1SkillId.ENCHANT_WEAPON:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1ItemInstance item = pc.Inventory.getItem(_itemobjid);
								if (( item != null ) && ( item.Item.Type2 == 1 ))
								{
									pc.sendPackets(new S_ServerMessage(161, item.LogName, "$245", "$247"));
									item.setSkillWeaponEnchant(pc, _skillId, _skill.BuffDuration * 1000);
								}
								else
								{
									pc.sendPackets(new S_ServerMessage(79));
								}
							}
							break;
						// 神聖武器、祝福魔法武器
						case L1SkillId.HOLY_WEAPON:
						case L1SkillId.BLESS_WEAPON:
							if (cha is L1PcInstance)
							{
								if (!( cha is L1PcInstance ))
								{
									return;
								}
								L1PcInstance pc = (L1PcInstance)cha;
								if (pc.Weapon == null)
								{
									pc.sendPackets(new S_ServerMessage(79));
									return;
								}
								foreach (L1ItemInstance item in pc.Inventory.Items)
								{
									if (pc.Weapon.Equals(item))
									{
										pc.sendPackets(new S_ServerMessage(161, item.LogName, "$245", "$247"));
										item.setSkillWeaponEnchant(pc, _skillId, _skill.BuffDuration * 1000);
										return;
									}
								}
							}
							break;
						// 鎧甲護持
						case L1SkillId.BLESSED_ARMOR:
							if (cha is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)cha;
								L1ItemInstance item = pc.Inventory.getItem(_itemobjid);
								if (( item != null ) && ( item.Item.Type2 == 2 ) && ( item.Item.Type == 2 ))
								{
									pc.sendPackets(new S_ServerMessage(161, item.LogName, "$245", "$247"));
									item.setSkillArmorEnchant(pc, _skillId, _skill.BuffDuration * 1000);
								}
								else
								{
									pc.sendPackets(new S_ServerMessage(79));
								}
							}
							break;
						default:
							L1BuffUtil.SkillEffect(L1SkillId._user, cha, _target, _skillId, _getBuffIconDuration, dmg);
							break;
					}

					// ■■■■ 個別処理ここまで ■■■■

					// 治癒性魔法攻擊不死係的怪物。
					if (( _skill.Type == L1Skills.TYPE_HEAL ) && ( _calcType == PC_NPC ) && ( undeadType == 1 ))
					{
						dmg *= -1;
					}
					// 治癒性魔法無法對此不死係起作用
					if (( _skill.Type == L1Skills.TYPE_HEAL ) && ( _calcType == PC_NPC ) && ( undeadType == 3 ))
					{
						dmg = 0;
					}
					// 無法對城門、守護塔補血
					if (( ( cha is L1TowerInstance ) || ( cha is L1DoorInstance ) ) && ( dmg < 0 ))
					{
						dmg = 0;
					}
					// 吸取魔力。
					if (( dmg > 0 ) || ( drainMana != 0 ))
					{
						_magic.commit(dmg, drainMana);
					}
					// 補血判斷
					if (( _skill.Type == L1Skills.TYPE_HEAL ) && ( dmg < 0 ))
					{
						cha.CurrentHp = ( dmg * -1 ) + cha.CurrentHp;
					}
					// 非治癒性魔法補血判斷(寒戰、吸吻等)
					if (heal > 0)
					{
						_user.CurrentHp = heal + _user.CurrentHp;
					}

					if (cha is L1PcInstance)
					{ // 更新自身狀態
						L1PcInstance pc = (L1PcInstance)cha;
						pc.turnOnOffLight();
						pc.sendPackets(new S_OwnCharAttrDef(pc));
						pc.sendPackets(new S_OwnCharStatus(pc));
						sendHappenMessage(pc); // ターゲットにメッセージを送信
					}

					addMagicList(cha, false); // ターゲットに魔法の効果時間を設定

					if (cha is L1PcInstance)
					{ // ターゲットがPCならば、ライト状態を更新
						L1PcInstance pc = (L1PcInstance)cha;
						pc.turnOnOffLight();
					}
				}

				// 解除隱身
				if (( _skillId == L1SkillId.DETECTION ) || ( _skillId == L1SkillId.COUNTER_DETECTION ))
				{ // 無所遁形、強力無所遁形
					detection(_player);
				}

			}
			catch (Exception e)
			{
				_log.Error(e);
			}
		}

		private void detection(L1PcInstance pc)
		{
			if (!pc.GmInvis && pc.Invisble)
			{ // 自己隱身中
				pc.delInvis();
				pc.beginInvisTimer();
			}

			foreach (L1PcInstance tgt in L1World.Instance.getVisiblePlayer(pc))
			{ // 畫面內其他隱身者
				if (!tgt.GmInvis && tgt.Invisble)
				{
					tgt.delInvis();
				}
			}
			L1WorldTraps.Instance.onDetection(pc);
		}

		// ターゲットについて計算する必要があるか返す
		private bool isTargetCalc(L1Character cha)
		{
			// 三重矢、屠宰者、暴擊、骷髏毀壞
			if (( _user is L1PcInstance ) && ( _skillId == L1SkillId.TRIPLE_ARROW || _skillId == L1SkillId.FOE_SLAYER || _skillId == L1SkillId.SMASH || _skillId == L1SkillId.BONE_BREAK ))
			{
				return true;
			}
			// 攻撃魔法のNon－PvP判定
			if (_skill.Target.Equals("attack") && ( _skillId != L1SkillId.TURN_UNDEAD ))
			{ // 攻撃魔法
				if (isPcSummonPet(cha))
				{ // 対象がPC、サモン、ペット
					if (( _player.ZoneType == 1 ) || ( cha.ZoneType == 1 ) || _player.checkNonPvP(_player, cha))
					{ // Non-PvP設定
						return false;
					}
				}
			}

			// フォグオブスリーピングは自分自身は対象外
			if (( _skillId == L1SkillId.FOG_OF_SLEEPING ) && ( _user.Id == cha.Id ))
			{
				return false;
			}

			// マススローは自分自身と自分のペットは対象外
			if (_skillId == L1SkillId.MASS_SLOW)
			{
				if (_user.Id == cha.Id)
				{
					return false;
				}
				if (cha is L1SummonInstance)
				{
					L1SummonInstance summon = (L1SummonInstance)cha;
					if (_user.Id == summon.Master.Id)
					{
						return false;
					}
				}
				else if (cha is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance)cha;
					if (_user.Id == pet.Master.Id)
					{
						return false;
					}
				}
			}

			// マステレポートは自分自身のみ対象（同時にクラン員もテレポートさせる）
			if (_skillId == L1SkillId.MASS_TELEPORT)
			{
				if (_user.Id != cha.Id)
				{
					return false;
				}
			}

			return true;
		}

		// 対象がPC、サモン、ペットかを返す
		private bool isPcSummonPet(L1Character cha)
		{
			if (_calcType == PC_PC)
			{ // 対象がPC
				return true;
			}

			if (_calcType == PC_NPC)
			{
				if (cha is L1SummonInstance)
				{ // 対象がサモン
					L1SummonInstance summon = (L1SummonInstance)cha;
					if (summon.ExsistMaster)
					{ // マスターが居る
						return true;
					}
				}
				if (cha is L1PetInstance)
				{ // 対象がペット
					return true;
				}
			}
			return false;
		}

		// ターゲットに対して必ず失敗になるか返す
		private bool isTargetFailure(L1Character cha)
		{
			bool isTU = false;
			bool isErase = false;
			bool isManaDrain = false;
			int undeadType = 0;

			if (( cha is L1TowerInstance ) || ( cha is L1DoorInstance ))
			{ // ガーディアンタワー、ドアには確率系スキル無効
				return true;
			}

			if (cha is L1PcInstance)
			{ // 対PCの場合
				if (( _calcType == PC_PC ) && _player.checkNonPvP(_player, cha))
				{ // Non-PvP設定
					L1PcInstance pc = (L1PcInstance)cha;
					if (( _player.Id == pc.Id ) || ( ( pc.Clanid != 0 ) && ( _player.Clanid == pc.Clanid ) ))
					{
						return false;
					}
					return true;
				}
				return false;
			}

			if (cha is L1MonsterInstance)
			{ // ターンアンデット可能か判定
				isTU = ( (L1MonsterInstance)cha ).NpcTemplate.get_IsTU();
			}

			if (cha is L1MonsterInstance)
			{ // イレースマジック可能か判定
				isErase = ( (L1MonsterInstance)cha ).NpcTemplate.get_IsErase();
			}

			if (cha is L1MonsterInstance)
			{ // アンデットの判定
				undeadType = ( (L1MonsterInstance)cha ).NpcTemplate.get_undead();
			}

			// マナドレインが可能か？
			if (cha is L1MonsterInstance)
			{
				isManaDrain = true;
			}
			/*
			 * 成功除外条件１：T-Uが成功したが、対象がアンデットではない。 成功除外条件２：T-Uが成功したが、対象にはターンアンデット無効。
			 * 成功除外条件３：スロー、マススロー、マナドレイン、エンタングル、イレースマジック、ウィンドシャックル無効
			 * 成功除外条件４：マナドレインが成功したが、モンスター以外の場合
			 */
			if (( ( _skillId == L1SkillId.TURN_UNDEAD ) && ( ( undeadType == 0 ) || ( undeadType == 2 ) ) ) || ( ( _skillId == L1SkillId.TURN_UNDEAD ) && ( isTU == false ) ) || ( ( ( _skillId == L1SkillId.ERASE_MAGIC ) || ( _skillId == L1SkillId.SLOW ) || ( _skillId == L1SkillId.MANA_DRAIN ) || ( _skillId == L1SkillId.MASS_SLOW ) || ( _skillId == L1SkillId.ENTANGLE ) || ( _skillId == L1SkillId.WIND_SHACKLE ) ) && ( isErase == false ) ) || ( ( _skillId == L1SkillId.MANA_DRAIN ) && ( isManaDrain == false ) ))
			{
				return true;
			}
			return false;
		}

		// 魔法屏障發動判斷
		private bool isUseCounterMagic(L1Character cha)
		{
			if (_isCounterMagic && cha.hasSkillEffect(L1SkillId.COUNTER_MAGIC))
			{
				cha.removeSkillEffect(L1SkillId.COUNTER_MAGIC);
				int castgfx = SkillsTable.Instance.getTemplate(L1SkillId.COUNTER_MAGIC).CastGfx;
				cha.broadcastPacket(new S_SkillSound(cha.Id, castgfx));
				if (cha is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance)cha;
					pc.sendPackets(new S_SkillSound(pc.Id, castgfx));
				}
				return true;
			}
			return false;
		}

	}

}