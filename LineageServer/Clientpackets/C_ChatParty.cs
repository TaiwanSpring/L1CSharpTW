using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來的聊天組隊封包
    /// </summary>
    class C_ChatParty : ClientBasePacket
    {

        private const string C_CHAT_PARTY = "[C] C_ChatParty";

        public C_ChatParty(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if ((pc == null) || pc.Ghost)
            {
                return;
            }

            int type = ReadC();
            if (type == 0)
            { // /chatbanish 的命令
                string name = ReadS();

                if (!pc.InChatParty)
                {
                    // 沒有加入聊天組隊
                    pc.sendPackets(new S_ServerMessage(425));
                    return;
                }
                if (!pc.ChatParty.isLeader(pc))
                {
                    // 只有隊長可以踢人
                    pc.sendPackets(new S_ServerMessage(427));
                    return;
                }
                L1PcInstance targetPc = L1World.Instance.getPlayer(name);
                if (targetPc == null)
                {
                    // 沒有叫%0的人。
                    pc.sendPackets(new S_ServerMessage(109));
                    return;
                }
                if (pc.Id == targetPc.Id)
                {
                    return;
                }

                foreach (L1PcInstance member in pc.ChatParty.Members)
                {
                    if (member.Name.ToLower().Equals(name.ToLower()))
                    {
                        pc.ChatParty.kickMember(member);
                        return;
                    }
                }
                // %0%d 不屬於任何隊伍。
                pc.sendPackets(new S_ServerMessage(426, name));
            }
            else if (type == 1)
            { // /chatoutparty 的命令
                if (pc.InChatParty)
                {
                    pc.ChatParty.leaveMember(pc);
                }
            }
            else if (type == 2)
            { // /chatparty 的命令
                L1ChatParty chatParty = pc.ChatParty;
                if (pc.InChatParty)
                {
                    pc.sendPackets(new S_Party("party", pc.Id, chatParty.Leader.Name, chatParty.MembersNameList));
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(425)); // 您並沒有參加任何隊伍。
                                                              // pc.sendPackets(new S_Party("party", pc.getId()));
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_CHAT_PARTY;
            }
        }

    }

}