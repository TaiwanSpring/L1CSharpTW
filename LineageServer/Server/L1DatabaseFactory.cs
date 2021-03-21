using System;

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
namespace LineageServer.Server
{

	using LeakCheckedConnection = LineageServer.Server.Server.utils.LeakCheckedConnection;

	using ComboPooledDataSource = com.mchange.v2.c3p0.ComboPooledDataSource;

	/// <summary>
	/// DBへのアクセスするための各種インターフェースを提供する.
	/// </summary>
	public class L1DatabaseFactory
	{
		/// <summary>
		/// 資料庫的實例 </summary>
		private static L1DatabaseFactory _instance;

		/// <summary>
		/// 資料庫連結的來源 </summary>
		private ComboPooledDataSource _source;

		/// <summary>
		/// 紀錄用 </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1DatabaseFactory).FullName);

		/* 連結資料庫相關的資訊 */
		/// <summary>
		/// 資料庫連結的驅動程式 </summary>
		private static string _driver;

		/// <summary>
		/// 資料庫連結的位址 </summary>
		private static string _url;

		/// <summary>
		/// 登入資料庫的使用者名稱 </summary>
		private static string _user;

		/// <summary>
		/// 登入資料庫的密碼 </summary>
		private static string _password;

		/// <summary>
		/// 設定資料庫登入的相關資訊
		/// </summary>
		/// <param name="driver"> 
		///            資料庫連結的驅動程式 </param>
		/// <param name="url">
		///            資料庫連結的位址 </param>
		/// <param name="user">
		///            登入資料庫的使用者名稱 </param>
		/// <param name="password">
		///            登入資料庫的密碼 </param>
		public static void setDatabaseSettings(in string driver, in string url, in string user, in string password)
		{
			_driver = driver;
			_url = url;
			_user = user;
			_password = password;
		}

		/// <summary>
		/// 資料庫連結的設定與配置
		/// </summary>
		/// <exception cref="SQLException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public L1DatabaseFactory() throws java.sql.SQLException
		public L1DatabaseFactory()
		{
			try
			{
				// DatabaseFactoryをL2Jから一部を除いて拝借
				_source = new ComboPooledDataSource();
				_source.DriverClass = _driver;
				_source.JdbcUrl = _url;
				_source.User = _user;
				_source.Password = _password;

				/* Test the connection */
				_source.Connection.close();
			}
			catch (SQLException x)
			{
				_log.fine("Database Connection FAILED");
				// rethrow the exception
				throw x;
			}
			catch (Exception e)
			{
				_log.fine("Database Connection FAILED");
				throw new SQLException("could not init DB connection:" + e);
			}
		}

		/// <summary>
		/// 伺服器關閉的時候要關閉與資料庫的連結
		/// </summary>
		public virtual void shutdown()
		{
			try
			{
				_source.close();
			}
			catch (Exception e)
			{
				_log.log(Level.INFO, "", e);
			}
			try
			{
				_source = null;
			}
			catch (Exception e)
			{
				_log.log(Level.INFO, "", e);
			}
		}

		/// <summary>
		/// 取得資料庫的實例（第一次實例為 null 的時候才新建立一個).
		/// </summary>
		/// <returns> L1DatabaseFactory </returns>
		/// <exception cref="SQLException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static L1DatabaseFactory getInstance() throws java.sql.SQLException
		public static L1DatabaseFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1DatabaseFactory();
				}
				return _instance;
			}
		}

		/// <summary>
		/// 取得資料庫連結時的連線
		/// </summary>
		/// <returns> Connection 連結對象 </returns>
		/// <exception cref="SQLException"> </exception>
		public virtual Connection Connection
		{
			get
			{
				Connection con = null;
    
				while (con == null)
				{
					try
					{
						con = _source.Connection;
					}
					catch (SQLException e)
					{
						_log.warning("L1DatabaseFactory: getConnection() failed, trying again " + e);
					}
				}
				return Config.DETECT_DB_RESOURCE_LEAKS ? LeakCheckedConnection.create(con) : con;
			}
		}
	}

}