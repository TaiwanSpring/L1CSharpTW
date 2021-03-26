using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using System;
using System.Text;
namespace LineageServer.Server.Server
{
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
        private static ILogger _log = Logger.getLogger(nameof(Account));
        private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Accounts);
        /// <summary>
        /// 建構式
        /// </summary>
        private Account()
        {
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">明文</param>
        /// <returns> String </returns>
        private static string Encode(string str)
        {
            byte[] buffer = GobalParameters.Encoding.GetBytes(str);
            var sha = System.Security.Cryptography.SHA256.Create();
            buffer = sha.ComputeHash(buffer);
            sha.Dispose();
            return GobalParameters.Encoding.GetString(buffer);
        }

        /// <summary>
        /// 建立新的帳號
        /// </summary>
        /// <param name="name">帳號名稱 </param>
        /// <param name="password">明文密碼 </param>
        /// <param name="ip">連結時的 IP </param>
        /// <param name="host">連結時的 dns 反查 </param>
        /// <returns> Account </returns>
        public static Account Create(string name, string password, string ip, string host)
        {
            try
            {
                Account account = new Account();
                account._name = name;
                account._password = Encode(password);
                account._ip = ip;
                account._host = host;
                account._lastActive = DateTime.Now;
                dataSource.NewRow().Insert()
                            .Set(Accounts.Column_login, account._name)
                            .Set(Accounts.Column_password, account._password)
                            .Set(Accounts.Column_lastactive, account._lastActive)
                            .Set(Accounts.Column_ip, account._ip)
                            .Set(Accounts.Column_host, account._host).Execute();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }

            return null;
        }

        /// <summary>
        /// 從資料庫中取得指定帳號的資料
        /// </summary>
        /// <param name="name">
        ///            帳號名稱 </param>
        /// <returns> Account </returns>
        public static Account Load(string name)
        {
            try
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Select().Where(Accounts.Column_login, name).Execute();
                Account account = new Account();
                account._name = dataSourceRow.getString(Accounts.Column_login);
                if (string.IsNullOrEmpty(account._name))
                {
                    return null;
                }
                account._password = dataSourceRow.getString(Accounts.Column_password);
                account._lastActive = dataSourceRow.getTimestamp(Accounts.Column_lastactive);
                account._accessLevel = dataSourceRow.getInt(Accounts.Column_access_level);
                account._ip = dataSourceRow.getString(Accounts.Column_ip);
                account._host = dataSourceRow.getString(Accounts.Column_host);
                account._banned = dataSourceRow.getInt(Accounts.Column_banned) == 0 ? false : true;
                account._online = dataSourceRow.getInt(Accounts.Column_online) == 0 ? false : true;
                account._characterSlot = dataSourceRow.getInt(Accounts.Column_character_slot);
                account._WarePassword = dataSourceRow.getInt(Accounts.Column_warepassword);
                account._onlineStatus = dataSourceRow.getInt(Accounts.Column_OnlineStatus) == 0 ? false : true;
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }

            return null;
        }

        /// <summary>
        /// 更新最後一次登入時的日期與時間
        /// </summary>
        /// <param name="account">
        ///            帳號 </param>
        public void UpdateLastActive(string ip)
        {
            try
            {
                dataSource.NewRow().Update()
                     .Set(Accounts.Column_lastactive, DateTime.Now)
                     .Set(Accounts.Column_ip, ip)
                     .Where(Accounts.Column_login, Name).Execute();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }

        /// <summary>
        /// 更新資料庫中角色數目
        /// </summary>
        /// <param name="account">
        ///            アカウント </param>
        public static void UpdateCharacterSlot(Account account)
        {
            try
            {
                dataSource.NewRow().Update()
                          .Set(Accounts.Column_character_slot, account.CharacterSlot)
                          .Where(Accounts.Column_login, account.Name).Execute();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }

        /// <summary>
        /// 取得帳號下的角色數目
        /// </summary>
        /// <returns> int </returns>
        public virtual int CountCharacters()
        {
            IDataSource charactersDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Characters);
            try
            {
                return charactersDataSource.Select()
                    .Where(Characters.Column_account_name, _name).Query().Count;
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            return 0;
        }

        /// <summary>
        /// @category 寫入帳號是否在線上 </summary>
        /// <param name="account"> 玩家帳號 </param>
        /// <param name="isOnline"> isOnline? </param>
        public static void SetOnline(Account account, bool isOnline)
        {
            lock (dataSource)
            {
                try
                {
                    dataSource.NewRow().Update()
                        .Where(Accounts.Column_login, account.Name)
                        .Set(Accounts.Column_online, isOnline ? 1 : 0).Execute();
                }
                catch (Exception e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
            }
        }

        /// <summary>
        /// @category 寫入是否有角色在線上 </summary>
        /// <param name="account"> 玩家帳號 </param>
        /// <param name="isOnline"> isOnline? </param>
        public static void SetOnlineStatus(Account account, bool isOnline)
        {
            lock (dataSource)
            {
                try
                {
                    dataSource.NewRow().Update()
                        .Where(Accounts.Column_login, account.Name)
                        .Set(Accounts.Column_OnlineStatus, isOnline ? 1 : 0).Execute();
                }
                catch (Exception e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
            }
        }

        /// <summary>
        /// 歸零所有帳號online=0, OnlineStatus=0
        /// </summary>
        public static void InitialOnlineStatus()
        {
            try
            {
                dataSource.NewRow().Update()
                       .Set(Accounts.Column_online, 0)
                       .Set(Accounts.Column_OnlineStatus, 0).Execute();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }

        /// <summary>
        /// 設定該帳號為禁止登入
        /// </summary>
        /// <param name="login">
        ///            アカウント名 </param>
        public static void Ban(string login)
        {
            try
            {
                dataSource.NewRow().Update()
                          .Set(Accounts.Column_login, login)
                          .Set(Accounts.Column_banned, 1)
                          .Set(Accounts.Column_online, 0)
                          .Set(Accounts.Column_OnlineStatus, 0).Execute();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }

        /// <summary>
        /// 確認輸入的密碼與資料庫中的密碼是否相同
        /// </summary>
        /// <param name="rawPassword">
        ///            明文密碼 </param>
        /// <returns> boolean </returns>
        public virtual bool ValidatePassword(in string rawPassword)
        {
            // 認證成功後如果再度認證就判斷為失敗
            if (_isValid)
            {
                return false;
            }
            try
            {
                _isValid = _password.Equals(Encode(rawPassword));
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
        public virtual void ChangeWarePassword(int newPassword)
        {
            try
            {
                dataSource.NewRow().Update()
                          .Set(Accounts.Column_login, Name)
                          .Set(Accounts.Column_warepassword, newPassword).Execute();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
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
            get
            {
                lock (dataSource)
                {
                    return _online;
                }
            }
            set
            {
                lock (dataSource)
                {
                    _online = value;
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
                lock (dataSource)
                {
                    _onlineStatus = value;
                }
            }
            get
            {
                lock (dataSource)
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