using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
	class ClanTable
	{
		private readonly static IDataSource dataSource =
		 Container.Instance.Resolve<IDataSourceFactory>()
		 .Factory(Enum.DataSourceTypeEnum.ClanData);

		private static ILogger _log = Logger.GetLogger(nameof(ClanTable));

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
				IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
				for (int i = 0; i < dataSourceRows.Count; i++)
				{
					IDataSourceRow dataSourceRow = dataSourceRows[i];
					L1Clan clan = new L1Clan();
					clan.ClanId = dataSourceRow.getInt(ClanData.Column_clan_id);
					clan.ClanName = dataSourceRow.getString(ClanData.Column_clan_name);
					clan.LeaderId = dataSourceRow.getInt(ClanData.Column_leader_id);
					clan.LeaderName = dataSourceRow.getString(ClanData.Column_leader_name);
					clan.CastleId = dataSourceRow.getInt(ClanData.Column_hascastle);
					clan.HouseId = dataSourceRow.getInt(ClanData.Column_hashouse);
					clan.FoundDate = dataSourceRow.getTimestamp(ClanData.Column_found_date);
					clan.Announcement = dataSourceRow.getString(ClanData.Column_announcement);
					clan.EmblemId = dataSourceRow.getInt(ClanData.Column_emblem_id);
					clan.EmblemStatus = dataSourceRow.getInt(ClanData.Column_emblem_status);

					L1World.Instance.storeClan(clan);

					_clans[clan.ClanId] = clan;
				}
			}

			ICollection<L1Clan> AllClan = L1World.Instance.AllClans;

			IDataSource charactersDataSource =
		 Container.Instance.Resolve<IDataSourceFactory>()
		 .Factory(Enum.DataSourceTypeEnum.Characters);

			foreach (L1Clan clan in AllClan)
			{
				IDataSourceRow dataSourceRow = charactersDataSource.NewRow();
				dataSourceRow.Select().Where(Characters.Column_ClanID, clan.ClanId).Execute();
				if (dataSourceRow.HaveData)
				{
					clan.addMemberName(dataSourceRow.getString(Characters.Column_char_name));
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
			clan.FoundDate = DateTime.Now;
			clan.Announcement = "";
			clan.EmblemId = 0;
			clan.EmblemStatus = 0;

			dataSource.NewRow().Insert()
				.Set(ClanData.Column_clan_id, clan.ClanId)
				.Set(ClanData.Column_clan_name, clan.ClanName)
				.Set(ClanData.Column_leader_id, clan.LeaderId)
				.Set(ClanData.Column_leader_name, clan.LeaderName)
				.Set(ClanData.Column_hascastle, clan.CastleId)
				.Set(ClanData.Column_hashouse, clan.HouseId)
				.Set(ClanData.Column_found_date, clan.FoundDate).Execute();
			//.Set(ClanData.Column_announcement, clan.ClanId)
			//.Set(ClanData.Column_emblem_id, clan.ClanId)
			//.Set(ClanData.Column_emblem_status, clan.ClanId).Execute();

			L1World.Instance.storeClan(clan);
			_clans[clan.ClanId] = clan;

			player.Clanid = clan.ClanId;
			player.Clanname = clan.ClanName;

			/// <summary>
			/// 授予一般君主權限 或者 聯盟王權限 </summary>
			if (player.Quest.isEnd(L1Quest.QUEST_LEVEL45))
			{
				// 通過45任務
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
			dataSource.NewRow().Update()
				.Set(ClanData.Column_clan_id, clan.ClanId)
				.Where(ClanData.Column_clan_name, clan.ClanName)
				.Set(ClanData.Column_leader_id, clan.LeaderId)
				.Set(ClanData.Column_leader_name, clan.LeaderName)
				.Set(ClanData.Column_hascastle, clan.CastleId)
				.Set(ClanData.Column_hashouse, clan.HouseId)
				.Set(ClanData.Column_found_date, clan.FoundDate)
			.Set(ClanData.Column_announcement, clan.Announcement)
			.Set(ClanData.Column_emblem_id, clan.EmblemId)
			.Set(ClanData.Column_emblem_status, clan.EmblemStatus).Execute();
		}

		public virtual void deleteClan(string clan_name)
		{
			L1Clan clan = L1World.Instance.getClan(clan_name);
			if (clan == null)
			{
				return;
			}
			dataSource.NewRow().Delete()
				.Where(ClanData.Column_clan_name, clan.ClanName).Execute();

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