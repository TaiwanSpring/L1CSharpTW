using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.DataBase
{
    class DataBaseFactory : IDatabaseFactory
    {
        [Obsolete]
        /// <summary>
        /// 取得資料庫連結時的連線
        /// </summary>
        public IDataBaseConnection Connection { get; private set; }
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
