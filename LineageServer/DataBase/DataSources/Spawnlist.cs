using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Spawnlist : DataSource
    {
        public const string TableName = "spawnlist";
        public const string Column_location = "location";
        public const string Column_id = "id";
        public const string Column_count = "count";
        public const string Column_npc_templateid = "npc_templateid";
        public const string Column_group_id = "group_id";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_randomx = "randomx";
        public const string Column_randomy = "randomy";
        public const string Column_locx1 = "locx1";
        public const string Column_locy1 = "locy1";
        public const string Column_locx2 = "locx2";
        public const string Column_locy2 = "locy2";
        public const string Column_heading = "heading";
        public const string Column_min_respawn_delay = "min_respawn_delay";
        public const string Column_max_respawn_delay = "max_respawn_delay";
        public const string Column_mapid = "mapid";
        public const string Column_movement_distance = "movement_distance";
        public const string Column_respawn_screen = "respawn_screen";
        public const string Column_rest = "rest";
        public const string Column_near_spawn = "near_spawn";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Spawnlist; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_templateid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_group_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomx, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomy, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_heading, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_min_respawn_delay, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_respawn_delay, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_movement_distance, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_respawn_screen, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_rest, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_near_spawn, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Spawnlist() : base(TableName)
        {
            
        }
    }
}
