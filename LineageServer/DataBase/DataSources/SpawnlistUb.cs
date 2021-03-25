using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistUb : DataSourceTable
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
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_ub_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_pattern, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_group_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npc_templateid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_spawn_delay, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_seal_count, DbType = DbType.Int32, IsPKey = false},
        };
        public SpawnlistUb() : base(TableName)
        {
            
        }
    }
}
