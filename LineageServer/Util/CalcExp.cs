using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System.Collections.Generic;
using LineageServer.Models;

namespace LineageServer.Utils
{
	class CalcExp
	{

		private const long serialVersionUID = 1L;

		private static ILogger _log = Logger.GetLogger(nameof(CalcExp));

		public static readonly long MAX_EXP = ExpTable.getExpByLevel(111) - 1;

		private CalcExp()
		{
		}
		private static L1NpcInstance _npc = null; //TODO 殷海薩的祝福
		public static long Serialversionuid
		{
			get
			{
				return serialVersionUID;
			}
		}

		public static void calcExp(L1PcInstance l1pcinstance, int targetid, IList<L1Character> acquisitorList, IList<int> hateList, long exp)
		{

			int i = 0;
			double party_level = 0;
			double dist = 0;
			long member_exp = 0;
			int member_lawful = 0;
			GameObject l1object = L1World.Instance.findObject(targetid);
			L1NpcInstance npc = (L1NpcInstance)l1object;

			// ヘイトの合計を取得
			L1Character acquisitor;
			int hate = 0;
			long acquire_exp = 0;
			int acquire_lawful = 0;
			long party_exp = 0;
			int party_lawful = 0;
			long totalHateExp = 0;
			int totalHateLawful = 0;
			long partyHateExp = 0;
			int partyHateLawful = 0;
			long ownHateExp = 0;

			if (acquisitorList.Count != hateList.Count)
			{
				return;
			}
			for (i = hateList.Count - 1; i >= 0; i--)
			{
				acquisitor = acquisitorList[i];
				hate = hateList[i];
				if (( acquisitor != null ) && !acquisitor.Dead)
				{
					totalHateExp += hate;
					if (acquisitor is L1PcInstance)
					{
						totalHateLawful += hate;
					}
				}
				else
				{ // nullだったり死んでいたら排除
					acquisitorList.RemoveAt(i);
					hateList.RemoveAt(i);
				}
			}
			if (totalHateExp == 0)
			{ // 取得者がいない場合
				return;
			}

			if (( l1object != null ) && !( npc is L1PetInstance ) && !( npc is L1SummonInstance ))
			{
				// int exp = npc.get_exp();
				/*if (!L1World.getInstance().isProcessingContributionTotal() && (l1pcinstance.getHomeTownId() > 0)) {
					int contribution = npc.getLevel() / 10;
					l1pcinstance.addContribution(contribution);
				}*/
				// 取消由打怪獲得村莊貢獻度，改由製作村莊福利品獲得貢獻度 for 3.3C
				int lawful = npc.Lawful;

				if (l1pcinstance.InParty)
				{ // パーティー中
				  // パーティーのヘイトの合計を算出
				  // パーティーメンバー以外にはそのまま配分
					partyHateExp = 0;
					partyHateLawful = 0;
					for (i = hateList.Count - 1; i >= 0; i--)
					{
						acquisitor = acquisitorList[i];
						hate = hateList[i];
						if (acquisitor is L1PcInstance)
						{
							L1PcInstance pc = (L1PcInstance)acquisitor;
							if (pc == l1pcinstance)
							{
								partyHateExp += hate;
								partyHateLawful += hate;
							}
							else if (l1pcinstance.Party.isMember(pc))
							{
								partyHateExp += hate;
								partyHateLawful += hate;
							}
							else
							{
								if (totalHateExp > 0)
								{
									acquire_exp = ( exp * hate / totalHateExp );
								}
								if (totalHateLawful > 0)
								{
									acquire_lawful = ( lawful * hate / totalHateLawful );
								}
								AddExp(pc, acquire_exp, acquire_lawful);
							}
						}
						else if (acquisitor is L1PetInstance)
						{
							L1PetInstance pet = (L1PetInstance)acquisitor;
							L1PcInstance master = (L1PcInstance)pet.Master;
							if (master == l1pcinstance)
							{
								partyHateExp += hate;
							}
							else if (l1pcinstance.Party.isMember(master))
							{
								partyHateExp += hate;
							}
							else
							{
								if (totalHateExp > 0)
								{
									acquire_exp = ( exp * hate / totalHateExp );
								}
								AddExpPet(pet, acquire_exp);
							}
						}
						else if (acquisitor is L1SummonInstance)
						{
							L1SummonInstance summon = (L1SummonInstance)acquisitor;
							L1PcInstance master = (L1PcInstance)summon.Master;
							if (master == l1pcinstance)
							{
								partyHateExp += hate;
							}
							else if (l1pcinstance.Party.isMember(master))
							{
								partyHateExp += hate;
							}
							else
							{
							}
						}
					}
					if (totalHateExp > 0)
					{
						party_exp = ( exp * partyHateExp / totalHateExp );
					}
					if (totalHateLawful > 0)
					{
						party_lawful = ( lawful * partyHateLawful / totalHateLawful );
					}

					// EXP、ロウフル配分

					// プリボーナス
					double pri_bonus = 0;
					L1PcInstance leader = l1pcinstance.Party.Leader;
					if (leader.Crown && ( l1pcinstance.knownsObject(leader) || l1pcinstance.Equals(leader) ))
					{
						pri_bonus = 0.059;
					}

					// PT経験値の計算
					L1PcInstance[] ptMembers = l1pcinstance.Party.Members;
					double pt_bonus = 0;
					foreach (L1PcInstance each in ptMembers)
					{
						if (l1pcinstance.knownsObject(each) || l1pcinstance.Equals(each))
						{
							party_level += each.Level * each.Level;
						}
						if (l1pcinstance.knownsObject(each))
						{
							pt_bonus += 0.04;
						}
					}

					party_exp = (long)( party_exp * ( 1 + pt_bonus + pri_bonus ) );

					// 自キャラクターとそのペット・サモンのヘイトの合計を算出
					if (party_level > 0)
					{
						dist = ( ( l1pcinstance.Level * l1pcinstance.Level ) / party_level );
					}
					member_exp = (long)( party_exp * dist );
					member_lawful = (int)( party_lawful * dist );

					ownHateExp = 0;
					for (i = hateList.Count - 1; i >= 0; i--)
					{
						acquisitor = acquisitorList[i];
						hate = hateList[i];
						if (acquisitor is L1PcInstance)
						{
							L1PcInstance pc = (L1PcInstance)acquisitor;
							if (pc == l1pcinstance)
							{
								ownHateExp += hate;
							}
						}
						else if (acquisitor is L1PetInstance)
						{
							L1PetInstance pet = (L1PetInstance)acquisitor;
							L1PcInstance master = (L1PcInstance)pet.Master;
							if (master == l1pcinstance)
							{
								ownHateExp += hate;
							}
						}
						else if (acquisitor is L1SummonInstance)
						{
							L1SummonInstance summon = (L1SummonInstance)acquisitor;
							L1PcInstance master = (L1PcInstance)summon.Master;
							if (master == l1pcinstance)
							{
								ownHateExp += hate;
							}
						}
					}
					// 自キャラクターとそのペット・サモンに分配
					if (ownHateExp != 0)
					{ // 攻撃に参加していた
						for (i = hateList.Count - 1; i >= 0; i--)
						{
							acquisitor = acquisitorList[i];
							hate = hateList[i];
							if (acquisitor is L1PcInstance)
							{
								L1PcInstance pc = (L1PcInstance)acquisitor;
								if (pc == l1pcinstance)
								{
									if (ownHateExp > 0)
									{
										acquire_exp = ( member_exp * hate / ownHateExp );
									}
									AddExp(pc, acquire_exp, member_lawful);
								}
							}
							else if (acquisitor is L1PetInstance)
							{
								L1PetInstance pet = (L1PetInstance)acquisitor;
								L1PcInstance master = (L1PcInstance)pet.Master;
								if (master == l1pcinstance)
								{
									if (ownHateExp > 0)
									{
										acquire_exp = ( member_exp * hate / ownHateExp );
									}
									AddExpPet(pet, acquire_exp);
								}
							}
							else if (acquisitor is L1SummonInstance)
							{
							}
						}
					}
					else
					{ // 攻撃に参加していなかった
					  // 自キャラクターのみに分配
						AddExp(l1pcinstance, member_exp, member_lawful);
					}

					// パーティーメンバーとそのペット・サモンのヘイトの合計を算出
					foreach (L1PcInstance ptMember in ptMembers)
					{
						if (l1pcinstance.knownsObject(ptMember))
						{
							if (party_level > 0)
							{
								dist = ( ( ptMember.Level * ptMember.Level ) / party_level );
							}
							member_exp = (int)( party_exp * dist );
							member_lawful = (int)( party_lawful * dist );

							ownHateExp = 0;
							for (i = hateList.Count - 1; i >= 0; i--)
							{
								acquisitor = acquisitorList[i];
								hate = hateList[i];
								if (acquisitor is L1PcInstance)
								{
									L1PcInstance pc = (L1PcInstance)acquisitor;
									if (pc == ptMember)
									{
										ownHateExp += hate;
									}
								}
								else if (acquisitor is L1PetInstance)
								{
									L1PetInstance pet = (L1PetInstance)acquisitor;
									L1PcInstance master = (L1PcInstance)pet.Master;
									if (master == ptMember)
									{
										ownHateExp += hate;
									}
								}
								else if (acquisitor is L1SummonInstance)
								{
									L1SummonInstance summon = (L1SummonInstance)acquisitor;
									L1PcInstance master = (L1PcInstance)summon.Master;
									if (master == ptMember)
									{
										ownHateExp += hate;
									}
								}
							}
							// パーティーメンバーとそのペット・サモンに分配
							if (ownHateExp != 0)
							{ // 攻撃に参加していた
								for (i = hateList.Count - 1; i >= 0; i--)
								{
									acquisitor = acquisitorList[i];
									hate = hateList[i];
									if (acquisitor is L1PcInstance)
									{
										L1PcInstance pc = (L1PcInstance)acquisitor;
										if (pc == ptMember)
										{
											if (ownHateExp > 0)
											{
												acquire_exp = ( member_exp * hate / ownHateExp );
											}
											AddExp(pc, acquire_exp, member_lawful);
										}
									}
									else if (acquisitor is L1PetInstance)
									{
										L1PetInstance pet = (L1PetInstance)acquisitor;
										L1PcInstance master = (L1PcInstance)pet.Master;
										if (master == ptMember)
										{
											if (ownHateExp > 0)
											{
												acquire_exp = ( member_exp * hate / ownHateExp );
											}
											AddExpPet(pet, acquire_exp);
										}
									}
									else if (acquisitor is L1SummonInstance)
									{
									}
								}
							}
							else
							{ // 攻撃に参加していなかった
							  // パーティーメンバーのみに分配
								AddExp(ptMember, member_exp, member_lawful);
							}
						}
					}
				}
				else
				{ // パーティーを組んでいない
				  // EXP、ロウフルの分配
					for (i = hateList.Count - 1; i >= 0; i--)
					{
						acquisitor = acquisitorList[i];
						hate = hateList[i];
						acquire_exp = ( exp * hate / totalHateExp );
						if (acquisitor is L1PcInstance)
						{
							if (totalHateLawful > 0)
							{
								acquire_lawful = ( lawful * hate / totalHateLawful );
							}
						}

						if (acquisitor is L1PcInstance)
						{
							L1PcInstance pc = (L1PcInstance)acquisitor;
							AddExp(pc, acquire_exp, acquire_lawful);
						}
						else if (acquisitor is L1PetInstance)
						{
							L1PetInstance pet = (L1PetInstance)acquisitor;
							AddExpPet(pet, acquire_exp);
						}
						else if (acquisitor is L1SummonInstance)
						{
						}
					}
				}
			}
		}

