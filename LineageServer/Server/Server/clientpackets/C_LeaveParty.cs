using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來離開組隊的封包
    /// </summary>
    class C_LeaveParty : ClientBasePacket
    {

        private const string C_LEAVE_PARTY = "[C] C_LeaveParty";
        public C_LeaveParty(sbyte[] decrypt, ClientThread client) : base(decrypt)
        {

            L1PcInstance player = client.ActiveChar;
            if (player == null)
            {
                return;
            }
            // 組隊中
            if (player.InParty)
            {
                player.Party.leaveMember(player);
            }
        }

        public override string Type
        {
            get
            {
                return C_LEAVE_PARTY;
            }
        }

    }

}