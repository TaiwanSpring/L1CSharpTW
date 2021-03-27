﻿using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.DataSources
{
    class ClanTable
    {
        private static ILogger _log = Logger.GetLogger(typeof(ClanTable).FullName);

        private readonly IDictionary<int, L1Clan> _clans = MapFactory.NewMap<int, L1Clan>();

        private static ClanTable _instance;

        public static ClanTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClanTable();
                }
                return _instance;
            }
        }

        private ClanTable()
        {
            {
                IDataBaseConnection con = null;
                PreparedStatement pstm = null;
                ResultSet rs = null;

                try
                {
                    con = L1DatabaseFactory.Instance.Connection;
                    pstm = con.prepareStatement("SELECT * FROM clan_data ORDER BY clan_id");

                    rs = pstm.executeQuery();
                    while (rs.next())
                    {
                        L1Clan clan = new L1Clan();
                        // clan.SetClanId(clanData.getInt(1));
                        int clan_id = dataSourceRow.getInt(1);
                        clan.ClanId = clan_id;
                        clan.ClanName = dataSourceRow.getString(2);
                        clan.LeaderId = dataSourceRow.getInt(3);
                        clan.LeaderName = dataSourceRow.getString(4);
                        clan.CastleId = dataSourceRow.getInt(5);
                        clan.HouseId = dataSourceRow.getInt(6);
                        clan.FoundDate = dataSourceRow.getTimestamp(7);
                        clan.Announcement = dataSourceRow.getString(8);
                        clan.EmblemId = dataSourceRow.getInt(9);
                        clan.EmblemStatus = dataSourceRow.getInt(10);

                        L1World.Instance.storeClan(clan);
                        _clans[clan_id] = clan;
                    }

                }
                catch (SQLException e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
                finally
                {
                    SQLUtil.close(rs);
                    SQLUtil.close(pstm);
                    SQLUtil.close(con);
                }
            }

            ICollection<L1Clan> AllClan = L1World.Instance.AllClans;
            foreach (L1Clan clan in AllClan)
            {
                IDataBaseConnection con = null;
                PreparedStatement pstm = null;
                ResultSet rs = null;

                try
                {
                    con = L1DatabaseFactory.Instance.Connection;
                    pstm = con.prepareStatement("SELECT char_name FROM characters WHERE ClanID = ?");
                    pstm.setInt(1, clan.ClanId);
                    rs = pstm.executeQuery();

                    while (rs.next())
                    {
                        clan.addMemberName(dataSourceRow.getString(1));
                    }
                }
                catch (SQLException e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
                finally
                {
                    SQLUtil.close(rs);
                    SQLUtil.close(pstm);
                    SQLUtil.close(con);
                }
            }
            // クラン倉庫のロード
            foreach (L1Clan clan in AllClan)
            {
                clan.DwarfForClanInventory.loadItems();
            }
        }

        public virtual L1Clan createClan(L1PcInstance player, string clan_name)
        {
            foreach (L1Clan oldClans in L1World.Instance.AllClans)
            {
                if (oldClans.ClanName == clan_name)

                {
                    return null;
                }
            }
            L1Clan clan = new L1Clan();
            clan.ClanId = IdFactory.Instance.nextId();
            clan.ClanName = clan_name;
            clan.LeaderId = player.Id;
            clan.LeaderName = player.Name;
            clan.CastleId = 0;
            clan.HouseId = 0;
            clan.FoundDate = new Timestamp(DateTimeHelper.CurrentUnixTimeMillis());
            clan.Announcement = "";
            clan.EmblemId = 0;
            clan.EmblemStatus = 0;

            IDataBaseConnection con = null;
            PreparedStatement pstm = null;

            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("INSERT INTO clan_data SET clan_id=?, clan_name=?, leader_id=?, leader_name=?, hascastle=?, hashouse=?, found_date=?, announcement=?, emblem_id=?, emblem_status=?");
                pstm.setInt(1, clan.ClanId);
                pstm.setString(2, clan.ClanName);
                pstm.setInt(3, clan.LeaderId);
                pstm.setString(4, clan.LeaderName);
                pstm.setInt(5, clan.CastleId);
                pstm.setInt(6, clan.HouseId);
                pstm.setTimestamp(7, new Timestamp(DateTimeHelper.CurrentUnixTimeMillis()));
                pstm.setString(8, "");
                pstm.setInt(9, 0);
                pstm.setInt(10, 0);
                pstm.execute();
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }

            L1World.Instance.storeClan(clan);
            _clans[clan.ClanId] = clan;

            player.Clanid = clan.ClanId;
            player.Clanname = clan.ClanName;

            /// <summary>
            /// 授予一般君主權限 或者 聯盟王權限 </summary>
            if (player.Quest.isEnd(L1Quest.QUEST_LEVEL45))
            { // 通過45任務
                player.ClanRank = L1Clan.CLAN_RANK_LEAGUE_PRINCE;
                player.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_LEAGUE_PRINCE, player.Name)); // 你的階級變更為%s
            }
            else
            {
                player.ClanRank = L1Clan.CLAN_RANK_PRINCE;
                player.sendPackets(new S_PacketBox(S_PacketBox.MSG_RANK_CHANGED, L1Clan.CLAN_RANK_PRINCE, player.Name)); // 你的階級變更為%s
            }

            clan.addMemberName(player.Name);
            try
            {
                // DBにキャラクター情報を書き込む
                player.Save();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return clan;
        }

        public virtual void updateClan(L1Clan clan)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("UPDATE clan_data SET clan_id=?, leader_id=?, leader_name=?, hascastle=?, hashouse=?, found_date=?, announcement=?, emblem_id=?, emblem_status=? WHERE clan_name=?");
                pstm.setInt(1, clan.ClanId);
                pstm.setInt(2, clan.LeaderId);
                pstm.setString(3, clan.LeaderName);
                pstm.setInt(4, clan.CastleId);
                pstm.setInt(5, clan.HouseId);
                pstm.setTimestamp(6, clan.FoundDate);
                pstm.setString(7, clan.Announcement);
                pstm.setInt(8, clan.EmblemId);
                pstm.setInt(9, clan.EmblemStatus);
                pstm.setString(10, clan.ClanName);
                pstm.execute();
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

        public virtual void deleteClan(string clan_name)
        {
            L1Clan clan = L1World.Instance.getClan(clan_name);
            if (clan == null)
            {
                return;
            }
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("DELETE FROM clan_data WHERE clan_name=?");
                pstm.setString(1, clan_name);
                pstm.execute();
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
            clan.DwarfForClanInventory.clearItems();
            clan.DwarfForClanInventory.deleteAllItems();

            L1World.Instance.removeClan(clan);
            _clans.Remove(clan.ClanId);
        }

        public virtual L1Clan getTemplate(int clan_id)
        {
            return _clans[clan_id];
        }

    }

}