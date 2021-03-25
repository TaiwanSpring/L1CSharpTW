using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class Accounts : DataSourceTable
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
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_login, DbType = DbType.String, IsPKey = true},
            new ColumnInfo() { Column = Column_password, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_ip, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_host, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_lastactive, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_access_level, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_online, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_banned, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_character_slot, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_warepassword, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OnlineStatus, DbType = DbType.Int32, IsPKey = false},
        };
        public Accounts() : base(TableName)
        {
            
        }
    }
}
