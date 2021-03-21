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
//	import static l1j.server.server.model.skill.L1SkillId.FOG_OF_SLEEPING;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using NPCTalkDataTable = LineageServer.Server.Server.datatables.NPCTalkDataTable;
	using L1Attack = LineageServer.Server.Server.Model.L1Attack;
	using L1CastleLocation = LineageServer.Server.Server.Model.L1CastleLocation;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1NpcTalkData = LineageServer.Server.Server.Model.L1NpcTalkData;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_NPCTalkReturn = LineageServer.Server.Server.serverpackets.S_NPCTalkReturn;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using Point = LineageServer.Server.Server.Types.Point;

	[Serializable]
	public class L1GuardInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;

		// ターゲットを探す
		public override void searchTarget()
		{
			// ターゲット捜索
			L1PcInstance targetPlayer = null;
			foreach (L1PcInstance pc in L1World.Instance.getVisiblePlayer(this))
			{
				if ((pc.CurrentHp <= 0) || pc.Dead || pc.Gm || pc.Ghost)
				{
					continue;
				}
				if (!pc.Invisble || NpcTemplate.is_agrocoi()) // インビジチェック
				{
					if (pc.Wanted)
					{ // PKで手配中か
						targetPlayer = pc;
						break;
					}
				}
			}
			if (targetPlayer != null)
			{
				_hateList.add(targetPlayer, 0);
				_target = targetPlayer;
			}
		}

		public virtual L1PcInstance Target
		{
			set
			{
				if (value != null)
				{
					_hateList.add(value, 0);
					_target = value;
				}
			}
		}

		// ターゲットがいない場合の処理
		public override bool noTarget()
		{
			if (Location.getTileLineDistance(new Point(HomeX, HomeY)) > 0)
			{
				int dir = moveDirection(HomeX, HomeY);
				if (dir != -1)
				{
					DirectionMove = dir;
					SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
				}
				else // 遠すぎるor経路が見つからない場合はテレポートして帰る
				{
					teleport(HomeX, HomeY, 1);
				}
			}
			else
			{
				if (L1World.Instance.getRecognizePlayer(this).Count == 0)
				{
					return true; // 周りにプレイヤーがいなくなったらＡＩ処理終了
				}
			}
			return false;
		}

		public L1GuardInstance(L1Npc template) : base(template)
		{
		}

		public override void onNpcAI()
		{
			if (AiRunning)
			{
				return;
			}
			Actived = false;
			startAI();
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			if (!Dead)
			{
				if (CurrentHp > 0)
				{
					L1Attack attack = new L1Attack(pc, this, skillId);
					if (attack.calcHit())
					{
						attack.calcDamage();
						attack.calcStaffOfMana();
						attack.addPcPoisonAttack(pc, this);
						attack.addChaserAttack();
					}
					attack.action();
					attack.commit();
				}
				else
				{
					L1Attack attack = new L1Attack(pc, this, skillId);
					attack.calcHit();
					attack.action();
				}
			}
		}

		public override void onTalkAction(L1PcInstance player)
		{
			int objid = Id;
			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
			int npcid = NpcTemplate.get_npcId();
			string htmlid = null;
			string[] htmldata = null;
			bool hascastle = false;
			string clan_name = "";
			string pri_name = "";

			if (talking != null)
			{
				// キーパー
				if ((npcid == 70549) || (npcid == 70985))
				{ // ケント城右外門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.KENT_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gateokeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if (npcid == 70656)
				{ // ケント城内門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.KENT_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gatekeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if ((npcid == 70600) || (npcid == 70986))
				{
					hascastle = checkHasCastle(player, L1CastleLocation.OT_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "orckeeper";
					}
					else
					{
						htmlid = "orckeeperop";
					}
				}
				else if ((npcid == 70687) || (npcid == 70987))
				{
					hascastle = checkHasCastle(player, L1CastleLocation.WW_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gateokeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if (npcid == 70778)
				{ // ウィンダウッド城内門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.WW_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gatekeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if ((npcid == 70800) || (npcid == 70988) || (npcid == 70989) || (npcid == 70990) || (npcid == 70991))
				{
					hascastle = checkHasCastle(player, L1CastleLocation.GIRAN_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gateokeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if (npcid == 70817)
				{ // ギラン城内門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.GIRAN_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gatekeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if ((npcid == 70862) || (npcid == 70992))
				{
					hascastle = checkHasCastle(player, L1CastleLocation.HEINE_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gateokeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if (npcid == 70863)
				{ // ハイネ城内門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.HEINE_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gatekeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if ((npcid == 70993) || (npcid == 70994))
				{
					hascastle = checkHasCastle(player, L1CastleLocation.DOWA_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gateokeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if (npcid == 70995)
				{ // ドワーフ城内門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.DOWA_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gatekeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}
				else if (npcid == 70996)
				{ // アデン城内門キーパー
					hascastle = checkHasCastle(player, L1CastleLocation.ADEN_CASTLE_ID);
					if (hascastle)
					{ // 城主クラン員
						htmlid = "gatekeeper";
						htmldata = new string[] {player.Name};
					}
					else
					{
						htmlid = "gatekeeperop";
					}
				}

				// 近衛兵
				else if (npcid == 60514)
				{ // ケント城近衛兵
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.KENT_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "ktguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if (npcid == 60560)
				{ // オーク近衛兵
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.OT_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "orcguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if (npcid == 60552)
				{ // ウィンダウッド城近衛兵
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.WW_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "wdguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if ((npcid == 60524) || (npcid == 60525) || (npcid == 60529))
				{ // ギラン城近衛兵
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.GIRAN_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "grguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if (npcid == 70857)
				{ // ハイネ城ハイネ ガード
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.HEINE_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "heguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if ((npcid == 60530) || (npcid == 60531))
				{
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.DOWA_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "dcguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if ((npcid == 60533) || (npcid == 60534))
				{
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.ADEN_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "adguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}
				else if (npcid == 81156)
				{ // アデン偵察兵（ディアド要塞）
					foreach (L1Clan clan in L1World.Instance.AllClans)
					{
						if (clan.CastleId == L1CastleLocation.DIAD_CASTLE_ID)
						{
							clan_name = clan.ClanName;
							pri_name = clan.LeaderName;
							break;
						}
					}
					htmlid = "ktguard6";
					htmldata = new string[] {Name, clan_name, pri_name};
				}

				// html表示パケット送信
				if (!string.ReferenceEquals(htmlid, null))
				{ // htmlidが指定されている場合
					if (htmldata != null)
					{ // html指定がある場合は表示
						player.sendPackets(new S_NPCTalkReturn(objid, htmlid, htmldata));
					}
					else
					{
						player.sendPackets(new S_NPCTalkReturn(objid, htmlid));
					}
				}
				else
				{
					if (player.Lawful < -1000)
					{ // プレイヤーがカオティック
						player.sendPackets(new S_NPCTalkReturn(talking, objid, 2));
					}
					else
					{
						player.sendPackets(new S_NPCTalkReturn(talking, objid, 1));
					}
				}
			}
		}

		public virtual void onFinalAction()
		{

		}

		public virtual void doFinalAction()
		{

		}

		public override L1Character Link
		{
			set
			{
				if ((value != null) && _hateList.Empty)
				{
					_hateList.add(value, 0);
					checkTarget();
				}
			}
		}

		public override void receiveDamage(L1Character attacker, int damage)
		{ // 攻撃でＨＰを減らすときはここを使用
			if ((CurrentHp > 0) && !Dead)
			{
				if (damage >= 0)
				{
					if (!(attacker is L1EffectInstance))
					{ // FWはヘイトなし
						setHate(attacker, damage);
					}
				}
				if (damage > 0)
				{
					removeSkillEffect(FOG_OF_SLEEPING);
				}

				onNpcAI();

				if ((attacker is L1PcInstance) && (damage > 0))
				{
					L1PcInstance pc = (L1PcInstance) attacker;
					pc.PetTarget = this;
					serchLink(pc, NpcTemplate.get_family());
				}

				int newHp = CurrentHp - damage;
				if ((newHp <= 0) && !Dead)
				{
					CurrentHpDirect = 0;
					Dead = true;
					Status = ActionCodes.ACTION_Die;
					Death death = new Death(this, attacker);
					GeneralThreadPool.Instance.execute(death);
				}
				if (newHp > 0)
				{
					CurrentHp = newHp;
				}
			}
			else if ((CurrentHp == 0) && !Dead)
			{
			}
			else if (!Dead)
			{ // 念のため
				Dead = true;
				Status = ActionCodes.ACTION_Die;
				Death death = new Death(this, attacker);
				GeneralThreadPool.Instance.execute(death);
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
    
				if (MaxHp > CurrentHp)
				{
					startHpRegeneration();
				}
			}
		}

		internal class Death : IRunnableStart
		{
			private readonly L1GuardInstance outerInstance;

			internal L1Character _lastAttacker;

			public Death(L1GuardInstance outerInstance, L1Character lastAttacker)
			{
				this.outerInstance = outerInstance;
				_lastAttacker = lastAttacker;
			}

			public override void run()
			{
				outerInstance.DeathProcessing = true;
				outerInstance.CurrentHpDirect = 0;
				outerInstance.Dead = true;
				outerInstance.Status = ActionCodes.ACTION_Die;

				outerInstance.Map.setPassable(outerInstance.Location, true);

				outerInstance.broadcastPacket(new S_DoActionGFX(outerInstance.Id, ActionCodes.ACTION_Die));

				outerInstance.startChat(CHAT_TIMING_DEAD);

				outerInstance.DeathProcessing = false;

				outerInstance.allTargetClear();

				outerInstance.startDeleteTimer();
			}
		}

		private bool checkHasCastle(L1PcInstance pc, int castleId)
		{
			bool isExistDefenseClan = false;
			foreach (L1Clan clan in L1World.Instance.AllClans)
			{
				if (castleId == clan.CastleId)
				{
					isExistDefenseClan = true;
					break;
				}
			}
			if (!isExistDefenseClan)
			{ // 城主クランが居ない
				return true;
			}

			if (pc.Clanid != 0)
			{ // クラン所属中
				L1Clan clan = L1World.Instance.getClan(pc.Clanname);
				if (clan != null)
				{
					if (clan.CastleId == castleId)
					{
						return true;
					}
				}
			}
			return false;
		}

	}

}