using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistNpc : DataSourceTable
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
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_templateid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_heading, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_respawn_delay, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_movement_distance, DbType = DbType.Int32, IsPKey = false},
        };
        public SpawnlistNpc() : base(TableName)
        {
            
        }
    }
}
