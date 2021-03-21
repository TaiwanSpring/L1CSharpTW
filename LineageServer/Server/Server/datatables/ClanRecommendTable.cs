namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class ClanRecommendTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ClanRecommendTable).FullName);

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
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO clan_recommend_record SET clan_id=?, clan_name=?, crown_name=?, clan_type=?, type_message=?");
				L1Clan clan = ClanTable.Instance.getTemplate(clan_id);
				pstm.setInt(1, clan_id);
				pstm.setString(2, clan.ClanName);
				pstm.setString(3, clan.LeaderName);
				pstm.setInt(4, clan_type);
				pstm.setString(5, type_message);
				pstm.execute();
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

		/// <summary>
		/// 血盟推薦 增加一筆申請 </summary>
		/// <param name="clan_id"> 申請的血盟ID </param>
		/// <param name="char_name"> 申請玩家名稱 </param>
		public virtual void addRecommendApply(int clan_id, string char_name)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO clan_recommend_apply SET clan_id=?, clan_name=?, char_name=?");
				L1Clan clan = ClanTable.Instance.getTemplate(clan_id);
				pstm.setInt(1, clan_id);
				pstm.setString(2, clan.ClanName);
				pstm.setString(3, char_name);
				pstm.execute();
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

		/// <summary>
		/// 更新登錄資料
		/// </summary>
		public virtual void updateRecommendRecord(int clan_id, int clan_type, string type_message)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE clan_recommend_record SET clan_name=?, crown_name=?, clan_type=?, type_message=? WHERE clan_id=?");
				L1Clan clan = ClanTable.Instance.getTemplate(clan_id);
				pstm.setString(1, clan.ClanName);
				pstm.setString(2, clan.LeaderName);
				pstm.setInt(3, clan_type);
				pstm.setString(4, type_message);
				pstm.setInt(5, clan_id);
				pstm.execute();
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

		/// <summary>
		/// 刪除血盟推薦申請 </summary>
		/// <param name="id"> 申請ID </param>
		public virtual void removeRecommendApply(int id)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM clan_recommend_apply WHERE id=?");
				pstm.setInt(1, id);
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

		/// <summary>
		/// 刪除血盟推薦 登錄 </summary>
		/// <param name="clan_id"> 血盟 id </param>
		public virtual void removeRecommendRecord(int clan_id)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM clan_recommend_record WHERE clan_id=?");
				pstm.setInt(1, clan_id);
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

		/// <summary>
		/// 取得申請的玩家名稱 </summary>
		/// <param name="index_id">
		/// @return </param>
		public virtual string getApplyPlayerName(int index_id)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			string charName = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM clan_recommend_apply WHERE id=?");
				pstm.setInt(1, index_id);
				rs = pstm.executeQuery();

				if (rs.first())
				{
					charName = rs.getString("char_name");
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
			return charName;
		}

		/// <summary>
		/// 該血盟是否登錄 </summary>
		/// <param name="clan_id">
		/// @return </param>
		public virtual bool isRecorded(int clan_id)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM clan_recommend_record WHERE clan_id=?");
				pstm.setInt(1, clan_id);
				rs = pstm.executeQuery();
				return rs.next();
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
			return false;
		}

		/// <summary>
		/// 該玩家是否提出申請 </summary>
		/// <param name="char_name">
		/// @return </param>
		public virtual bool isApplied(string char_name)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM clan_recommend_apply WHERE char_name=?");
				pstm.setString(1, char_name);
				rs = pstm.executeQuery();
				return rs.next();
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
			return false;
		}

		/// <summary>
		/// 該血盟是否有人申請加入
		/// </summary>
		public virtual bool isClanApplyByPlayer(int clan_id)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM clan_recommend_apply WHERE clan_id=?");
				pstm.setInt(1, clan_id);
				rs = pstm.executeQuery();
				return rs.next();
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
			return false;
		}

		/// <summary>
		/// 是否對該血盟提出申請 </summary>
		/// <param name="clan_id"> 血盟Id </param>
		/// <returns> True:False </returns>
		public virtual bool isApplyForTheClan(int clan_id, string char_name)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM clan_recommend_apply WHERE clan_id=? AND char_name=?");
				pstm.setInt(1, clan_id);
				pstm.setString(2, char_name);
				rs = pstm.executeQuery();
				return rs.next();
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
			return false;
		}

	}

}