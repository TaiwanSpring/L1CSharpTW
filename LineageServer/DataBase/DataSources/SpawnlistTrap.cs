using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistTrap : DataSource
    {
        public const string TableName = "spawnlist_trap";
        public const string Column_note = "note";
        public const string Column_id = "id";
        public const string Column_trapId = "trapId";
        public const string Column_mapId = "mapId";
        public const string Column_locX = "locX";
        public const string Column_locY = "locY";
        public const string Column_locRndX = "locRndX";
        public const string Column_locRndY = "locRndY";
        public const string Column_count = "count";
        public const string Column_span = "span";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistTrap; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_trapId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locX, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locY, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locRndX, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locRndY, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_span, DbType = DbType.Int32, IsPKey = false},
        };
        public SpawnlistTrap() : base(TableName)
        {
            
        }
    }
}
