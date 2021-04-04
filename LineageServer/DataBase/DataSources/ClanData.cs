using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class ClanData : DataSource
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
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.ClanData; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_clan_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_leader_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_announcement, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_found_date, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_clan_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_leader_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hascastle, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hashouse, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_emblem_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_emblem_status, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public ClanData() : base(TableName)
        {
            
        }
    }
}
