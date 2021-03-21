using System.Collections.Generic;

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
namespace LineageServer.Server.Server.command
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Command = LineageServer.Server.Server.Templates.L1Command;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	/// <summary>
	/// 處理 GM 指令
	/// </summary>
	public class L1Commands
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1Commands).FullName);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private static l1j.server.server.templates.L1Command fromResultSet(java.sql.ResultSet rs) throws java.sql.SQLException
		private static L1Command fromResultSet(ResultSet rs)
		{
			return new L1Command(rs.getString("name"), rs.getInt("access_level"), rs.getString("class_name"));
		}

		public static L1Command get(string name)
		{
			/*
			 * 每次為便於調試和實驗，以便讀取數據庫。緩存性能低於理論是微不足道的。
			 */
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM commands WHERE name=?");
				pstm.setString(1, name);
				rs = pstm.executeQuery();
				if (!rs.next())
				{
					return null;
				}
				return fromResultSet(rs);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "錯誤的指令", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return null;
		}

		public static IList<L1Command> availableCommandList(int accessLevel)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			IList<L1Command> result = Lists.newList();
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM commands WHERE access_level <= ?");
				pstm.setInt(1, accessLevel);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					result.Add(fromResultSet(rs));
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "錯誤的指令", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return result;
		}
	}

}