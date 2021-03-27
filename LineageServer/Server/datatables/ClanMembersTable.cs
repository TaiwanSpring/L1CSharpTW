/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using IdFactory = LineageServer.Server.IdFactory;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class ClanMembersTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(ClanMembersTable).FullName);

		private static ClanMembersTable _instance;

		public static ClanMembersTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ClanMembersTable();
				}
				return _instance;
			}
		}

		/// <summary>
		/// 寫入新的血盟成員紀錄
		/// </summary>
		public virtual void newMember(L1PcInstance pc)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm1 = null;
			ResultSet rs = null;
			PreparedStatement pstm2 = null;
			int nextId = IdFactory.Instance.nextId();
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm1 = con.prepareStatement("SELECT * FROM clan_members ORDER BY clan_id");
				rs = pstm1.executeQuery();
				pstm2 = con.prepareStatement("INSERT INTO clan_members SET clan_id=?, index_id=?, char_id=?, char_name=?, notes=?");
				pstm2.setInt(1, pc.Clanid);
				pstm2.setInt(2, nextId);
				pstm2.setInt(3, pc.Id);
				pstm2.setString(4, pc.Name);
				pstm2.setString(5, "");
				pstm2.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm1);
				SQLUtil.close(pstm2);
				SQLUtil.close(con);
			}
			pc.ClanMemberId = nextId;
		}

		/// <summary>
		/// 更新血盟成員資料
		/// </summary>
		public virtual void updateMember(L1PcInstance pc)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE clan_members SET clan_id=?, index_id=?, char_id=?, char_name=?");
				pstm.setInt(1, pc.Clanid);
				pstm.setInt(2, IdFactory.Instance.nextId());
				pstm.setInt(3, pc.Id);
				pstm.setString(4, pc.Name);
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
		/// 更新血盟成員備註欄位
		/// </summary>
		public virtual void updateMemberNotes(L1PcInstance pc, string notes)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm1 = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm1 = con.prepareStatement("UPDATE clan_members SET notes=? WHERE char_id=?");
				pstm1.setString(1, notes);
				pstm1.setInt(2, pc.Id);
				pstm1.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm1);
				SQLUtil.close(con);
			}
		}

		/// <summary>
		/// 刪除血盟成員
		/// </summary>
		public virtual void deleteMember(int charId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM clan_members WHERE char_id=?");
				pstm.setInt(1, charId);
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
		/// 刪除整個血盟
		/// </summary>
		public virtual void deleteAllMember(int clanId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM clan_members WHERE clan_id=?");
				pstm.setInt(1, clanId);
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
	}

}