using System;
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
namespace LineageServer.Server.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.Utils.SQLUtil;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;

	public class IpTable
	{

		public static IpTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new IpTable();
				}
				return _instance;
			}
		}

		private IpTable()
		{
			if (!isInitialized)
			{
				_banip = Lists.newList();
				IpTable;
			}
		}

		public virtual void banIp(string ip)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO ban_ip SET ip=?");
				pstm.setString(1, ip);
				pstm.execute();
				_banip.Add(ip);
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

		public virtual bool isBannedIp(string s)
		{
			foreach (string BanIpAddress in _banip)
			{ //被封鎖的IP
				// 判斷如果使用*結尾
				if (BanIpAddress.EndsWith("*", StringComparison.Ordinal))
				{
					// 取回第一次出現*的index
					int fStarindex = BanIpAddress.IndexOf("*", StringComparison.Ordinal);
					// 取得0~fStar長度
					string reip = BanIpAddress.Substring(0, fStarindex);
					// 抓得Banip表單內ip在*號前的子字串 xxx.xxx||xxx.xxx.xxx
					string newaddress = s.Substring(0, fStarindex);
					if (newaddress.Equals(reip, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				else
				{
					if (s.Equals(BanIpAddress, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual void getIpTable()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM ban_ip");

				rs = pstm.executeQuery();

				while (rs.next())
				{
					_banip.Add(dataSourceRow.getString(1));
				}

				isInitialized = true;

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

		public virtual bool liftBanIp(string ip)
		{
			bool ret = false;
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM ban_ip WHERE ip=?");
				pstm.setString(1, ip);
				pstm.execute();
				ret = _banip.Remove(ip);
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
			return ret;
		}

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(IpTable).FullName);

		private static IList<string> _banip;

		public static bool isInitialized;

		private static IpTable _instance;

	}

}