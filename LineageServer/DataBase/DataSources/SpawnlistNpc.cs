using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class SpawnlistNpc : DataSource
    {
        public const string TableName = "spawnlist_npc";
        public const string Column_location = "location";
        public const string Column_id = "id";
        public const string Column_count = "count";
        public const string Column_npc_templateid = "npc_templateid";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_randomx = "randomx";
        public const string Column_randomy = "randomy";
        public const string Column_heading = "heading";
        public const string Column_respawn_delay = "respawn_delay";
        public const string Column_mapid = "mapid";
        public const string Column_movement_distance = "movement_distance";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistNpc; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_templateid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomy, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_heading, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_respawn_delay, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_movement_distance, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public SpawnlistNpc() : base(TableName)
        {
            
        }
    }
}
