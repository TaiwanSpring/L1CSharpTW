using LineageServer.Interfaces;
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
    /// 處理收到由客戶端傳來的聊天封包
    /// </summary>
    class C_Chat : ClientBasePacket
    {

        private const string C_CHAT = "[C] C_Chat";

        public C_Chat(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            if (clientthread.ActiveChar == null)
            {
                return;
            }

            L1PcInstance pc = clientthread.ActiveChar;

            int chatType = ReadC();
            string chatText = ReadS();
            // 修正對話出現太長的字串會斷線 start
            if (!string.IsNullOrEmpty(chatText) && chatText.Length > 130)
            {
                chatText = chatText.Substring(0, 130);
            }
            // 修正對話出現太長的字串會斷線 end
            if (pc.hasSkillEffect(L1SkillId.SILENCE) ||
                pc.hasSkillEffect(L1SkillId.AREA_OF_SILENCE) ||
                pc.hasSkillEffect(L1SkillId.STATUS_POISON_SILENCE))
            {
                return;
            }
            if (pc.hasSkillEffect(L1SkillId.STATUS_CHAT_PROHIBITED))
            { // 被魔封
                pc.sendPackets(new S_ServerMessage(242)); // 你從現在被禁止閒談。
                return;
            }

            if (chatType == 0)
            { // 一般聊天
                if (pc.Ghost && !(pc.Gm || pc.Monitor))
                {
                    return;
                }
                // GM指令
                if (chatText.StartsWith(".") && (pc.Gm || pc.Monitor))
                {
                    string cmd = chatText.Substring(1);
                    GMCommands.Instance.HandleCommands(pc, cmd);
                    return;
                }

                // 交易頻道
                // 本来はchatType==12になるはずだが、行頭の$が送信されない
                if (chatText.StartsWith("$", StringComparison.Ordinal))
                {
                    string text = chatText.Substring(1);
                    chatWorld(pc, text, 12);
                    if (!pc.Gm)
                    {
                        pc.checkChatInterval();
                    }
                    return;
                }

                ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_NORMALCHAT, 0);
                if (!pc.ExcludingList.contains(pc.Name))
                {
                    pc.sendPackets(s_chatpacket);
                }
                // GM偷聽一般
                if (Config.GM_OVERHEARD0)
                {
                    foreach (GameObject visible in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                    {
                        if (visible is L1PcInstance)
                        {
                            L1PcInstance GM = (L1PcInstance)visible;
                            if (GM.Gm && pc.Id != GM.Id)
                            {
                                GM.sendPackets(new S_SystemMessage("" + "【一般】" + pc.Name + ":" + chatText));
                            }
                        }
                    }
                }
                // GM偷聽一般  end
                foreach (L1PcInstance listner in Container.Instance.Resolve<IGameWorld>().getRecognizePlayer(pc))
                {
                    if (listner.MapId < 16384 || listner.MapId > 25088 || listner.InnKeyId == pc.InnKeyId) // 旅館內判斷
                    {
                        if (!listner.ExcludingList.contains(pc.Name))
                        {
                            listner.sendPackets(s_chatpacket);
                        }
                    }
                }
                // 怪物模仿
                foreach (GameObject obj in pc.KnownObjects)
                {
                    if (obj is L1MonsterInstance)
                    {
                        L1MonsterInstance mob = (L1MonsterInstance)obj;
                        if (mob.NpcTemplate.is_doppel() && mob.Name.Equals(pc.Name) && !mob.Dead)
                        {
                            mob.broadcastPacket(new S_NpcChatPacket(mob, chatText, 0));
                        }
                    }
                }
            }
            else if (chatType == 2)
            { // 喊叫
                if (pc.Ghost)
                {
                    return;
                }
                ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_NORMALCHAT, 2);
                if (!pc.ExcludingList.contains(pc.Name))
                {
                    pc.sendPackets(s_chatpacket);
                }
                foreach (L1PcInstance listner in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(pc, 50))
                {
                    if (listner.MapId < 16384 || listner.MapId > 25088 || listner.InnKeyId == pc.InnKeyId) // 旅館內判斷
                    {
                        if (!listner.ExcludingList.contains(pc.Name))
                        {
                            listner.sendPackets(s_chatpacket);
                        }
                    }
                }

                // 怪物模仿
                foreach (GameObject obj in pc.KnownObjects)
                {
                    if (obj is L1MonsterInstance)
                    {
                        L1MonsterInstance mob = (L1MonsterInstance)obj;
                        if (mob.NpcTemplate.is_doppel() && mob.Name.Equals(pc.Name) && !mob.Dead)
                        {
                            foreach (L1PcInstance listner in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(mob, 50))
                            {
                                listner.sendPackets(new S_NpcChatPacket(mob, chatText, 2));
                            }
                        }
                    }
                }
            }
            else if (chatType == 3)
            { // 全體聊天
                chatWorld(pc, chatText, chatType);
            }
            else if (chatType == 4)
            { // 血盟聊天
                if (pc.Clanid != 0)
                { // 所屬血盟
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                    if ((clan != null))
                    {
                        ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                        S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, 4);
                        L1PcInstance[] clanMembers = clan.OnlineClanMember;
                        // GM偷聽血盟 
                        if (Config.GM_OVERHEARD4)
                        {
                            foreach (GameObject visible in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                            {
                                if (visible is L1PcInstance)
                                {
                                    L1PcInstance GM = (L1PcInstance)visible;
                                    if (GM.Gm && pc.Id != GM.Id)
                                    {
                                        GM.sendPackets(new S_SystemMessage("" + "【血盟】" + pc.Name + ":" + chatText));
                                    }
                                }
                            }
                        }
                        // GM偷聽血盟  end
                        foreach (L1PcInstance listner in clanMembers)
                        {
                            if (!listner.ExcludingList.contains(pc.Name))
                            {
                                if (listner.ShowClanChat && chatType == 4) // 血盟
                                {
                                    listner.sendPackets(s_chatpacket);
                                }
                            }
                        }
                    }
                }
            }
            else if (chatType == 11)
            { // 組隊聊天
                if (pc.InParty)
                { // 組隊中
                    ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                    S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, 11);
                    L1PcInstance[] partyMembers = pc.Party.Members;
                    // GM偷聽隊伍
                    if (Config.GM_OVERHEARD11)
                    {
                        foreach (GameObject visible in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                        {
                            if (visible is L1PcInstance)
                            {
                                L1PcInstance GM = (L1PcInstance)visible;
                                if (GM.Gm && pc.Id != GM.Id)
                                {
                                    GM.sendPackets(new S_SystemMessage("" + "【隊伍】" + pc.Name + ":" + chatText));
                                }
                            }
                        }
                    }
                    // GM偷聽隊伍  end
                    foreach (L1PcInstance listner in partyMembers)
                    {
                        if (!listner.ExcludingList.contains(pc.Name))
                        {
                            if (listner.ShowPartyChat && chatType == 11) // 組隊
                            {
                                listner.sendPackets(s_chatpacket);
                            }
                        }
                    }
                }
            }
            else if (chatType == 12)
            { // 交易聊天
                chatWorld(pc, chatText, chatType);
            }
            else if (chatType == 13)
            { // 聯合血盟
                if (pc.Clanid != 0)
                { // 在血盟中
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                    int rank = pc.ClanRank;
                    if ((clan != null) && ((rank == L1Clan.CLAN_RANK_GUARDIAN) || (rank == L1Clan.CLAN_RANK_LEAGUE_PRINCE) || (rank == L1Clan.CLAN_RANK_LEAGUE_VICEPRINCE) || (rank == L1Clan.CLAN_RANK_LEAGUE_GUARDIAN)))
                    {
                        ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                        S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, 13);
                        L1PcInstance[] clanMembers = clan.OnlineClanMember;
                        // GM偷聽聯盟
                        if (Config.GM_OVERHEARD13)
                        {
                            foreach (GameObject visible in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                            {
                                if (visible is L1PcInstance)
                                {
                                    L1PcInstance GM = (L1PcInstance)visible;
                                    if (GM.Gm && pc.Id != GM.Id)
                                    {
                                        GM.sendPackets(new S_SystemMessage("" + "【聯盟】" + pc.Name + ":" + chatText));
                                    }
                                }
                            }
                        }
                        // GM偷聽聯盟  end
                        foreach (L1PcInstance listner in clanMembers)
                        {
                            int listnerRank = listner.ClanRank;
                            if (!listner.ExcludingList.contains(pc.Name) && ((listnerRank == L1Clan.CLAN_RANK_GUARDIAN) || (listnerRank == L1Clan.CLAN_RANK_LEAGUE_PRINCE) || (rank == L1Clan.CLAN_RANK_LEAGUE_VICEPRINCE) || (rank == L1Clan.CLAN_RANK_LEAGUE_GUARDIAN)))
                            {
                                listner.sendPackets(s_chatpacket);
                            }
                        }
                    }
                }
            }
            else if (chatType == 14)
            { // 聊天組隊
                if (pc.InChatParty)
                { // 聊天組隊
                    ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                    S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_NORMALCHAT, 14);
                    L1PcInstance[] partyMembers = pc.ChatParty.Members;
                    foreach (L1PcInstance listner in partyMembers)
                    {
                        if (!listner.ExcludingList.contains(pc.Name))
                        {
                            listner.sendPackets(s_chatpacket);
                        }
                    }
                }
            }
            else if (chatType == 17)
            { // 血盟王族公告頻道
                if (pc.ClanRank == 10 || pc.ClanRank == 4)
                {
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                    S_ChatPacket s_chatpacket = new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, 17);
                    L1PcInstance[] clanMembers = clan.OnlineClanMember;
                    foreach (L1PcInstance listner in clanMembers)
                    {
                        listner.sendPackets(s_chatpacket);
                    }
                }
            }
            if (!pc.Gm)
            {
                pc.checkChatInterval();
            }
        }

        private void chatWorld(L1PcInstance pc, string chatText, int chatType)
        {
            if (pc.Gm)
            {
                ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, chatType));
            }
            else if (pc.Level >= Config.GLOBAL_CHAT_LEVEL)
            {
                if (Container.Instance.Resolve<IGameWorld>().WorldChatElabled)
                {
                    if (pc.get_food() >= 2)
                    {
                        //TODO 廣播扣飽食度
                        if (Config.DeleteFood)
                        {
                            pc.set_food(pc.get_food() - 2);
                        }
                        //end
                        ChatLogTable.Instance.storeChat(pc, null, chatText, chatType);
                        pc.sendPackets(new S_PacketBox(S_PacketBox.FOOD, pc.get_food()));
                        foreach (L1PcInstance listner in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                        {
                            if (!listner.ExcludingList.contains(pc.Name))
                            {
                                if (listner.ShowTradeChat && (chatType == 12))
                                {
                                    listner.sendPackets(new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, chatType));
                                }
                                else if (listner.ShowWorldChat && (chatType == 3))
                                {
                                    listner.sendPackets(new S_ChatPacket(pc, chatText, Opcodes.S_OPCODE_GLOBALCHAT, chatType));
                                }
                            }
                        }
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(462)); // 你太過於饑餓以致於無法談話。
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(510)); // 現在ワールドチャットは停止中となっております。しばらくの間ご了承くださいませ。
                }
            }
            else
            {
                // 等級 以下的角色無法使用公頻或買賣頻道。
                pc.sendPackets(new S_ServerMessage(195, Config.GLOBAL_CHAT_LEVEL.ToString()));
            }
        }

        public override string Type
        {
            get
            {
                return C_CHAT;
            }
        }
    }

}