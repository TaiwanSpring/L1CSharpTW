using System;
using System.Text;
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
	using Random = LineageServer.Server.Server.Utils.Random;

	using RunnableExecuter = LineageServer.Server.Server.RunnableExecuter;
	using NPCTalkDataTable = LineageServer.Server.Server.DataSources.NPCTalkDataTable;
	using L1Attack = LineageServer.Server.Server.Model.L1Attack;
	using L1NpcTalkData = LineageServer.Server.Server.Model.L1NpcTalkData;
	using L1Quest = LineageServer.Server.Server.Model.L1Quest;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1NpcHtml = LineageServer.Server.Server.Model.Npc.L1NpcHtml;
	using S_NPCTalkReturn = LineageServer.Server.Server.serverpackets.S_NPCTalkReturn;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	// Referenced classes of package l1j.server.server.model:
	// L1NpcInstance, L1Teleport, L1NpcTalkData, L1PcInstance,
	// L1TeleporterPrices, L1TeleportLocations

	[Serializable]
	public class L1TeleporterInstance : L1NpcInstance
	{

		/// 
		private const long serialVersionUID = 1L;

		public L1TeleporterInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			L1Attack attack = new L1Attack(pc, this, skillId);
			attack.calcHit();
			attack.action();
			attack.addChaserAttack();
			attack.calcDamage();
			attack.calcStaffOfMana();
			attack.addPcPoisonAttack(pc, this);
			attack.commit();
		}

		public override void onTalkAction(L1PcInstance player)
		{
			int objid = Id;
			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
			int npcid = NpcTemplate.get_npcId();
			L1Quest quest = player.Quest;
			string htmlid = null;

			if (talking != null)
			{
				if (npcid == 50014)
				{ // ディロン
					if (player.Wizard)
					{ // ウィザード
						if (quest.get_step(L1Quest.QUEST_LEVEL30) == 1 && !player.Inventory.checkItem(40579))
						{ // アンデッドの骨
							htmlid = "dilong1";
						}
						else
						{
							htmlid = "dilong3";
						}
					}
				}
				else if (npcid == 70779)
				{ // ゲートアント
					if (player.TempCharGfx == 2437)
					{ // ジャイアントアント変身
						htmlid = "ants3";
					}
					else if (player.TempCharGfx == 2438)
					{ // ジャイアントアントソルジャー変身
						if (player.Crown)
						{ // 君主
							if (quest.get_step(L1Quest.QUEST_LEVEL30) == 1)
							{
								if (player.Inventory.checkItem(40547))
								{ // 住民たちの遺品
									htmlid = "antsn";
								}
								else
								{
									htmlid = "ants1";
								}
							}
							else
							{ // Step1以外
								htmlid = "antsn";
							}
						}
						else
						{ // 君主以外
							htmlid = "antsn";
						}
					}
				}
				else if (npcid == 70853)
				{ // フェアリープリンセス
					if (player.Elf)
					{ // エルフ
						if (quest.get_step(L1Quest.QUEST_LEVEL30) == 1)
						{
							if (!player.Inventory.checkItem(40592))
							{ // 呪われた精霊書
								if (RandomHelper.Next(100) < 50)
								{ // 50%でダークマールダンジョン
									htmlid = "fairyp2";
								}
								else
								{ // ダークエルフダンジョン
									htmlid = "fairyp1";
								}
							}
						}
					}
				}
				else if (npcid == 50031)
				{ // セピア
					if (player.Elf)
					{ // エルフ
						if (quest.get_step(L1Quest.QUEST_LEVEL45) == 2)
						{
							if (!player.Inventory.checkItem(40602))
							{ // ブルーフルート
								htmlid = "sepia1";
							}
						}
					}
				}
				else if (npcid == 50043)
				{ // ラムダ
					if (quest.get_step(L1Quest.QUEST_LEVEL50) == L1Quest.QUEST_END)
					{
						htmlid = "ramuda2";
					}
					else if (quest.get_step(L1Quest.QUEST_LEVEL50) == 1)
					{ // ディガルディン同意済み
						if (player.Crown)
						{ // 君主
							if (_isNowDely)
							{ // テレポートディレイ中
								htmlid = "ramuda4";
							}
							else
							{
								htmlid = "ramudap1";
							}
						}
						else
						{ // 君主以外
							htmlid = "ramuda1";
						}
					}
					else
					{
						htmlid = "ramuda3";
					}
				}
				// 歌う島のテレポーター
				else if (npcid == 50082)
				{
					if (player.Level < 13)
					{
						htmlid = "en0221";
					}
					else
					{
						if (player.Elf)
						{
							htmlid = "en0222e";
						}
						else if (player.Darkelf)
						{
							htmlid = "en0222d";
						}
						else
						{
							htmlid = "en0222";
						}
					}
				}
				// バルニア
				else if (npcid == 50001)
				{
					if (player.Elf)
					{
						htmlid = "barnia3";
					}
					else if (player.Knight || player.Crown)
					{
						htmlid = "barnia2";
					}
					else if (player.Wizard || player.Darkelf)
					{
						htmlid = "barnia1";
					}
				}
				else if (npcid == 81258)
				{ // 幻術士 艾希雅
					if (player.Illusionist)
					{
						htmlid = "asha1";
					}
					else
					{
						htmlid = "asha2";
					}
				}
				else if (npcid == 81259)
				{ // 龍騎士 費艾娜
					if (player.DragonKnight)
					{
						htmlid = "feaena1";
					}
					else
					{
						htmlid = "feaena2";
					}
				}
				else if (npcid == 71013)
				{ // 卡連
					if (player.Darkelf)
					{
						if (player.Level < 14)
						{
							htmlid = "karen1";
						}
						else
						{
							htmlid = "karen4";
						}
					}
					else
					{
						htmlid = "karen2";
					}
				}
				else if (npcid == 71095)
				{ // 墮落的靈魂
					if (player.Darkelf)
					{ // 黑暗妖精
						if (player.Level >= 50)
						{
							int lv50_step = quest.get_step(L1Quest.QUEST_LEVEL50);
							if (lv50_step == L1Quest.QUEST_END)
							{
								htmlid = "csoulq3";
							}
							else if (lv50_step >= 3)
							{
								bool find = false;
								foreach (object objs in L1World.Instance.getVisibleObjects(306).Values)
								{
									if (objs is L1PcInstance)
									{
										L1PcInstance _pc = (L1PcInstance) objs;
										if (_pc != null)
										{
											find = true;
											htmlid = "csoulqn"; // 你的邪念還不夠！
											break;
										}
									}
								}
								if (!find)
								{
									htmlid = "csoulq1";
								}
								else
								{
									htmlid = "csoulqn";
								}
							}
						}
					}
				}

				// html表示
				if (!string.ReferenceEquals(htmlid, null))
				{ // htmlidが指定されている場合
					player.sendPackets(new S_NPCTalkReturn(objid, htmlid));
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
			else
			{
				_log.finest((new StringBuilder()).Append("No actions for npc id : ").Append(objid).ToString());
			}
		}

		public override void onFinalAction(L1PcInstance player, string action)
		{
			int objid = Id;
			L1NpcTalkData talking = NPCTalkDataTable.Instance.getTemplate(NpcTemplate.get_npcId());
			if (action.Equals("teleportURL", StringComparison.OrdinalIgnoreCase))
			{
				L1NpcHtml html = new L1NpcHtml(talking.TeleportURL);
				player.sendPackets(new S_NPCTalkReturn(objid, html));
			}
			else if (action.Equals("teleportURLA", StringComparison.OrdinalIgnoreCase))
			{
				L1NpcHtml html = new L1NpcHtml(talking.TeleportURLA);
				player.sendPackets(new S_NPCTalkReturn(objid, html));
			}
			if (action.StartsWith("teleport ", StringComparison.Ordinal))
			{
				_log.finest((new StringBuilder()).Append("Setting action to : ").Append(action).ToString());
				doFinalAction(player, action);
			}
		}

		private void doFinalAction(L1PcInstance player, string action)
		{
			int objid = Id;

			int npcid = NpcTemplate.get_npcId();
			string htmlid = null;
			bool isTeleport = true;

			if (npcid == 50014)
			{ // ディロン
				if (!player.Inventory.checkItem(40581))
				{ // アンデッドのキー
					isTeleport = false;
					htmlid = "dilongn";
				}
			}
			else if (npcid == 50043)
			{ // ラムダ
				if (_isNowDely)
				{ // テレポートディレイ中
					isTeleport = false;
				}
			}
			else if (npcid == 50625)
			{ // 古代人（Lv50クエスト古代の空間2F）
				if (_isNowDely)
				{ // テレポートディレイ中
					isTeleport = false;
				}
			}

			if (isTeleport)
			{ // テレポート実行
				try
				{
					// ミュータントアントダンジョン(君主Lv30クエスト)
					if (action.Equals("teleport mutant-dungen", StringComparison.OrdinalIgnoreCase))
					{
						// 3マス以内のPc
						foreach (L1PcInstance otherPc in L1World.Instance.getVisiblePlayer(player, 3))
						{
							if (otherPc.Clanid == player.Clanid && otherPc.Id != player.Id)
							{
								L1Teleport.teleport(otherPc, 32740, 32800, (short) 217, 5, true);
							}
						}
						L1Teleport.teleport(player, 32740, 32800, (short) 217, 5, true);
					}
					// 試練のダンジョン（ウィザードLv30クエスト）
					else if (action.Equals("teleport mage-quest-dungen", StringComparison.OrdinalIgnoreCase))
					{
						L1Teleport.teleport(player, 32791, 32788, (short) 201, 5, true);
					}
					else if (action.Equals("teleport 29", StringComparison.OrdinalIgnoreCase))
					{ // ラムダ
						L1PcInstance kni = null;
						L1PcInstance elf = null;
						L1PcInstance wiz = null;
						// 3マス以内のPc
						foreach (L1PcInstance otherPc in L1World.Instance.getVisiblePlayer(player, 3))
						{
							L1Quest quest = otherPc.Quest;
							if (otherPc.Knight && quest.get_step(L1Quest.QUEST_LEVEL50) == 1)
							{ // ディガルディン同意済み
								if (kni == null)
								{
									kni = otherPc;
								}
							}
							else if (otherPc.Elf && quest.get_step(L1Quest.QUEST_LEVEL50) == 1)
							{ // ディガルディン同意済み
								if (elf == null)
								{
									elf = otherPc;
								}
							}
							else if (otherPc.Wizard && quest.get_step(L1Quest.QUEST_LEVEL50) == 1)
							{ // ディガルディン同意済み
								if (wiz == null)
								{
									wiz = otherPc;
								}
							}
						}
						if (kni != null && elf != null && wiz != null)
						{ // 全クラス揃っている
							L1Teleport.teleport(player, 32723, 32850, (short) 2000, 2, true);
							L1Teleport.teleport(kni, 32750, 32851, (short) 2000, 6, true);
							L1Teleport.teleport(elf, 32878, 32980, (short) 2000, 6, true);
							L1Teleport.teleport(wiz, 32876, 33003, (short) 2000, 0, true);
							TeleportDelyTimer timer = new TeleportDelyTimer(this);
							RunnableExecuter.Instance.execute(timer);
						}
					}
					else if (action.Equals("teleport barlog", StringComparison.OrdinalIgnoreCase))
					{ // 古代人（Lv50クエスト古代の空間2F）
						L1Teleport.teleport(player, 32755, 32844, (short) 2002, 5, true);
						TeleportDelyTimer timer = new TeleportDelyTimer(this);
						RunnableExecuter.Instance.execute(timer);
					}
				}
				catch (Exception)
				{
				}
			}
			if (!string.ReferenceEquals(htmlid, null))
			{ // 表示するhtmlがある場合
				player.sendPackets(new S_NPCTalkReturn(objid, htmlid));
			}
		}

		internal class TeleportDelyTimer : IRunnableStart
		{
			private readonly L1TeleporterInstance outerInstance;


			public TeleportDelyTimer(L1TeleporterInstance outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void run()
			{
				try
				{
					outerInstance._isNowDely = true;
					Thread.Sleep(900000); // 15分
				}
				catch (Exception)
				{
					outerInstance._isNowDely = false;
				}
				outerInstance._isNowDely = false;
			}
		}

		private bool _isNowDely = false;
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(LineageServer.Server.Server.Model.Instance.L1TeleporterInstance).FullName);

	}
}