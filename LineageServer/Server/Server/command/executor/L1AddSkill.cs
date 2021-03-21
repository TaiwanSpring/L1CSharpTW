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
namespace LineageServer.Server.Server.command.executor
{
	using SkillsTable = LineageServer.Server.Server.datatables.SkillsTable;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_AddSkill = LineageServer.Server.Server.serverpackets.S_AddSkill;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Skills = LineageServer.Server.Server.Templates.L1Skills;

	/// <summary>
	/// TODO: 翻譯 GM指令：增加魔法
	/// </summary>
	public class L1AddSkill : L1CommandExecutor
	{
		private L1AddSkill()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1AddSkill();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				int cnt = 0; // 計數器
				string skill_name = ""; // 技能名稱
				int skill_id = 0; // 技能ID

				int object_id = pc.Id; // キャラクタのobjectidを取得
				pc.sendPackets(new S_SkillSound(object_id, '\x00E3')); // 魔法習得的效果音效
				pc.broadcastPacket(new S_SkillSound(object_id, '\x00E3'));

				if (pc.Crown)
				{
					pc.sendPackets(new S_AddSkill(255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
					for (cnt = 1; cnt <= 16; cnt++) // LV1~2魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
					for (cnt = 113; cnt <= 120; cnt++) // プリ魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
				else if (pc.Knight)
				{
					pc.sendPackets(new S_AddSkill(255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 192, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
					for (cnt = 1; cnt <= 8; cnt++) // LV1魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
					for (cnt = 87; cnt <= 91; cnt++) // ナイト魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
				else if (pc.Elf)
				{
					pc.sendPackets(new S_AddSkill(255, 255, 127, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 127, 3, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0));
					for (cnt = 1; cnt <= 48; cnt++) // LV1~6魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
					for (cnt = 129; cnt <= 176; cnt++) // エルフ魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
				else if (pc.Wizard)
				{
					pc.sendPackets(new S_AddSkill(255, 255, 127, 255, 255, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
					for (cnt = 1; cnt <= 80; cnt++) // LV1~10魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
				else if (pc.Darkelf)
				{
					pc.sendPackets(new S_AddSkill(255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 127, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
					for (cnt = 1; cnt <= 16; cnt++) // LV1~2魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
					for (cnt = 97; cnt <= 111; cnt++) // DE魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
				else if (pc.DragonKnight)
				{
					pc.sendPackets(new S_AddSkill(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 240, 255, 7, 0, 0, 0));
					for (cnt = 181; cnt <= 195; cnt++) // ドラゴンナイト秘技
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
				else if (pc.Illusionist)
				{
					pc.sendPackets(new S_AddSkill(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 15));
					for (cnt = 201; cnt <= 220; cnt++) // イリュージョニスト魔法
					{
						L1Skills l1skills = SkillsTable.Instance.getTemplate(cnt); // 技能情報取得
						skill_name = l1skills.Name;
						skill_id = l1skills.SkillId;
						SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 寫入DB
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + " 指令錯誤。"));
			}
		}
	}

}