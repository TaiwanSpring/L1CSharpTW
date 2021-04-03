using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    class C_PledgeRecommendation : ClientBasePacket
    {
        private const string C_PledgeRecommendation_Conflict = "[C] C_PledgeRecommendation";

        public C_PledgeRecommendation(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int data = ReadC();

            if (data == 0)
            { // 登陸推薦血盟
                /// <summary>
                /// 血盟類型 戰鬥/打怪/友好 </summary>
                int clanType = ReadC();
                string TypeMessage = ReadS();
                if (ClanRecommendTable.Instance.isRecorded(pc.Clanid))
                { // Update
                    ClanRecommendTable.Instance.updateRecommendRecord(pc.Clanid, clanType, TypeMessage);
                }
                else
                {
                    ClanRecommendTable.Instance.addRecommendRecord(pc.Clanid, clanType, TypeMessage);
                }
                pc.sendPackets(new S_PledgeRecommendation(true, pc.Clanid));
            }
            else if (data == 1)
            { // 取消登錄
                ClanRecommendTable.Instance.removeRecommendRecord(pc.Clanid);
                pc.sendPackets(new S_PledgeRecommendation(false, pc.Clanid));
            }
            else if (data == 2)
            { // 打開推薦血盟
                pc.sendPackets(new S_PledgeRecommendation(data, pc.Name));
            }
            else if (data == 3)
            { // 打開申請目錄
                pc.sendPackets(new S_PledgeRecommendation(data, pc.Name));
            }
            else if (data == 4)
            { // 打開邀請目錄
                if (pc.ClanRank > 0)
                {
                    pc.sendPackets(new S_PledgeRecommendation(data, pc.Clanid));
                }
            }
            else if (data == 5)
            { // 申請加入
                int clan_id = ReadD();
                ClanRecommendTable.Instance.addRecommendApply(clan_id, pc.Name);
                pc.sendPackets(new S_PledgeRecommendation(data, clan_id, 0));
            }
            else if (data == 6)
            { // 審核已登記資料
                int index = ReadD();
                int type = ReadC();

                if (type == 1)
                { // 接受玩家加入
                    L1Clan clan = pc.Clan;
                    L1PcInstance joinPc = Container.Instance.Resolve<IGameWorld>().getPlayer(ClanRecommendTable.Instance.getApplyPlayerName(index));
                    foreach (L1PcInstance clanMembers in clan.OnlineClanMember)
                    {
                        clanMembers.sendPackets(new S_ServerMessage(94, joinPc.Name)); // \f1你接受%0當你的血盟成員。
                    }
                    joinPc.Clanid = clan.ClanId;
                    joinPc.Clanname = clan.ClanName;
                    joinPc.ClanMemberNotes = "";
                    joinPc.Title = "";
                    joinPc.sendPackets(new S_CharTitle(joinPc.Id, ""));
                    joinPc.broadcastPacket(new S_CharTitle(joinPc.Id, ""));
                    clan.addMemberName(joinPc.Name);
                    ClanMembersTable.Instance.newMember(joinPc);
                    // 聯盟
                    if (pc.ClanRank < 7)
                    {
                        joinPc.ClanRank = L1Clan.CLAN_RANK_LEAGUE_PUBLIC;
                        joinPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_LEAGUE_PUBLIC, joinPc.Name)); // 你的階級變更為
                    }
                    else
                    { // 一般血盟
                        joinPc.ClanRank = L1Clan.CLAN_RANK_PUBLIC;
                        joinPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_PUBLIC, joinPc.Name)); // 你的階級變更為
                    }
                    joinPc.Save(); // 儲存加入的玩家資料
                    joinPc.sendPackets(new S_ServerMessage(95, clan.ClanName)); // \f1加入%0血盟。
                    joinPc.sendPackets(new S_ClanName(joinPc, true));
                    joinPc.sendPackets(new S_CharReset(joinPc.Id, clan.ClanId));
                    joinPc.sendPackets(new S_PacketBox(S_PacketBox.PLEDGE_EMBLEM_STATUS, pc.Clan.EmblemStatus));
                    joinPc.sendPackets(new S_ClanAttention());
                    foreach (L1PcInstance player in clan.OnlineClanMember)
                    {
                        player.sendPackets(new S_CharReset(joinPc.Id, joinPc.Clan.EmblemId));
                        player.broadcastPacket(new S_CharReset(player.Id, joinPc.Clan.EmblemId));
                    }
                }
                else if (type == 2)
                { // 拒絕玩家加入
                    ClanRecommendTable.Instance.removeRecommendApply(index);
                }
                else if (type == 3)
                { // 刪除申請
                    ClanRecommendTable.Instance.removeRecommendApply(index);
                }
                pc.sendPackets(new S_PledgeRecommendation(data, index, type));
            }


        }

        public override string Type
        {
            get
            {
                return C_PledgeRecommendation_Conflict;
            }
        }
    }

}