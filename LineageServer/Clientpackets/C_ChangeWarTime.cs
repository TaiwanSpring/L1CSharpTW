using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳來改變攻城時間的封包
    /// </summary>
    class C_ChangeWarTime : ClientBasePacket
    {
        private const string C_CHANGE_WAR_TIME = "[C] C_ChangeWarTime";

        public C_ChangeWarTime(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance player = clientthread.ActiveChar;
            if (player == null)
            {
                return;
            }

            L1Clan clan = L1World.Instance.getClan(player.Clanname);
            if (clan != null)
            {
                int castle_id = clan.CastleId;
                if (castle_id != 0)
                { 
                    // 有城
                    L1Castle l1castle = CastleTable.Instance.getCastleTable(castle_id);
                    DateTime cal = l1castle.WarTime;
                    player.sendPackets(new S_WarTime(cal));
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_CHANGE_WAR_TIME;
            }
        }

    }

}