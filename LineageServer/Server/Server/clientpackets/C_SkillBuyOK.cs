
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來買魔法OK的封包
	/// </summary>
	class C_SkillBuyOK : ClientBasePacket
	{

		private const string C_SKILL_BUY_OK = "[C] C_SkillBuyOK";

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public C_SkillBuyOK(byte abyte0[], l1j.server.server.ClientThread clientthread) throws Exception
		public C_SkillBuyOK(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}

			int count = readH();
			int[] sid = new int[count];
			int price = 0;
			int level1 = 0;
			int level2 = 0;
			int level3 = 0;
			int level1_cost = 0;
			int level2_cost = 0;
			int level3_cost = 0;
			string skill_name = null;
			int skill_id = 0;

			for (int i = 0; i < count; i++)
			{
				sid[i] = readD();
				switch (sid[i])
				{
					// Lv1魔法
					case 0:
						level1 += 1;
						level1_cost += 100;
						break;
					case 1:
						level1 += 2;
						level1_cost += 100;
						break;
					case 2:
						level1 += 4;
						level1_cost += 100;
						break;
					case 3:
						level1 += 8;
						level1_cost += 100;
						break;
					case 4:
						level1 += 16;
						level1_cost += 100;
						break;
					case 5:
						level1 += 32;
						level1_cost += 100;
						break;
					case 6:
						level1 += 64;
						level1_cost += 100;
						break;
					case 7:
						level1 += 128;
						level1_cost += 100;
						break;

					// Lv2魔法
					case 8:
						level2 += 1;
						level2_cost += 400;
						break;
					case 9:
						level2 += 2;
						level2_cost += 400;
						break;
					case 10:
						level2 += 4;
						level2_cost += 400;
						break;
					case 11:
						level2 += 8;
						level2_cost += 400;
						break;
					case 12:
						level2 += 16;
						level2_cost += 400;
						break;
					case 13:
						level2 += 32;
						level2_cost += 400;
						break;
					case 14:
						level2 += 64;
						level2_cost += 400;
						break;
					case 15:
						level2 += 128;
						level2_cost += 400;
						break;

					// Lv3魔法
					case 16:
						level3 += 1;
						level3_cost += 900;
						break;
					case 17:
						level3 += 2;
						level3_cost += 900;
						break;
					case 18:
						level3 += 4;
						level3_cost += 900;
						break;
					case 19:
						level3 += 8;
						level3_cost += 900;
						break;
					case 20:
						level3 += 16;
						level3_cost += 900;
						break;
					case 21:
						level3 += 32;
						level3_cost += 900;
						break;
					case 22:
						level3 += 64;
						level3_cost += 900;
						break;
					case 23:
						level3 += 128;
						level3_cost += 900;
						break;

					default:
						break;
				}
			}

			if (!pc.Gm)
			{
				switch (pc.Type)
				{
					case 0: // 君主
						if (pc.Level < 10)
						{
							level1 = 0;
							level1_cost = 0;
						}
						if (pc.Level < 20)
						{
							level2 = 0;
							level2_cost = 0;
						}
						level3 = 0;
						level3_cost = 0;
						break;

					case 1: // ナイト
						if (pc.Level < 50)
						{
							level1 = 0;
							level1_cost = 0;
						}
						level2 = 0;
						level2_cost = 0;
						level3 = 0;
						level3_cost = 0;
						break;

					case 2: // エルフ
						if (pc.Level < 8)
						{
							level1 = 0;
							level1_cost = 0;
						}
						if (pc.Level < 16)
						{
							level2 = 0;
							level2_cost = 0;
						}
						if (pc.Level < 24)
						{
							level3 = 0;
							level3_cost = 0;
						}
						break;

					case 3: // WIZ
						if (pc.Level < 4)
						{
							level1 = 0;
							level1_cost = 0;
						}
						if (pc.Level < 8)
						{
							level2 = 0;
							level2_cost = 0;
						}
						if (pc.Level < 12)
						{
							level3 = 0;
							level3_cost = 0;
						}
						break;

					case 4: // DE
						if (pc.Level < 12)
						{
							level1 = 0;
							level1_cost = 0;
						}
						if (pc.Level < 24)
						{
							level2 = 0;
							level2_cost = 0;
						}
						level3 = 0;
						level3_cost = 0;
						break;

					default:
						break;
				}
			}

			if ((level1 == 0) && (level2 == 0) && (level3 == 0))
			{
				return;
			}
			price = level1_cost + level2_cost + level3_cost;
			if (pc.Inventory.checkItem(L1ItemId.ADENA, price))
			{
				pc.Inventory.consumeItem(L1ItemId.ADENA, price);
				S_SkillSound s_skillSound = new S_SkillSound(pc.Id, 224);
				pc.sendPackets(s_skillSound);
				pc.broadcastPacket(s_skillSound);
				pc.sendPackets(new S_AddSkill(level1, level2, level3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));

				if ((level1 & 1) == 1)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(1);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 2) == 2)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(2);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 4) == 4)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(3);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 8) == 8)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(4);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 16) == 16)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(5);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 32) == 32)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(6);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 64) == 64)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(7);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level1 & 128) == 128)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(8);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}

				if ((level2 & 1) == 1)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(9);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 2) == 2)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(10);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 4) == 4)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(11);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 8) == 8)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(12);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 16) == 16)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(13);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 32) == 32)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(14);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 64) == 64)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(15);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level2 & 128) == 128)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(16);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}

				if ((level3 & 1) == 1)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(17);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level3 & 2) == 2)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(18);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level3 & 4) == 4)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(19);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level3 & 8) == 8)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(20);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level3 & 16) == 16)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(21);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level3 & 32) == 32)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(22);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
				if ((level3 & 64) == 64)
				{
					L1Skills l1skills = SkillsTable.Instance.getTemplate(23);
					skill_name = l1skills.Name;
					skill_id = l1skills.SkillId;
					SkillsTable.Instance.spellMastery(pc.Id, skill_id, skill_name, 0, 0);
				}
			}
			else
			{
				pc.sendPackets(new S_ServerMessage(189)); // \f1アデナが不足しています。
			}
		}

		public override string Type
		{
			get
			{
				return C_SKILL_BUY_OK;
			}
		}

	}

}