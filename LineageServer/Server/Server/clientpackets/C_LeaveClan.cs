using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System.IO;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來離開血盟的封包
    /// </summary>
    class C_LeaveClan : ClientBasePacket
    {

        private const string C_LEAVE_CLAN = "[C] C_LeaveClan";
        public C_LeaveClan(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            string clan_name = readS();

            L1PcInstance player = clientthread.ActiveChar;
            if (player == null)
            {
                return;
            }

            string player_name = player.Name;
            int clan_id = player.Clanid;
            if (clan_id == 0)
            { // 還沒加入血盟
                return;
            }

            L1Clan clan = L1World.Instance.getClan(clan_name);
            if (clan != null)
            {
                string[] clan_member_name = clan.AllMembers;
                int i;
                if (player.Crown && player.Id == clan.LeaderId)
                { //是王族而且是連盟王
                    int castleId = clan.CastleId;
                    int houseId = clan.HouseId;
                    if (castleId != 0 || houseId != 0)
                    {
                        player.sendPackets(new S_ServerMessage(665)); // \f1城やアジトを所有した状態で血盟を解散することはできません。
                        return;
                    }
                    foreach (L1War war in L1World.Instance.WarList)
                    {
                        if (war.CheckClanInWar(clan_name))
                        {
                            player.sendPackets(new S_ServerMessage(302)); // \f1解散させることができません。
                            return;
                        }
                    }

                    for (i = 0; i < clan_member_name.Length; i++)
                    { // 取得所有血盟成員
                        L1PcInstance online_pc = L1World.Instance.getPlayer(clan_member_name[i]);
                        if (online_pc != null)
                        { // 在線上的血盟成員
                            online_pc.sendPackets(new S_ClanAttention());
                            online_pc.sendPackets(new S_ServerMessage(269, player_name, clan_name)); // 血盟的盟主%0%s解散了血盟
                            online_pc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, 0x0b, ""));
                            online_pc.sendPackets(new S_CharReset(online_pc.Id, 0));
                            online_pc.sendPackets(new S_ClanName(online_pc, false));
                            online_pc.sendPackets(new S_ClanAttention());
                            online_pc.Clanid = 0;
                            online_pc.Clanname = "";
                            online_pc.ClanRank = 0;
                            online_pc.ClanMemberId = 0;
                            online_pc.ClanMemberNotes = "";
                            online_pc.Title = "";
                            online_pc.sendPackets(new S_CharTitle(online_pc.Id, ""));
                            online_pc.broadcastPacket(new S_CharTitle(online_pc.Id, ""));
                            online_pc.broadcastPacket(new S_CharReset(online_pc.Id, 0));
                            online_pc.Save(); // 儲存玩家資料到資料庫中
                        }
                        else
                        { // 非線上的血盟成員
                            L1PcInstance offline_pc = CharacterTable.Instance.restoreCharacter(clan_member_name[i]);
                            offline_pc.Clanid = 0;
                            offline_pc.Clanname = "";
                            offline_pc.ClanRank = 0;
                            offline_pc.Title = "";
                            offline_pc.Save(); // 儲存玩家資料到資料庫中
                        }
                    }
                    string emblem_file = $"emblem/{clan.EmblemId}";
                    if (File.Exists(emblem_file))
                    {
                        File.Delete(emblem_file);
                    }
                    ClanTable.Instance.deleteClan(clan_name);
                    ClanMembersTable.Instance.deleteAllMember(clan.ClanId); // 刪除所有成員資料
                }
                else
                { // 除了聯盟王之外
                    L1PcInstance[] clanMember = clan.OnlineClanMember;
                    for (i = 0; i < clanMember.Length; i++)
                    {
                        clanMember[i].sendPackets(new S_ServerMessage(178, player_name, clan_name)); // \f1%0が%1血盟を脱退しました。
                    }
                    if (clan.WarehouseUsingChar == player.Id)
                    {
                        clan.WarehouseUsingChar = 0; // 移除使用血盟倉庫的成員
                    }
                    player.Clanid = 0;
                    player.Clanname = "";
                    player.ClanRank = 0;
                    player.ClanMemberId = 0;
                    player.ClanMemberNotes = "";
                    player.Title = "";
                    player.sendPackets(new S_CharTitle(player.Id, ""));
                    player.broadcastPacket(new S_CharTitle(player.Id, ""));
                    player.Save(); // 儲存玩家資料到資料庫中
                    player.sendPackets(new S_ClanAttention());
                    player.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, 0x0b, ""));
                    player.sendPackets(new S_CharReset(player.Id, 0));
                    player.broadcastPacket(new S_CharReset(player.Id, 0));
                    player.sendPackets(new S_ClanName(player, false));

                    clan.delMemberName(player_name);
                    ClanMembersTable.Instance.deleteMember(player.Id);
                }
            }
            else
            {
                player.Clanid = 0;
                player.Clanname = "";
                player.ClanRank = 0;
                player.ClanMemberId = 0;
                player.ClanMemberNotes = "";
                player.Title = "";
                player.sendPackets(new S_CharTitle(player.Id, ""));
                player.broadcastPacket(new S_CharTitle(player.Id, ""));
                player.sendPackets(new S_CharReset(player.Id, 0));
                player.broadcastPacket(new S_CharReset(player.Id, 0));
                player.Save(); // 儲存玩家資料到資料庫中
                player.sendPackets(new S_ServerMessage(178, player_name, clan_name)); // \f1%0が%1血盟を脱退しました。
                ClanMembersTable.Instance.deleteMember(player.Id);
            }
        }

        public override string Type
        {
            get
            {
                return C_LEAVE_CLAN;
            }
        }

    }

}