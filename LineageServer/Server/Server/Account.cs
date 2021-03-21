using LineageServer.Interfaces;
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

    using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
    using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

    /// <summary>
    /// 帳號相關資訊
    /// </summary>
    public class Account
    {
        /// <summary>
        /// 使用者帳號名稱 </summary>
        private string _name;

        /// <summary>
        /// 來源IP位址 </summary>
        private string _ip;

        /// <summary>
        /// 加密過後的密碼 </summary>
        private string _password;

        /// <summary>
        /// 上一次登入的日期 </summary>
        private DateTime _lastActive;

        /// <summary>
        /// 權限(是否為GM) </summary>
        private int _accessLevel;

        /// <summary>
        /// 來源 DNS 反解 </summary>
        private string _host;

        /// <summary>
        /// 是否被禁止登入 (True 代表禁止). </summary>
        private bool _banned;

        /// <summary>
        /// 可使用的角色數目 </summary>
        private int _characterSlot;

        /// <summary>
        /// 帳戶是否有效 (True 代表有效). </summary>
        private bool _isValid = false;

        /// <summary>
        /// 倉庫密碼 </summary>
        private int _WarePassword = 0;

        /// <summary>
        /// 帳號是否在線上 </summary>
        private bool _online = false;

        /// <summary>
        /// 是否有角色在線上 </summary>
        private bool _onlineStatus = false;

        /// <summary>
        /// 紀錄用 </summary>
        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static Logger _log = Logger.getLogger(typeof(Account).FullName);
        internal static readonly Base64.Encoder encoder = Base64.Encoder;
        /// <summary>
        /// 建構式
        /// </summary>
        private Account()
        {
        }

        /// <summary>
        /// 將明文密碼加密
        /// </summary>
        /// <param name="rawPassword">
        ///            明文密碼 </param>
        /// <returns> String </returns>
        /// <exception cref="NoSuchAlgorithmException">
        ///             密碼使用不存在的演算法加密 </exception>
        /// <exception cref="UnsupportedEncodingException">
        ///             文字編碼不支援 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: private static String encodePassword(final String rawPassword) throws java.security.NoSuchAlgorithmException, java.io.UnsupportedEncodingException
        private static string encodePassword(in string rawPassword)
        {
            sbyte[] buf = rawPassword.GetBytes(Encoding.UTF8);
            buf = MessageDigest.getInstance("SHA").digest(buf);

            return encoder.encodeToString(buf);
        }

        /// <summary>
        /// 建立新的帳號
        /// </summary>
        /// <param name="name">
        ///            帳號名稱 </param>
        /// <param name="rawPassword">
        ///            明文密碼 </param>
        /// <param name="ip">
        ///            連結時的 IP </param>
        /// <param name="host">
        ///            連結時的 dns 反查 </param>
        /// <returns> Account </returns>
        public static Account create(in string name, in string rawPassword, in string ip, in string host)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                Account account = new Account();
                account._name = name;
                account._password = encodePassword(rawPassword);
                account._ip = ip;
                account._host = host;
                account._banned = false;
                account._lastActive = DateTime.Now;

                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "INSERT INTO accounts SET login=?,password=?,lastactive=?,access_level=?,ip=?,host=?,online=?,banned=?,character_slot=?,OnlineStatus=?";
                pstm = con.prepareStatement(sqlstr);
                pstm.setString(1, account._name);
                pstm.setString(2, account._password);
                pstm.setTimestamp(3, account._lastActive);
                pstm.setInt(4, 0);
                pstm.setString(5, account._ip);
                pstm.setString(6, account._host);
                pstm.setInt(7, 0);
                pstm.setInt(8, account._banned ? 1 : 0);
                pstm.setInt(9, account._online ? 1 : 0);
                pstm.setInt(10, account._onlineStatus ? 1 : 0);
                pstm.execute();
                _log.info("created new account for " + name);

                return account;
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
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
            return null;
        }

        /// <summary>
        /// 從資料庫中取得指定帳號的資料
        /// </summary>
        /// <param name="name">
        ///            帳號名稱 </param>
        /// <returns> Account </returns>
        public static Account load(in string name)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;

            Account account = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "SELECT * FROM accounts WHERE login=? LIMIT 1";
                pstm = con.prepareStatement(sqlstr);
                pstm.setString(1, name);
                rs = pstm.executeQuery();
                if (!rs.next())
                {
                    return null;
                }
                account = new Account();
                account._name = rs.getString("login");
                account._password = rs.getString("password");
                account._lastActive = rs.getTimestamp("lastactive");
                account._accessLevel = rs.getInt("access_level");
                account._ip = rs.getString("ip");
                account._host = rs.getString("host");
                account._banned = rs.getInt("banned") == 0 ? false : true;
                account._online = rs.getInt("online") == 0 ? false : true;
                account._characterSlot = rs.getInt("character_slot");
                account._WarePassword = rs.getInt("warepassword");
                account._onlineStatus = rs.getInt("OnlineStatus") == 0 ? false : true;

                _log.fine("account exists");
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

            return account;
        }

        /// <summary>
        /// 更新最後一次登入時的日期與時間
        /// </summary>
        /// <param name="account">
        ///            帳號 </param>
        public static void updateLastActive(in Account account, in string ip)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            Timestamp ts = new Timestamp(DateTimeHelper.CurrentUnixTimeMillis());

            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "UPDATE accounts SET lastactive=?, ip=? ,online=1 WHERE login = ?";
                pstm = con.prepareStatement(sqlstr);
                pstm.setTimestamp(1, ts);
                pstm.setString(2, ip);
                pstm.setString(3, account.Name);
                pstm.execute();
                account._lastActive = ts;
                _log.fine("update lastactive for " + account.Name);
            }
            catch (Exception e)
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
        /// 更新資料庫中角色數目
        /// </summary>
        /// <param name="account">
        ///            アカウント </param>
        public static void updateCharacterSlot(in Account account)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;

            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "UPDATE accounts SET character_slot=? WHERE login=?";
                pstm = con.prepareStatement(sqlstr);
                pstm.setInt(1, account.CharacterSlot);
                pstm.setString(2, account.Name);
                pstm.execute();
                account._characterSlot = account.CharacterSlot;
                _log.fine("update characterslot for " + account.Name);
            }
            catch (Exception e)
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
        /// 取得帳號下的角色數目
        /// </summary>
        /// <returns> int </returns>
        public virtual int countCharacters()
        {
            int result = 0;
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "SELECT count(*) as cnt FROM characters WHERE account_name=?";
                pstm = con.prepareStatement(sqlstr);
                pstm.setString(1, _name);
                rs = pstm.executeQuery();
                if (rs.next())
                {
                    result = rs.getInt("cnt");
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
            return result;
        }

        /// <summary>
        /// @category 寫入帳號是否在線上 </summary>
        /// <param name="account"> 玩家帳號 </param>
        /// <param name="i"> isOnline? </param>
        public static void online(Account account, bool i)
        {
            lock (typeof(Account))
            {
                IDataBaseConnection con = null;
                PreparedStatement pstm = null;
                try
                {
                    con = L1DatabaseFactory.Instance.Connection;
                    string sqlstr = "UPDATE accounts SET online=? WHERE login=?";
                    pstm = con.prepareStatement(sqlstr);
                    pstm.setInt(1, i ? 1 : 0);
                    pstm.setString(2, account.Name);
                    pstm.execute();
                    account.Online = i;
                }
                catch (SQLException)
                {
                }
                finally
                {
                    SQLUtil.close(pstm);
                    SQLUtil.close(con);
                }
            }
        }

        /// <summary>
        /// @category 寫入是否有角色在線上 </summary>
        /// <param name="account"> 玩家帳號 </param>
        /// <param name="i"> isOnline? </param>
        public static void OnlineStatus(Account account, bool i)
        {
            lock (typeof(Account))
            {
                IDataBaseConnection con = null;
                PreparedStatement pstm = null;
                try
                {
                    con = L1DatabaseFactory.Instance.Connection;
                    string sqlstr = "UPDATE accounts SET OnlineStatus=? WHERE login=?";
                    pstm = con.prepareStatement(sqlstr);
                    pstm.setInt(1, i ? 1 : 0);
                    pstm.setString(2, account.Name);
                    pstm.execute();
                    account.OnlineStatus = i;
                }
                catch (SQLException)
                {
                }
                finally
                {
                    SQLUtil.close(pstm);
                    SQLUtil.close(con);
                }
            }
        }

        /// <summary>
        /// 歸零所有帳號online=0, OnlineStatus=0
        /// </summary>
        public static void InitialOnlineStatus()
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "UPDATE accounts SET online=0, OnlineStatus=0 WHERE online=1 OR OnlineStatus=1";
                pstm = con.prepareStatement(sqlstr);
                pstm.execute();
            }
            catch (Exception e)
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
        /// 設定該帳號為禁止登入
        /// </summary>
        /// <param name="login">
        ///            アカウント名 </param>
        public static void ban(in string login)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                string sqlstr = "UPDATE accounts SET banned=1, online=0, OnlineStatus=0 WHERE login=?";
                pstm = con.prepareStatement(sqlstr);
                pstm.setString(1, login);
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
        /// 確認輸入的密碼與資料庫中的密碼是否相同
        /// </summary>
        /// <param name="rawPassword">
        ///            明文密碼 </param>
        /// <returns> boolean </returns>
        public virtual bool validatePassword(in string rawPassword)
        {
            // 認證成功後如果再度認證就判斷為失敗
            if (_isValid)
            {
                return false;
            }
            try
            {
                _isValid = _password.Equals(encodePassword(rawPassword));
                if (_isValid)
                {
                    _password = null; // 認證成功後就將記憶體中的密碼清除
                }
                return _isValid;
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            return false;
        }

        /// <summary>
        /// 變更倉庫密碼
        /// </summary>
        /// <param name="newPassword">
        ///            新的密碼 </param>
        public virtual void changeWarePassword(int newPassword)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;

                pstm = con.prepareStatement("UPDATE `accounts` SET `warepassword` = ? WHERE `login` = ?");
                pstm.setInt(1, newPassword);
                pstm.setString(2, Name);
                pstm.execute();

                _WarePassword = newPassword;
            }
            catch (SQLException)
            {
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

        /// <summary>
        /// 取得帳號使否有效 (True 為有效).
        /// </summary>
        /// <returns> boolean </returns>
        public virtual bool Valid
        {
            get
            {
                return _isValid;
            }
        }

        /// <summary>
        /// 取得是否為GM (True 代表為GM).
        /// </summary>
        /// <returns> boolean </returns>
        public virtual bool GameMaster
        {
            get
            {
                return 0 < _accessLevel;
            }
        }

        /// <summary>
        /// 取得帳號名稱
        /// </summary>
        /// <returns> String </returns>
        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// 取得連線時的 IP
        /// </summary>
        /// <returns> String </returns>
        public virtual string Ip
        {
            get
            {
                return _ip;
            }
        }

        /// <summary>
        /// 取得上次登入的時間
        /// </summary>
        public virtual DateTime LastActive
        {
            get
            {
                return _lastActive;
            }
        }

        /// <summary>
        /// 取得權限
        /// </summary>
        /// <returns> int </returns>
        public virtual int AccessLevel
        {
            get
            {
                return _accessLevel;
            }
        }

        /// <summary>
        /// 取得 DNS 反解的域名
        /// </summary>
        /// <returns> String </returns>
        public virtual string Host
        {
            get
            {
                return _host;
            }
        }

        /// <summary>
        /// 設定帳號是否在線上 </summary>
        /// <param name="i"> </param>
        public virtual bool Online
        {
            set
            {
                lock (this)
                {
                    _online = value;
                }
            }
        }

        /// <summary>
        /// 取得帳號是否在線上
        /// @return
        /// </summary>
        public virtual bool Onlined
        {
            get
            {
                lock (this)
                {
                    return _online;
                }
            }
        }

        /// <summary>
        /// 設定是否有角色在線上 </summary>
        /// <param name="i"> </param>
        public virtual bool OnlineStatus
        {
            set
            {
                lock (this)
                {
                    _onlineStatus = value;
                }
            }
            get
            {
                lock (this)
                {
                    return _onlineStatus;
                }
            }
        }


        /// <summary>
        /// 取得是否被禁止登入
        /// </summary>
        /// <returns> boolean </returns>
        public virtual bool Banned
        {
            get
            {
                return _banned;
            }
        }

        /// <summary>
        /// 取得角色數目
        /// </summary>
        /// <returns> int </returns>
        public virtual int CharacterSlot
        {
            get
            {
                return _characterSlot;
            }
            set
            {
                _characterSlot = value;
            }
        }


        /// <summary>
        /// 取得倉庫密碼
        /// </summary>
        /// <returns> 倉庫密碼 </returns>
        public virtual int WarePassword
        {
            get
            {
                return _WarePassword;
            }
        }
    }

}