		private static void AddExp(L1PcInstance pc, long exp, int lawful)
		{

			int add_lawful = (int)( lawful * Config.RATE_LA ) * -1;
			pc.addLawful(add_lawful);

			double exppenalty = ExpTable.getPenaltyRate(pc.Level);
			double foodBonus = 1.0;
			double expBonus = 1.0;
			double ainBonus = 1.0; // TODO 殷海薩的祝福
								   // 魔法料理經驗加成
			if (pc.hasSkillEffect(L1SkillId.COOKING_1_7_N) || pc.hasSkillEffect(L1SkillId.COOKING_1_7_S))
			{
				foodBonus = 1.01;
			}
			if (pc.hasSkillEffect(L1SkillId.COOKING_2_7_N) || pc.hasSkillEffect(L1SkillId.COOKING_2_7_S))
			{
				foodBonus = 1.02;
			}
			if (pc.hasSkillEffect(L1SkillId.COOKING_3_7_N) || pc.hasSkillEffect(L1SkillId.COOKING_3_7_S))
			{
				foodBonus = 1.03;
			}
			// 戰鬥藥水、神力藥水經驗加成
			if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_BATTLE))
			{
				expBonus = 1.2;
			}
			else if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_EXP_150))
			{
				expBonus = 2.5;
			}
			else if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_EXP_175))
			{
				expBonus = 2.75;
			}
			else if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_EXP_200))
			{
				expBonus = 3.0;
			}
			else if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_EXP_225))
			{
				expBonus = 3.25;
			}
			else if (pc.hasSkillEffect(L1SkillId.EFFECT_POTION_OF_EXP_250))
			{
				expBonus = 3.5;
			}
			// TODO 殷海薩的祝福 計算公式仍需驗證
			if (pc.AinPoint > 0)
			{
				if (!( _npc is L1PetInstance || _npc is L1SummonInstance || _npc is L1ScarecrowInstance ))
				{
					//TODO 木人寵物召喚不計算加成
					pc.AinPoint = pc.AinPoint - 1;
					pc.sendPackets(new S_SkillIconExp(pc.AinPoint));
					ainBonus = 1.77; //TODO 額外的經驗 77%
				}
				// TODO 殷海薩的祝福 計算公式仍需驗證
			}
			long add_exp = (long)( exp * exppenalty * Config.RATE_XP * foodBonus * expBonus * ainBonus );
			// TODO 暴等洗血修正
			if (add_exp < 0)
			{ //TODO 經驗值大於2147483647會變負值導致錯誤.
				add_exp = 0;
			}
			else if (add_exp > 36065092)
			{ //TODO 升一級所需要的EXP=36065092
				add_exp = 36065092;
			}
			pc.addExp(add_exp);
			pc.addMonsKill();
		}

		private static void AddExpPet(L1PetInstance pet, long exp)
		{
			L1PcInstance pc = (L1PcInstance)pet.Master;

			int petItemObjId = pet.ItemObjId;

			int levelBefore = pet.Level;
			//TODO 寵物經驗倍率
			long totalExp = (long)( exp * Config.RATE_XP_PET + pet.Exp );
			//TODO 寵物經驗倍率

			//TODO 寵物最高等級
			if (totalExp >= ExpTable.getExpByLevel(Config.Pet_Max_LV + 1))
			{
				totalExp = ExpTable.getExpByLevel(Config.Pet_Max_LV + 1) - 1;
				//TODO 寵物最高等級
			}
			pet.Exp = totalExp;

			pet.Level = ExpTable.getLevelByExp(totalExp);

			int expPercentage = ExpTable.getExpPercentage(pet.Level, totalExp);

			int gap = pet.Level - levelBefore;
			for (int i = 1; i <= gap; i++)
			{
				IntRange hpUpRange = pet.PetType.HpUpRange;
				IntRange mpUpRange = pet.PetType.MpUpRange;
				pet.addMaxHp(hpUpRange.randomValue());
				pet.addMaxMp(mpUpRange.randomValue());
			}

			pet.ExpPercent = expPercentage;
			pc.sendPackets(new S_PetPack(pet, pc));

			if (gap != 0)
			{ // レベルアップしたらDBに書き込む
				L1Pet petTemplate = PetTable.Instance.getTemplate(petItemObjId);
				if (petTemplate == null)
				{ // PetTableにない
					_log.Warning("L1Pet == null");
					return;
				}
				petTemplate.set_exp((int)pet.Exp);
				petTemplate.set_level(pet.Level);
				petTemplate.set_hp(pet.MaxHp);
				petTemplate.set_mp(pet.MaxMp);
				PetTable.Instance.storePet(petTemplate); // DBに書き込み
				pc.sendPackets(new S_ServerMessage(320, pet.Name)); // \f1%0のレベルが上がりました。
			}
		}
	}
}