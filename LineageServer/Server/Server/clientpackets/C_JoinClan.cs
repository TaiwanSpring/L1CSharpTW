using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.utils;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來加入血盟的封包
    /// </summary>
    class C_JoinClan : ClientBasePacket
    {

        private const string C_JOIN_CLAN = "[C] C_JoinClan";

        public C_JoinClan(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            if ((pc == null) || pc.Ghost)
            {
                return;
            }

            L1PcInstance target = FaceToFace.faceToFace(pc, true);
            if (target != null)
            {
                JoinClan(pc, target);
            }
        }

        private void JoinClan(L1PcInstance player, L1PcInstance target)
        {
            // 如果面對的對象不是王族或守護騎士
            if (!target.Crown && (target.ClanRank != L1Clan.CLAN_RANK_GUARDIAN))
            {
                player.sendPackets(new S_ServerMessage(92, target.Name)); // \f1%0はプリンスやプリンセスではありません。
                return;
            }

            if (player.Clanid == target.Clanid)
            {
                // 同一血盟
                player.sendPackets(new S_ServerMessage(1199));
                return;
            }

            int clan_id = target.Clanid;
            string clan_name = target.Clanname;
            if (clan_id == 0)
            { // 面對的對象沒有創立血盟
                player.sendPackets(new S_ServerMessage(90, target.Name)); // \f1%0は血盟を創設していない状態です。
                return;
            }

            L1Clan clan = L1World.Instance.getClan(clan_name);
            if (clan == null)
            {
                return;
            }

            if (target.ClanRank != L1Clan.CLAN_RANK_PRINCE && target.ClanRank != L1Clan.CLAN_RANK_GUARDIAN && target.ClanRank != L1Clan.CLAN_RANK_LEAGUE_GUARDIAN && target.ClanRank != L1Clan.CLAN_RANK_LEAGUE_PRINCE && target.ClanRank != L1Clan.CLAN_RANK_LEAGUE_VICEPRINCE)
            {
                // 面對的對象不是盟主
                player.sendPackets(new S_ServerMessage(92, target.Name));
                return;
            }

            if (player.Clanid != 0)
            { // 已經加入血盟
                if (player.Crown)
                { // 自己是盟主
                    string player_clan_name = player.Clanname;
                    L1Clan player_clan = L1World.Instance.getClan(player_clan_name);
                    if (player_clan == null)
                    {
                        return;
                    }

                    if (player.Id != player_clan.LeaderId)
                    { // 已經加入其他血盟
                        player.sendPackets(new S_ServerMessage(89)); // \f1あなたはすでに血盟に加入しています。
                        return;
                    }

                    if ((player_clan.CastleId != 0) || (player_clan.HouseId != 0))
                    {
                        player.sendPackets(new S_ServerMessage(665)); // \f1城やアジトを所有した状態で血盟を解散することはできません。
                        return;
                    }
                }
                else
                {
                    player.sendPackets(new S_ServerMessage(89)); // \f1あなたはすでに血盟に加入しています。
                    return;
                }
            }

            target.TempID = player.Id; // 暫時保存面對的人的ID
            target.sendPackets(new S_Message_YN(97, player.Name)); // %0が血盟に加入したがっています。承諾しますか？（Y/N）
        }

        public override string Type
        {
            get
            {
                return C_JOIN_CLAN;
            }
        }
    }

}