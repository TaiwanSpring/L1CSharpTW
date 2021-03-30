using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;

namespace LineageServer.Server.DataTables
{
	class ClanRecommendTable
	{

		private readonly static IDataSource clanRecommendApplyDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.ClanRecommendApply);

		private readonly static IDataSource clanRecommendRecordDataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.ClanRecommendRecord);

		private static ClanRecommendTable _instance;

		public static ClanRecommendTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ClanRecommendTable();
				}
				return _instance;
			}
		}

		/// <summary>
		/// 血盟推薦 登陸 </summary>
		/// <param name="clan_id"> 血盟 id </param>
		/// <param name="clan_type"> 血盟類型 友好/打怪/戰鬥 </param>
		/// <param name="type_message"> 類型說明文字 </param>
		public virtual void addRecommendRecord(int clan_id, int clan_type, string type_message)
		{
			L1Clan clan = ClanTable.Instance.getTemplate(clan_id);

			clanRecommendRecordDataSource.NewRow().Insert()
				.Set(ClanRecommendRecord.Column_clan_id, clan_id)
				.Set(ClanRecommendRecord.Column_clan_name, clan.ClanName)
				.Set(ClanRecommendRecord.Column_crown_name, clan.LeaderName)
				.Set(ClanRecommendRecord.Column_clan_type, clan_type)
				.Set(ClanRecommendRecord.Column_type_message, type_message)
				.Execute();
		}

		/// <summary>
		/// 血盟推薦 增加一筆申請 </summary>
		/// <param name="clan_id"> 申請的血盟ID </param>
		/// <param name="char_name"> 申請玩家名稱 </param>
		public virtual void addRecommendApply(int clan_id, string char_name)
		{
			L1Clan clan = ClanTable.Instance.getTemplate(clan_id);
			clanRecommendApplyDataSource.NewRow().Insert()
				.Set(ClanRecommendApply.Column_clan_id, clan_id)
				.Set(ClanRecommendApply.Column_clan_name, clan.ClanName)
				.Set(ClanRecommendApply.Column_char_name, char_name)
				.Execute();
		}

		/// <summary>
		/// 更新登錄資料
		/// </summary>
		public virtual void updateRecommendRecord(int clan_id, int clan_type, string type_message)
		{
			//L1Clan clan = ClanTable.Instance.getTemplate(clan_id);

			clanRecommendRecordDataSource.NewRow().Update()
				.Where(ClanRecommendRecord.Column_clan_id, clan_id)
				//.Set(ClanRecommendRecord.Column_clan_name, clan.ClanName)
				//.Set(ClanRecommendRecord.Column_crown_name, clan.LeaderName)
				.Set(ClanRecommendRecord.Column_clan_type, clan_type)
				.Set(ClanRecommendRecord.Column_type_message, type_message)
				.Execute();
		}

		/// <summary>
		/// 刪除血盟推薦申請 </summary>
		/// <param name="id"> 申請ID </param>
		public virtual void removeRecommendApply(int id)
		{
			clanRecommendApplyDataSource.NewRow().Delete()
			.Where(ClanRecommendApply.Column_id, id)
			.Execute();
		}

		/// <summary>
		/// 刪除血盟推薦 登錄 </summary>
		/// <param name="clan_id"> 血盟 id </param>
		public virtual void removeRecommendRecord(int clan_id)
		{
			clanRecommendRecordDataSource.NewRow().Delete()
				.Where(ClanRecommendRecord.Column_clan_id, clan_id)
				.Execute();
		}

		/// <summary>
		/// 取得申請的玩家名稱 </summary>
		/// <param name="index_id">
		/// @return </param>
		public virtual string getApplyPlayerName(int index_id)
		{
			IDataSourceRow dataSourceRow = clanRecommendApplyDataSource.NewRow();
			dataSourceRow.Select()
				.Where(ClanRecommendApply.Column_id, index_id)
				.Execute();
			if (dataSourceRow.HaveData)
			{
				return dataSourceRow.getString(ClanRecommendApply.Column_char_name);
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// 該血盟是否登錄 </summary>
		/// <param name="clan_id">
		/// @return </param>
		public virtual bool isRecorded(int clan_id)
		{
			IDataSourceRow dataSourceRow = clanRecommendRecordDataSource.NewRow();
			dataSourceRow.Select()
			.Where(ClanRecommendRecord.Column_clan_id, clan_id)
			.Execute();
			return dataSourceRow.HaveData;
		}

		/// <summary>
		/// 該玩家是否提出申請 </summary>
		/// <param name="char_name">
		/// @return </param>
		public virtual bool isApplied(string char_name)
		{
			IDataSourceRow dataSourceRow = clanRecommendApplyDataSource.NewRow();
			dataSourceRow.Select()
				.Where(ClanRecommendApply.Column_char_name, char_name)
				.Execute();
			return dataSourceRow.HaveData;
		}

		/// <summary>
		/// 該血盟是否有人申請加入
		/// </summary>
		public virtual bool isClanApplyByPlayer(int clan_id)
		{
			IDataSourceRow dataSourceRow = clanRecommendApplyDataSource.NewRow();
			dataSourceRow.Select()
				.Where(ClanRecommendApply.Column_clan_id, clan_id)
				.Execute();
			return dataSourceRow.HaveData;
		}

		/// <summary>
		/// 是否對該血盟提出申請 </summary>
		/// <param name="clan_id"> 血盟Id </param>
		/// <returns> True:False </returns>
		public virtual bool isApplyForTheClan(int clan_id, string char_name)
		{
			IDataSourceRow dataSourceRow = clanRecommendApplyDataSource.NewRow();
			dataSourceRow.Select()
				.Where(ClanRecommendApply.Column_clan_id, clan_id)
				.Where(ClanRecommendApply.Column_char_name, char_name)
				.Execute();
			return dataSourceRow.HaveData;
		}
	}
}