using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Accounts : DataSource
    {
        public const string TableName = "accounts";
        public const string Column_login = "login";
        public const string Column_password = "password";
        public const string Column_ip = "ip";
        public const string Column_host = "host";
        public const string Column_lastactive = "lastactive";
        public const string Column_access_level = "access_level";
        public const string Column_online = "online";
        public const string Column_banned = "banned";
        public const string Column_character_slot = "character_slot";
        public const string Column_warepassword = "warepassword";
        public const string Column_OnlineStatus = "OnlineStatus";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Accounts; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_login, MySqlDbType = MySqlDbType.Text, IsPKey = true},
            new ColumnInfo() { Column = Column_password, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_ip, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_host, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_lastactive, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_access_level, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_online, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_banned, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_character_slot, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_warepassword, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OnlineStatus, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Accounts() : base(TableName)
        {
            
        }
    }
}
