using System;
using System.Text;

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
namespace LineageServer.Server.Server
{

	using Config = LineageServer.Server.Config;
	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class Logins
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(Logins).FullName);
		internal static readonly Base64.Decoder decoder = Base64.Decoder;
		internal static readonly Base64.Encoder encoder = Base64.Encoder;
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static boolean loginValid(String account, String password, String ip, String host) throws java.io.IOException
		public static bool loginValid(string account, string password, string ip, string host)
		{
			bool flag1 = false;
			_log.info("Connect from : " + account);

			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				sbyte[] abyte1;
				sbyte[] abyte2;
				MessageDigest messagedigest = MessageDigest.getInstance("SHA");
				sbyte[] abyte0 = password.GetBytes(Encoding.UTF8);
				abyte1 = messagedigest.digest(abyte0);
				abyte2 = null;

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT password FROM accounts WHERE login=? LIMIT 1");
				pstm.setString(1, account);
				rs = pstm.executeQuery();
				if (rs.next())
				{
					abyte2 = decoder.decode(rs.getString(1));
					_log.fine("account exists");
				}
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);

				if (abyte2 == null)
				{
					if (Config.AUTO_CREATE_ACCOUNTS)
					{
						con = L1DatabaseFactory.Instance.Connection;
						pstm = con.prepareStatement("INSERT INTO accounts SET login=?,password=?,lastactive=?,access_level=?,ip=?,host=?");
						pstm.setString(1, account);
						pstm.setString(2, encoder.encodeToString(abyte1));
						pstm.setLong(3, 0L);
						pstm.setInt(4, 0);
						pstm.setString(5, ip);
						pstm.setString(6, host);
						pstm.execute();
						_log.info("created new account for " + account);
						return true;
					}
					else
					{
						_log.warning("account missing for user " + account);
						return false;
					}
				}

				try
				{
					flag1 = true;
					int i = 0;
					do
					{
						if (i >= abyte2.Length)
						{
							break;
						}
						if (abyte1[i] != abyte2[i])
						{
							flag1 = false;
							break;
						}
						i++;
					} while (true);
				}
				catch (Exception e)
				{
					_log.warning("could not check password:" + e);
					flag1 = false;
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (NoSuchAlgorithmException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (UnsupportedEncodingException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return flag1;
		}
	}
}