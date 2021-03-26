using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistBoss : DataSource
    {
        public const string TableName = "spawnlist_boss";
        public const string Column_location = "location";
        public const string Column_cycle_type = "cycle_type";
        public const string Column_id = "id";
        public const string Column_count = "count";
        public const string Column_npc_id = "npc_id";
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
        public const string Column_mapid = "mapid";
        public const string Column_movement_distance = "movement_distance";
        public const string Column_respawn_screen = "respawn_screen";
        public const string Column_rest = "rest";
        public const string Column_spawn_type = "spawn_type";
        public const string Column_percentage = "percentage";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistBoss; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_cycle_type, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_group_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_heading, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_movement_distance, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_respawn_screen, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_rest, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_spawn_type, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_percentage, DbType = DbType.Boolean, IsPKey = false},
        };
        public SpawnlistBoss() : base(TableName)
        {
            
        }
    }
}
