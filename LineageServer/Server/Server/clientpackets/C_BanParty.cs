using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳來離開組隊的封包
    /// </summary>
    class C_BanParty : ClientBasePacket
    {

        private const string C_BAN_PARTY = "[C] C_BanParty";

        public C_BanParty(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {

            L1PcInstance player = client.ActiveChar;
            if (player == null)
            {
                return;
            }

            string s = readS();

            if (!player.Party.isLeader(player))
            {
                // 非組隊隊長
                player.sendPackets(new S_ServerMessage(427)); // 只有領導者才有驅逐隊伍成員的權力。
                return;
            }

            foreach (L1PcInstance member in player.Party.Members)
            {
                if (member.Name.ToLower().Equals(s.ToLower()))
                {
                    player.Party.kickMember(member);
                    return;
                }
            }

            player.sendPackets(new S_ServerMessage(426, s)); // %0%d 不屬於任何隊伍。
        }

        public override string Type
        {
            get
            {
                return C_BAN_PARTY;
            }
        }
    }
}