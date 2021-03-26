using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class LogChat : DataSource
    {
        public const string TableName = "log_chat";
        public const string Column_account_name = "account_name";
        public const string Column_name = "name";
        public const string Column_clan_name = "clan_name";
        public const string Column_target_account_name = "target_account_name";
        public const string Column_target_name = "target_name";
        public const string Column_target_clan_name = "target_clan_name";
        public const string Column_content = "content";
        public const string Column_datetime = "datetime";
        public const string Column_id = "id";
        public const string Column_char_id = "char_id";
        public const string Column_clan_id = "clan_id";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public const string Column_type = "type";
        public const string Column_target_id = "target_id";
        public const string Column_target_clan_id = "target_clan_id";
        public const string Column_target_locx = "target_locx";
        public const string Column_target_locy = "target_locy";
        public const string Column_target_mapid = "target_mapid";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.LogChat; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_account_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_target_account_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_target_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_target_clan_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_content, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_datetime, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_char_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_type, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_target_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_target_clan_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_target_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_target_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_target_mapid, DbType = DbType.Int32, IsPKey = false},
        };
        public LogChat() : base(TableName)
        {
            
        }
    }
}
