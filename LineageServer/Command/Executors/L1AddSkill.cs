using LineageServer.Interfaces;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// TODO: 翻譯 GM指令：增加魔法
    /// </summary>
    class L1AddSkill : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
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