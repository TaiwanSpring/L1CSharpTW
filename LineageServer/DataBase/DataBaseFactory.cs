using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.DataBase
{
    class DataBaseFactory : IDatabaseFactory
    {
        static DataBaseFactory()
        {
            Container.Instance.RegisterInstance<IDatabaseFactory>(new DataBaseFactory());
        }
        /// <summary>
        /// 取得資料庫連結時的連線
        /// </summary>
        public IDataBaseConnection Connection { get; private set; }

        private DataBaseFactory() { }

        public bool Initialize(string hostName, string user, string password, string dbName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName">資料庫位址</param>
        /// <param name="user">資料庫使用者名稱</param>
        /// <param name="password">資料庫使用者密碼</param>
        /// <param name="dbName">資料庫名稱</param>
        /// <returns></returns>
        public bool Initialize(string hostName, string user, string password, string dbName)
        {
            this.connectString = $"server={hostName};uid={user};pwd={password};database={dbName}";
            try
            {
                MySqlConnection connection = new MySqlConnection(this.connectString);
                connection.Open();

                Connection = new DataBaseConnectionImp(connection);
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        System.Console.WriteLine("無法連線到資料庫.");
                        break;
                    case 1045:
                        System.Console.WriteLine("使用者帳號或密碼錯誤,請再試一次.");
                        break;
                }
                return false;
            }
            catch (Exception e)
            {
                //_log.fine("Database Connection FAILED");
                System.Console.WriteLine("could not init DB connection:" + e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 伺服器關閉的時候要關閉與資料庫的連結
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }
    }
}
