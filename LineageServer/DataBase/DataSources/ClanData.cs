using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class ClanData : DataSourceTable
    {
        public const string TableName = "clan_data";
        public const string Column_clan_name = "clan_name";
        public const string Column_leader_name = "leader_name";
        public const string Column_announcement = "announcement";
        public const string Column_found_date = "found_date";
        public const string Column_clan_id = "clan_id";
        public const string Column_leader_id = "leader_id";
        public const string Column_hascastle = "hascastle";
        public const string Column_hashouse = "hashouse";
        public const string Column_emblem_id = "emblem_id";
        public const string Column_emblem_status = "emblem_status";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_clan_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_leader_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_announcement, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_found_date, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_leader_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hascastle, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hashouse, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_emblem_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_emblem_status, DbType = DbType.Boolean, IsPKey = false},
        };
        public ClanData() : base(TableName)
        {
            
        }
    }
}
