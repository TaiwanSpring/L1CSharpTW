using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來使用魔法的封包
    /// </summary>
    class C_UseSkill : ClientBasePacket
    {
        public C_UseSkill(byte[] abyte0, ClientThread client) : base(abyte0)
        {
            L1PcInstance pc = client.ActiveChar;
            if ((pc == null) || pc.Teleport || pc.Dead)
            {
                return;
            }

            int row = ReadC();
            int column = ReadC();
            int skillId = (row * 8) + column + 1;
            string charName = null;
            string message = null;
            int targetId = 0;
            int targetX = 0;
            int targetY = 0;

            if (!pc.Map.UsableSkill)
            {
                pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
                return;
            }
            if (!pc.isSkillMastery(skillId))
            {
                return;
            }

            // 檢查使用魔法的間隔
            if (Config.CHECK_SPELL_INTERVAL)
            {
                int result;
                // FIXME 判斷有向及無向的魔法
                if (SkillsTable.Instance.getTemplate(skillId).ActionId == ActionCodes.ACTION_SkillAttack)
                {
                    result = pc.AcceleratorChecker.checkInterval(AcceleratorChecker.ACT_TYPE.SPELL_DIR);
                }
                else
                {
                    result = pc.AcceleratorChecker.checkInterval(AcceleratorChecker.ACT_TYPE.SPELL_NODIR);
                }
                if (result == AcceleratorChecker.R_DISPOSED)
                {
                    return;
                }
            }

            if (abyte0.Length > 4)
            {
                try
                {
                    if ((skillId == L1SkillId.CALL_CLAN) || (skillId == L1SkillId.RUN_CLAN))
                    { // コールクラン、ランクラン
                        charName = ReadS();
                    }
                    else if (skillId == L1SkillId.TRUE_TARGET)
                    { // トゥルーターゲット
                        targetId = ReadD();
                        targetX = ReadH();
                        targetY = ReadH();
                        message = ReadS();
                    }
                    else if ((skillId == L1SkillId.TELEPORT) || (skillId == L1SkillId.MASS_TELEPORT))
                    { // テレポート、マステレポート
                        ReadH(); // MapID
                        targetId = ReadD(); // Bookmark ID
                    }
                    else if ((skillId == L1SkillId.FIRE_WALL) || (skillId == L1SkillId.LIFE_STREAM))
                    { // ファイアーウォール、ライフストリーム
                        targetX = ReadH();
                        targetY = ReadH();
                    }
                    else if (skillId == L1SkillId.SUMMON_MONSTER)
                    { // 法師魔法 (召喚術)
                        if (pc.Inventory is L1PcInventory pcInventory && pcInventory.checkEquipped(20284))
                        { // 有裝備召喚戒指
                            int summonId = ReadD();
                            pc.SummonId = summonId;
                        }
                        else
                        {
                            targetId = ReadD();
                        }
                    }
                    else
                    {
                        targetId = ReadD();
                        targetX = ReadH();
                        targetY = ReadH();
                    }
                }
                catch (Exception)
                {
                    // _log.log(Enum.Level.Server, "", e);
                }
            }

            if (pc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
            { // 取消絕對屏障
                pc.removeSkillEffect(L1SkillId.ABSOLUTE_BARRIER);
            }
            if (pc.hasSkillEffect(L1SkillId.MEDITATION))
            { // 取消冥想效果
                pc.removeSkillEffect(L1SkillId.MEDITATION);
            }

            try
            {
                if ((skillId == L1SkillId.CALL_CLAN) || (skillId == L1SkillId.RUN_CLAN))
                { // コールクラン、ランクラン
                    if (charName.Length == 0)
                    {
                        // 名前が空の場合クライアントで弾かれるはず
                        return;
                    }

                    L1PcInstance target = L1World.Instance.getPlayer(charName);

                    if (target == null)
                    {
                        // メッセージが正確であるか未調査
                        pc.sendPackets(new S_ServerMessage(73, charName)); // \f1%0はゲームをしていません。
                        return;
                    }
                    if (pc.Clanid != target.Clanid)
                    {
                        pc.sendPackets(new S_ServerMessage(414)); // 同じ血盟員ではありません。
                        return;
                    }
                    targetId = target.Id;
                    if (skillId == L1SkillId.CALL_CLAN)
                    {
                        // 移動せずに連続して同じクラン員にコールクランした場合、向きは前回の向きになる
                        int callClanId = pc.CallClanId;
                        if ((callClanId == 0) || (callClanId != targetId))
                        {
                            pc.CallClanId = targetId;
                            pc.CallClanHeading = pc.Heading;
                        }
                    }
                }
                L1SkillUse l1skilluse = new L1SkillUse();
                l1skilluse.handleCommands(pc, skillId, targetId, targetX, targetY, message, 0, L1SkillUse.TYPE_NORMAL);

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                System.Console.Write(e.StackTrace);
            }
        }
    }

}