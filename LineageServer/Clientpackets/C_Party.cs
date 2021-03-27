
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來隊伍的封包
    /// </summary>
    class C_Party : ClientBasePacket
    {

        private const string C_PARTY = "[C] C_Party";

        public C_Party(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if ((pc == null) || pc.Ghost)
            {
                return;
            }
            L1Party party = pc.Party;
            if (pc.InParty)
            {
                pc.sendPackets(new S_Party("party", pc.Id, party.Leader.Name, party.MembersNameList));
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(425)); // パーティーに加入していません。
            }
        }

        public override string Type
        {
            get
            {
                return C_PARTY;
            }
        }

    }

}