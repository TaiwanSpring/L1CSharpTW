using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class SpawnlistUb : DataSource
    {
        public const string TableName = "spawnlist_ub";
        public const string Column_location = "location";
        public const string Column_id = "id";
        public const string Column_ub_id = "ub_id";
        public const string Column_pattern = "pattern";
        public const string Column_group_id = "group_id";
        public const string Column_npc_templateid = "npc_templateid";
        public const string Column_count = "count";
        public const string Column_spawn_delay = "spawn_delay";
        public const string Column_seal_count = "seal_count";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistUb; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_ub_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_pattern, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_group_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_templateid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_spawn_delay, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_seal_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public SpawnlistUb() : base(TableName)
        {
            
        }
    }
}
