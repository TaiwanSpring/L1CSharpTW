using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class SpawnlistDoor : DataSource
    {
        public const string TableName = "spawnlist_door";
        public const string Column_location = "location";
        public const string Column_id = "id";
        public const string Column_gfxid = "gfxid";
        public const string Column_locx = "locx";
        public const string Column_locy = "locy";
        public const string Column_mapid = "mapid";
        public const string Column_hp = "hp";
        public const string Column_keeper = "keeper";
        public const string Column_isOpening = "isOpening";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistDoor; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_location, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_gfxid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locx, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_locy, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_keeper, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_isOpening, DbType = DbType.Boolean, IsPKey = false},
        };
        public SpawnlistDoor() : base(TableName)
        {
            
        }
    }
}
